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
		public int TextureFilterMin = Gl.GL_LINEAR;
		public int TextureFilterMag = Gl.GL_LINEAR;
		public int TextureWrapS = Gl.GL_REPEAT;
		public int TextureWrapT = Gl.GL_REPEAT;
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
		public MGLLight[] Lights = new MGLLight[] { new MGLLight(), new MGLLight(), new MGLLight(), new MGLLight() };
		public MGLMaterial Material = new MGLMaterial();
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

	class MGLMaterial
	{
		public float[] Ambient = new float[ 3 ];
		public float[] Diffuse = new float[ 3 ];
		public float[] Specular = new float[ 3 ];
		public float Alpha = 1.0f;
		public float SpecularCoefficient = 1.0f;
	}
}
