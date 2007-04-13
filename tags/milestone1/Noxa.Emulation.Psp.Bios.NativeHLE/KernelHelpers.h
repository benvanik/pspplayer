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
using namespace Noxa::Emulation::Psp::Media;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				
				class Kernel;

				namespace KernelHelpers
				{
					// mallocs the copy - make sure to free!
					char* ToNativeString( String^ string );

					IMediaItem^ FindPath( Kernel* kernel, String^ path );

					String^ ReadString( IMemory^ memory, const int address );
					String^ ReadString( MemorySystem* memory, const int address );
					int WriteString( IMemory^ memory, const int address, String^ value );
					int WriteString( MemorySystem* memory, const int address, String^ value );

					int ReadString( MemorySystem* memory, const int address, byte* buffer, const int bufferSize );
					void WriteString( MemorySystem* memory, const int address, const char* buffer );

					DateTime ReadTime( IMemory^ memory, const int address );
					int WriteTime( IMemory^ memory, const int address, DateTime time );

					// Operations on UTC FILETIME's (100-nanosecond intervals since January 1, 1601)
					int64 ReadTime( MemorySystem* memory, const int address );
					int WriteTime( MemorySystem* memory, const int address, int64 time );

				};
			}
		}
	}
}
