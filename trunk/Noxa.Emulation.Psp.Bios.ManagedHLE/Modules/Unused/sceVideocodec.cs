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
	class sceVideocodec : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVideocodec";
			}
		}

		#endregion

		#region State Management

		public sceVideocodec( Kernel kernel )
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
		[BiosFunction( 0x2D31F5B1, "sceVideocodecGetEDRAM" )]
		public int sceVideocodecGetEDRAM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F160BF4, "sceVideocodecReleaseEDRAM" )]
		public int sceVideocodecReleaseEDRAM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC01EC829, "sceVideocodecOpen" )]
		public int sceVideocodecOpen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17099F0A, "sceVideocodecInit" )]
		public int sceVideocodecInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBA273FA, "sceVideocodecDecode" )]
		public int sceVideocodecDecode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2F0564E, "sceVideocodecStop" )]
		public int sceVideocodecStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x307E6E1C, "sceVideocodecDelete" )]
		public int sceVideocodecDelete(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x745A7B7A, "sceVideocodecSetMemory" )]
		public int sceVideocodecSetMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2F385E7F, "sceVideocodecScanHeader" )]
		public int sceVideocodecScanHeader(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26927D19, "sceVideocodecGetVersion" )]
		public int sceVideocodecGetVersion(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - D298F71E */
