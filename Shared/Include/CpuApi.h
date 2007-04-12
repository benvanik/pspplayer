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

					typedef void (*MarshalCompleteFunction)();

					typedef struct CpuApi_t
					{
						// -- Execution --

						// Execute the next block of code - returns the # of instructions executed
						// If breakFlags != 0, it is the value from BreakExecute()
						int (*Execute)( int* breakFlags );

						// Use to break execution of the current block and return to the kernel
						// flags is passed back from Execute
						void (*BreakExecute)( int flags );

						// -- Threading --

						// Allocate and return the ID of a new thread storage context
						int (*AllocateContextStorage)( int pc, int registers[ 32 ] );
						// Release an existing thread storage context
						void (*ReleaseContextStorage)( int tcsId );
						// Save the current context and switch to the given new one
						void (*SwitchContext)( int newTcsId );
						// Suspend the current context and run the given callback in the given context, resuming when complete
						void (*MarshalCallback)( int tcsId, int callbackAddress, int callbackArgs, MarshalCompleteFunction biosCallback );

						// -- Interrupts --

						// Get the interrupt mask
						int (*GetInterruptState)();
						// Set the interrupt mask, returning the previous value
						int (*SetInterruptState)( int newState );
						// Register an interrupt handler (in user code)
						void (*RegisterInterruptHandler)( int intNumber, int subNumber, int callbackAddress, int callbackArgs );
						// Unregister an interrupt handler
						void (*UnregisterInterruptHandler)( int intNumber, int subNumber );
						// Set an interrupt as pending
						void (*SetPendingInterrupt)( int intNumber );

					} CpuApi;

				}
			}
		}
	}
}
