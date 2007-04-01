// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Kernel.h"
#include "KernelPartition.h"
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

				ref class KernelPool : public KernelHandle
				{
				public:
					Bios::Kernel^		Kernel;
					int					ID;
					String^				Name;
					int					Attributes;
					uint				BlockSize;
					bool				IsFixed;

					KernelPartition^	Partition;

					List<KernelMemoryBlock^>^	Blocks;
					List<KernelMemoryBlock^>^	FreeList;
					List<KernelMemoryBlock^>^	UsedList;

				public:
					KernelPool( Bios::Kernel^ kernel, int id, String^ name, KernelPartition^ partition, int attributes, uint blockSize, int blockCount );
					void Cleanup();

					KernelMemoryBlock^ Allocate();
					void Free( int address );
					void Free( KernelMemoryBlock^ block );
				};

			}
		}
	}
}
