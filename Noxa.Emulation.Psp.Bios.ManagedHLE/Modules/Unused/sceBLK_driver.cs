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
	class sceBLK_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceBLK_driver";
			}
		}

		#endregion

		#region State Management

		public sceBLK_driver( Kernel kernel )
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
		[BiosFunction( 0x6216FF5D, "sceBLKInit" )]
		int sceBLKInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06CD0A03, "sceBLKTerm" )]
		int sceBLKTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FD88F7C, "sceBLKStart" )]
		int sceBLKStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39150835, "sceBLKStop" )]
		int sceBLKStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x550511CE, "sceBLKRegister" )]
		int sceBLKRegister(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43AEEFB6, "sceBLKUnRegister" )]
		int sceBLKUnRegister(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E5285A5, "sceBLKGetDeviceData" )]
		int sceBLKGetDeviceData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA86B6778, "sceBLK_driver_A86B6778" )]
		int sceBLK_driver_A86B6778(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46C91FF8, "sceBLKGetUnitNum" )]
		int sceBLKGetUnitNum(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC4A18BC, "sceBLKStartFeatures" )]
		int sceBLKStartFeatures(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C225F06D */
