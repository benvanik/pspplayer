// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class KernelThread;

				ref class KernelInterruptHandler : public KernelHandle
				{
				public:
					int					InterruptCode;
					int					SubCode;
					int					InterruptLevel;

					bool				Enabled;

					// Note that the thread that it runs on doesn't matter
					KernelThread^		Thread;

					int					Argument;
					int					EntryAddress;
					//int					CommonAddress;
					//int					GlobalPointer;

					int					CallCount;

					// Timing info?
					// total clock (int64), min clock (int64), max clock (int64)?

				public:
					KernelInterruptHandler( int id )
						: KernelHandle( KernelHandleType::InterruptHandler, id ){}
				};

			}
		}
	}
}
