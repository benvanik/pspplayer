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
	class sceMesgLed_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMesgLed_driver";
			}
		}

		#endregion

		#region State Management

		public sceMesgLed_driver( Kernel kernel )
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
		[BiosFunction( 0x84A04017, "sceMesgLed_driver_84A04017" )]
		int sceMesgLed_driver_84A04017(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA86D5005, "sceMesgLed_driver_A86D5005" )]
		int sceMesgLed_driver_A86D5005(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4547DF1, "sceMesgLed_driver_A4547DF1" )]
		int sceMesgLed_driver_A4547DF1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94EB1072, "sceMesgLed_driver_94EB1072" )]
		int sceMesgLed_driver_94EB1072(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x198FD3BE, "sceMesgLed_driver_198FD3BE" )]
		int sceMesgLed_driver_198FD3BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBC694C7, "sceMesgLed_driver_FBC694C7" )]
		int sceMesgLed_driver_FBC694C7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x07E152BE, "sceMesgLed_driver_07E152BE" )]
		int sceMesgLed_driver_07E152BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9906F33A, "sceMesgLed_driver_9906F33A" )]
		int sceMesgLed_driver_9906F33A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46AC0E78, "sceMesgLed_driver_46AC0E78" )]
		int sceMesgLed_driver_46AC0E78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55C8785E, "sceMesgLed_driver_55C8785E" )]
		int sceMesgLed_driver_55C8785E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67A5ECDF, "sceMesgLed_driver_67A5ECDF" )]
		int sceMesgLed_driver_67A5ECDF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x85B9D9F3, "sceMesgLed_driver_85B9D9F3" )]
		int sceMesgLed_driver_85B9D9F3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x951F4A5B, "sceMesgLed_driver_951F4A5B" )]
		int sceMesgLed_driver_951F4A5B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58999D8E, "sceMesgLed_driver_58999D8E" )]
		int sceMesgLed_driver_58999D8E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FC926A0, "sceMesgLed_driver_9FC926A0" )]
		int sceMesgLed_driver_9FC926A0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A922276, "sceMesgLed_driver_7A922276" )]
		int sceMesgLed_driver_7A922276(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4121E651 */
