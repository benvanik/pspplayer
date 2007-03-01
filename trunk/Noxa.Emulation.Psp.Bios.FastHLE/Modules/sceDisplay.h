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

					ref class sceDisplay : public Module
					{
					public:
						sceDisplay( Kernel^ kernel ) : Module( kernel ) {}
						~sceDisplay(){}

						property String^ Name { virtual String^ get() override { return "sceDisplay"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x0E20F177, "sceDisplaySetMode" )] [Stateless]
						// int sceDisplaySetMode(int mode, int width, int height); (/display/pspdisplay.h:53)
						int sceDisplaySetMode( int mode, int width, int height ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDEA197D4, "sceDisplayGetMode" )] [Stateless]
						// int sceDisplayGetMode(int *pmode, int *pwidth, int *pheight); (/display/pspdisplay.h:64)
						int sceDisplayGetMode( int pmode, int pwidth, int pheight ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x289D82FE, "sceDisplaySetFrameBuf" )] [Stateless]
						// void sceDisplaySetFrameBuf(void *topaddr, int bufferwidth, int pixelformat, int sync); (/display/pspdisplay.h:74)
						void sceDisplaySetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int sync ){}

						[NotImplemented]
						[BiosFunction( 0xEEDA2E54, "sceDisplayGetFrameBuf" )] [Stateless]
						// int sceDisplayGetFrameBuf(void **topaddr, int *bufferwidth, int *pixelformat, int *unk1); (/display/pspdisplay.h:84)
						int sceDisplayGetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int unk1 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9C6EAAD7, "sceDisplayGetVcount" )] [Stateless]
						// unsigned int sceDisplayGetVcount(); (/display/pspdisplay.h:89)
						int sceDisplayGetVcount(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x36CDFADE, "sceDisplayWaitVblank" )] [Stateless]
						// int sceDisplayWaitVblank(); (/display/pspdisplay.h:104)
						int sceDisplayWaitVblank(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8EB9EC49, "sceDisplayWaitVblankCB" )] [Stateless]
						// int sceDisplayWaitVblankCB(); (/display/pspdisplay.h:109)
						int sceDisplayWaitVblankCB(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x984C27E7, "sceDisplayWaitVblankStart" )] [Stateless]
						// int sceDisplayWaitVblankStart(); (/display/pspdisplay.h:94)
						int sceDisplayWaitVblankStart(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x46F186C3, "sceDisplayWaitVblankStartCB" )] [Stateless]
						// int sceDisplayWaitVblankStartCB(); (/display/pspdisplay.h:99)
						int sceDisplayWaitVblankStartCB(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - CD31A16C */
