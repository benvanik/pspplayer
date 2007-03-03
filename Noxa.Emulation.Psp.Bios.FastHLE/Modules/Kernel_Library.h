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

					ref class Kernel_Library : public Module
					{
					public:
						Kernel_Library( Kernel^ kernel ) : Module( kernel ) {}
						~Kernel_Library(){}

						property String^ Name { virtual String^ get() override { return "Kernel_Library"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x092968F4, "sceKernelCpuSuspendIntr" )] [Stateless]
						// unsigned int sceKernelCpuSuspendIntr(); (/user/pspintrman.h:77)
						int sceKernelCpuSuspendIntr(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5F10D406, "sceKernelCpuResumeIntr" )] [Stateless]
						// void sceKernelCpuResumeIntr(unsigned int flags); (/user/pspintrman.h:84)
						void sceKernelCpuResumeIntr( int flags ){}

						[NotImplemented]
						[BiosFunction( 0x3B84732D, "sceKernelCpuResumeIntrWithSync" )] [Stateless]
						// void sceKernelCpuResumeIntrWithSync(unsigned int flags); (/user/pspintrman.h:91)
						void sceKernelCpuResumeIntrWithSync( int flags ){}

						[NotImplemented]
						[BiosFunction( 0x47A0B729, "sceKernelIsCpuIntrSuspended" )] [Stateless]
						// int sceKernelIsCpuIntrSuspended(unsigned int flags); (/user/pspintrman.h:100)
						int sceKernelIsCpuIntrSuspended( int flags ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB55249D2, "sceKernelIsCpuIntrEnable" )] [Stateless]
						// int sceKernelIsCpuIntrEnable(); (/user/pspintrman.h:107)
						int sceKernelIsCpuIntrEnable(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 8764123F */
