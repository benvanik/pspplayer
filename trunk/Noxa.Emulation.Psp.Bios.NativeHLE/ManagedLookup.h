// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				void SetupManagedLookup();
				void ClearManagedLookup();

				generic<typename T> int AddObject( T object );
				generic<typename T> T LookupObject( int tag );
				generic<typename T> T RemoveObject( int tag );

			}
		}
	}
}
