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
	class sceChkreg_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceChkreg_driver";
			}
		}

		#endregion

		#region State Management

		public sceChkreg_driver( Kernel kernel )
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
		[BiosFunction( 0x54495B19, "sceChkreg_driver_54495B19" )]
		int sceChkreg_driver_54495B19(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4B0A979A */
