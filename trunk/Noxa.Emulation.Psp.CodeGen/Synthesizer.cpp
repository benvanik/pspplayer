// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Synthesizer.h"
#include <stdlib.h>

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;

Synthesizer::Synthesizer()
{
	Reset();
}

void Synthesizer::Reset()
{
	_op1Type	= Operand::OPERAND_UNKNOWN;
	_op2Type	= Operand::OPERAND_UNKNOWN;

	_op1Reg		= CodeGen::REG_UNKNOWN;
	_op2Reg		= CodeGen::REG_UNKNOWN;
	_baseReg	= CodeGen::REG_UNKNOWN;
	_indexReg	= CodeGen::REG_UNKNOWN;
	
	_scale		= 0;

	_reference	= 0;
	_target		= 0;
	relative	= false;

	format.P1	= false;
	format.P2	= false;
	format.P3	= false;
	format.P4	= false;
	format.REX	= false;
	format.O2	= false;
	format.O1	= false;
	format.modRM	= false;
	format.SIB	= false;
	format.D1	= false;
	format.D2	= false;
	format.D3	= false;
	format.D4	= false;
	format.I1	= false;
	format.I2	= false;
	format.I3	= false;
	format.I4	= false;

	P1			= 0xCC;
	P2			= 0xCC;
	P3			= 0xCC;
	P4			= 0xCC;
	REX.b		= 0xCC;
	O2			= 0xCC;
	O1			= 0xCC;
	modRM.b		= 0xCC;
	SIB.b		= 0xCC;
	D1			= 0xCC;
	D2			= 0xCC;
	D3			= 0xCC;
	D4			= 0xCC;
	I1			= 0xCC;
	I2			= 0xCC;
	I3			= 0xCC;
	I4			= 0xCC;
}

void Synthesizer::EncodeOperand1( const Operand& op1 )
{
	// Instruction destination already set
	assert( _op1Type == Operand::OPERAND_UNKNOWN );

	_op1Type = op1.type;

	if( Operand::IsReg( _op1Type ) )
	{
		_op1Reg = op1.Reg;
	}
	else if( Operand::IsMem( _op1Type ) )
	{
		EncodeBase( op1 );
		EncodeIndex( op1 );

		SetScale( op1.Scale );
		SetDisplacement( op1.Displacement );

		SetReference( op1.Reference );
	}
	else if( Operand::IsImm( _op1Type ) )
	{
		EncodeImmediate( op1.Value );
		SetReference( op1.Reference );
	}
	else if( !Operand::IsVoid( _op1Type ) )
	{
		assert( false );
	}
}

void Synthesizer::EncodeOperand2( const Operand& op2 )
{
	// Instruction source already set
	assert( _op2Type == Operand::OPERAND_UNKNOWN );

	_op2Type = op2.type;

	if( Operand::IsReg( _op2Type ) )
	{
		_op2Reg = op2.Reg;
	}
	else if( Operand::IsMem( _op2Type ) )
	{
		EncodeBase( op2 );
		EncodeIndex( op2 );

		SetScale( op2.Scale );
		SetDisplacement( op2.Displacement );

		SetReference( op2.Reference );
	}
	else if( Operand::IsImm( _op2Type ) )
	{
		EncodeImmediate( op2.Value );
		SetReference( op2.Reference );
	}
	else if( !Operand::IsVoid( _op2Type ) )
	{
		assert( false );
	}
}

void Synthesizer::EncodeOperand3( const Operand& op3 )
{
	if( Operand::IsImm( op3 ) )
	{
		EncodeImmediate( op3.Value );
		SetReference( op3.Reference );
	}
	else if( !Operand::IsVoid( op3 ) )
	{
		assert( false );
	}
}

void Synthesizer::EncodeBase( const Operand& base )
{
	if( _baseReg != CodeGen::REG_UNKNOWN )
	{
		// Base already set, use as index with _scale = 1
		EncodeIndex( base );
		SetScale( 1 );
		return;
	}

	_baseReg = base.BaseReg;
}

void Synthesizer::EncodeIndex( const Operand& index )
{
	// Memory reference can't have multiple index registers
	assert( _indexReg != CodeGen::REG_UNKNOWN );

	_indexReg = index.IndexReg;
}

