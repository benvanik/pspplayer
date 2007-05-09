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
	class scePEQ_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePEQ_driver";
			}
		}

		#endregion

		#region State Management

		public scePEQ_driver( Kernel kernel )
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
		[BiosFunction( 0x213DE849, "scePEQ_driver_213DE849" )]
		int scePEQ_driver_213DE849(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC45514B, "scePEQ_driver_FC45514B" )]
		int scePEQ_driver_FC45514B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7EA0632, "scePEQ_driver_F7EA0632" )]
		int scePEQ_driver_F7EA0632(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED13C3B5, "scePEQ_driver_ED13C3B5" )]
		int scePEQ_driver_ED13C3B5(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - CAADA060 */
