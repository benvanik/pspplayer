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
	class sceMePower_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMePower_driver";
			}
		}

		#endregion

		#region State Management

		public sceMePower_driver( Kernel kernel )
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
		[BiosFunction( 0x55F11AF6, "sceMePower_driver_55F11AF6" )]
		int sceMePower_driver_55F11AF6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EBBBBA3, "sceMePower_driver_8EBBBBA3" )]
		int sceMePower_driver_8EBBBBA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6FE3E62, "sceMePower_driver_B6FE3E62" )]
		int sceMePower_driver_B6FE3E62(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05B2C420, "sceMePowerControlAvcPower" )]
		int sceMePowerControlAvcPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x579ACEA9, "sceMePowerSelectAvcClock" )]
		int sceMePowerSelectAvcClock(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 0275832D */
