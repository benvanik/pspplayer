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

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

GenerateInstructionR R4000Generator::TableR[ 64 ];
GenerateInstructionI R4000Generator::TableI[ 64 ];
GenerateInstructionJ R4000Generator::TableJ[ 64 ];
GenerateInstructionR R4000Generator::TableAllegrex[ 64 ];
GenerateInstructionSpecial3 R4000Generator::TableSpecial3[ 64 ];
GenerateInstructionI R4000Generator::TableCopA[ 64 ];
GenerateInstructionI R4000Generator::TableCopB[ 64 ];
GenerateInstructionFpu R4000Generator::TableFpu[ 64 ];

const char* R4000Generator::TableR_n[ 64 ];
const char* R4000Generator::TableI_n[ 64 ];
const char* R4000Generator::TableJ_n[ 64 ];
const char* R4000Generator::TableAllegrex_n[ 64 ];
const char* R4000Generator::TableSpecial3_n[ 64 ];
const char* R4000Generator::TableCopA_n[ 64 ];
const char* R4000Generator::TableCopB_n[ 64 ];
const char* R4000Generator::TableFpu_n[ 64 ];

GenerationResult UnknownR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	Debug::Assert( false );
	return GenerationResult::Invalid;
}

GenerationResult UnknownI( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	Debug::Assert( false );
	return GenerationResult::Invalid;
}

GenerationResult UnknownJ( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm )
{
	Debug::Assert( false );
	return GenerationResult::Invalid;
}

GenerationResult UnknownCop0( R4000GenContext^ context, int pass, int address, uint code, byte function )
{
	Debug::Assert( false );
	return GenerationResult::Invalid;
}

