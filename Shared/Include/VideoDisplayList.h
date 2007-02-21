// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "VideoCommands.h"
#include "VideoPacket.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {
				namespace Native {

					typedef struct VideoDisplayList_t
					{
						int				ID;

						VideoPacket*	Packets;
						int				PacketCount;
						int				PacketCapacity;

						bool			Ready;
						int				CallbackID;
						int				Argument;

						int				StallAddress;
						int				Base;
					} VideoDisplayList;

					typedef struct VdlRef_t
					{
						VideoDisplayList*	List;
						VdlRef_t*			Next;
					} VdlRef;

				}
			}
		}
	}
}
