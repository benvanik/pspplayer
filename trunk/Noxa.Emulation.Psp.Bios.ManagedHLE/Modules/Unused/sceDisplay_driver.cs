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
	class sceDisplay_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDisplay_driver";
			}
		}

		#endregion

		#region State Management

		public sceDisplay_driver( Kernel kernel )
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
		[BiosFunction( 0x206276C2, "sceDisplayInit" )]
		int sceDisplayInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A10289D, "sceDisplayEnd" )]
		int sceDisplayEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E20F177, "sceDisplaySetMode" )]
		int sceDisplaySetMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEA197D4, "sceDisplayGetMode" )]
		int sceDisplayGetMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBA6C4C4, "sceDisplayGetFramePerSec" )]
		int sceDisplayGetFramePerSec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x432D133F, "sceDisplayEnable" )]
		int sceDisplayEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x681EE6A7, "sceDisplayDisable" )]
		int sceDisplayDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ED59BC4, "sceDisplaySetHoldMode" )]
		int sceDisplaySetHoldMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA544C486, "sceDisplaySetResumeMode" )]
		int sceDisplaySetResumeMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63E22A26, "sceDisplay_driver_63E22A26" )]
		int sceDisplay_driver_63E22A26(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B5AEFAD, "sceDisplay_driver_5B5AEFAD" )]
		int sceDisplay_driver_5B5AEFAD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x289D82FE, "sceDisplaySetFrameBuf" )]
		int sceDisplaySetFrameBuf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEDA2E54, "sceDisplayGetFrameBuf" )]
		int sceDisplayGetFrameBuf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4F378FA, "sceDisplayIsForeground" )]
		int sceDisplayIsForeground(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC14F1FF, "sceDisplayGetForegroundLevel" )]
		int sceDisplayGetForegroundLevel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E3C6DC6, "sceDisplay_driver_9E3C6DC6" )]
		int sceDisplay_driver_9E3C6DC6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31C4BAA8, "sceDisplay_driver_31C4BAA8" )]
		int sceDisplay_driver_31C4BAA8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C6EAAD7, "sceDisplayGetVcount" )]
		int sceDisplayGetVcount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D4E10EC, "sceDisplayIsVblank" )]
		int sceDisplayIsVblank(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69B53541, "sceDisplayGetVblankRest" )]
		int sceDisplayGetVblankRest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36CDFADE, "sceDisplayWaitVblank" )]
		int sceDisplayWaitVblank(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EB9EC49, "sceDisplayWaitVblankCB" )]
		int sceDisplayWaitVblankCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x984C27E7, "sceDisplayWaitVblankStart" )]
		int sceDisplayWaitVblankStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46F186C3, "sceDisplayWaitVblankStartCB" )]
		int sceDisplayWaitVblankStartCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x773DD3A3, "sceDisplayGetCurrentHcount" )]
		int sceDisplayGetCurrentHcount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x210EAB3A, "sceDisplayGetAccumulatedHcount" )]
		int sceDisplayGetAccumulatedHcount(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 53279459 */
