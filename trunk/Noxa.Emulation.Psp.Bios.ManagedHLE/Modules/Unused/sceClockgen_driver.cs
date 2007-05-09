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
	class sceClockgen_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceClockgen_driver";
			}
		}

		#endregion

		#region State Management

		public sceClockgen_driver( Kernel kernel )
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
		[BiosFunction( 0x29160F5D, "sceClockgenInit" )]
		int sceClockgenInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36F9B49D, "sceClockgenEnd" )]
		int sceClockgenEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32C8CA86, "sceClockgenGetRevision" )]
		int sceClockgenGetRevision(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9716D48, "sceClockgenGetRegValue" )]
		int sceClockgenGetRegValue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4EB657D5, "sceClockgen_driver_4EB657D5" )]
		int sceClockgen_driver_4EB657D5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F8328FD, "sceClockgen_driver_5F8328FD" )]
		int sceClockgen_driver_5F8328FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9FE99D1, "sceClockgen_driver_B9FE99D1" )]
		int sceClockgen_driver_B9FE99D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7FF82F6F, "sceClockgen_driver_7FF82F6F" )]
		int sceClockgen_driver_7FF82F6F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBE5F283, "sceClockgen_driver_DBE5F283" )]
		int sceClockgen_driver_DBE5F283(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9AF3102, "sceClockgen_driver_C9AF3102" )]
		int sceClockgen_driver_C9AF3102(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9C3095F3 */
