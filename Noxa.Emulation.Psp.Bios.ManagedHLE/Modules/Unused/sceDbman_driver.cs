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
	class sceDbman_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDbman_driver";
			}
		}

		#endregion

		#region State Management

		public sceDbman_driver( Kernel kernel )
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
		[BiosFunction( 0xB2B8C3F9, "sceDbmanSelect" )]
		int sceDbmanSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34B53D46, "sceDbman_driver_34B53D46" )]
		int sceDbman_driver_34B53D46(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - E35D4331 */
