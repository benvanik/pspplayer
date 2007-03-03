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
				namespace KernelHelpers {

					__inline static String^ ReadString( IMemory^ memory, int address );
					__inline static int WriteString( IMemory^ memory, const int address, String^ value );

					__inline static String^ ReadString( byte* memory, const int address );
					__inline static int WriteString( byte* memory, const int address, String^ value );

					__inline static DateTime ReadTime( IMemory^ memory, const int address );
					__inline static int WriteTime( IMemory^ memory, const int address, DateTime time );

					__inline static DateTime ReadTime( byte* memory, const int address );
					__inline static int WriteTime( byte* memory, const int address, DateTime time );

				}
			}
		}
	}
}
