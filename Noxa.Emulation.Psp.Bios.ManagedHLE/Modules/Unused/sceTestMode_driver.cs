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
	class sceTestMode_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceTestMode_driver";
			}
		}

		#endregion

		#region State Management

		public sceTestMode_driver( Kernel kernel )
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
		[BiosFunction( 0x2342B106, "sceTestModeStartTestMode" )]
		int sceTestModeStartTestMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB347D587, "sceTestMode_driver_B347D587" )]
		int sceTestMode_driver_B347D587(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 42C0E8D7 */
