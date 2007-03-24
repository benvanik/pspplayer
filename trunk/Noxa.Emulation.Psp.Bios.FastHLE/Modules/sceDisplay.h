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

					public ref class sceDisplay : public Module
					{
					public:
						sceDisplay( Kernel^ kernel ) : Module( kernel ) {}
						~sceDisplay(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceDisplay"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xDBA6C4C4, "sceDisplayGetFramePerSec" )] [Stateless]
						// manual add - is this int or float return?
						int sceDisplayGetFramePerSec(){ return NISTUBRETURN; }

						[BiosFunction( 0x0E20F177, "sceDisplaySetMode" )] [Stateless]
						// int sceDisplaySetMode(int mode, int width, int height); (/display/pspdisplay.h:53)
						int sceDisplaySetMode( int mode, int width, int height );

						[BiosFunction( 0xDEA197D4, "sceDisplayGetMode" )] [Stateless]
						// int sceDisplayGetMode(int *pmode, int *pwidth, int *pheight); (/display/pspdisplay.h:64)
						int sceDisplayGetMode( IMemory^ memory, int pmode, int pwidth, int pheight );

						[BiosFunction( 0x289D82FE, "sceDisplaySetFrameBuf" )] [Stateless]
						// void sceDisplaySetFrameBuf(void *topaddr, int bufferwidth, int pixelformat, int sync); (/display/pspdisplay.h:74)
						int sceDisplaySetFrameBuf( IMemory^ memory, int topaddr, int bufferwidth, int pixelformat, int sync );

						[BiosFunction( 0xEEDA2E54, "sceDisplayGetFrameBuf" )] [Stateless]
						// int sceDisplayGetFrameBuf(void **topaddr, int *bufferwidth, int *pixelformat, int *unk1); (/display/pspdisplay.h:84)
						int sceDisplayGetFrameBuf( IMemory^ memory, int topaddr, int bufferwidth, int pixelformat, int unk1 );

						[BiosFunction( 0x9C6EAAD7, "sceDisplayGetVcount" )] [Stateless]
						// unsigned int sceDisplayGetVcount(); (/display/pspdisplay.h:89)
						int sceDisplayGetVcount();

						[BiosFunction( 0x773DD3A3, "sceDisplayGetCurrentHcount" )] [Stateless]
						// manual add
						int sceDisplayGetCurrentHcount(){ return 0; }

						// This should be a video api call
						[NotImplemented]
						[BiosFunction( 0x210EAB3A, "sceDisplayGetAccumulatedHcount" )] [Stateless]
						// manual add
						int sceDisplayGetAccumulatedHcount(){ return NISTUBRETURN; }

						[BiosFunction( 0x36CDFADE, "sceDisplayWaitVblank" )] [Stateless]
						// int sceDisplayWaitVblank(); (/display/pspdisplay.h:104)
						int sceDisplayWaitVblank();

						[NotImplemented]
						[BiosFunction( 0x8EB9EC49, "sceDisplayWaitVblankCB" )]
						// int sceDisplayWaitVblankCB(); (/display/pspdisplay.h:109)
						int sceDisplayWaitVblankCB();

						[BiosFunction( 0x984C27E7, "sceDisplayWaitVblankStart" )] [Stateless]
						// int sceDisplayWaitVblankStart(); (/display/pspdisplay.h:94)
						int sceDisplayWaitVblankStart();

						[NotImplemented]
						[BiosFunction( 0x46F186C3, "sceDisplayWaitVblankStartCB" )]
						// int sceDisplayWaitVblankStartCB(); (/display/pspdisplay.h:99)
						int sceDisplayWaitVblankStartCB();

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - CD31A16C */
