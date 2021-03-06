// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			class MemoryPool;

			namespace Video {
				namespace Native {

					typedef struct CallbackHandlers_t
					{
						uint	SignalAddress;
						uint	SignalArg;
						uint	FinishAddress;
						uint	FinishArg;
					} CallbackHandlers;

					typedef enum VideoSyncType_e
					{
						SyncListDone		= 0,
						SyncListQueued		= 1,
						SyncListDrawn		= 2,
						SyncListStalled		= 3,
						SyncListCancelled	= 4,
					} VideoSyncType;

					typedef struct VideoApi_t
					{
						// Setup & tear-down
						void (*Setup)( NativeMemorySystem* memory, MemoryPool* pool );
						void (*Cleanup)();

						uint (*GetVcount)();

						// Switch framebuffer (used to doublebuffer, etc)
						void (*SwitchFrameBuffer)( int address, int bufferWidth, int pixelFormat, int syncMode );
						int (*GetFrameBuffer)();

						int (*AddCallbackHandlers)( CallbackHandlers* handlers );
						void (*RemoveCallbackHandlers)( int cbid );

						void (*SaveContext)( void* buffer );
						void (*RestoreContext)( void* buffer );

						int (*EnqueueList)( void* startAddress, void* stallAddress, int callbackId, bool immediate );
						void (*UpdateList)( int listId, void* stallAddress );
						void (*CancelList)( int listId );

						int (*SyncList)( int listId, int syncType );
						int (*Sync)( int syncType );

						void (*WaitForVsync)();

					} VideoApi;

				}
			}
		}
	}
}
