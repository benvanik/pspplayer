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

					public ref class LoadExecForUser : public Module
					{
					public:
						LoadExecForUser( Kernel^ kernel ) : Module( kernel ) {}
						~LoadExecForUser(){}

					public:
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
						// int sceKernelLoadExec(const char *file, struct SceKernelLoadExecParam *param); (/user/psploadexec.h:80)
						int sceKernelLoadExec( int file, int param ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x05572A5F, "sceKernelExitGame" )] [Stateless]
						// void sceKernelExitGame(); (/user/psploadexec.h:57)
						void sceKernelExitGame(){}

						[NotImplemented]
						[BiosFunction( 0x4AC57943, "sceKernelRegisterExitCallback" )] [Stateless]
						// int sceKernelRegisterExitCallback(int cbid); (/user/psploadexec.h:49)
						int sceKernelRegisterExitCallback( int cbid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 00A2C2A7 */
