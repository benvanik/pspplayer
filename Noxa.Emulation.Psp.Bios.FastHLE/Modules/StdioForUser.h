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

					ref class StdioForUser : public Module
					{
					public:
						StdioForUser( Kernel^ kernel ) : Module( kernel ) {}
						~StdioForUser(){}

						property String^ Name { virtual String^ get() override { return "StdioForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x172D316E, "sceKernelStdin" )] [Stateless]
						// /user/pspstdio.h:35: SceUID sceKernelStdin();
						int sceKernelStdin(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA6BAB2E9, "sceKernelStdout" )] [Stateless]
						// /user/pspstdio.h:42: SceUID sceKernelStdout();
						int sceKernelStdout(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF78BA90A, "sceKernelStderr" )] [Stateless]
						// /user/pspstdio.h:49: SceUID sceKernelStderr();
						int sceKernelStderr(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B62EF295 */
