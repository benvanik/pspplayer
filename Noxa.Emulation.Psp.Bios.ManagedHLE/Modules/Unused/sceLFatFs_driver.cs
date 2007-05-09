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
	class sceLFatFs_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLFatFs_driver";
			}
		}

		#endregion

		#region State Management

		public sceLFatFs_driver( Kernel kernel )
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
		[BiosFunction( 0x933F6E29, "sceLFatFs_driver_933F6E29" )]
		int sceLFatFs_driver_933F6E29(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F0560E0, "sceLFatFs_driver_8F0560E0" )]
		int sceLFatFs_driver_8F0560E0(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - DC846FF3 */
