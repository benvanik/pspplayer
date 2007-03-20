// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "CodeGenerator.h"
#include <malloc.h>
#include <Windows.h>
#include "InstructionSet.h"
#include "Synthesizer.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;

#define DEFAULTMAXIMUMSIZE	512 * 5

CodeGenerator::CodeGenerator( int maximumSize )
{
	if( maximumSize == 0 )
		maximumSize = DEFAULTMAXIMUMSIZE;
	_maximumSize = maximumSize;

	// Initialize the instruction set (this will happen once)
	InstructionSet* is = new InstructionSet();
	delete is;

	_offset = 0;
	_buffer = ( byte* )calloc( 1, _maximumSize );

	_labelIndex = 0;
	_labelTable = ( Label* )calloc( sizeof( Label ), _maximumSize );
	_referenceIndex = 0;
	_referenceTable = ( Reference* )calloc( sizeof( Reference ), _maximumSize );

	_synth = new Synthesizer();
}

CodeGenerator::~CodeGenerator()
{
	SAFEDELETE( _synth );
	SAFEFREE( _buffer );
	SAFEFREE( _labelTable );
	SAFEFREE( _referenceTable );
}

FunctionPointer CodeGenerator::GenerateCode()
{
#ifdef TRACE
#endif

	if( _offset == 0 )
		return 0;

	// Allocate the target buffer (with execute privs) and copy over the code
	void* ptr = VirtualAlloc( NULL, _offset, MEM_COMMIT, PAGE_EXECUTE_READWRITE );
	memcpy( ptr, _buffer, _offset );

	// Perform fixups
	for( int n = 0; n < _referenceIndex; n++ )
	{
		Reference* r = &_referenceTable[ n ];

		int targetOffset = ( r->Label != NULL ) ? r->Label->Offset : 0;

		byte* dest = ( byte* )ptr + ( r->CodePointer - _buffer );

		switch( r->Type )
		{
		case RefRelative:
			*( ( int* )dest ) = targetOffset - r->Offset - r->Length;
			break;
		case RefCall:
			*( ( int* )dest ) -= ( ( int )ptr + r->Offset + r->Length );
			break;
		case RefAbsoluteImmediate:
			*( ( int* )dest ) = ( int )ptr + targetOffset;
			break;
		case RefAbsoluteDisplacement:
			*( ( int* )dest ) += targetOffset - r->Offset - r->Length;
			break;
		}
	}

	return ( FunctionPointer )ptr;
}

void CodeGenerator::FreeCode( FunctionPointer pointer )
{
	if( pointer != NULL )
		VirtualFree( pointer, 0, MEM_RELEASE );
}

void CodeGenerator::Reset()
{
	_offset = 0;
#ifdef SAFE
	memset( _buffer, 0, _maximumSize );
#endif

	_labelIndex = 0;
	_referenceIndex = 0;
#ifdef SAFE
	// Clearing is done by DefineLabel/AddReference to speed things up
	memset( _labelTable, 0, sizeof( Label ) * _maximumSize );
	memset( _referenceTable, 0, sizeof( Reference ) * _maximumSize );
#endif
}

Label* CodeGenerator::DefineLabel()
{
	Label* l = &_labelTable[ _labelIndex++ ];
	l->Offset = 0;
	return l;
}

void CodeGenerator::MarkLabel( Label* label )
{
#ifdef TRACE
#endif

	label->Offset = _offset;
}

Reference* CodeGenerator::ReferenceLabel( enum ReferenceType type, Label* label, int offset )
{
	Reference* r = &_referenceTable[ _referenceIndex++ ];
	r->Offset = offset;
	r->Label = label;
	r->Type = type;
	r->CodePointer = 0;
	r->Length = 0;
	return r;
}

byte* CodeGenerator::ResolveReference( Reference* reference )
{
	return 0;
}

void CodeGenerator::Encode( const int instructionId, const Operand& op1, const Operand& op2, const Operand& op3 )
{
	Instruction* instruction = &InstructionSet::Table[ instructionId ];

	_synth->Reset();

	_synth->EncodeOperand1( op1 );
	_synth->EncodeOperand2( op2 );
	_synth->EncodeOperand3( op3 );

	_synth->EncodeInstruction( instruction );

	byte* ptr = _buffer + _offset;

	Reference* target = NULL;
	Label* label = _synth->GetReference();
	if( label != NULL )
	{
		if( _synth->RelativeReference() == true )
		{
			target = this->ReferenceLabel( RefRelative, label, _offset );
			target->CodePointer = _buffer + _offset;
			target->Length = _synth->GetLength();

			int jumpOffset = /* ResolveReference( reference ) - ( int )ptr - length */0;
			_synth->SetJumpOffset( jumpOffset );
		}
		else
		{
			int address = /* ( int )ResolveReference( reference ) */0;

			// Encoded as memory reference or immediate?
			if( _synth->HasDisplacement() == true )
			{
				target = this->ReferenceLabel( RefAbsoluteDisplacement, label, _offset );
				_synth->AddDisplacement( address );
			}
			else if( _synth->HasImmediate() == true )
			{
				target = this->ReferenceLabel( RefAbsoluteImmediate, label, _offset );
				_synth->SetImmediate( address );
			}
			else
			{
				assert( false );
				return;
			}

			target->CodePointer = _buffer + _offset;
			target->Length = _synth->GetLength();
		}
	}
	else if( ( _synth->HasImmediate() == true ) &&
		( _synth->RelativeReference() == true ) )
	{
		target = this->ReferenceLabel( RefCall, NULL, _offset );
		target->CodePointer = _buffer + _offset;
		target->Length = _synth->GetLength();

		int64 callOffset = /* _synth->GetImmediate() - ( int64 )ptr - _synth->GetLength() */_synth->GetImmediate();
		_synth->SetCallOffset( ( int )callOffset );
	}

	if( target != NULL )
		_synth->SetTarget( target );

	/*if( x64 && _synth->IsRipRelative() )
	{
		int64 displacement = encoding.getDisplacement() - (int64)ptr - encoding.length(currentCode);
		encoding.setDisplacement(displacement);
	}*/

	_offset += _synth->Commit( _buffer + _offset );
}

