// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000GenContext.h"
#include "R4000Ctx.h"

#include "CodeGenerator.h"

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				// Tries to emit a VFPU instruction - if result is Invalid, the instruction was not matched
				GenerationResult TryEmitVfpu( R4000GenContext^ context, int pass, int address, uint code );

			}
		}
	}
}
