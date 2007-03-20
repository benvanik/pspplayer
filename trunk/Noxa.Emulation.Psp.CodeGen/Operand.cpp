// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Operand.h"
#include <string.h>
#include <stdio.h>

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;

const Specifier Specifier::SpecifierSet[] =
{
	{TYPE_UNKNOWN,	""},

	{TYPE_NEAR,		"NEAR"},
	{TYPE_SHORT,	"SHORT"},
//	{TYPE_FAR,		"FAR"},

	{TYPE_BYTE,		"BYTE"},
	{TYPE_WORD,		"WORD"},
	{TYPE_DWORD,	"DWORD"},
//	{TYPE_TWORD,	"TWORD"},
	{TYPE_QWORD,	"QWORD"},
	{TYPE_MMWORD,	"MMWORD"},
	{TYPE_XMMWORD,	"XMMWORD"},
	{TYPE_XWORD,	"XWORD"},
	{TYPE_OWORD,	"OWORD"},

	{TYPE_PTR,		"PTR"}
};

const int Specifier::Count = sizeof( SpecifierSet ) / sizeof( Specifier );

Specifier::Type Specifier::Scan( const char* string )
{
	if( string == 0 )
		return TYPE_UNKNOWN;

	for( int n = 0; n < Count; n++ )
	{
		if( _stricmp( string, SpecifierSet[ n ].Notation ) == 0 )
			return SpecifierSet[ n ].type;	
	}

	return TYPE_UNKNOWN;
}

bool Operand::operator==(Operand &op)
{
	return type == op.type &&
	       BaseReg == op.BaseReg &&
		   IndexReg == op.IndexReg &&
		   Scale == op.Scale &&
		   Displacement == op.Displacement;
}

bool Operand::operator!=(Operand &op)
{
	return type != op.type ||
	       BaseReg != op.BaseReg ||
		   IndexReg != op.IndexReg ||
		   Scale != op.Scale ||
		   Displacement != op.Displacement;
}

bool Operand::IsSubtypeOf(Type type, Type baseType)
{
	return (type & baseType) == type;
}

bool Operand::IsSubtypeOf(Type baseType) const
{
	return IsSubtypeOf(type, baseType);
}

const char* Operand::string() const
{
	static char string[256];

	if(IsVoid(type))
	{
		return 0;
	}
	else if(IsImm(type))
	{
		if(Reference)
		{
			_snprintf_s( string, 255, 255, "ref %d", Reference->Offset );
			return string;
		}
		else
		{
			if(Value <= 127 && Value >= -128)
			{
				_snprintf_s(string, 255, 255, "0x%0.2X", Value & 0xFF);
			}
			else if(Value <= 32767 && Value -32768)
			{
				_snprintf_s(string, 255, 255, "0x%0.4X", Value & 0xFFFF);
			}
			else
			{
				_snprintf_s(string, 255, 255, "0x%0.8X", Value);
			}
		}
	}
	else if(IsReg(type))
	{
		return RegName();
	}
	else if(IsMem(type))
	{
		switch(type)
		{
		case OPERAND_MEM8:
			_snprintf_s(string, 255, 255, "byte ptr [");
			break;
		case OPERAND_MEM16:
			_snprintf_s(string, 255, 255, "word ptr [");
			break;
		case OPERAND_MEM32:
			_snprintf_s(string, 255, 255, "dword ptr [");
			break;
		case OPERAND_MEM64:
			_snprintf_s(string, 255, 255, "qword ptr [");
			break;
		case OPERAND_MEM128:
			_snprintf_s(string, 255, 255, "xmmword ptr [");
			break;
		case OPERAND_MEM:
		default:
			_snprintf_s(string, 255, 255, "byte ptr [");
		}

		if(BaseReg != CodeGen::REG_UNKNOWN)
		{
			_snprintf_s(string, 255, 255, "%s%s", string, RegName());

			if(IndexReg != CodeGen::REG_UNKNOWN)
			{
				_snprintf_s(string, 255, 255, "%s+", string);
			}
		}

		if(IndexReg != CodeGen::REG_UNKNOWN)
		{
			_snprintf_s(string, 255, 255, "%s%s", string, IndexName());
		}

		switch(Scale)
		{
		case 0:
		case 1:
			break;
		case 2:
			_snprintf_s(string, 255, 255, "%s*2", string);
			break;
		case 4:
			_snprintf_s(string, 255, 255, "%s*4", string);
			break;
		case 8:
			_snprintf_s(string, 255, 255, "%s*8", string);
			break;
		default:
			assert( false );
			break;
		}

		if(Displacement)
		{
			if(BaseReg != CodeGen::REG_UNKNOWN ||
			   IndexReg != CodeGen::REG_UNKNOWN)
			{
				_snprintf_s(string, 255, 255, "%s+", string);
			}

			if(Reference)
			{
				_snprintf_s(string, 255, 255, "%s%s", string, Reference);
			}
			else
			{
				if(Displacement <= 32767 && Displacement >= -32768)
				{
					_snprintf_s(string, 255, 255, "%s%d", string, Displacement);
				}
				else
				{					
					_snprintf_s(string, 255, 255, "%s0x%0.8X", string, Displacement);
				}
			}
		}

		_snprintf_s(string, 255, 255, "%s]", string);
	}
	else
	{
		assert( false );
	}

	_strlwr_s( string, 255 );
	return string;
}

