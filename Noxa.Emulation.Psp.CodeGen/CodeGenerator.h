// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Label.h"
#include "Operand.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace CodeGen {

				typedef void (*FunctionPointer)();

				class Synthesizer;
				struct Operand;

				class CodeGenerator
				{
				protected:
					int				_maximumSize;

					byte*			_buffer;
					int				_offset;

					int				_labelIndex;
					Label*			_labelTable;
					int				_referenceIndex;
					Reference*		_referenceTable;

					Synthesizer*	_synth;

				public:
					CodeGenerator( int maximumSize );
					~CodeGenerator();

					FunctionPointer GenerateCode();
					void FreeCode( FunctionPointer pointer );
					void Reset();

					Label* DefineLabel();
					void MarkLabel( Label* label );

				protected:
					Reference* ReferenceLabel( enum ReferenceType type, Label* label, int offset );
					byte* ResolveReference( Reference* reference );
					void Encode( const int instructionId,
						const Operand& op1 = OperandVOID(),
						const Operand& op2 = OperandVOID(),
						const Operand& op3 = OperandVOID() );

				public:
					static const OperandAL al;
					static const OperandCL cl;
					static const OperandREG8 dl;
					static const OperandREG8 bl;
					static const OperandREG8 ah;
					static const OperandREG8 ch;
					static const OperandREG8 dh;
					static const OperandREG8 bh;
					static const OperandAL r0b;
					static const OperandCL r1b;
					static const OperandREG8 r2b;
					static const OperandREG8 r3b;
					static const OperandREG8 r4b;
					static const OperandREG8 r5b;
					static const OperandREG8 r6b;
					static const OperandREG8 r7b;
					static const OperandREG8 r8b;
					static const OperandREG8 r9b;
					static const OperandREG8 r10b;
					static const OperandREG8 r11b;
					static const OperandREG8 r12b;
					static const OperandREG8 r13b;
					static const OperandREG8 r14b;
					static const OperandREG8 r15b;

					static const OperandAX ax;
					static const OperandCX cx;
					static const OperandDX dx;
					static const OperandREG16 bx;
					static const OperandREG16 sp;
					static const OperandREG16 bp;
					static const OperandREG16 si;
					static const OperandREG16 di;
					static const OperandAX r0w;
					static const OperandCX r1w;
					static const OperandDX r2w;
					static const OperandREG16 r3w;
					static const OperandREG16 r4w;
					static const OperandREG16 r5w;
					static const OperandREG16 r6w;
					static const OperandREG16 r7w;
					static const OperandREG16 r8w;
					static const OperandREG16 r9w;
					static const OperandREG16 r10w;
					static const OperandREG16 r11w;
					static const OperandREG16 r12w;
					static const OperandREG16 r13w;
					static const OperandREG16 r14w;
					static const OperandREG16 r15w;

					static const OperandEAX eax;
					static const OperandECX ecx;
					static const OperandREG32 edx;
					static const OperandREG32 ebx;
					static const OperandREG32 esp;
					static const OperandREG32 ebp;
					static const OperandREG32 esi;
					static const OperandREG32 edi;
					static const OperandEAX r0d;
					static const OperandECX r1d;
					static const OperandREG32 r2d;
					static const OperandREG32 r3d;
					static const OperandREG32 r4d;
					static const OperandREG32 r5d;
					static const OperandREG32 r6d;
					static const OperandREG32 r7d;
					static const OperandREG32 r8d;
					static const OperandREG32 r9d;
					static const OperandREG32 r10d;
					static const OperandREG32 r11d;
					static const OperandREG32 r12d;
					static const OperandREG32 r13d;
					static const OperandREG32 r14d;
					static const OperandREG32 r15d;

					static const OperandREG64 rax;
					static const OperandREG64 rcx;
					static const OperandREG64 rdx;
					static const OperandREG64 rbx;
					static const OperandREG64 rsp;
					static const OperandREG64 rbp;
					static const OperandREG64 rsi;
					static const OperandREG64 rdi;
					static const OperandREG64 r0;
					static const OperandREG64 r1;
					static const OperandREG64 r2;
					static const OperandREG64 r3;
					static const OperandREG64 r4;
					static const OperandREG64 r5;
					static const OperandREG64 r6;
					static const OperandREG64 r7;
					static const OperandREG64 r8;
					static const OperandREG64 r9;
					static const OperandREG64 r10;
					static const OperandREG64 r11;
					static const OperandREG64 r12;
					static const OperandREG64 r13;
					static const OperandREG64 r14;
					static const OperandREG64 r15;

					static const OperandST0 st;
					static const OperandST0 st0;
					static const OperandFPUREG st1;
					static const OperandFPUREG st2;
					static const OperandFPUREG st3;
					static const OperandFPUREG st4;
					static const OperandFPUREG st5;
					static const OperandFPUREG st6;
					static const OperandFPUREG st7;

					static const OperandMMREG mm0;
					static const OperandMMREG mm1;
					static const OperandMMREG mm2;
					static const OperandMMREG mm3;
					static const OperandMMREG mm4;
					static const OperandMMREG mm5;
					static const OperandMMREG mm6;
					static const OperandMMREG mm7;

					static const OperandXMMREG xmm0;
					static const OperandXMMREG xmm1;
					static const OperandXMMREG xmm2;
					static const OperandXMMREG xmm3;
					static const OperandXMMREG xmm4;
					static const OperandXMMREG xmm5;
					static const OperandXMMREG xmm6;
					static const OperandXMMREG xmm7;

					static const OperandMEM8 byte_ptr;
					static const OperandMEM16 word_ptr;
					static const OperandMEM32 dword_ptr;
					static const OperandMEM64 mmword_ptr;
					static const OperandMEM64 qword_ptr;
					static const OperandMEM128 xmmword_ptr;
					static const OperandMEM128 xword_ptr;

				public:
					typedef OperandIMM IMM;
					typedef OperandAL AL;
					typedef OperandAX AX;
					typedef OperandEAX EAX;
					typedef OperandRAX RAX;
					typedef OperandDX DX;
					typedef OperandCL CL;
					typedef OperandCX CX;
					typedef OperandECX ECX;
					typedef OperandST0 ST0;
					typedef OperandREG8 REG8;
					typedef OperandREG16 REG16;
					typedef OperandREG32 REG32;
					typedef OperandREG64 REG64;
					typedef OperandFPUREG FPUREG;
					typedef OperandMMREG MMREG;
					typedef OperandXMMREG XMMREG;
					typedef OperandMEM8 MEM8;
					typedef OperandMEM16 MEM16;
					typedef OperandMEM32 MEM32;
					typedef OperandMEM64 MEM64;
					typedef OperandMEM128 MEM128;
					typedef OperandR_M8 R_M8;
					typedef OperandR_M16 R_M16;
					typedef OperandR_M32 R_M32;
					typedef OperandR_M64 R_M64;
					typedef OperandR_M128 R_M128;
					typedef OperandXMM32 XMM32;
					typedef OperandXMM64 XMM64;
					typedef OperandMM64 MM64;
					typedef OperandREF REF;

					#define enc void

					enc aaa(){Encode(0);}
					enc aad(){Encode(1);}
					enc aad(REF a){Encode(2,a);}
					enc aad(dword a){Encode(2,(IMM)a);}
					enc aam(){Encode(3);}
					enc aam(REF a){Encode(4,a);}
					enc aam(dword a){Encode(4,(IMM)a);}
					enc aas(){Encode(5);}
					enc adc(REG8 a,REG8 b){Encode(6,a,b);}
					enc adc(MEM8 a,REG8 b){Encode(6,a,b);}
					enc adc(R_M8 a,REG8 b){Encode(6,a,b);}
					enc adc(REG16 a,REG16 b){Encode(7,a,b);}
					enc adc(MEM16 a,REG16 b){Encode(7,a,b);}
					enc adc(R_M16 a,REG16 b){Encode(7,a,b);}
					enc adc(REG32 a,REG32 b){Encode(8,a,b);}
					enc adc(MEM32 a,REG32 b){Encode(8,a,b);}
					enc adc(R_M32 a,REG32 b){Encode(8,a,b);}
					enc adc(REG64 a,REG64 b){Encode(9,a,b);}
					enc adc(MEM64 a,REG64 b){Encode(9,a,b);}
					enc adc(R_M64 a,REG64 b){Encode(9,a,b);}
					enc adc(REG8 a,MEM8 b){Encode(10,a,b);}
					enc adc(REG8 a,R_M8 b){Encode(10,a,b);}
					enc adc(REG16 a,MEM16 b){Encode(11,a,b);}
					enc adc(REG16 a,R_M16 b){Encode(11,a,b);}
					enc adc(REG32 a,MEM32 b){Encode(12,a,b);}
					enc adc(REG32 a,R_M32 b){Encode(12,a,b);}
					enc adc(REG64 a,MEM64 b){Encode(13,a,b);}
					enc adc(REG64 a,R_M64 b){Encode(13,a,b);}
					enc adc(REG8 a,byte b){Encode(14,a,(IMM)b);}
					enc adc(AL a,byte b){Encode(14,a,(IMM)b);}
					enc adc(CL a,byte b){Encode(14,a,(IMM)b);}
					enc adc(MEM8 a,byte b){Encode(14,a,(IMM)b);}
					enc adc(R_M8 a,byte b){Encode(14,a,(IMM)b);}
					enc adc(REG16 a,byte b){Encode(15,a,(IMM)b);}
					enc adc(REG16 a,word b){Encode(15,a,(IMM)b);}
					enc adc(MEM16 a,byte b){Encode(15,a,(IMM)b);}
					enc adc(MEM16 a,word b){Encode(15,a,(IMM)b);}
					enc adc(R_M16 a,byte b){Encode(15,a,(IMM)b);}
					enc adc(R_M16 a,word b){Encode(15,a,(IMM)b);}
					enc adc(REG32 a,REF b){Encode(16,a,b);}
					enc adc(REG32 a,dword b){Encode(16,a,(IMM)b);}
					enc adc(MEM32 a,REF b){Encode(16,a,b);}
					enc adc(MEM32 a,dword b){Encode(16,a,(IMM)b);}
					enc adc(R_M32 a,REF b){Encode(16,a,b);}
					enc adc(R_M32 a,dword b){Encode(16,a,(IMM)b);}
					enc adc(REG64 a,REF b){Encode(17,a,b);}
					enc adc(REG64 a,dword b){Encode(17,a,(IMM)b);}
					enc adc(MEM64 a,REF b){Encode(17,a,b);}
					enc adc(MEM64 a,dword b){Encode(17,a,(IMM)b);}
					enc adc(R_M64 a,REF b){Encode(17,a,b);}
					enc adc(R_M64 a,dword b){Encode(17,a,(IMM)b);}
					enc add(REG8 a,REG8 b){Encode(25,a,b);}
					enc add(MEM8 a,REG8 b){Encode(25,a,b);}
					enc add(R_M8 a,REG8 b){Encode(25,a,b);}
					enc add(REG16 a,REG16 b){Encode(26,a,b);}
					enc add(MEM16 a,REG16 b){Encode(26,a,b);}
					enc add(R_M16 a,REG16 b){Encode(26,a,b);}
					enc add(REG32 a,REG32 b){Encode(27,a,b);}
					enc add(MEM32 a,REG32 b){Encode(27,a,b);}
					enc add(R_M32 a,REG32 b){Encode(27,a,b);}
					enc add(REG64 a,REG64 b){Encode(28,a,b);}
					enc add(MEM64 a,REG64 b){Encode(28,a,b);}
					enc add(R_M64 a,REG64 b){Encode(28,a,b);}
					enc add(REG8 a,MEM8 b){Encode(29,a,b);}
					enc add(REG8 a,R_M8 b){Encode(29,a,b);}
					enc add(REG16 a,MEM16 b){Encode(30,a,b);}
					enc add(REG16 a,R_M16 b){Encode(30,a,b);}
					enc add(REG32 a,MEM32 b){Encode(31,a,b);}
					enc add(REG32 a,R_M32 b){Encode(31,a,b);}
					enc add(REG64 a,MEM64 b){Encode(32,a,b);}
					enc add(REG64 a,R_M64 b){Encode(32,a,b);}
					enc add(REG8 a,byte b){Encode(33,a,(IMM)b);}
					enc add(AL a,byte b){Encode(33,a,(IMM)b);}
					enc add(CL a,byte b){Encode(33,a,(IMM)b);}
					enc add(MEM8 a,byte b){Encode(33,a,(IMM)b);}
					enc add(R_M8 a,byte b){Encode(33,a,(IMM)b);}
					enc add(REG16 a,byte b){Encode(34,a,(IMM)b);}
					enc add(REG16 a,word b){Encode(34,a,(IMM)b);}
					enc add(MEM16 a,byte b){Encode(34,a,(IMM)b);}
					enc add(MEM16 a,word b){Encode(34,a,(IMM)b);}
					enc add(R_M16 a,byte b){Encode(34,a,(IMM)b);}
					enc add(R_M16 a,word b){Encode(34,a,(IMM)b);}
					enc add(REG32 a,REF b){Encode(35,a,b);}
					enc add(REG32 a,dword b){Encode(35,a,(IMM)b);}
					enc add(MEM32 a,REF b){Encode(35,a,b);}
					enc add(MEM32 a,dword b){Encode(35,a,(IMM)b);}
					enc add(R_M32 a,REF b){Encode(35,a,b);}
					enc add(R_M32 a,dword b){Encode(35,a,(IMM)b);}
					enc add(REG64 a,REF b){Encode(36,a,b);}
					enc add(REG64 a,dword b){Encode(36,a,(IMM)b);}
					enc add(MEM64 a,REF b){Encode(36,a,b);}
					enc add(MEM64 a,dword b){Encode(36,a,(IMM)b);}
					enc add(R_M64 a,REF b){Encode(36,a,b);}
					enc add(R_M64 a,dword b){Encode(36,a,(IMM)b);}
					enc addpd(XMMREG a,XMMREG b){Encode(44,a,b);}
					enc addpd(XMMREG a,MEM128 b){Encode(44,a,b);}
					enc addpd(XMMREG a,R_M128 b){Encode(44,a,b);}
					enc addps(XMMREG a,XMMREG b){Encode(45,a,b);}
					enc addps(XMMREG a,MEM128 b){Encode(45,a,b);}
					enc addps(XMMREG a,R_M128 b){Encode(45,a,b);}
					enc addsd(XMMREG a,XMMREG b){Encode(46,a,b);}
					enc addsd(XMMREG a,MEM64 b){Encode(46,a,b);}
					enc addsd(XMMREG a,XMM64 b){Encode(46,a,b);}
					enc addss(XMMREG a,XMMREG b){Encode(47,a,b);}
					enc addss(XMMREG a,MEM32 b){Encode(47,a,b);}
					enc addss(XMMREG a,XMM32 b){Encode(47,a,b);}
					enc addsubpd(XMMREG a,XMMREG b){Encode(48,a,b);}
					enc addsubpd(XMMREG a,MEM128 b){Encode(48,a,b);}
					enc addsubpd(XMMREG a,R_M128 b){Encode(48,a,b);}
					enc addsubps(XMMREG a,XMMREG b){Encode(49,a,b);}
					enc addsubps(XMMREG a,MEM128 b){Encode(49,a,b);}
					enc addsubps(XMMREG a,R_M128 b){Encode(49,a,b);}
					enc align(REF a){Encode(50,a);}
					enc align(dword a){Encode(50,(IMM)a);}
					enc and(REG8 a,REG8 b){Encode(51,a,b);}
					enc and(MEM8 a,REG8 b){Encode(51,a,b);}
					enc and(R_M8 a,REG8 b){Encode(51,a,b);}
					enc and(REG16 a,REG16 b){Encode(52,a,b);}
					enc and(MEM16 a,REG16 b){Encode(52,a,b);}
					enc and(R_M16 a,REG16 b){Encode(52,a,b);}
					enc and(REG32 a,REG32 b){Encode(53,a,b);}
					enc and(MEM32 a,REG32 b){Encode(53,a,b);}
					enc and(R_M32 a,REG32 b){Encode(53,a,b);}
					enc and(REG64 a,REG64 b){Encode(54,a,b);}
					enc and(MEM64 a,REG64 b){Encode(54,a,b);}
					enc and(R_M64 a,REG64 b){Encode(54,a,b);}
					enc and(REG8 a,MEM8 b){Encode(55,a,b);}
					enc and(REG8 a,R_M8 b){Encode(55,a,b);}
					enc and(REG16 a,MEM16 b){Encode(56,a,b);}
					enc and(REG16 a,R_M16 b){Encode(56,a,b);}
					enc and(REG32 a,MEM32 b){Encode(57,a,b);}
					enc and(REG32 a,R_M32 b){Encode(57,a,b);}
					enc and(REG64 a,MEM64 b){Encode(58,a,b);}
					enc and(REG64 a,R_M64 b){Encode(58,a,b);}
					enc and(REG8 a,byte b){Encode(59,a,(IMM)b);}
					enc and(AL a,byte b){Encode(59,a,(IMM)b);}
					enc and(CL a,byte b){Encode(59,a,(IMM)b);}
					enc and(MEM8 a,byte b){Encode(59,a,(IMM)b);}
					enc and(R_M8 a,byte b){Encode(59,a,(IMM)b);}
					enc and(REG16 a,byte b){Encode(60,a,(IMM)b);}
					enc and(REG16 a,word b){Encode(60,a,(IMM)b);}
					enc and(MEM16 a,byte b){Encode(60,a,(IMM)b);}
					enc and(MEM16 a,word b){Encode(60,a,(IMM)b);}
					enc and(R_M16 a,byte b){Encode(60,a,(IMM)b);}
					enc and(R_M16 a,word b){Encode(60,a,(IMM)b);}
					enc and(REG32 a,REF b){Encode(61,a,b);}
					enc and(REG32 a,dword b){Encode(61,a,(IMM)b);}
					enc and(MEM32 a,REF b){Encode(61,a,b);}
					enc and(MEM32 a,dword b){Encode(61,a,(IMM)b);}
					enc and(R_M32 a,REF b){Encode(61,a,b);}
					enc and(R_M32 a,dword b){Encode(61,a,(IMM)b);}
					enc and(REG64 a,REF b){Encode(62,a,b);}
					enc and(REG64 a,dword b){Encode(62,a,(IMM)b);}
					enc and(MEM64 a,REF b){Encode(62,a,b);}
					enc and(MEM64 a,dword b){Encode(62,a,(IMM)b);}
					enc and(R_M64 a,REF b){Encode(62,a,b);}
					enc and(R_M64 a,dword b){Encode(62,a,(IMM)b);}
					enc andnpd(XMMREG a,XMMREG b){Encode(70,a,b);}
					enc andnpd(XMMREG a,MEM128 b){Encode(70,a,b);}
					enc andnpd(XMMREG a,R_M128 b){Encode(70,a,b);}
					enc andnps(XMMREG a,XMMREG b){Encode(71,a,b);}
					enc andnps(XMMREG a,MEM128 b){Encode(71,a,b);}
					enc andnps(XMMREG a,R_M128 b){Encode(71,a,b);}
					enc andpd(XMMREG a,XMMREG b){Encode(72,a,b);}
					enc andpd(XMMREG a,MEM128 b){Encode(72,a,b);}
					enc andpd(XMMREG a,R_M128 b){Encode(72,a,b);}
					enc andps(XMMREG a,XMMREG b){Encode(73,a,b);}
					enc andps(XMMREG a,MEM128 b){Encode(73,a,b);}
					enc andps(XMMREG a,R_M128 b){Encode(73,a,b);}
					enc bound(REG16 a,MEM8 b){Encode(74,a,b);}
					enc bound(REG16 a,MEM16 b){Encode(74,a,b);}
					enc bound(REG16 a,MEM32 b){Encode(74,a,b);}
					enc bound(REG16 a,MEM64 b){Encode(74,a,b);}
					enc bound(REG16 a,MEM128 b){Encode(74,a,b);}
					enc bound(REG32 a,MEM8 b){Encode(75,a,b);}
					enc bound(REG32 a,MEM16 b){Encode(75,a,b);}
					enc bound(REG32 a,MEM32 b){Encode(75,a,b);}
					enc bound(REG32 a,MEM64 b){Encode(75,a,b);}
					enc bound(REG32 a,MEM128 b){Encode(75,a,b);}
					enc bsf(REG16 a,REG16 b){Encode(76,a,b);}
					enc bsf(REG16 a,MEM16 b){Encode(76,a,b);}
					enc bsf(REG16 a,R_M16 b){Encode(76,a,b);}
					enc bsf(REG32 a,REG32 b){Encode(77,a,b);}
					enc bsf(REG32 a,MEM32 b){Encode(77,a,b);}
					enc bsf(REG32 a,R_M32 b){Encode(77,a,b);}
					enc bsf(REG64 a,REG64 b){Encode(78,a,b);}
					enc bsf(REG64 a,MEM64 b){Encode(78,a,b);}
					enc bsf(REG64 a,R_M64 b){Encode(78,a,b);}
					enc bsr(REG16 a,REG16 b){Encode(79,a,b);}
					enc bsr(REG16 a,MEM16 b){Encode(79,a,b);}
					enc bsr(REG16 a,R_M16 b){Encode(79,a,b);}
					enc bsr(REG32 a,REG32 b){Encode(80,a,b);}
					enc bsr(REG32 a,MEM32 b){Encode(80,a,b);}
					enc bsr(REG32 a,R_M32 b){Encode(80,a,b);}
					enc bsr(REG64 a,REG64 b){Encode(81,a,b);}
					enc bsr(REG64 a,MEM64 b){Encode(81,a,b);}
					enc bsr(REG64 a,R_M64 b){Encode(81,a,b);}
					enc bswap(REG32 a){Encode(82,a);}
					enc bswap(REG64 a){Encode(83,a);}
					enc bt(REG16 a,REG16 b){Encode(84,a,b);}
					enc bt(MEM16 a,REG16 b){Encode(84,a,b);}
					enc bt(R_M16 a,REG16 b){Encode(84,a,b);}
					enc bt(REG32 a,REG32 b){Encode(85,a,b);}
					enc bt(MEM32 a,REG32 b){Encode(85,a,b);}
					enc bt(R_M32 a,REG32 b){Encode(85,a,b);}
					enc bt(REG64 a,REG64 b){Encode(86,a,b);}
					enc bt(MEM64 a,REG64 b){Encode(86,a,b);}
					enc bt(R_M64 a,REG64 b){Encode(86,a,b);}
					enc bt(REG16 a,byte b){Encode(87,a,(IMM)b);}
					enc bt(MEM16 a,byte b){Encode(87,a,(IMM)b);}
					enc bt(R_M16 a,byte b){Encode(87,a,(IMM)b);}
					enc bt(REG32 a,byte b){Encode(88,a,(IMM)b);}
					enc bt(MEM32 a,byte b){Encode(88,a,(IMM)b);}
					enc bt(R_M32 a,byte b){Encode(88,a,(IMM)b);}
					enc bt(REG64 a,byte b){Encode(89,a,(IMM)b);}
					enc bt(RAX a,byte b){Encode(89,a,(IMM)b);}
					enc bt(MEM64 a,byte b){Encode(89,a,(IMM)b);}
					enc bt(R_M64 a,byte b){Encode(89,a,(IMM)b);}
					enc btc(REG16 a,REG16 b){Encode(90,a,b);}
					enc btc(MEM16 a,REG16 b){Encode(90,a,b);}
					enc btc(R_M16 a,REG16 b){Encode(90,a,b);}
					enc btc(REG32 a,REG32 b){Encode(91,a,b);}
					enc btc(MEM32 a,REG32 b){Encode(91,a,b);}
					enc btc(R_M32 a,REG32 b){Encode(91,a,b);}
					enc btc(REG64 a,REG64 b){Encode(92,a,b);}
					enc btc(MEM64 a,REG64 b){Encode(92,a,b);}
					enc btc(R_M64 a,REG64 b){Encode(92,a,b);}
					enc btc(REG16 a,byte b){Encode(93,a,(IMM)b);}
					enc btc(MEM16 a,byte b){Encode(93,a,(IMM)b);}
					enc btc(R_M16 a,byte b){Encode(93,a,(IMM)b);}
					enc btc(REG32 a,byte b){Encode(94,a,(IMM)b);}
					enc btc(MEM32 a,byte b){Encode(94,a,(IMM)b);}
					enc btc(R_M32 a,byte b){Encode(94,a,(IMM)b);}
					enc btc(REG64 a,byte b){Encode(95,a,(IMM)b);}
					enc btc(RAX a,byte b){Encode(95,a,(IMM)b);}
					enc btc(MEM64 a,byte b){Encode(95,a,(IMM)b);}
					enc btc(R_M64 a,byte b){Encode(95,a,(IMM)b);}
					enc btr(REG16 a,REG16 b){Encode(96,a,b);}
					enc btr(MEM16 a,REG16 b){Encode(96,a,b);}
					enc btr(R_M16 a,REG16 b){Encode(96,a,b);}
					enc btr(REG32 a,REG32 b){Encode(97,a,b);}
					enc btr(MEM32 a,REG32 b){Encode(97,a,b);}
					enc btr(R_M32 a,REG32 b){Encode(97,a,b);}
					enc btr(REG64 a,REG64 b){Encode(98,a,b);}
					enc btr(MEM64 a,REG64 b){Encode(98,a,b);}
					enc btr(R_M64 a,REG64 b){Encode(98,a,b);}
					enc btr(REG16 a,byte b){Encode(99,a,(IMM)b);}
					enc btr(MEM16 a,byte b){Encode(99,a,(IMM)b);}
					enc btr(R_M16 a,byte b){Encode(99,a,(IMM)b);}
					enc btr(REG32 a,byte b){Encode(100,a,(IMM)b);}
					enc btr(MEM32 a,byte b){Encode(100,a,(IMM)b);}
					enc btr(R_M32 a,byte b){Encode(100,a,(IMM)b);}
					enc btr(REG64 a,byte b){Encode(101,a,(IMM)b);}
					enc btr(RAX a,byte b){Encode(101,a,(IMM)b);}
					enc btr(MEM64 a,byte b){Encode(101,a,(IMM)b);}
					enc btr(R_M64 a,byte b){Encode(101,a,(IMM)b);}
					enc bts(REG16 a,REG16 b){Encode(102,a,b);}
					enc bts(MEM16 a,REG16 b){Encode(102,a,b);}
					enc bts(R_M16 a,REG16 b){Encode(102,a,b);}
					enc bts(REG32 a,REG32 b){Encode(103,a,b);}
					enc bts(MEM32 a,REG32 b){Encode(103,a,b);}
					enc bts(R_M32 a,REG32 b){Encode(103,a,b);}
					enc bts(REG64 a,REG64 b){Encode(104,a,b);}
					enc bts(MEM64 a,REG64 b){Encode(104,a,b);}
					enc bts(R_M64 a,REG64 b){Encode(104,a,b);}
					enc bts(REG16 a,byte b){Encode(105,a,(IMM)b);}
					enc bts(MEM16 a,byte b){Encode(105,a,(IMM)b);}
					enc bts(R_M16 a,byte b){Encode(105,a,(IMM)b);}
					enc bts(REG32 a,byte b){Encode(106,a,(IMM)b);}
					enc bts(MEM32 a,byte b){Encode(106,a,(IMM)b);}
					enc bts(R_M32 a,byte b){Encode(106,a,(IMM)b);}
					enc bts(REG64 a,byte b){Encode(107,a,(IMM)b);}
					enc bts(RAX a,byte b){Encode(107,a,(IMM)b);}
					enc bts(MEM64 a,byte b){Encode(107,a,(IMM)b);}
					enc bts(R_M64 a,byte b){Encode(107,a,(IMM)b);}
					enc call(REF a){Encode(108,a);}
					enc call(dword a){Encode(108,(IMM)a);}
					enc call(REG16 a){Encode(109,a);}
					enc call(MEM16 a){Encode(109,a);}
					enc call(R_M16 a){Encode(109,a);}
					enc call(REG32 a){Encode(110,a);}
					enc call(MEM32 a){Encode(110,a);}
					enc call(R_M32 a){Encode(110,a);}
					enc call(REG64 a){Encode(111,a);}
					enc call(MEM64 a){Encode(111,a);}
					enc call(R_M64 a){Encode(111,a);}
					enc cbw(){Encode(112);}
					enc cdq(){Encode(113);}
					enc cdqe(){Encode(114);}
					enc clc(){Encode(115);}
					enc cld(){Encode(116);}
					enc clflush(MEM8 a){Encode(117,a);}
					enc clflush(MEM16 a){Encode(117,a);}
					enc clflush(MEM32 a){Encode(117,a);}
					enc clflush(MEM64 a){Encode(117,a);}
					enc clflush(MEM128 a){Encode(117,a);}
					enc cli(){Encode(118);}
					enc cmc(){Encode(119);}
					enc cmova(REG16 a,REG16 b){Encode(120,a,b);}
					enc cmova(REG16 a,MEM16 b){Encode(120,a,b);}
					enc cmova(REG16 a,R_M16 b){Encode(120,a,b);}
					enc cmova(REG32 a,REG32 b){Encode(121,a,b);}
					enc cmova(REG32 a,MEM32 b){Encode(121,a,b);}
					enc cmova(REG32 a,R_M32 b){Encode(121,a,b);}
					enc cmova(REG64 a,REG64 b){Encode(122,a,b);}
					enc cmova(REG64 a,MEM64 b){Encode(122,a,b);}
					enc cmova(REG64 a,R_M64 b){Encode(122,a,b);}
					enc cmovae(REG16 a,REG16 b){Encode(123,a,b);}
					enc cmovae(REG16 a,MEM16 b){Encode(123,a,b);}
					enc cmovae(REG16 a,R_M16 b){Encode(123,a,b);}
					enc cmovae(REG32 a,REG32 b){Encode(124,a,b);}
					enc cmovae(REG32 a,MEM32 b){Encode(124,a,b);}
					enc cmovae(REG32 a,R_M32 b){Encode(124,a,b);}
					enc cmovae(REG64 a,REG64 b){Encode(125,a,b);}
					enc cmovae(REG64 a,MEM64 b){Encode(125,a,b);}
					enc cmovae(REG64 a,R_M64 b){Encode(125,a,b);}
					enc cmovb(REG16 a,REG16 b){Encode(126,a,b);}
					enc cmovb(REG16 a,MEM16 b){Encode(126,a,b);}
					enc cmovb(REG16 a,R_M16 b){Encode(126,a,b);}
					enc cmovb(REG32 a,REG32 b){Encode(127,a,b);}
					enc cmovb(REG32 a,MEM32 b){Encode(127,a,b);}
					enc cmovb(REG32 a,R_M32 b){Encode(127,a,b);}
					enc cmovb(REG64 a,REG64 b){Encode(128,a,b);}
					enc cmovb(REG64 a,MEM64 b){Encode(128,a,b);}
					enc cmovb(REG64 a,R_M64 b){Encode(128,a,b);}
					enc cmovbe(REG16 a,REG16 b){Encode(129,a,b);}
					enc cmovbe(REG16 a,MEM16 b){Encode(129,a,b);}
					enc cmovbe(REG16 a,R_M16 b){Encode(129,a,b);}
					enc cmovbe(REG32 a,REG32 b){Encode(130,a,b);}
					enc cmovbe(REG32 a,MEM32 b){Encode(130,a,b);}
					enc cmovbe(REG32 a,R_M32 b){Encode(130,a,b);}
					enc cmovbe(REG64 a,REG64 b){Encode(131,a,b);}
					enc cmovbe(REG64 a,MEM64 b){Encode(131,a,b);}
					enc cmovbe(REG64 a,R_M64 b){Encode(131,a,b);}
					enc cmovc(REG16 a,REG16 b){Encode(132,a,b);}
					enc cmovc(REG16 a,MEM16 b){Encode(132,a,b);}
					enc cmovc(REG16 a,R_M16 b){Encode(132,a,b);}
					enc cmovc(REG32 a,REG32 b){Encode(133,a,b);}
					enc cmovc(REG32 a,MEM32 b){Encode(133,a,b);}
					enc cmovc(REG32 a,R_M32 b){Encode(133,a,b);}
					enc cmovc(REG64 a,REG64 b){Encode(134,a,b);}
					enc cmovc(REG64 a,MEM64 b){Encode(134,a,b);}
					enc cmovc(REG64 a,R_M64 b){Encode(134,a,b);}
					enc cmove(REG16 a,REG16 b){Encode(135,a,b);}
					enc cmove(REG16 a,MEM16 b){Encode(135,a,b);}
					enc cmove(REG16 a,R_M16 b){Encode(135,a,b);}
					enc cmove(REG32 a,REG32 b){Encode(136,a,b);}
					enc cmove(REG32 a,MEM32 b){Encode(136,a,b);}
					enc cmove(REG32 a,R_M32 b){Encode(136,a,b);}
					enc cmove(REG64 a,REG64 b){Encode(137,a,b);}
					enc cmove(REG64 a,MEM64 b){Encode(137,a,b);}
					enc cmove(REG64 a,R_M64 b){Encode(137,a,b);}
					enc cmovg(REG16 a,REG16 b){Encode(138,a,b);}
					enc cmovg(REG16 a,MEM16 b){Encode(138,a,b);}
					enc cmovg(REG16 a,R_M16 b){Encode(138,a,b);}
					enc cmovg(REG32 a,REG32 b){Encode(139,a,b);}
					enc cmovg(REG32 a,MEM32 b){Encode(139,a,b);}
					enc cmovg(REG32 a,R_M32 b){Encode(139,a,b);}
					enc cmovg(REG64 a,REG64 b){Encode(140,a,b);}
					enc cmovg(REG64 a,MEM64 b){Encode(140,a,b);}
					enc cmovg(REG64 a,R_M64 b){Encode(140,a,b);}
					enc cmovge(REG16 a,REG16 b){Encode(141,a,b);}
					enc cmovge(REG16 a,MEM16 b){Encode(141,a,b);}
					enc cmovge(REG16 a,R_M16 b){Encode(141,a,b);}
					enc cmovge(REG32 a,REG32 b){Encode(142,a,b);}
					enc cmovge(REG32 a,MEM32 b){Encode(142,a,b);}
					enc cmovge(REG32 a,R_M32 b){Encode(142,a,b);}
					enc cmovge(REG64 a,REG64 b){Encode(143,a,b);}
					enc cmovge(REG64 a,MEM64 b){Encode(143,a,b);}
					enc cmovge(REG64 a,R_M64 b){Encode(143,a,b);}
					enc cmovl(REG16 a,REG16 b){Encode(144,a,b);}
					enc cmovl(REG16 a,MEM16 b){Encode(144,a,b);}
					enc cmovl(REG16 a,R_M16 b){Encode(144,a,b);}
					enc cmovl(REG32 a,REG32 b){Encode(145,a,b);}
					enc cmovl(REG32 a,MEM32 b){Encode(145,a,b);}
					enc cmovl(REG32 a,R_M32 b){Encode(145,a,b);}
					enc cmovl(REG64 a,REG64 b){Encode(146,a,b);}
					enc cmovl(REG64 a,MEM64 b){Encode(146,a,b);}
					enc cmovl(REG64 a,R_M64 b){Encode(146,a,b);}
					enc cmovle(REG16 a,REG16 b){Encode(147,a,b);}
					enc cmovle(REG16 a,MEM16 b){Encode(147,a,b);}
					enc cmovle(REG16 a,R_M16 b){Encode(147,a,b);}
					enc cmovle(REG32 a,REG32 b){Encode(148,a,b);}
					enc cmovle(REG32 a,MEM32 b){Encode(148,a,b);}
					enc cmovle(REG32 a,R_M32 b){Encode(148,a,b);}
					enc cmovle(REG64 a,REG64 b){Encode(149,a,b);}
					enc cmovle(REG64 a,MEM64 b){Encode(149,a,b);}
					enc cmovle(REG64 a,R_M64 b){Encode(149,a,b);}
					enc cmovna(REG16 a,REG16 b){Encode(150,a,b);}
					enc cmovna(REG16 a,MEM16 b){Encode(150,a,b);}
					enc cmovna(REG16 a,R_M16 b){Encode(150,a,b);}
					enc cmovna(REG32 a,REG32 b){Encode(151,a,b);}
					enc cmovna(REG32 a,MEM32 b){Encode(151,a,b);}
					enc cmovna(REG32 a,R_M32 b){Encode(151,a,b);}
					enc cmovna(REG64 a,REG64 b){Encode(152,a,b);}
					enc cmovna(REG64 a,MEM64 b){Encode(152,a,b);}
					enc cmovna(REG64 a,R_M64 b){Encode(152,a,b);}
					enc cmovnb(REG16 a,REG16 b){Encode(153,a,b);}
					enc cmovnb(REG16 a,MEM16 b){Encode(153,a,b);}
					enc cmovnb(REG16 a,R_M16 b){Encode(153,a,b);}
					enc cmovnb(REG32 a,REG32 b){Encode(154,a,b);}
					enc cmovnb(REG32 a,MEM32 b){Encode(154,a,b);}
					enc cmovnb(REG32 a,R_M32 b){Encode(154,a,b);}
					enc cmovnb(REG64 a,REG64 b){Encode(155,a,b);}
					enc cmovnb(REG64 a,MEM64 b){Encode(155,a,b);}
					enc cmovnb(REG64 a,R_M64 b){Encode(155,a,b);}
					enc cmovnbe(REG16 a,REG16 b){Encode(156,a,b);}
					enc cmovnbe(REG16 a,MEM16 b){Encode(156,a,b);}
					enc cmovnbe(REG16 a,R_M16 b){Encode(156,a,b);}
					enc cmovnbe(REG32 a,REG32 b){Encode(157,a,b);}
					enc cmovnbe(REG32 a,MEM32 b){Encode(157,a,b);}
					enc cmovnbe(REG32 a,R_M32 b){Encode(157,a,b);}
					enc cmovnbe(REG64 a,REG64 b){Encode(158,a,b);}
					enc cmovnbe(REG64 a,MEM64 b){Encode(158,a,b);}
					enc cmovnbe(REG64 a,R_M64 b){Encode(158,a,b);}
					enc cmovnc(REG16 a,REG16 b){Encode(159,a,b);}
					enc cmovnc(REG16 a,MEM16 b){Encode(159,a,b);}
					enc cmovnc(REG16 a,R_M16 b){Encode(159,a,b);}
					enc cmovnc(REG32 a,REG32 b){Encode(160,a,b);}
					enc cmovnc(REG32 a,MEM32 b){Encode(160,a,b);}
					enc cmovnc(REG32 a,R_M32 b){Encode(160,a,b);}
					enc cmovnc(REG64 a,REG64 b){Encode(161,a,b);}
					enc cmovnc(REG64 a,MEM64 b){Encode(161,a,b);}
					enc cmovnc(REG64 a,R_M64 b){Encode(161,a,b);}
					enc cmovne(REG16 a,REG16 b){Encode(162,a,b);}
					enc cmovne(REG16 a,MEM16 b){Encode(162,a,b);}
					enc cmovne(REG16 a,R_M16 b){Encode(162,a,b);}
					enc cmovne(REG32 a,REG32 b){Encode(163,a,b);}
					enc cmovne(REG32 a,MEM32 b){Encode(163,a,b);}
					enc cmovne(REG32 a,R_M32 b){Encode(163,a,b);}
					enc cmovne(REG64 a,REG64 b){Encode(164,a,b);}
					enc cmovne(REG64 a,MEM64 b){Encode(164,a,b);}
					enc cmovne(REG64 a,R_M64 b){Encode(164,a,b);}
					enc cmovnea(REG16 a,REG16 b){Encode(165,a,b);}
					enc cmovnea(REG16 a,MEM16 b){Encode(165,a,b);}
					enc cmovnea(REG16 a,R_M16 b){Encode(165,a,b);}
					enc cmovnea(REG32 a,REG32 b){Encode(166,a,b);}
					enc cmovnea(REG32 a,MEM32 b){Encode(166,a,b);}
					enc cmovnea(REG32 a,R_M32 b){Encode(166,a,b);}
					enc cmovnea(REG64 a,REG64 b){Encode(167,a,b);}
					enc cmovnea(REG64 a,MEM64 b){Encode(167,a,b);}
					enc cmovnea(REG64 a,R_M64 b){Encode(167,a,b);}
					enc cmovng(REG16 a,REG16 b){Encode(168,a,b);}
					enc cmovng(REG16 a,MEM16 b){Encode(168,a,b);}
					enc cmovng(REG16 a,R_M16 b){Encode(168,a,b);}
					enc cmovng(REG32 a,REG32 b){Encode(169,a,b);}
					enc cmovng(REG32 a,MEM32 b){Encode(169,a,b);}
					enc cmovng(REG32 a,R_M32 b){Encode(169,a,b);}
					enc cmovng(REG64 a,REG64 b){Encode(170,a,b);}
					enc cmovng(REG64 a,MEM64 b){Encode(170,a,b);}
					enc cmovng(REG64 a,R_M64 b){Encode(170,a,b);}
					enc cmovnge(REG16 a,REG16 b){Encode(171,a,b);}
					enc cmovnge(REG16 a,MEM16 b){Encode(171,a,b);}
					enc cmovnge(REG16 a,R_M16 b){Encode(171,a,b);}
					enc cmovnge(REG32 a,REG32 b){Encode(172,a,b);}
					enc cmovnge(REG32 a,MEM32 b){Encode(172,a,b);}
					enc cmovnge(REG32 a,R_M32 b){Encode(172,a,b);}
					enc cmovnge(REG64 a,REG64 b){Encode(173,a,b);}
					enc cmovnge(REG64 a,MEM64 b){Encode(173,a,b);}
					enc cmovnge(REG64 a,R_M64 b){Encode(173,a,b);}
					enc cmovnl(REG16 a,REG16 b){Encode(174,a,b);}
					enc cmovnl(REG16 a,MEM16 b){Encode(174,a,b);}
					enc cmovnl(REG16 a,R_M16 b){Encode(174,a,b);}
					enc cmovnl(REG32 a,REG32 b){Encode(175,a,b);}
					enc cmovnl(REG32 a,MEM32 b){Encode(175,a,b);}
					enc cmovnl(REG32 a,R_M32 b){Encode(175,a,b);}
					enc cmovnl(REG64 a,REG64 b){Encode(176,a,b);}
					enc cmovnl(REG64 a,MEM64 b){Encode(176,a,b);}
					enc cmovnl(REG64 a,R_M64 b){Encode(176,a,b);}
					enc cmovnle(REG16 a,REG16 b){Encode(177,a,b);}
					enc cmovnle(REG16 a,MEM16 b){Encode(177,a,b);}
					enc cmovnle(REG16 a,R_M16 b){Encode(177,a,b);}
					enc cmovnle(REG32 a,REG32 b){Encode(178,a,b);}
					enc cmovnle(REG32 a,MEM32 b){Encode(178,a,b);}
					enc cmovnle(REG32 a,R_M32 b){Encode(178,a,b);}
					enc cmovnle(REG64 a,REG64 b){Encode(179,a,b);}
					enc cmovnle(REG64 a,MEM64 b){Encode(179,a,b);}
					enc cmovnle(REG64 a,R_M64 b){Encode(179,a,b);}
					enc cmovno(REG16 a,REG16 b){Encode(180,a,b);}
					enc cmovno(REG16 a,MEM16 b){Encode(180,a,b);}
					enc cmovno(REG16 a,R_M16 b){Encode(180,a,b);}
					enc cmovno(REG32 a,REG32 b){Encode(181,a,b);}
					enc cmovno(REG32 a,MEM32 b){Encode(181,a,b);}
					enc cmovno(REG32 a,R_M32 b){Encode(181,a,b);}
					enc cmovno(REG64 a,REG64 b){Encode(182,a,b);}
					enc cmovno(REG64 a,MEM64 b){Encode(182,a,b);}
					enc cmovno(REG64 a,R_M64 b){Encode(182,a,b);}
					enc cmovnp(REG16 a,REG16 b){Encode(183,a,b);}
					enc cmovnp(REG16 a,MEM16 b){Encode(183,a,b);}
					enc cmovnp(REG16 a,R_M16 b){Encode(183,a,b);}
					enc cmovnp(REG32 a,REG32 b){Encode(184,a,b);}
					enc cmovnp(REG32 a,MEM32 b){Encode(184,a,b);}
					enc cmovnp(REG32 a,R_M32 b){Encode(184,a,b);}
					enc cmovnp(REG64 a,REG64 b){Encode(185,a,b);}
					enc cmovnp(REG64 a,MEM64 b){Encode(185,a,b);}
					enc cmovnp(REG64 a,R_M64 b){Encode(185,a,b);}
					enc cmovns(REG16 a,REG16 b){Encode(186,a,b);}
					enc cmovns(REG16 a,MEM16 b){Encode(186,a,b);}
					enc cmovns(REG16 a,R_M16 b){Encode(186,a,b);}
					enc cmovns(REG32 a,REG32 b){Encode(187,a,b);}
					enc cmovns(REG32 a,MEM32 b){Encode(187,a,b);}
					enc cmovns(REG32 a,R_M32 b){Encode(187,a,b);}
					enc cmovns(REG64 a,REG64 b){Encode(188,a,b);}
					enc cmovns(REG64 a,MEM64 b){Encode(188,a,b);}
					enc cmovns(REG64 a,R_M64 b){Encode(188,a,b);}
					enc cmovnz(REG16 a,REG16 b){Encode(189,a,b);}
					enc cmovnz(REG16 a,MEM16 b){Encode(189,a,b);}
					enc cmovnz(REG16 a,R_M16 b){Encode(189,a,b);}
					enc cmovnz(REG32 a,REG32 b){Encode(190,a,b);}
					enc cmovnz(REG32 a,MEM32 b){Encode(190,a,b);}
					enc cmovnz(REG32 a,R_M32 b){Encode(190,a,b);}
					enc cmovnz(REG64 a,REG64 b){Encode(191,a,b);}
					enc cmovnz(REG64 a,MEM64 b){Encode(191,a,b);}
					enc cmovnz(REG64 a,R_M64 b){Encode(191,a,b);}
					enc cmovo(REG16 a,REG16 b){Encode(192,a,b);}
					enc cmovo(REG16 a,MEM16 b){Encode(192,a,b);}
					enc cmovo(REG16 a,R_M16 b){Encode(192,a,b);}
					enc cmovo(REG32 a,REG32 b){Encode(193,a,b);}
					enc cmovo(REG32 a,MEM32 b){Encode(193,a,b);}
					enc cmovo(REG32 a,R_M32 b){Encode(193,a,b);}
					enc cmovo(REG64 a,REG64 b){Encode(194,a,b);}
					enc cmovo(REG64 a,MEM64 b){Encode(194,a,b);}
					enc cmovo(REG64 a,R_M64 b){Encode(194,a,b);}
					enc cmovp(REG16 a,REG16 b){Encode(195,a,b);}
					enc cmovp(REG16 a,MEM16 b){Encode(195,a,b);}
					enc cmovp(REG16 a,R_M16 b){Encode(195,a,b);}
					enc cmovp(REG32 a,REG32 b){Encode(196,a,b);}
					enc cmovp(REG32 a,MEM32 b){Encode(196,a,b);}
					enc cmovp(REG32 a,R_M32 b){Encode(196,a,b);}
					enc cmovp(REG64 a,REG64 b){Encode(197,a,b);}
					enc cmovp(REG64 a,MEM64 b){Encode(197,a,b);}
					enc cmovp(REG64 a,R_M64 b){Encode(197,a,b);}
					enc cmovpe(REG16 a,REG16 b){Encode(198,a,b);}
					enc cmovpe(REG16 a,MEM16 b){Encode(198,a,b);}
					enc cmovpe(REG16 a,R_M16 b){Encode(198,a,b);}
					enc cmovpe(REG32 a,REG32 b){Encode(199,a,b);}
					enc cmovpe(REG32 a,MEM32 b){Encode(199,a,b);}
					enc cmovpe(REG32 a,R_M32 b){Encode(199,a,b);}
					enc cmovpe(REG64 a,REG64 b){Encode(200,a,b);}
					enc cmovpe(REG64 a,MEM64 b){Encode(200,a,b);}
					enc cmovpe(REG64 a,R_M64 b){Encode(200,a,b);}
					enc cmovpo(REG16 a,REG16 b){Encode(201,a,b);}
					enc cmovpo(REG16 a,MEM16 b){Encode(201,a,b);}
					enc cmovpo(REG16 a,R_M16 b){Encode(201,a,b);}
					enc cmovpo(REG32 a,REG32 b){Encode(202,a,b);}
					enc cmovpo(REG32 a,MEM32 b){Encode(202,a,b);}
					enc cmovpo(REG32 a,R_M32 b){Encode(202,a,b);}
					enc cmovpo(REG64 a,REG64 b){Encode(203,a,b);}
					enc cmovpo(REG64 a,MEM64 b){Encode(203,a,b);}
					enc cmovpo(REG64 a,R_M64 b){Encode(203,a,b);}
					enc cmovs(REG16 a,REG16 b){Encode(204,a,b);}
					enc cmovs(REG16 a,MEM16 b){Encode(204,a,b);}
					enc cmovs(REG16 a,R_M16 b){Encode(204,a,b);}
					enc cmovs(REG32 a,REG32 b){Encode(205,a,b);}
					enc cmovs(REG32 a,MEM32 b){Encode(205,a,b);}
					enc cmovs(REG32 a,R_M32 b){Encode(205,a,b);}
					enc cmovs(REG32 a,REG64 b){Encode(206,a,b);}
					enc cmovs(REG32 a,MEM64 b){Encode(206,a,b);}
					enc cmovs(REG32 a,R_M64 b){Encode(206,a,b);}
					enc cmovz(REG16 a,REG16 b){Encode(207,a,b);}
					enc cmovz(REG16 a,MEM16 b){Encode(207,a,b);}
					enc cmovz(REG16 a,R_M16 b){Encode(207,a,b);}
					enc cmovz(REG32 a,REG32 b){Encode(208,a,b);}
					enc cmovz(REG32 a,MEM32 b){Encode(208,a,b);}
					enc cmovz(REG32 a,R_M32 b){Encode(208,a,b);}
					enc cmovz(REG64 a,REG64 b){Encode(209,a,b);}
					enc cmovz(REG64 a,MEM64 b){Encode(209,a,b);}
					enc cmovz(REG64 a,R_M64 b){Encode(209,a,b);}
					enc cmp(REG8 a,REG8 b){Encode(210,a,b);}
					enc cmp(MEM8 a,REG8 b){Encode(210,a,b);}
					enc cmp(R_M8 a,REG8 b){Encode(210,a,b);}
					enc cmp(REG16 a,REG16 b){Encode(211,a,b);}
					enc cmp(MEM16 a,REG16 b){Encode(211,a,b);}
					enc cmp(R_M16 a,REG16 b){Encode(211,a,b);}
					enc cmp(REG32 a,REG32 b){Encode(212,a,b);}
					enc cmp(MEM32 a,REG32 b){Encode(212,a,b);}
					enc cmp(R_M32 a,REG32 b){Encode(212,a,b);}
					enc cmp(REG64 a,REG64 b){Encode(213,a,b);}
					enc cmp(MEM64 a,REG64 b){Encode(213,a,b);}
					enc cmp(R_M64 a,REG64 b){Encode(213,a,b);}
					enc cmp(REG8 a,MEM8 b){Encode(214,a,b);}
					enc cmp(REG8 a,R_M8 b){Encode(214,a,b);}
					enc cmp(REG16 a,MEM16 b){Encode(215,a,b);}
					enc cmp(REG16 a,R_M16 b){Encode(215,a,b);}
					enc cmp(REG32 a,MEM32 b){Encode(216,a,b);}
					enc cmp(REG32 a,R_M32 b){Encode(216,a,b);}
					enc cmp(REG64 a,MEM64 b){Encode(217,a,b);}
					enc cmp(REG64 a,R_M64 b){Encode(217,a,b);}
					enc cmp(REG8 a,byte b){Encode(218,a,(IMM)b);}
					enc cmp(AL a,byte b){Encode(218,a,(IMM)b);}
					enc cmp(CL a,byte b){Encode(218,a,(IMM)b);}
					enc cmp(MEM8 a,byte b){Encode(218,a,(IMM)b);}
					enc cmp(R_M8 a,byte b){Encode(218,a,(IMM)b);}
					enc cmp(REG16 a,byte b){Encode(219,a,(IMM)b);}
					enc cmp(REG16 a,word b){Encode(219,a,(IMM)b);}
					enc cmp(MEM16 a,byte b){Encode(219,a,(IMM)b);}
					enc cmp(MEM16 a,word b){Encode(219,a,(IMM)b);}
					enc cmp(R_M16 a,byte b){Encode(219,a,(IMM)b);}
					enc cmp(R_M16 a,word b){Encode(219,a,(IMM)b);}
					enc cmp(REG32 a,REF b){Encode(220,a,b);}
					enc cmp(REG32 a,dword b){Encode(220,a,(IMM)b);}
					enc cmp(MEM32 a,REF b){Encode(220,a,b);}
					enc cmp(MEM32 a,dword b){Encode(220,a,(IMM)b);}
					enc cmp(R_M32 a,REF b){Encode(220,a,b);}
					enc cmp(R_M32 a,dword b){Encode(220,a,(IMM)b);}
					enc cmp(REG64 a,REF b){Encode(221,a,b);}
					enc cmp(REG64 a,dword b){Encode(221,a,(IMM)b);}
					enc cmp(MEM64 a,REF b){Encode(221,a,b);}
					enc cmp(MEM64 a,dword b){Encode(221,a,(IMM)b);}
					enc cmp(R_M64 a,REF b){Encode(221,a,b);}
					enc cmp(R_M64 a,dword b){Encode(221,a,(IMM)b);}
					enc cmpeqpd(XMMREG a,XMMREG b){Encode(229,a,b);}
					enc cmpeqpd(XMMREG a,MEM128 b){Encode(229,a,b);}
					enc cmpeqpd(XMMREG a,R_M128 b){Encode(229,a,b);}
					enc cmpeqps(XMMREG a,XMMREG b){Encode(230,a,b);}
					enc cmpeqps(XMMREG a,MEM128 b){Encode(230,a,b);}
					enc cmpeqps(XMMREG a,R_M128 b){Encode(230,a,b);}
					enc cmpeqsd(XMMREG a,XMMREG b){Encode(231,a,b);}
					enc cmpeqsd(XMMREG a,MEM64 b){Encode(231,a,b);}
					enc cmpeqsd(XMMREG a,XMM64 b){Encode(231,a,b);}
					enc cmpeqss(XMMREG a,XMMREG b){Encode(232,a,b);}
					enc cmpeqss(XMMREG a,MEM32 b){Encode(232,a,b);}
					enc cmpeqss(XMMREG a,XMM32 b){Encode(232,a,b);}
					enc cmplepd(XMMREG a,XMMREG b){Encode(233,a,b);}
					enc cmplepd(XMMREG a,MEM128 b){Encode(233,a,b);}
					enc cmplepd(XMMREG a,R_M128 b){Encode(233,a,b);}
					enc cmpleps(XMMREG a,XMMREG b){Encode(234,a,b);}
					enc cmpleps(XMMREG a,MEM128 b){Encode(234,a,b);}
					enc cmpleps(XMMREG a,R_M128 b){Encode(234,a,b);}
					enc cmplesd(XMMREG a,XMMREG b){Encode(235,a,b);}
					enc cmplesd(XMMREG a,MEM64 b){Encode(235,a,b);}
					enc cmplesd(XMMREG a,XMM64 b){Encode(235,a,b);}
					enc cmpless(XMMREG a,XMMREG b){Encode(236,a,b);}
					enc cmpless(XMMREG a,MEM32 b){Encode(236,a,b);}
					enc cmpless(XMMREG a,XMM32 b){Encode(236,a,b);}
					enc cmpltpd(XMMREG a,XMMREG b){Encode(237,a,b);}
					enc cmpltpd(XMMREG a,MEM128 b){Encode(237,a,b);}
					enc cmpltpd(XMMREG a,R_M128 b){Encode(237,a,b);}
					enc cmpltps(XMMREG a,XMMREG b){Encode(238,a,b);}
					enc cmpltps(XMMREG a,MEM128 b){Encode(238,a,b);}
					enc cmpltps(XMMREG a,R_M128 b){Encode(238,a,b);}
					enc cmpltsd(XMMREG a,XMMREG b){Encode(239,a,b);}
					enc cmpltsd(XMMREG a,MEM64 b){Encode(239,a,b);}
					enc cmpltsd(XMMREG a,XMM64 b){Encode(239,a,b);}
					enc cmpltss(XMMREG a,XMMREG b){Encode(240,a,b);}
					enc cmpltss(XMMREG a,MEM32 b){Encode(240,a,b);}
					enc cmpltss(XMMREG a,XMM32 b){Encode(240,a,b);}
					enc cmpneqpd(XMMREG a,XMMREG b){Encode(241,a,b);}
					enc cmpneqpd(XMMREG a,MEM128 b){Encode(241,a,b);}
					enc cmpneqpd(XMMREG a,R_M128 b){Encode(241,a,b);}
					enc cmpneqps(XMMREG a,XMMREG b){Encode(242,a,b);}
					enc cmpneqps(XMMREG a,MEM128 b){Encode(242,a,b);}
					enc cmpneqps(XMMREG a,R_M128 b){Encode(242,a,b);}
					enc cmpneqsd(XMMREG a,XMMREG b){Encode(243,a,b);}
					enc cmpneqsd(XMMREG a,MEM64 b){Encode(243,a,b);}
					enc cmpneqsd(XMMREG a,XMM64 b){Encode(243,a,b);}
					enc cmpneqss(XMMREG a,XMMREG b){Encode(244,a,b);}
					enc cmpneqss(XMMREG a,MEM32 b){Encode(244,a,b);}
					enc cmpneqss(XMMREG a,XMM32 b){Encode(244,a,b);}
					enc cmpnlepd(XMMREG a,XMMREG b){Encode(245,a,b);}
					enc cmpnlepd(XMMREG a,MEM128 b){Encode(245,a,b);}
					enc cmpnlepd(XMMREG a,R_M128 b){Encode(245,a,b);}
					enc cmpnleps(XMMREG a,XMMREG b){Encode(246,a,b);}
					enc cmpnleps(XMMREG a,MEM128 b){Encode(246,a,b);}
					enc cmpnleps(XMMREG a,R_M128 b){Encode(246,a,b);}
					enc cmpnlesd(XMMREG a,XMMREG b){Encode(247,a,b);}
					enc cmpnlesd(XMMREG a,MEM64 b){Encode(247,a,b);}
					enc cmpnlesd(XMMREG a,XMM64 b){Encode(247,a,b);}
					enc cmpnless(XMMREG a,XMMREG b){Encode(248,a,b);}
					enc cmpnless(XMMREG a,MEM32 b){Encode(248,a,b);}
					enc cmpnless(XMMREG a,XMM32 b){Encode(248,a,b);}
					enc cmpnltpd(XMMREG a,XMMREG b){Encode(249,a,b);}
					enc cmpnltpd(XMMREG a,MEM128 b){Encode(249,a,b);}
					enc cmpnltpd(XMMREG a,R_M128 b){Encode(249,a,b);}
					enc cmpnltps(XMMREG a,XMMREG b){Encode(250,a,b);}
					enc cmpnltps(XMMREG a,MEM128 b){Encode(250,a,b);}
					enc cmpnltps(XMMREG a,R_M128 b){Encode(250,a,b);}
					enc cmpnltsd(XMMREG a,XMMREG b){Encode(251,a,b);}
					enc cmpnltsd(XMMREG a,MEM64 b){Encode(251,a,b);}
					enc cmpnltsd(XMMREG a,XMM64 b){Encode(251,a,b);}
					enc cmpnltss(XMMREG a,XMMREG b){Encode(252,a,b);}
					enc cmpnltss(XMMREG a,MEM32 b){Encode(252,a,b);}
					enc cmpnltss(XMMREG a,XMM32 b){Encode(252,a,b);}
					enc cmpordpd(XMMREG a,XMMREG b){Encode(253,a,b);}
					enc cmpordpd(XMMREG a,MEM128 b){Encode(253,a,b);}
					enc cmpordpd(XMMREG a,R_M128 b){Encode(253,a,b);}
					enc cmpordps(XMMREG a,XMMREG b){Encode(254,a,b);}
					enc cmpordps(XMMREG a,MEM128 b){Encode(254,a,b);}
					enc cmpordps(XMMREG a,R_M128 b){Encode(254,a,b);}
					enc cmpordsd(XMMREG a,XMMREG b){Encode(255,a,b);}
					enc cmpordsd(XMMREG a,MEM64 b){Encode(255,a,b);}
					enc cmpordsd(XMMREG a,XMM64 b){Encode(255,a,b);}
					enc cmpordss(XMMREG a,XMMREG b){Encode(256,a,b);}
					enc cmpordss(XMMREG a,MEM32 b){Encode(256,a,b);}
					enc cmpordss(XMMREG a,XMM32 b){Encode(256,a,b);}
					enc cmppd(XMMREG a,XMMREG b,byte c){Encode(257,a,b,(IMM)c);}
					enc cmppd(XMMREG a,MEM128 b,byte c){Encode(257,a,b,(IMM)c);}
					enc cmppd(XMMREG a,R_M128 b,byte c){Encode(257,a,b,(IMM)c);}
					enc cmpps(XMMREG a,XMMREG b,byte c){Encode(258,a,b,(IMM)c);}
					enc cmpps(XMMREG a,MEM128 b,byte c){Encode(258,a,b,(IMM)c);}
					enc cmpps(XMMREG a,R_M128 b,byte c){Encode(258,a,b,(IMM)c);}
					enc cmpsb(){Encode(259);}
					enc cmpsd(){Encode(260);}
					enc cmpsd(XMMREG a,XMMREG b,byte c){Encode(261,a,b,(IMM)c);}
					enc cmpsd(XMMREG a,MEM64 b,byte c){Encode(261,a,b,(IMM)c);}
					enc cmpsd(XMMREG a,XMM64 b,byte c){Encode(261,a,b,(IMM)c);}
					enc cmpsq(){Encode(262);}
					enc cmpss(XMMREG a,XMMREG b,byte c){Encode(263,a,b,(IMM)c);}
					enc cmpss(XMMREG a,MEM32 b,byte c){Encode(263,a,b,(IMM)c);}
					enc cmpss(XMMREG a,XMM32 b,byte c){Encode(263,a,b,(IMM)c);}
					enc cmpsw(){Encode(264);}
					enc cmpunordpd(XMMREG a,XMMREG b){Encode(265,a,b);}
					enc cmpunordpd(XMMREG a,MEM128 b){Encode(265,a,b);}
					enc cmpunordpd(XMMREG a,R_M128 b){Encode(265,a,b);}
					enc cmpunordps(XMMREG a,XMMREG b){Encode(266,a,b);}
					enc cmpunordps(XMMREG a,MEM128 b){Encode(266,a,b);}
					enc cmpunordps(XMMREG a,R_M128 b){Encode(266,a,b);}
					enc cmpunordsd(XMMREG a,XMMREG b){Encode(267,a,b);}
					enc cmpunordsd(XMMREG a,MEM64 b){Encode(267,a,b);}
					enc cmpunordsd(XMMREG a,XMM64 b){Encode(267,a,b);}
					enc cmpunordss(XMMREG a,XMMREG b){Encode(268,a,b);}
					enc cmpunordss(XMMREG a,MEM32 b){Encode(268,a,b);}
					enc cmpunordss(XMMREG a,XMM32 b){Encode(268,a,b);}
					enc cmpxchg(REG8 a,REG8 b){Encode(269,a,b);}
					enc cmpxchg(MEM8 a,REG8 b){Encode(269,a,b);}
					enc cmpxchg(R_M8 a,REG8 b){Encode(269,a,b);}
					enc cmpxchg(REG16 a,REG16 b){Encode(270,a,b);}
					enc cmpxchg(MEM16 a,REG16 b){Encode(270,a,b);}
					enc cmpxchg(R_M16 a,REG16 b){Encode(270,a,b);}
					enc cmpxchg(REG32 a,REG32 b){Encode(271,a,b);}
					enc cmpxchg(MEM32 a,REG32 b){Encode(271,a,b);}
					enc cmpxchg(R_M32 a,REG32 b){Encode(271,a,b);}
					enc cmpxchg(REG64 a,REG64 b){Encode(272,a,b);}
					enc cmpxchg(MEM64 a,REG64 b){Encode(272,a,b);}
					enc cmpxchg(R_M64 a,REG64 b){Encode(272,a,b);}
					enc cmpxchg16b(MEM8 a){Encode(273,a);}
					enc cmpxchg16b(MEM16 a){Encode(273,a);}
					enc cmpxchg16b(MEM32 a){Encode(273,a);}
					enc cmpxchg16b(MEM64 a){Encode(273,a);}
					enc cmpxchg16b(MEM128 a){Encode(273,a);}
					enc cmpxchg8b(MEM8 a){Encode(274,a);}
					enc cmpxchg8b(MEM16 a){Encode(274,a);}
					enc cmpxchg8b(MEM32 a){Encode(274,a);}
					enc cmpxchg8b(MEM64 a){Encode(274,a);}
					enc cmpxchg8b(MEM128 a){Encode(274,a);}
					enc comisd(XMMREG a,XMMREG b){Encode(275,a,b);}
					enc comisd(XMMREG a,MEM64 b){Encode(275,a,b);}
					enc comisd(XMMREG a,XMM64 b){Encode(275,a,b);}
					enc comiss(XMMREG a,XMMREG b){Encode(276,a,b);}
					enc comiss(XMMREG a,MEM32 b){Encode(276,a,b);}
					enc comiss(XMMREG a,XMM32 b){Encode(276,a,b);}
					enc cpuid(){Encode(277);}
					enc cqo(){Encode(278);}
					enc cvtdq2pd(XMMREG a,XMMREG b){Encode(279,a,b);}
					enc cvtdq2pd(XMMREG a,MEM64 b){Encode(279,a,b);}
					enc cvtdq2pd(XMMREG a,XMM64 b){Encode(279,a,b);}
					enc cvtdq2ps(XMMREG a,XMMREG b){Encode(280,a,b);}
					enc cvtdq2ps(XMMREG a,MEM128 b){Encode(280,a,b);}
					enc cvtdq2ps(XMMREG a,R_M128 b){Encode(280,a,b);}
					enc cvtpd2dq(XMMREG a,XMMREG b){Encode(281,a,b);}
					enc cvtpd2dq(XMMREG a,MEM128 b){Encode(281,a,b);}
					enc cvtpd2dq(XMMREG a,R_M128 b){Encode(281,a,b);}
					enc cvtpd2pi(MMREG a,XMMREG b){Encode(282,a,b);}
					enc cvtpd2pi(MMREG a,MEM128 b){Encode(282,a,b);}
					enc cvtpd2pi(MMREG a,R_M128 b){Encode(282,a,b);}
					enc cvtpd2ps(XMMREG a,XMMREG b){Encode(283,a,b);}
					enc cvtpd2ps(XMMREG a,MEM128 b){Encode(283,a,b);}
					enc cvtpd2ps(XMMREG a,R_M128 b){Encode(283,a,b);}
					enc cvtpi2pd(XMMREG a,MMREG b){Encode(284,a,b);}
					enc cvtpi2pd(XMMREG a,MEM64 b){Encode(284,a,b);}
					enc cvtpi2pd(XMMREG a,MM64 b){Encode(284,a,b);}
					enc cvtpi2ps(XMMREG a,MMREG b){Encode(285,a,b);}
					enc cvtpi2ps(XMMREG a,MEM64 b){Encode(285,a,b);}
					enc cvtpi2ps(XMMREG a,MM64 b){Encode(285,a,b);}
					enc cvtps2dq(XMMREG a,XMMREG b){Encode(286,a,b);}
					enc cvtps2dq(XMMREG a,MEM128 b){Encode(286,a,b);}
					enc cvtps2dq(XMMREG a,R_M128 b){Encode(286,a,b);}
					enc cvtps2pd(XMMREG a,XMMREG b){Encode(287,a,b);}
					enc cvtps2pd(XMMREG a,MEM64 b){Encode(287,a,b);}
					enc cvtps2pd(XMMREG a,XMM64 b){Encode(287,a,b);}
					enc cvtps2pi(MMREG a,XMMREG b){Encode(288,a,b);}
					enc cvtps2pi(MMREG a,MEM64 b){Encode(288,a,b);}
					enc cvtps2pi(MMREG a,XMM64 b){Encode(288,a,b);}
					enc cvtsd2si(REG32 a,XMMREG b){Encode(289,a,b);}
					enc cvtsd2si(REG32 a,MEM64 b){Encode(289,a,b);}
					enc cvtsd2si(REG32 a,XMM64 b){Encode(289,a,b);}
					enc cvtsi2sd(XMMREG a,REG32 b){Encode(290,a,b);}
					enc cvtsi2sd(XMMREG a,MEM32 b){Encode(290,a,b);}
					enc cvtsi2sd(XMMREG a,R_M32 b){Encode(290,a,b);}
					enc cvtsi2ss(XMMREG a,REG32 b){Encode(291,a,b);}
					enc cvtsi2ss(XMMREG a,MEM32 b){Encode(291,a,b);}
					enc cvtsi2ss(XMMREG a,R_M32 b){Encode(291,a,b);}
					enc cvtss2sd(XMMREG a,XMMREG b){Encode(292,a,b);}
					enc cvtss2sd(XMMREG a,MEM32 b){Encode(292,a,b);}
					enc cvtss2sd(XMMREG a,XMM32 b){Encode(292,a,b);}
					enc cvtss2si(REG32 a,XMMREG b){Encode(293,a,b);}
					enc cvtss2si(REG32 a,MEM32 b){Encode(293,a,b);}
					enc cvtss2si(REG32 a,XMM32 b){Encode(293,a,b);}
					enc cvttpd2dq(XMMREG a,XMMREG b){Encode(294,a,b);}
					enc cvttpd2dq(XMMREG a,MEM128 b){Encode(294,a,b);}
					enc cvttpd2dq(XMMREG a,R_M128 b){Encode(294,a,b);}
					enc cvttpd2pi(MMREG a,XMMREG b){Encode(295,a,b);}
					enc cvttpd2pi(MMREG a,MEM128 b){Encode(295,a,b);}
					enc cvttpd2pi(MMREG a,R_M128 b){Encode(295,a,b);}
					enc cvttps2dq(XMMREG a,XMMREG b){Encode(296,a,b);}
					enc cvttps2dq(XMMREG a,MEM128 b){Encode(296,a,b);}
					enc cvttps2dq(XMMREG a,R_M128 b){Encode(296,a,b);}
					enc cvttps2pi(MMREG a,XMMREG b){Encode(297,a,b);}
					enc cvttps2pi(MMREG a,MEM64 b){Encode(297,a,b);}
					enc cvttps2pi(MMREG a,XMM64 b){Encode(297,a,b);}
					enc cvttsd2si(REG32 a,XMMREG b){Encode(298,a,b);}
					enc cvttsd2si(REG32 a,MEM64 b){Encode(298,a,b);}
					enc cvttsd2si(REG32 a,XMM64 b){Encode(298,a,b);}
					enc cvttss2si(REG32 a,XMMREG b){Encode(299,a,b);}
					enc cvttss2si(REG32 a,MEM32 b){Encode(299,a,b);}
					enc cvttss2si(REG32 a,XMM32 b){Encode(299,a,b);}
					enc cwd(){Encode(300);}
					enc cwde(){Encode(301);}
					enc daa(){Encode(302);}
					enc das(){Encode(303);}
					enc db(){Encode(304);}
					enc db(byte a){Encode(305,(IMM)a);}
					enc db(MEM8 a){Encode(306,a);}
					enc db(MEM16 a){Encode(306,a);}
					enc db(MEM32 a){Encode(306,a);}
					enc db(MEM64 a){Encode(306,a);}
					enc db(MEM128 a){Encode(306,a);}
					enc dd(){Encode(307);}
					enc dd(REF a){Encode(308,a);}
					enc dd(dword a){Encode(308,(IMM)a);}
					enc dd(MEM8 a){Encode(309,a);}
					enc dd(MEM16 a){Encode(309,a);}
					enc dd(MEM32 a){Encode(309,a);}
					enc dd(MEM64 a){Encode(309,a);}
					enc dd(MEM128 a){Encode(309,a);}
					enc dec(REG8 a){Encode(310,a);}
					enc dec(MEM8 a){Encode(310,a);}
					enc dec(R_M8 a){Encode(310,a);}
					enc dec(REG16 a){Encode(311,a);}
					enc dec(MEM16 a){Encode(311,a);}
					enc dec(R_M16 a){Encode(311,a);}
					enc dec(REG32 a){Encode(312,a);}
					enc dec(MEM32 a){Encode(312,a);}
					enc dec(R_M32 a){Encode(312,a);}
					enc dec(REG64 a){Encode(313,a);}
					enc dec(MEM64 a){Encode(313,a);}
					enc dec(R_M64 a){Encode(313,a);}
					enc div(REG8 a){Encode(314,a);}
					enc div(MEM8 a){Encode(314,a);}
					enc div(R_M8 a){Encode(314,a);}
					enc div(REG16 a){Encode(315,a);}
					enc div(MEM16 a){Encode(315,a);}
					enc div(R_M16 a){Encode(315,a);}
					enc div(REG32 a){Encode(316,a);}
					enc div(MEM32 a){Encode(316,a);}
					enc div(R_M32 a){Encode(316,a);}
					enc div(REG64 a){Encode(317,a);}
					enc div(MEM64 a){Encode(317,a);}
					enc div(R_M64 a){Encode(317,a);}
					enc divpd(XMMREG a,XMMREG b){Encode(318,a,b);}
					enc divpd(XMMREG a,MEM128 b){Encode(318,a,b);}
					enc divpd(XMMREG a,R_M128 b){Encode(318,a,b);}
					enc divps(XMMREG a,XMMREG b){Encode(319,a,b);}
					enc divps(XMMREG a,MEM128 b){Encode(319,a,b);}
					enc divps(XMMREG a,R_M128 b){Encode(319,a,b);}
					enc divsd(XMMREG a,XMMREG b){Encode(320,a,b);}
					enc divsd(XMMREG a,MEM64 b){Encode(320,a,b);}
					enc divsd(XMMREG a,XMM64 b){Encode(320,a,b);}
					enc divss(XMMREG a,XMMREG b){Encode(321,a,b);}
					enc divss(XMMREG a,MEM32 b){Encode(321,a,b);}
					enc divss(XMMREG a,XMM32 b){Encode(321,a,b);}
					enc dw(){Encode(322);}
					enc dw(byte a){Encode(323,(IMM)a);}
					enc dw(word a){Encode(323,(IMM)a);}
					enc dw(MEM8 a){Encode(324,a);}
					enc dw(MEM16 a){Encode(324,a);}
					enc dw(MEM32 a){Encode(324,a);}
					enc dw(MEM64 a){Encode(324,a);}
					enc dw(MEM128 a){Encode(324,a);}
					enc emms(){Encode(325);}
					enc f2xm1(){Encode(326);}
					enc fabs(){Encode(327);}
					enc fadd(MEM32 a){Encode(328,a);}
					enc fadd(MEM64 a){Encode(329,a);}
					enc fadd(FPUREG a){Encode(330,a);}
					enc fadd(ST0 a,FPUREG b){Encode(331,a,b);}
					enc fadd(FPUREG a,ST0 b){Encode(332,a,b);}
					enc faddp(){Encode(333);}
					enc faddp(FPUREG a){Encode(334,a);}
					enc faddp(FPUREG a,ST0 b){Encode(335,a,b);}
					enc fchs(){Encode(336);}
					enc fclex(){Encode(337);}
					enc fcmovb(FPUREG a){Encode(338,a);}
					enc fcmovb(ST0 a,FPUREG b){Encode(339,a,b);}
					enc fcmovbe(FPUREG a){Encode(340,a);}
					enc fcmovbe(ST0 a,FPUREG b){Encode(341,a,b);}
					enc fcmove(FPUREG a){Encode(342,a);}
					enc fcmove(ST0 a,FPUREG b){Encode(343,a,b);}
					enc fcmovnb(FPUREG a){Encode(344,a);}
					enc fcmovnb(ST0 a,FPUREG b){Encode(345,a,b);}
					enc fcmovnbe(FPUREG a){Encode(346,a);}
					enc fcmovnbe(ST0 a,FPUREG b){Encode(347,a,b);}
					enc fcmovne(FPUREG a){Encode(348,a);}
					enc fcmovne(ST0 a,FPUREG b){Encode(349,a,b);}
					enc fcmovnu(FPUREG a){Encode(350,a);}
					enc fcmovnu(ST0 a,FPUREG b){Encode(351,a,b);}
					enc fcmovu(FPUREG a){Encode(352,a);}
					enc fcmovu(ST0 a,FPUREG b){Encode(353,a,b);}
					enc fcom(MEM32 a){Encode(354,a);}
					enc fcom(MEM64 a){Encode(355,a);}
					enc fcom(FPUREG a){Encode(356,a);}
					enc fcom(ST0 a,FPUREG b){Encode(357,a,b);}
					enc fcomi(FPUREG a){Encode(358,a);}
					enc fcomi(ST0 a,FPUREG b){Encode(359,a,b);}
					enc fcomip(FPUREG a){Encode(360,a);}
					enc fcomip(ST0 a,FPUREG b){Encode(361,a,b);}
					enc fcomp(MEM32 a){Encode(362,a);}
					enc fcomp(MEM64 a){Encode(363,a);}
					enc fcomp(FPUREG a){Encode(364,a);}
					enc fcomp(ST0 a,FPUREG b){Encode(365,a,b);}
					enc fcompp(){Encode(366);}
					enc fcos(){Encode(367);}
					enc fdecstp(){Encode(368);}
					enc fdisi(){Encode(369);}
					enc fdiv(MEM32 a){Encode(370,a);}
					enc fdiv(MEM64 a){Encode(371,a);}
					enc fdiv(FPUREG a){Encode(372,a);}
					enc fdiv(ST0 a,FPUREG b){Encode(373,a,b);}
					enc fdiv(FPUREG a,ST0 b){Encode(374,a,b);}
					enc fdivp(){Encode(375);}
					enc fdivp(FPUREG a){Encode(376,a);}
					enc fdivp(FPUREG a,ST0 b){Encode(377,a,b);}
					enc fdivr(MEM32 a){Encode(378,a);}
					enc fdivr(MEM64 a){Encode(379,a);}
					enc fdivr(FPUREG a){Encode(380,a);}
					enc fdivr(ST0 a,FPUREG b){Encode(381,a,b);}
					enc fdivr(FPUREG a,ST0 b){Encode(382,a,b);}
					enc fdivrp(){Encode(383);}
					enc fdivrp(FPUREG a){Encode(384,a);}
					enc fdivrp(FPUREG a,ST0 b){Encode(385,a,b);}
					enc femms(){Encode(386);}
					enc feni(){Encode(387);}
					enc ffree(FPUREG a){Encode(388,a);}
					enc fiadd(MEM16 a){Encode(389,a);}
					enc fiadd(MEM32 a){Encode(390,a);}
					enc ficom(MEM16 a){Encode(391,a);}
					enc ficom(MEM32 a){Encode(392,a);}
					enc ficomp(MEM16 a){Encode(393,a);}
					enc ficomp(MEM32 a){Encode(394,a);}
					enc fidiv(MEM16 a){Encode(395,a);}
					enc fidiv(MEM32 a){Encode(396,a);}
					enc fidivr(MEM16 a){Encode(397,a);}
					enc fidivr(MEM32 a){Encode(398,a);}
					enc fild(MEM16 a){Encode(399,a);}
					enc fild(MEM32 a){Encode(400,a);}
					enc fild(MEM64 a){Encode(401,a);}
					enc fimul(MEM16 a){Encode(402,a);}
					enc fimul(MEM32 a){Encode(403,a);}
					enc fincstp(){Encode(404);}
					enc finit(){Encode(405);}
					enc fist(MEM16 a){Encode(406,a);}
					enc fist(MEM32 a){Encode(407,a);}
					enc fistp(MEM16 a){Encode(408,a);}
					enc fistp(MEM32 a){Encode(409,a);}
					enc fistp(MEM64 a){Encode(410,a);}
					enc fisttp(MEM16 a){Encode(411,a);}
					enc fisttp(MEM32 a){Encode(412,a);}
					enc fisttp(MEM64 a){Encode(413,a);}
					enc fisub(MEM16 a){Encode(414,a);}
					enc fisub(MEM32 a){Encode(415,a);}
					enc fisubr(MEM16 a){Encode(416,a);}
					enc fisubr(MEM32 a){Encode(417,a);}
					enc fld(MEM32 a){Encode(418,a);}
					enc fld(MEM64 a){Encode(419,a);}
					enc fld(FPUREG a){Encode(420,a);}
					enc fld1(){Encode(421);}
					enc fldcw(MEM16 a){Encode(422,a);}
					enc fldenv(MEM8 a){Encode(423,a);}
					enc fldenv(MEM16 a){Encode(423,a);}
					enc fldenv(MEM32 a){Encode(423,a);}
					enc fldenv(MEM64 a){Encode(423,a);}
					enc fldenv(MEM128 a){Encode(423,a);}
					enc fldl2e(){Encode(424);}
					enc fldl2t(){Encode(425);}
					enc fldlg2(){Encode(426);}
					enc fldln2(){Encode(427);}
					enc fldpi(){Encode(428);}
					enc fldz(){Encode(429);}
					enc fmul(MEM32 a){Encode(430,a);}
					enc fmul(MEM64 a){Encode(431,a);}
					enc fmul(){Encode(432);}
					enc fmul(FPUREG a){Encode(433,a);}
					enc fmul(ST0 a,FPUREG b){Encode(434,a,b);}
					enc fmul(FPUREG a,ST0 b){Encode(435,a,b);}
					enc fmulp(FPUREG a){Encode(436,a);}
					enc fmulp(FPUREG a,ST0 b){Encode(437,a,b);}
					enc fmulp(){Encode(438);}
					enc fnclex(){Encode(439);}
					enc fndisi(){Encode(440);}
					enc fneni(){Encode(441);}
					enc fninit(){Encode(442);}
					enc fnop(){Encode(443);}
					enc fnsave(MEM8 a){Encode(444,a);}
					enc fnsave(MEM16 a){Encode(444,a);}
					enc fnsave(MEM32 a){Encode(444,a);}
					enc fnsave(MEM64 a){Encode(444,a);}
					enc fnsave(MEM128 a){Encode(444,a);}
					enc fnstcw(MEM16 a){Encode(445,a);}
					enc fnstenv(MEM8 a){Encode(446,a);}
					enc fnstenv(MEM16 a){Encode(446,a);}
					enc fnstenv(MEM32 a){Encode(446,a);}
					enc fnstenv(MEM64 a){Encode(446,a);}
					enc fnstenv(MEM128 a){Encode(446,a);}
					enc fnstsw(MEM16 a){Encode(447,a);}
					enc fnstsw(AX a){Encode(448,a);}
					enc fpatan(){Encode(449);}
					enc fprem(){Encode(450);}
					enc fprem1(){Encode(451);}
					enc fptan(){Encode(452);}
					enc frndint(){Encode(453);}
					enc frstor(MEM8 a){Encode(454,a);}
					enc frstor(MEM16 a){Encode(454,a);}
					enc frstor(MEM32 a){Encode(454,a);}
					enc frstor(MEM64 a){Encode(454,a);}
					enc frstor(MEM128 a){Encode(454,a);}
					enc fsave(MEM8 a){Encode(455,a);}
					enc fsave(MEM16 a){Encode(455,a);}
					enc fsave(MEM32 a){Encode(455,a);}
					enc fsave(MEM64 a){Encode(455,a);}
					enc fsave(MEM128 a){Encode(455,a);}
					enc fscale(){Encode(456);}
					enc fsetpm(){Encode(457);}
					enc fsin(){Encode(458);}
					enc fsincos(){Encode(459);}
					enc fsqrt(){Encode(460);}
					enc fst(MEM32 a){Encode(461,a);}
					enc fst(MEM64 a){Encode(462,a);}
					enc fst(FPUREG a){Encode(463,a);}
					enc fstcw(MEM16 a){Encode(464,a);}
					enc fstenv(MEM8 a){Encode(465,a);}
					enc fstenv(MEM16 a){Encode(465,a);}
					enc fstenv(MEM32 a){Encode(465,a);}
					enc fstenv(MEM64 a){Encode(465,a);}
					enc fstenv(MEM128 a){Encode(465,a);}
					enc fstp(MEM32 a){Encode(466,a);}
					enc fstp(MEM64 a){Encode(467,a);}
					enc fstp(FPUREG a){Encode(468,a);}
					enc fstsw(MEM16 a){Encode(469,a);}
					enc fstsw(AX a){Encode(470,a);}
					enc fsub(MEM32 a){Encode(471,a);}
					enc fsub(MEM64 a){Encode(472,a);}
					enc fsub(FPUREG a){Encode(473,a);}
					enc fsub(ST0 a,FPUREG b){Encode(474,a,b);}
					enc fsub(FPUREG a,ST0 b){Encode(475,a,b);}
					enc fsubp(){Encode(476);}
					enc fsubp(FPUREG a){Encode(477,a);}
					enc fsubp(FPUREG a,ST0 b){Encode(478,a,b);}
					enc fsubr(MEM32 a){Encode(479,a);}
					enc fsubr(MEM64 a){Encode(480,a);}
					enc fsubr(FPUREG a){Encode(481,a);}
					enc fsubr(ST0 a,FPUREG b){Encode(482,a,b);}
					enc fsubr(FPUREG a,ST0 b){Encode(483,a,b);}
					enc fsubrp(){Encode(484);}
					enc fsubrp(FPUREG a){Encode(485,a);}
					enc fsubrp(FPUREG a,ST0 b){Encode(486,a,b);}
					enc ftst(){Encode(487);}
					enc fucom(FPUREG a){Encode(488,a);}
					enc fucom(ST0 a,FPUREG b){Encode(489,a,b);}
					enc fucomi(FPUREG a){Encode(490,a);}
					enc fucomi(ST0 a,FPUREG b){Encode(491,a,b);}
					enc fucomip(FPUREG a){Encode(492,a);}
					enc fucomip(ST0 a,FPUREG b){Encode(493,a,b);}
					enc fucomp(FPUREG a){Encode(494,a);}
					enc fucomp(ST0 a,FPUREG b){Encode(495,a,b);}
					enc fucompp(){Encode(496);}
					enc fwait(){Encode(497);}
					enc fxam(){Encode(498);}
					enc fxch(){Encode(499);}
					enc fxch(FPUREG a){Encode(500,a);}
					enc fxch(FPUREG a,ST0 b){Encode(501,a,b);}
					enc fxch(ST0 a,FPUREG b){Encode(502,a,b);}
					enc fxtract(){Encode(503);}
					enc fyl2x(){Encode(504);}
					enc fyl2xp1(){Encode(505);}
					enc haddpd(XMMREG a,XMMREG b){Encode(506,a,b);}
					enc haddpd(XMMREG a,MEM128 b){Encode(506,a,b);}
					enc haddpd(XMMREG a,R_M128 b){Encode(506,a,b);}
					enc haddps(XMMREG a,XMMREG b){Encode(507,a,b);}
					enc haddps(XMMREG a,MEM128 b){Encode(507,a,b);}
					enc haddps(XMMREG a,R_M128 b){Encode(507,a,b);}
					enc hlt(){Encode(508);}
					enc hsubpd(XMMREG a,XMMREG b){Encode(509,a,b);}
					enc hsubpd(XMMREG a,MEM128 b){Encode(509,a,b);}
					enc hsubpd(XMMREG a,R_M128 b){Encode(509,a,b);}
					enc hsubps(XMMREG a,XMMREG b){Encode(510,a,b);}
					enc hsubps(XMMREG a,MEM128 b){Encode(510,a,b);}
					enc hsubps(XMMREG a,R_M128 b){Encode(510,a,b);}
					enc idiv(REG8 a){Encode(511,a);}
					enc idiv(MEM8 a){Encode(511,a);}
					enc idiv(R_M8 a){Encode(511,a);}
					enc idiv(REG16 a){Encode(512,a);}
					enc idiv(MEM16 a){Encode(512,a);}
					enc idiv(R_M16 a){Encode(512,a);}
					enc idiv(REG32 a){Encode(513,a);}
					enc idiv(MEM32 a){Encode(513,a);}
					enc idiv(R_M32 a){Encode(513,a);}
					enc idiv(REG64 a){Encode(514,a);}
					enc idiv(MEM64 a){Encode(514,a);}
					enc idiv(R_M64 a){Encode(514,a);}
					enc imul(REG8 a){Encode(515,a);}
					enc imul(MEM8 a){Encode(515,a);}
					enc imul(R_M8 a){Encode(515,a);}
					enc imul(REG16 a){Encode(516,a);}
					enc imul(MEM16 a){Encode(516,a);}
					enc imul(R_M16 a){Encode(516,a);}
					enc imul(REG32 a){Encode(517,a);}
					enc imul(MEM32 a){Encode(517,a);}
					enc imul(R_M32 a){Encode(517,a);}
					enc imul(REG64 a){Encode(518,a);}
					enc imul(MEM64 a){Encode(518,a);}
					enc imul(R_M64 a){Encode(518,a);}
					enc imul(REG16 a,REG16 b){Encode(519,a,b);}
					enc imul(REG16 a,MEM16 b){Encode(519,a,b);}
					enc imul(REG16 a,R_M16 b){Encode(519,a,b);}
					enc imul(REG32 a,REG32 b){Encode(520,a,b);}
					enc imul(REG32 a,MEM32 b){Encode(520,a,b);}
					enc imul(REG32 a,R_M32 b){Encode(520,a,b);}
					enc imul(REG64 a,REG64 b){Encode(521,a,b);}
					enc imul(REG64 a,MEM64 b){Encode(521,a,b);}
					enc imul(REG64 a,R_M64 b){Encode(521,a,b);}
					enc imul(REG16 a,byte b){Encode(522,a,(IMM)b);}
					enc imul(REG32 a,byte b){Encode(523,a,(IMM)b);}
					enc imul(REG64 a,byte b){Encode(524,a,(IMM)b);}
					enc imul(RAX a,byte b){Encode(524,a,(IMM)b);}
					enc imul(REG16 a,word b){Encode(525,a,(IMM)b);}
					enc imul(REG32 a,REF b){Encode(526,a,b);}
					enc imul(REG32 a,dword b){Encode(526,a,(IMM)b);}
					enc imul(REG64 a,REF b){Encode(527,a,b);}
					enc imul(REG64 a,dword b){Encode(527,a,(IMM)b);}
					enc imul(REG16 a,REG16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG16 a,AX b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG16 a,DX b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG16 a,CX b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG16 a,MEM16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG16 a,R_M16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(AX a,REG16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(AX a,MEM16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(AX a,R_M16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(DX a,REG16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(DX a,MEM16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(DX a,R_M16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(CX a,REG16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(CX a,MEM16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(CX a,R_M16 b,byte c){Encode(528,a,b,(IMM)c);}
					enc imul(REG32 a,REG32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(REG32 a,EAX b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(REG32 a,ECX b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(REG32 a,MEM32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(REG32 a,R_M32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(EAX a,REG32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(EAX a,MEM32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(EAX a,R_M32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(ECX a,REG32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(ECX a,MEM32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(ECX a,R_M32 b,byte c){Encode(529,a,b,(IMM)c);}
					enc imul(REG64 a,REG64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(REG64 a,RAX b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(REG64 a,MEM64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(REG64 a,R_M64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(RAX a,REG64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(RAX a,MEM64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(RAX a,R_M64 b,byte c){Encode(530,a,b,(IMM)c);}
					enc imul(REG16 a,REG16 b,word c){Encode(531,a,b,(IMM)c);}
					enc imul(REG16 a,MEM16 b,word c){Encode(531,a,b,(IMM)c);}
					enc imul(REG16 a,R_M16 b,word c){Encode(531,a,b,(IMM)c);}
					enc imul(REG32 a,REG32 b,REF c){Encode(532,a,b,c);}
					enc imul(REG32 a,REG32 b,dword c){Encode(532,a,b,(IMM)c);}
					enc imul(REG32 a,MEM32 b,REF c){Encode(532,a,b,c);}
					enc imul(REG32 a,MEM32 b,dword c){Encode(532,a,b,(IMM)c);}
					enc imul(REG32 a,R_M32 b,REF c){Encode(532,a,b,c);}
					enc imul(REG32 a,R_M32 b,dword c){Encode(532,a,b,(IMM)c);}
					enc imul(REG64 a,REG64 b,REF c){Encode(533,a,b,c);}
					enc imul(REG64 a,REG64 b,dword c){Encode(533,a,b,(IMM)c);}
					enc imul(REG64 a,MEM64 b,REF c){Encode(533,a,b,c);}
					enc imul(REG64 a,MEM64 b,dword c){Encode(533,a,b,(IMM)c);}
					enc imul(REG64 a,R_M64 b,REF c){Encode(533,a,b,c);}
					enc imul(REG64 a,R_M64 b,dword c){Encode(533,a,b,(IMM)c);}
					enc in(AL a,byte b){Encode(534,a,(IMM)b);}
					enc in(AX a,byte b){Encode(535,a,(IMM)b);}
					enc in(EAX a,byte b){Encode(536,a,(IMM)b);}
					enc in(AL a,DX b){Encode(537,a,b);}
					enc in(AX a,DX b){Encode(538,a,b);}
					enc in(EAX a,DX b){Encode(539,a,b);}
					enc inc(REG8 a){Encode(540,a);}
					enc inc(MEM8 a){Encode(540,a);}
					enc inc(R_M8 a){Encode(540,a);}
					enc inc(REG16 a){Encode(541,a);}
					enc inc(MEM16 a){Encode(541,a);}
					enc inc(R_M16 a){Encode(541,a);}
					enc inc(REG32 a){Encode(542,a);}
					enc inc(MEM32 a){Encode(542,a);}
					enc inc(R_M32 a){Encode(542,a);}
					enc inc(REG64 a){Encode(543,a);}
					enc inc(MEM64 a){Encode(543,a);}
					enc inc(R_M64 a){Encode(543,a);}
					enc insb(){Encode(544);}
					enc insd(){Encode(545);}
					enc insw(){Encode(546);}
					enc int03(){Encode(547);}
					enc int3(){Encode(548);}
					enc into(){Encode(549);}
					enc ja(byte a){Encode(550,(IMM)a);}
					enc ja(REF a){Encode(551,a);}
					enc ja(dword a){Encode(551,(IMM)a);}
					enc jae(byte a){Encode(552,(IMM)a);}
					enc jae(REF a){Encode(553,a);}
					enc jae(dword a){Encode(553,(IMM)a);}
					enc jb(byte a){Encode(554,(IMM)a);}
					enc jb(REF a){Encode(555,a);}
					enc jb(dword a){Encode(555,(IMM)a);}
					enc jbe(byte a){Encode(556,(IMM)a);}
					enc jbe(REF a){Encode(557,a);}
					enc jbe(dword a){Encode(557,(IMM)a);}
					enc jc(byte a){Encode(558,(IMM)a);}
					enc jc(REF a){Encode(559,a);}
					enc jc(dword a){Encode(559,(IMM)a);}
					enc jcxz(byte a){Encode(560,(IMM)a);}
					enc je(byte a){Encode(561,(IMM)a);}
					enc je(REF a){Encode(562,a);}
					enc je(dword a){Encode(562,(IMM)a);}
					enc jecxz(byte a){Encode(563,(IMM)a);}
					enc jg(byte a){Encode(564,(IMM)a);}
					enc jg(REF a){Encode(565,a);}
					enc jg(dword a){Encode(565,(IMM)a);}
					enc jge(byte a){Encode(566,(IMM)a);}
					enc jge(REF a){Encode(567,a);}
					enc jge(dword a){Encode(567,(IMM)a);}
					enc jl(byte a){Encode(568,(IMM)a);}
					enc jl(REF a){Encode(569,a);}
					enc jl(dword a){Encode(569,(IMM)a);}
					enc jle(byte a){Encode(570,(IMM)a);}
					enc jle(REF a){Encode(571,a);}
					enc jle(dword a){Encode(571,(IMM)a);}
					enc jmp(REF a){Encode(572,a);}
					enc jmp(dword a){Encode(572,(IMM)a);}
					enc jmp(MEM8 a){Encode(574,a);}
					enc jmp(MEM16 a){Encode(574,a);}
					enc jmp(MEM32 a){Encode(574,a);}
					enc jmp(MEM64 a){Encode(574,a);}
					enc jmp(MEM128 a){Encode(574,a);}
					enc jmp(REG16 a){Encode(575,a);}
					enc jmp(R_M16 a){Encode(575,a);}
					enc jmp(REG32 a){Encode(576,a);}
					enc jmp(R_M32 a){Encode(576,a);}
					enc jmp(REG64 a){Encode(577,a);}
					enc jmp(R_M64 a){Encode(577,a);}
					enc jna(byte a){Encode(578,(IMM)a);}
					enc jna(REF a){Encode(579,a);}
					enc jna(dword a){Encode(579,(IMM)a);}
					enc jnae(byte a){Encode(580,(IMM)a);}
					enc jnae(REF a){Encode(581,a);}
					enc jnae(dword a){Encode(581,(IMM)a);}
					enc jnb(byte a){Encode(582,(IMM)a);}
					enc jnb(REF a){Encode(583,a);}
					enc jnb(dword a){Encode(583,(IMM)a);}
					enc jnbe(byte a){Encode(584,(IMM)a);}
					enc jnbe(REF a){Encode(585,a);}
					enc jnbe(dword a){Encode(585,(IMM)a);}
					enc jnc(byte a){Encode(586,(IMM)a);}
					enc jnc(REF a){Encode(587,a);}
					enc jnc(dword a){Encode(587,(IMM)a);}
					enc jne(byte a){Encode(588,(IMM)a);}
					enc jne(REF a){Encode(589,a);}
					enc jne(dword a){Encode(589,(IMM)a);}
					enc jng(byte a){Encode(590,(IMM)a);}
					enc jng(REF a){Encode(591,a);}
					enc jng(dword a){Encode(591,(IMM)a);}
					enc jnge(byte a){Encode(592,(IMM)a);}
					enc jnge(REF a){Encode(593,a);}
					enc jnge(dword a){Encode(593,(IMM)a);}
					enc jnl(byte a){Encode(594,(IMM)a);}
					enc jnl(REF a){Encode(595,a);}
					enc jnl(dword a){Encode(595,(IMM)a);}
					enc jnle(byte a){Encode(596,(IMM)a);}
					enc jnle(REF a){Encode(597,a);}
					enc jnle(dword a){Encode(597,(IMM)a);}
					enc jno(byte a){Encode(598,(IMM)a);}
					enc jno(REF a){Encode(599,a);}
					enc jno(dword a){Encode(599,(IMM)a);}
					enc jnp(byte a){Encode(600,(IMM)a);}
					enc jnp(REF a){Encode(601,a);}
					enc jnp(dword a){Encode(601,(IMM)a);}
					enc jns(byte a){Encode(602,(IMM)a);}
					enc jns(REF a){Encode(603,a);}
					enc jns(dword a){Encode(603,(IMM)a);}
					enc jnz(byte a){Encode(604,(IMM)a);}
					enc jnz(REF a){Encode(605,a);}
					enc jnz(dword a){Encode(605,(IMM)a);}
					enc jo(byte a){Encode(606,(IMM)a);}
					enc jo(REF a){Encode(607,a);}
					enc jo(dword a){Encode(607,(IMM)a);}
					enc jp(byte a){Encode(608,(IMM)a);}
					enc jp(REF a){Encode(609,a);}
					enc jp(dword a){Encode(609,(IMM)a);}
					enc jpe(byte a){Encode(610,(IMM)a);}
					enc jpe(REF a){Encode(611,a);}
					enc jpe(dword a){Encode(611,(IMM)a);}
					enc jpo(byte a){Encode(612,(IMM)a);}
					enc jpo(REF a){Encode(613,a);}
					enc jpo(dword a){Encode(613,(IMM)a);}
					enc jrcxz(byte a){Encode(614,(IMM)a);}
					enc js(byte a){Encode(615,(IMM)a);}
					enc js(REF a){Encode(616,a);}
					enc js(dword a){Encode(616,(IMM)a);}
					enc jz(byte a){Encode(617,(IMM)a);}
					enc jz(REF a){Encode(618,a);}
					enc jz(dword a){Encode(618,(IMM)a);}
					enc lahf(){Encode(619);}
					enc lddqu(XMMREG a,MEM8 b){Encode(620,a,b);}
					enc lddqu(XMMREG a,MEM16 b){Encode(620,a,b);}
					enc lddqu(XMMREG a,MEM32 b){Encode(620,a,b);}
					enc lddqu(XMMREG a,MEM64 b){Encode(620,a,b);}
					enc lddqu(XMMREG a,MEM128 b){Encode(620,a,b);}
					enc ldmxcsr(MEM32 a){Encode(621,a);}
					enc lds(REG16 a,MEM8 b){Encode(622,a,b);}
					enc lds(REG16 a,MEM16 b){Encode(622,a,b);}
					enc lds(REG16 a,MEM32 b){Encode(622,a,b);}
					enc lds(REG16 a,MEM64 b){Encode(622,a,b);}
					enc lds(REG16 a,MEM128 b){Encode(622,a,b);}
					enc lds(REG32 a,MEM8 b){Encode(623,a,b);}
					enc lds(REG32 a,MEM16 b){Encode(623,a,b);}
					enc lds(REG32 a,MEM32 b){Encode(623,a,b);}
					enc lds(REG32 a,MEM64 b){Encode(623,a,b);}
					enc lds(REG32 a,MEM128 b){Encode(623,a,b);}
					enc lea(REG16 a,MEM8 b){Encode(624,a,b);}
					enc lea(REG16 a,MEM16 b){Encode(624,a,b);}
					enc lea(REG16 a,MEM32 b){Encode(624,a,b);}
					enc lea(REG16 a,MEM64 b){Encode(624,a,b);}
					enc lea(REG16 a,MEM128 b){Encode(624,a,b);}
					enc lea(REG32 a,MEM8 b){Encode(625,a,b);}
					enc lea(REG32 a,MEM16 b){Encode(625,a,b);}
					enc lea(REG32 a,MEM32 b){Encode(625,a,b);}
					enc lea(REG32 a,MEM64 b){Encode(625,a,b);}
					enc lea(REG32 a,MEM128 b){Encode(625,a,b);}
					enc lea(REG64 a,MEM8 b){Encode(626,a,b);}
					enc lea(REG64 a,MEM16 b){Encode(626,a,b);}
					enc lea(REG64 a,MEM32 b){Encode(626,a,b);}
					enc lea(REG64 a,MEM64 b){Encode(626,a,b);}
					enc lea(REG64 a,MEM128 b){Encode(626,a,b);}
					enc leave(){Encode(627);}
					enc les(REG16 a,MEM8 b){Encode(628,a,b);}
					enc les(REG16 a,MEM16 b){Encode(628,a,b);}
					enc les(REG16 a,MEM32 b){Encode(628,a,b);}
					enc les(REG16 a,MEM64 b){Encode(628,a,b);}
					enc les(REG16 a,MEM128 b){Encode(628,a,b);}
					enc les(REG32 a,MEM8 b){Encode(629,a,b);}
					enc les(REG32 a,MEM16 b){Encode(629,a,b);}
					enc les(REG32 a,MEM32 b){Encode(629,a,b);}
					enc les(REG32 a,MEM64 b){Encode(629,a,b);}
					enc les(REG32 a,MEM128 b){Encode(629,a,b);}
					enc lfence(){Encode(630);}
					enc lfs(REG16 a,MEM8 b){Encode(631,a,b);}
					enc lfs(REG16 a,MEM16 b){Encode(631,a,b);}
					enc lfs(REG16 a,MEM32 b){Encode(631,a,b);}
					enc lfs(REG16 a,MEM64 b){Encode(631,a,b);}
					enc lfs(REG16 a,MEM128 b){Encode(631,a,b);}
					enc lfs(REG32 a,MEM8 b){Encode(632,a,b);}
					enc lfs(REG32 a,MEM16 b){Encode(632,a,b);}
					enc lfs(REG32 a,MEM32 b){Encode(632,a,b);}
					enc lfs(REG32 a,MEM64 b){Encode(632,a,b);}
					enc lfs(REG32 a,MEM128 b){Encode(632,a,b);}
					enc lgs(REG16 a,MEM8 b){Encode(633,a,b);}
					enc lgs(REG16 a,MEM16 b){Encode(633,a,b);}
					enc lgs(REG16 a,MEM32 b){Encode(633,a,b);}
					enc lgs(REG16 a,MEM64 b){Encode(633,a,b);}
					enc lgs(REG16 a,MEM128 b){Encode(633,a,b);}
					enc lgs(REG32 a,MEM8 b){Encode(634,a,b);}
					enc lgs(REG32 a,MEM16 b){Encode(634,a,b);}
					enc lgs(REG32 a,MEM32 b){Encode(634,a,b);}
					enc lgs(REG32 a,MEM64 b){Encode(634,a,b);}
					enc lgs(REG32 a,MEM128 b){Encode(634,a,b);}
					enc lock_adc(MEM8 a,REG8 b){Encode(635,a,b);}
					enc lock_adc(MEM16 a,REG16 b){Encode(636,a,b);}
					enc lock_adc(MEM32 a,REG32 b){Encode(637,a,b);}
					enc lock_adc(MEM8 a,byte b){Encode(638,a,(IMM)b);}
					enc lock_adc(MEM16 a,byte b){Encode(639,a,(IMM)b);}
					enc lock_adc(MEM16 a,word b){Encode(639,a,(IMM)b);}
					enc lock_adc(MEM32 a,REF b){Encode(640,a,b);}
					enc lock_adc(MEM32 a,dword b){Encode(640,a,(IMM)b);}
					enc lock_add(MEM8 a,REG8 b){Encode(643,a,b);}
					enc lock_add(MEM16 a,REG16 b){Encode(644,a,b);}
					enc lock_add(MEM32 a,REG32 b){Encode(645,a,b);}
					enc lock_add(MEM8 a,byte b){Encode(646,a,(IMM)b);}
					enc lock_add(MEM16 a,byte b){Encode(647,a,(IMM)b);}
					enc lock_add(MEM16 a,word b){Encode(647,a,(IMM)b);}
					enc lock_add(MEM32 a,REF b){Encode(648,a,b);}
					enc lock_add(MEM32 a,dword b){Encode(648,a,(IMM)b);}
					enc lock_and(MEM8 a,REG8 b){Encode(651,a,b);}
					enc lock_and(MEM16 a,REG16 b){Encode(652,a,b);}
					enc lock_and(MEM32 a,REG32 b){Encode(653,a,b);}
					enc lock_and(MEM8 a,byte b){Encode(654,a,(IMM)b);}
					enc lock_and(MEM16 a,byte b){Encode(655,a,(IMM)b);}
					enc lock_and(MEM16 a,word b){Encode(655,a,(IMM)b);}
					enc lock_and(MEM32 a,REF b){Encode(656,a,b);}
					enc lock_and(MEM32 a,dword b){Encode(656,a,(IMM)b);}
					enc lock_btc(MEM16 a,REG16 b){Encode(659,a,b);}
					enc lock_btc(MEM32 a,REG32 b){Encode(660,a,b);}
					enc lock_btc(MEM16 a,byte b){Encode(661,a,(IMM)b);}
					enc lock_btc(MEM32 a,byte b){Encode(662,a,(IMM)b);}
					enc lock_btr(MEM16 a,REG16 b){Encode(663,a,b);}
					enc lock_btr(MEM32 a,REG32 b){Encode(664,a,b);}
					enc lock_btr(MEM16 a,byte b){Encode(665,a,(IMM)b);}
					enc lock_btr(MEM32 a,byte b){Encode(666,a,(IMM)b);}
					enc lock_bts(MEM16 a,REG16 b){Encode(667,a,b);}
					enc lock_bts(MEM32 a,REG32 b){Encode(668,a,b);}
					enc lock_bts(MEM16 a,byte b){Encode(669,a,(IMM)b);}
					enc lock_bts(MEM32 a,byte b){Encode(670,a,(IMM)b);}
					enc lock_cmpxchg(MEM8 a,REG8 b){Encode(671,a,b);}
					enc lock_cmpxchg(MEM16 a,REG16 b){Encode(672,a,b);}
					enc lock_cmpxchg(MEM32 a,REG32 b){Encode(673,a,b);}
					enc lock_cmpxchg8b(MEM8 a){Encode(674,a);}
					enc lock_cmpxchg8b(MEM16 a){Encode(674,a);}
					enc lock_cmpxchg8b(MEM32 a){Encode(674,a);}
					enc lock_cmpxchg8b(MEM64 a){Encode(674,a);}
					enc lock_cmpxchg8b(MEM128 a){Encode(674,a);}
					enc lock_dec(MEM8 a){Encode(675,a);}
					enc lock_dec(MEM16 a){Encode(676,a);}
					enc lock_dec(MEM32 a){Encode(677,a);}
					enc lock_inc(MEM8 a){Encode(678,a);}
					enc lock_inc(MEM16 a){Encode(679,a);}
					enc lock_inc(MEM32 a){Encode(680,a);}
					enc lock_neg(MEM8 a){Encode(681,a);}
					enc lock_neg(MEM16 a){Encode(682,a);}
					enc lock_neg(MEM32 a){Encode(683,a);}
					enc lock_not(MEM8 a){Encode(684,a);}
					enc lock_not(MEM16 a){Encode(685,a);}
					enc lock_not(MEM32 a){Encode(686,a);}
					enc lock_or(MEM8 a,REG8 b){Encode(687,a,b);}
					enc lock_or(MEM16 a,REG16 b){Encode(688,a,b);}
					enc lock_or(MEM32 a,REG32 b){Encode(689,a,b);}
					enc lock_or(MEM8 a,byte b){Encode(690,a,(IMM)b);}
					enc lock_or(MEM16 a,byte b){Encode(691,a,(IMM)b);}
					enc lock_or(MEM16 a,word b){Encode(691,a,(IMM)b);}
					enc lock_or(MEM32 a,REF b){Encode(692,a,b);}
					enc lock_or(MEM32 a,dword b){Encode(692,a,(IMM)b);}
					enc lock_sbb(MEM8 a,REG8 b){Encode(695,a,b);}
					enc lock_sbb(MEM16 a,REG16 b){Encode(696,a,b);}
					enc lock_sbb(MEM32 a,REG32 b){Encode(697,a,b);}
					enc lock_sbb(MEM8 a,byte b){Encode(698,a,(IMM)b);}
					enc lock_sbb(MEM16 a,byte b){Encode(699,a,(IMM)b);}
					enc lock_sbb(MEM16 a,word b){Encode(699,a,(IMM)b);}
					enc lock_sbb(MEM32 a,REF b){Encode(700,a,b);}
					enc lock_sbb(MEM32 a,dword b){Encode(700,a,(IMM)b);}
					enc lock_sub(MEM8 a,byte b){Encode(703,a,(IMM)b);}
					enc lock_sub(MEM16 a,byte b){Encode(704,a,(IMM)b);}
					enc lock_sub(MEM16 a,word b){Encode(704,a,(IMM)b);}
					enc lock_sub(MEM32 a,REF b){Encode(705,a,b);}
					enc lock_sub(MEM32 a,dword b){Encode(705,a,(IMM)b);}
					enc lock_sub(MEM8 a,REG8 b){Encode(708,a,b);}
					enc lock_sub(MEM16 a,REG16 b){Encode(709,a,b);}
					enc lock_sub(MEM32 a,REG32 b){Encode(710,a,b);}
					enc lock_xadd(MEM8 a,REG8 b){Encode(711,a,b);}
					enc lock_xadd(MEM16 a,REG16 b){Encode(712,a,b);}
					enc lock_xadd(MEM32 a,REG32 b){Encode(713,a,b);}
					enc lock_xchg(MEM8 a,REG8 b){Encode(714,a,b);}
					enc lock_xchg(MEM16 a,REG16 b){Encode(715,a,b);}
					enc lock_xchg(MEM32 a,REG32 b){Encode(716,a,b);}
					enc lock_xor(MEM8 a,REG8 b){Encode(717,a,b);}
					enc lock_xor(MEM16 a,REG16 b){Encode(718,a,b);}
					enc lock_xor(MEM32 a,REG32 b){Encode(719,a,b);}
					enc lock_xor(MEM8 a,byte b){Encode(720,a,(IMM)b);}
					enc lock_xor(MEM16 a,byte b){Encode(721,a,(IMM)b);}
					enc lock_xor(MEM16 a,word b){Encode(721,a,(IMM)b);}
					enc lock_xor(MEM32 a,REF b){Encode(722,a,b);}
					enc lock_xor(MEM32 a,dword b){Encode(722,a,(IMM)b);}
					enc lodsb(){Encode(725);}
					enc lodsd(){Encode(726);}
					enc lodsq(){Encode(727);}
					enc lodsw(){Encode(728);}
					enc loop(REF a){Encode(729,a);}
					enc loop(dword a){Encode(729,(IMM)a);}
					enc loop(REF a,CX b){Encode(730,a,b);}
					enc loop(dword a,CX b){Encode(730,(IMM)a,b);}
					enc loop(REF a,ECX b){Encode(731,a,b);}
					enc loop(dword a,ECX b){Encode(731,(IMM)a,b);}
					enc loope(REF a){Encode(732,a);}
					enc loope(dword a){Encode(732,(IMM)a);}
					enc loope(REF a,CX b){Encode(733,a,b);}
					enc loope(dword a,CX b){Encode(733,(IMM)a,b);}
					enc loope(REF a,ECX b){Encode(734,a,b);}
					enc loope(dword a,ECX b){Encode(734,(IMM)a,b);}
					enc loopne(REF a){Encode(735,a);}
					enc loopne(dword a){Encode(735,(IMM)a);}
					enc loopne(REF a,CX b){Encode(736,a,b);}
					enc loopne(dword a,CX b){Encode(736,(IMM)a,b);}
					enc loopne(REF a,ECX b){Encode(737,a,b);}
					enc loopne(dword a,ECX b){Encode(737,(IMM)a,b);}
					enc loopnz(REF a){Encode(738,a);}
					enc loopnz(dword a){Encode(738,(IMM)a);}
					enc loopnz(REF a,CX b){Encode(739,a,b);}
					enc loopnz(dword a,CX b){Encode(739,(IMM)a,b);}
					enc loopnz(REF a,ECX b){Encode(740,a,b);}
					enc loopnz(dword a,ECX b){Encode(740,(IMM)a,b);}
					enc loopz(REF a){Encode(741,a);}
					enc loopz(dword a){Encode(741,(IMM)a);}
					enc loopz(REF a,CX b){Encode(742,a,b);}
					enc loopz(dword a,CX b){Encode(742,(IMM)a,b);}
					enc loopz(REF a,ECX b){Encode(743,a,b);}
					enc loopz(dword a,ECX b){Encode(743,(IMM)a,b);}
					enc lss(REG16 a,MEM8 b){Encode(744,a,b);}
					enc lss(REG16 a,MEM16 b){Encode(744,a,b);}
					enc lss(REG16 a,MEM32 b){Encode(744,a,b);}
					enc lss(REG16 a,MEM64 b){Encode(744,a,b);}
					enc lss(REG16 a,MEM128 b){Encode(744,a,b);}
					enc lss(REG32 a,MEM8 b){Encode(745,a,b);}
					enc lss(REG32 a,MEM16 b){Encode(745,a,b);}
					enc lss(REG32 a,MEM32 b){Encode(745,a,b);}
					enc lss(REG32 a,MEM64 b){Encode(745,a,b);}
					enc lss(REG32 a,MEM128 b){Encode(745,a,b);}
					enc maskmovdqu(XMMREG a,XMMREG b){Encode(746,a,b);}
					enc maskmovq(MMREG a,MMREG b){Encode(747,a,b);}
					enc maxpd(XMMREG a,XMMREG b){Encode(748,a,b);}
					enc maxpd(XMMREG a,MEM128 b){Encode(748,a,b);}
					enc maxpd(XMMREG a,R_M128 b){Encode(748,a,b);}
					enc maxps(XMMREG a,XMMREG b){Encode(749,a,b);}
					enc maxps(XMMREG a,MEM128 b){Encode(749,a,b);}
					enc maxps(XMMREG a,R_M128 b){Encode(749,a,b);}
					enc maxsd(XMMREG a,XMMREG b){Encode(750,a,b);}
					enc maxsd(XMMREG a,MEM64 b){Encode(750,a,b);}
					enc maxsd(XMMREG a,XMM64 b){Encode(750,a,b);}
					enc maxss(XMMREG a,XMMREG b){Encode(751,a,b);}
					enc maxss(XMMREG a,MEM32 b){Encode(751,a,b);}
					enc maxss(XMMREG a,XMM32 b){Encode(751,a,b);}
					enc mfence(){Encode(752);}
					enc minpd(XMMREG a,XMMREG b){Encode(753,a,b);}
					enc minpd(XMMREG a,MEM128 b){Encode(753,a,b);}
					enc minpd(XMMREG a,R_M128 b){Encode(753,a,b);}
					enc minps(XMMREG a,XMMREG b){Encode(754,a,b);}
					enc minps(XMMREG a,MEM128 b){Encode(754,a,b);}
					enc minps(XMMREG a,R_M128 b){Encode(754,a,b);}
					enc minsd(XMMREG a,XMMREG b){Encode(755,a,b);}
					enc minsd(XMMREG a,MEM64 b){Encode(755,a,b);}
					enc minsd(XMMREG a,XMM64 b){Encode(755,a,b);}
					enc minss(XMMREG a,XMMREG b){Encode(756,a,b);}
					enc minss(XMMREG a,MEM32 b){Encode(756,a,b);}
					enc minss(XMMREG a,XMM32 b){Encode(756,a,b);}
					enc monitor(){Encode(757);}
					enc mov(REG8 a,REG8 b){Encode(758,a,b);}
					enc mov(MEM8 a,REG8 b){Encode(758,a,b);}
					enc mov(R_M8 a,REG8 b){Encode(758,a,b);}
					enc mov(REG16 a,REG16 b){Encode(759,a,b);}
					enc mov(MEM16 a,REG16 b){Encode(759,a,b);}
					enc mov(R_M16 a,REG16 b){Encode(759,a,b);}
					enc mov(REG32 a,REG32 b){Encode(760,a,b);}
					enc mov(MEM32 a,REG32 b){Encode(760,a,b);}
					enc mov(R_M32 a,REG32 b){Encode(760,a,b);}
					enc mov(REG64 a,REG64 b){Encode(761,a,b);}
					enc mov(MEM64 a,REG64 b){Encode(761,a,b);}
					enc mov(R_M64 a,REG64 b){Encode(761,a,b);}
					enc mov(REG8 a,MEM8 b){Encode(762,a,b);}
					enc mov(REG8 a,R_M8 b){Encode(762,a,b);}
					enc mov(REG16 a,MEM16 b){Encode(763,a,b);}
					enc mov(REG16 a,R_M16 b){Encode(763,a,b);}
					enc mov(REG32 a,MEM32 b){Encode(764,a,b);}
					enc mov(REG32 a,R_M32 b){Encode(764,a,b);}
					enc mov(REG64 a,MEM64 b){Encode(765,a,b);}
					enc mov(REG64 a,R_M64 b){Encode(765,a,b);}
					enc mov(REG8 a,byte b){Encode(766,a,(IMM)b);}
					enc mov(AL a,byte b){Encode(766,a,(IMM)b);}
					enc mov(CL a,byte b){Encode(766,a,(IMM)b);}
					enc mov(REG16 a,byte b){Encode(767,a,(IMM)b);}
					enc mov(REG16 a,word b){Encode(767,a,(IMM)b);}
					enc mov(REG32 a,REF b){Encode(768,a,b);}
					enc mov(REG32 a,dword b){Encode(768,a,(IMM)b);}
					enc mov(MEM8 a,byte b){Encode(769,a,(IMM)b);}
					enc mov(R_M8 a,byte b){Encode(769,a,(IMM)b);}
					enc mov(MEM16 a,byte b){Encode(770,a,(IMM)b);}
					enc mov(MEM16 a,word b){Encode(770,a,(IMM)b);}
					enc mov(R_M16 a,byte b){Encode(770,a,(IMM)b);}
					enc mov(R_M16 a,word b){Encode(770,a,(IMM)b);}
					enc mov(MEM32 a,REF b){Encode(771,a,b);}
					enc mov(MEM32 a,dword b){Encode(771,a,(IMM)b);}
					enc mov(R_M32 a,REF b){Encode(771,a,b);}
					enc mov(R_M32 a,dword b){Encode(771,a,(IMM)b);}
					enc mov(REG64 a,REF b){Encode(772,a,b);}
					enc mov(REG64 a,dword b){Encode(772,a,(IMM)b);}
					enc mov(MEM64 a,REF b){Encode(772,a,b);}
					enc mov(MEM64 a,dword b){Encode(772,a,(IMM)b);}
					enc mov(R_M64 a,REF b){Encode(772,a,b);}
					enc mov(R_M64 a,dword b){Encode(772,a,(IMM)b);}
					enc movapd(XMMREG a,XMMREG b){Encode(773,a,b);}
					enc movapd(XMMREG a,MEM128 b){Encode(773,a,b);}
					enc movapd(XMMREG a,R_M128 b){Encode(773,a,b);}
					enc movapd(MEM128 a,XMMREG b){Encode(774,a,b);}
					enc movapd(R_M128 a,XMMREG b){Encode(774,a,b);}
					enc movaps(XMMREG a,XMMREG b){Encode(775,a,b);}
					enc movaps(XMMREG a,MEM128 b){Encode(775,a,b);}
					enc movaps(XMMREG a,R_M128 b){Encode(775,a,b);}
					enc movaps(MEM128 a,XMMREG b){Encode(776,a,b);}
					enc movaps(R_M128 a,XMMREG b){Encode(776,a,b);}
					enc movd(MMREG a,REG32 b){Encode(777,a,b);}
					enc movd(MMREG a,MEM32 b){Encode(777,a,b);}
					enc movd(MMREG a,R_M32 b){Encode(777,a,b);}
					enc movd(MMREG a,REG64 b){Encode(778,a,b);}
					enc movd(MMREG a,MEM64 b){Encode(778,a,b);}
					enc movd(MMREG a,R_M64 b){Encode(778,a,b);}
					enc movd(REG32 a,MMREG b){Encode(779,a,b);}
					enc movd(MEM32 a,MMREG b){Encode(779,a,b);}
					enc movd(R_M32 a,MMREG b){Encode(779,a,b);}
					enc movd(REG64 a,MMREG b){Encode(780,a,b);}
					enc movd(MEM64 a,MMREG b){Encode(780,a,b);}
					enc movd(R_M64 a,MMREG b){Encode(780,a,b);}
					enc movd(XMMREG a,REG32 b){Encode(781,a,b);}
					enc movd(XMMREG a,MEM32 b){Encode(781,a,b);}
					enc movd(XMMREG a,R_M32 b){Encode(781,a,b);}
					enc movd(XMMREG a,REG64 b){Encode(782,a,b);}
					enc movd(XMMREG a,MEM64 b){Encode(782,a,b);}
					enc movd(XMMREG a,R_M64 b){Encode(782,a,b);}
					enc movd(REG32 a,XMMREG b){Encode(783,a,b);}
					enc movd(MEM32 a,XMMREG b){Encode(783,a,b);}
					enc movd(R_M32 a,XMMREG b){Encode(783,a,b);}
					enc movd(REG64 a,XMMREG b){Encode(784,a,b);}
					enc movd(MEM64 a,XMMREG b){Encode(784,a,b);}
					enc movd(R_M64 a,XMMREG b){Encode(784,a,b);}
					enc movddup(XMMREG a,XMMREG b){Encode(785,a,b);}
					enc movddup(XMMREG a,MEM128 b){Encode(785,a,b);}
					enc movddup(XMMREG a,R_M128 b){Encode(785,a,b);}
					enc movdq2q(MMREG a,XMMREG b){Encode(786,a,b);}
					enc movdqa(XMMREG a,XMMREG b){Encode(787,a,b);}
					enc movdqa(XMMREG a,MEM128 b){Encode(787,a,b);}
					enc movdqa(XMMREG a,R_M128 b){Encode(787,a,b);}
					enc movdqa(MEM128 a,XMMREG b){Encode(788,a,b);}
					enc movdqa(R_M128 a,XMMREG b){Encode(788,a,b);}
					enc movdqu(XMMREG a,XMMREG b){Encode(789,a,b);}
					enc movdqu(XMMREG a,MEM128 b){Encode(789,a,b);}
					enc movdqu(XMMREG a,R_M128 b){Encode(789,a,b);}
					enc movdqu(MEM128 a,XMMREG b){Encode(790,a,b);}
					enc movdqu(R_M128 a,XMMREG b){Encode(790,a,b);}
					enc movhlps(XMMREG a,XMMREG b){Encode(791,a,b);}
					enc movhpd(XMMREG a,MEM64 b){Encode(792,a,b);}
					enc movhpd(MEM64 a,XMMREG b){Encode(793,a,b);}
					enc movhps(XMMREG a,MEM64 b){Encode(794,a,b);}
					enc movhps(MEM64 a,XMMREG b){Encode(795,a,b);}
					enc movhps(XMMREG a,XMMREG b){Encode(796,a,b);}
					enc movlhps(XMMREG a,XMMREG b){Encode(797,a,b);}
					enc movlpd(XMMREG a,MEM64 b){Encode(798,a,b);}
					enc movlpd(MEM64 a,XMMREG b){Encode(799,a,b);}
					enc movlps(XMMREG a,MEM64 b){Encode(800,a,b);}
					enc movlps(MEM64 a,XMMREG b){Encode(801,a,b);}
					enc movmskpd(REG32 a,XMMREG b){Encode(802,a,b);}
					enc movmskps(REG32 a,XMMREG b){Encode(803,a,b);}
					enc movntdq(MEM128 a,XMMREG b){Encode(804,a,b);}
					enc movnti(MEM32 a,REG32 b){Encode(805,a,b);}
					enc movnti(MEM64 a,REG64 b){Encode(806,a,b);}
					enc movntpd(MEM128 a,XMMREG b){Encode(807,a,b);}
					enc movntps(MEM128 a,XMMREG b){Encode(808,a,b);}
					enc movntq(MEM64 a,MMREG b){Encode(809,a,b);}
					enc movq(MMREG a,MMREG b){Encode(810,a,b);}
					enc movq(MMREG a,MEM64 b){Encode(810,a,b);}
					enc movq(MMREG a,MM64 b){Encode(810,a,b);}
					enc movq(MEM64 a,MMREG b){Encode(811,a,b);}
					enc movq(MM64 a,MMREG b){Encode(811,a,b);}
					enc movq(XMMREG a,XMMREG b){Encode(812,a,b);}
					enc movq(XMMREG a,MEM64 b){Encode(812,a,b);}
					enc movq(XMMREG a,XMM64 b){Encode(812,a,b);}
					enc movq(MEM64 a,XMMREG b){Encode(813,a,b);}
					enc movq(XMM64 a,XMMREG b){Encode(813,a,b);}
					enc movq2dq(XMMREG a,MMREG b){Encode(814,a,b);}
					enc movsb(){Encode(815);}
					enc movsd(){Encode(816);}
					enc movsd(XMMREG a,XMMREG b){Encode(817,a,b);}
					enc movsd(XMMREG a,MEM64 b){Encode(817,a,b);}
					enc movsd(XMMREG a,XMM64 b){Encode(817,a,b);}
					enc movsd(MEM64 a,XMMREG b){Encode(818,a,b);}
					enc movsd(XMM64 a,XMMREG b){Encode(818,a,b);}
					enc movshdup(XMMREG a,XMMREG b){Encode(819,a,b);}
					enc movshdup(XMMREG a,MEM128 b){Encode(819,a,b);}
					enc movshdup(XMMREG a,R_M128 b){Encode(819,a,b);}
					enc movsldup(XMMREG a,XMMREG b){Encode(820,a,b);}
					enc movsldup(XMMREG a,MEM128 b){Encode(820,a,b);}
					enc movsldup(XMMREG a,R_M128 b){Encode(820,a,b);}
					enc movsq(){Encode(821);}
					enc movss(XMMREG a,XMMREG b){Encode(822,a,b);}
					enc movss(XMMREG a,MEM32 b){Encode(822,a,b);}
					enc movss(XMMREG a,XMM32 b){Encode(822,a,b);}
					enc movss(MEM32 a,XMMREG b){Encode(823,a,b);}
					enc movss(XMM32 a,XMMREG b){Encode(823,a,b);}
					enc movsw(){Encode(824);}
					enc movsx(REG16 a,REG8 b){Encode(825,a,b);}
					enc movsx(REG16 a,MEM8 b){Encode(825,a,b);}
					enc movsx(REG16 a,R_M8 b){Encode(825,a,b);}
					enc movsx(REG32 a,REG8 b){Encode(826,a,b);}
					enc movsx(REG32 a,MEM8 b){Encode(826,a,b);}
					enc movsx(REG32 a,R_M8 b){Encode(826,a,b);}
					enc movsx(REG64 a,REG8 b){Encode(827,a,b);}
					enc movsx(REG64 a,MEM8 b){Encode(827,a,b);}
					enc movsx(REG64 a,R_M8 b){Encode(827,a,b);}
					enc movsx(REG32 a,REG16 b){Encode(828,a,b);}
					enc movsx(REG32 a,MEM16 b){Encode(828,a,b);}
					enc movsx(REG32 a,R_M16 b){Encode(828,a,b);}
					enc movsx(REG64 a,REG16 b){Encode(829,a,b);}
					enc movsx(REG64 a,MEM16 b){Encode(829,a,b);}
					enc movsx(REG64 a,R_M16 b){Encode(829,a,b);}
					enc movsxd(REG64 a,REG32 b){Encode(830,a,b);}
					enc movsxd(REG64 a,MEM32 b){Encode(830,a,b);}
					enc movsxd(REG64 a,R_M32 b){Encode(830,a,b);}
					enc movupd(XMMREG a,XMMREG b){Encode(831,a,b);}
					enc movupd(XMMREG a,MEM128 b){Encode(831,a,b);}
					enc movupd(XMMREG a,R_M128 b){Encode(831,a,b);}
					enc movupd(MEM128 a,XMMREG b){Encode(832,a,b);}
					enc movupd(R_M128 a,XMMREG b){Encode(832,a,b);}
					enc movups(XMMREG a,XMMREG b){Encode(833,a,b);}
					enc movups(XMMREG a,MEM128 b){Encode(833,a,b);}
					enc movups(XMMREG a,R_M128 b){Encode(833,a,b);}
					enc movups(MEM128 a,XMMREG b){Encode(834,a,b);}
					enc movups(R_M128 a,XMMREG b){Encode(834,a,b);}
					enc movzx(REG16 a,REG8 b){Encode(835,a,b);}
					enc movzx(REG16 a,MEM8 b){Encode(835,a,b);}
					enc movzx(REG16 a,R_M8 b){Encode(835,a,b);}
					enc movzx(REG32 a,REG8 b){Encode(836,a,b);}
					enc movzx(REG32 a,MEM8 b){Encode(836,a,b);}
					enc movzx(REG32 a,R_M8 b){Encode(836,a,b);}
					enc movzx(REG64 a,REG8 b){Encode(837,a,b);}
					enc movzx(REG64 a,MEM8 b){Encode(837,a,b);}
					enc movzx(REG64 a,R_M8 b){Encode(837,a,b);}
					enc movzx(REG32 a,REG16 b){Encode(838,a,b);}
					enc movzx(REG32 a,MEM16 b){Encode(838,a,b);}
					enc movzx(REG32 a,R_M16 b){Encode(838,a,b);}
					enc movzx(REG64 a,REG16 b){Encode(839,a,b);}
					enc movzx(REG64 a,MEM16 b){Encode(839,a,b);}
					enc movzx(REG64 a,R_M16 b){Encode(839,a,b);}
					enc mul(REG8 a){Encode(840,a);}
					enc mul(MEM8 a){Encode(840,a);}
					enc mul(R_M8 a){Encode(840,a);}
					enc mul(REG16 a){Encode(841,a);}
					enc mul(MEM16 a){Encode(841,a);}
					enc mul(R_M16 a){Encode(841,a);}
					enc mul(REG32 a){Encode(842,a);}
					enc mul(MEM32 a){Encode(842,a);}
					enc mul(R_M32 a){Encode(842,a);}
					enc mul(REG64 a){Encode(843,a);}
					enc mul(MEM64 a){Encode(843,a);}
					enc mul(R_M64 a){Encode(843,a);}
					enc mulpd(XMMREG a,XMMREG b){Encode(844,a,b);}
					enc mulpd(XMMREG a,MEM128 b){Encode(844,a,b);}
					enc mulpd(XMMREG a,R_M128 b){Encode(844,a,b);}
					enc mulps(XMMREG a,XMMREG b){Encode(845,a,b);}
					enc mulps(XMMREG a,MEM128 b){Encode(845,a,b);}
					enc mulps(XMMREG a,R_M128 b){Encode(845,a,b);}
					enc mulsd(XMMREG a,XMMREG b){Encode(846,a,b);}
					enc mulsd(XMMREG a,MEM64 b){Encode(846,a,b);}
					enc mulsd(XMMREG a,XMM64 b){Encode(846,a,b);}
					enc mulss(XMMREG a,XMMREG b){Encode(847,a,b);}
					enc mulss(XMMREG a,MEM32 b){Encode(847,a,b);}
					enc mulss(XMMREG a,XMM32 b){Encode(847,a,b);}
					enc mwait(){Encode(848);}
					enc neg(REG8 a){Encode(849,a);}
					enc neg(MEM8 a){Encode(849,a);}
					enc neg(R_M8 a){Encode(849,a);}
					enc neg(REG16 a){Encode(850,a);}
					enc neg(MEM16 a){Encode(850,a);}
					enc neg(R_M16 a){Encode(850,a);}
					enc neg(REG32 a){Encode(851,a);}
					enc neg(MEM32 a){Encode(851,a);}
					enc neg(R_M32 a){Encode(851,a);}
					enc neg(REG64 a){Encode(852,a);}
					enc neg(MEM64 a){Encode(852,a);}
					enc neg(R_M64 a){Encode(852,a);}
					enc nop(){Encode(853);}
					enc not(REG8 a){Encode(854,a);}
					enc not(MEM8 a){Encode(854,a);}
					enc not(R_M8 a){Encode(854,a);}
					enc not(REG16 a){Encode(855,a);}
					enc not(MEM16 a){Encode(855,a);}
					enc not(R_M16 a){Encode(855,a);}
					enc not(REG32 a){Encode(856,a);}
					enc not(MEM32 a){Encode(856,a);}
					enc not(R_M32 a){Encode(856,a);}
					enc not(REG64 a){Encode(857,a);}
					enc not(MEM64 a){Encode(857,a);}
					enc not(R_M64 a){Encode(857,a);}
					enc null(){Encode(858);}
					enc or(REG8 a,REG8 b){Encode(859,a,b);}
					enc or(MEM8 a,REG8 b){Encode(859,a,b);}
					enc or(R_M8 a,REG8 b){Encode(859,a,b);}
					enc or(REG16 a,REG16 b){Encode(860,a,b);}
					enc or(MEM16 a,REG16 b){Encode(860,a,b);}
					enc or(R_M16 a,REG16 b){Encode(860,a,b);}
					enc or(REG32 a,REG32 b){Encode(861,a,b);}
					enc or(MEM32 a,REG32 b){Encode(861,a,b);}
					enc or(R_M32 a,REG32 b){Encode(861,a,b);}
					enc or(REG64 a,REG64 b){Encode(862,a,b);}
					enc or(MEM64 a,REG64 b){Encode(862,a,b);}
					enc or(R_M64 a,REG64 b){Encode(862,a,b);}
					enc or(REG8 a,MEM8 b){Encode(863,a,b);}
					enc or(REG8 a,R_M8 b){Encode(863,a,b);}
					enc or(REG16 a,MEM16 b){Encode(864,a,b);}
					enc or(REG16 a,R_M16 b){Encode(864,a,b);}
					enc or(REG32 a,MEM32 b){Encode(865,a,b);}
					enc or(REG32 a,R_M32 b){Encode(865,a,b);}
					enc or(REG64 a,MEM64 b){Encode(866,a,b);}
					enc or(REG64 a,R_M64 b){Encode(866,a,b);}
					enc or(REG8 a,byte b){Encode(867,a,(IMM)b);}
					enc or(AL a,byte b){Encode(867,a,(IMM)b);}
					enc or(CL a,byte b){Encode(867,a,(IMM)b);}
					enc or(MEM8 a,byte b){Encode(867,a,(IMM)b);}
					enc or(R_M8 a,byte b){Encode(867,a,(IMM)b);}
					enc or(REG16 a,byte b){Encode(868,a,(IMM)b);}
					enc or(REG16 a,word b){Encode(868,a,(IMM)b);}
					enc or(MEM16 a,byte b){Encode(868,a,(IMM)b);}
					enc or(MEM16 a,word b){Encode(868,a,(IMM)b);}
					enc or(R_M16 a,byte b){Encode(868,a,(IMM)b);}
					enc or(R_M16 a,word b){Encode(868,a,(IMM)b);}
					enc or(REG32 a,REF b){Encode(869,a,b);}
					enc or(REG32 a,dword b){Encode(869,a,(IMM)b);}
					enc or(MEM32 a,REF b){Encode(869,a,b);}
					enc or(MEM32 a,dword b){Encode(869,a,(IMM)b);}
					enc or(R_M32 a,REF b){Encode(869,a,b);}
					enc or(R_M32 a,dword b){Encode(869,a,(IMM)b);}
					enc or(REG64 a,REF b){Encode(870,a,b);}
					enc or(REG64 a,dword b){Encode(870,a,(IMM)b);}
					enc or(MEM64 a,REF b){Encode(870,a,b);}
					enc or(MEM64 a,dword b){Encode(870,a,(IMM)b);}
					enc or(R_M64 a,REF b){Encode(870,a,b);}
					enc or(R_M64 a,dword b){Encode(870,a,(IMM)b);}
					enc orpd(XMMREG a,XMMREG b){Encode(878,a,b);}
					enc orpd(XMMREG a,MEM128 b){Encode(878,a,b);}
					enc orpd(XMMREG a,R_M128 b){Encode(878,a,b);}
					enc orps(XMMREG a,XMMREG b){Encode(879,a,b);}
					enc orps(XMMREG a,MEM128 b){Encode(879,a,b);}
					enc orps(XMMREG a,R_M128 b){Encode(879,a,b);}
					enc out(byte a,AL b){Encode(880,(IMM)a,b);}
					enc out(byte a,AX b){Encode(881,(IMM)a,b);}
					enc out(byte a,EAX b){Encode(882,(IMM)a,b);}
					enc out(DX a,AL b){Encode(883,a,b);}
					enc out(DX a,AX b){Encode(884,a,b);}
					enc out(DX a,EAX b){Encode(885,a,b);}
					enc outsb(){Encode(886);}
					enc outsd(){Encode(887);}
					enc outsw(){Encode(888);}
					enc packssdw(MMREG a,MMREG b){Encode(889,a,b);}
					enc packssdw(MMREG a,MEM64 b){Encode(889,a,b);}
					enc packssdw(MMREG a,MM64 b){Encode(889,a,b);}
					enc packssdw(XMMREG a,XMMREG b){Encode(890,a,b);}
					enc packssdw(XMMREG a,MEM128 b){Encode(890,a,b);}
					enc packssdw(XMMREG a,R_M128 b){Encode(890,a,b);}
					enc packsswb(MMREG a,MMREG b){Encode(891,a,b);}
					enc packsswb(MMREG a,MEM64 b){Encode(891,a,b);}
					enc packsswb(MMREG a,MM64 b){Encode(891,a,b);}
					enc packsswb(XMMREG a,XMMREG b){Encode(892,a,b);}
					enc packsswb(XMMREG a,MEM128 b){Encode(892,a,b);}
					enc packsswb(XMMREG a,R_M128 b){Encode(892,a,b);}
					enc packuswb(MMREG a,MMREG b){Encode(893,a,b);}
					enc packuswb(MMREG a,MEM64 b){Encode(893,a,b);}
					enc packuswb(MMREG a,MM64 b){Encode(893,a,b);}
					enc packuswb(XMMREG a,XMMREG b){Encode(894,a,b);}
					enc packuswb(XMMREG a,MEM128 b){Encode(894,a,b);}
					enc packuswb(XMMREG a,R_M128 b){Encode(894,a,b);}
					enc paddb(MMREG a,MMREG b){Encode(895,a,b);}
					enc paddb(MMREG a,MEM64 b){Encode(895,a,b);}
					enc paddb(MMREG a,MM64 b){Encode(895,a,b);}
					enc paddb(XMMREG a,XMMREG b){Encode(896,a,b);}
					enc paddb(XMMREG a,MEM128 b){Encode(896,a,b);}
					enc paddb(XMMREG a,R_M128 b){Encode(896,a,b);}
					enc paddd(MMREG a,MMREG b){Encode(897,a,b);}
					enc paddd(MMREG a,MEM64 b){Encode(897,a,b);}
					enc paddd(MMREG a,MM64 b){Encode(897,a,b);}
					enc paddd(XMMREG a,XMMREG b){Encode(898,a,b);}
					enc paddd(XMMREG a,MEM128 b){Encode(898,a,b);}
					enc paddd(XMMREG a,R_M128 b){Encode(898,a,b);}
					enc paddq(MMREG a,MMREG b){Encode(899,a,b);}
					enc paddq(MMREG a,MEM64 b){Encode(899,a,b);}
					enc paddq(MMREG a,MM64 b){Encode(899,a,b);}
					enc paddq(XMMREG a,XMMREG b){Encode(900,a,b);}
					enc paddq(XMMREG a,MEM128 b){Encode(900,a,b);}
					enc paddq(XMMREG a,R_M128 b){Encode(900,a,b);}
					enc paddsb(MMREG a,MMREG b){Encode(901,a,b);}
					enc paddsb(MMREG a,MEM64 b){Encode(901,a,b);}
					enc paddsb(MMREG a,MM64 b){Encode(901,a,b);}
					enc paddsb(XMMREG a,XMMREG b){Encode(902,a,b);}
					enc paddsb(XMMREG a,MEM128 b){Encode(902,a,b);}
					enc paddsb(XMMREG a,R_M128 b){Encode(902,a,b);}
					enc paddsiw(MMREG a,MMREG b){Encode(903,a,b);}
					enc paddsiw(MMREG a,MEM64 b){Encode(903,a,b);}
					enc paddsiw(MMREG a,MM64 b){Encode(903,a,b);}
					enc paddsw(MMREG a,MMREG b){Encode(904,a,b);}
					enc paddsw(MMREG a,MEM64 b){Encode(904,a,b);}
					enc paddsw(MMREG a,MM64 b){Encode(904,a,b);}
					enc paddsw(XMMREG a,XMMREG b){Encode(905,a,b);}
					enc paddsw(XMMREG a,MEM128 b){Encode(905,a,b);}
					enc paddsw(XMMREG a,R_M128 b){Encode(905,a,b);}
					enc paddusb(MMREG a,MMREG b){Encode(906,a,b);}
					enc paddusb(MMREG a,MEM64 b){Encode(906,a,b);}
					enc paddusb(MMREG a,MM64 b){Encode(906,a,b);}
					enc paddusb(XMMREG a,XMMREG b){Encode(907,a,b);}
					enc paddusb(XMMREG a,MEM128 b){Encode(907,a,b);}
					enc paddusb(XMMREG a,R_M128 b){Encode(907,a,b);}
					enc paddusw(MMREG a,MMREG b){Encode(908,a,b);}
					enc paddusw(MMREG a,MEM64 b){Encode(908,a,b);}
					enc paddusw(MMREG a,MM64 b){Encode(908,a,b);}
					enc paddusw(XMMREG a,XMMREG b){Encode(909,a,b);}
					enc paddusw(XMMREG a,MEM128 b){Encode(909,a,b);}
					enc paddusw(XMMREG a,R_M128 b){Encode(909,a,b);}
					enc paddw(MMREG a,MMREG b){Encode(910,a,b);}
					enc paddw(MMREG a,MEM64 b){Encode(910,a,b);}
					enc paddw(MMREG a,MM64 b){Encode(910,a,b);}
					enc paddw(XMMREG a,XMMREG b){Encode(911,a,b);}
					enc paddw(XMMREG a,MEM128 b){Encode(911,a,b);}
					enc paddw(XMMREG a,R_M128 b){Encode(911,a,b);}
					enc pand(MMREG a,MMREG b){Encode(912,a,b);}
					enc pand(MMREG a,MEM64 b){Encode(912,a,b);}
					enc pand(MMREG a,MM64 b){Encode(912,a,b);}
					enc pand(XMMREG a,XMMREG b){Encode(913,a,b);}
					enc pand(XMMREG a,MEM128 b){Encode(913,a,b);}
					enc pand(XMMREG a,R_M128 b){Encode(913,a,b);}
					enc pandn(MMREG a,MMREG b){Encode(914,a,b);}
					enc pandn(MMREG a,MEM64 b){Encode(914,a,b);}
					enc pandn(MMREG a,MM64 b){Encode(914,a,b);}
					enc pandn(XMMREG a,XMMREG b){Encode(915,a,b);}
					enc pandn(XMMREG a,MEM128 b){Encode(915,a,b);}
					enc pandn(XMMREG a,R_M128 b){Encode(915,a,b);}
					enc pause(){Encode(916);}
					enc paveb(MMREG a,MMREG b){Encode(917,a,b);}
					enc paveb(MMREG a,MEM64 b){Encode(917,a,b);}
					enc paveb(MMREG a,MM64 b){Encode(917,a,b);}
					enc pavgb(MMREG a,MMREG b){Encode(918,a,b);}
					enc pavgb(MMREG a,MEM64 b){Encode(918,a,b);}
					enc pavgb(MMREG a,MM64 b){Encode(918,a,b);}
					enc pavgb(XMMREG a,XMMREG b){Encode(919,a,b);}
					enc pavgb(XMMREG a,MEM128 b){Encode(919,a,b);}
					enc pavgb(XMMREG a,R_M128 b){Encode(919,a,b);}
					enc pavgusb(MMREG a,MMREG b){Encode(920,a,b);}
					enc pavgusb(MMREG a,MEM64 b){Encode(920,a,b);}
					enc pavgusb(MMREG a,MM64 b){Encode(920,a,b);}
					enc pavgw(MMREG a,MMREG b){Encode(921,a,b);}
					enc pavgw(MMREG a,MEM64 b){Encode(921,a,b);}
					enc pavgw(MMREG a,MM64 b){Encode(921,a,b);}
					enc pavgw(XMMREG a,XMMREG b){Encode(922,a,b);}
					enc pavgw(XMMREG a,MEM128 b){Encode(922,a,b);}
					enc pavgw(XMMREG a,R_M128 b){Encode(922,a,b);}
					enc pcmpeqb(MMREG a,MMREG b){Encode(923,a,b);}
					enc pcmpeqb(MMREG a,MEM64 b){Encode(923,a,b);}
					enc pcmpeqb(MMREG a,MM64 b){Encode(923,a,b);}
					enc pcmpeqb(XMMREG a,XMMREG b){Encode(924,a,b);}
					enc pcmpeqb(XMMREG a,MEM128 b){Encode(924,a,b);}
					enc pcmpeqb(XMMREG a,R_M128 b){Encode(924,a,b);}
					enc pcmpeqd(MMREG a,MMREG b){Encode(925,a,b);}
					enc pcmpeqd(MMREG a,MEM64 b){Encode(925,a,b);}
					enc pcmpeqd(MMREG a,MM64 b){Encode(925,a,b);}
					enc pcmpeqd(XMMREG a,XMMREG b){Encode(926,a,b);}
					enc pcmpeqd(XMMREG a,MEM128 b){Encode(926,a,b);}
					enc pcmpeqd(XMMREG a,R_M128 b){Encode(926,a,b);}
					enc pcmpeqw(MMREG a,MMREG b){Encode(927,a,b);}
					enc pcmpeqw(MMREG a,MEM64 b){Encode(927,a,b);}
					enc pcmpeqw(MMREG a,MM64 b){Encode(927,a,b);}
					enc pcmpeqw(XMMREG a,XMMREG b){Encode(928,a,b);}
					enc pcmpeqw(XMMREG a,MEM128 b){Encode(928,a,b);}
					enc pcmpeqw(XMMREG a,R_M128 b){Encode(928,a,b);}
					enc pcmpgtb(MMREG a,MMREG b){Encode(929,a,b);}
					enc pcmpgtb(MMREG a,MEM64 b){Encode(929,a,b);}
					enc pcmpgtb(MMREG a,MM64 b){Encode(929,a,b);}
					enc pcmpgtb(XMMREG a,XMMREG b){Encode(930,a,b);}
					enc pcmpgtb(XMMREG a,MEM128 b){Encode(930,a,b);}
					enc pcmpgtb(XMMREG a,R_M128 b){Encode(930,a,b);}
					enc pcmpgtd(MMREG a,MMREG b){Encode(931,a,b);}
					enc pcmpgtd(MMREG a,MEM64 b){Encode(931,a,b);}
					enc pcmpgtd(MMREG a,MM64 b){Encode(931,a,b);}
					enc pcmpgtd(XMMREG a,XMMREG b){Encode(932,a,b);}
					enc pcmpgtd(XMMREG a,MEM128 b){Encode(932,a,b);}
					enc pcmpgtd(XMMREG a,R_M128 b){Encode(932,a,b);}
					enc pcmpgtw(MMREG a,MMREG b){Encode(933,a,b);}
					enc pcmpgtw(MMREG a,MEM64 b){Encode(933,a,b);}
					enc pcmpgtw(MMREG a,MM64 b){Encode(933,a,b);}
					enc pcmpgtw(XMMREG a,XMMREG b){Encode(934,a,b);}
					enc pcmpgtw(XMMREG a,MEM128 b){Encode(934,a,b);}
					enc pcmpgtw(XMMREG a,R_M128 b){Encode(934,a,b);}
					enc pdistib(MMREG a,MEM64 b){Encode(935,a,b);}
					enc pextrw(REG32 a,MMREG b,byte c){Encode(936,a,b,(IMM)c);}
					enc pextrw(EAX a,MMREG b,byte c){Encode(936,a,b,(IMM)c);}
					enc pextrw(ECX a,MMREG b,byte c){Encode(936,a,b,(IMM)c);}
					enc pextrw(REG32 a,XMMREG b,byte c){Encode(937,a,b,(IMM)c);}
					enc pextrw(EAX a,XMMREG b,byte c){Encode(937,a,b,(IMM)c);}
					enc pextrw(ECX a,XMMREG b,byte c){Encode(937,a,b,(IMM)c);}
					enc pf2id(MMREG a,MMREG b){Encode(938,a,b);}
					enc pf2id(MMREG a,MEM64 b){Encode(938,a,b);}
					enc pf2id(MMREG a,MM64 b){Encode(938,a,b);}
					enc pf2iw(MMREG a,MMREG b){Encode(939,a,b);}
					enc pf2iw(MMREG a,MEM64 b){Encode(939,a,b);}
					enc pf2iw(MMREG a,MM64 b){Encode(939,a,b);}
					enc pfacc(MMREG a,MMREG b){Encode(940,a,b);}
					enc pfacc(MMREG a,MEM64 b){Encode(940,a,b);}
					enc pfacc(MMREG a,MM64 b){Encode(940,a,b);}
					enc pfadd(MMREG a,MMREG b){Encode(941,a,b);}
					enc pfadd(MMREG a,MEM64 b){Encode(941,a,b);}
					enc pfadd(MMREG a,MM64 b){Encode(941,a,b);}
					enc pfcmpeq(MMREG a,MMREG b){Encode(942,a,b);}
					enc pfcmpeq(MMREG a,MEM64 b){Encode(942,a,b);}
					enc pfcmpeq(MMREG a,MM64 b){Encode(942,a,b);}
					enc pfcmpge(MMREG a,MMREG b){Encode(943,a,b);}
					enc pfcmpge(MMREG a,MEM64 b){Encode(943,a,b);}
					enc pfcmpge(MMREG a,MM64 b){Encode(943,a,b);}
					enc pfcmpgt(MMREG a,MMREG b){Encode(944,a,b);}
					enc pfcmpgt(MMREG a,MEM64 b){Encode(944,a,b);}
					enc pfcmpgt(MMREG a,MM64 b){Encode(944,a,b);}
					enc pfmax(MMREG a,MMREG b){Encode(945,a,b);}
					enc pfmax(MMREG a,MEM64 b){Encode(945,a,b);}
					enc pfmax(MMREG a,MM64 b){Encode(945,a,b);}
					enc pfmin(MMREG a,MMREG b){Encode(946,a,b);}
					enc pfmin(MMREG a,MEM64 b){Encode(946,a,b);}
					enc pfmin(MMREG a,MM64 b){Encode(946,a,b);}
					enc pfmul(MMREG a,MMREG b){Encode(947,a,b);}
					enc pfmul(MMREG a,MEM64 b){Encode(947,a,b);}
					enc pfmul(MMREG a,MM64 b){Encode(947,a,b);}
					enc pfnacc(MMREG a,MMREG b){Encode(948,a,b);}
					enc pfnacc(MMREG a,MEM64 b){Encode(948,a,b);}
					enc pfnacc(MMREG a,MM64 b){Encode(948,a,b);}
					enc pfpnacc(MMREG a,MMREG b){Encode(949,a,b);}
					enc pfpnacc(MMREG a,MEM64 b){Encode(949,a,b);}
					enc pfpnacc(MMREG a,MM64 b){Encode(949,a,b);}
					enc pfrcp(MMREG a,MMREG b){Encode(950,a,b);}
					enc pfrcp(MMREG a,MEM64 b){Encode(950,a,b);}
					enc pfrcp(MMREG a,MM64 b){Encode(950,a,b);}
					enc pfrcpit1(MMREG a,MMREG b){Encode(951,a,b);}
					enc pfrcpit1(MMREG a,MEM64 b){Encode(951,a,b);}
					enc pfrcpit1(MMREG a,MM64 b){Encode(951,a,b);}
					enc pfrcpit2(MMREG a,MMREG b){Encode(952,a,b);}
					enc pfrcpit2(MMREG a,MEM64 b){Encode(952,a,b);}
					enc pfrcpit2(MMREG a,MM64 b){Encode(952,a,b);}
					enc pfrsqit1(MMREG a,MMREG b){Encode(953,a,b);}
					enc pfrsqit1(MMREG a,MEM64 b){Encode(953,a,b);}
					enc pfrsqit1(MMREG a,MM64 b){Encode(953,a,b);}
					enc pfrsqrt(MMREG a,MMREG b){Encode(954,a,b);}
					enc pfrsqrt(MMREG a,MEM64 b){Encode(954,a,b);}
					enc pfrsqrt(MMREG a,MM64 b){Encode(954,a,b);}
					enc pfsub(MMREG a,MMREG b){Encode(955,a,b);}
					enc pfsub(MMREG a,MEM64 b){Encode(955,a,b);}
					enc pfsub(MMREG a,MM64 b){Encode(955,a,b);}
					enc pfsubr(MMREG a,MMREG b){Encode(956,a,b);}
					enc pfsubr(MMREG a,MEM64 b){Encode(956,a,b);}
					enc pfsubr(MMREG a,MM64 b){Encode(956,a,b);}
					enc pi2fd(MMREG a,MMREG b){Encode(957,a,b);}
					enc pi2fd(MMREG a,MEM64 b){Encode(957,a,b);}
					enc pi2fd(MMREG a,MM64 b){Encode(957,a,b);}
					enc pi2fw(MMREG a,MMREG b){Encode(958,a,b);}
					enc pi2fw(MMREG a,MEM64 b){Encode(958,a,b);}
					enc pi2fw(MMREG a,MM64 b){Encode(958,a,b);}
					enc pinsrw(MMREG a,REG16 b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(MMREG a,AX b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(MMREG a,DX b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(MMREG a,CX b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(MMREG a,MEM16 b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(MMREG a,R_M16 b,byte c){Encode(959,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,REG16 b,byte c){Encode(960,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,AX b,byte c){Encode(960,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,DX b,byte c){Encode(960,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,CX b,byte c){Encode(960,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,MEM16 b,byte c){Encode(960,a,b,(IMM)c);}
					enc pinsrw(XMMREG a,R_M16 b,byte c){Encode(960,a,b,(IMM)c);}
					enc pmachriw(MMREG a,MEM64 b){Encode(961,a,b);}
					enc pmaddwd(MMREG a,MMREG b){Encode(962,a,b);}
					enc pmaddwd(MMREG a,MEM64 b){Encode(962,a,b);}
					enc pmaddwd(MMREG a,MM64 b){Encode(962,a,b);}
					enc pmaddwd(XMMREG a,XMMREG b){Encode(963,a,b);}
					enc pmaddwd(XMMREG a,MEM128 b){Encode(963,a,b);}
					enc pmaddwd(XMMREG a,R_M128 b){Encode(963,a,b);}
					enc pmagw(MMREG a,MMREG b){Encode(964,a,b);}
					enc pmagw(MMREG a,MEM64 b){Encode(964,a,b);}
					enc pmagw(MMREG a,MM64 b){Encode(964,a,b);}
					enc pmaxsw(XMMREG a,XMMREG b){Encode(965,a,b);}
					enc pmaxsw(XMMREG a,MEM128 b){Encode(965,a,b);}
					enc pmaxsw(XMMREG a,R_M128 b){Encode(965,a,b);}
					enc pmaxsw(MMREG a,MMREG b){Encode(966,a,b);}
					enc pmaxsw(MMREG a,MEM64 b){Encode(966,a,b);}
					enc pmaxsw(MMREG a,MM64 b){Encode(966,a,b);}
					enc pmaxub(MMREG a,MMREG b){Encode(967,a,b);}
					enc pmaxub(MMREG a,MEM64 b){Encode(967,a,b);}
					enc pmaxub(MMREG a,MM64 b){Encode(967,a,b);}
					enc pmaxub(XMMREG a,XMMREG b){Encode(968,a,b);}
					enc pmaxub(XMMREG a,MEM128 b){Encode(968,a,b);}
					enc pmaxub(XMMREG a,R_M128 b){Encode(968,a,b);}
					enc pminsw(MMREG a,MMREG b){Encode(969,a,b);}
					enc pminsw(MMREG a,MEM64 b){Encode(969,a,b);}
					enc pminsw(MMREG a,MM64 b){Encode(969,a,b);}
					enc pminsw(XMMREG a,XMMREG b){Encode(970,a,b);}
					enc pminsw(XMMREG a,MEM128 b){Encode(970,a,b);}
					enc pminsw(XMMREG a,R_M128 b){Encode(970,a,b);}
					enc pminub(MMREG a,MMREG b){Encode(971,a,b);}
					enc pminub(MMREG a,MEM64 b){Encode(971,a,b);}
					enc pminub(MMREG a,MM64 b){Encode(971,a,b);}
					enc pminub(XMMREG a,XMMREG b){Encode(972,a,b);}
					enc pminub(XMMREG a,MEM128 b){Encode(972,a,b);}
					enc pminub(XMMREG a,R_M128 b){Encode(972,a,b);}
					enc pmovmskb(REG32 a,MMREG b){Encode(973,a,b);}
					enc pmovmskb(REG32 a,XMMREG b){Encode(974,a,b);}
					enc pmulhriw(MMREG a,MMREG b){Encode(975,a,b);}
					enc pmulhriw(MMREG a,MEM64 b){Encode(975,a,b);}
					enc pmulhriw(MMREG a,MM64 b){Encode(975,a,b);}
					enc pmulhrwa(MMREG a,MMREG b){Encode(976,a,b);}
					enc pmulhrwa(MMREG a,MEM64 b){Encode(976,a,b);}
					enc pmulhrwa(MMREG a,MM64 b){Encode(976,a,b);}
					enc pmulhrwc(MMREG a,MMREG b){Encode(977,a,b);}
					enc pmulhrwc(MMREG a,MEM64 b){Encode(977,a,b);}
					enc pmulhrwc(MMREG a,MM64 b){Encode(977,a,b);}
					enc pmulhuw(MMREG a,MMREG b){Encode(978,a,b);}
					enc pmulhuw(MMREG a,MEM64 b){Encode(978,a,b);}
					enc pmulhuw(MMREG a,MM64 b){Encode(978,a,b);}
					enc pmulhuw(XMMREG a,XMMREG b){Encode(979,a,b);}
					enc pmulhuw(XMMREG a,MEM128 b){Encode(979,a,b);}
					enc pmulhuw(XMMREG a,R_M128 b){Encode(979,a,b);}
					enc pmulhw(MMREG a,MMREG b){Encode(980,a,b);}
					enc pmulhw(MMREG a,MEM64 b){Encode(980,a,b);}
					enc pmulhw(MMREG a,MM64 b){Encode(980,a,b);}
					enc pmulhw(XMMREG a,XMMREG b){Encode(981,a,b);}
					enc pmulhw(XMMREG a,MEM128 b){Encode(981,a,b);}
					enc pmulhw(XMMREG a,R_M128 b){Encode(981,a,b);}
					enc pmullw(MMREG a,MMREG b){Encode(982,a,b);}
					enc pmullw(MMREG a,MEM64 b){Encode(982,a,b);}
					enc pmullw(MMREG a,MM64 b){Encode(982,a,b);}
					enc pmullw(XMMREG a,XMMREG b){Encode(983,a,b);}
					enc pmullw(XMMREG a,MEM128 b){Encode(983,a,b);}
					enc pmullw(XMMREG a,R_M128 b){Encode(983,a,b);}
					enc pmuludq(MMREG a,MMREG b){Encode(984,a,b);}
					enc pmuludq(MMREG a,MEM64 b){Encode(984,a,b);}
					enc pmuludq(MMREG a,MM64 b){Encode(984,a,b);}
					enc pmuludq(XMMREG a,XMMREG b){Encode(985,a,b);}
					enc pmuludq(XMMREG a,MEM128 b){Encode(985,a,b);}
					enc pmuludq(XMMREG a,R_M128 b){Encode(985,a,b);}
					enc pmvgezb(MMREG a,MEM64 b){Encode(986,a,b);}
					enc pmvlzb(MMREG a,MEM64 b){Encode(987,a,b);}
					enc pmvnzb(MMREG a,MEM64 b){Encode(988,a,b);}
					enc pmvzb(MMREG a,MEM64 b){Encode(989,a,b);}
					enc pop(REG16 a){Encode(990,a);}
					enc pop(REG32 a){Encode(991,a);}
					enc pop(REG64 a){Encode(992,a);}
					enc pop(MEM16 a){Encode(993,a);}
					enc pop(R_M16 a){Encode(993,a);}
					enc pop(MEM32 a){Encode(994,a);}
					enc pop(R_M32 a){Encode(994,a);}
					enc pop(MEM64 a){Encode(995,a);}
					enc pop(R_M64 a){Encode(995,a);}
					enc popa(){Encode(996);}
					enc popad(){Encode(997);}
					enc popaw(){Encode(998);}
					enc popf(){Encode(999);}
					enc popfd(){Encode(1000);}
					enc popfq(){Encode(1001);}
					enc popfw(){Encode(1002);}
					enc por(MMREG a,MMREG b){Encode(1003,a,b);}
					enc por(MMREG a,MEM64 b){Encode(1003,a,b);}
					enc por(MMREG a,MM64 b){Encode(1003,a,b);}
					enc por(XMMREG a,XMMREG b){Encode(1004,a,b);}
					enc por(XMMREG a,MEM128 b){Encode(1004,a,b);}
					enc por(XMMREG a,R_M128 b){Encode(1004,a,b);}
					enc prefetch(MEM8 a){Encode(1005,a);}
					enc prefetch(MEM16 a){Encode(1005,a);}
					enc prefetch(MEM32 a){Encode(1005,a);}
					enc prefetch(MEM64 a){Encode(1005,a);}
					enc prefetch(MEM128 a){Encode(1005,a);}
					enc prefetchnta(MEM8 a){Encode(1006,a);}
					enc prefetchnta(MEM16 a){Encode(1006,a);}
					enc prefetchnta(MEM32 a){Encode(1006,a);}
					enc prefetchnta(MEM64 a){Encode(1006,a);}
					enc prefetchnta(MEM128 a){Encode(1006,a);}
					enc prefetcht0(MEM8 a){Encode(1007,a);}
					enc prefetcht0(MEM16 a){Encode(1007,a);}
					enc prefetcht0(MEM32 a){Encode(1007,a);}
					enc prefetcht0(MEM64 a){Encode(1007,a);}
					enc prefetcht0(MEM128 a){Encode(1007,a);}
					enc prefetcht1(MEM8 a){Encode(1008,a);}
					enc prefetcht1(MEM16 a){Encode(1008,a);}
					enc prefetcht1(MEM32 a){Encode(1008,a);}
					enc prefetcht1(MEM64 a){Encode(1008,a);}
					enc prefetcht1(MEM128 a){Encode(1008,a);}
					enc prefetcht2(MEM8 a){Encode(1009,a);}
					enc prefetcht2(MEM16 a){Encode(1009,a);}
					enc prefetcht2(MEM32 a){Encode(1009,a);}
					enc prefetcht2(MEM64 a){Encode(1009,a);}
					enc prefetcht2(MEM128 a){Encode(1009,a);}
					enc prefetchw(MEM8 a){Encode(1010,a);}
					enc prefetchw(MEM16 a){Encode(1010,a);}
					enc prefetchw(MEM32 a){Encode(1010,a);}
					enc prefetchw(MEM64 a){Encode(1010,a);}
					enc prefetchw(MEM128 a){Encode(1010,a);}
					enc psadbw(MMREG a,MMREG b){Encode(1011,a,b);}
					enc psadbw(MMREG a,MEM64 b){Encode(1011,a,b);}
					enc psadbw(MMREG a,MM64 b){Encode(1011,a,b);}
					enc psadbw(XMMREG a,XMMREG b){Encode(1012,a,b);}
					enc psadbw(XMMREG a,MEM128 b){Encode(1012,a,b);}
					enc psadbw(XMMREG a,R_M128 b){Encode(1012,a,b);}
					enc pshufd(XMMREG a,XMMREG b,byte c){Encode(1013,a,b,(IMM)c);}
					enc pshufd(XMMREG a,MEM128 b,byte c){Encode(1013,a,b,(IMM)c);}
					enc pshufd(XMMREG a,R_M128 b,byte c){Encode(1013,a,b,(IMM)c);}
					enc pshufhw(XMMREG a,XMMREG b,byte c){Encode(1014,a,b,(IMM)c);}
					enc pshufhw(XMMREG a,MEM128 b,byte c){Encode(1014,a,b,(IMM)c);}
					enc pshufhw(XMMREG a,R_M128 b,byte c){Encode(1014,a,b,(IMM)c);}
					enc pshuflw(XMMREG a,XMMREG b,byte c){Encode(1015,a,b,(IMM)c);}
					enc pshuflw(XMMREG a,MEM128 b,byte c){Encode(1015,a,b,(IMM)c);}
					enc pshuflw(XMMREG a,R_M128 b,byte c){Encode(1015,a,b,(IMM)c);}
					enc pshufw(MMREG a,MMREG b,byte c){Encode(1016,a,b,(IMM)c);}
					enc pshufw(MMREG a,MEM64 b,byte c){Encode(1016,a,b,(IMM)c);}
					enc pshufw(MMREG a,MM64 b,byte c){Encode(1016,a,b,(IMM)c);}
					enc pslld(MMREG a,MMREG b){Encode(1017,a,b);}
					enc pslld(MMREG a,MEM64 b){Encode(1017,a,b);}
					enc pslld(MMREG a,MM64 b){Encode(1017,a,b);}
					enc pslld(MMREG a,byte b){Encode(1018,a,(IMM)b);}
					enc pslld(XMMREG a,XMMREG b){Encode(1019,a,b);}
					enc pslld(XMMREG a,MEM128 b){Encode(1019,a,b);}
					enc pslld(XMMREG a,R_M128 b){Encode(1019,a,b);}
					enc pslld(XMMREG a,byte b){Encode(1020,a,(IMM)b);}
					enc psllq(MMREG a,MMREG b){Encode(1021,a,b);}
					enc psllq(MMREG a,MEM64 b){Encode(1021,a,b);}
					enc psllq(MMREG a,MM64 b){Encode(1021,a,b);}
					enc psllq(MMREG a,byte b){Encode(1022,a,(IMM)b);}
					enc psllq(XMMREG a,XMMREG b){Encode(1023,a,b);}
					enc psllq(XMMREG a,MEM128 b){Encode(1023,a,b);}
					enc psllq(XMMREG a,R_M128 b){Encode(1023,a,b);}
					enc psllq(XMMREG a,byte b){Encode(1024,a,(IMM)b);}
					enc psllw(MMREG a,MMREG b){Encode(1025,a,b);}
					enc psllw(MMREG a,MEM64 b){Encode(1025,a,b);}
					enc psllw(MMREG a,MM64 b){Encode(1025,a,b);}
					enc psllw(MMREG a,byte b){Encode(1026,a,(IMM)b);}
					enc psllw(XMMREG a,XMMREG b){Encode(1027,a,b);}
					enc psllw(XMMREG a,MEM128 b){Encode(1027,a,b);}
					enc psllw(XMMREG a,R_M128 b){Encode(1027,a,b);}
					enc psllw(XMMREG a,byte b){Encode(1028,a,(IMM)b);}
					enc psrad(MMREG a,MMREG b){Encode(1029,a,b);}
					enc psrad(MMREG a,MEM64 b){Encode(1029,a,b);}
					enc psrad(MMREG a,MM64 b){Encode(1029,a,b);}
					enc psrad(MMREG a,byte b){Encode(1030,a,(IMM)b);}
					enc psrad(XMMREG a,XMMREG b){Encode(1031,a,b);}
					enc psrad(XMMREG a,MEM128 b){Encode(1031,a,b);}
					enc psrad(XMMREG a,R_M128 b){Encode(1031,a,b);}
					enc psrad(XMMREG a,byte b){Encode(1032,a,(IMM)b);}
					enc psraw(MMREG a,MMREG b){Encode(1033,a,b);}
					enc psraw(MMREG a,MEM64 b){Encode(1033,a,b);}
					enc psraw(MMREG a,MM64 b){Encode(1033,a,b);}
					enc psraw(MMREG a,byte b){Encode(1034,a,(IMM)b);}
					enc psraw(XMMREG a,XMMREG b){Encode(1035,a,b);}
					enc psraw(XMMREG a,MEM128 b){Encode(1035,a,b);}
					enc psraw(XMMREG a,R_M128 b){Encode(1035,a,b);}
					enc psraw(XMMREG a,byte b){Encode(1036,a,(IMM)b);}
					enc psrld(MMREG a,MMREG b){Encode(1037,a,b);}
					enc psrld(MMREG a,MEM64 b){Encode(1037,a,b);}
					enc psrld(MMREG a,MM64 b){Encode(1037,a,b);}
					enc psrld(MMREG a,byte b){Encode(1038,a,(IMM)b);}
					enc psrld(XMMREG a,XMMREG b){Encode(1039,a,b);}
					enc psrld(XMMREG a,MEM128 b){Encode(1039,a,b);}
					enc psrld(XMMREG a,R_M128 b){Encode(1039,a,b);}
					enc psrld(XMMREG a,byte b){Encode(1040,a,(IMM)b);}
					enc psrldq(XMMREG a,byte b){Encode(1041,a,(IMM)b);}
					enc psrlq(MMREG a,MMREG b){Encode(1042,a,b);}
					enc psrlq(MMREG a,MEM64 b){Encode(1042,a,b);}
					enc psrlq(MMREG a,MM64 b){Encode(1042,a,b);}
					enc psrlq(MMREG a,byte b){Encode(1043,a,(IMM)b);}
					enc psrlq(XMMREG a,XMMREG b){Encode(1044,a,b);}
					enc psrlq(XMMREG a,MEM128 b){Encode(1044,a,b);}
					enc psrlq(XMMREG a,R_M128 b){Encode(1044,a,b);}
					enc psrlq(XMMREG a,byte b){Encode(1045,a,(IMM)b);}
					enc psrlw(MMREG a,MMREG b){Encode(1046,a,b);}
					enc psrlw(MMREG a,MEM64 b){Encode(1046,a,b);}
					enc psrlw(MMREG a,MM64 b){Encode(1046,a,b);}
					enc psrlw(MMREG a,byte b){Encode(1047,a,(IMM)b);}
					enc psrlw(XMMREG a,XMMREG b){Encode(1048,a,b);}
					enc psrlw(XMMREG a,MEM128 b){Encode(1048,a,b);}
					enc psrlw(XMMREG a,R_M128 b){Encode(1048,a,b);}
					enc psrlw(XMMREG a,byte b){Encode(1049,a,(IMM)b);}
					enc psubb(MMREG a,MMREG b){Encode(1050,a,b);}
					enc psubb(MMREG a,MEM64 b){Encode(1050,a,b);}
					enc psubb(MMREG a,MM64 b){Encode(1050,a,b);}
					enc psubb(XMMREG a,XMMREG b){Encode(1051,a,b);}
					enc psubb(XMMREG a,MEM128 b){Encode(1051,a,b);}
					enc psubb(XMMREG a,R_M128 b){Encode(1051,a,b);}
					enc psubd(MMREG a,MMREG b){Encode(1052,a,b);}
					enc psubd(MMREG a,MEM64 b){Encode(1052,a,b);}
					enc psubd(MMREG a,MM64 b){Encode(1052,a,b);}
					enc psubd(XMMREG a,XMMREG b){Encode(1053,a,b);}
					enc psubd(XMMREG a,MEM128 b){Encode(1053,a,b);}
					enc psubd(XMMREG a,R_M128 b){Encode(1053,a,b);}
					enc psubq(MMREG a,MMREG b){Encode(1054,a,b);}
					enc psubq(MMREG a,MEM64 b){Encode(1054,a,b);}
					enc psubq(MMREG a,MM64 b){Encode(1054,a,b);}
					enc psubq(XMMREG a,XMMREG b){Encode(1055,a,b);}
					enc psubq(XMMREG a,MEM128 b){Encode(1055,a,b);}
					enc psubq(XMMREG a,R_M128 b){Encode(1055,a,b);}
					enc psubsb(MMREG a,MMREG b){Encode(1056,a,b);}
					enc psubsb(MMREG a,MEM64 b){Encode(1056,a,b);}
					enc psubsb(MMREG a,MM64 b){Encode(1056,a,b);}
					enc psubsb(XMMREG a,XMMREG b){Encode(1057,a,b);}
					enc psubsb(XMMREG a,MEM128 b){Encode(1057,a,b);}
					enc psubsb(XMMREG a,R_M128 b){Encode(1057,a,b);}
					enc psubsiw(MMREG a,MMREG b){Encode(1058,a,b);}
					enc psubsiw(MMREG a,MEM64 b){Encode(1058,a,b);}
					enc psubsiw(MMREG a,MM64 b){Encode(1058,a,b);}
					enc psubsw(MMREG a,MMREG b){Encode(1059,a,b);}
					enc psubsw(MMREG a,MEM64 b){Encode(1059,a,b);}
					enc psubsw(MMREG a,MM64 b){Encode(1059,a,b);}
					enc psubsw(XMMREG a,XMMREG b){Encode(1060,a,b);}
					enc psubsw(XMMREG a,MEM128 b){Encode(1060,a,b);}
					enc psubsw(XMMREG a,R_M128 b){Encode(1060,a,b);}
					enc psubusb(MMREG a,MMREG b){Encode(1061,a,b);}
					enc psubusb(MMREG a,MEM64 b){Encode(1061,a,b);}
					enc psubusb(MMREG a,MM64 b){Encode(1061,a,b);}
					enc psubusb(XMMREG a,XMMREG b){Encode(1062,a,b);}
					enc psubusb(XMMREG a,MEM128 b){Encode(1062,a,b);}
					enc psubusb(XMMREG a,R_M128 b){Encode(1062,a,b);}
					enc psubusw(MMREG a,MMREG b){Encode(1063,a,b);}
					enc psubusw(MMREG a,MEM64 b){Encode(1063,a,b);}
					enc psubusw(MMREG a,MM64 b){Encode(1063,a,b);}
					enc psubusw(XMMREG a,XMMREG b){Encode(1064,a,b);}
					enc psubusw(XMMREG a,MEM128 b){Encode(1064,a,b);}
					enc psubusw(XMMREG a,R_M128 b){Encode(1064,a,b);}
					enc psubw(MMREG a,MMREG b){Encode(1065,a,b);}
					enc psubw(MMREG a,MEM64 b){Encode(1065,a,b);}
					enc psubw(MMREG a,MM64 b){Encode(1065,a,b);}
					enc psubw(XMMREG a,XMMREG b){Encode(1066,a,b);}
					enc psubw(XMMREG a,MEM128 b){Encode(1066,a,b);}
					enc psubw(XMMREG a,R_M128 b){Encode(1066,a,b);}
					enc pswapd(MMREG a,MMREG b){Encode(1067,a,b);}
					enc pswapd(MMREG a,MEM64 b){Encode(1067,a,b);}
					enc pswapd(MMREG a,MM64 b){Encode(1067,a,b);}
					enc punpckhbw(MMREG a,MMREG b){Encode(1068,a,b);}
					enc punpckhbw(MMREG a,MEM64 b){Encode(1068,a,b);}
					enc punpckhbw(MMREG a,MM64 b){Encode(1068,a,b);}
					enc punpckhbw(XMMREG a,XMMREG b){Encode(1069,a,b);}
					enc punpckhbw(XMMREG a,MEM128 b){Encode(1069,a,b);}
					enc punpckhbw(XMMREG a,R_M128 b){Encode(1069,a,b);}
					enc punpckhdq(MMREG a,MMREG b){Encode(1070,a,b);}
					enc punpckhdq(MMREG a,MEM64 b){Encode(1070,a,b);}
					enc punpckhdq(MMREG a,MM64 b){Encode(1070,a,b);}
					enc punpckhdq(XMMREG a,XMMREG b){Encode(1071,a,b);}
					enc punpckhdq(XMMREG a,MEM128 b){Encode(1071,a,b);}
					enc punpckhdq(XMMREG a,R_M128 b){Encode(1071,a,b);}
					enc punpckhqdq(XMMREG a,XMMREG b){Encode(1072,a,b);}
					enc punpckhqdq(XMMREG a,MEM128 b){Encode(1072,a,b);}
					enc punpckhqdq(XMMREG a,R_M128 b){Encode(1072,a,b);}
					enc punpckhwd(MMREG a,MMREG b){Encode(1073,a,b);}
					enc punpckhwd(MMREG a,MEM64 b){Encode(1073,a,b);}
					enc punpckhwd(MMREG a,MM64 b){Encode(1073,a,b);}
					enc punpckhwd(XMMREG a,XMMREG b){Encode(1074,a,b);}
					enc punpckhwd(XMMREG a,MEM128 b){Encode(1074,a,b);}
					enc punpckhwd(XMMREG a,R_M128 b){Encode(1074,a,b);}
					enc punpcklbw(MMREG a,MMREG b){Encode(1075,a,b);}
					enc punpcklbw(MMREG a,MEM64 b){Encode(1075,a,b);}
					enc punpcklbw(MMREG a,MM64 b){Encode(1075,a,b);}
					enc punpcklbw(XMMREG a,XMMREG b){Encode(1076,a,b);}
					enc punpcklbw(XMMREG a,MEM128 b){Encode(1076,a,b);}
					enc punpcklbw(XMMREG a,R_M128 b){Encode(1076,a,b);}
					enc punpckldq(MMREG a,MMREG b){Encode(1077,a,b);}
					enc punpckldq(MMREG a,MEM64 b){Encode(1077,a,b);}
					enc punpckldq(MMREG a,MM64 b){Encode(1077,a,b);}
					enc punpckldq(XMMREG a,XMMREG b){Encode(1078,a,b);}
					enc punpckldq(XMMREG a,MEM128 b){Encode(1078,a,b);}
					enc punpckldq(XMMREG a,R_M128 b){Encode(1078,a,b);}
					enc punpcklqdq(XMMREG a,XMMREG b){Encode(1079,a,b);}
					enc punpcklqdq(XMMREG a,MEM128 b){Encode(1079,a,b);}
					enc punpcklqdq(XMMREG a,R_M128 b){Encode(1079,a,b);}
					enc punpcklwd(MMREG a,MMREG b){Encode(1080,a,b);}
					enc punpcklwd(MMREG a,MEM64 b){Encode(1080,a,b);}
					enc punpcklwd(MMREG a,MM64 b){Encode(1080,a,b);}
					enc punpcklwd(XMMREG a,XMMREG b){Encode(1081,a,b);}
					enc punpcklwd(XMMREG a,MEM128 b){Encode(1081,a,b);}
					enc punpcklwd(XMMREG a,R_M128 b){Encode(1081,a,b);}
					enc push(REG16 a){Encode(1082,a);}
					enc push(REG32 a){Encode(1083,a);}
					enc push(REG64 a){Encode(1084,a);}
					enc push(MEM16 a){Encode(1085,a);}
					enc push(R_M16 a){Encode(1085,a);}
					enc push(MEM32 a){Encode(1086,a);}
					enc push(R_M32 a){Encode(1086,a);}
					enc push(MEM64 a){Encode(1087,a);}
					enc push(R_M64 a){Encode(1087,a);}
					enc push(byte a){Encode(1088,(IMM)a);}
					enc push(word a){Encode(1089,(IMM)a);}
					enc push(REF a){Encode(1090,a);}
					enc push(dword a){Encode(1090,(IMM)a);}
					enc pusha(){Encode(1092);}
					enc pushad(){Encode(1093);}
					enc pushaw(){Encode(1094);}
					enc pushf(){Encode(1095);}
					enc pushfd(){Encode(1096);}
					enc pushfw(){Encode(1097);}
					enc pxor(MMREG a,MMREG b){Encode(1098,a,b);}
					enc pxor(MMREG a,MEM64 b){Encode(1098,a,b);}
					enc pxor(MMREG a,MM64 b){Encode(1098,a,b);}
					enc pxor(XMMREG a,XMMREG b){Encode(1099,a,b);}
					enc pxor(XMMREG a,MEM128 b){Encode(1099,a,b);}
					enc pxor(XMMREG a,R_M128 b){Encode(1099,a,b);}
					enc rcl(REG8 a,CL b){Encode(1101,a,b);}
					enc rcl(MEM8 a,CL b){Encode(1101,a,b);}
					enc rcl(R_M8 a,CL b){Encode(1101,a,b);}
					enc rcl(REG8 a,byte b){Encode(1102,a,(IMM)b);}
					enc rcl(AL a,byte b){Encode(1102,a,(IMM)b);}
					enc rcl(CL a,byte b){Encode(1102,a,(IMM)b);}
					enc rcl(MEM8 a,byte b){Encode(1102,a,(IMM)b);}
					enc rcl(R_M8 a,byte b){Encode(1102,a,(IMM)b);}
					enc rcl(REG16 a,CL b){Encode(1104,a,b);}
					enc rcl(MEM16 a,CL b){Encode(1104,a,b);}
					enc rcl(R_M16 a,CL b){Encode(1104,a,b);}
					enc rcl(REG16 a,byte b){Encode(1105,a,(IMM)b);}
					enc rcl(MEM16 a,byte b){Encode(1105,a,(IMM)b);}
					enc rcl(R_M16 a,byte b){Encode(1105,a,(IMM)b);}
					enc rcl(REG32 a,CL b){Encode(1107,a,b);}
					enc rcl(MEM32 a,CL b){Encode(1107,a,b);}
					enc rcl(R_M32 a,CL b){Encode(1107,a,b);}
					enc rcl(REG32 a,byte b){Encode(1108,a,(IMM)b);}
					enc rcl(MEM32 a,byte b){Encode(1108,a,(IMM)b);}
					enc rcl(R_M32 a,byte b){Encode(1108,a,(IMM)b);}
					enc rcl(REG64 a,CL b){Encode(1110,a,b);}
					enc rcl(MEM64 a,CL b){Encode(1110,a,b);}
					enc rcl(R_M64 a,CL b){Encode(1110,a,b);}
					enc rcl(REG64 a,byte b){Encode(1111,a,(IMM)b);}
					enc rcl(RAX a,byte b){Encode(1111,a,(IMM)b);}
					enc rcl(MEM64 a,byte b){Encode(1111,a,(IMM)b);}
					enc rcl(R_M64 a,byte b){Encode(1111,a,(IMM)b);}
					enc rcpps(XMMREG a,XMMREG b){Encode(1112,a,b);}
					enc rcpps(XMMREG a,MEM128 b){Encode(1112,a,b);}
					enc rcpps(XMMREG a,R_M128 b){Encode(1112,a,b);}
					enc rcpss(XMMREG a,XMMREG b){Encode(1113,a,b);}
					enc rcpss(XMMREG a,MEM32 b){Encode(1113,a,b);}
					enc rcpss(XMMREG a,XMM32 b){Encode(1113,a,b);}
					enc rcr(REG8 a,CL b){Encode(1115,a,b);}
					enc rcr(MEM8 a,CL b){Encode(1115,a,b);}
					enc rcr(R_M8 a,CL b){Encode(1115,a,b);}
					enc rcr(REG8 a,byte b){Encode(1116,a,(IMM)b);}
					enc rcr(AL a,byte b){Encode(1116,a,(IMM)b);}
					enc rcr(CL a,byte b){Encode(1116,a,(IMM)b);}
					enc rcr(MEM8 a,byte b){Encode(1116,a,(IMM)b);}
					enc rcr(R_M8 a,byte b){Encode(1116,a,(IMM)b);}
					enc rcr(REG16 a,CL b){Encode(1118,a,b);}
					enc rcr(MEM16 a,CL b){Encode(1118,a,b);}
					enc rcr(R_M16 a,CL b){Encode(1118,a,b);}
					enc rcr(REG16 a,byte b){Encode(1119,a,(IMM)b);}
					enc rcr(MEM16 a,byte b){Encode(1119,a,(IMM)b);}
					enc rcr(R_M16 a,byte b){Encode(1119,a,(IMM)b);}
					enc rcr(REG32 a,CL b){Encode(1121,a,b);}
					enc rcr(MEM32 a,CL b){Encode(1121,a,b);}
					enc rcr(R_M32 a,CL b){Encode(1121,a,b);}
					enc rcr(REG32 a,byte b){Encode(1122,a,(IMM)b);}
					enc rcr(MEM32 a,byte b){Encode(1122,a,(IMM)b);}
					enc rcr(R_M32 a,byte b){Encode(1122,a,(IMM)b);}
					enc rdmsr(){Encode(1126);}
					enc rdpmc(){Encode(1127);}
					enc rdtsc(){Encode(1128);}
					enc rep_insb(){Encode(1129);}
					enc rep_insd(){Encode(1130);}
					enc rep_insw(){Encode(1131);}
					enc rep_lodsb(){Encode(1132);}
					enc rep_lodsd(){Encode(1133);}
					enc rep_lodsw(){Encode(1134);}
					enc rep_movsb(){Encode(1135);}
					enc rep_movsd(){Encode(1136);}
					enc rep_movsw(){Encode(1137);}
					enc rep_outsb(){Encode(1138);}
					enc rep_outsd(){Encode(1139);}
					enc rep_outsw(){Encode(1140);}
					enc rep_scasb(){Encode(1141);}
					enc rep_scasd(){Encode(1142);}
					enc rep_scasw(){Encode(1143);}
					enc rep_stosb(){Encode(1144);}
					enc rep_stosd(){Encode(1145);}
					enc rep_stosw(){Encode(1146);}
					enc repe_cmpsb(){Encode(1147);}
					enc repe_cmpsd(){Encode(1148);}
					enc repe_cmpsw(){Encode(1149);}
					enc repe_scasb(){Encode(1150);}
					enc repe_scasd(){Encode(1151);}
					enc repe_scasw(){Encode(1152);}
					enc repne_cmpsb(){Encode(1153);}
					enc repne_cmpsd(){Encode(1154);}
					enc repne_cmpsw(){Encode(1155);}
					enc repne_scasb(){Encode(1156);}
					enc repne_scasd(){Encode(1157);}
					enc repne_scasw(){Encode(1158);}
					enc repnz_cmpsb(){Encode(1159);}
					enc repnz_cmpsd(){Encode(1160);}
					enc repnz_cmpsw(){Encode(1161);}
					enc repnz_scasb(){Encode(1162);}
					enc repnz_scasd(){Encode(1163);}
					enc repnz_scasw(){Encode(1164);}
					enc repz_cmpsb(){Encode(1165);}
					enc repz_cmpsd(){Encode(1166);}
					enc repz_cmpsw(){Encode(1167);}
					enc repz_scasb(){Encode(1168);}
					enc repz_scasd(){Encode(1169);}
					enc repz_scasw(){Encode(1170);}
					enc ret(){Encode(1171);}
					enc ret(byte a){Encode(1172,(IMM)a);}
					enc ret(word a){Encode(1172,(IMM)a);}
					enc rol(REG8 a,CL b){Encode(1174,a,b);}
					enc rol(MEM8 a,CL b){Encode(1174,a,b);}
					enc rol(R_M8 a,CL b){Encode(1174,a,b);}
					enc rol(REG8 a,byte b){Encode(1175,a,(IMM)b);}
					enc rol(AL a,byte b){Encode(1175,a,(IMM)b);}
					enc rol(CL a,byte b){Encode(1175,a,(IMM)b);}
					enc rol(MEM8 a,byte b){Encode(1175,a,(IMM)b);}
					enc rol(R_M8 a,byte b){Encode(1175,a,(IMM)b);}
					enc rol(REG16 a,CL b){Encode(1177,a,b);}
					enc rol(MEM16 a,CL b){Encode(1177,a,b);}
					enc rol(R_M16 a,CL b){Encode(1177,a,b);}
					enc rol(REG16 a,byte b){Encode(1178,a,(IMM)b);}
					enc rol(MEM16 a,byte b){Encode(1178,a,(IMM)b);}
					enc rol(R_M16 a,byte b){Encode(1178,a,(IMM)b);}
					enc rol(REG32 a,CL b){Encode(1180,a,b);}
					enc rol(MEM32 a,CL b){Encode(1180,a,b);}
					enc rol(R_M32 a,CL b){Encode(1180,a,b);}
					enc rol(REG32 a,byte b){Encode(1181,a,(IMM)b);}
					enc rol(MEM32 a,byte b){Encode(1181,a,(IMM)b);}
					enc rol(R_M32 a,byte b){Encode(1181,a,(IMM)b);}
					enc ror(REG8 a,CL b){Encode(1186,a,b);}
					enc ror(MEM8 a,CL b){Encode(1186,a,b);}
					enc ror(R_M8 a,CL b){Encode(1186,a,b);}
					enc ror(REG8 a,byte b){Encode(1187,a,(IMM)b);}
					enc ror(AL a,byte b){Encode(1187,a,(IMM)b);}
					enc ror(CL a,byte b){Encode(1187,a,(IMM)b);}
					enc ror(MEM8 a,byte b){Encode(1187,a,(IMM)b);}
					enc ror(R_M8 a,byte b){Encode(1187,a,(IMM)b);}
					enc ror(REG16 a,CL b){Encode(1189,a,b);}
					enc ror(MEM16 a,CL b){Encode(1189,a,b);}
					enc ror(R_M16 a,CL b){Encode(1189,a,b);}
					enc ror(REG16 a,byte b){Encode(1190,a,(IMM)b);}
					enc ror(MEM16 a,byte b){Encode(1190,a,(IMM)b);}
					enc ror(R_M16 a,byte b){Encode(1190,a,(IMM)b);}
					enc ror(REG32 a,CL b){Encode(1192,a,b);}
					enc ror(MEM32 a,CL b){Encode(1192,a,b);}
					enc ror(R_M32 a,CL b){Encode(1192,a,b);}
					enc ror(REG32 a,byte b){Encode(1193,a,(IMM)b);}
					enc ror(MEM32 a,byte b){Encode(1193,a,(IMM)b);}
					enc ror(R_M32 a,byte b){Encode(1193,a,(IMM)b);}
					enc ror(REG64 a,CL b){Encode(1195,a,b);}
					enc ror(MEM64 a,CL b){Encode(1195,a,b);}
					enc ror(R_M64 a,CL b){Encode(1195,a,b);}
					enc ror(REG64 a,byte b){Encode(1196,a,(IMM)b);}
					enc ror(RAX a,byte b){Encode(1196,a,(IMM)b);}
					enc ror(MEM64 a,byte b){Encode(1196,a,(IMM)b);}
					enc ror(R_M64 a,byte b){Encode(1196,a,(IMM)b);}
					enc rsm(){Encode(1197);}
					enc rsqrtps(XMMREG a,XMMREG b){Encode(1198,a,b);}
					enc rsqrtps(XMMREG a,MEM128 b){Encode(1198,a,b);}
					enc rsqrtps(XMMREG a,R_M128 b){Encode(1198,a,b);}
					enc rsqrtss(XMMREG a,XMMREG b){Encode(1199,a,b);}
					enc rsqrtss(XMMREG a,MEM32 b){Encode(1199,a,b);}
					enc rsqrtss(XMMREG a,XMM32 b){Encode(1199,a,b);}
					enc sahf(){Encode(1200);}
					enc sal(REG8 a,CL b){Encode(1202,a,b);}
					enc sal(MEM8 a,CL b){Encode(1202,a,b);}
					enc sal(R_M8 a,CL b){Encode(1202,a,b);}
					enc sal(REG8 a,byte b){Encode(1203,a,(IMM)b);}
					enc sal(AL a,byte b){Encode(1203,a,(IMM)b);}
					enc sal(CL a,byte b){Encode(1203,a,(IMM)b);}
					enc sal(MEM8 a,byte b){Encode(1203,a,(IMM)b);}
					enc sal(R_M8 a,byte b){Encode(1203,a,(IMM)b);}
					enc sal(REG16 a,CL b){Encode(1205,a,b);}
					enc sal(MEM16 a,CL b){Encode(1205,a,b);}
					enc sal(R_M16 a,CL b){Encode(1205,a,b);}
					enc sal(REG16 a,byte b){Encode(1206,a,(IMM)b);}
					enc sal(MEM16 a,byte b){Encode(1206,a,(IMM)b);}
					enc sal(R_M16 a,byte b){Encode(1206,a,(IMM)b);}
					enc sal(REG32 a,CL b){Encode(1208,a,b);}
					enc sal(MEM32 a,CL b){Encode(1208,a,b);}
					enc sal(R_M32 a,CL b){Encode(1208,a,b);}
					enc sal(REG32 a,byte b){Encode(1209,a,(IMM)b);}
					enc sal(MEM32 a,byte b){Encode(1209,a,(IMM)b);}
					enc sal(R_M32 a,byte b){Encode(1209,a,(IMM)b);}
					enc sal(REG64 a,CL b){Encode(1211,a,b);}
					enc sal(MEM64 a,CL b){Encode(1211,a,b);}
					enc sal(R_M64 a,CL b){Encode(1211,a,b);}
					enc sal(REG64 a,byte b){Encode(1212,a,(IMM)b);}
					enc sal(RAX a,byte b){Encode(1212,a,(IMM)b);}
					enc sal(MEM64 a,byte b){Encode(1212,a,(IMM)b);}
					enc sal(R_M64 a,byte b){Encode(1212,a,(IMM)b);}
					enc sar(REG8 a,CL b){Encode(1214,a,b);}
					enc sar(MEM8 a,CL b){Encode(1214,a,b);}
					enc sar(R_M8 a,CL b){Encode(1214,a,b);}
					enc sar(REG8 a,byte b){Encode(1215,a,(IMM)b);}
					enc sar(AL a,byte b){Encode(1215,a,(IMM)b);}
					enc sar(CL a,byte b){Encode(1215,a,(IMM)b);}
					enc sar(MEM8 a,byte b){Encode(1215,a,(IMM)b);}
					enc sar(R_M8 a,byte b){Encode(1215,a,(IMM)b);}
					enc sar(REG16 a,CL b){Encode(1217,a,b);}
					enc sar(MEM16 a,CL b){Encode(1217,a,b);}
					enc sar(R_M16 a,CL b){Encode(1217,a,b);}
					enc sar(REG16 a,byte b){Encode(1218,a,(IMM)b);}
					enc sar(MEM16 a,byte b){Encode(1218,a,(IMM)b);}
					enc sar(R_M16 a,byte b){Encode(1218,a,(IMM)b);}
					enc sar(REG32 a,CL b){Encode(1220,a,b);}
					enc sar(MEM32 a,CL b){Encode(1220,a,b);}
					enc sar(R_M32 a,CL b){Encode(1220,a,b);}
					enc sar(REG32 a,byte b){Encode(1221,a,(IMM)b);}
					enc sar(MEM32 a,byte b){Encode(1221,a,(IMM)b);}
					enc sar(R_M32 a,byte b){Encode(1221,a,(IMM)b);}
					enc sar(REG64 a,CL b){Encode(1223,a,b);}
					enc sar(MEM64 a,CL b){Encode(1223,a,b);}
					enc sar(R_M64 a,CL b){Encode(1223,a,b);}
					enc sar(REG64 a,byte b){Encode(1224,a,(IMM)b);}
					enc sar(RAX a,byte b){Encode(1224,a,(IMM)b);}
					enc sar(MEM64 a,byte b){Encode(1224,a,(IMM)b);}
					enc sar(R_M64 a,byte b){Encode(1224,a,(IMM)b);}
					enc sbb(REG8 a,REG8 b){Encode(1225,a,b);}
					enc sbb(MEM8 a,REG8 b){Encode(1225,a,b);}
					enc sbb(R_M8 a,REG8 b){Encode(1225,a,b);}
					enc sbb(REG16 a,REG16 b){Encode(1226,a,b);}
					enc sbb(MEM16 a,REG16 b){Encode(1226,a,b);}
					enc sbb(R_M16 a,REG16 b){Encode(1226,a,b);}
					enc sbb(REG32 a,REG32 b){Encode(1227,a,b);}
					enc sbb(MEM32 a,REG32 b){Encode(1227,a,b);}
					enc sbb(R_M32 a,REG32 b){Encode(1227,a,b);}
					enc sbb(REG64 a,REG64 b){Encode(1228,a,b);}
					enc sbb(MEM64 a,REG64 b){Encode(1228,a,b);}
					enc sbb(R_M64 a,REG64 b){Encode(1228,a,b);}
					enc sbb(REG8 a,MEM8 b){Encode(1229,a,b);}
					enc sbb(REG8 a,R_M8 b){Encode(1229,a,b);}
					enc sbb(REG16 a,MEM16 b){Encode(1230,a,b);}
					enc sbb(REG16 a,R_M16 b){Encode(1230,a,b);}
					enc sbb(REG32 a,MEM32 b){Encode(1231,a,b);}
					enc sbb(REG32 a,R_M32 b){Encode(1231,a,b);}
					enc sbb(REG64 a,MEM64 b){Encode(1232,a,b);}
					enc sbb(REG64 a,R_M64 b){Encode(1232,a,b);}
					enc sbb(REG8 a,byte b){Encode(1233,a,(IMM)b);}
					enc sbb(AL a,byte b){Encode(1233,a,(IMM)b);}
					enc sbb(CL a,byte b){Encode(1233,a,(IMM)b);}
					enc sbb(MEM8 a,byte b){Encode(1233,a,(IMM)b);}
					enc sbb(R_M8 a,byte b){Encode(1233,a,(IMM)b);}
					enc sbb(REG16 a,byte b){Encode(1234,a,(IMM)b);}
					enc sbb(REG16 a,word b){Encode(1234,a,(IMM)b);}
					enc sbb(MEM16 a,byte b){Encode(1234,a,(IMM)b);}
					enc sbb(MEM16 a,word b){Encode(1234,a,(IMM)b);}
					enc sbb(R_M16 a,byte b){Encode(1234,a,(IMM)b);}
					enc sbb(R_M16 a,word b){Encode(1234,a,(IMM)b);}
					enc sbb(REG32 a,REF b){Encode(1235,a,b);}
					enc sbb(REG32 a,dword b){Encode(1235,a,(IMM)b);}
					enc sbb(MEM32 a,REF b){Encode(1235,a,b);}
					enc sbb(MEM32 a,dword b){Encode(1235,a,(IMM)b);}
					enc sbb(R_M32 a,REF b){Encode(1235,a,b);}
					enc sbb(R_M32 a,dword b){Encode(1235,a,(IMM)b);}
					enc sbb(REG64 a,REF b){Encode(1236,a,b);}
					enc sbb(REG64 a,dword b){Encode(1236,a,(IMM)b);}
					enc sbb(MEM64 a,REF b){Encode(1236,a,b);}
					enc sbb(MEM64 a,dword b){Encode(1236,a,(IMM)b);}
					enc sbb(R_M64 a,REF b){Encode(1236,a,b);}
					enc sbb(R_M64 a,dword b){Encode(1236,a,(IMM)b);}
					enc scasb(){Encode(1244);}
					enc scasd(){Encode(1245);}
					enc scasq(){Encode(1246);}
					enc scasw(){Encode(1247);}
					enc seta(REG8 a){Encode(1248,a);}
					enc seta(MEM8 a){Encode(1248,a);}
					enc seta(R_M8 a){Encode(1248,a);}
					enc setae(REG8 a){Encode(1249,a);}
					enc setae(MEM8 a){Encode(1249,a);}
					enc setae(R_M8 a){Encode(1249,a);}
					enc setb(REG8 a){Encode(1250,a);}
					enc setb(MEM8 a){Encode(1250,a);}
					enc setb(R_M8 a){Encode(1250,a);}
					enc setbe(REG8 a){Encode(1251,a);}
					enc setbe(MEM8 a){Encode(1251,a);}
					enc setbe(R_M8 a){Encode(1251,a);}
					enc setc(REG8 a){Encode(1252,a);}
					enc setc(MEM8 a){Encode(1252,a);}
					enc setc(R_M8 a){Encode(1252,a);}
					enc sete(REG8 a){Encode(1253,a);}
					enc sete(MEM8 a){Encode(1253,a);}
					enc sete(R_M8 a){Encode(1253,a);}
					enc setg(REG8 a){Encode(1254,a);}
					enc setg(MEM8 a){Encode(1254,a);}
					enc setg(R_M8 a){Encode(1254,a);}
					enc setge(REG8 a){Encode(1255,a);}
					enc setge(MEM8 a){Encode(1255,a);}
					enc setge(R_M8 a){Encode(1255,a);}
					enc setl(REG8 a){Encode(1256,a);}
					enc setl(MEM8 a){Encode(1256,a);}
					enc setl(R_M8 a){Encode(1256,a);}
					enc setle(REG8 a){Encode(1257,a);}
					enc setle(MEM8 a){Encode(1257,a);}
					enc setle(R_M8 a){Encode(1257,a);}
					enc setna(REG8 a){Encode(1258,a);}
					enc setna(MEM8 a){Encode(1258,a);}
					enc setna(R_M8 a){Encode(1258,a);}
					enc setnb(REG8 a){Encode(1259,a);}
					enc setnb(MEM8 a){Encode(1259,a);}
					enc setnb(R_M8 a){Encode(1259,a);}
					enc setnbe(REG8 a){Encode(1260,a);}
					enc setnbe(MEM8 a){Encode(1260,a);}
					enc setnbe(R_M8 a){Encode(1260,a);}
					enc setnc(REG8 a){Encode(1261,a);}
					enc setnc(MEM8 a){Encode(1261,a);}
					enc setnc(R_M8 a){Encode(1261,a);}
					enc setne(REG8 a){Encode(1262,a);}
					enc setne(MEM8 a){Encode(1262,a);}
					enc setne(R_M8 a){Encode(1262,a);}
					enc setnea(REG8 a){Encode(1263,a);}
					enc setnea(MEM8 a){Encode(1263,a);}
					enc setnea(R_M8 a){Encode(1263,a);}
					enc setng(REG8 a){Encode(1264,a);}
					enc setng(MEM8 a){Encode(1264,a);}
					enc setng(R_M8 a){Encode(1264,a);}
					enc setnge(REG8 a){Encode(1265,a);}
					enc setnge(MEM8 a){Encode(1265,a);}
					enc setnge(R_M8 a){Encode(1265,a);}
					enc setnl(REG8 a){Encode(1266,a);}
					enc setnl(MEM8 a){Encode(1266,a);}
					enc setnl(R_M8 a){Encode(1266,a);}
					enc setnle(REG8 a){Encode(1267,a);}
					enc setnle(MEM8 a){Encode(1267,a);}
					enc setnle(R_M8 a){Encode(1267,a);}
					enc setno(REG8 a){Encode(1268,a);}
					enc setno(MEM8 a){Encode(1268,a);}
					enc setno(R_M8 a){Encode(1268,a);}
					enc setnp(REG8 a){Encode(1269,a);}
					enc setnp(MEM8 a){Encode(1269,a);}
					enc setnp(R_M8 a){Encode(1269,a);}
					enc setns(REG8 a){Encode(1270,a);}
					enc setns(MEM8 a){Encode(1270,a);}
					enc setns(R_M8 a){Encode(1270,a);}
					enc setnz(REG8 a){Encode(1271,a);}
					enc setnz(MEM8 a){Encode(1271,a);}
					enc setnz(R_M8 a){Encode(1271,a);}
					enc seto(REG8 a){Encode(1272,a);}
					enc seto(MEM8 a){Encode(1272,a);}
					enc seto(R_M8 a){Encode(1272,a);}
					enc setp(REG8 a){Encode(1273,a);}
					enc setp(MEM8 a){Encode(1273,a);}
					enc setp(R_M8 a){Encode(1273,a);}
					enc setpe(REG8 a){Encode(1274,a);}
					enc setpe(MEM8 a){Encode(1274,a);}
					enc setpe(R_M8 a){Encode(1274,a);}
					enc setpo(REG8 a){Encode(1275,a);}
					enc setpo(MEM8 a){Encode(1275,a);}
					enc setpo(R_M8 a){Encode(1275,a);}
					enc sets(REG8 a){Encode(1276,a);}
					enc sets(MEM8 a){Encode(1276,a);}
					enc sets(R_M8 a){Encode(1276,a);}
					enc setz(REG8 a){Encode(1277,a);}
					enc setz(MEM8 a){Encode(1277,a);}
					enc setz(R_M8 a){Encode(1277,a);}
					enc sfence(){Encode(1278);}
					enc shl(REG8 a,CL b){Encode(1280,a,b);}
					enc shl(MEM8 a,CL b){Encode(1280,a,b);}
					enc shl(R_M8 a,CL b){Encode(1280,a,b);}
					enc shl(REG8 a,byte b){Encode(1281,a,(IMM)b);}
					enc shl(AL a,byte b){Encode(1281,a,(IMM)b);}
					enc shl(CL a,byte b){Encode(1281,a,(IMM)b);}
					enc shl(MEM8 a,byte b){Encode(1281,a,(IMM)b);}
					enc shl(R_M8 a,byte b){Encode(1281,a,(IMM)b);}
					enc shl(REG16 a,CL b){Encode(1283,a,b);}
					enc shl(MEM16 a,CL b){Encode(1283,a,b);}
					enc shl(R_M16 a,CL b){Encode(1283,a,b);}
					enc shl(REG16 a,byte b){Encode(1284,a,(IMM)b);}
					enc shl(MEM16 a,byte b){Encode(1284,a,(IMM)b);}
					enc shl(R_M16 a,byte b){Encode(1284,a,(IMM)b);}
					enc shl(REG32 a,CL b){Encode(1286,a,b);}
					enc shl(MEM32 a,CL b){Encode(1286,a,b);}
					enc shl(R_M32 a,CL b){Encode(1286,a,b);}
					enc shl(REG32 a,byte b){Encode(1287,a,(IMM)b);}
					enc shl(MEM32 a,byte b){Encode(1287,a,(IMM)b);}
					enc shl(R_M32 a,byte b){Encode(1287,a,(IMM)b);}
					enc shl(REG64 a,CL b){Encode(1289,a,b);}
					enc shl(MEM64 a,CL b){Encode(1289,a,b);}
					enc shl(R_M64 a,CL b){Encode(1289,a,b);}
					enc shl(REG64 a,byte b){Encode(1290,a,(IMM)b);}
					enc shl(RAX a,byte b){Encode(1290,a,(IMM)b);}
					enc shl(MEM64 a,byte b){Encode(1290,a,(IMM)b);}
					enc shl(R_M64 a,byte b){Encode(1290,a,(IMM)b);}
					enc shld(REG16 a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(REG16 a,AX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(REG16 a,DX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(REG16 a,CX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(AX a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(DX a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(CX a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(MEM16 a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(MEM16 a,AX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(MEM16 a,DX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(MEM16 a,CX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(R_M16 a,REG16 b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(R_M16 a,AX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(R_M16 a,DX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(R_M16 a,CX b,byte c){Encode(1291,a,b,(IMM)c);}
					enc shld(REG32 a,REG32 b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(REG32 a,EAX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(REG32 a,ECX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(EAX a,REG32 b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(ECX a,REG32 b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(MEM32 a,REG32 b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(MEM32 a,EAX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(MEM32 a,ECX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(R_M32 a,REG32 b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(R_M32 a,EAX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(R_M32 a,ECX b,byte c){Encode(1292,a,b,(IMM)c);}
					enc shld(REG64 a,REG64 b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(REG64 a,RAX b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(RAX a,REG64 b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(MEM64 a,REG64 b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(MEM64 a,RAX b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(R_M64 a,REG64 b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(R_M64 a,RAX b,byte c){Encode(1293,a,b,(IMM)c);}
					enc shld(REG16 a,REG16 b,CL c){Encode(1294,a,b,c);}
					enc shld(MEM16 a,REG16 b,CL c){Encode(1294,a,b,c);}
					enc shld(R_M16 a,REG16 b,CL c){Encode(1294,a,b,c);}
					enc shld(REG32 a,REG32 b,CL c){Encode(1295,a,b,c);}
					enc shld(MEM32 a,REG32 b,CL c){Encode(1295,a,b,c);}
					enc shld(R_M32 a,REG32 b,CL c){Encode(1295,a,b,c);}
					enc shld(REG64 a,REG64 b,CL c){Encode(1296,a,b,c);}
					enc shld(MEM64 a,REG64 b,CL c){Encode(1296,a,b,c);}
					enc shld(R_M64 a,REG64 b,CL c){Encode(1296,a,b,c);}
					enc shr(REG8 a,CL b){Encode(1298,a,b);}
					enc shr(MEM8 a,CL b){Encode(1298,a,b);}
					enc shr(R_M8 a,CL b){Encode(1298,a,b);}
					enc shr(REG8 a,byte b){Encode(1299,a,(IMM)b);}
					enc shr(AL a,byte b){Encode(1299,a,(IMM)b);}
					enc shr(CL a,byte b){Encode(1299,a,(IMM)b);}
					enc shr(MEM8 a,byte b){Encode(1299,a,(IMM)b);}
					enc shr(R_M8 a,byte b){Encode(1299,a,(IMM)b);}
					enc shr(REG16 a,CL b){Encode(1301,a,b);}
					enc shr(MEM16 a,CL b){Encode(1301,a,b);}
					enc shr(R_M16 a,CL b){Encode(1301,a,b);}
					enc shr(REG16 a,byte b){Encode(1302,a,(IMM)b);}
					enc shr(MEM16 a,byte b){Encode(1302,a,(IMM)b);}
					enc shr(R_M16 a,byte b){Encode(1302,a,(IMM)b);}
					enc shr(REG32 a,CL b){Encode(1304,a,b);}
					enc shr(MEM32 a,CL b){Encode(1304,a,b);}
					enc shr(R_M32 a,CL b){Encode(1304,a,b);}
					enc shr(REG32 a,byte b){Encode(1305,a,(IMM)b);}
					enc shr(MEM32 a,byte b){Encode(1305,a,(IMM)b);}
					enc shr(R_M32 a,byte b){Encode(1305,a,(IMM)b);}
					enc shr(REG64 a,CL b){Encode(1307,a,b);}
					enc shr(MEM64 a,CL b){Encode(1307,a,b);}
					enc shr(R_M64 a,CL b){Encode(1307,a,b);}
					enc shr(REG64 a,byte b){Encode(1308,a,(IMM)b);}
					enc shr(RAX a,byte b){Encode(1308,a,(IMM)b);}
					enc shr(MEM64 a,byte b){Encode(1308,a,(IMM)b);}
					enc shr(R_M64 a,byte b){Encode(1308,a,(IMM)b);}
					enc shrd(REG16 a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(REG16 a,AX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(REG16 a,DX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(REG16 a,CX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(AX a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(DX a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(CX a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(MEM16 a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(MEM16 a,AX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(MEM16 a,DX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(MEM16 a,CX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(R_M16 a,REG16 b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(R_M16 a,AX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(R_M16 a,DX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(R_M16 a,CX b,byte c){Encode(1309,a,b,(IMM)c);}
					enc shrd(REG32 a,REG32 b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(REG32 a,EAX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(REG32 a,ECX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(EAX a,REG32 b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(ECX a,REG32 b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(MEM32 a,REG32 b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(MEM32 a,EAX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(MEM32 a,ECX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(R_M32 a,REG32 b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(R_M32 a,EAX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(R_M32 a,ECX b,byte c){Encode(1310,a,b,(IMM)c);}
					enc shrd(REG64 a,REG64 b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(REG64 a,RAX b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(RAX a,REG64 b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(MEM64 a,REG64 b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(MEM64 a,RAX b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(R_M64 a,REG64 b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(R_M64 a,RAX b,byte c){Encode(1311,a,b,(IMM)c);}
					enc shrd(REG16 a,REG16 b,CL c){Encode(1312,a,b,c);}
					enc shrd(MEM16 a,REG16 b,CL c){Encode(1312,a,b,c);}
					enc shrd(R_M16 a,REG16 b,CL c){Encode(1312,a,b,c);}
					enc shrd(REG32 a,REG32 b,CL c){Encode(1313,a,b,c);}
					enc shrd(MEM32 a,REG32 b,CL c){Encode(1313,a,b,c);}
					enc shrd(R_M32 a,REG32 b,CL c){Encode(1313,a,b,c);}
					enc shrd(REG64 a,REG64 b,CL c){Encode(1314,a,b,c);}
					enc shrd(MEM64 a,REG64 b,CL c){Encode(1314,a,b,c);}
					enc shrd(R_M64 a,REG64 b,CL c){Encode(1314,a,b,c);}
					enc shufpd(XMMREG a,XMMREG b,byte c){Encode(1315,a,b,(IMM)c);}
					enc shufpd(XMMREG a,MEM128 b,byte c){Encode(1315,a,b,(IMM)c);}
					enc shufpd(XMMREG a,R_M128 b,byte c){Encode(1315,a,b,(IMM)c);}
					enc shufps(XMMREG a,XMMREG b,byte c){Encode(1316,a,b,(IMM)c);}
					enc shufps(XMMREG a,MEM128 b,byte c){Encode(1316,a,b,(IMM)c);}
					enc shufps(XMMREG a,R_M128 b,byte c){Encode(1316,a,b,(IMM)c);}
					enc smint(){Encode(1317);}
					enc smintold(){Encode(1318);}
					enc sqrtpd(XMMREG a,XMMREG b){Encode(1319,a,b);}
					enc sqrtpd(XMMREG a,MEM128 b){Encode(1319,a,b);}
					enc sqrtpd(XMMREG a,R_M128 b){Encode(1319,a,b);}
					enc sqrtps(XMMREG a,XMMREG b){Encode(1320,a,b);}
					enc sqrtps(XMMREG a,MEM128 b){Encode(1320,a,b);}
					enc sqrtps(XMMREG a,R_M128 b){Encode(1320,a,b);}
					enc sqrtsd(XMMREG a,XMMREG b){Encode(1321,a,b);}
					enc sqrtsd(XMMREG a,MEM64 b){Encode(1321,a,b);}
					enc sqrtsd(XMMREG a,XMM64 b){Encode(1321,a,b);}
					enc sqrtss(XMMREG a,XMMREG b){Encode(1322,a,b);}
					enc sqrtss(XMMREG a,MEM32 b){Encode(1322,a,b);}
					enc sqrtss(XMMREG a,XMM32 b){Encode(1322,a,b);}
					enc stc(){Encode(1323);}
					enc std(){Encode(1324);}
					enc sti(){Encode(1325);}
					enc stmxcsr(MEM32 a){Encode(1326,a);}
					enc stosb(){Encode(1327);}
					enc stosd(){Encode(1328);}
					enc stosq(){Encode(1329);}
					enc stosw(){Encode(1330);}
					enc sub(REG8 a,REG8 b){Encode(1331,a,b);}
					enc sub(MEM8 a,REG8 b){Encode(1331,a,b);}
					enc sub(R_M8 a,REG8 b){Encode(1331,a,b);}
					enc sub(REG16 a,REG16 b){Encode(1332,a,b);}
					enc sub(MEM16 a,REG16 b){Encode(1332,a,b);}
					enc sub(R_M16 a,REG16 b){Encode(1332,a,b);}
					enc sub(REG32 a,REG32 b){Encode(1333,a,b);}
					enc sub(MEM32 a,REG32 b){Encode(1333,a,b);}
					enc sub(R_M32 a,REG32 b){Encode(1333,a,b);}
					enc sub(REG64 a,REG64 b){Encode(1334,a,b);}
					enc sub(MEM64 a,REG64 b){Encode(1334,a,b);}
					enc sub(R_M64 a,REG64 b){Encode(1334,a,b);}
					enc sub(REG8 a,MEM8 b){Encode(1335,a,b);}
					enc sub(REG8 a,R_M8 b){Encode(1335,a,b);}
					enc sub(REG16 a,MEM16 b){Encode(1336,a,b);}
					enc sub(REG16 a,R_M16 b){Encode(1336,a,b);}
					enc sub(REG32 a,MEM32 b){Encode(1337,a,b);}
					enc sub(REG32 a,R_M32 b){Encode(1337,a,b);}
					enc sub(REG64 a,MEM64 b){Encode(1338,a,b);}
					enc sub(REG64 a,R_M64 b){Encode(1338,a,b);}
					enc sub(REG8 a,byte b){Encode(1339,a,(IMM)b);}
					enc sub(AL a,byte b){Encode(1339,a,(IMM)b);}
					enc sub(CL a,byte b){Encode(1339,a,(IMM)b);}
					enc sub(MEM8 a,byte b){Encode(1339,a,(IMM)b);}
					enc sub(R_M8 a,byte b){Encode(1339,a,(IMM)b);}
					enc sub(REG16 a,byte b){Encode(1340,a,(IMM)b);}
					enc sub(REG16 a,word b){Encode(1340,a,(IMM)b);}
					enc sub(MEM16 a,byte b){Encode(1340,a,(IMM)b);}
					enc sub(MEM16 a,word b){Encode(1340,a,(IMM)b);}
					enc sub(R_M16 a,byte b){Encode(1340,a,(IMM)b);}
					enc sub(R_M16 a,word b){Encode(1340,a,(IMM)b);}
					enc sub(REG32 a,REF b){Encode(1341,a,b);}
					enc sub(REG32 a,dword b){Encode(1341,a,(IMM)b);}
					enc sub(MEM32 a,REF b){Encode(1341,a,b);}
					enc sub(MEM32 a,dword b){Encode(1341,a,(IMM)b);}
					enc sub(R_M32 a,REF b){Encode(1341,a,b);}
					enc sub(R_M32 a,dword b){Encode(1341,a,(IMM)b);}
					enc sub(REG64 a,REF b){Encode(1342,a,b);}
					enc sub(REG64 a,dword b){Encode(1342,a,(IMM)b);}
					enc sub(MEM64 a,REF b){Encode(1342,a,b);}
					enc sub(MEM64 a,dword b){Encode(1342,a,(IMM)b);}
					enc sub(R_M64 a,REF b){Encode(1342,a,b);}
					enc sub(R_M64 a,dword b){Encode(1342,a,(IMM)b);}
					enc subpd(XMMREG a,XMMREG b){Encode(1350,a,b);}
					enc subpd(XMMREG a,MEM128 b){Encode(1350,a,b);}
					enc subpd(XMMREG a,R_M128 b){Encode(1350,a,b);}
					enc subps(XMMREG a,XMMREG b){Encode(1351,a,b);}
					enc subps(XMMREG a,MEM128 b){Encode(1351,a,b);}
					enc subps(XMMREG a,R_M128 b){Encode(1351,a,b);}
					enc subsd(XMMREG a,XMMREG b){Encode(1352,a,b);}
					enc subsd(XMMREG a,MEM64 b){Encode(1352,a,b);}
					enc subsd(XMMREG a,XMM64 b){Encode(1352,a,b);}
					enc subss(XMMREG a,XMMREG b){Encode(1353,a,b);}
					enc subss(XMMREG a,MEM32 b){Encode(1353,a,b);}
					enc subss(XMMREG a,XMM32 b){Encode(1353,a,b);}
					enc sysenter(){Encode(1354);}
					enc test(REG8 a,REG8 b){Encode(1355,a,b);}
					enc test(MEM8 a,REG8 b){Encode(1355,a,b);}
					enc test(R_M8 a,REG8 b){Encode(1355,a,b);}
					enc test(REG16 a,REG16 b){Encode(1356,a,b);}
					enc test(MEM16 a,REG16 b){Encode(1356,a,b);}
					enc test(R_M16 a,REG16 b){Encode(1356,a,b);}
					enc test(REG32 a,REG32 b){Encode(1357,a,b);}
					enc test(MEM32 a,REG32 b){Encode(1357,a,b);}
					enc test(R_M32 a,REG32 b){Encode(1357,a,b);}
					enc test(REG64 a,REG64 b){Encode(1358,a,b);}
					enc test(MEM64 a,REG64 b){Encode(1358,a,b);}
					enc test(R_M64 a,REG64 b){Encode(1358,a,b);}
					enc test(REG8 a,byte b){Encode(1359,a,(IMM)b);}
					enc test(AL a,byte b){Encode(1359,a,(IMM)b);}
					enc test(CL a,byte b){Encode(1359,a,(IMM)b);}
					enc test(MEM8 a,byte b){Encode(1359,a,(IMM)b);}
					enc test(R_M8 a,byte b){Encode(1359,a,(IMM)b);}
					enc test(REG16 a,byte b){Encode(1360,a,(IMM)b);}
					enc test(REG16 a,word b){Encode(1360,a,(IMM)b);}
					enc test(MEM16 a,byte b){Encode(1360,a,(IMM)b);}
					enc test(MEM16 a,word b){Encode(1360,a,(IMM)b);}
					enc test(R_M16 a,byte b){Encode(1360,a,(IMM)b);}
					enc test(R_M16 a,word b){Encode(1360,a,(IMM)b);}
					enc test(REG32 a,REF b){Encode(1361,a,b);}
					enc test(REG32 a,dword b){Encode(1361,a,(IMM)b);}
					enc test(MEM32 a,REF b){Encode(1361,a,b);}
					enc test(MEM32 a,dword b){Encode(1361,a,(IMM)b);}
					enc test(R_M32 a,REF b){Encode(1361,a,b);}
					enc test(R_M32 a,dword b){Encode(1361,a,(IMM)b);}
					enc test(REG64 a,REF b){Encode(1362,a,b);}
					enc test(REG64 a,dword b){Encode(1362,a,(IMM)b);}
					enc test(MEM64 a,REF b){Encode(1362,a,b);}
					enc test(MEM64 a,dword b){Encode(1362,a,(IMM)b);}
					enc test(R_M64 a,REF b){Encode(1362,a,b);}
					enc test(R_M64 a,dword b){Encode(1362,a,(IMM)b);}
					enc ucomisd(XMMREG a,XMMREG b){Encode(1367,a,b);}
					enc ucomisd(XMMREG a,MEM64 b){Encode(1367,a,b);}
					enc ucomisd(XMMREG a,XMM64 b){Encode(1367,a,b);}
					enc ucomiss(XMMREG a,XMMREG b){Encode(1368,a,b);}
					enc ucomiss(XMMREG a,MEM32 b){Encode(1368,a,b);}
					enc ucomiss(XMMREG a,XMM32 b){Encode(1368,a,b);}
					enc ud2(){Encode(1369);}
					enc unpckhpd(XMMREG a,XMMREG b){Encode(1370,a,b);}
					enc unpckhpd(XMMREG a,MEM128 b){Encode(1370,a,b);}
					enc unpckhpd(XMMREG a,R_M128 b){Encode(1370,a,b);}
					enc unpckhps(XMMREG a,XMMREG b){Encode(1371,a,b);}
					enc unpckhps(XMMREG a,MEM128 b){Encode(1371,a,b);}
					enc unpckhps(XMMREG a,R_M128 b){Encode(1371,a,b);}
					enc unpcklpd(XMMREG a,XMMREG b){Encode(1372,a,b);}
					enc unpcklpd(XMMREG a,MEM128 b){Encode(1372,a,b);}
					enc unpcklpd(XMMREG a,R_M128 b){Encode(1372,a,b);}
					enc unpcklps(XMMREG a,XMMREG b){Encode(1373,a,b);}
					enc unpcklps(XMMREG a,MEM128 b){Encode(1373,a,b);}
					enc unpcklps(XMMREG a,R_M128 b){Encode(1373,a,b);}
					enc wait(){Encode(1374);}
					enc wrmsr(){Encode(1375);}
					enc xadd(REG8 a,REG8 b){Encode(1376,a,b);}
					enc xadd(MEM8 a,REG8 b){Encode(1376,a,b);}
					enc xadd(R_M8 a,REG8 b){Encode(1376,a,b);}
					enc xadd(REG16 a,REG16 b){Encode(1377,a,b);}
					enc xadd(MEM16 a,REG16 b){Encode(1377,a,b);}
					enc xadd(R_M16 a,REG16 b){Encode(1377,a,b);}
					enc xadd(REG32 a,REG32 b){Encode(1378,a,b);}
					enc xadd(MEM32 a,REG32 b){Encode(1378,a,b);}
					enc xadd(R_M32 a,REG32 b){Encode(1378,a,b);}
					enc xadd(REG64 a,REG64 b){Encode(1379,a,b);}
					enc xadd(MEM64 a,REG64 b){Encode(1379,a,b);}
					enc xadd(R_M64 a,REG64 b){Encode(1379,a,b);}
					enc xchg(REG8 a,REG8 b){Encode(1380,a,b);}
					enc xchg(REG8 a,MEM8 b){Encode(1380,a,b);}
					enc xchg(REG8 a,R_M8 b){Encode(1380,a,b);}
					enc xchg(REG16 a,REG16 b){Encode(1381,a,b);}
					enc xchg(REG16 a,MEM16 b){Encode(1381,a,b);}
					enc xchg(REG16 a,R_M16 b){Encode(1381,a,b);}
					enc xchg(REG32 a,REG32 b){Encode(1382,a,b);}
					enc xchg(REG32 a,MEM32 b){Encode(1382,a,b);}
					enc xchg(REG32 a,R_M32 b){Encode(1382,a,b);}
					enc xchg(REG64 a,REG64 b){Encode(1383,a,b);}
					enc xchg(REG64 a,MEM64 b){Encode(1383,a,b);}
					enc xchg(REG64 a,R_M64 b){Encode(1383,a,b);}
					enc xchg(MEM8 a,REG8 b){Encode(1384,a,b);}
					enc xchg(R_M8 a,REG8 b){Encode(1384,a,b);}
					enc xchg(MEM16 a,REG16 b){Encode(1385,a,b);}
					enc xchg(R_M16 a,REG16 b){Encode(1385,a,b);}
					enc xchg(MEM32 a,REG32 b){Encode(1386,a,b);}
					enc xchg(R_M32 a,REG32 b){Encode(1386,a,b);}
					enc xchg(MEM64 a,REG64 b){Encode(1387,a,b);}
					enc xchg(R_M64 a,REG64 b){Encode(1387,a,b);}
					enc xlatb(){Encode(1394);}
					enc xor(REG8 a,REG8 b){Encode(1395,a,b);}
					enc xor(MEM8 a,REG8 b){Encode(1395,a,b);}
					enc xor(R_M8 a,REG8 b){Encode(1395,a,b);}
					enc xor(REG16 a,REG16 b){Encode(1396,a,b);}
					enc xor(MEM16 a,REG16 b){Encode(1396,a,b);}
					enc xor(R_M16 a,REG16 b){Encode(1396,a,b);}
					enc xor(REG32 a,REG32 b){Encode(1397,a,b);}
					enc xor(MEM32 a,REG32 b){Encode(1397,a,b);}
					enc xor(R_M32 a,REG32 b){Encode(1397,a,b);}
					enc xor(REG64 a,REG64 b){Encode(1398,a,b);}
					enc xor(MEM64 a,REG64 b){Encode(1398,a,b);}
					enc xor(R_M64 a,REG64 b){Encode(1398,a,b);}
					enc xor(REG8 a,MEM8 b){Encode(1399,a,b);}
					enc xor(REG8 a,R_M8 b){Encode(1399,a,b);}
					enc xor(REG16 a,MEM16 b){Encode(1400,a,b);}
					enc xor(REG16 a,R_M16 b){Encode(1400,a,b);}
					enc xor(REG32 a,MEM32 b){Encode(1401,a,b);}
					enc xor(REG32 a,R_M32 b){Encode(1401,a,b);}
					enc xor(REG64 a,MEM64 b){Encode(1402,a,b);}
					enc xor(REG64 a,R_M64 b){Encode(1402,a,b);}
					enc xor(REG8 a,byte b){Encode(1403,a,(IMM)b);}
					enc xor(AL a,byte b){Encode(1403,a,(IMM)b);}
					enc xor(CL a,byte b){Encode(1403,a,(IMM)b);}
					enc xor(MEM8 a,byte b){Encode(1403,a,(IMM)b);}
					enc xor(R_M8 a,byte b){Encode(1403,a,(IMM)b);}
					enc xor(REG16 a,byte b){Encode(1404,a,(IMM)b);}
					enc xor(REG16 a,word b){Encode(1404,a,(IMM)b);}
					enc xor(MEM16 a,byte b){Encode(1404,a,(IMM)b);}
					enc xor(MEM16 a,word b){Encode(1404,a,(IMM)b);}
					enc xor(R_M16 a,byte b){Encode(1404,a,(IMM)b);}
					enc xor(R_M16 a,word b){Encode(1404,a,(IMM)b);}
					enc xor(REG32 a,REF b){Encode(1405,a,b);}
					enc xor(REG32 a,dword b){Encode(1405,a,(IMM)b);}
					enc xor(MEM32 a,REF b){Encode(1405,a,b);}
					enc xor(MEM32 a,dword b){Encode(1405,a,(IMM)b);}
					enc xor(R_M32 a,REF b){Encode(1405,a,b);}
					enc xor(R_M32 a,dword b){Encode(1405,a,(IMM)b);}
					enc xor(REG64 a,REF b){Encode(1406,a,b);}
					enc xor(REG64 a,dword b){Encode(1406,a,(IMM)b);}
					enc xor(MEM64 a,REF b){Encode(1406,a,b);}
					enc xor(MEM64 a,dword b){Encode(1406,a,(IMM)b);}
					enc xor(R_M64 a,REF b){Encode(1406,a,b);}
					enc xor(R_M64 a,dword b){Encode(1406,a,(IMM)b);}
					enc xorps(XMMREG a,XMMREG b){Encode(1414,a,b);}
					enc xorps(XMMREG a,MEM128 b){Encode(1414,a,b);}
					enc xorps(XMMREG a,R_M128 b){Encode(1414,a,b);}

					#undef enc

				};

			}
		}
	}
}
