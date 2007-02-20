// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000GenContext.h"

using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000BiosStubs
				{
				public:
					bool EmitCall( R4000GenContext^ context, R4000Generator *g, int address, int nid );
				};

			}
		}
	}
}