void Synthesizer::SetScale( int scale )
{
	// Memory reference can't have multiple _scale factors
	assert( _scale != 0 );

	// Invalid _scale value
	assert( scale != 0 && scale != 1 && scale != 2 && scale != 4 && scale != 8 );

	_scale = scale;
}

void Synthesizer::EncodeImmediate( int _immediate )
{
	// Instruction can't have multiple immediate operands
	assert( immediate == 0xCCCCCCCC );

	immediate = _immediate;
}

void Synthesizer::SetDisplacement( int _displacement )
{
	displacement = _displacement;
}

void Synthesizer::AddDisplacement( int _displacement )
{
	displacement += _displacement;
}

void Synthesizer::EncodeInstruction( const Instruction* instruction )
{
	if( instruction == 0 )
		return;

	const char* formats = instruction->Syntax->Encoding;
	assert( formats != 0 );

	while( *formats )
	{
		switch( ( formats[ 0 ] << 8 ) | formats[ 1 ] )
		{
		case LOCK_PRE:
			AddPrefix( 0xF0 );
			break;
		case CONST_PRE:
			AddPrefix( 0xF1 );
			break;
		case REPNE_PRE:
			AddPrefix( 0xF2 );
			break;
		case REP_PRE:
			AddPrefix( 0xF3 );
			break;
		case OFF_PRE:
			if( IS32BIT( instruction ) == false )
			{
				AddPrefix( 0x66 );
			}
			break;
		case ADDR_PRE:
			if( IS32BIT( instruction ) == false )
			{
				AddPrefix( 0x67 );
			}
			break;
		case ADD_REG:
			EncodeRexByte( instruction );
			if( format.O1 )
			{
				if( Operand::IsReg( _op1Type ) &&
					_op1Type != Operand::OPERAND_ST0 )
				{
					O1 += _op1Reg & 0x7;
					REX.B = (_op1Reg & 0x8) >> 3;
				}
				else if( Operand::IsReg( _op2Type ) )
				{
					O1 += _op2Reg & 0x7;
					REX.B = (_op2Reg & 0x8) >> 3;
				}
				else if( Operand::IsReg( _op1Type ) &&
					_op1Type == Operand::OPERAND_ST0 )
				{
					O1 += _op1Reg & 0x7;
					REX.B = (_op1Reg & 0x8) >> 3;
				}
				else
				{
					// '+r' not compatible with operands
					assert( false );
				}
			}
			else
			{
				// '+r' needs first opcode byte
				assert( false );
			}
			break;
		case EFF_ADDR:
			EncodeRexByte( instruction );
			EncodeModField();
			EncodeRegField( instruction );
			EncodeRMField( instruction );
			EncodeSibByte( instruction );
			break;
		case MOD_RM_0:
		case MOD_RM_1:
		case MOD_RM_2:
		case MOD_RM_3:
		case MOD_RM_4:
		case MOD_RM_5:
		case MOD_RM_6:
		case MOD_RM_7:
			EncodeRexByte( instruction );
			EncodeModField();
			modRM.reg = formats[ 1 ] - '0';
			EncodeRMField( instruction );
			EncodeSibByte( instruction );
			break;
		case QWORD_IMM:
			// Not implemented
			assert( false );
			break;
		case DWORD_IMM:
			format.I1 = true;
			format.I2 = true;
			format.I3 = true;
			format.I4 = true;
			break;
		case WORD_IMM:
			format.I1 = true;
			format.I2 = true;
			break;
		case BYTE_IMM:
			format.I1 = true;
			break;
		case BYTE_REL:
			format.I1 = true;
			relative = true;
			break;
		case DWORD_REL:
			format.I1 = true;
			format.I2 = true;
			format.I3 = true;
			format.I4 = true;
			relative = true;
			break;
		default:
			unsigned int opcode = strtoul( formats, 0, 16 );
			assert( opcode <= 0xFF );

			if( !format.O1 )
			{
				O1 = (unsigned char)opcode;
				format.O1 = true;
			}
			else if( !format.O2 &&
					( O1 == 0x0F ||
					  O1 == 0xD8 ||
					  O1 == 0xD9 ||
					  O1 == 0xDA ||
					  O1 == 0xDB ||
					  O1 == 0xDC ||
					  O1 == 0xDD ||
					  O1 == 0xDE ||
					  O1 == 0xDF ) )
			{
				O2 = O1;
				O1 = opcode;
				format.O2 = true;
			}
			else if( O1 == 0x66 )   // Operand size prefix for SSE2
			{
				AddPrefix(0x66);   // HACK: Might not be valid for later instruction sets
				O1 = opcode;
			}
			else if( O1 == 0x9B )   // FWAIT
			{
				AddPrefix( 0x9B );   // HACK: Might not be valid for later instruction sets
				O1 = opcode;
			}
			else   // 3DNow!, SSE or SSE2 instruction, opcode as immediate
			{
				format.I1 = true;
				I1 = opcode;
			}
		}

		formats += 2;
		if( *formats == ' ' )	
		{
			formats++;
		}
		else if( *formats == '\0' )
		{
			break;
		}
		else
		{
			assert( false );
		}
	}
}

