// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class KSemaphore : public KHandle
				{
				public:
					char*			Name;
					uint			Attributes;

					int				InitialCount;
					int				CurrentCount;
					int				MaximumCount;

					LL<KThreadN*>*	WaitingThreads;

				public:
					KSemaphore( char* name, uint attributes, int initialCount, int maximumCount )
					{
						Name = strdup( name );
						Attributes = attributes;

						InitialCount = initialCount;
						CurrentCount = initialCount;
						MaximumCount = maximumCount;

						WaitingThreads = new LL<KThreadN*>();
					}

					~KSemaphore()
					{
						SAFEFREE( Name );
						SAFEDELETE( WaitingThreads );
					}
				}

			}
		}
	}
}
