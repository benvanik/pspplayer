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

					public ref class sceWlanDrv : public Module
					{
					public:
						sceWlanDrv( Kernel^ kernel ) : Module( kernel ) {}
						~sceWlanDrv(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceWlanDrv"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x93440B11, "sceWlanDevIsPowerOn" )] [Stateless]
						// int sceWlanDevIsPowerOn(); (/wlan/pspwlan.h:24)
						int sceWlanDevIsPowerOn(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD7763699, "sceWlanGetSwitchState" )] [Stateless]
						// int sceWlanGetSwitchState(); (/wlan/pspwlan.h:31)
						int sceWlanGetSwitchState(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0C622081, "sceWlanGetEtherAddr" )] [Stateless]
						// int sceWlanGetEtherAddr(char *etherAddr); (/wlan/pspwlan.h:39)
						int sceWlanGetEtherAddr( int etherAddr ){ return NISTUBRETURN; }

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

/* GenerateStubsV2: auto-generated - 2A7D2524 */
