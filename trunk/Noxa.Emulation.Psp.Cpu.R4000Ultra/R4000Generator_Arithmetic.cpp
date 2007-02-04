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

GenerationResult SLL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->shl( EAX, shamt );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SRL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->shr( EAX, shamt );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SRA( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->sar( EAX, shamt );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SLLV( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->mov( EBX, MREG( rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
		g->shl( EAX, CL );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SRLV( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->mov( EBX, MREG( rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
		g->shr( EAX, CL );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SRAV( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->mov( EBX, MREG( rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
		g->sar( EAX, CL );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MOVZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		char label[20];
		sprintf( label, "l%X", address );
		g->mov( EAX, MREG( rt ) );
		g->cmp( EAX, 0 );
		g->jne( label );
		g->mov( EAX, MREG( rs ) );
		g->mov( MREG( rd ), EAX );
		g->label( label );
	}
	return GenerationResult::Success;
}

GenerationResult MOVN( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		char label[20];
		sprintf( label, "l%X", address );
		g->mov( EAX, MREG( rt ) );
		g->cmp( EAX, 0 );
		g->je( label );
		g->mov( EAX, MREG( rs ) );
		g->mov( MREG( rd ), EAX );
		g->label( label );
	}
	return GenerationResult::Success;
}

GenerationResult MFHI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MHI() );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MTHI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( MHI(), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MFLO( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MLO() );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MTLO( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( MLO(), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MULT( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->imul( MREG( rt ) );
		g->mov( MLO(), EAX );
		g->mov( MHI(), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult MULTU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mul( MREG( rt ) );
		g->mov( MLO(), EAX );
		g->mov( MHI(), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult DIV( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EDX, 0 );
		g->idiv( MREG( rt ) );
		g->mov( MLO(), EAX );
		g->mov( MHI(), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult DIVU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EDX, 0 );
		g->div( MREG( rt ) );
		g->mov( MLO(), EAX );
		g->mov( MHI(), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult ADD( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->add( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult ADDU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->add( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SUB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->sub( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SUBU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->sub( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult AND( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->and( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult OR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->or( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult XOR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->xor( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult NOR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->or( EAX, EBX );
		g->not( EAX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SLT( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->setl( BL );
		g->mov( MREG( rd ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult SLTU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, MREG( rt ) );
		g->setb( BL );
		g->mov( MREG( rd ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult MAX( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->cmp( EAX, EBX );
		g->cmovl( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MIN( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->mov( EBX, MREG( rt ) );
		g->cmp( EAX, EBX );
		g->cmovg( EAX, EBX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult ADDI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult ADDIU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SLTI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, SE( imm ) );
		g->setl( BL );
		g->mov( MREG( rt ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult SLTIU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( rs ) );
		g->cmp( EAX, SE( imm ) );
		g->setb( BL );
		g->mov( MREG( rt ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult ANDI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->and( EAX, ZE( imm ) );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult ORI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->or( EAX, ZE( imm ) );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult XORI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->xor( EAX, ZE( imm ) );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LUI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( MREG( rt ), ( int )( ( uint )imm << 16 ) );
	}
	return GenerationResult::Success;
}

GenerationResult EXT( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rt == 0 )
		return GenerationResult::Success;

	// 89032c8:	7c823dc0 	ext	v0,a0,0x17,0x8
	// 890946c:	7cc75500 	ext	a3,a2,0x14,0xb
	// GPR[rt] = 0:32-(msbd+1): || GPR[rs]:msbd+lsb..lsb:

	// size in rd, pos in function
	byte rs = ( byte )( ( code >> 21 ) & 0x1F );

	// This is kind of a trick I thought of ^_^
	long bittemp = 0x00000000FFFFFFFF;
	bittemp <<= rd + 1;
	bittemp >>= 32;
	int bitmask = ( int )bittemp;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// value =>> pos
		// value &= bitfield
		g->mov( EAX, MREG( rs ) );
		g->shr( EAX, function );
		g->and( EAX, bitmask );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult INS( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rt == 0 )
		return GenerationResult::Success;

	// 8909260:	7c4df504 	ins	t5,v0,0x14,0xb
	// GPR[rt] = GPR[rt]31..msb+1 || GPR[rs]msb-lsb..0 || GPR[rt]lsb-1..0

	// size in rd, pos in function
	// rs is bitmask, rt is value
	byte rs = ( byte )( ( code >> 21 ) & 0x1F );

	int size = rd - function + 1;

	// This is kind of a trick I thought of ^_^
	long bittemp = 0x00000000FFFFFFFF;
	bittemp <<= size;
	bittemp >>= 32;
	int rsmask = ( int )bittemp;
	bittemp <<= function;
	int rtmask = ( int )bittemp;
	rtmask = ~rtmask;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->and( EAX, rsmask );
		g->shl( EAX, function );
		g->mov( EBX, MREG( rt ) );
		g->and( EBX, rtmask );
		g->or( EAX, EBX );
		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SEB( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->movsx( EAX, AL );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult SEH( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rt ) );
		g->movsx( EAX, AX );
		g->mov( MREG( rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult CLZ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult CLO( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}
