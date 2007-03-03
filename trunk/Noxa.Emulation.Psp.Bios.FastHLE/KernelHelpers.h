// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				
				ref class KernelHelpers
				{
				public:

					static String^ ReadString( IMemory^ memory, int address );
					static int WriteString( IMemory^ memory, const int address, String^ value );

					static String^ ReadString( byte* memory, const int address );
					static int WriteString( byte* memory, const int address, String^ value );

					static DateTime ReadTime( IMemory^ memory, const int address );
					static int WriteTime( IMemory^ memory, const int address, DateTime time );

					static DateTime ReadTime( byte* memory, const int address );
					static int WriteTime( byte* memory, const int address, DateTime time );

				};
			}
		}
	}
}
