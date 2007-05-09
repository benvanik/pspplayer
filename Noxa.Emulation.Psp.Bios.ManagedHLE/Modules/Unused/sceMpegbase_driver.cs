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
	class sceMpegbase_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMpegbase_driver";
			}
		}

		#endregion

		#region State Management

		public sceMpegbase_driver( Kernel kernel )
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
		[BiosFunction( 0x27A2982F, "sceMpegBaseInit" )]
		int sceMpegBaseInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBEA18F91, "sceMpegbase_driver_BEA18F91" )]
		int sceMpegbase_driver_BEA18F91(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x492B5E4B, "sceMpegBaseCscInit" )]
		int sceMpegBaseCscInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC9E717E, "sceMpegbase_driver_AC9E717E" )]
		int sceMpegbase_driver_AC9E717E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0530BE4E, "sceMpegbase_driver_0530BE4E" )]
		int sceMpegbase_driver_0530BE4E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91929A21, "sceMpegBaseCscAvc" )]
		int sceMpegBaseCscAvc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE8EB837, "sceMpegBaseCscVme" )]
		int sceMpegBaseCscVme(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AC0321A, "sceMpegBaseYCrCbCopy" )]
		int sceMpegBaseYCrCbCopy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE45C284, "sceMpegBaseYCrCbCopyVme" )]
		int sceMpegBaseYCrCbCopyVme(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 05912F99 */
