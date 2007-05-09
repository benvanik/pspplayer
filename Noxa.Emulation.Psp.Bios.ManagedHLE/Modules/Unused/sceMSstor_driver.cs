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
	class sceMSstor_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMSstor_driver";
			}
		}

		#endregion

		#region State Management

		public sceMSstor_driver( Kernel kernel )
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
		[BiosFunction( 0x6FC1E8AE, "sceMSstorEntry" )]
		int sceMSstorEntry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x714782D6, "sceMSstor_driver_714782D6" )]
		int sceMSstor_driver_714782D6(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 616844E6 */
