// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"
#include "KernelCallback.h"
#include "KernelDevice.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Media;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class KernelFileHandle : public KernelHandle
				{
				public:
					KernelFileDevice^		Device;
					IMediaItem^				MediaItem;

					int						FolderOffset;
					Stream^					Stream;

					bool					IsOpen;
					bool					CanWrite;
					bool					CanSeek;

					KernelCallback^			Callback;

				public:
					KernelFileHandle( int id )
						: KernelHandle( KernelHandleType::File, id ){}
				};

			}
		}
	}
}
