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
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( MPC(), EAX );
		g->mov( MPCVALID(), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult JALR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	//bool theEnd = ( context->LastBranchTarget <= address );

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( MREG( rd ), address + 4 );
		g->mov( EAX, MREG( rs ) );
		g->mov( MPC(), EAX );
		g->mov( MPCVALID(), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult J( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	//bool theEnd = ( context->LastBranchTarget <= address );

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		uint target = code & 0x03FFFFFF;
		uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );
		g->mov( MPC(), pc );
		g->mov( MPCVALID(), 1 );
	}
	return GenerationResult::Jump;
}

GenerationResult JAL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	//bool theEnd = ( context->LastBranchTarget <= address );

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( MREG( 31 ), address + 4 );
		uint target = code & 0x3FFFFFF;
		uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );
		g->mov( MPC(), pc );
		g->mov( MPCVALID(), 1 );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->sete( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->setne( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setle( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setg( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->sete( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setne( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
	}
	return GenerationResult::Branch;
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->setne( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->sete( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
	}
	return GenerationResult::Branch;
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setle( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setnle( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setg( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setng( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setnl( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setnge( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->cmovl( EAX, address + 4 );
		g->cmovnl( EAX, MREG( 31 ) );
		g->mov( MREG( 31 ), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->cmovge( EAX, address + 4 );
		g->cmovnge( EAX, MREG( 31 ) );
		g->mov( MREG( 31 ), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setl( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setnl( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
		g->cmovl( EAX, address + 4 );
		g->cmovnl( EAX, MREG( 31 ) );
		g->mov( MREG( 31 ), EAX );
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

		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, 0 );
		g->setge( AL );
		g->movzx( EAX, AL );
		g->mov( MPCVALID(), EAX );
		g->setnge( AL );
		g->movzx( EAX, AL );
		g->mov( MNULLDELAY(), EAX );
		g->cmovge( EAX, address + 4 );
		g->cmovnge( EAX, MREG( 31 ) );
		g->mov( MREG( 31 ), EAX );
		// TODO: some more elegant code
		// if( true )
		//     eax = address + 4
		// else
		//     eax = $31
		// $31 = eax
	}
	return GenerationResult::BranchAndNullifyDelay;
}
