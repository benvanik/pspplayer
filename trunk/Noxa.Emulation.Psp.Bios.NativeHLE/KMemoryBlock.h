// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
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

				// NOTE: most memory blocks will not have UIDs - only ones allocated via SysMemForUser
				
				class KMemoryBlock : public KHandle
				{
				public:
					KPartition*			Partition;
					char*				Name;

					uint				Address;
					uint				Size;
					uint				UpperBound;

					bool				IsFree;

				public:
					KMemoryBlock( KPartition* partition, uint address, uint size );
					~KMemoryBlock();
				};

			}
		}
	}
}
