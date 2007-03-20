// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace CodeGen {

				typedef struct Label_t
				{
					int			Offset;
				} Label;

				enum ReferenceType
				{
					RefRelative,
					RefCall,
					RefAbsoluteImmediate,
					RefAbsoluteDisplacement,
				};

				typedef struct Reference_t
				{
					Label*				Label;
					int					Offset;
					int					Length;
					enum ReferenceType	Type;

					byte*				CodePointer;
				} Reference;

			}
		}
	}
}
