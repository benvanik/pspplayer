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
using namespace Noxa::Emulation::Psp::Debugging::Protocol;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;

				ref class R4000Controller : public IDebugController, MarshalByRefObject
				{
				public:
					R4000Cpu^			Cpu;

				public:
					R4000Controller( R4000Cpu^ cpu );
					~R4000Controller();

					virtual void Run();
					virtual void RunUntil( uint address );
					virtual void Break();

					virtual void SetNext( uint address );

					virtual void Step();
					virtual void StepOver();
					virtual void StepOut();
				};

			}
		}
	}
}
