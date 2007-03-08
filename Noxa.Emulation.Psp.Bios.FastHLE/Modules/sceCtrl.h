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
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Input;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					public ref class sceCtrl : public Module
					{
					internal:
						enum class ControlSamplingMode
						{
							DigitalOnly = 0,
							AnalogAndDigital = 1
						};

						ref class ControlSample
						{
						public:
							uint		Timestamp;
							PadButtons	Buttons;
							int			AnalogX;
							int			AnalogY;
						};

						int								_sampleCycle;
						ControlSamplingMode				_sampleMode;
						CircularList<ControlSample^>^	_buffer;
						AutoResetEvent^					_dataPresent;
						
						bool							_threadRunning;
						Thread^							_thread;

						static const int				InputPollInterval = 75;

					public:
						sceCtrl( Kernel^ kernel ) : Module( kernel ) {}
						~sceCtrl(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceCtrl"; } }

						virtual void Start() override;
						virtual void Stop() override;
						virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;
						void InputThread();

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[BiosFunction( 0x6A2774F3, "sceCtrlSetSamplingCycle" )] [Stateless]
						// int sceCtrlSetSamplingCycle(int cycle); (/ctrl/pspctrl.h:119)
						int sceCtrlSetSamplingCycle( int cycle );

						[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )] [Stateless]
						// int sceCtrlGetSamplingCycle(int *pcycle); (/ctrl/pspctrl.h:128)
						int sceCtrlGetSamplingCycle( IMemory^ memory, int pcycle );

						[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )] [Stateless]
						// int sceCtrlSetSamplingMode(int mode); (/ctrl/pspctrl.h:137)
						int sceCtrlSetSamplingMode( int mode );

						[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )] [Stateless]
						// int sceCtrlGetSamplingMode(int *pmode); (/ctrl/pspctrl.h:146)
						int sceCtrlGetSamplingMode( IMemory^ memory, int pmode );

						[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )] [Stateless]
						// int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:148)
						int sceCtrlPeekBufferPositive( IMemory^ memory, int pad_data, int count );

						[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )] [Stateless]
						// int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:150)
						int sceCtrlPeekBufferNegative( IMemory^ memory, int pad_data, int count );

						[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )] [Stateless]
						// int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:168)
						int sceCtrlReadBufferPositive( IMemory^ memory, int pad_data, int count );

						[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )] [Stateless]
						// int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:170)
						int sceCtrlReadBufferNegative( IMemory^ memory, int pad_data, int count );

						[NotImplemented]
						[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )] [Stateless]
						// int sceCtrlPeekLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:172)
						int sceCtrlPeekLatch( IMemory^ memory, int latch_data );

						[NotImplemented]
						[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )] [Stateless]
						// int sceCtrlReadLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:174)
						int sceCtrlReadLatch( IMemory^ memory, int latch_data );

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - A6CBB232 */
