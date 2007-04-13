// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				enum class KernelHandleType
				{
					Thread,
					Stdio,
					File,
					MemoryBlock,
					Pool,
					Callback,
					GeCallback,
					Semaphore,
					Event,
					InterruptHandler,
				};

				ref class KernelHandle
				{
				public:
					KernelHandleType		HandleType;
					int						ID;

				public:
					KernelHandle( KernelHandleType handleType, int id )
					{
						HandleType = handleType;
						ID = id;
					}

				};

			}
		}
	}
}