GenerationResult UnknownSpecial3( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
{
	Debug::Assert( false );
	return GenerationResult::Invalid;
}

GenerationResult UnknownFpu( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	Debug::Assert( false );
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
		TableCopA[ n ] = UnknownI;
		TableCopB[ n ] = UnknownI;
		TableFpu[ n ] = UnknownFpu;

		TableR_n[ n ] = NULL;
		TableI_n[ n ] = NULL;
		TableJ_n[ n ] = NULL;
		TableAllegrex_n[ n ] = NULL;
		TableSpecial3_n[ n ] = NULL;
		TableCopA_n[ n ] = NULL;
		TableCopB_n[ n ] = NULL;
		TableFpu_n[ n ] = NULL;
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
	TableR[ 28 ] = MADD;
	TableR[ 29 ] = MADDU;
	TableR[ 30 ] = MSUB;
	TableR[ 31 ] = MSUBU;
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

	TableR_n[ 0 ] = "SLL";
	TableR_n[ 2 ] = "SRL";
	TableR_n[ 3 ] = "SRA";
	TableR_n[ 4 ] = "SLLV";
	TableR_n[ 6 ] = "SRLV";
	TableR_n[ 7 ] = "SRAV";
	TableR_n[ 8 ] = "JR";
	TableR_n[ 9 ] = "JALR";
	TableR_n[ 10 ] = "MOVZ";
	TableR_n[ 11 ] = "MOVN";
	TableR_n[ 12 ] = "SYSCALL";
	TableR_n[ 13 ] = "BREAK";
	TableR_n[ 15 ] = "SYNC";
	TableR_n[ 16 ] = "MFHI";
	TableR_n[ 17 ] = "MTHI";
	TableR_n[ 18 ] = "MFLO";
	TableR_n[ 19 ] = "MTLO";
	TableR_n[ 22 ] = "CLZ";
	TableR_n[ 23 ] = "CLO";
	TableR_n[ 24 ] = "MULT";
	TableR_n[ 25 ] = "MULTU";
	TableR_n[ 26 ] = "DIV";
	TableR_n[ 27 ] = "DIVU";
	TableR_n[ 28 ] = "MADD";
	TableR_n[ 29 ] = "MADDU";
	TableR_n[ 30 ] = "MSUB";
	TableR_n[ 31 ] = "MSUBU";
	TableR_n[ 32 ] = "ADD";
	TableR_n[ 33 ] = "ADDU";
	TableR_n[ 34 ] = "SUB";
	TableR_n[ 35 ] = "SUBU";
	TableR_n[ 36 ] = "AND";
	TableR_n[ 37 ] = "OR";
	TableR_n[ 38 ] = "XOR";
	TableR_n[ 39 ] = "NOR";
	TableR_n[ 42 ] = "SLT";
	TableR_n[ 43 ] = "SLTU";
	TableR_n[ 44 ] = "MAX";
	TableR_n[ 45 ] = "MIN";

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

	TableI_n[ 2 ] = "J";
	TableI_n[ 3 ] = "JAL";
	TableI_n[ 4 ] = "BEQ";
	TableI_n[ 5 ] = "BNE";
	TableI_n[ 6 ] = "BLEZ";
	TableI_n[ 7 ] = "BGTZ";
	TableI_n[ 8 ] = "ADDI";
	TableI_n[ 9 ] = "ADDIU";
	TableI_n[ 10 ] = "SLTI";
	TableI_n[ 11 ] = "SLTIU";
	TableI_n[ 12 ] = "ANDI";
	TableI_n[ 13 ] = "ORI";
	TableI_n[ 14 ] = "XORI";
	TableI_n[ 15 ] = "LUI";
	TableI_n[ 17 ] = "COP1";
	TableI_n[ 18 ] = "COP2";
	TableI_n[ 20 ] = "BEQL";
	TableI_n[ 21 ] = "BNEL";
	TableI_n[ 22 ] = "BLEZL";
	TableI_n[ 23 ] = "BGTZL";
	TableI_n[ 32 ] = "LB";
	TableI_n[ 33 ] = "LH";
	TableI_n[ 34 ] = "LWL";
	TableI_n[ 35 ] = "LW";
	TableI_n[ 36 ] = "LBU";
	TableI_n[ 37 ] = "LHU";
	TableI_n[ 38 ] = "LWR";
	TableI_n[ 40 ] = "SB";
	TableI_n[ 41 ] = "SH";
	TableI_n[ 42 ] = "SWL";
	TableI_n[ 43 ] = "SW";
	TableI_n[ 46 ] = "SWR";
	TableI_n[ 47 ] = "CACHE";
	TableI_n[ 48 ] = "LL";
	TableI_n[ 49 ] = "LWCz";
	TableI_n[ 50 ] = "LWCz";
	TableI_n[ 56 ] = "SC";
	TableI_n[ 57 ] = "SWCz";
	TableI_n[ 58 ] = "SWCz";

	TableJ[ 0 ] = BLTZ;
	TableJ[ 1 ] = BGEZ;
	TableJ[ 2 ] = BLTZL;
	TableJ[ 3 ] = BGEZL;
	TableJ[ 16 ] = BLTZAL;
	TableJ[ 17 ] = BGEZAL;
	TableJ[ 18 ] = BLTZALL;
	TableJ[ 19 ] = BGEZALL;

	TableJ_n[ 0 ] = "BLTZ";
	TableJ_n[ 1 ] = "BGEZ";
	TableJ_n[ 2 ] = "BLTZL";
	TableJ_n[ 3 ] = "BGEZL";
	TableJ_n[ 16 ] = "BLTZAL";
	TableJ_n[ 17 ] = "BGEZAL";
	TableJ_n[ 18 ] = "BLTZALL";
	TableJ_n[ 19 ] = "BGEZALL";

	TableAllegrex[ 0 ] = HALT;
	TableAllegrex[ 0x24 ] = MFIC;
	TableAllegrex[ 0x26 ] = MTIC;

	TableAllegrex_n[ 0 ] = "HALT";
	TableAllegrex_n[ 0x24 ] = "MFIC";
	TableAllegrex_n[ 0x26 ] = "MTIC";

	TableSpecial3[ 0 ] = EXT;
	TableSpecial3[ 2 ] = WSBH;
	TableSpecial3[ 3 ] = WSBW;
	TableSpecial3[ 4 ] = INS;
	TableSpecial3[ 16 ] = SEB;
	TableSpecial3[ 24 ] = SEH;

	TableSpecial3_n[ 0 ] = "EXT";
	TableSpecial3_n[ 2 ] = "WSBH";
	TableSpecial3_n[ 3 ] = "WSBW";
	TableSpecial3_n[ 4 ] = "INS";
	TableSpecial3_n[ 16 ] = "SEB";
	TableSpecial3_n[ 24 ] = "SEH";

	TableCopA[ 0 ] = MFCz;
	TableCopA[ 4 ] = MTCz;
	TableCopA[ 2 ] = CFCz;
	TableCopA[ 6 ] = CTCz;

	TableCopA_n[ 0 ] = "MFCz";
	TableCopA_n[ 4 ] = "MTCz";
	TableCopA_n[ 2 ] = "CFCz";
	TableCopA_n[ 6 ] = "CTCz";

	TableCopB[ 0 ] = BCzF;
	TableCopB[ 1 ] = BCzT;
	TableCopB[ 2 ] = BCzFL;
	TableCopB[ 3 ] = BCzTL;

	TableCopB_n[ 0 ] = "BCzF";
	TableCopB_n[ 1 ] = "BCzT";
	TableCopB_n[ 2 ] = "BCzFL";
	TableCopB_n[ 3 ] = "BCzTL";

	TableFpu[ 0 ] = FADD;
	TableFpu[ 1 ] = FSUB;
	TableFpu[ 2 ] = FMUL;
	TableFpu[ 3 ] = FDIV;
	TableFpu[ 4 ] = FSQRT;
	TableFpu[ 5 ] = FABS;
	TableFpu[ 6 ] = FMOV;
	TableFpu[ 7 ] = FNEG;
	TableFpu[ 8 ] = ROUNDL;
	TableFpu[ 9 ] = TRUNCL;
	TableFpu[ 10 ] = CEILL;
	TableFpu[ 11 ] = FLOORL;
	TableFpu[ 12 ] = ROUNDW;
	TableFpu[ 13 ] = TRUNCW;
	TableFpu[ 14 ] = CEILW;
	TableFpu[ 15 ] = FLOORW;
	TableFpu[ 32 ] = CVTS;
	TableFpu[ 33 ] = CVTD;
	TableFpu[ 36 ] = CVTW;
	TableFpu[ 37 ] = CVTL;
	for( int n = 48; n <= 63; n++ )
		TableFpu[ n ] = FCOMPARE;

	TableFpu_n[ 0 ] = "FADD";
	TableFpu_n[ 1 ] = "FSUB";
	TableFpu_n[ 2 ] = "FMUL";
	TableFpu_n[ 3 ] = "FDIV";
	TableFpu_n[ 4 ] = "FSQRT";
	TableFpu_n[ 5 ] = "FABS";
	TableFpu_n[ 6 ] = "FMOV";
	TableFpu_n[ 7 ] = "FNEG";
	TableFpu_n[ 8 ] = "ROUNDL";
	TableFpu_n[ 9 ] = "TRUNCL";
	TableFpu_n[ 10 ] = "CEILL";
	TableFpu_n[ 11 ] = "FLOORL";
	TableFpu_n[ 12 ] = "ROUNDW";
	TableFpu_n[ 13 ] = "TRUNCW";
	TableFpu_n[ 14 ] = "CEILW";
	TableFpu_n[ 15 ] = "FLOORW";
	TableFpu_n[ 32 ] = "CVTS";
	TableFpu_n[ 33 ] = "CVTD";
	TableFpu_n[ 36 ] = "CVTW";
	TableFpu_n[ 37 ] = "CVTL";
	for( int n = 48; n <= 63; n++ )
		TableFpu_n[ n ] = "FCOMPARE";
}