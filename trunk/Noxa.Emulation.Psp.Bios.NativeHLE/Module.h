// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

#pragma warning( disable : 4677 )

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				public ref class Module abstract : public IModule
				{
				internal:
					Kernel*						_kernel;

				internal:
					Module( IntPtr kernel ){ _kernel = ( Kernel* )kernel.ToPointer(); }
					~Module(){ this->Clear(); }

				public:
					property String^ Name { virtual String^ get() = 0; }

					virtual void Start(){}
					virtual void Stop(){}
					virtual void Clear(){}

				internal:
					virtual void* QueryNativePointer( uint nid ){ return 0; }

				};

			}
		}
	}
}
