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
#define CTXUNUSED0		148
#define CTXLL			152
#define CTXCP1CONDBIT	156
#define CTXCP1REGS		160
//#define CTXCP0REGS	
//#define CTXCP0CONTROL	
#define CTXSIZE			672

#define SSE_ALIGN __declspec( align( 16 ) )

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				// Note: for perf, everything should be 32 bit ints
				// Note: Cp1Registers MUST be 16-byte aligned for SSE
				
				#pragma pack(push)
				#pragma pack(1)
				typedef struct R4000Ctx_t
				{
													// Start offset
					int		Registers[ 32 ];		// +0 (128)
					int		LO;						// +128
					int		HI;						// +132
					int		NullifyDelay;			// +136
					int		PCValid;				// +140
					int		PC;						// +144
					int		Unused0;				// +148
					int		LL;						// +152
					int		Cp1ConditionBit;		// +156
					SSE_ALIGN
					float	Cp1Registers[ 32 * 4 ];	// +160 (512) - large because we want 16b aligned elements
					//int		Cp0Registers[ 32 ];		// + (128)
					//int		Cp0Control[ 32 ];		// + (128)
				} R4000Ctx;
				#pragma pack(pop)

				typedef int (*bouncefn)( int targetAddress );
			}
		}
	}
}
