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

				typedef struct DisplayList_t
				{
					int				ID;

					VideoPacket*	Packets;
					void*			StartAddress;
					void*			StallAddress;

					bool			Queued;
					bool			Done;
					bool			Stalled;
					bool			Drawn;
					bool			Cancelled;

					int				CallbackID;
					int				Argument;

					int				Base;
					//void*			ReturnAddress;
					void*			Stack[ 32 ];
					int				StackIndex;
				} DisplayList;

			}
		}
	}
}
