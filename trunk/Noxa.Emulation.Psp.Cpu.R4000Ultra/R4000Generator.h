// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000GenContext.h"
#include "R4000Ctx.h"

#include "CodeGenerator.hpp"

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace SoftWire;

#define EAX g->eax
#define EBX g->ebx
#define ECX g->ecx
#define EDX g->edx
#define ESP g->esp
#define AX g->ax
#define BX g->bx
#define CX g->cx
#define AL g->al
#define BL g->bl
#define CL g->cl

#define ZE( x ) ((int)(uint)x)
#define SE( x ) ((int)(short)x)

#define LOADCTXBASE( xr )	
#define CTX					CTXP( context->CtxPointer )
#define CTXP( x )			( ( int )x )
#define MREG( xr, r )		g->dword_ptr[ xr + CTXREGS + ( r << 2 ) ]
#define MLO( xr )			g->dword_ptr[ xr + CTXLO ]
#define MHI( xr )			g->dword_ptr[ xr + CTXHI ]
#define MNULLDELAY( xr )	g->dword_ptr[ xr + CTXNULLDELAY ]
#define MPCVALID( xr )		g->dword_ptr[ xr + CTXPCVALID ]
#define MPC( xr )			g->dword_ptr[ xr + CTXPC ]
#define MCP1REG( xr, r )	g->dword_ptr[ xr + CTXCP1REGS + ( r << 2 ) ]
#define MCP1CONDBIT( xr )	g->dword_ptr[ xr + CTXCP1CONDBIT ]

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				typedef GenerationResult (*GenerateInstructionR)( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function );
				typedef GenerationResult (*GenerateInstructionI)( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm );
				typedef GenerationResult (*GenerateInstructionJ)( R4000GenContext^ context, int pass, int address, uint code, byte opcode, uint imm );
				typedef GenerationResult (*GenerateInstructionCop0)( R4000GenContext^ context, int pass, int address, uint code, byte function );
				typedef GenerationResult (*GenerateInstructionSpecial3)( R4000GenContext^ context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl );
				typedef GenerationResult (*GenerateInstructionFpu)( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function );

				class R4000Generator : public CodeGenerator
				{
				public:
					R4000Generator();
					void Setup();

					int getCodeLength();

				public:
					static GenerateInstructionR TableR[ 64 ];
					static GenerateInstructionI TableI[ 64 ];
					static GenerateInstructionJ TableJ[ 64 ];
					static GenerateInstructionR TableAllegrex[ 64 ];
					static GenerateInstructionSpecial3 TableSpecial3[ 64 ];
				};

			}
		}
	}
}
