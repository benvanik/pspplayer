// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Kernel.h"
#include "KernelMemoryBlock.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class Kernel;

				enum class KernelAllocationType
				{
					Low					= 0,	// Allocate at lowest address
					High				= 1,	// Allocate at highest address
					SpecificAddress		= 2,	// Allocate at specified address
				};

				ref class KernelPartition
				{
				public:
					Bios::Kernel^		Kernel;
					int					ID;
					uint				BaseAddress;
					uint				Size;
					uint				Top;
					uint				Bottom;
					uint				FreeSize;

					List<KernelMemoryBlock^>^	Blocks;
					List<KernelMemoryBlock^>^	FreeList;

				public:
					KernelPartition( Bios::Kernel^ kernel, int id, uint baseAddress, uint size );

					KernelMemoryBlock^ Allocate( KernelAllocationType type, uint address, uint size );
					void Free( KernelMemoryBlock^ block );

				private:
					KernelMemoryBlock^ SplitBlock( KernelMemoryBlock^ block, uint address, uint size );
					int BlockCompare( KernelMemoryBlock^ a, KernelMemoryBlock^ b );
				};

			}
		}
	}
}
