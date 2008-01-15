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

				typedef struct TextureEntry_t
				{
					int				PixelStorage;
					int				Address;
					int				LineWidth;
					int				Width;
					int				Height;

					int				TextureID;

					uint			Checksum;
					uint			Cookie;
					uint			CookieOriginal;

					// If PixelStorage & 0x4, these are valid
					uint			ClutPointer;
					uint			ClutChecksum;

				} TextureEntry;

				typedef struct OglTexture_t
				{
					int				PixelStorage;
					int				Address;
					int				LineWidth;
					int				Width;
					int				Height;
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
				bool GenerateTexture( OglContext_t* context, OglTexture* texture, uint checksum );
				uint CalculateTextureChecksum( byte* address, int width, int height, int pixelStorage );

				uint Convert5650(ushort source);
				uint Convert5551(ushort source);
				uint Convert4444(ushort source);
			}
		}
	}
}
