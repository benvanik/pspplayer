// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
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
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

// If defined, all syscalls that don't return anything will still set $v0
//#define SAFESYSCALLRETURN

extern uint _instructionsExecuted;

R4000Cpu::R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalCpu = this;

	// Ugly: has to be above the block builder constructor!
	_ctx = ( R4000Ctx* )_aligned_malloc( sizeof( R4000Ctx ), 16 );
	memset( _ctx, 0, sizeof( R4000Ctx ) );

	_emu = emulator;
	_params = parameters;
	_caps = gcnew R4000Capabilities();
	_clock = gcnew R4000Clock();
	_memory = gcnew R4000Memory();
	_core0 = gcnew R4000Core( this, ( R4000Ctx* )_ctx );
	_codeCache = gcnew R4000Cache();

	_stats = gcnew R4000Statistics();
#ifdef STATISTICS
	_timer = gcnew PerformanceTimer();
	_timeSinceLastIpsPrint = 0.0;
#endif

	_lastSyscall = -1;
	_syscalls = gcnew array<BiosFunction^>( 1024 );
	_syscallShims = gcnew array<BiosShim^>( 1024 );
#ifdef SYSCALLSTATS
	_syscallCounts = gcnew array<int>( 1024 );
#endif

	_hasExecuted = false;

	R4000AdvancedBlockBuilder^ builder = gcnew R4000AdvancedBlockBuilder( this, _core0 );
	R4000Generator* gen = new R4000Generator();
	_context = gcnew R4000GenContext( builder, gen );
	_biosStubs = gcnew R4000BiosStubs();
	_videoInterface = gcnew R4000VideoInterface( this );

	_bounce = builder->BuildBounce();

	_globalCpuFieldInfo = ( R4000Cpu::typeid )->GetField( "GlobalCpu" );
	_privateMemoryFieldInfo = ( R4000Cpu::typeid )->GetField( "_memory" );
}

R4000Cpu::~R4000Cpu()
{
	if( _ctx != NULL )
		_aligned_free( _ctx );
	_ctx = NULL;
	SAFEFREE( _bounce );
}

int R4000Cpu::RegisterSyscall( unsigned int nid )
{
	BiosFunction^ function = _emu->Bios->FindFunction( nid );
	if( function == nullptr )
		return -1;

	int sid = ++_lastSyscall;
	_syscalls[ sid ] = function;

	void* memory = ( void* )_memory->MainMemory;
	void* registers = ( ( R4000Ctx* )_ctx )->Registers;
	_syscallShims[ sid ] = EmitShim( function, memory, registers );

	return sid;
}

void R4000Cpu::Cleanup()
{
	_memory->Clear();
}

