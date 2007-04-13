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

				class KIntHandler : public KHandle
				{
				public:
					Kernel*		Kernel;

					int			InterruptNumber;
					int			Slot;

					uint		Address;
					uint		Argument;

					uint		CallCount;

					// Timing info?
					// total clock (int64), min clock (int64), max clock (int64)?

				private:
					bool		_isInstalled;

				public:
					KIntHandler( Bios::Kernel* kernel, int interruptNumber, int slot, uint address, uint argument );
					~KIntHandler();

					bool IsEnabled(){ return _isInstalled; }
					void SetEnabled( bool enabled );
				};

			}
		}
	}
}
