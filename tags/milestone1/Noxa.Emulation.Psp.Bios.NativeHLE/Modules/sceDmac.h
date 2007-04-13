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

					public ref class sceDmac : public Module
					{
					public:
						sceDmac( IntPtr kernel ) : Module( kernel ) {}
						~sceDmac(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceDmac"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[BiosFunction( 0x617F3FE6, "sceDmacMemcpy" )] [Stateless]
						// manual add
						int sceDmacMemcpy( IMemory^ memory, int dest, int source, int size );

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 0E7D4FF8 */
