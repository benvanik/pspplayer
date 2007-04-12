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

				//typedef int (*SceKernelCallbackFunction)(int count, int arg, void *common);

				class KThread;

				class KCallback : public KHandle
				{
				public:
					char*			Name;
					KThread*		Thread;

					uint			Address;
					uint			CommonAddress;

					uint			NotifyCount;
					uint			NotifyArguments;

				public:
					KCallback( char* name, KThread* thread, uint address, uint commonAddress );
					~KCallback();
				};

			}
		}
	}
}
