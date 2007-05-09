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
	class sceUtility_private : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUtility_private";
			}
		}

		#endregion

		#region State Management

		public sceUtility_private( Kernel kernel )
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
		[BiosFunction( 0x17CB4D96, "sceUtility_private_17CB4D96" )]
		int sceUtility_private_17CB4D96(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE7AC503, "sceUtility_private_EE7AC503" )]
		int sceUtility_private_EE7AC503(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FF96ED3, "sceUtility_private_5FF96ED3" )]
		int sceUtility_private_5FF96ED3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C9DD5BC, "sceUtility_private_9C9DD5BC" )]
		int sceUtility_private_9C9DD5BC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4405BA38, "sceUtility_private_4405BA38" )]
		int sceUtility_private_4405BA38(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1DFA62EF, "sceUtility_private_1DFA62EF" )]
		int sceUtility_private_1DFA62EF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x680C0EA8, "sceUtilityDialogSetStatus" )]
		int sceUtilityDialogSetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB222E887, "sceUtilityDialogGetType" )]
		int sceUtilityDialogGetType(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F2206BC, "sceUtilityDialogGetParam" )]
		int sceUtilityDialogGetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE01FE32A, "sceUtilityDialogGetSpeed" )]
		int sceUtilityDialogGetSpeed(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x19461966, "sceUtility_private_19461966" )]
		int sceUtility_private_19461966(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6F923BD3, "sceUtilityDialogSetThreadId" )]
		int sceUtilityDialogSetThreadId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA5168A5D, "sceUtilityDialogLoadModule" )]
		int sceUtilityDialogLoadModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3CEAE1DF, "sceUtilityDialogPowerLock" )]
		int sceUtilityDialogPowerLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56BEDCA4, "sceUtilityDialogPowerUnlock" )]
		int sceUtilityDialogPowerUnlock(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - A4774831 */
