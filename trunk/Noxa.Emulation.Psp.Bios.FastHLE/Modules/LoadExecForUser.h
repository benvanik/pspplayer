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

					ref class LoadExecForUser : public Module
					{
					public:
						LoadExecForUser( Kernel^ kernel ) : Module( kernel ) {}
						~LoadExecForUser(){}

						property String^ Name { virtual String^ get() override { return "LoadExecForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xBD2F1094, "sceKernelLoadExec" )] [Stateless]
						// /user/psploadexec.h:80: int sceKernelLoadExec(const char *file, struct SceKernelLoadExecParam *param);
						int sceKernelLoadExec( int file, int param ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x05572A5F, "sceKernelExitGame" )] [Stateless]
						// /user/psploadexec.h:57: void sceKernelExitGame();
						void sceKernelExitGame(){}

						[NotImplemented]
						[BiosFunction( 0x4AC57943, "sceKernelRegisterExitCallback" )] [Stateless]
						// /user/psploadexec.h:49: int sceKernelRegisterExitCallback(int cbid);
						int sceKernelRegisterExitCallback( int cbid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B5CB02F4 */
