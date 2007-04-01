// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "Vector.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class HandleTable;

				class Kernel
				{
				public:
					HandleTable*		Handles;
					KThread*			Threads;
					KPartition*			Partitions;

					KThread*			MainThread;
					KThread*			ActiveThread;

					KIntHandler**		InterruptHandlers;

					KFileHandle*		StdIn;
					KFileHandle*		StdOut;
					KFileHandle*		StdErr;

				public:
					Kernel();
					~Kernel();



					bool Schedule();
					void Execute();

					__inline KHandle* FindHandle( const int id );
					__inline KDevice* FindDevice( const char* path );

					// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
					int64 GetClockTime();
					// Time in microseconds since the game started.
					int64 GetRunTime();

				private:
					void CreateStdio();
				};

			}
		}
	}
}
