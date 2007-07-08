// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

#define CTXREGS			0
#define CTXLO			128
#define CTXHI			132
#define CTXNULLDELAY	136
#define CTXPCVALID		140
#define CTXPC			144
#define CTXINTMASK		148
#define CTXLL			152
#define CTXCP1CONDBIT	156
#define CTXCP1REGS		160
#define CTXCP2REGS		672
#define CTXSTOPFLAG		1184
#define CTXCP2PFX		1188
#define CTXCP2WM		1200
//#define CTXCP0REGS	
//#define CTXCP0CONTROL	
#define CTXSIZE			1204

#define SSE_ALIGN __declspec( align( 16 ) )

// This address is used as $ra to detect the end of a marshalled call/interrupt
#define CUSTOM_METHOD_TRAP	0xCAFE0000
#define CALL_RETURN_DUMMY	CUSTOM_METHOD_TRAP + 1
#define BIOS_SAFETY_DUMMY	CUSTOM_METHOD_TRAP + 2

// Callstack sentinel values
#define CALLSTACK_SENTINEL	0xBABE0000
#define CS_MARSHALLED_CALL	CALLSTACK_SENTINEL + 1
#define CS_INTERRUPT		CALLSTACK_SENTINEL + 2

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				enum CtxStopFlags
				{
					// Continue execution (no stop)
					CtxContinue			= 0,
					// BIOS requested a break
					CtxBreakRequest		= 1,
					// BIOS wants to make a context switch
					CtxContextSwitch	= 2,
					// BIOS is marshalling a call
					CtxMarshal			= 3,
					// Interrupt pending
					CtxInterruptPending	= 4,
					// BreakAndWait request
					CtxBreakAndWait		= 5,
				};

				// Note: for perf, everything should be 32 bit ints
				// Note: Cp1Registers MUST be 16-byte aligned for SSE
				
				#pragma pack(push)
				#pragma pack(1)
				typedef struct R4000Ctx_t
				{
													// Start offset
					uint			Registers[ 32 ];		// +0 (128)
					uint			LO;						// +128
					uint			HI;						// +132
					int				NullifyDelay;			// +136
					int				PCValid;				// +140
					uint			PC;						// +144
					int				InterruptMask;			// +148
					int				LL;						// +152
					int				Cp1ConditionBit;		// +156
					SSE_ALIGN
					float			Cp1Registers[ 32 * 4 ];	// +160 (512) - large because we want 16b aligned elements
					SSE_ALIGN
					float			Cp2Registers[ 128 ];	// +672 (512) - not aligned because we may access as packed
					CtxStopFlags	StopFlag;				// +1184 - used to detect stop conditions
					int				Cp2Pfx[ 3 ];			// +1188 (12) - s, t, d prefixes
					int				Cp2Wm;					// +1200 - write mask (bits 0-3)
					//int			Cp0Registers[ 32 ];		// + (128)
					//int			Cp0Control[ 32 ];		// + (128)
				} R4000Ctx;
				#pragma pack(pop)

				typedef int (*bouncefn)( int targetAddress );
			}
		}
	}
}
