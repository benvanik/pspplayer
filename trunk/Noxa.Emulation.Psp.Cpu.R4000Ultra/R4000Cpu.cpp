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

#include "R4000AdvancedBlockBuilder.h"
#include "R4000Generator.h"
#include "R4000Ctx.h"
#include "R4000BiosStubs.h"
#include "R4000VideoInterface.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

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
		// Has to happen late in the game because we need to
		// make sure the video subsystem is ready
		_videoInterface->Prepare();

		_hasExecuted = true;
	}

	R4000Ctx* ctx = ( R4000Ctx* )_ctx;
	//if( ctx->PC == 0 )
	//	ctx->PC = _core0->PC;

	// Get/build block
	int pc = ctx->PC;
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
	ctx->InstructionCount = 0;

	// Bounce in to it
	bouncefn bounce = ( bouncefn )_bounce;
	int x = bounce( ( int )block->Pointer );

	// PC updated via __updateCorePC
	_core0->DelayNop = ( ctx->NullifyDelay == 1 ) ? true : false;

	int instructionsExecuted = ctx->InstructionCount;

#ifdef STATISTICS

	_stats->InstructionsExecuted += instructionsExecuted;

	double blockTime = _timer->Elapsed - blockStart;
	if( blockTime <= 0.0 )
		blockTime = 0.000001;

	_stats->IPS = ( _stats->IPS * .8 ) + ( ( ( double )instructionsExecuted / blockTime ) * .2 );
	
	_timeSinceLastIpsPrint += blockTime;
	if( _timeSinceLastIpsPrint > 1.0 )
	{
		double ips = ( ( double )instructionsExecuted / blockTime );
		//Debug::WriteLine( String::Format( "IPS: {0}", ( long )ips ) );
		_timeSinceLastIpsPrint = 0.0;
	}
#endif

	return instructionsExecuted;
}

void R4000Cpu::PrintStatistics()
{
#ifdef STATISTICS
		if( _stats->InstructionsExecuted == 0 )
			return;
		_stats->GatherStats();
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
#endif
}