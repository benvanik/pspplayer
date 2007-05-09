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
	class sceNetApDialogDummy : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetApDialogDummy";
			}
		}

		#endregion

		#region State Management

		public sceNetApDialogDummy( Kernel kernel )
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
		[BiosFunction( 0xBB73FF67, "sceNetApDialogDummyInit" )]
		public int sceNetApDialogDummyInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF213BE65, "sceNetApDialogDummyTerm" )]
		public int sceNetApDialogDummyTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3811281E, "sceNetApDialogDummyConnect" )]
		public int sceNetApDialogDummyConnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA9BE5BF, "sceNetApDialogDummyGetState" )]
		public int sceNetApDialogDummyGetState(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 1569E2AB */
