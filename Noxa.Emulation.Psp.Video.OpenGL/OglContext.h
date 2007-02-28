// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "OglTextures.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				typedef struct OglContext_t
				{
					// Memory from the CPU
					byte*			MemoryPointer;

					// FrameBuffer
					uint			FrameBufferPointer;
					uint			FrameBufferWidth;

					// Matrices
					float			ProjectionMatrix[ 16 ];
					float			ViewMatrix[ 16 ];
					float			WorldMatrix[ 16 ];
					float			TextureMatrix[ 16 ];

					// Textures
					bool			TexturesEnabled;
					bool			TexturesSwizzled;
					int				MipMapLevel;
					int				TextureStorageMode;
					OglTexture		Textures[ 8 ];

					// Stuff
				} OglContext;

			}
		}
	}
}
