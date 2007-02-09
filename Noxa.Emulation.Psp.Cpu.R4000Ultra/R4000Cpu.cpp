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

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

R4000Cpu::R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalCpu = this;

	// Ugly: has to be above the block builder constructor!
	_ctx = new R4000Ctx();

	_emu = emulator;
	_params = parameters;
	_caps = gcnew R4000Capabilities();
	_clock = gcnew R4000Clock();
	_memory = gcnew R4000Memory();
	_core0 = gcnew R4000Core( this, ( R4000Ctx* )_ctx );
	_codeCache = gcnew R4000Cache();

#ifdef _DEBUG
	_timer = gcnew PerformanceTimer();
	_timeSinceLastIpsPrint = 0.0;
#endif

	_lastSyscall = -1;
	_syscalls = gcnew array<BiosFunction^>( 1024 );

	R4000AdvancedBlockBuilder^ builder = gcnew R4000AdvancedBlockBuilder( this, _core0 );
	R4000Generator* gen = new R4000Generator();
	_context = gcnew R4000GenContext( builder, gen );

	_bounce = builder->BuildBounce();
}

R4000Cpu::~R4000Cpu()
{
	if( _ctx != NULL )
		delete ( R4000Ctx* )_ctx;
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
#ifdef _DEBUG
	double blockStart = _timer->Elapsed;
#endif

	int instructionsExecuted = 0;

	R4000Ctx* ctx = ( R4000Ctx* )_ctx;
	//if( ctx->PC == 0 )
	//	ctx->PC = _core0->PC;

	// Get/build block
	int pc = ctx->PC;
	CodeBlock^ block = _codeCache->Find( pc );
	if( block == nullptr )
		block = _context->_builder->Build( pc );
	Debug::Assert( block != nullptr );

	//Debug::WriteLine( String::Format( "Executing block 0x{0:X8} (codegen at 0x{1:X8})", pc, (uint)block->Pointer ) );

	// Populate ctx
	//ctx->PCValid = false;
	//ctx->NullifyDelay = false;
	//memcpy( ctx->Registers, _core0->Registers, sizeof( int ) * 32 );
	//ctx->LO = _core0->LO;
	//ctx->HI = _core0->HI;
	//ctx->Cp1ConditionBit = _core0->Cp1->ConditionBit;
	// TODO: fixup cp1 to use float* so memcpy can happen
	//pin_ptr<float> cp1r = &_core0->Cp1->Registers[ 0 ];
	//memcpy( ctx->Cp1Registers, ( void* )cp1r, sizeof( float ) * 32 );

	// Bounce in to it
	bouncefn bounce = ( bouncefn )_bounce;
	int x = bounce( ( int )block->Pointer );

	// PC updated via __updateCorePC
	//_core0->PC = ctx->PC;
	_core0->DelayNop = ( ctx->NullifyDelay == 1 ) ? true : false;
	// Registers in ctx are a memory reference to core0 registers
	//memcpy( _core0->Registers, ctx->Registers, sizeof( int ) * 32 );
	//_core0->LO = ctx->LO;
	//_core0->HI = ctx->HI;
	//_core0->Cp1->ConditionBit = ( ctx->Cp1ConditionBit == 1 ) ? true : false;
	// TODO: make cp1 registers in ctx a ref to cp1 registers (fix cp1 registers first)
	//memcpy( ( void* )cp1r, ctx->Cp1Registers, sizeof( float ) * 32 );

#ifdef _DEBUG
	double blockTime = _timer->Elapsed - blockStart;
	if( blockTime <= 0.0 )
		blockTime = 0.000001;
	
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