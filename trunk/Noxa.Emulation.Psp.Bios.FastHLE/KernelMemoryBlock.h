// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Kernel.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class KernelPartition;

				ref class KernelMemoryBlock : public KernelHandle
				{
				public:
					String^				Name;
					KernelPartition^	Partition;
					uint				Size;
					uint				Address;
					bool				IsFree;

				public:
					KernelMemoryBlock( int id )
						: KernelHandle( KernelHandleType::MemoryBlock, id ){}

					property uint UpperBound
					{
						uint get()
						{
							return Address + Size;
						}
					}

				};

			}
		}
	}
}