int R4000Cpu::ExecuteBlock()
{
#ifdef STATISTICS
	double blockStart = _timer->Elapsed;
	if( _stats->RunTime == 0.0 )
		_stats->RunTime = _timer->Elapsed;
#endif

	if( _hasExecuted == false )
	{
		// Prepare tracer
#ifdef TRACE
		Tracer::OpenFile( TRACEFILE );
#endif

		// Has to happen late in the game because we need to
		// make sure the video subsystem is ready
		_videoInterface->Prepare();

		_hasExecuted = true;
	}

	R4000Ctx* ctx = ( R4000Ctx* )_ctx;
	//if( ctx->PC == 0 )
	//	ctx->PC = _core0->PC;

	// Get/build block
	int pc = ctx->PC & 0x3FFFFFFF;
	CodeBlock^ block = _codeCache->Find( pc );
	if( block == nullptr )
	{
		block = _context->_builder->Build( pc );
#ifdef STATISTICS
		_stats->CodeCacheMisses++;
#endif
	}
	else
	{
#ifdef STATISTICS
		_stats->CodeCacheHits++;
#endif
	}
	Debug::Assert( block != nullptr );

#ifdef STATISTICS
	_stats->ExecutionLoops++;

	block->ExecutionCount++;
#endif

	//Debug::WriteLine( String::Format( "Executing block 0x{0:X8} (codegen at 0x{1:X8})", pc, (uint)block->Pointer ) );

	// Populate ctx
	//ctx->PCValid = false;
	//ctx->NullifyDelay = false;

#ifdef STATISTICS
	uint startInstrExec = _instructionsExecuted;
#endif

	// Bounce in to it
	bouncefn bounce = ( bouncefn )_bounce;
	int x = bounce( ( int )block->Pointer );

	// PC updated via __updateCorePC
	_core0->DelayNop = ( ctx->NullifyDelay == 1 ) ? true : false;

#ifdef STATISTICS

	double blockTime = _timer->Elapsed - blockStart;
	if( blockTime <= 0.0 )
		blockTime = 0.000001;

	uint instructionsExecuted = _instructionsExecuted - startInstrExec;
	_stats->IPS = ( _stats->IPS * .8 ) + ( ( ( double )instructionsExecuted / blockTime ) * .2 );
	
	_timeSinceLastIpsPrint += blockTime;
	if( _timeSinceLastIpsPrint > 1.0 )
	{
		double ips = ( ( double )instructionsExecuted / blockTime );
		//Debug::WriteLine( String::Format( "IPS: {0}", ( long )ips ) );
		_timeSinceLastIpsPrint = 0.0;
	}

	return instructionsExecuted;
#else
	return 1;
#endif
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
	StringBuilder^ sb = gcnew StringBuilder();
	array<FieldInfo^>^ fields = ( R4000Statistics::typeid )->GetFields();
	for( int n = 0; n < fields->Length; n++ )
	{
		Object^ value = fields[ n ]->GetValue( _stats );
		sb->AppendFormat( "{0}: {1}\n", fields[ n ]->Name, value );
	}
	Debug::WriteLine( "Ultra CPU Statistics: ---------------------------------------" );
	Debug::WriteLine( sb->ToString() );

#ifdef SYSCALLSTATS
	// Syscall stats
	int callCount = 0;
	for( int n = 0; n < _syscallCounts->Length; n++ )
	{
		int value = _syscallCounts[ n ];
		callCount += value;
	}

	Debug::WriteLine( "Syscall statistics (in percent of all calls):" );
	for( int n = 0; n < _syscallCounts->Length; n++ )
	{
		int value = _syscallCounts[ n ];
		if( value == 0 )
			continue;
		BiosFunction^ func = _syscalls[ n ];
		float p = value / ( float )callCount;
		p *= 100.0f;
		Debug::WriteLine( String::Format( "{0,-50} {1,10}x, {2,3}%",
			String::Format( "{0}::{1}:", func->Module->Name, func->Name ), value, p ) );
	}
#endif
	Debug::WriteLine( "" );
#endif
}

// The shim is a small call that loads the required number of arguments, calls the
// method, and stores back the return value.
// Since at shim generation time we know the fixed memory and register (in ctx) addresses
// we can even put those right in to the generated code, meaning the shim need not take
// any arguments.
// We still support passing IMemory to the bios functions - as long as it is defined
// as the first parameter will we pass it through.

