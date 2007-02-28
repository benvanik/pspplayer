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

				typedef struct OglTexture_t
				{
					int				PixelStorage;
					int				Address;
					int				LineWidth;
					int				Width;
					int				Height;
					int				TextureID;
				} OglTexture;

				struct OglContext_t;

				bool IsTextureValid( OglTexture* texture );
				bool GenerateTexture( OglContext_t* context, OglTexture* texture );

			}
		}
	}
}