bool Operand::IsVoid(Type type)
{
	return type == OPERAND_VOID;
}

bool Operand::IsImm(Type type)
{
	return (type & OPERAND_IMM) == type;
}

bool Operand::IsReg(Type type)
{
	return (type & OPERAND_REG) == type;
}

bool Operand::IsMem(Type type)
{
	return (type & OPERAND_MEM) == type;
}

bool Operand::IsRM(Type type)
{
	return (type & OPERAND_R_M) == type;
}

bool Operand::IsVoid(const Operand &operand)
{
	return IsVoid(operand.type);
}

bool Operand::IsImm(const Operand &operand)
{
	return IsImm(operand.type);
}

bool Operand::IsReg(const Operand &operand)
{
	return IsReg(operand.type);
}

bool Operand::IsMem(const Operand &operand)
{
	return IsMem(operand.type);
}

bool Operand::IsRM(const Operand &operand)
{
	return IsRM(operand.type);
}

const Operand::Register Operand::RegisterSet[] =
{
	{OPERAND_VOID,		""},

	{OPERAND_AL,		"al", CodeGen::AL},
	{OPERAND_CL,		"cl", CodeGen::CL},
	{OPERAND_REG8,		"dl", CodeGen::DL},
	{OPERAND_REG8,		"bl", CodeGen::BL},
	{OPERAND_REG8,		"ah", CodeGen::AH},
	{OPERAND_REG8,		"ch", CodeGen::CH},
	{OPERAND_REG8,		"dh", CodeGen::DH},
	{OPERAND_REG8,		"bh", CodeGen::BH},

	{OPERAND_AX,		"ax", CodeGen::AX},
	{OPERAND_CX,		"cx", CodeGen::CX},
	{OPERAND_DX,		"dx", CodeGen::DX},
	{OPERAND_REG16,		"bx", CodeGen::BX},
	{OPERAND_REG16,		"sp", CodeGen::SP},
	{OPERAND_REG16,		"bp", CodeGen::BP},
	{OPERAND_REG16,		"si", CodeGen::SI},
	{OPERAND_REG16,		"di", CodeGen::DI},

	{OPERAND_EAX,		"eax", CodeGen::EAX},
	{OPERAND_ECX,		"ecx", CodeGen::ECX},
	{OPERAND_REG32,		"edx", CodeGen::EDX},
	{OPERAND_REG32,		"ebx", CodeGen::EBX},
	{OPERAND_REG32,		"esp", CodeGen::ESP},
	{OPERAND_REG32,		"ebp", CodeGen::EBP},
	{OPERAND_REG32,		"esi", CodeGen::ESI},
	{OPERAND_REG32,		"edi", CodeGen::EDI},

	{OPERAND_ST0,		"st",  CodeGen::ST0},
	{OPERAND_ST0,		"st0", CodeGen::ST0},
	{OPERAND_FPUREG,	"st1", CodeGen::ST1},
	{OPERAND_FPUREG,	"st2", CodeGen::ST2},
	{OPERAND_FPUREG,	"st3", CodeGen::ST3},
	{OPERAND_FPUREG,	"st4", CodeGen::ST4},
	{OPERAND_FPUREG,	"st5", CodeGen::ST5},
	{OPERAND_FPUREG,	"st6", CodeGen::ST6},
	{OPERAND_FPUREG,	"st7", CodeGen::ST7},

	{OPERAND_MMREG,		"mm0", CodeGen::MM0},
	{OPERAND_MMREG,		"mm1", CodeGen::MM1},
	{OPERAND_MMREG,		"mm2", CodeGen::MM2},
	{OPERAND_MMREG,		"mm3", CodeGen::MM3},
	{OPERAND_MMREG,		"mm4", CodeGen::MM4},
	{OPERAND_MMREG,		"mm5", CodeGen::MM5},
	{OPERAND_MMREG,		"mm6", CodeGen::MM6},
	{OPERAND_MMREG,		"mm7", CodeGen::MM7},

	{OPERAND_XMMREG,	"xmm0", CodeGen::XMM0},
	{OPERAND_XMMREG,	"xmm1", CodeGen::XMM1},
	{OPERAND_XMMREG,	"xmm2", CodeGen::XMM2},
	{OPERAND_XMMREG,	"xmm3", CodeGen::XMM3},
	{OPERAND_XMMREG,	"xmm4", CodeGen::XMM4},
	{OPERAND_XMMREG,	"xmm5", CodeGen::XMM5},
	{OPERAND_XMMREG,	"xmm6", CodeGen::XMM6},
	{OPERAND_XMMREG,	"xmm7", CodeGen::XMM7}
};

