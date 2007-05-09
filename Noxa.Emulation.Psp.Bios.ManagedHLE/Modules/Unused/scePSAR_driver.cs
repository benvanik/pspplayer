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
	class scePSAR_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePSAR_driver";
			}
		}

		#endregion

		#region State Management

		public scePSAR_driver( Kernel kernel )
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
		[BiosFunction( 0x294BEB21, "_scePSARModuleStart" )]
		int _scePSARModuleStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4DB5ABE, "scePSAR_driver_F4DB5ABE" )]
		int scePSAR_driver_F4DB5ABE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F7C1EDC, "scePSAR_driver_3F7C1EDC" )]
		int scePSAR_driver_3F7C1EDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE564B1DA, "scePSAR_driver_E564B1DA" )]
		int scePSAR_driver_E564B1DA(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 53BAF980 */
