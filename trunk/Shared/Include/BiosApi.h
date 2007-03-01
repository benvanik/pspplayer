// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			class MemoryPool;

			namespace Bios {
				namespace Native {

					typedef struct BiosApi_t
					{
						// Setup & tear-down
						void (*Setup)();
						void (*Cleanup)();

						// Querying/retreival
						void* (*QueryNID)( uint nid );

					} BiosApi;

				}
			}
		}
	}
}
