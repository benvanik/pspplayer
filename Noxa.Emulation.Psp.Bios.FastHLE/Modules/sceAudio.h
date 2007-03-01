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

					ref class sceAudio : public Module
					{
					public:
						sceAudio( Kernel^ kernel ) : Module( kernel ) {}
						~sceAudio(){}

						property String^ Name { virtual String^ get() override { return "sceAudio"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x8C1009B2, "sceAudioOutput" )] [Stateless]
						// /audio/pspaudio.h:73: int sceAudioOutput(int channel, int vol, void *buf);
						int sceAudioOutput( int channel, int vol, int buf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x136CAF51, "sceAudioOutputBlocking" )] [Stateless]
						// /audio/pspaudio.h:79: int sceAudioOutputBlocking(int channel, int vol, void *buf);
						int sceAudioOutputBlocking( int channel, int vol, int buf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE2D56B2D, "sceAudioOutputPanned" )] [Stateless]
						// /audio/pspaudio.h:85: int sceAudioOutputPanned(int channel, int leftvol, int rightvol, void *buffer);
						int sceAudioOutputPanned( int channel, int leftvol, int rightvol, int buffer ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x13F592BC, "sceAudioOutputPannedBlocking" )] [Stateless]
						// /audio/pspaudio.h:91: int sceAudioOutputPannedBlocking(int channel, int leftvol, int rightvol, void *buffer);
						int sceAudioOutputPannedBlocking( int channel, int leftvol, int rightvol, int buffer ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5EC81C55, "sceAudioChReserve" )] [Stateless]
						// /audio/pspaudio.h:61: int sceAudioChReserve(int channel, int samplecount, int format);
						int sceAudioChReserve( int channel, int samplecount, int format ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6FC46853, "sceAudioChRelease" )] [Stateless]
						// /audio/pspaudio.h:70: int sceAudioChRelease(int channel);
						int sceAudioChRelease( int channel ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE9D97901, "sceAudioGetChannelRestLen" )] [Stateless]
						// /audio/pspaudio.h:97: int sceAudioGetChannelRestLen(int channel);
						int sceAudioGetChannelRestLen( int channel ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCB2E439E, "sceAudioSetChannelDataLen" )] [Stateless]
						// /audio/pspaudio.h:103: int sceAudioSetChannelDataLen(int channel, int samplecount);
						int sceAudioSetChannelDataLen( int channel, int samplecount ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x95FD0C2D, "sceAudioChangeChannelConfig" )] [Stateless]
						// /audio/pspaudio.h:109: int sceAudioChangeChannelConfig(int channel, int format);
						int sceAudioChangeChannelConfig( int channel, int format ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB7E1D8E7, "sceAudioChangeChannelVolume" )] [Stateless]
						// /audio/pspaudio.h:115: int sceAudioChangeChannelVolume(int channel, int leftvol, int rightvol);
						int sceAudioChangeChannelVolume( int channel, int leftvol, int rightvol ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - EF1722BE */