const OperandAL CodeGenerator::al;
const OperandCL CodeGenerator::cl;
const OperandREG8 CodeGenerator::dl(CodeGen::DL);
const OperandREG8 CodeGenerator::bl(CodeGen::BL);
const OperandREG8 CodeGenerator::ah(CodeGen::AH);
const OperandREG8 CodeGenerator::ch(CodeGen::CH);
const OperandREG8 CodeGenerator::dh(CodeGen::DH);
const OperandREG8 CodeGenerator::bh(CodeGen::BH);
const OperandAL CodeGenerator::r0b;
const OperandCL CodeGenerator::r1b;
const OperandREG8 CodeGenerator::r2b(CodeGen::R2);
const OperandREG8 CodeGenerator::r3b(CodeGen::R3);
const OperandREG8 CodeGenerator::r4b(CodeGen::R4);
const OperandREG8 CodeGenerator::r5b(CodeGen::R5);
const OperandREG8 CodeGenerator::r6b(CodeGen::R6);
const OperandREG8 CodeGenerator::r7b(CodeGen::R7);
const OperandREG8 CodeGenerator::r8b(CodeGen::R8);
const OperandREG8 CodeGenerator::r9b(CodeGen::R9);
const OperandREG8 CodeGenerator::r10b(CodeGen::R10);
const OperandREG8 CodeGenerator::r11b(CodeGen::R11);
const OperandREG8 CodeGenerator::r12b(CodeGen::R12);
const OperandREG8 CodeGenerator::r13b(CodeGen::R13);
const OperandREG8 CodeGenerator::r14b(CodeGen::R14);
const OperandREG8 CodeGenerator::r15b(CodeGen::R15);

const OperandAX CodeGenerator::ax;
const OperandCX CodeGenerator::cx;
const OperandDX CodeGenerator::dx;
const OperandREG16 CodeGenerator::bx(CodeGen::BX);
const OperandREG16 CodeGenerator::sp(CodeGen::SP);
const OperandREG16 CodeGenerator::bp(CodeGen::BP);
const OperandREG16 CodeGenerator::si(CodeGen::SI);
const OperandREG16 CodeGenerator::di(CodeGen::DI);
const OperandAX CodeGenerator::r0w;
const OperandCX CodeGenerator::r1w;
const OperandDX CodeGenerator::r2w;
const OperandREG16 CodeGenerator::r3w(CodeGen::R3);
const OperandREG16 CodeGenerator::r4w(CodeGen::R4);
const OperandREG16 CodeGenerator::r5w(CodeGen::R5);
const OperandREG16 CodeGenerator::r6w(CodeGen::R6);
const OperandREG16 CodeGenerator::r7w(CodeGen::R7);
const OperandREG16 CodeGenerator::r8w(CodeGen::R8);
const OperandREG16 CodeGenerator::r9w(CodeGen::R9);
const OperandREG16 CodeGenerator::r10w(CodeGen::R10);
const OperandREG16 CodeGenerator::r11w(CodeGen::R11);
const OperandREG16 CodeGenerator::r12w(CodeGen::R12);
const OperandREG16 CodeGenerator::r13w(CodeGen::R13);
const OperandREG16 CodeGenerator::r14w(CodeGen::R14);
const OperandREG16 CodeGenerator::r15w(CodeGen::R15);