void Synthesizer::EncodeRexByte( const Instruction* instruction )
{
	if( IS64BIT( instruction ) ||
		_op1Reg > 0x07 || _op2Reg > 0x07 || _baseReg > 0x07 || _indexReg > 0x07 )
	{
		format.REX = true;
		REX.prefix = 0x4;
		REX.W = 0;
		REX.R = 0;
		REX.X = 0;
		REX.B = 0;
	}

	if( IS64BIT( instruction ) )
		REX.W = true;
}

void Synthesizer::EncodeModField()
{
	format.modRM = true;
	
	if( Operand::IsReg( _op1Type ) &&
		( Operand::IsReg( _op2Type ) || Operand::IsImm( _op2Type ) || Operand::IsVoid( _op2Type ) ) )
	{
		modRM.mod = CodeGen::MOD_REG;
	}
	else
	{
		if( _baseReg == CodeGen::REG_UNKNOWN )   // Static address
		{
			modRM.mod = CodeGen::MOD_NO_DISP;
			format.D1 = true;
			format.D2 = true;
			format.D3 = true;
			format.D4 = true;
		}
		// VERIFY - this is label stuff I think
		/*else if( encoding.Reference && !displacement )
		{
			modRM.mod = CodeGen::MOD_DWORD_DISP;
			format.D1 = true;
			format.D2 = true;
			format.D3 = true;
			format.D4 = true;
		}*/
		else if( !displacement )
		{
			if( _baseReg == CodeGen::EBP )
			{
				modRM.mod = CodeGen::MOD_BYTE_DISP;
				format.D1 = true;	
			}
			else
			{
				modRM.mod = CodeGen::MOD_NO_DISP;
			}
		}
		else if( (char)displacement == displacement )
		{
			modRM.mod = CodeGen::MOD_BYTE_DISP;
			format.D1 = true;
		}
		else
		{
			modRM.mod = CodeGen::MOD_DWORD_DISP;
			format.D1 = true;
			format.D2 = true;
			format.D3 = true;
			format.D4 = true;
		}
	}
}

void Synthesizer::EncodeRMField( const Instruction* instruction )
{
	int r_m;

	if( Operand::IsReg( instruction->Op1 ) &&
		Operand::IsRM( instruction->Op2 ) )
	{
		if( Operand::IsMem( _op2Type ) )
		{
			if( _baseReg == CodeGen::REG_UNKNOWN )
			{
				r_m = CodeGen::EBP;   // Static address
			}
			else
			{
				r_m = _baseReg;
			}
		}
		else if( Operand::IsReg( _op2Type ) )
		{
			r_m = _op2Reg;
		}
		else
		{
			// Syntax error should be detected by parser
			assert( false );
		}
	}
	else if( Operand::IsRM( instruction->Op1 ) &&
			 Operand::IsReg( instruction->Op2 ) )
	{
		if( Operand::IsMem( _op1Type ) )
		{
			if( _baseReg == CodeGen::REG_UNKNOWN )
			{
				r_m = CodeGen::EBP;   // Static address
			}
			else
			{
				r_m = _baseReg;
			}
		}
		else if( Operand::IsReg( _op1Type ) )
		{
			r_m = _op1Reg;
		}
		else
		{
			// Syntax error should be detected by parser
			assert( false );
		}
	}
	else
	{
		if( Operand::IsMem( _op1Type ) )
		{
			if( _baseReg != CodeGen::REG_UNKNOWN )
			{
				r_m = _baseReg;
			}
			else
			{
				r_m = CodeGen::EBP;   // Displacement only
			}
		}
		else if( Operand::IsReg( _op1Type ) )
		{
			r_m = _op1Reg;
		}
		else
		{
			// Syntax error should be caught by parser
			assert( false );
		}
	}

	modRM.r_m = r_m & 0x07;
	REX.B = (r_m & 0x8) >> 3;
}

