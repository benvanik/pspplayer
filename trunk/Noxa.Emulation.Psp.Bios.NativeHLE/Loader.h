// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NativeBios.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class Loader : public ILoader
				{
				internal:
					NativeBios^				_bios;

				public:
					Loader( NativeBios^ bios );
					~Loader();

					property IBios^ Bios
					{
						virtual IBios^ get()
						{
							return _bios;
						}
					}

					virtual LoadResults^ LoadModule( ModuleType type, Stream^ moduleStream, LoadParameters^ parameters );
				};

			}
		}
	}
}
