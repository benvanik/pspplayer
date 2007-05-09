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
	class sceUsbBus_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUsbBus_driver";
			}
		}

		#endregion

		#region State Management

		public sceUsbBus_driver( Kernel kernel )
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
		[BiosFunction( 0xC21645A4, "sceUsbGetState" )]
		int sceUsbGetState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1644BE7, "sceUsbbdRegister" )]
		int sceUsbbdRegister(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1E2A540, "sceUsbbdUnregister" )]
		int sceUsbbdUnregister(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x951A24CC, "sceUsbbdClearFIFO" )]
		int sceUsbbdClearFIFO(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE65441C1, "sceUsbbdStall" )]
		int sceUsbbdStall(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23E51D8F, "sceUsbbdReqSend" )]
		int sceUsbbdReqSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x913EC15D, "sceUsbbdReqRecv" )]
		int sceUsbbdReqRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC57EC9D, "sceUsbbdReqCancel" )]
		int sceUsbbdReqCancel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5E53685, "sceUsbbdReqCancelAll" )]
		int sceUsbbdReqCancelAll(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 365E8140 */
