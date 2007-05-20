// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "OglTextures.h"
#pragma unmanaged
#include "LRU.h"
#pragma managed

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				#define CLUTSIZE	65536

				typedef struct OglContext_t
				{
					// Memory from the CPU
					NativeMemorySystem*	Memory;

					// FrameBuffer
					uint			FrameBufferPointer;
					uint			FrameBufferWidth;

					// Misc state
					int				Scissor[ 4 ];

					// Matrices
					//float			ProjectionMatrix[ 16 ];
					float			ViewMatrix[ 16 ];
					float			WorldMatrix[ 16 ];
					float			TextureMatrix[ 16 ];

					// Textures / colors
					bool			TexturesEnabled;
					bool			TexturesSwizzled;
					int				MipMapLevel;
					int				TextureStorageMode;
					OglTexture		Textures[ 8 ];
					ushort			TextureFilterMin;
					ushort			TextureFilterMag;
					ushort			TextureWrapS;
					ushort			TextureWrapT;
					int				TextureEnvMode;
					float			TextureOffset[ 2 ];
					float			TextureScale[ 2 ];
					
					LRU<TextureEntry*>*	TextureCache;

					void*			ClutTable;		// Allocated to CLUTSIZE and pallettes are copied in
					uint			ClutPointer;
					uint			ClutChecksum;
					int				ClutFormat;
					int				ClutShift;
					int				ClutMask;
					int				ClutStart;

					uint			SourceFix;
					uint			DestFix;

					// Lighting / materials
					bool			LightingEnabled;

					float			AmbientMaterial[ 4 ];

					// Patches
					int				PatchFrontFace;
					int				PatchDivS;
					int				PatchDivT;

					// Texture transmission (sceGuCopyImage...)
					struct
					{
						int			SourceAddress;
						int			SourceLineWidth;
						int			DestinationAddress;
						int			DestinationLineWidth;

						int			PixelSize;	// 0 = 16 bit, 1 = 32 bit
						int			Width;
						int			Height;

						int			SX, SY;
						int			DX, DY;
					} TextureTx;

					// Stuff
				} OglContext;

				enum VertexType
				{
					VTNone				= 0x0,

					VTTextureMask		= 0x3,
					VTTextureFixed8		= 0x1,
					VTTextureFixed16	= 0x2,
					VTTextureFloat		= 0x3,

					VTColorMask			= 0x7 << 2,
					VTColorBGR5650		= 0x4 << 2,
					VTColorABGR5551		= 0x5 << 2,
					VTColorABGR4444		= 0x6 << 2,
					VTColorABGR8888		= 0x7 << 2,

					VTNormalMask		= 0x3 << 5,
					VTNormalFixed8		= 0x1 << 5,
					VTNormalFixed16		= 0x2 << 5,
					VTNormalFloat		= 0x3 << 5,

					VTPositionMask		= 0x3 << 7,
					VTPositionFixed8	= 0x1 << 7,
					VTPositionFixed16	= 0x2 << 7,
					VTPositionFloat		= 0x3 << 7,

					VTWeightMask		= 0x3 << 9,
					VTWeightFixed8		= 0x1 << 9,
					VTWeightFixed16		= 0x2 << 9,
					VTWeightFloat		= 0x3 << 9,

					VTIndexMask			= 0x2 << 11,
					VTIndex8			= 0x1 << 11,
					VTIndex16			= 0x2 << 11,

					VTWeightCountMask	= 0x3 << 14,	// skinning weight count
					VTMorphCountMask	= 0x3 << 18,	// morphing vertex count

					VTTransformedMask	= 0x1 << 23,	// 1 if raw
				};

				enum TexturePixelStorage
				{
					TPSBGR5650			= 0,
					TPSABGR5551			= 1,
					TPSABGR4444			= 2,
					TPSABGR8888			= 3,
					TPSIndexed4			= 4,
					TPSIndexed8			= 5,
					TPSIndexed16		= 6,
					TPSIndexed32		= 7,
					TPSDXT1				= 8,
					TPSDXT3				= 9,
					TPSDXT5				= 10,
				};

			}
		}
	}
}
