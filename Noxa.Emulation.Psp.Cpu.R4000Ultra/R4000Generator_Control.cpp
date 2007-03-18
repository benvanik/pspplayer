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

#include "Loader.hpp"
#include "CodeGenerator.hpp"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

#define g context->Generator

GenerationResult JR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	context->JumpTarget = 0x0;
	context->JumpRegister = rs;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MPC( CTX ), EAX );
		g->mov( MPCVALID( CTX ), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult JALR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	//bool theEnd = ( context->LastBranchTarget <= address );

	context->JumpTarget = 0x0;
	context->JumpRegister = rs;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( MREG( CTX, rd ), address + 4 );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MPC( CTX ), EAX );
		g->mov( MPCVALID( CTX ), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult J( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	//bool theEnd = ( context->LastBranchTarget <= address );
	uint target = code & 0x3FFFFFF;
	uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );

	context->JumpTarget = pc;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( MPC( CTX ), pc );
		g->mov( MPCVALID( CTX ), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult JAL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	//bool theEnd = ( context->LastBranchTarget <= address );
	uint target = code & 0x3FFFFFF;
	uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );

	context->JumpTarget = pc;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( MREG( CTX, 31 ), address + 4 );
		g->mov( MPC( CTX ), pc );
		g->mov( MPCVALID( CTX ), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult BEQ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->sete( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BNE( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->setne( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BLEZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setle( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BGTZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setg( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BEQL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->sete( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BNEL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->setne( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BLEZL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setle( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BGTZL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	int target = address + ( SE( imm ) << 2 );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setg( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BLTZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BGEZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
	}
	return GenerationResult::Branch;
}

GenerationResult BLTZL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BGEZL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BLTZAL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->cmovl( EAX, address + 4 );
		g->cmovnl( EAX, MREG( CTX, 31 ) );
		g->mov( MREG( CTX, 31 ), EAX );
		// TODO: some more elegant code
		// if( true )
		//     eax = address + 4
		// else
		//     eax = $31
		// $31 = eax
	}
	return GenerationResult::Branch;
}

GenerationResult BGEZAL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->cmovge( EAX, address + 4 );
		g->cmovnge( EAX, MREG( CTX, 31 ) );
		g->mov( MREG( CTX, 31 ), EAX );
		// TODO: some more elegant code
		// if( true )
		//     eax = address + 4
		// else
		//     eax = $31
		// $31 = eax
	}
	return GenerationResult::Branch;
}

GenerationResult BLTZALL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
		g->cmovl( EAX, address + 4 );
		g->cmovnl( EAX, MREG( CTX, 31 ) );
		g->mov( MREG( CTX, 31 ), EAX );
		// TODO: some more elegant code
		// if( true )
		//     eax = address + 4
		// else
		//     eax = $31
		// $31 = eax
	}
	return GenerationResult::BranchAndNullifyDelay;
}

GenerationResult BGEZALL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	int target = address + ( SE( imm ) << 2 );
	int rs = ( int )( ( code >> 21 ) & 0x1F );

	if( pass == 0 )
	{
		context->DefineBranchTarget( target );
	}
	else if( pass == 1 )
	{
		LabelMarker^ targetLabel = context->BranchLabels[ target ];
		Debug::Assert( targetLabel != nullptr );
		context->BranchTarget = targetLabel;

		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID( CTX ), EAX );
		g->xor( EAX, 0x1 ); // nulldelay = !pcvalid
		g->mov( MNULLDELAY( CTX ), EAX );
		g->cmovge( EAX, address + 4 );
		g->cmovnge( EAX, MREG( CTX, 31 ) );
		g->mov( MREG( CTX, 31 ), EAX );
		// TODO: some more elegant code
		// if( true )
		//     eax = address + 4
		// else
		//     eax = $31
		// $31 = eax
	}
	return GenerationResult::BranchAndNullifyDelay;
}
