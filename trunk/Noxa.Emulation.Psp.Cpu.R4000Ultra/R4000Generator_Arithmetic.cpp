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
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

#define g context->Generator

// Will try to prevent some crazy scenarios
//#define SAFEARITHMETIC

GenerationResult SLL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( rd == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			g->shl( EAX, shamt );
		else
			Debug::Assert( false );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			g->shr( EAX, shamt );
		else
			Debug::Assert( false );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			g->sar( EAX, shamt );
		else
			Debug::Assert( false );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( EBX, MREG( CTX, rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
#ifdef SAFEARITHMETIC
		char skip[20];
		sprintf_s( skip, 20, "l%0Xs", address - 4 );
		g->cmp( CL, 0 );
		g->jl( skip );
		g->cmp( CL, 32 );
		g->jge( skip );
#endif
		g->shl( EAX, CL );
#ifdef SAFEARITHMETIC
		g->label( skip );
#endif
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( EBX, MREG( CTX, rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
#ifdef SAFEARITHMETIC
		char skip[20];
		sprintf_s( skip, 20, "l%0Xs", address - 4 );
		g->cmp( CL, 0 );
		g->jl( skip );
		g->cmp( CL, 32 );
		g->jge( skip );
#endif
		g->shr( EAX, CL );
#ifdef SAFEARITHMETIC
		g->label( skip );
#endif
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( EBX, MREG( CTX, rs ) );
		g->and( EBX, 0x1F );
		g->mov( CL, BL );
#ifdef SAFEARITHMETIC
		char skip[20];
		sprintf_s( skip, 20, "l%0Xs", address - 4 );
		g->cmp( CL, 0 );
		g->jl( skip );
		g->cmp( CL, 32 );
		g->jge( skip );
#endif
		g->sar( EAX, CL );
#ifdef SAFEARITHMETIC
		g->label( skip );
#endif
		g->mov( MREG( CTX, rd ), EAX );
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
		Label* l = g->DefineLabel();
		g->mov( EAX, MREG( CTX, rt ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->jne( l );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MREG( CTX, rd ), EAX );
		g->MarkLabel( l );
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
		Label* l = g->DefineLabel();
		g->mov( EAX, MREG( CTX, rt ) );
		g->test( EAX, EAX ); // cmp EAX, 0
		g->je( l );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MREG( CTX, rd ), EAX );
		g->MarkLabel( l );
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
		g->mov( EAX, MHI( CTX ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MHI( CTX ), EAX );
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
		g->mov( EAX, MLO( CTX ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MLO( CTX ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->imul( MREG( CTX, rt ) );
		g->mov( MLO( CTX ), EAX );
		g->mov( MHI( CTX ), EDX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->mul( MREG( CTX, rt ) );
		g->mov( MLO( CTX ), EAX );
		g->mov( MHI( CTX ), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult MUL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rs ) );
		g->imul( MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult MADD( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rs ) );
		g->imul( MREG( CTX, rt ) );
		g->add( MLO( CTX ), EAX );
		g->adc( MHI( CTX ), EDX );	// add with carry for 64 bit add
	}
	return GenerationResult::Success;
}

GenerationResult MADDU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rs ) );
		g->mul( MREG( CTX, rt ) );
		g->add( MLO( CTX ), EAX );
		g->adc( MHI( CTX ), EDX );	// add with carry for 64 bit add
	}
	return GenerationResult::Success;
}

GenerationResult MSUB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rs ) );
		g->imul( MREG( CTX, rt ) );
		g->sub( MLO( CTX ), EAX );
		g->sbb( MHI( CTX ), EDX );
	}
	return GenerationResult::Success;
}

