// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000BlockBuilder.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000AdvancedBlockBuilder : public R4000BlockBuilder
				{
				protected:
					virtual int InternalBuild( int startAddress, CodeBlock^ block ) override;

					void GeneratePreamble();
					void GenerateTail( int address, bool tailJump, int targetAddress );

				public:
					R4000AdvancedBlockBuilder( R4000Cpu^ cpu, R4000Core^ core );
					~R4000AdvancedBlockBuilder();
				};

			}
		}
	}
}
