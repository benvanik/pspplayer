// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Label.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace CodeGen {

				struct Specifier
				{
					enum Type
					{
						TYPE_UNKNOWN = 0,

						TYPE_NEAR,
						TYPE_SHORT = TYPE_NEAR,
					//	TYPE_FAR,
						
						TYPE_BYTE,
						TYPE_WORD,
						TYPE_DWORD,
					//	TYPE_TWORD,   // 80-bit long double not supported
						TYPE_QWORD,
						TYPE_MMWORD = TYPE_QWORD,
						TYPE_XMMWORD,
						TYPE_XWORD = TYPE_XMMWORD,
						TYPE_OWORD = TYPE_XMMWORD,

						TYPE_PTR
					};

					Type				type;
					union
					{
						const char*		Reference;
						const char*		Notation;
					};

					static Specifier::Type Scan( const char* string );

					static const Specifier SpecifierSet[];
					static const int Count;

				};

				enum Reg
				{
					REG_UNKNOWN = -1,

					R0 = 0, AL = 0, AX = 0, EAX = 0, RAX = 0, ST0 = 0, MM0 = 0, XMM0 = 0,
					R1 = 1, CL = 1, CX = 1, ECX = 1, RCX = 1, ST1 = 1, MM1 = 1, XMM1 = 1,
					R2 = 2, DL = 2, DX = 2, EDX = 2, RDX = 2, ST2 = 2, MM2 = 2, XMM2 = 2,
					R3 = 3, BL = 3, BX = 3, EBX = 3, RBX = 3, ST3 = 3, MM3 = 3, XMM3 = 3,
					R4 = 4, AH = 4, SP = 4, ESP = 4, RSP = 4, ST4 = 4, MM4 = 4, XMM4 = 4,
					R5 = 5, CH = 5, BP = 5, EBP = 5, RBP = 5, ST5 = 5, MM5 = 5, XMM5 = 5,
					R6 = 6, DH = 6, SI = 6, ESI = 6, RSI = 6, ST6 = 6, MM6 = 6, XMM6 = 6,
					R7 = 7, BH = 7, DI = 7, EDI = 7, RDI = 7, ST7 = 7, MM7 = 7, XMM7 = 7,
					R8 = 8,
					R9 = 9,
					R10 = 10,
					R11 = 11,
					R12 = 12,
					R13 = 13,
					R14 = 14,
					R15 = 15
				};

				enum Mod
				{
					MOD_NO_DISP = 0,
					MOD_BYTE_DISP = 1,
					MOD_DWORD_DISP = 2,
					MOD_REG = 3
				};

				enum Scale
				{
					SCALE_UNKNOWN = 0,
					SCALE_1 = 0,
					SCALE_2 = 1,
					SCALE_4 = 2,
					SCALE_8 = 3
				};

				struct OperandREG;

				struct Operand
				{
					enum Type
					{
						OPERAND_UNKNOWN	= 0,

						OPERAND_VOID	= 0x00000001,

						OPERAND_ONE		= 0x00000002,
						OPERAND_EXT8	= 0x00000004 | OPERAND_ONE,   // Sign extended
						OPERAND_REF		= 0x00000008,
						OPERAND_IMM8	= 0x00000010 | OPERAND_EXT8 | OPERAND_ONE,
						OPERAND_IMM16	= 0x00000020 | OPERAND_IMM8 | OPERAND_EXT8 | OPERAND_ONE,
						OPERAND_IMM32	= 0x00000040 | OPERAND_REF | OPERAND_IMM16 | OPERAND_IMM8 | OPERAND_EXT8 | OPERAND_ONE,
						OPERAND_IMM		= OPERAND_IMM32 | OPERAND_IMM16 | OPERAND_IMM8 | OPERAND_EXT8 | OPERAND_ONE,

						OPERAND_AL		= 0x00000080,
						OPERAND_CL		= 0x00000100,
						OPERAND_REG8	= OPERAND_CL | OPERAND_AL,

						OPERAND_AX		= 0x00000200,
						OPERAND_DX		= 0x00000400,
						OPERAND_CX		= 0x00000800,
						OPERAND_REG16	= OPERAND_CX | OPERAND_DX | OPERAND_AX,

						OPERAND_EAX		= 0x00001000,
						OPERAND_ECX		= 0x00002000,
						OPERAND_REG32	= OPERAND_ECX | OPERAND_EAX,

						OPERAND_RAX		= 0x00004000,
						OPERAND_REG64	= 0x00008000 | OPERAND_RAX,

						OPERAND_CS		= 0,   // No need to touch these in protected mode
						OPERAND_DS		= 0,
						OPERAND_ES		= 0,
						OPERAND_SS		= 0,
						OPERAND_FS		= 0,
						OPERAND_GS		= 0,
						OPERAND_SEGREG	= OPERAND_GS | OPERAND_FS | OPERAND_SS | OPERAND_ES | OPERAND_DS | OPERAND_CS,

						OPERAND_ST0		= 0x00010000,
						OPERAND_FPUREG	= 0x00020000 | OPERAND_ST0,
						
						OPERAND_CR		= 0,   // You won't need these in a JIT assembler
						OPERAND_DR		= 0,
						OPERAND_TR		= 0,

						OPERAND_MMREG	= 0x00040000,
						OPERAND_XMMREG	= 0x00080000,

						OPERAND_REG		= OPERAND_XMMREG | OPERAND_MMREG | OPERAND_TR | OPERAND_DR | OPERAND_CR | OPERAND_FPUREG | OPERAND_SEGREG | OPERAND_REG32 | OPERAND_REG64 | OPERAND_REG16 | OPERAND_REG8,

						OPERAND_MEM8	= 0x00100000,
						OPERAND_MEM16	= 0x00200000,
						OPERAND_MEM32	= 0x00400000,
						OPERAND_MEM64	= 0x00800000,
						OPERAND_MEM128	= 0x01000000,
						OPERAND_MEM		= OPERAND_MEM128 | OPERAND_MEM64 | OPERAND_MEM32 | OPERAND_MEM16 | OPERAND_MEM8,
					
						OPERAND_XMM32	= OPERAND_MEM32 | OPERAND_XMMREG,
						OPERAND_XMM64	= OPERAND_MEM64 | OPERAND_XMMREG,

						OPERAND_R_M8	= OPERAND_MEM8 | OPERAND_REG8,
						OPERAND_R_M16	= OPERAND_MEM16 | OPERAND_REG16,
						OPERAND_R_M32	= OPERAND_MEM32 | OPERAND_REG32,
						OPERAND_R_M64	= OPERAND_MEM64 | OPERAND_REG64,
						OPERAND_MM64	= OPERAND_MEM64 | OPERAND_MMREG,
						OPERAND_R_M128	= OPERAND_MEM128 | OPERAND_XMMREG,
						OPERAND_R_M		= OPERAND_MEM | OPERAND_REG
					};

					Type				type;
					union
					{
						Label*			Reference;
						const char*		Notation;
					};

					union
					{
						int				Value;     // For immediates
						int				Reg;       // For registers
						int				BaseReg;   // For memory references
					};

					int					IndexReg;
					int					Scale;
					int					Displacement;

					Operand( Type type = OPERAND_VOID )
					{
						this->type = type;
					}

					bool operator==( Operand &op );
					bool operator!=( Operand &op );

					static bool IsSubtypeOf( Type type, Type baseType );
					bool IsSubtypeOf( Type baseType ) const;

					const char* string() const;

					static bool IsVoid( Type type );
					static bool IsImm( Type type );
					static bool IsReg( Type type );
					static bool IsMem( Type type );
					static bool IsRM( Type type );

					static bool IsVoid( const Operand& operand );
					static bool IsImm( const Operand& operand );
					static bool IsReg( const Operand& operand );
					static bool IsMem( const Operand& operand );
					static bool IsRM( const Operand& operand );

					const char* RegName() const;
					const char* IndexName() const;

					static Operand::Type Scan( const char* string );

					struct Register
					{
						Type		type;
						const char*	Notation;
						int			Reg;		// Index
					};

					static const Register RegisterSet[];
					static const Register SyntaxSet[];
				};

				struct OperandVOID : virtual Operand
				{
					OperandVOID() : Operand( OPERAND_VOID )
					{
					}
				};

				struct OperandIMM : virtual Operand
				{
					OperandIMM(int imm = 0) : Operand( OPERAND_IMM )
					{
						Value = imm;
						Reference = 0;
					}
				};

				struct OperandREF : virtual Operand
				{
					OperandREF(const void *ref = 0) : Operand( OPERAND_REF )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = (int)ref;
						Reference = 0;
					}

					OperandREF(Label* ref) : Operand( OPERAND_IMM )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = ref;
					}

					OperandREF(int ref) : Operand( OPERAND_REF )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = ref;
						Reference = 0;
					}

					const OperandREF operator+(const void *disp) const
					{
						OperandREF returnReg;

						returnReg.BaseReg = BaseReg;
						returnReg.IndexReg = IndexReg;
						returnReg.Scale = Scale;
						returnReg.Displacement = Displacement + (int)disp;

						return returnReg;
					}

					const OperandREF operator+(int disp) const
					{
						OperandREF returnReg;

						returnReg.BaseReg = BaseReg;
						returnReg.IndexReg = IndexReg;
						returnReg.Scale = Scale;
						returnReg.Displacement = Displacement + disp;

						return returnReg;
					}

					const OperandREF operator-(int disp) const
					{
						OperandREF returnReg;

						returnReg.BaseReg = BaseReg;
						returnReg.IndexReg = IndexReg;
						returnReg.Scale = Scale;
						returnReg.Displacement = Displacement - disp;

						return returnReg;
					}

					bool operator==(const OperandREF &ref) const
					{
						return BaseReg == ref.BaseReg &&
							   IndexReg == ref.IndexReg &&
							   Scale == ref.Scale &&
							   Displacement == ref.Displacement;
					}

					bool operator!=(const OperandREF &ref) const
					{
						return !(*this == ref);
					}
				};

				struct OperandMEM : virtual Operand
				{
					OperandMEM() : Operand( OPERAND_MEM )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					OperandMEM(const OperandREF &ref) : Operand( OPERAND_MEM )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					OperandMEM operator[](const OperandREF &ref) const
					{
						return OperandMEM(ref);
					}

					const OperandMEM operator+(int disp) const
					{
						OperandMEM returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM operator-(int disp) const
					{
						OperandMEM returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandMEM8 : OperandMEM
				{
					OperandMEM8() : Operand( OPERAND_MEM8 ) {};

					explicit OperandMEM8(const OperandREF &ref) : Operand( OPERAND_MEM8 )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					explicit OperandMEM8(const OperandMEM &mem) : Operand( OPERAND_MEM8 )
					{
						BaseReg = mem.BaseReg;
						IndexReg = mem.IndexReg;
						Scale = mem.Scale;
						Displacement = mem.Displacement;
						Reference = mem.Reference;
					}

					explicit OperandMEM8(const Operand &r_m8) : Operand( OPERAND_MEM8 )
					{
						BaseReg = r_m8.BaseReg;
						IndexReg = r_m8.IndexReg;
						Scale = r_m8.Scale;
						Displacement = r_m8.Displacement;
						Reference = r_m8.Reference;
					}

					OperandMEM8 operator[](const OperandREF &ref) const
					{
						return OperandMEM8(ref);
					}

					const OperandMEM8 operator+(int disp) const
					{
						OperandMEM8 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM8 operator-(int disp) const
					{
						OperandMEM8 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandMEM16 : OperandMEM
				{
					OperandMEM16() : Operand( OPERAND_MEM16 ) {};

					explicit OperandMEM16(const OperandREF &ref) : Operand( OPERAND_MEM16 )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					explicit OperandMEM16(const OperandMEM &mem) : Operand( OPERAND_MEM16 )
					{
						BaseReg = mem.BaseReg;
						IndexReg = mem.IndexReg;
						Scale = mem.Scale;
						Displacement = mem.Displacement;
						Reference = mem.Reference;
					}

					explicit OperandMEM16(const Operand &r_m16) : Operand( OPERAND_MEM16 )
					{
						BaseReg = r_m16.BaseReg;
						IndexReg = r_m16.IndexReg;
						Scale = r_m16.Scale;
						Displacement = r_m16.Displacement;
						Reference = r_m16.Reference;
					}

					OperandMEM16 operator[](const OperandREF &ref) const
					{
						return OperandMEM16(ref);
					}

					const OperandMEM16 operator+(int disp) const
					{
						OperandMEM16 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM16 operator-(int disp) const
					{
						OperandMEM16 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandMEM32 : OperandMEM
				{
					OperandMEM32() : Operand( OPERAND_MEM32 ) {};

					explicit OperandMEM32(const OperandMEM &mem) : Operand( OPERAND_MEM32 )
					{
						BaseReg = mem.BaseReg;
						IndexReg = mem.IndexReg;
						Scale = mem.Scale;
						Displacement = mem.Displacement;
						Reference = mem.Reference;
					}

					explicit OperandMEM32(const OperandREF &ref) : Operand( OPERAND_MEM32 )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					explicit OperandMEM32(const Operand &r32) : Operand( OPERAND_MEM32 )
					{
						BaseReg = r32.BaseReg;
						IndexReg = r32.IndexReg;
						Scale = r32.Scale;
						Displacement = r32.Displacement;
						Reference = r32.Reference;
					}		

					OperandMEM32 operator[](const OperandREF &ref) const
					{
						return OperandMEM32(ref);
					}

					const OperandMEM32 operator+(int disp) const
					{
						OperandMEM32 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM32 operator-(int disp) const
					{
						OperandMEM32 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandMEM64 : OperandMEM
				{
					OperandMEM64() : Operand( OPERAND_MEM64 ) {};

					explicit OperandMEM64(const OperandMEM &mem) : Operand( OPERAND_MEM64 )
					{
						BaseReg = mem.BaseReg;
						IndexReg = mem.IndexReg;
						Scale = mem.Scale;
						Displacement = mem.Displacement;
						Reference = mem.Reference;
					}

					explicit OperandMEM64(const OperandREF &ref) : Operand( OPERAND_MEM64 )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					explicit OperandMEM64(const Operand &r_m64) : Operand( OPERAND_MEM64 )
					{
						BaseReg = r_m64.BaseReg;
						IndexReg = r_m64.IndexReg;
						Scale = r_m64.Scale;
						Displacement = r_m64.Displacement;
						Reference = r_m64.Reference;
					}

					OperandMEM64 operator[](const OperandREF &ref) const
					{
						return OperandMEM64(ref);
					}

					const OperandMEM64 operator+(int disp) const
					{
						OperandMEM64 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM64 operator-(int disp) const
					{
						OperandMEM64 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandMEM128 : OperandMEM
				{
					OperandMEM128() : Operand( OPERAND_MEM128 ) {};

					explicit OperandMEM128(const OperandMEM &mem) : Operand( OPERAND_MEM128 )
					{
						BaseReg = mem.BaseReg;
						IndexReg = mem.IndexReg;
						Scale = mem.Scale;
						Displacement = mem.Displacement;
						Reference = mem.Reference;
					}

					explicit OperandMEM128(const OperandREF &ref) : Operand( OPERAND_MEM128 )
					{
						BaseReg = ref.BaseReg;
						IndexReg = ref.IndexReg;
						Scale = ref.Scale;
						Displacement = ref.Displacement;
						Reference = ref.Reference;
					}

					explicit OperandMEM128(const Operand &r_m128) : Operand( OPERAND_MEM128 )
					{
						BaseReg = r_m128.BaseReg;
						IndexReg = r_m128.IndexReg;
						Scale = r_m128.Scale;
						Displacement = r_m128.Displacement;
						Reference = r_m128.Reference;
					}

					OperandMEM128 operator[](const OperandREF &ref) const
					{
						return OperandMEM128(ref);
					}

					const OperandMEM128 operator+(int disp) const
					{
						OperandMEM128 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement + disp;
						returnMem.Reference = Reference;

						return returnMem;
					}

					const OperandMEM128 operator-(int disp) const
					{
						OperandMEM128 returnMem;

						returnMem.BaseReg = BaseReg;
						returnMem.IndexReg = IndexReg;
						returnMem.Scale = Scale;
						returnMem.Displacement = Displacement - disp;
						returnMem.Reference = Reference;

						return returnMem;
					}
				};

				struct OperandR_M32 : virtual Operand
				{
					OperandR_M32() : Operand( OPERAND_R_M32 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandR_M32(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandR_M16 : virtual Operand
				{
					OperandR_M16() : Operand( OPERAND_R_M16 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandR_M16(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}

					explicit OperandR_M16(const OperandR_M32 &r_m32) : Operand( OPERAND_R_M16 )
					{
						BaseReg = r_m32.BaseReg;
						IndexReg = r_m32.IndexReg;
						Scale = r_m32.Scale;
						Displacement = r_m32.Displacement;
						Reference = r_m32.Reference;
					}
				};

				struct OperandR_M8 : virtual Operand
				{
					OperandR_M8() : Operand( OPERAND_R_M8 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandR_M8(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}

					explicit OperandR_M8(const OperandR_M16 &r_m16) : Operand( OPERAND_R_M8 )
					{
						BaseReg = r_m16.BaseReg;
						IndexReg = r_m16.IndexReg;
						Scale = r_m16.Scale;
						Displacement = r_m16.Displacement;
						Reference = r_m16.Reference;
					}

					explicit OperandR_M8(const OperandR_M32 &r_m32) : Operand( OPERAND_R_M8 )
					{
						BaseReg = r_m32.BaseReg;
						IndexReg = r_m32.IndexReg;
						Scale = r_m32.Scale;
						Displacement = r_m32.Displacement;
						Reference = r_m32.Reference;
					}
				};

				struct OperandR_M64 : virtual Operand
				{
					OperandR_M64() : Operand( OPERAND_R_M64 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandR_M64(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandR_M128 : virtual Operand
				{
					OperandR_M128() : Operand( OPERAND_R_M128 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandR_M128(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandXMM32 : virtual Operand
				{
					OperandXMM32() : Operand( OPERAND_XMM32 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandXMM32(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandXMM64 : virtual Operand
				{
					OperandXMM64() : Operand( OPERAND_XMM64 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandXMM64(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandMM64 : virtual Operand
				{
					OperandMM64() : Operand( OPERAND_MM64 )
					{
						BaseReg = CodeGen::REG_UNKNOWN;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandMM64(const Operand &reg)
					{
						type = reg.type;
						BaseReg = reg.BaseReg;
						IndexReg = reg.IndexReg;
						Scale = reg.Scale;
						Displacement = reg.Displacement;
						Reference = reg.Reference;
					}
				};

				struct OperandREG : virtual Operand
				{
					OperandREG() : Operand( OPERAND_VOID )
					{
					}

					OperandREG(const Operand &reg)
					{
						type = reg.type;
						Reg = reg.Reg;
						Reference = reg.Reference;
					}
				};

				struct OperandREG32;
				struct OperandREG64;

				struct OperandREGxX : OperandREF
				{
					OperandREGxX(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_REG32 )
					{
						Reg = reg;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					friend const OperandREF operator+(const OperandREGxX &ref1, const OperandREG32 &ref2);
					friend const OperandREF operator+(const OperandREGxX &ref1, const OperandREG64 &ref2);
					friend const OperandREGxX operator+(const OperandREGxX &ref, void *disp);
					friend const OperandREGxX operator+(const OperandREGxX &ref, int disp);
					friend const OperandREGxX operator-(const OperandREGxX &ref, int disp);

					friend const OperandREF operator+(const OperandREG32 &ref2, const OperandREGxX &ref1);
					friend const OperandREF operator+(const OperandREG64 &ref2, const OperandREGxX &ref1);
					friend const OperandREGxX operator+(void *disp, const OperandREGxX &ref);
					friend const OperandREGxX operator+(int disp, const OperandREGxX &ref);
					friend const OperandREGxX operator-(int disp, const OperandREGxX &ref);
				};

				struct OperandREG32 : OperandR_M32, OperandREF, OperandREG
				{
					OperandREG32(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_REG32 )
					{
						Reg = reg;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandREG32(const OperandR_M32 &r_m32) : Operand( OPERAND_REG32 )
					{
						Reg = r_m32.Reg;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					friend const OperandREF operator+(const OperandREG32 ref, const OperandREG32 &ref2);

					friend const OperandREG32 operator+(const OperandREG32 ref, void *disp);
					friend const OperandREG32 operator+(const OperandREG32 ref, int disp);
					friend const OperandREG32 operator-(const OperandREG32 ref, int disp);
					friend const OperandREGxX operator*(const OperandREG32 ref, int Scale);

					friend const OperandREG32 operator+(void *disp, const OperandREG32 ref);
					friend const OperandREG32 operator+(int disp, const OperandREG32 ref);
					friend const OperandREG32 operator-(int disp, const OperandREG32 ref);
					friend const OperandREGxX operator*(int Scale, const OperandREG32 ref);
				};

				struct OperandREG64 : OperandR_M64, OperandREF, OperandREG
				{
					OperandREG64(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_REG64 )
					{
						Reg = reg;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					explicit OperandREG64(const OperandR_M64 &r_m64) : Operand( OPERAND_REG64 )
					{
						Reg = r_m64.Reg;
						IndexReg = CodeGen::REG_UNKNOWN;
						Scale = 0;
						Displacement = 0;
						Reference = 0;
					}

					friend const OperandREF operator+(const OperandREG64 ref, const OperandREG64 &ref2);

					friend const OperandREG64 operator+(const OperandREG64 ref, void *disp);
					friend const OperandREG64 operator+(const OperandREG64 ref, int disp);
					friend const OperandREG64 operator-(const OperandREG64 ref, int disp);
					friend const OperandREGxX operator*(const OperandREG64 ref, int Scale);

					friend const OperandREG64 operator+(void *disp, const OperandREG64 ref);
					friend const OperandREG64 operator+(int disp, const OperandREG64 ref);
					friend const OperandREG64 operator-(int disp, const OperandREG64 ref);
					friend const OperandREGxX operator*(int Scale, const OperandREG64 ref);
				};

				struct OperandREG16 : OperandR_M16, OperandREG
				{
					OperandREG16(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_REG16 )
					{
						Reg = reg;
						Reference = 0;
					}

					explicit OperandREG16(const OperandREG32 &r32) : Operand( OPERAND_REG16 )
					{
						Reg = r32.Reg;
						Reference = 0;
					}

					explicit OperandREG16(const OperandR_M16 &r_m16) : Operand( OPERAND_REG16 )
					{
						Reg = r_m16.Reg;
						Reference = 0;
					}
				};

				struct OperandREG8 : OperandR_M8, OperandREG
				{
					OperandREG8(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_REG8 )
					{
						Reg = reg;
						Reference = 0;
					}

					explicit OperandREG8(const OperandREG16 &r16) : Operand( OPERAND_REG8 )
					{
						Reg = r16.Reg;
						Reference = 0;
					}

					explicit OperandREG8(const OperandREG32 &r32) : Operand( OPERAND_REG8 )
					{
						Reg = r32.Reg;
						Reference = 0;
					}

					explicit OperandREG8(const OperandR_M8 &r_m8) : Operand( OPERAND_REG8 )
					{
						Reg = r_m8.Reg;
						Reference = 0;
					}
				};

				struct OperandFPUREG : virtual Operand, OperandREG
				{
					OperandFPUREG(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_FPUREG )
					{
						Reg = reg;
						Reference = 0;
					}
				};

				struct OperandMMREG : OperandMM64, OperandREG
				{
					OperandMMREG(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_MMREG )
					{
						Reg = reg;
						Reference = 0;
					}

					explicit OperandMMREG(const OperandMM64 r_m64) : Operand( OPERAND_MMREG )
					{
						Reg = r_m64.Reg;
						Reference = 0;
					}
				};

				struct OperandXMMREG : OperandR_M128, OperandXMM32, OperandXMM64, OperandREG
				{
					OperandXMMREG(int reg = CodeGen::REG_UNKNOWN) : Operand( OPERAND_XMMREG )
					{
						Reg = reg;
						Reference = 0;
					}

					explicit OperandXMMREG(const OperandXMM32 &xmm32) : Operand( OPERAND_XMMREG )
					{
						Reg = xmm32.Reg;
						Reference = 0;
					}

					explicit OperandXMMREG(const OperandXMM64 &xmm64) : Operand( OPERAND_XMMREG )
					{
						Reg = xmm64.Reg;
						Reference = 0;
					}

					explicit OperandXMMREG(const OperandR_M128 &r_m128) : Operand( OPERAND_XMMREG )
					{
						Reg = r_m128.Reg;
						Reference = 0;
					}
				};

				struct OperandAL : OperandREG8
				{
					OperandAL() : Operand( OPERAND_AL )
					{
						Reg = CodeGen::AL;
						Reference = 0;
					}
				};

				struct OperandCL : OperandREG8
				{
					OperandCL() : Operand( OPERAND_CL )
					{
						Reg = CodeGen::CL;
						Reference = 0;
					}
				};

				struct OperandAX : OperandREG16
				{
					OperandAX() : Operand( OPERAND_AX )
					{
						Reg = CodeGen::AX;
						Reference = 0;
					}
				};

				struct OperandDX : OperandREG16
				{
					OperandDX() : Operand( OPERAND_DX )
					{
						Reg = CodeGen::DX;
						Reference = 0;
					}
				};

				struct OperandCX : OperandREG16
				{
					OperandCX() : Operand( OPERAND_CX )
					{
						Reg = CodeGen::CX;
						Reference = 0;
					}
				};

				struct OperandEAX : OperandREG32
				{
					OperandEAX() : Operand( OPERAND_EAX )
					{
						Reg = CodeGen::EAX;
						Reference = 0;
					}
				};

				struct OperandRAX : OperandREG32
				{
					OperandRAX() : Operand( OPERAND_RAX )
					{
						Reg = CodeGen::RAX;
						Reference = 0;
					}
				};

				struct OperandECX : OperandREG32
				{
					OperandECX() : Operand( OPERAND_ECX )
					{
						Reg = CodeGen::ECX;
						Reference = 0;
					}
				};

				struct OperandST0 : OperandFPUREG
				{
					OperandST0() : Operand( OPERAND_ST0 )
					{
						Reg = CodeGen::ST0;
						Reference = 0;
					}
				};
			
				inline const OperandREF operator+(const OperandREGxX &ref1, const OperandREG32 &ref2)
				{
					OperandREF returnReg;

					returnReg.BaseReg = ref2.BaseReg;
					returnReg.IndexReg = ref1.IndexReg;
					returnReg.Scale = ref1.Scale;
					returnReg.Displacement = ref1.Displacement + ref2.Displacement;

					return returnReg;
				}

				inline const OperandREF operator+(const OperandREGxX &ref1, const OperandREG64 &ref2)
				{
					OperandREF returnReg;

					returnReg.BaseReg = ref2.BaseReg;
					returnReg.IndexReg = ref1.IndexReg;
					returnReg.Scale = ref1.Scale;
					returnReg.Displacement = ref1.Displacement + ref2.Displacement;

					return returnReg;
				}

				inline const OperandREGxX operator+(const OperandREGxX &ref, void *disp)
				{
					OperandREGxX returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + (int)disp;

					return returnReg;
				}

				inline const OperandREGxX operator+(const OperandREGxX &ref, int disp)
				{
					OperandREGxX returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + disp;

					return returnReg;
				}

				inline const OperandREGxX operator-(const OperandREGxX &ref, int disp)
				{
					OperandREGxX returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement - disp;

					return returnReg;
				}

				inline const OperandREF operator+(const OperandREG32 &ref2, const OperandREGxX &ref1)
				{
					return ref1 + ref2;
				}

				inline const OperandREF operator+(const OperandREG64 &ref2, const OperandREGxX &ref1)
				{
					return ref1 + ref2;
				}

				inline const OperandREGxX operator+(void *disp, const OperandREGxX &ref)
				{
					return ref + disp;
				}

				inline const OperandREGxX operator+(int disp, const OperandREGxX &ref)
				{
					return ref + disp;
				}

				inline const OperandREGxX operator-(int disp, const OperandREGxX &ref)
				{
					return ref + disp;
				}

				inline const OperandREF operator+(const OperandREG32 ref1, const OperandREG32 &ref2)
				{
					OperandREF returnReg;

					returnReg.BaseReg = ref1.Reg;
					returnReg.IndexReg = ref2.Reg;
					returnReg.Scale = 1;
					returnReg.Displacement = ref1.Displacement + ref2.Displacement;

					return returnReg;
				}

				inline const OperandREG32 operator+(const OperandREG32 ref, void *disp)
				{
					OperandREG32 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + (int)disp;

					return returnReg;
				}

				inline const OperandREG32 operator+(const OperandREG32 ref, int disp)
				{
					OperandREG32 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + disp;

					return returnReg;
				}

				inline const OperandREG32 operator-(const OperandREG32 ref, int disp)
				{
					OperandREG32 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement - disp;

					return returnReg;
				}

				inline const OperandREGxX operator*(const OperandREG32 ref, int Scale)
				{
					OperandREGxX returnReg;

					returnReg.BaseReg = CodeGen::REG_UNKNOWN;
					returnReg.IndexReg = ref.BaseReg;
					returnReg.Scale = Scale;
					returnReg.Displacement = ref.Displacement;

					return returnReg;
				}

				inline const OperandREG32 operator+(void *disp, const OperandREG32 ref)
				{
					return ref + disp;
				}

				inline const OperandREG32 operator+(int disp, const OperandREG32 ref)
				{
					return ref + disp;
				}

				inline const OperandREG32 operator-(int disp, const OperandREG32 ref)
				{
					return ref - disp;
				}

				inline const OperandREGxX operator*(int Scale, const OperandREG32 ref)
				{
					return ref * Scale;
				}

				inline const OperandREF operator+(const OperandREG64 ref1, const OperandREG64 &ref2)
				{
					OperandREF returnReg;

					returnReg.BaseReg = ref1.Reg;
					returnReg.IndexReg = ref2.Reg;
					returnReg.Scale = 1;
					returnReg.Displacement = ref1.Displacement + ref2.Displacement;

					return returnReg;
				}

				inline const OperandREG64 operator+(const OperandREG64 ref, void *disp)
				{
					OperandREG64 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + (int)disp;

					return returnReg;
				}

				inline const OperandREG64 operator+(const OperandREG64 ref, int disp)
				{
					OperandREG64 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement + disp;

					return returnReg;
				}

				inline const OperandREG64 operator-(const OperandREG64 ref, int disp)
				{
					OperandREG64 returnReg;

					returnReg.BaseReg = ref.BaseReg;
					returnReg.IndexReg = ref.IndexReg;
					returnReg.Scale = ref.Scale;
					returnReg.Displacement = ref.Displacement - disp;

					return returnReg;
				}

				inline const OperandREGxX operator*(const OperandREG64 ref, int Scale)
				{
					OperandREGxX returnReg;

					returnReg.BaseReg = CodeGen::REG_UNKNOWN;
					returnReg.IndexReg = ref.BaseReg;
					returnReg.Scale = Scale;
					returnReg.Displacement = ref.Displacement;

					return returnReg;
				}

				inline const OperandREG64 operator+(void *disp, const OperandREG64 ref)
				{
					return ref + disp;
				}

				inline const OperandREG64 operator+(int disp, const OperandREG64 ref)
				{
					return ref + disp;
				}

				inline const OperandREG64 operator-(int disp, const OperandREG64 ref)
				{
					return ref - disp;
				}

				inline const OperandREGxX operator*(int Scale, const OperandREG64 ref)
				{
					return ref * Scale;
				}

			}
		}
	}
}
