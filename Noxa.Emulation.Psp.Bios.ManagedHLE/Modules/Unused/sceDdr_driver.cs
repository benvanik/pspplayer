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
	class sceDdr_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDdr_driver";
			}
		}

		#endregion

		#region State Management

		public sceDdr_driver( Kernel kernel )
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
		[BiosFunction( 0x17D39E17, "sceDdrInit" )]
		int sceDdrInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8698F5DA, "sceDdrEnd" )]
		int sceDdrEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x623A233F, "sceDdrSuspend" )]
		int sceDdrSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92289EC0, "sceDdrResume" )]
		int sceDdrResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7C6C313, "sceDdrSetup" )]
		int sceDdrSetup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CE55E76, "sceDdrChangePllClock" )]
		int sceDdrChangePllClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87D86769, "sceDdrFlush" )]
		int sceDdrFlush(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x286E1372, "sceDdr_driver_286E1372" )]
		int sceDdr_driver_286E1372(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DC43DE4, "sceDdr_driver_0DC43DE4" )]
		int sceDdr_driver_0DC43DE4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7251F5AB, "sceDdr_driver_7251F5AB" )]
		int sceDdr_driver_7251F5AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B5831E5, "sceDdr_driver_5B5831E5" )]
		int sceDdr_driver_5B5831E5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF23B7983, "sceDdr_driver_F23B7983" )]
		int sceDdr_driver_F23B7983(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x655A9AB0, "sceDdrWriteMaxAllocate" )]
		int sceDdrWriteMaxAllocate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6955346C, "sceDdr_driver_6955346C" )]
		int sceDdr_driver_6955346C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6078F911, "sceDdr_driver_6078F911" )]
		int sceDdr_driver_6078F911(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91CD8F94, "sceDdrResetDevice" )]
		int sceDdrResetDevice(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5694ECD, "sceDdr_driver_B5694ECD" )]
		int sceDdr_driver_B5694ECD(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 553AE96E */
