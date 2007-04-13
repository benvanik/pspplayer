// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"
#include "KernelThread.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				[Flags]
				enum class KernelEventAttributes
				{
					// Allow multiple threads to wait on the event
					WaitMultiple	= 0x200,
				};

				ref class KernelEvent : public KernelHandle
				{
				public:
					String^					Name;
					KernelEventAttributes	Attributes;
					int						InitialBitMask;
					int						BitMask;

					int						WaitingThreads;

				public:
					KernelEvent( int id )
						: KernelHandle( KernelHandleType::Event, id ){}

					bool Matches( int userMask, KernelThreadWaitTypes waitType )
					{
						bool matched = false;
						if( ( waitType & KernelThreadWaitTypes::And ) == KernelThreadWaitTypes::And )
						{
							// &
							return ( BitMask & userMask ) != 0;
						}
						else
						{
							// Must be |
							Debug::Assert( ( waitType & KernelThreadWaitTypes::Or ) == KernelThreadWaitTypes::Or );

							return ( BitMask | userMask ) != 0;
						}
					}
				};

			}
		}
	}
}