// Here we are in the x86 dynarec CPU emitting MSIL. I'm craaazzy ----___----
BiosShim^ R4000Cpu::EmitShim( BiosFunction^ function, void* memory, void* registers )
{
	Type^ voidStar = ( void::typeid )->MakePointerType();
	array<Type^>^ shimArgs = { voidStar, voidStar };

	DynamicMethod^ shim = gcnew DynamicMethod( String::Format( "Shim{0:X8}", function->NID ),
		void::typeid, shimArgs,
		BiosShim::typeid->Module );
	ILGenerator^ ilgen = shim->GetILGenerator();

	// TRICK: if we have a return, we'll need a local to store the return from the
	// function while we build the address (cause order is address, value for stind).
	// BUT: if we push the return address stuff here we can just stind the value
	// after the function call! We saved a local! Woo!
	// BUT: this only works for int32 returns. With 64 we need to store the value around.
	// MAYBE: we could use stind_i8?
	if( function->HasReturn == true )
	{
		// Shared code for $v0
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );		// load registers
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Ldc_I4, 2 << 2 );					// v0 = $2
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Add );							// register base + register offset

		if( function->DoubleWordReturn == true )
		{
			// 64 bit return - lower in v0 ($2), upper in v1 ($3)

			// Local for storage - we only need 32 bits
			ilgen->DeclareLocal( int::typeid );

			ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );	// load registers
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Ldc_I4, 3 << 2 );				// v1 = $3
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );						// register base + register offset

			// stack now contains address of $v0, $v1
		}
		else
		{
			// 32 bit return - in v0 ($2)
			
			// stack now contains address of $v0
		}
	}

	// Push on parameters (in reverse order)
	Debug::Assert( function->ParameterCount <= 12 );
	for( int n = function->ParameterCount; n > 0; n-- )
	{
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );	// load registers
		ilgen->Emit( OpCodes::Conv_I );
		switch( n - 1 )
		{
		case 0:
		case 1:
		case 2:
		case 3:
			// Emit a0-a3
			ilgen->Emit( OpCodes::Ldc_I4, ( 4 + n ) << 2 );	// a0 = $4...
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// register base + register offset
			ilgen->Emit( OpCodes::Ldind_I4 );				// load value
			break;
		case 4:
		case 5:
		case 6:
		case 7:
			// Emit t0-t3
			ilgen->Emit( OpCodes::Ldc_I4, ( 4 + n ) << 2 );	// t0 = $8... - +4 works out because we are case 4-7!
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// register base + register offset
			ilgen->Emit( OpCodes::Ldind_I4 );				// load value
			break;
		case 8:
		case 9:
		case 10:
		case 11:
			// Emit sp + 0/4/8/12
			// value = memory[ ( reg( sp ) - 0x08000000 ) + offset ]
			ilgen->Emit( OpCodes::Ldc_I4, 29 << 2 );		// sp = $29...
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// register base + register offset
			ilgen->Emit( OpCodes::Ldind_I4 );				// load $sp
			ilgen->Emit( OpCodes::Ldc_I4, MainMemoryBase );	// 0x08000000
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Sub );					// addr = $sp - 0x08000000
			ilgen->Emit( OpCodes::Ldc_I4, ( n - 8 ) << 2 ); // offset + 0..4 words
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// addr = ( $sp - 0x08000000 ) + offset
			ilgen->Emit( OpCodes::Ldc_I4, ( int )memory );	// load memory
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// finaladdr = addr + memory ptr
			ilgen->Emit( OpCodes::Ldind_I4 );				// load value
			break;
		default:
			throw gcnew NotSupportedException( "R4000Ultra - EmitInvoke - cannot emit more than 12 arguments" );
		}
	}

	// Handle IMemory usage - this is tricky
	if( function->UsesMemorySystem == true )
	{
		// Perform a R4000Cpu::GlobalCpu->_memory load
		ilgen->Emit( OpCodes::Ldfld, _globalCpuFieldInfo );
		ilgen->Emit( OpCodes::Ldfld, _privateMemoryFieldInfo );
	}

	// Invoke method
	ilgen->EmitCall( OpCodes::Call, function->MethodInfo, nullptr );

	// Handle return
	if( function->HasReturn == true )
	{
		if( function->DoubleWordReturn == true )
		{
			// 64 bit return - lower in v0 ($2), upper in v1 ($3)
			// stack contains long

			// Put ret in local - we truncate to 32 bits so we don't have to do it later
			ilgen->Emit( OpCodes::Dup );
			ilgen->Emit( OpCodes::Conv_I4 );
			ilgen->Emit( OpCodes::Stloc_0 );

			// We need to store $v1 (upper word) - we still have the long on the stack
			ilgen->Emit( OpCodes::Ldc_I4, 32 );
			ilgen->Emit( OpCodes::Shr_Un );					// =>> 32
			ilgen->Emit( OpCodes::Conv_I4 );
			ilgen->Emit( OpCodes::Stind_I4 );				// store in $v1

			// Load back local for $v0 code below
			ilgen->Emit( OpCodes::Ldloc_0 );
		}
		else
		{
			// 32 bit return - in v0 ($2)
			// stack contains int
		}

		// Shared code for $v0
		ilgen->Emit( OpCodes::Stind_I4 );					// store in $v0
	}
	else
	{
#ifdef SAFESYSCALLRETURN
		// Always set v0 ($2) to 0, because a lot of our stubs may say they have no return but really do
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );	// load registers
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Ldc_I4, 2 << 2 );				// v0 = $2
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Add );						// register base + register offset
		ilgen->Emit( OpCodes::Ldc_I4_0 );
		ilgen->Emit( OpCodes::Stind_I4 );					// store 0 in $v0
#endif
	}

	ilgen->Emit( OpCodes::Ret );

	BiosShim^ del = ( BiosShim^ )shim->CreateDelegate( BiosShim::typeid );
	Debug::Assert( del != nullptr );

	return del;
}
