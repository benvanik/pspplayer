// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class KernelStatistics
				{
				public:
					uint			LeaveIdleCount;
					uint			ThreadSwitchCount;
					uint			VfpuSwitchCount;

				internal:
					void GatherStats();
				};

			}
		}
	}
}
