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

					ref class sceSuspendForUser : public Module
					{
					public:
						sceSuspendForUser( Kernel^ kernel ) : Module( kernel ) {}
						~sceSuspendForUser(){}

						property String^ Name { virtual String^ get() override { return "sceSuspendForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xEADB1BD7, "sceKernelPowerLock" )] [Stateless]
						// manual add
						int sceKernelPowerLock( int type ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3AEE7261, "sceKernelPowerUnlock" )] [Stateless]
						// manual add
						int sceKernelPowerUnlock( int type ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x090CCB3F, "sceKernelPowerTick" )] [Stateless]
						// int sceKernelPowerTick(int ticktype); (/include/kernelutils.h:167)
						int sceKernelPowerTick( int type ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - FA33C104 */