const OperandEAX CodeGenerator::eax;
const OperandECX CodeGenerator::ecx;
const OperandREG32 CodeGenerator::edx(CodeGen::EDX);
const OperandREG32 CodeGenerator::ebx(CodeGen::EBX);
const OperandREG32 CodeGenerator::esp(CodeGen::ESP);
const OperandREG32 CodeGenerator::ebp(CodeGen::EBP);
const OperandREG32 CodeGenerator::esi(CodeGen::ESI);
const OperandREG32 CodeGenerator::edi(CodeGen::EDI);
const OperandEAX CodeGenerator::r0d;
const OperandECX CodeGenerator::r1d;
const OperandREG32 CodeGenerator::r2d(CodeGen::R2);
const OperandREG32 CodeGenerator::r3d(CodeGen::R3);
const OperandREG32 CodeGenerator::r4d(CodeGen::R4);
const OperandREG32 CodeGenerator::r5d(CodeGen::R5);
const OperandREG32 CodeGenerator::r6d(CodeGen::R6);
const OperandREG32 CodeGenerator::r7d(CodeGen::R7);
const OperandREG32 CodeGenerator::r8d(CodeGen::R8);
const OperandREG32 CodeGenerator::r9d(CodeGen::R9);
const OperandREG32 CodeGenerator::r10d(CodeGen::R10);
const OperandREG32 CodeGenerator::r11d(CodeGen::R11);
const OperandREG32 CodeGenerator::r12d(CodeGen::R12);
const OperandREG32 CodeGenerator::r13d(CodeGen::R13);
const OperandREG32 CodeGenerator::r14d(CodeGen::R14);
const OperandREG32 CodeGenerator::r15d(CodeGen::R15);

const OperandREG64 CodeGenerator::rax(CodeGen::R0);
const OperandREG64 CodeGenerator::rcx(CodeGen::R1);
const OperandREG64 CodeGenerator::rdx(CodeGen::R2);
const OperandREG64 CodeGenerator::rbx(CodeGen::R3);
const OperandREG64 CodeGenerator::rsp(CodeGen::R4);
const OperandREG64 CodeGenerator::rbp(CodeGen::R5);
const OperandREG64 CodeGenerator::rsi(CodeGen::R6);
const OperandREG64 CodeGenerator::rdi(CodeGen::R7);
const OperandREG64 CodeGenerator::r0(CodeGen::R0);
const OperandREG64 CodeGenerator::r1(CodeGen::R1);
const OperandREG64 CodeGenerator::r2(CodeGen::R2);
const OperandREG64 CodeGenerator::r3(CodeGen::R3);
const OperandREG64 CodeGenerator::r4(CodeGen::R4);
const OperandREG64 CodeGenerator::r5(CodeGen::R5);
const OperandREG64 CodeGenerator::r6(CodeGen::R6);
const OperandREG64 CodeGenerator::r7(CodeGen::R7);
const OperandREG64 CodeGenerator::r8(CodeGen::R8);
const OperandREG64 CodeGenerator::r9(CodeGen::R9);
const OperandREG64 CodeGenerator::r10(CodeGen::R10);
const OperandREG64 CodeGenerator::r11(CodeGen::R11);
const OperandREG64 CodeGenerator::r12(CodeGen::R12);
const OperandREG64 CodeGenerator::r13(CodeGen::R13);
const OperandREG64 CodeGenerator::r14(CodeGen::R14);
const OperandREG64 CodeGenerator::r15(CodeGen::R15);

const OperandST0 CodeGenerator::st;
const OperandST0 CodeGenerator::st0;
const OperandFPUREG CodeGenerator::st1(CodeGen::ST1);
const OperandFPUREG CodeGenerator::st2(CodeGen::ST2);
const OperandFPUREG CodeGenerator::st3(CodeGen::ST3);
const OperandFPUREG CodeGenerator::st4(CodeGen::ST4);
const OperandFPUREG CodeGenerator::st5(CodeGen::ST5);
const OperandFPUREG CodeGenerator::st6(CodeGen::ST6);
const OperandFPUREG CodeGenerator::st7(CodeGen::ST7);

const OperandMMREG CodeGenerator::mm0(CodeGen::MM0);
const OperandMMREG CodeGenerator::mm1(CodeGen::MM1);
const OperandMMREG CodeGenerator::mm2(CodeGen::MM2);
const OperandMMREG CodeGenerator::mm3(CodeGen::MM3);
const OperandMMREG CodeGenerator::mm4(CodeGen::MM4);
const OperandMMREG CodeGenerator::mm5(CodeGen::MM5);
const OperandMMREG CodeGenerator::mm6(CodeGen::MM6);
const OperandMMREG CodeGenerator::mm7(CodeGen::MM7);

const OperandXMMREG CodeGenerator::xmm0(CodeGen::XMM0);
const OperandXMMREG CodeGenerator::xmm1(CodeGen::XMM1);
const OperandXMMREG CodeGenerator::xmm2(CodeGen::XMM2);
const OperandXMMREG CodeGenerator::xmm3(CodeGen::XMM3);
const OperandXMMREG CodeGenerator::xmm4(CodeGen::XMM4);
const OperandXMMREG CodeGenerator::xmm5(CodeGen::XMM5);
const OperandXMMREG CodeGenerator::xmm6(CodeGen::XMM6);
const OperandXMMREG CodeGenerator::xmm7(CodeGen::XMM7);

const OperandMEM8 CodeGenerator::byte_ptr;
const OperandMEM16 CodeGenerator::word_ptr;
const OperandMEM32 CodeGenerator::dword_ptr;
const OperandMEM64 CodeGenerator::mmword_ptr;
const OperandMEM64 CodeGenerator::qword_ptr;
const OperandMEM128 CodeGenerator::xmmword_ptr;
const OperandMEM128 CodeGenerator::xword_ptr;
