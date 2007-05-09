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
	class sceI2c_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceI2c_driver";
			}
		}

		#endregion

		#region State Management

		public sceI2c_driver( Kernel kernel )
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
		[BiosFunction( 0x20ADF12A, "sceI2cInit" )]
		int sceI2cInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4809D85F, "sceI2cEnd" )]
		int sceI2cEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21A8282C, "sceI2cSuspend" )]
		int sceI2cSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB952BA65, "sceI2cResume" )]
		int sceI2cResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB6C4F9B, "sceI2c_driver_DB6C4F9B" )]
		int sceI2c_driver_DB6C4F9B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0397408B, "sceI2c_driver_0397408B" )]
		int sceI2c_driver_0397408B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17B0DA59, "sceI2cReset" )]
		int sceI2cReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6E9836A, "sceI2cSetClock" )]
		int sceI2cSetClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9463B99, "sceI2cMasterTransmit" )]
		int sceI2cMasterTransmit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF87D1ED6, "sceI2cMasterReceive" )]
		int sceI2cMasterReceive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6536B1B6, "sceI2cMasterTransmitReceive" )]
		int sceI2cMasterTransmitReceive(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 01793CB4 */
