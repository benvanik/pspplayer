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
	enum VertexType
	{
		None = 0x0,

		TextureMask = 0x3,
		TextureFixed8 = 0x1,
		TextureFixed16 = 0x2,
		TextureFloat = 0x3,

		ColorMask = 0x7 << 2,
		ColorBGR5650 = 0x4 << 2,
		ColorABGR5551 = 0x5 << 2,
		ColorABGR4444 = 0x6 << 2,
		ColorABGR8888 = 0x7 << 2,

		NormalMask = 0x3 << 5,
		NormalFixed8 = 0x1 << 5,
		NormalFixed16 = 0x2 << 5,
		NormalFloat = 0x3 << 5,

		PositionMask = 0x3 << 7,
		PositionFixed8 = 0x1 << 7,
		PositionFixed16 = 0x2 << 7,
		PositionFloat = 0x3 << 7,

		WeightMask = 0x3 << 9,
		WeightFixed8 = 0x1 << 9,
		WeightFixed16 = 0x2 << 9,
		WeightFloat = 0x3 << 9,

		IndexMask = 0x2 << 11,
		Index8 = 0x1 << 11,
		Index16 = 0x2 << 11,

		WeightCountMask = 0x3 << 14,	// skinning weight count
		MorphCountMask = 0x3 << 18,		// morphing vertex count

		TransformedMask = 0x1 << 23,	// 1 if raw
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
