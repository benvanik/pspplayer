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

					public ref class sceAtrac3plus : public Module
					{
					public:
						sceAtrac3plus( Kernel^ kernel ) : Module( kernel ) {}
						~sceAtrac3plus(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceAtrac3plus"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						enum class AtracCodec
						{
							At3Plus			= 0x00001000,
							At3				= 0x00001001,
						};

						// NEEDED:
						// 0x5d268707 » sceAtracGetStreamDataInfo
						// 0x7db31251 » sceAtracAddStreamData
						// 0x83e85ea0 » sceAtracGetSecondBufferInfo
						// 0x83bf7afd » sceAtracSetSecondBuffer
						// 0xe23e3a35 » sceAtracGetNextDecodePosition
						// 0xa2bba8be » sceAtracGetSoundSample
						// 0xca3ca3d2 » sceAtracGetBufferInfoForReseting
						// 0x644e5607 » sceAtracResetPlayPosition
						// 0xe88f759b » sceAtracGetInternalErrorInfo
						// Not sure if any of these are actually used

						[NotImplemented]
						[BiosFunction( 0x780F88D1, "sceAtracGetAtracID" )] [Stateless]
						// manual add
						int sceAtracGetAtracID( int codec ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x61EB33F5, "sceAtracReleaseAtracID" )] [Stateless]
						// int sceAtracReleaseAtracID(int atracID); (/atrac3/pspatrac3.h:87)
						int sceAtracReleaseAtracID( int atracID ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0E2A73AB, "sceAtracSetData" )] [Stateless]
						// manual add
						int sceAtracSetData( int atracID, int buf, int bufsize ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7A20E7AF, "sceAtracSetDataAndGetID" )] [Stateless]
						// int sceAtracSetDataAndGetID(void *buf, SceSize bufsize); (/atrac3/pspatrac3.h:27)
						int sceAtracSetDataAndGetID( int buf, int bufsize ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6A8C3CD5, "sceAtracDecodeData" )] [Stateless]
						// int sceAtracDecodeData(int atracID, u16 *outSamples, int *outN, int *outEnd, int *outRemainFrame); (/atrac3/pspatrac3.h:43)
						int sceAtracDecodeData( int atracID, int outSamples, int outN, int outEnd, int outRemainFrame ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9AE849A7, "sceAtracGetRemainFrame" )] [Stateless]
						// int sceAtracGetRemainFrame(int atracID, int *outRemainFrame); (/atrac3/pspatrac3.h:55)
						int sceAtracGetRemainFrame( int atracID, int outRemainFrame ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD6A5F2F7, "sceAtracGetMaxSample" )] [Stateless]
						// int sceAtracGetMaxSample(int atracID, int *outMax); (/atrac3/pspatrac3.h:109)
						int sceAtracGetMaxSample( int atracID, int outMax ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x36FAABFB, "sceAtracGetNextSample" )] [Stateless]
						// int sceAtracGetNextSample(int atracID, int *outN); (/atrac3/pspatrac3.h:98)
						int sceAtracGetNextSample( int atracID, int outN ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA554A158, "sceAtracGetBitrate" )] [Stateless]
						// int sceAtracGetBitrate(int atracID, int *outBitrate); (/atrac3/pspatrac3.h:66)
						int sceAtracGetBitrate( int atracID, int outBitrate ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x868120B5, "sceAtracSetLoopNum" )] [Stateless]
						// int sceAtracSetLoopNum(int atracID, int nloops); (/atrac3/pspatrac3.h:77)
						int sceAtracSetLoopNum( int atracID, int nloops ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE88F759B, "sceAtracGetInternalErrorInfo" )] [Stateless]
						// manual add
						int sceAtracGetInternalErrorInfo(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 7A4A3A56 */
