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
	class sceOpenPSID : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceOpenPSID";
			}
		}

		#endregion

		#region State Management

		public sceOpenPSID( Kernel kernel )
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
		[BiosFunction( 0xC69BEBCE, "sceOpenPSIDGetOpenPSID" )]
		int sceOpenPSIDGetOpenPSID(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 23CB43E8 */
