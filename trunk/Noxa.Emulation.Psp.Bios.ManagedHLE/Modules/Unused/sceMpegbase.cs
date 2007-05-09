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
	class sceMpegbase : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMpegbase";
			}
		}

		#endregion

		#region State Management

		public sceMpegbase( Kernel kernel )
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
		[BiosFunction( 0xBEA18F91, "sceMpegbase_BEA18F91" )]
		public int sceMpegbase_BEA18F91(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x492B5E4B, "sceMpegBaseCscInit" )]
		public int sceMpegBaseCscInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC9E717E, "sceMpegbase_AC9E717E" )]
		public int sceMpegbase_AC9E717E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0530BE4E, "sceMpegbase_0530BE4E" )]
		public int sceMpegbase_0530BE4E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91929A21, "sceMpegBaseCscAvc" )]
		public int sceMpegBaseCscAvc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE8EB837, "sceMpegBaseCscVme" )]
		public int sceMpegBaseCscVme(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AC0321A, "sceMpegBaseYCrCbCopy" )]
		public int sceMpegBaseYCrCbCopy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE45C284, "sceMpegBaseYCrCbCopyVme" )]
		public int sceMpegBaseYCrCbCopyVme(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 7E65DDA5 */
