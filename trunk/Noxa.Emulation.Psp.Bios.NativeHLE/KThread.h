// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "Vector.h"
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

				enum KThreadWaitTypes
				{
					KThreadWaitAnd			= 0x00,		// Wait for all bits to be set
					KThreadWaitOr			= 0x01,		// Wait for one or more bits to be set
					KThreadWaitClearAll		= 0x10,		// Clear the entire bitmask when it matches
					KThreadWaitClearPattern	= 0x20,		// Clear the just the matched pattern when it matches
				};

				enum KThreadWaits
				{
					KThreadWaitSleep,			// Wait until woken up
					KThreadWaitDelay,			// Wait until a specific time
					KThreadWaitSemaphore,		// Waiting on semaphore signal
					KThreadWaitEvent,			// Wait until an event matches
					KThreadWaitMbx,				// mailbox something
					KThreadWaitVpl,				// Wait until VPL has space
					KThreadWaitFpl,				// Wait until FPL has space
					KThreadWaitMpp,				// message pipe stuff
					KThreadWaitJoin,			// Wait until the target thread ends
					KThreadWaitEventHandler,	// message pump something
					KThreadWaitUnknown2,		// ?? callbacks?
				};

				enum KThreadStates
				{
					KThreadRunning		= 1,
					KThreadReady		= 2,
					KThreadWaiting		= 4,
					KThreadSuspended	= 8,
					KThreadStopped		= 16,
					KThreadDead			= 32,
				};

				enum KThreadAttributes
				{
					KThreadVFPU			= 0x00004000,	// Allow VFPU usage
					KThreadUser			= 0x80000000,	// Start thread in user mode
					KThreadUsbWlan		= 0xA0000000,	// Thread is part of USB/WLAN API
					KThreadVsh			= 0xC0000000,	// Thread is part of VSH API
					KThreadScratchSram	= 0x00008000,	// Allow scratchpad usage
					KThreadNoFillStack	= 0x00100000,	// Don't fill stack with 0xFF on create
					KThreadClearStack	= 0x00200000,	// Clear stack when thread deleted
				};

				class KPartition;
				class KMemoryBlock;
				class KEvent;
				class KCallback;

				typedef struct KThreadContext_t
				{
					int			ContextID;
				} KThreadContext;

				class KThread : public KHandle
				{
				public:
					Kernel*				Kernel;

					char*				Name;
					uint				EntryAddress;
					uint				GlobalPointer;
					int					InitialPriority;
					int					Priority;		// 0-32, lower = better
					KThreadAttributes	Attributes;

					uint				State;
					bool				Suspended;
					int					ExitCode;

					KPartition*			Partition;
					KMemoryBlock*		StackBlock;
					KMemoryBlock*		TLSBlock;

					LL<KThread*>*		ExitWaiters;		// Threads waiting for exit

					KThreadContext*		Context;
					int					ReturnValue;		// After woken, the return value for $v0 (or int::MinValue for invalid)

					int64				RunClocks;

					uint				WakeupCount;
					uint				ReleaseCount;
					uint				InterruptPreemptionCount;
					uint				ThreadPreemptionCount;

					bool				CanHandleCallbacks;

					// Wait stuff
					uint				WaitingOn;
					int64				WaitTimestamp;		// Time wait was set
					uint				WaitTimeout;		// 0 for infinite - either a timeout or an end time of KThreadWaitDelay - in ticks!
					KEvent*				WaitEvent;			// Event waiting on (if WaitingOn = KThreadWaitEvent)
					uint				WaitEventMode;		// Event wait mode (if WaitingOn = KThreadWaitEvent)
					KThread*			WaitThread;			// Thread waiting on (if WaitingOn = KThreadWaitJoin)
					uint				WaitArgument;
					uint				WaitAddress;		// For output parameters

				public:
					KThread( Bios::Kernel* kernel, KPartition* partition, char* name, uint entryAddress, int priority, KThreadAttributes attributes, uint stackSize );
					~KThread();

					void Start( uint argumentsLength, uint argumentsPointer );
					void Exit( int code );

					void ChangePriority( int newPriority );

					void Wake();
					void ReleaseWait();
					void Suspend();
					void Resume();
					void Sleep( bool canHandleCallbacks );
					void Delay( uint waitTimeUs, bool canHandleCallbacks );
					void Join( KThread* targetThread, uint timeoutUs, bool canHandleCallbacks );
					void Wait( KEvent* ev, uint waitEventMode, uint userValue, uint outAddress, uint timeoutUs, bool canHandleCallbacks );

					void AddToSchedule();
					void RemoveFromSchedule();
				};

			}
		}
	}
}
