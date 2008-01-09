// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			class Tracer
			{
			private:
				static HANDLE _file;

			public:
				static void OpenFile( const char* fileName );
				static void CloseFile();

				static void WriteLine( const char* line );
				static void WriteBytes( const byte* buffer, int length );
				static void Flush();
			};

		}
	}
}
