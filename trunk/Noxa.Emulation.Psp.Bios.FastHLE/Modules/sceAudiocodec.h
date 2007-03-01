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

					ref class sceAudiocodec : public Module
					{
					public:
						sceAudiocodec( Kernel^ kernel ) : Module( kernel ) {}
						~sceAudiocodec(){}

						property String^ Name { virtual String^ get() override { return "sceAudiocodec"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x5B37EB1D, "sceAudiocodecInit" )] [Stateless]
						// /audio/pspaudiocodec.h:18: int sceAudiocodecInit(unsigned long *Buffer, int Type);
						int sceAudiocodecInit( int Buffer, int Type ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x70A703F8, "sceAudiocodecDecode" )] [Stateless]
						// /audio/pspaudiocodec.h:19: int sceAudiocodecDecode(unsigned long *Buffer, int Type);
						int sceAudiocodecDecode( int Buffer, int Type ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3A20A200, "sceAudiocodecGetEDRAM" )] [Stateless]
						// /audio/pspaudiocodec.h:20: int sceAudiocodecGetEDRAM(unsigned long *Buffer, int Type);
						int sceAudiocodecGetEDRAM( int Buffer, int Type ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x29681260, "sceAudiocodecReleaseEDRAM" )] [Stateless]
						// /audio/pspaudiocodec.h:21: int sceAudiocodecReleaseEDRAM(unsigned long *Buffer);
						int sceAudiocodecReleaseEDRAM( int Buffer ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B29475AF */
