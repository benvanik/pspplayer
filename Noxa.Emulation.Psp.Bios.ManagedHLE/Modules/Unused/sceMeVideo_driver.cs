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
	class sceMeVideo_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMeVideo_driver";
			}
		}

		#endregion

		#region State Management

		public sceMeVideo_driver( Kernel kernel )
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
		[BiosFunction( 0xB655AD4E, "sceMeVideo_driver_B655AD4E" )]
		int sceMeVideo_driver_B655AD4E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06F0236A, "sceMeVideo_driver_06F0236A" )]
		int sceMeVideo_driver_06F0236A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x176BBE07, "sceMeVideo_driver_176BBE07" )]
		int sceMeVideo_driver_176BBE07(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE2E7E89, "sceMeVideo_driver_DE2E7E89" )]
		int sceMeVideo_driver_DE2E7E89(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE67F4E0A, "sceMeVideo_driver_E67F4E0A" )]
		int sceMeVideo_driver_E67F4E0A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C6A60E1, "sceMeVideo_driver_1C6A60E1" )]
		int sceMeVideo_driver_1C6A60E1(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - FD24A9D1 */
