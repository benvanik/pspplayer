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
	class sceWmd_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceWmd_driver";
			}
		}

		#endregion

		#region State Management

		public sceWmd_driver( Kernel kernel )
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
		[BiosFunction( 0x7A0E484C, "sceWmd_driver_7A0E484C" )]
		int sceWmd_driver_7A0E484C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7CE9041, "sceWmd_driver_B7CE9041" )]
		int sceWmd_driver_B7CE9041(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 8A84F7B9 */