const char* Operand::RegName() const
{
	for(int i = 0; i < sizeof( RegisterSet ) / sizeof( Operand::Register ); i++)
	{
		if(Reg == RegisterSet[i].Reg)
		{
			if(IsSubtypeOf(OPERAND_MEM) && Operand::IsSubtypeOf(RegisterSet[i].type, OPERAND_REG32) ||
			   Operand::IsSubtypeOf(RegisterSet[i].type, type) && Reg == RegisterSet[i].Reg)
			{
				return RegisterSet[i].Notation;
			}
		}
	}

	assert( false );
	return 0;
}

const char* Operand::IndexName() const
{
	for(int i = 0; i < sizeof( RegisterSet ) / sizeof( Operand::Register ); i++)
	{
		if( IndexReg == RegisterSet[i].Reg &&
			Operand::IsSubtypeOf(RegisterSet[i].type, OPERAND_REG32))
		{
			return RegisterSet[i].Notation;
		}
	}

	assert( false );
	return 0;
}

const Operand::Register Operand::SyntaxSet[] =
{
	{OPERAND_VOID,		""},

	{OPERAND_ONE,		"1"},
	{OPERAND_IMM,		"imm"},
	{OPERAND_IMM8,		"imm8"},
	{OPERAND_IMM16,		"imm16"},
	{OPERAND_IMM32,		"imm32"},
//	{OPERAND_IMM64,		"imm64"},

	{OPERAND_AL,		"AL"},
	{OPERAND_AX,		"AX"},
	{OPERAND_EAX,		"EAX"},
	{OPERAND_RAX,		"RAX"},
	{OPERAND_DX,		"DX"},
	{OPERAND_CL,		"CL"},
	{OPERAND_CX,		"CX"},
	{OPERAND_ECX,		"ECX"},
	{OPERAND_ST0,		"ST0"},

	{OPERAND_REG8,		"reg8"},
	{OPERAND_REG16,		"reg16"},
	{OPERAND_REG32,		"reg32"},
	{OPERAND_REG64,		"reg64"},
	{OPERAND_FPUREG,	"fpureg"},
	{OPERAND_MMREG,		"mmreg"},
	{OPERAND_XMMREG,	"xmmreg"},

	{OPERAND_MEM,		"mem"},
	{OPERAND_MEM8,		"mem8"},
	{OPERAND_MEM16,		"mem16"},
	{OPERAND_MEM32,		"mem32"},
	{OPERAND_MEM64,		"mem64"},
	{OPERAND_MEM128,	"mem128"},

	{OPERAND_R_M8,		"r/m8"},
	{OPERAND_R_M16,		"r/m16"},
	{OPERAND_R_M32,		"r/m32"},
	{OPERAND_R_M64,		"r/m64"},
	{OPERAND_R_M128,	"r/m128"},

	{OPERAND_XMM32,		"xmm32"},
	{OPERAND_XMM64,		"xmm64"},
	{OPERAND_MM64,		"mm64"}
};

Operand::Type Operand::Scan( const char* string )
{
	if( string == NULL )
		return OPERAND_UNKNOWN;

	for( int n = 0; n < sizeof( SyntaxSet ) / sizeof( Operand::Register ); n++ )
	{
		if( _stricmp( string, SyntaxSet[ n ].Notation ) == 0 )
		{
			return SyntaxSet[ n ].type;
		}
	}

	return OPERAND_UNKNOWN;
}
