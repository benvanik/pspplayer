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

					public ref class sceSircs : public Module
					{
					public:
						sceSircs( IntPtr kernel ) : Module( kernel ) {}
						~sceSircs(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceSircs"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x71EEF62D, "sceSircsSend" )] [Stateless]
						// int sceSircsSend(struct sircs_data* sd, int count); (/sircs/pspsircs.h:37)
						int sceSircsSend( int sd, int count ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B1916DC3 */
