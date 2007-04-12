// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "HashTable.h"
#include "KHandle.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class Kernel;

				class HandleTable
				{
				private:
					Kernel*			_kernel;
					HashTable		_hashTable;

				public:
					HandleTable( Kernel* kernel );
					~HandleTable();

					KHandle* Add( KHandle* handle );
					void Remove( int uid );
					void Remove( KHandle* handle );
					KHandle* Lookup( int uid );

				};

			}
		}
	}
}
