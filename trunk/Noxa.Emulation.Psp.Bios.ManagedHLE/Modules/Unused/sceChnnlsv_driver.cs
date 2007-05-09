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
	class sceChnnlsv_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceChnnlsv_driver";
			}
		}

		#endregion

		#region State Management

		public sceChnnlsv_driver( Kernel kernel )
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
		[BiosFunction( 0xE7833020, "sceChnnlsv_driver_E7833020" )]
		int sceChnnlsv_driver_E7833020(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF21A1FCA, "sceChnnlsv_driver_F21A1FCA" )]
		int sceChnnlsv_driver_F21A1FCA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4C494F8, "sceChnnlsv_driver_C4C494F8" )]
		int sceChnnlsv_driver_C4C494F8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABFDFC8B, "sceChnnlsv_driver_ABFDFC8B" )]
		int sceChnnlsv_driver_ABFDFC8B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x850A7FA1, "sceChnnlsv_driver_850A7FA1" )]
		int sceChnnlsv_driver_850A7FA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21BE78B4, "sceChnnlsv_driver_21BE78B4" )]
		int sceChnnlsv_driver_21BE78B4(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 32E0372F */
