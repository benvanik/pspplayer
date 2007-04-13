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

				typedef struct TextureFormat_t
				{
					uint		Format;
					uint		Size;
					void		(*Copy)( const TextureFormat_t* format, void* dest, const void* source, const uint width );
					uint		Flags;
					uint		GLFormat;
				} TextureFormat;
				#define TFAlpha		1

				struct OglContext_t;

				bool IsTextureValid( OglTexture* texture );
				bool GenerateTexture( OglContext_t* context, OglTexture* texture );

			}
		}
	}
}
