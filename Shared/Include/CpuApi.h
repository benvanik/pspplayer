// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {
				namespace Native {

					typedef void (*MarshalCompleteFunction)( int tcsId, int state );
					typedef void (*CpuResumeFunction)( bool timedOut, void* state );

					typedef struct CpuApi_t
					{
						// Get the interrupt mask
						int (*GetInterruptState)();
						// Set the interrupt mask, returning the previous value
						int (*SetInterruptState)( int newState );
						// Set an interrupt as pending
						void (*SetPendingInterrupt)( int intNumber );

						/*
						// Resume execution from a break and wait
						void Resume();
						// Break execution until a resume is called
						void BreakAndWait();
						// Break execution until a resume is called or timeout elapses
						void BreakAndWaitTimeout( int timeoutMs );
						// Break execution until a resume is called or timeout elapses
						// Callback is issued right before resuming
						void BreakAndWaitTimeoutCallback( int timeoutMs, CpuResumeFunction callback, void* state );
						*/
					} CpuApi;

				}
			}
		}
	}
}
