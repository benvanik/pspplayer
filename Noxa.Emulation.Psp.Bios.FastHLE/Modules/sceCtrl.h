// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					ref class sceCtrl : public Module
					{
					public:
						sceCtrl( Kernel^ kernel ) : Module( kernel ) {}
						~sceCtrl(){}

						property String^ Name { virtual String^ get() override { return "sceCtrl"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x6A2774F3, "sceCtrlSetSamplingCycle" )] [Stateless]
						// /ctrl/pspctrl.h:119: int sceCtrlSetSamplingCycle(int cycle);
						int sceCtrlSetSamplingCycle( int cycle ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )] [Stateless]
						// /ctrl/pspctrl.h:128: int sceCtrlGetSamplingCycle(int *pcycle);
						int sceCtrlGetSamplingCycle( int pcycle ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )] [Stateless]
						// /ctrl/pspctrl.h:137: int sceCtrlSetSamplingMode(int mode);
						int sceCtrlSetSamplingMode( int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )] [Stateless]
						// /ctrl/pspctrl.h:146: int sceCtrlGetSamplingMode(int *pmode);
						int sceCtrlGetSamplingMode( int pmode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )] [Stateless]
						// /ctrl/pspctrl.h:148: int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count);
						int sceCtrlPeekBufferPositive( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )] [Stateless]
						// /ctrl/pspctrl.h:150: int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count);
						int sceCtrlPeekBufferNegative( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )] [Stateless]
						// /ctrl/pspctrl.h:168: int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count);
						int sceCtrlReadBufferPositive( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )] [Stateless]
						// /ctrl/pspctrl.h:170: int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count);
						int sceCtrlReadBufferNegative( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )] [Stateless]
						// /ctrl/pspctrl.h:172: int sceCtrlPeekLatch(SceCtrlLatch *latch_data);
						int sceCtrlPeekLatch( int latch_data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )] [Stateless]
						// /ctrl/pspctrl.h:174: int sceCtrlReadLatch(SceCtrlLatch *latch_data);
						int sceCtrlReadLatch( int latch_data ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - C5D8CDE0 */
