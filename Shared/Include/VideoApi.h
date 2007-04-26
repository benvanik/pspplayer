// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "VideoCommands.h"
#include "VideoPacket.h"
#include "VideoDisplayList.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			class MemoryPool;

			namespace Video {
				namespace Native {

					typedef enum VideoSyncType_e
					{
						SYNC_LIST_DONE			= 0,
						SYNC_LIST_QUEUED		= 1,
						SYNC_LIST_DRAWING_DONE	= 2,
						SYNC_LIST_STALL_REACHED	= 3,
						SYNC_LIST_CANCEL_DONE	= 4,
					} VideoSyncType;

					typedef struct VideoApi_t
					{
						// Setup & tear-down
						void (*Setup)( NativeMemorySystem* memory, MemoryPool* pool );
						void (*Cleanup)();

						uint (*GetVcount)();

						// Switch framebuffer (used to doublebuffer, etc)
						void (*SwitchFrameBuffer)( int address, int bufferWidth, int pixelFormat, int syncMode );

						// Find an enqueued display list
						VideoDisplayList* (*FindList)( int listId );

						// Enqueue a new display list
						int (*EnqueueList)( VideoDisplayList* list, bool immediate );

						// Dequeue an existing list (abort)
						void (*DequeueList)( int listId );

						// Signal that a list has been updated
						void (*SignalUpdate)( int listId );

						// Sync a list
						void (*SyncList)( int listId, VideoSyncType syncType );

						// Sync the video system
						void (*Sync)( VideoSyncType syncType );

						void (*WaitForVsync)();

					} VideoApi;

				}
			}
		}
	}
}
