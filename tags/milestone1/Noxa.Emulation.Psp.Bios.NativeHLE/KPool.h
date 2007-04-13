// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "LL.h"
#include "KHandle.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class KPartition;
				class KMemoryBlock;

				class KPool : public KHandle
				{
				public:
					char*				Name;
					uint				Attributes;
					uint				BlockSize;

					KPartition*			Partition;

					LL<KMemoryBlock*>*	Blocks;
					LL<KMemoryBlock*>*	UsedBlocks;
					LL<KMemoryBlock*>*	FreeBlocks;

				public:
					KPool( KPartition* partition, char* name, uint attributes, uint blockSize );
					~KPool();

					KMemoryBlock* Allocate();
					void Free( int address );
					void Free( KMemoryBlock* block );

				protected:
					virtual bool AllocateBlocks(){ return false; }
				};

				class KFixedPool : public KPool
				{
				public:
					KFixedPool( KPartition* partition, char* name, uint attributes, uint blockSize, int blockCount );
				};

				class KVariablePool : public KPool
				{
				public:
					KVariablePool( KPartition* partition, char* name, uint attributes, uint blockSize );

					virtual bool AllocateBlocks();
				};

			}
		}
	}
}
