// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	public static class VertexType
	{
		public const uint None = 0x0;

		public const uint TextureMask = 0x3;
		public const uint TextureFixed8 = 0x1;
		public const uint TextureFixed16 = 0x2;
		public const uint TextureFloat = 0x3;

		public const uint ColorMask = 0x7 << 2;
		public const uint ColorBGR5650 = 0x4 << 2;
		public const uint ColorABGR5551 = 0x5 << 2;
		public const uint ColorABGR4444 = 0x6 << 2;
		public const uint ColorABGR8888 = 0x7 << 2;

		public const uint NormalMask = 0x3 << 5;
		public const uint NormalFixed8 = 0x1 << 5;
		public const uint NormalFixed16 = 0x2 << 5;
		public const uint NormalFloat = 0x3 << 5;

		public const uint PositionMask = 0x3 << 7;
		public const uint PositionFixed8 = 0x1 << 7;
		public const uint PositionFixed16 = 0x2 << 7;
		public const uint PositionFloat = 0x3 << 7;

		public const uint WeightMask = 0x3 << 9;
		public const uint WeightFixed8 = 0x1 << 9;
		public const uint WeightFixed16 = 0x2 << 9;
		public const uint WeightFloat = 0x3 << 9;

		public const uint IndexMask = 0x2 << 11;
		public const uint Index8 = 0x1 << 11;
		public const uint Index16 = 0x2 << 11;

		public const uint WeightCountMask = 0x3 << 14;		// skinning weight count
		public const uint MorphCountMask = 0x3 << 18;		// morphing vertex count

		public const uint TransformedMask = 0x1 << 23;		// 1 if raw
	}

	enum TexturePixelStorage
	{
		BGR5650 = 0,
		ABGR5551 = 1,
		ABGR4444 = 2,
		ABGR8888 = 3,
		Indexed4 = 4,
		Indexed8 = 5,
		Indexed16 = 6,
		Indexed32 = 7,
		DXT1 = 8,
		DXT3 = 9,
		DXT5 = 10,
	}
}
