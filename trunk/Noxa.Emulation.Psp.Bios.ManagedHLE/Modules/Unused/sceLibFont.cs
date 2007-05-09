// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceLibFont : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLibFont";
			}
		}

		#endregion

		#region State Management

		public sceLibFont( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67F17ED7, "sceFontNewLib" )]
		public int sceFontNewLib(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x574B6FBC, "sceFontDoneLib" )]
		public int sceFontDoneLib(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x48293280, "sceFontSetResolution" )]
		public int sceFontSetResolution(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27F6E642, "sceFontGetNumFontList" )]
		public int sceFontGetNumFontList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC75D85B, "sceFontGetFontList" )]
		public int sceFontGetFontList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x099EF33C, "sceFontFindOptimumFont" )]
		public int sceFontFindOptimumFont(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x681E61A7, "sceFontFindFont" )]
		public int sceFontFindFont(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2F67356A, "sceFontCalcMemorySize" )]
		public int sceFontCalcMemorySize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5333322D, "sceFontGetFontInfoByIndexNumber" )]
		public int sceFontGetFontInfoByIndexNumber(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA834319D, "sceFontOpen" )]
		public int sceFontOpen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57FCB733, "sceFontOpenUserFile" )]
		public int sceFontOpenUserFile(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB8E7FE6, "sceFontOpenUserMemory" )]
		public int sceFontOpenUserMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AEA8CB6, "sceFontClose" )]
		public int sceFontClose(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DA7535E, "sceFontGetFontInfo" )]
		public int sceFontGetFontInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDCC80C2F, "sceFontGetCharInfo" )]
		public int sceFontGetCharInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C3E4A9E, "sceFontGetCharImageRect" )]
		public int sceFontGetCharImageRect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x980F4895, "sceFontGetCharGlyphImage" )]
		public int sceFontGetCharGlyphImage(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA1E6945, "sceFontGetCharGlyphImage_Clip" )]
		public int sceFontGetCharGlyphImage_Clip(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74B21701, "sceFontPixelToPointH" )]
		public int sceFontPixelToPointH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8F0752E, "sceFontPixelToPointV" )]
		public int sceFontPixelToPointV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x472694CD, "sceFontPointToPixelH" )]
		public int sceFontPointToPixelH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3C4B7E82, "sceFontPointToPixelV" )]
		public int sceFontPointToPixelV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE232411, "sceFontSetAltCharacterCode" )]
		public int sceFontSetAltCharacterCode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA3DE7B5, "sceFontGetShadowInfo" )]
		public int sceFontGetShadowInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x48B06520, "sceFontGetShadowImageRect" )]
		public int sceFontGetShadowImageRect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x568BE516, "sceFontGetShadowGlyphImage" )]
		public int sceFontGetShadowGlyphImage(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5DCF6858, "sceFontGetShadowGlyphImage_Clip" )]
		public int sceFontGetShadowGlyphImage_Clip(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 96A2667C */
