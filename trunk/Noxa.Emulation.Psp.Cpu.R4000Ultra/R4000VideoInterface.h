// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "MemoryPool.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;

				ref class R4000VideoInterface
				{
				protected:
					R4000Cpu^			_cpu;
					MemoryPool*			_pool;

				public:
					R4000VideoInterface( R4000Cpu^ cpu );
					~R4000VideoInterface();

					void Prepare();
				};

			}
		}
	}
}
