// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "LL.h"
#include "KMemoryBlock.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				enum KAllocType
				{
					KAllocLow		= 0,	// Allocate at lowest address possible
					KAllocHigh		= 1,	// Allocate at highest address possible
					KAllocSpecific	= 2,	// Allocate at the specified address
				};

				class Kernel;

				class KPartition
				{
				public:
					Kernel*					Kernel;
					
					uint					BaseAddress;
					uint					Size;
					uint					Top;
					uint					Bottom;
					uint					FreeSize;

					LL<KMemoryBlock*>*		Blocks;
					LL<KMemoryBlock*>*		FreeList;

				public:
					KPartition( Bios::Kernel* kernel, uint baseAddress, uint size );
					~KPartition();

					KMemoryBlock* Allocate( KAllocType type, uint address, uint size );
					void Free( KMemoryBlock* block );

				private:
					void AddToFreeList( KMemoryBlock* block );
					KMemoryBlock* SplitBlock( KMemoryBlock* block, uint address, uint size );
				};

			}
		}
	}
}
