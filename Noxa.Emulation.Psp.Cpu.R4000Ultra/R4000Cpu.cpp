// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "TraceOptions.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "Tracer.h"

#include "R4000AdvancedBlockBuilder.h"
#include "R4000Generator.h"
#include "R4000Ctx.h"
#include "R4000BiosStubs.h"
#include "R4000VideoInterface.h"

using namespace System::Diagnostics;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Debugging::DebugData;

extern uint _instructionsExecuted;

#ifdef SYSCALLSTATS
uint _syscallCounts[ 1024 ];
#endif

R4000Cpu::R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalCpu = this;

	// Ugly: has to be above the block builder constructor!
	_ctx = ( R4000Ctx* )_aligned_malloc( sizeof( R4000Ctx ), 16 );
	memset( _ctx, 0, sizeof( R4000Ctx ) );

	_emu = emulator;
	_params = parameters;
	_caps = gcnew R4000Capabilities();
	_memory = gcnew R4000Memory();
	_core0 = gcnew R4000Core( this, ( R4000Ctx* )_ctx );
	_codeCache = new R4000Cache();

	_stats = gcnew R4000Statistics();
#ifdef STATISTICS
	_timer = gcnew PerformanceTimer();
	_timeSinceLastIpsPrint = 0.0;
#endif

	_lastSyscall = -1;
	_syscalls = gcnew array<BiosFunction^>( 1024 );
	_syscallShims = gcnew array<BiosShim^>( 1024 );
	_syscallShimsN = gcnew array<IntPtr>( 1024 );
	_moduleInstances = gcnew array<IModule^>( 64 );
	_userExports = gcnew Dictionary<uint, uint>( 1024 );

	_hasExecuted = false;

	R4000Generator* gen = new R4000Generator();
	_context = gcnew R4000GenContext( gen, _memory->MainMemory, _memory->VideoMemory );
	_builder = gcnew R4000AdvancedBlockBuilder( this, _core0 );
	_biosStubs = gcnew R4000BiosStubs();
	_videoInterface = gcnew R4000VideoInterface( this );

	_bounce = ( bouncefn )_builder->BuildBounce();

	_privateMemoryFieldInfo = ( R4000Cpu::typeid )->GetField( "_memory", BindingFlags::Instance | BindingFlags::NonPublic );
	_privateModuleInstancesFieldInfo = ( R4000Cpu::typeid )->GetField( "_moduleInstances", BindingFlags::Instance | BindingFlags::NonPublic );

	_nativeInterface = ( CpuApi* )calloc( 1, sizeof( CpuApi ) );
	this->SetupNativeInterface();

	this->SetupThreading();
}

R4000Cpu::~R4000Cpu()
{
	this->Cleanup();
}

uint R4000Cpu::RegisterSyscall( unsigned int nid )
{
	BiosFunction^ function = _emu->Bios->FindFunction( nid );
	Debug::Assert( function != nullptr );
	if( function == nullptr )
		return -1;
	
	int sid = ++_lastSyscall;
	_syscalls[ sid ] = function;

	if( function->IsMissing == false )
	{
		void* registers = ( ( R4000Ctx* )_ctx )->Registers;
		_syscallShims[ sid ] = EmitShim( function, _memory->System, registers );
		if( function->NativeMethod != IntPtr::Zero )
			_syscallShimsN[ sid ] = IntPtr( EmitShimN( function, _memory->NativeSystem, registers ) );
		else
			_syscallShimsN[ sid ] = IntPtr::Zero;
	}
	else
	{
		_syscallShims[ sid ] = nullptr;
		_syscallShimsN[ sid ] = IntPtr::Zero;
	}

	return ( uint )sid;
}

void R4000Cpu::RegisterUserExports( BiosModule^ module )
{
	Debug::Assert( module != nullptr );

	for each( StubExport^ ex in module->Exports )
	{
		// Ignore system exports
		if( ex->IsSystem == true )
			continue;
		_userExports->Add( ex->NID, ex->Address );
	}
}

