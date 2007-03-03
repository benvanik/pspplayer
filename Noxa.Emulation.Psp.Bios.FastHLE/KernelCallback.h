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

				enum class KernelCallbackType
				{
					AsyncIO,
					Exit,
				};

				//typedef int (*SceKernelCallbackFunction)(int count, int arg, void *common);
				ref class KernelCallback : public KernelHandle
				{
				public:
					String^				Name;

					KernelThread^		Thread;

					int					FunctionAddress;
					int					CommonAddress;

					int					NotifyCount;
					int					NotifyArguments;

				public:
					KernelCallback( int id )
						: KernelHandle( KernelHandleType::Callback, id ){}
				};

			}
		}
	}
}
