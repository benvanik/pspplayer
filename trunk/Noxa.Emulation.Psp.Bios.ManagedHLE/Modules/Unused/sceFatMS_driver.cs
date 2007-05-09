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
	class sceFatMS_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceFatMS_driver";
			}
		}

		#endregion

		#region State Management

		public sceFatMS_driver( Kernel kernel )
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
		[BiosFunction( 0xBCB9DEDB, "sceFatMS_driver_BCB9DEDB" )]
		int sceFatMS_driver_BCB9DEDB(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 536717B8 */
