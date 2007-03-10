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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			g->shl( EAX, shamt );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			g->shr( EAX, shamt );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rt ) );
		if( ( shamt >= 0 ) && ( shamt < 32 ) )
			//g->sar( EAX, shamt );
			g->shr( EAX, shamt );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		//g->sar( EAX, CL );
		g->shr( EAX, CL );
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
		char label[20];
		sprintf( label, "l%X", address - 4 );
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rt ) );
		g->cmp( EAX, 0 );
		g->jne( label );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		sprintf( label, "l%X", address - 4 );
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rt ) );
		g->cmp( EAX, 0 );
		g->je( label );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( MREG( CTX, rd ), EAX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
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
		LOADCTXBASE( ECX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EDX, 0 );
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
		LOADCTXBASE( ECX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EDX, 0 );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->add( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->add( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->sub( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->sub( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->or( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->xor( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->mov( EBX, MREG( CTX, rt ) );
		g->or( EAX, EBX );
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
		LOADCTXBASE( EDX );
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		g->setl( BL );
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
		LOADCTXBASE( EDX );
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, MREG( CTX, rt ) );
		//g->setb( BL );
		g->setl( BL );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, SE( imm ) );
		g->setl( BL );
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
		LOADCTXBASE( EDX );
		g->xor( EBX, EBX );
		g->mov( EAX, MREG( CTX, rs ) );
		g->cmp( EAX, SE( imm ) );
		//g->setb( BL );
		g->setl( BL );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		long long bittemp = 0x00000000FFFFFFFF;
		bittemp <<= rd + 1;
		bittemp >>= 32;
		int bitmask = ( int )bittemp;

		// value =>> pos
		// value &= bitfield
		LOADCTXBASE( EDX );
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
		long long bittemp = 0x00000000FFFFFFFF;
		bittemp <<= size;
		bittemp >>= 32;
		int rsmask = ( int )bittemp;
		bittemp <<= function;
		int rtmask = ( int )bittemp;
		rtmask = ~rtmask;

		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
		LOADCTXBASE( EDX );
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