GenerationResult MSUBU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rs ) );
		g->mul( MREG( CTX, rt ) );
		g->sub( MLO( CTX ), EAX );
		g->sbb( MHI( CTX ), EDX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->cdq();						// Sign extend EAX in to EDX
		g->idiv( MREG( CTX, rt ) );
		g->mov( MLO( CTX ), EAX );
		g->mov( MHI( CTX ), EDX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->xor( EDX, EDX );
		g->div( MREG( CTX, rt ) );
		g->mov( MLO( CTX ), EAX );
		g->mov( MHI( CTX ), EDX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->add( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->add( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->sub( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->sub( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->and( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->or( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->xor( EAX, MREG( CTX, rt ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->or( EAX, MREG( CTX, rt ) );
		g->not( EAX );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->setl( BL );
		g->movzx( EBX, BL );
		g->mov( MREG( CTX, rd ), EBX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->setb( BL );
		//g->setl( BL );
		g->movzx( EBX, BL );
		g->mov( MREG( CTX, rd ), EBX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->cmp( EAX, EBX );
		g->cmovl( EAX, EBX );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->cmp( EAX, EBX );
		g->cmovg( EAX, EBX );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->add( EAX, SE( imm ) );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->add( EAX, SE( imm ) );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, SE( imm ) );
		g->setl( BL );
		g->movzx( EBX, BL );
		g->mov( MREG( CTX, rt ), EBX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, SE( imm ) );
		g->setb( BL );
		//g->setl( BL );
		g->movzx( EBX, BL );
		g->mov( MREG( CTX, rt ), EBX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->and( EAX, ZE( imm ) );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->or( EAX, ZE( imm ) );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		g->xor( EAX, ZE( imm ) );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( MREG( CTX, rt ), ( int )( ( uint )imm << 16 ) & 0xFFFF0000 );
	}
	return GenerationResult::Success;
}

GenerationResult EXT( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// 89032c8:	7c823dc0 	ext	v0,a0,0x17,0x8
		// 890946c:	7cc75500 	ext	a3,a2,0x14,0xb
		// GPR[rt] = 0:32-(msbd+1): || GPR[rs]:msbd+lsb..lsb:

		// size in rd, pos in function
		byte rs = ( byte )( ( code >> 21 ) & 0x1F );

		// This is kind of a trick I thought of ^_^
		uint64 bittemp = 0x00000000FFFFFFFF;
		bittemp <<= rd + 1;
		bittemp >>= 32;
		int bitmask = ( int )bittemp;

		// value =>> pos
		// value &= bitfield
		g->mov( EAX, MREG( CTX, rs ) );
		g->shr( EAX, function );
		g->and( EAX, bitmask );
		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult INS( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( rt == 0 )
		return GenerationResult::Success;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// 8909260:	7c4df504 	ins	t5,v0,0x14,0xb
		// GPR[rt] = GPR[rt]31..msb+1 || GPR[rs]msb-lsb..0 || GPR[rt]lsb-1..0

		// size in rd, pos in function
		// rs is bitmask, rt is value
		byte rs = ( byte )( ( code >> 21 ) & 0x1F );

		int size = rd - function + 1;

		// This is kind of a trick I thought of ^_^
		uint64 bittemp = 0x00000000FFFFFFFF;
		bittemp <<= size;
		bittemp >>= 32;
		int rsmask = ( int )bittemp;
		bittemp <<= function;
		int rtmask = ( int )bittemp;
		rtmask = ~rtmask;

		g->mov( EAX, MREG( CTX, rs ) );
		g->and( EAX, rsmask );
		g->shl( EAX, function );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, rtmask );
		g->or( EAX, EBX );
		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		g->movsx( EAX, AL );
		g->mov( MREG( CTX, rd ), EAX );
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
		g->mov( EAX, MREG( CTX, rt ) );
		g->movsx( EAX, AX );
		g->mov( MREG( CTX, rd ), EAX );
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
		// rd = # of leading 0's in rs
		/*for( int n = 31; n >= 0; n-- )
		{
			if( rs >> n == 1 )
				return 31 - n;
		}
		return 32;*/

		// USE BSR?
		
		Label* loop = g->DefineLabel();
		Label* found = g->DefineLabel();
		Label* done = g->DefineLabel();

		g->mov( EBX, MREG( CTX, rs ) );
		g->mov( CL, 32 );
		g->MarkLabel( loop );
		g->sub( CL, 1 );				// n--
		g->mov( EAX, EBX );
		g->shr( EAX, CL );
		g->cmp( EAX, 1 );				// if( rs >> n == 1 )...
		g->je( found );

		g->cmp( CL, 0 );
		g->jge( loop );					// loop while n >= 0
		g->mov( MREG( CTX, rd ), 32 );	// return 32
		g->jmp( done );

		g->MarkLabel( found );
		g->mov( EAX, 31 );
		g->movzx( ECX, CL );
		g->sub( EAX, ECX );
		g->mov( MREG( CTX, rd ), EAX );	// return 31 - n
		
		g->MarkLabel( done );
	}
	return GenerationResult::Success;
}

GenerationResult CLO( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// rd = # of leading 1's in rs
		/*for( int n = 31; n >= 0; n-- )
		{
			if( ( rs >> n ) & 0x1 == 0 )
				return 31 - n;
		}
		return 32;*/
		
		Label* loop = g->DefineLabel();
		Label* found = g->DefineLabel();
		Label* done = g->DefineLabel();

		g->mov( EBX, MREG( CTX, rs ) );
		g->mov( CL, 32 );
		g->MarkLabel( loop );
		g->sub( CL, 1 );				// n--
		g->mov( EAX, EBX );
		g->shr( EAX, CL );
		g->and( EAX, 1 );				// & 0x1 so that we ignore the other 1's
		g->cmp( EAX, 0 );				// if( ( rs >> n ) & 0x1 == 0 )...
		g->je( found );

		g->cmp( CL, 0 );
		g->jge( loop );					// loop while n >= 0
		g->mov( MREG( CTX, rd ), 32 );	// return 32
		g->jmp( done );

		g->MarkLabel( found );
		g->mov( EAX, 31 );
		g->movzx( ECX, CL );
		g->sub( EAX, ECX );
		g->mov( MREG( CTX, rd ), EAX );	// return 31 - n
		
		g->MarkLabel( done );
	}
	return GenerationResult::Success;
}

GenerationResult WSBH( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// rd = rt swapped inside each halfword
		g->mov( EAX, MREG( CTX, rt ) );

		// There may be an instruction for this, but I don't have my manuals handy

		// a = reg(rt)
		// b = ( ( a & 0x00FF00FF ) << 8 )
		// b |= ( ( a & 0xFF00FF00 ) >> 8 )

		// b = ( ( a & 0x00FF00FF ) << 8 )
		g->mov( EBX, EAX );
		g->and( EBX, 0x00FF00FF );
		g->shl( EBX, 8 );

		// b |= ( ( a & 0xFF00FF00 ) >> 8 )
		g->and( EAX, 0xFF00FF00 );
		g->shr( EAX, 8 );
		g->or( EAX, EBX );

		g->mov( MREG( CTX, rd ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult WSBW( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( CTX, rt ) );
		g->bswap( EAX );
		g->mov( MREG( CTX, rd ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult BITREV( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	// rd = bitrev( rt )

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// Taken from http://graphics.stanford.edu/~seander/bithacks.html#ReverseByteWith32Bits

		g->int3();

		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( ECX, EAX );

		// swap consecutive pairs
		// v = ((v >> 2) & 0x33333333) | ((v & 0x33333333) << 2);
		g->lea( EDX, g->dword_ptr[ ECX * 4 ] );
		//lea         edx,[ecx*4] 
		g->sar( EAX, 2 );
		g->xor( EAX, EDX );
		g->add( ECX, ECX );
		g->and( EAX, 0x33333333 );
		g->add( ECX, ECX );
		g->xor( EAX, ECX ); 
		// swap nibbles ... 
		// v = ((v >> 4) & 0x0F0F0F0F) | ((v & 0x0F0F0F0F) << 4);
		g->mov( ECX, EAX );
		g->mov( EDX, EAX );
		g->shl( EDX, 4 );
		g->sar( ECX, 4 );
		g->xor( ECX, EDX );
		g->shl( EAX, 4 );
		g->and( ECX, 0x0F0F0F0F );
		g->xor( ECX, EAX ); 
		// swap bytes
		// v = ((v >> 8) & 0x00FF00FF) | ((v & 0x00FF00FF) << 8);
		g->mov( EDX, ECX );
		g->sar( EDX, 8 );
		g->mov( EAX, ECX );
		g->shl( EAX, 8 );
		g->xor( EDX, EAX );
		g->and( EDX, 0x0FF00FF );
		g->shl( ECX, 8 );
		g->xor( EDX, ECX ); 
		// swap 2-byte long pairs
		// v = ( v >> 16             ) | ( v               << 16);
		g->mov( EAX, EDX );
		g->sar( EAX, 0x10 );
		g->shl( EDX, 0x10 );
		g->or( EAX, EDX );

		g->int3();

		g->mov( MREG( CTX, rd ), EAX );
	}
	return GenerationResult::Success;
}
