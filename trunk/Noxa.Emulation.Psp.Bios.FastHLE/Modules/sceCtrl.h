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

					public ref class sceCtrl : public Module
					{
					public:
						sceCtrl( Kernel^ kernel ) : Module( kernel ) {}
						~sceCtrl(){}

					public:
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
						// int sceCtrlSetSamplingCycle(int cycle); (/ctrl/pspctrl.h:119)
						int sceCtrlSetSamplingCycle( int cycle ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )] [Stateless]
						// int sceCtrlGetSamplingCycle(int *pcycle); (/ctrl/pspctrl.h:128)
						int sceCtrlGetSamplingCycle( int pcycle ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )] [Stateless]
						// int sceCtrlSetSamplingMode(int mode); (/ctrl/pspctrl.h:137)
						int sceCtrlSetSamplingMode( int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )] [Stateless]
						// int sceCtrlGetSamplingMode(int *pmode); (/ctrl/pspctrl.h:146)
						int sceCtrlGetSamplingMode( int pmode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )] [Stateless]
						// int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:148)
						int sceCtrlPeekBufferPositive( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )] [Stateless]
						// int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:150)
						int sceCtrlPeekBufferNegative( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )] [Stateless]
						// int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:168)
						int sceCtrlReadBufferPositive( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )] [Stateless]
						// int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:170)
						int sceCtrlReadBufferNegative( int pad_data, int count ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )] [Stateless]
						// int sceCtrlPeekLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:172)
						int sceCtrlPeekLatch( int latch_data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )] [Stateless]
						// int sceCtrlReadLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:174)
						int sceCtrlReadLatch( int latch_data ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - A6CBB232 */
