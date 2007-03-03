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

					ref class sceUtility_netparam_internal : public Module
					{
					public:
						sceUtility_netparam_internal( Kernel^ kernel ) : Module( kernel ) {}
						~sceUtility_netparam_internal(){}

						property String^ Name { virtual String^ get() override { return "sceUtility_netparam_internal"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x072DEBF2, "sceUtilityCreateNetParam" )] [Stateless]
						// int sceUtilityCreateNetParam(int conf); (/utility/psputility_netparam.h:81)
						int sceUtilityCreateNetParam( int conf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9CE50172, "sceUtilityDeleteNetParam" )] [Stateless]
						// int sceUtilityDeleteNetParam(int conf); (/utility/psputility_netparam.h:111)
						int sceUtilityDeleteNetParam( int conf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFB0C4840, "sceUtilityCopyNetParam" )] [Stateless]
						// int sceUtilityCopyNetParam(int src, int dest); (/utility/psputility_netparam.h:102)
						int sceUtilityCopyNetParam( int src, int dest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFC4516F3, "sceUtilitySetNetParam" )] [Stateless]
						// int sceUtilitySetNetParam(int param, const void *val); (/utility/psputility_netparam.h:92)
						int sceUtilitySetNetParam( int param, int val ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 476DA472 */
