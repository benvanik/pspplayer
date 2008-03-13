// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe class MGLContext
	{
		public const int ClutSize = 65536;

		// The values of all the commands
		public uint[] Values = new uint[ 256 ];

		// Frame buffer
		public uint FrameBufferPointer;
		public uint FrameBufferWidth;

		// Clipping/depth/fog
		public float NearZ;
		public float FarZ;
		public int[] Scissor = new int[ 4 ];
		public float FogEnd;
		public float FogDepth;

		// Matrices
		public float[] ProjectionMatrix = new float[ 16 ];
		public float[] ViewMatrix = new float[ 16 ];
		public float[] WorldMatrix = new float[ 16 ];
		public float[] TextureMatrix = new float[ 16 ];

		// Blending/etc
		public uint SourceFix;
		public uint DestFix;

		// Textures
		public bool TexturesEnabled;
		public bool TexturesSwizzled;
		public int MipMapLevel;
		public int TextureStorageMode;
		//public MGLTexture[] Textures = new MGLTexture[ 8 ];
		public ushort TextureFilterMin = Gl.GL_LINEAR;
		public ushort TextureFilterMag = Gl.GL_LINEAR;
		public ushort TextureWrapS = Gl.GL_REPEAT;
		public ushort TextureWrapT = Gl.GL_REPEAT;
		public int TextureEnvMode = Gl.GL_MODULATE;
		public float TextureOffsetS;
		public float TextureOffsetT;
		public float TextureScaleS = 1.0f;
		public float TextureScaleT = 1.0f;

		// CLUT
		public void* ClutTable;
		public uint ClutPointer;
		public uint ClutChecksum;
		public int ClutFormat;
		public int ClutShift;
		public int ClutMask;
		public int ClutStart;

		// Materials
		public bool LightingEnabled;
		public float[] AmbientMaterial = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };

		// Patches
		public int PatchFrontFace;
		public int PatchDivS;
		public int PatchDivT;

		// Texture transfer
		public MGLTextureTransfer TextureTx = new MGLTextureTransfer();
	}

	class MGLTextureTransfer
	{
		public uint SourceAddress;
		public uint SourceLineWidth;
		public uint DestinationAddress;
		public uint DestinationLineWidth;

		public int PixelSize; // 0 = 16 bit, 1 = 32 bit
		public int Width;
		public int Height;

		public int SX, SY;
		public int DX, DY;
	}
}
