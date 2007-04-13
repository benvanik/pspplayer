// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "LL.h"
#include "KHandle.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				enum KEventAttributes
				{
					// Allow multiple threads to wait on the event
					KEventWaitMultiple		= 0x200,
				};

				class Kernel;
				class KThread;

				class KEvent : public KHandle
				{
				public:
					char*			Name;
					uint			Attributes;
					uint			InitialValue;
					uint			Value;

					LL<KThread*>*	WaitingThreads;

				public:
					KEvent( char* name, uint attributes, uint initialValue );
					~KEvent();

					__inline bool Matches( uint userValue, uint waitType );
					bool Signal( Kernel* kernel );
				};

			}
		}
	}
}
