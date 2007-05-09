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
	class sceResmap_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceResmap_driver";
			}
		}

		#endregion

		#region State Management

		public sceResmap_driver( Kernel kernel )
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
		[BiosFunction( 0xE5659590, "sceResmapPrepare" )]
		int sceResmapPrepare(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4434E59F, "sceResmap_driver_4434E59F" )]
		int sceResmap_driver_4434E59F(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 250DED50 */
