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

					ref class sceWlanDrv_lib : public Module
					{
					public:
						sceWlanDrv_lib( Kernel^ kernel ) : Module( kernel ) {}
						~sceWlanDrv_lib(){}

						property String^ Name { virtual String^ get() override { return "sceWlanDrv_lib"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x482CAE9A, "sceWlanDevAttach" )] [Stateless]
						// int sceWlanDevAttach(); (/wlan/pspwlan.h:46)
						int sceWlanDevAttach(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC9A8CAB7, "sceWlanDevDetach" )] [Stateless]
						// int sceWlanDevDetach(); (/wlan/pspwlan.h:53)
						int sceWlanDevDetach(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - C3C6478D */