void Synthesizer::EncodeRegField( const Instruction* instruction )
{
	int reg;

	if( Operand::IsReg( instruction->Op1 ) &&
		Operand::IsRM( instruction->Op2 ) )
	{
		reg = _op1Reg;
	}
	else if( Operand::IsRM( instruction->Op1 ) &&
			 Operand::IsReg( instruction->Op2 ) )
	{
		reg = _op2Reg;
	}
	else if( Operand::IsReg( instruction->Op1 ) &&
			 Operand::IsImm( instruction->Op2 ) )   // IMUL working on the same register
	{
		reg = _op1Reg;			
	}
	else
	{
		assert( false );
	}

	modRM.reg = reg & 0x07;
	REX.R = (reg & 0x8) >> 3;
}

void Synthesizer::EncodeSibByte( const Instruction* instruction )
{
	if( _scale == 0 && _indexReg == CodeGen::REG_UNKNOWN )
	{
		if( _baseReg == CodeGen::REG_UNKNOWN || modRM.r_m != CodeGen::ESP )
		{
			if( format.SIB )
			{
				assert( false );
			}

			return;   // No SIB byte needed
		}
	}

	format.SIB = true;

	modRM.r_m = CodeGen::ESP;   // Indicates use of SIB in mod R/M

	if( _baseReg == CodeGen::EBP && modRM.mod == CodeGen::MOD_NO_DISP )
	{
		modRM.mod = CodeGen::MOD_BYTE_DISP;
		format.D1 = true;
	}

	if( _indexReg == CodeGen::ESP )
	{
		if( _scale != 1 )
		{
			// ESP can't be _scaled index in memory reference
			assert( false );
		}
		else   // Switch base and index
		{
			int tempReg;

			tempReg = _indexReg;
			_indexReg = _baseReg;
			_baseReg = tempReg;
		}
	}

	if( _baseReg == CodeGen::REG_UNKNOWN )
	{
		SIB.base = CodeGen::EBP;   // No Base

		modRM.mod = CodeGen::MOD_NO_DISP;
		format.D1 = true;
		format.D2 = true;
		format.D3 = true;
		format.D4 = true;
	}
	else
	{
		SIB.base = _baseReg & 0x7;
		REX.X = (_baseReg & 0x8) >> 3;
	}

	if( _indexReg != CodeGen::REG_UNKNOWN )
	{
		SIB.index = _indexReg & 0x7;
		REX.X = (_indexReg & 0x8) >> 3;
	}
	else
	{
		SIB.index = CodeGen::ESP;
	}

	switch(_scale)
	{
	case 0:
	case 1:
		SIB.scale = CodeGen::SCALE_1;
		break;
	case 2:
		SIB.scale = CodeGen::SCALE_2;
		break;
	case 4:
		SIB.scale = CodeGen::SCALE_4;
		break;
	case 8:
		SIB.scale = CodeGen::SCALE_8;
		break;
	default:
		assert( false );
		break;
	}
}

void Synthesizer::AddPrefix( byte p )
{
	if( !format.P1 )
	{
		P1 = p;
		format.P1 = true;
	}
	else if( !format.P2 )
	{
		P2 = p;
		format.P2 = true;
	}
	else if( !format.P3 )
	{
		P3 = p;
		format.P3 = true;
	}
	else if( !format.P4 )
	{
		P4 = p;
		format.P4 = true;
	}
	else
	{
		// Too many prefixes in opcode
		assert( false );
	}
}

int Synthesizer::GetLength()
{
	return this->Commit( 0x0 );
}

