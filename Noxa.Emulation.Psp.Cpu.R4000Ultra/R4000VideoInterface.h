// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

#include "MemoryPool.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				class R4000VideoInterface
				{
				protected:
					MemoryPool*			_pool;

				public:
					R4000VideoInterface();
					~R4000VideoInterface();
				};

			}
		}
	}
}
