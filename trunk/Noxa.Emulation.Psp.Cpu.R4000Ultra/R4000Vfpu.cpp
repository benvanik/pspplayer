// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Generator.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000GenContext.h"
#include "R4000Vfpu.h"

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

#define g context->Generator

#define MCP2REG( xr, r )		g->dword_ptr[ xr + CTXCP2REGS + ( r << 4 ) ]

enum VfpuConditions
{
	VFPU_FL,	VFPU_EQ,	VFPU_LT,	VFPU_LE,	VFPU_TR,	VFPU_NE,	VFPU_GE,	VFPU_GT,
	VFPU_EZ,	VFPU_EN,	VFPU_EI,	VFPU_ES,	VFPU_NZ,	VFPU_NN,	VFPU_NI,	VFPU_NS,
};
enum VfpuConstants
{
	VFPU_VALUE,	VFPU_HUGE,	VFPU_SQRT2,	VFPU_SQRT1_2,	VFPU_2_SQRTPI,	VFPU_2_PI,
	VFPU_1_PI,	VFPU_PI_4,	VFPU_PI_2,	VFPU_PI,		VFPU_E,			VFPU_LOG2E,	VFPU_LOG10E,
	VFPU_LN2,	VFPU_LN10,	VFPU_2PI,	VFPU_PI_6,		VFPU_LOG10TWO,	VFPU_LOG2TEN,
	VFPU_SQRT3_2,
	VFPU_NUM_CONSTANTS,
};
enum VfpuRwb{		VFPU_WT,	VFPU_WB,	VFPU_RWB3,	VFPU_RWB4,	};
enum VfpuPfxCst{	VFPU_CST_0,	VFPU_CST_1,	VFPU_CST_2,	VFPU_CST_1_2,	VFPU_CST_3,	VFPU_CST_1_3,	VFPU_CST_1_4,	VFPU_CST_1_6,	};
enum VfpuPfxSwz{	VFPU_X,	VFPU_Y,	VFPU_Z,	VFPU_W,	};
enum VfpuPfxSat{	VFPU_SAT0,	VFPU_SAT_0_1,	VFPU_SAT2,	VFPU_SAT_1_1,	};

enum VfpuAttributes
{
	VFPU_NORMAL			= 0x00,
	VFPU_BRANCH			= 0x01,
	VFPU_BRANCH_LIKELY	= 0x02,
};
typedef int (*VfpuImpl)( R4000Ctx* ctx, uint address, uint code );
typedef bool (*VfpuGen)( R4000GenContext^ context, int address, uint code );
typedef struct VfpuInstruction_t
{
	char*			Name;
	char*			Arguments;
	uint			Opcode;
	uint			Mask;
	uint			Attributes;
	VfpuGen			Generate;
	VfpuImpl		Execute;
} VfpuInstruction;
bool VfpuGenDummy( R4000GenContext^ context, int address, uint code );
int VfpuImplDummy( R4000Ctx* ctx, uint address, uint code );

// Contains _vfpuInstructions table, with _vfpuNumInstructions entries
#include "R4000Vfpu_Instructions.h"

GenerationResult Noxa::Emulation::Psp::Cpu::TryEmitVfpu( R4000GenContext^ context, int pass, int address, uint code )
{
	// Look up instruction - slowly
	bool foundInstruction = false;
	VfpuInstruction* instr = ( VfpuInstruction* )&_vfpuInstructions[ 0 ];
	for( int n = 0; n < _vfpuNumInstructions; n++, instr++ )
	{
		if( ( code & instr->Mask ) == instr->Opcode )
		{
			// Found!
			foundInstruction = true;
			break;
		}
	}
	if( foundInstruction == false )
		return GenerationResult::Invalid;

#ifdef _DEBUG
	if( pass == 0 )
	{
		Debug::WriteLine( String::Format( "[0x{0:X8}] {1:X8}\t{2} {3}", address - 4, code, gcnew String( instr->Name ), gcnew String( instr->Arguments ) ) );
	}
#endif

	GenerationResult result = GenerationResult::Success;
	if( instr->Attributes  == VFPU_BRANCH )
		result = GenerationResult::Branch;
	else if( instr->Attributes == VFPU_BRANCH_LIKELY )
		result = GenerationResult::BranchAndNullifyDelay;
	bool isBranch = ( result != GenerationResult::Success );

	if( pass == 0 )
	{
		if( isBranch == true )
		{
			int target = address + ( SE( code & 0xFFFF ) << 2 );
			context->DefineBranchTarget( target );
		}
		return result;
	}
	else if( pass == 1 )
	{
		if( isBranch == true )
		{
			int target = address + ( SE( code & 0xFFFF ) << 2 );
			LabelMarker^ targetLabel = context->BranchLabels[ target ];
			Debug::Assert( targetLabel != nullptr );
			context->BranchTarget = targetLabel;
		}

		if( instr->Generate != VfpuGenDummy )
		{
			// Emit code
			instr->Generate( context, address, code );
		}
		else
		{
			// Call
			g->push( code );
			g->push( ( uint )address );
			g->push( ( uint )CTX );
			g->call( instr->Execute );
			g->add( ESP, 12 );
		}

		if( isBranch == true )
		{
			// EAX contains branch result (0 = no branch, 1 = branch), store it back
			g->mov( MPCVALID( CTX ), EAX );
			if( result == GenerationResult::BranchAndNullifyDelay )
			{
				g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
				g->mov( MNULLDELAY( CTX ), EAX );
			}
		}

		return result;
	}
	else
		return GenerationResult::Invalid;
}

bool VfpuGenDummy( R4000GenContext^ context, int address, uint code )
{
	// ^_^
	return true;
}

int VfpuImplDummy( R4000Ctx* ctx, uint address, uint code )
{
	// ^_^
	return 0;
}
