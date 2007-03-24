// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				enum class KernelThreadState
				{
					Running			= 0x01,
					Ready			= 0x02,
					Waiting			= 0x04,
					Suspended		= 0x08,
					Stopped			= 0x10,
					Killed			= 0x20,
				};

				ref class KernelThreadContext
				{
				public:
					Object^			CoreState;
					int				ProgramCounter;
				};

				enum class KernelThreadWait
				{
					ThreadEnd,
					Delay,
					Sleep,
					Event,
				};

				[Flags]
				enum class KernelThreadWaitTypes
				{
					And				= 0x00,		// Wait for all bits to be set
					Or				= 0x01,		// Wait for one or more bits to be set
					ClearAll		= 0x10,		// Clear the entire bitmask when it matches
					ClearPattern	= 0x20,		// Clear the just the matched pattern when it matches
				};

				[Flags]
				enum class KernelThreadAttributes : uint
				{
					/// <summary>
					/// Enable VFPU access for the thread.
					/// </summary>
					Vfpu			= 0x00004000,
					/// <summary>
					/// Start the thread in user mode (done automatically if the thread creating it is in user mode).
					/// </summary>
					User			= 0x80000000,
					/// <summary>
					/// Thread is part of the USB/WLAN API.
					/// </summary>
					UsbWlan			= 0xA0000000,
					/// <summary>
					/// Thread is part of the VSH API.
					/// </summary>
					Vsh				= 0xC0000000,
					/// <summary>
					/// Allow using scratchpad memory for a thread, NOT USABLE ON V1.0.
					/// </summary>
					ScratchSram		= 0x00008000,
					/// <summary>
					/// Disables filling the stack with 0xFF on creation.
					/// </summary>
					NoFillStack		= 0x00100000,
					/// <summary>
					/// Clear the stack when the thread is deleted.
					/// </summary>
					ClearStack		= 0x00200000,
				};

				ref class KernelCallback;
				ref class KernelEvent;
				ref class KernelMemoryBlock;
				ref class KernelPartition;

				ref class KernelThread : public KernelHandle
				{
				public:
					String^					Name;
					int						EntryAddress;
					int						Priority;				// Lower = better
					int						InitialPriority;
					KernelThreadAttributes	Attributes;

					int						ArgumentsLength;
					int						ArgumentsPointer;

					KernelThreadState		State;
					int						ExitCode;
					uint					StackSize;
					KernelMemoryBlock^		StackBlock;
					KernelThreadContext^	Context;

					int64					RunClocks;

					uint					WakeupCount;
					uint					ReleaseCount;
					uint					InterruptPreemptionCount;
					uint					ThreadPreemptionCount;

					bool					CanHandleCallbacks;
					List<KernelCallback^>^	Callbacks;
					
					KernelThreadWait		WaitClass;
					KernelThreadWaitTypes	WaitType;
					int						WaitID;
					int						WaitTimeout;
					KernelEvent^			WaitEvent;
					int						OutAddress;				// Helps with wait states that may need output
					int64					WaitTimestamp;			// Tick at which the wait was issued

				public:
					KernelThread( int id, String^ name, int entryAddress, int priority, uint stackSize, int attributes );

					void Start( KernelPartition^ partition, int argumentsLength, int argumentsPointer );
					void Exit( int code );

					void Wait( KernelEvent^ ev, KernelThreadWaitTypes waitType, int bitMask, int outAddress );
				};

			}
		}
	}
}
