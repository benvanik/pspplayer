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
	class sceLeptonUpdate_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLeptonUpdate_driver";
			}
		}

		#endregion

		#region State Management

		public sceLeptonUpdate_driver( Kernel kernel )
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
		[BiosFunction( 0xE593B09C, "sceLeptonUpdateInit" )]
		int sceLeptonUpdateInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF30C2D7A, "sceLeptonUpdateEnd" )]
		int sceLeptonUpdateEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E2759BB, "sceLeptonUpdateGetVersion" )]
		int sceLeptonUpdateGetVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x006D3BF8, "sceLeptonUpdateSetDebugCode" )]
		int sceLeptonUpdateSetDebugCode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14847053, "sceLeptonUpdateLeptonPowerOn" )]
		int sceLeptonUpdateLeptonPowerOn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x281E8901, "sceLeptonUpdateLeptonPowerOff" )]
		int sceLeptonUpdateLeptonPowerOff(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE2B6671, "sceLeptonUpdateGetUmdDrive" )]
		int sceLeptonUpdateGetUmdDrive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x231A4635, "sceLeptonUpdateChangePowerMode" )]
		int sceLeptonUpdateChangePowerMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40EF92EC, "sceLeptonUpdate_driver_40EF92EC" )]
		int sceLeptonUpdate_driver_40EF92EC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6B2405D, "sceLeptonUpdateChangeLeptonState" )]
		int sceLeptonUpdateChangeLeptonState(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 73FF3754 */
