// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				typedef struct OglContext_t
				{
					// Memory from the CPU
					byte*			MemoryPointer;
					int				MemoryBaseAddress;

					// FrameBuffer
					uint			FrameBufferPointer;
					uint			FrameBufferWidth;

					// Matrices
					float			ProjectionMatrix[ 16 ];
					float			ViewMatrix[ 12 ];
					float			WorldMatrix[ 12 ];
					float			TextureMatrix[ 12 ];

					// Textures
					bool			SwizzleTextures;
					int				MipMapLevel;
					int				TextureStorageMode;

					// Stuff
				} OglContext;

			}
		}
	}
}
