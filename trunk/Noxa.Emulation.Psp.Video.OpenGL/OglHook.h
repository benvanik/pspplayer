// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::Hooks;
using namespace Noxa::Emulation::Psp::Debugging::Protocol;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				ref class OglDriver;

				ref class OglHook : public IVideoHook
				{
				public:
					OglDriver^			Driver;

				public:
					OglHook( OglDriver^ driver );
				};

			}
		}
	}
}
