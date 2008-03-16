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
		public float[] ViewMatrix = new float[ 16 ] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f };
		public float[] WorldMatrix = new float[ 16 ]{ 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f };
		public float[] TextureMatrix = new float[ 16 ] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f };

		// Blending/etc
		public uint SourceFix;
		public uint DestFix;

		// Textures
		public bool TexturesEnabled;
		public bool TexturesSwizzled;
		public int MipMapLevel;
		public int TextureMinFilter;
		public MGLTextureInfo[] Textures = new MGLTextureInfo[ 8 ] { new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo(), new MGLTextureInfo() };
		public MGLTextureCache TextureCache;
		public float TextureOffsetS;
		public float TextureOffsetT;
		public float TextureScaleS = 1.0f;
		public float TextureScaleT = 1.0f;
		public MGLClut Clut;

		// Materials
		public bool LightingEnabled;
		public MGLLight[] Lights = new MGLLight[] { new MGLLight(), new MGLLight(), new MGLLight(), new MGLLight() };
		public float[] AmbientModelColor = new float[ 4 ];
	}

	class MGLTextureInfo
	{
		public TexturePixelStorage PixelStorage;
		public uint Address;
		public uint LineWidth;
		public uint Width;
		public uint Height;
	}

	enum LightType
	{
		Directional = 0,
		Point = 1,
		Spot = 2,
	}

	enum LightMode
	{
		Diffuse = 0,
		DiffuseSpecular = 1,
		PoweredDiffuseSpecular = 2,
	}

	class MGLLight
	{
		public bool Enabled = false;
		public LightType Type = LightType.Directional;
		public LightMode Mode = LightMode.Diffuse;
		public float[] Position = new float[ 3 ];
		public float[] Direction = new float[ 3 ];
		public float[] Attenuation = new float[ 3 ];
		public float Convergence = 1.0f;
		public float Cutoff = 1.0f;
		public float[] Color = new float[ 3 ];
	}
}
