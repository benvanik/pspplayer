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
	class LflashFatfmt : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "LflashFatfmt";
			}
		}

		#endregion

		#region State Management

		public LflashFatfmt( Kernel kernel )
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
		[BiosFunction( 0xB7A424A4, "sceLflashFatfmtStartFatfmt" )]
		int sceLflashFatfmtStartFatfmt(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 331F5E24 */
