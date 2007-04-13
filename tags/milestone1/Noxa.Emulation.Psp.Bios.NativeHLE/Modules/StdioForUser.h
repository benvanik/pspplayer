// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"
#include "KFile.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					public ref class StdioForUser : public Module
					{
					public:
						StdioForUser( IntPtr kernel ) : Module( kernel ) {}
						~StdioForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "StdioForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

						[BiosFunction( 0x172D316E, "sceKernelStdin" )] [Stateless]
						// SceUID sceKernelStdin(); (/user/pspstdio.h:35)
						int sceKernelStdin(){ return _kernel->StdIn->UID; }

						[BiosFunction( 0xA6BAB2E9, "sceKernelStdout" )] [Stateless]
						// SceUID sceKernelStdout(); (/user/pspstdio.h:42)
						int sceKernelStdout(){ return _kernel->StdOut->UID; }

						[BiosFunction( 0xF78BA90A, "sceKernelStderr" )] [Stateless]
						// SceUID sceKernelStderr(); (/user/pspstdio.h:49)
						int sceKernelStderr(){ return _kernel->StdErr->UID; }

					public: // ------ Stubbed calls ------

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 5D298879 */