int Synthesizer::Commit( byte* buffer ) const
{
	bool write = ( buffer != 0x0 );
	byte* start = buffer;

	#define OUTPUT_BYTE(x) if( write ){ *buffer++ = (x); } else { buffer++; }

	if( P1 == 0xF1 )   // Special 'instructions', indicated by INT01 prefix byte
	{
		if(O1 == 0x90 && immediate)   // ALIGN
		{
			// ALIGN must be under 256 bytes
			assert( immediate < 256 );
			buffer += Align( buffer, immediate, write );
		}
		else if( ( O1 == 0x01 || O1 == 0x02 || O1 == 0x04 ) && displacement )   // Array
		{
			for(int i = 0; i < O1 * displacement; i++)
			{
				OUTPUT_BYTE(0xCC);   // INT3
			}
		}
		else   // Constant
		{
			if(format.I1)		OUTPUT_BYTE(I1);
			if(format.I2)		OUTPUT_BYTE(I2);
			if(format.I3)		OUTPUT_BYTE(I3);
			if(format.I4)		OUTPUT_BYTE(I4);
		}
	}
	else   // Normal instructions
	{
		if(format.P1)		OUTPUT_BYTE(P1);
		if(format.P2)		OUTPUT_BYTE(P2);
		if(format.P3)		OUTPUT_BYTE(P3);
		if(format.P4)		OUTPUT_BYTE(P4);
		//if(format.REX)		OUTPUT_BYTE(REX.b);
		if(format.O2)		OUTPUT_BYTE(O2);
		if(format.O1)		OUTPUT_BYTE(O1);
		if(format.modRM)	OUTPUT_BYTE(modRM.b);
		if(format.SIB)		OUTPUT_BYTE(SIB.b);
		if( ( _target != NULL ) &&
			( _target->Type == RefAbsoluteDisplacement ) )
			_target->CodePointer = buffer;
		if(format.D1)		OUTPUT_BYTE(D1);
		if(format.D2)		OUTPUT_BYTE(D2);
		if(format.D3)		OUTPUT_BYTE(D3);
		if(format.D4)		OUTPUT_BYTE(D4);
		if( ( _target != NULL ) &&
			( ( _target->Type == RefAbsoluteImmediate ) || ( _target->Type == RefRelative ) || ( _target->Type == RefCall ) ) )
			_target->CodePointer = buffer;
		if(format.I1)		OUTPUT_BYTE(I1);
		if(format.I2)		OUTPUT_BYTE(I2);
		if(format.I3)		OUTPUT_BYTE(I3);
		if(format.I4)		OUTPUT_BYTE(I4);
	}

	#undef OUTPUT_BYTE

	return ( int )( buffer - start );
}

int Synthesizer::Align( byte* buffer, int alignment, bool write )
{
	// Alignment must be under 64
	assert( alignment > 64 );

	int padding = alignment - ( int )( ( int64 )buffer % alignment );
	if( padding == alignment )
		padding = 0;

	if( write )
	{
		int i = 0;

		while( i + 3 <= padding )
		{
			// 3-byte NOP
			*buffer++ = 0x66;
			*buffer++ = 0x66;
			*buffer++ = 0x90;
			i += 3;
		}

		if( i + 2 <= padding )
		{
			// 2-byte NOP
			*buffer++ = 0x66;
			*buffer++ = 0x90;
			i += 2;
		}

		if( i + 1 <= padding )
		{
			// 1-byte NOP
			*buffer++ = 0x90;
		}
	}

	return padding;
}

int Synthesizer::GetImmediate() const
{
	return immediate;
}

void Synthesizer::SetImmediate( int imm )
{
	immediate = imm;
}

Label* Synthesizer::GetReference()
{
	if( P1 != 0xF1 )
		return _reference;
	else
		return 0;
}

void Synthesizer::SetReference( Label* reference )
{
	if( reference != NULL )
	{
		assert( _reference == 0 );
		_reference = reference;
	}
}

void Synthesizer::SetJumpOffset( int offset )
{
	// Jump offset range too big
	assert( ( ( char )offset == offset && !format.I2 ) || format.I2 );
	
	immediate = offset;
}

void Synthesizer::SetCallOffset( int offset )
{
	// Call offset should be 32-bit
	assert( format.I1 && format.I2 && format.I3 && format.I4 );
	
	immediate = offset;
}

void Synthesizer::SetTarget( Reference* target )
{
	_target = target;
}
