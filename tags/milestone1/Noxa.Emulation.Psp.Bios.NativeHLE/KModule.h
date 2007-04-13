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

				class Kernel;

				class KModule : public KHandle
				{
				public:
					Kernel*		Kernel;

					gcref<BiosModule^>	Module;

					char*		Name;

					uint		ModuleInfo;
					uint		ModuleBootStart;
					uint		ModuleRebootBefore;
					uint		ModuleStart;
					uint		ModuleStartThreadParam;
					uint		ModuleStop;
					uint		ModuleStopThreadParam;

				public:
					KModule( Bios::Kernel* kernel, BiosModule^ module );
					~KModule();
				};

			}
		}
	}
}
