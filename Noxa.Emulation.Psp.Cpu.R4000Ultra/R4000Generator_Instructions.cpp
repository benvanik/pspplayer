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

#include "Loader.hpp"
#include "CodeGenerator.hpp"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

GenerateInstructionR R4000Generator::TableR[ 64 ];
GenerateInstructionI R4000Generator::TableI[ 64 ];
GenerateInstructionJ R4000Generator::TableJ[ 64 ];
GenerateInstructionR R4000Generator::TableAllegrex[ 64 ];
GenerateInstructionSpecial3 R4000Generator::TableSpecial3[ 64 ];

GenerationResult UnknownR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	return GenerationResult::Invalid;
}

GenerationResult UnknownI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	return GenerationResult::Invalid;
}

GenerationResult UnknownJ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	return GenerationResult::Invalid;
}

GenerationResult UnknownCop0( R4000GenContext^ context, int pass, int address, uint code, byte function )
{
	return GenerationResult::Invalid;
}

GenerationResult UnknownSpecial3( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	return GenerationResult::Invalid;
}

GenerationResult UnknownFpu( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	return GenerationResult::Invalid;
}

#include "R4000Generator_Instructions.h"

void R4000Generator::Setup()
{
	for( int n = 0; n < 64; n++ )
	{
		TableR[ n ] = UnknownR;
		TableI[ n ] = UnknownI;
		TableJ[ n ] = UnknownJ;
		TableAllegrex[ n ] = UnknownR;
		TableSpecial3[ n ] = UnknownSpecial3;
	}

	TableR[ 0 ] = SLL;
	TableR[ 2 ] = SRL;
	TableR[ 3 ] = SRA;
	TableR[ 4 ] = SLLV;
	TableR[ 6 ] = SRLV;
	TableR[ 7 ] = SRAV;
	TableR[ 8 ] = JR;
	TableR[ 9 ] = JALR;
	TableR[ 10 ] = MOVZ;
	TableR[ 11 ] = MOVN;
	TableR[ 12 ] = SYSCALL;
	TableR[ 13 ] = BREAK;
	TableR[ 15 ] = SYNC;
	TableR[ 16 ] = MFHI;
	TableR[ 17 ] = MTHI;
	TableR[ 18 ] = MFLO;
	TableR[ 19 ] = MTLO;
	TableR[ 22 ] = CLZ;
	TableR[ 23 ] = CLO;
	TableR[ 24 ] = MULT;
	TableR[ 25 ] = MULTU;
	TableR[ 26 ] = DIV;
	TableR[ 27 ] = DIVU;
	TableR[ 32 ] = ADD;
	TableR[ 33 ] = ADDU;
	TableR[ 34 ] = SUB;
	TableR[ 35 ] = SUBU;
	TableR[ 36 ] = AND;
	TableR[ 37 ] = OR;
	TableR[ 38 ] = XOR;
	TableR[ 39 ] = NOR;
	TableR[ 42 ] = SLT;
	TableR[ 43 ] = SLTU;
	TableR[ 44 ] = MAX;
	TableR[ 45 ] = MIN;

	TableI[ 2 ] = J;
	TableI[ 3 ] = JAL;
	TableI[ 4 ] = BEQ;
	TableI[ 5 ] = BNE;
	TableI[ 6 ] = BLEZ;
	TableI[ 7 ] = BGTZ;
	TableI[ 8 ] = ADDI;
	TableI[ 9 ] = ADDIU;
	TableI[ 10 ] = SLTI;
	TableI[ 11 ] = SLTIU;
	TableI[ 12 ] = ANDI;
	TableI[ 13 ] = ORI;
	TableI[ 14 ] = XORI;
	TableI[ 15 ] = LUI;
	TableI[ 17 ] = COP1;
	TableI[ 18 ] = COP2;
	TableI[ 20 ] = BEQL;
	TableI[ 21 ] = BNEL;
	TableI[ 22 ] = BLEZL;
	TableI[ 23 ] = BGTZL;
	TableI[ 32 ] = LB;
	TableI[ 33 ] = LH;
	TableI[ 34 ] = LWL;
	TableI[ 35 ] = LW;
	TableI[ 36 ] = LBU;
	TableI[ 37 ] = LHU;
	TableI[ 38 ] = LWR;
	TableI[ 40 ] = SB;
	TableI[ 41 ] = SH;
	TableI[ 42 ] = SWL;
	TableI[ 43 ] = SW;
	TableI[ 46 ] = SWR;
	TableI[ 47 ] = CACHE;
	TableI[ 48 ] = LL;
	TableI[ 49 ] = LWCz;
	TableI[ 50 ] = LWCz;
	TableI[ 56 ] = SC;
	TableI[ 57 ] = SWCz;
	TableI[ 58 ] = SWCz;

	TableJ[ 0 ] = BLTZ;
	TableJ[ 1 ] = BGEZ;
	TableJ[ 2 ] = BLTZL;
	TableJ[ 3 ] = BGEZL;
	TableJ[ 16 ] = BLTZAL;
	TableJ[ 17 ] = BGEZAL;
	TableJ[ 18 ] = BLTZALL;
	TableJ[ 19 ] = BGEZALL;

	TableAllegrex[ 0 ] = HALT;
	TableAllegrex[ 0x24 ] = MFIC;
	TableAllegrex[ 0x26 ] = MTIC;

	TableSpecial3[ 0 ] = EXT;
	TableSpecial3[ 4 ] = INS;
	TableSpecial3[ 16 ] = SEB;
	TableSpecial3[ 24 ] = SEH;
}