// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "VideoCommands.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {
				namespace Native {

					typedef struct VideoPacket_t
					{
						byte		Command;
						int			Argument;
					} VideoPacket;

					#define ARGF( argument ) ( float )( ( void* )( argument << 8 ) )

				}
			}
		}
	}
}
