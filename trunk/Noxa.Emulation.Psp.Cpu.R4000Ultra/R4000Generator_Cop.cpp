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

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

#define g context->Generator

GenerationResult BCzF( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		g->mov( EAX, MCP1CONDBIT( CTX ) );
		// EAX = 1 if cond true, 0 if cond false
		g->xor( EAX, 0x1 ); // <- flip, as we are F
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BCzFL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		g->mov( EAX, MCP1CONDBIT( CTX ) );
		// EAX = 1 if cond true, 0 if cond false
		g->xor( EAX, 0x1 ); // <- flip, as we are F
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BCzT( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		g->mov( EAX, MCP1CONDBIT( CTX ) );
		// EAX = 1 if cond true, 0 if cond false
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BCzTL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		g->mov( EAX, MCP1CONDBIT( CTX ) );
		// EAX = 1 if cond true, 0 if cond false
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult MFCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MCP1REG( CTX, rs, 0 ) );
		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MTCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	Debug::Assert( opcode == 1, "Only support COP1" );

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( MCP1REG( CTX, rs, 0 ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult CFCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	return GenerationResult::Invalid;
}

GenerationResult CTCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	return GenerationResult::Invalid;
}
