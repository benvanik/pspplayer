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
#define CTXCP1REGS		148
#define CTXCP1CONDBIT	276
//#define CTXCP0REGS	
//#define CTXCP0CONTROL	
#define CTXSIZE			280

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {
				
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
					float	Cp1Registers[ 32 ];		// +148 (128)
					int		Cp1ConditionBit;		// +276
					//int		Cp0Registers[ 32 ];		// + (128)
					//int		Cp0Control[ 32 ];		// + (128)
				} R4000Ctx;
				#pragma pack(pop)

				typedef int (*bouncefn)( int targetAddress );
			}
		}
	}
}