uint R4000Cpu::LookupUserExport( uint nid )
{
	uint address;
	if( _userExports->TryGetValue( nid, address ) == true )
		return address;
	else
		return 0;
}

void R4000Cpu::Cleanup()
{
	this->DestroyThreading();

	this->DestroyNativeInterface();
	SAFEFREE( _nativeInterface );

	_memory->Clear();

	if( _ctx != NULL )
		_aligned_free( _ctx );
	_ctx = NULL;
	//SAFEFREE( _bounce );
	SAFEDELETE( _codeCache );
}

void R4000Cpu::SetupGame( GameInformation^ game, Stream^ bootStream )
{
	Debug::Assert( _hasExecuted == false );
	if( _hasExecuted == false )
	{
		// Prepare tracer
#ifdef TRACE
		Tracer::OpenFile( TRACEFILE );
#endif

#ifdef TRACESYMBOLS
		Debug::Assert( bootStream != nullptr );
		_symbols = ProgramDebugData::Load( Debugging::DebugDataType::Symbols, bootStream );
		if( ( _symbols == nullptr ) ||
			( _symbols->MethodCount == 0 ) )
			Debug::WriteLine( "Cpu: no debug information found in program - method names will be unavailable" );
#endif

		// Has to happen late in the game because we need to
		// make sure the video subsystem is ready
		_videoInterface->Prepare();

		_hasExecuted = true;
	}
}

void R4000Cpu::PrintStatistics()
{
#ifdef TRACE
	Tracer::CloseFile();
#endif

#ifdef STATISTICS
	_stats->GatherStats();
	if( _stats->InstructionsExecuted == 0 )
		return;
	_stats->AverageCodeBlockLength /= _stats->CodeBlocksGenerated;
	_stats->AverageGenerationTime /= _stats->CodeBlocksGenerated;
	_stats->RunTime = _timer->Elapsed - _stats->RunTime;
	_stats->IPS = _stats->InstructionsExecuted / _stats->RunTime;

	Log::WriteLine( Verbosity::Normal, Feature::Statistics, "Ultra CPU Statistics: ---------------------------------------" );
	array<FieldInfo^>^ fields = ( R4000Statistics::typeid )->GetFields();
	for( int n = 0; n < fields->Length; n++ )
	{
		Object^ value = fields[ n ]->GetValue( _stats );
		Log::WriteLine( Verbosity::Normal, Feature::Statistics, "{0}: {1}", fields[ n ]->Name, value );
	}
	
#ifdef SYSCALLSTATS
	// Syscall stats
	int syscallCount = sizeof( _syscallCounts ) / sizeof( uint );
	int callCount = 0;
	for( int n = 0; n < syscallCount; n++ )
	{
		int value = _syscallCounts[ n ];
		callCount += value;
	}

	Log::WriteLine( Verbosity::Normal, Feature::Statistics, "Syscall statistics (in percent of all calls):" );
	for( int n = 0; n < syscallCount; n++ )
	{
		int value = _syscallCounts[ n ];
		if( value == 0 )
			continue;
		BiosFunction^ func = _syscalls[ n ];
		float p = value / ( float )callCount;
		p *= 100.0f;
		String^ type = String::Format( "{0}{1}{2}{3}{4}{5}",
			func->IsMissing == false ? "" : "(NOT FOUND)",
			func->IsImplemented ? "" : "(NI)",
			func->HasCpuImplementation ? "(CPU)" : "",
			func->NativeMethod == IntPtr::Zero ? "" : "(Native)",
			func->NativeImplementationSuggested ? "(NativeSugg)" : "",
			func->IsStateless ? "(Stateless)" : "" );
		Log::WriteLine( Verbosity::Normal, Feature::Statistics, "{0,-50} {1,10}x, {2,3}%\t{3}",
			String::Format( "{0}::{1}:", ( func->Module != nullptr ) ? func->Module->Name : "*unknown*", ( func->Name != nullptr ) ? func->Name : String::Format( "0x{0:X8}", func->NID ) ),
			value, p, type );
	}
#endif
#endif
}
