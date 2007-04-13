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

					typedef struct VideoApi_t
					{
						// Setup & tear-down
						void (*Setup)( MemorySystem* memory, MemoryPool* pool );
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

						// Sync a list
						void (*SyncList)( int listId );

						// Sync the video system
						void (*Sync)();

						void (*WaitForVsync)();

					} VideoApi;

				}
			}
		}
	}
}
