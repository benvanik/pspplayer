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
using namespace Noxa::Emulation::Psp::Media;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class KernelDevice abstract
				{
				public:
					String^				Name;
					array<String^>^		Paths;
					bool				IsSeekable;
					bool				ReadOnly;

					KernelDevice( String^ name, array<String^>^ paths, bool isSeekable, bool readOnly )
					{
						Name = name;
						Paths = paths;
						IsSeekable = isSeekable;
						ReadOnly = readOnly;
					}
				};

				ref class KernelBlockDevice : public KernelDevice
				{
				public:
					int					BlockSize;

					KernelBlockDevice( String^ name, array<String^>^ paths, bool isSeekable, bool readOnly, int blockSize )
						: KernelDevice( name, paths, isSeekable, readOnly )
					{
						BlockSize = blockSize;
					}
				};

				ref class KernelFileDevice : public KernelDevice
				{
				public:
					IMediaDevice^		MediaDevice;
					IMediaFolder^		MediaRoot;

					KernelFileDevice( String^ name, array<String^>^ paths, bool isSeekable, bool readOnly, IMediaDevice^ mediaDevice, IMediaFolder^ mediaRoot )
						: KernelDevice( name, paths, isSeekable, readOnly )
					{
						MediaDevice = mediaDevice;
						MediaRoot = mediaRoot;
					}
				};

			}
		}
	}
}
