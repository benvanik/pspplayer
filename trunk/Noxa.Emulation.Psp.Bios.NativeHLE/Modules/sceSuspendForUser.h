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

					public ref class sceSuspendForUser : public Module
					{
					public:
						sceSuspendForUser( IntPtr kernel ) : Module( kernel ) {}
						~sceSuspendForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceSuspendForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						// Currently just ignore power switch locking - maybe one day
						// we should obey it so saves don't get corrupt/etc?

						[BiosFunction( 0xEADB1BD7, "sceKernelPowerLock" )] [Stateless]
						// manual add
						int sceKernelPowerLock( int type ){ return 0; }

						[BiosFunction( 0x3AEE7261, "sceKernelPowerUnlock" )] [Stateless]
						// manual add
						int sceKernelPowerUnlock( int type ){ return 0; }

						[BiosFunction( 0x090CCB3F, "sceKernelPowerTick" )] [Stateless]
						// int sceKernelPowerTick(int ticktype); (/include/kernelutils.h:167)
						int sceKernelPowerTick( int type ){ return 0; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - FA33C104 */
