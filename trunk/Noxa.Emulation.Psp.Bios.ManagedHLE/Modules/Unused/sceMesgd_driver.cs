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
	class sceMesgd_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMesgd_driver";
			}
		}

		#endregion

		#region State Management

		public sceMesgd_driver( Kernel kernel )
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
		[BiosFunction( 0x102DC8AF, "sceMesgd_driver_102DC8AF" )]
		int sceMesgd_driver_102DC8AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xADD0CB66, "sceMesgd_driver_ADD0CB66" )]
		int sceMesgd_driver_ADD0CB66(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - A9E32274 */
