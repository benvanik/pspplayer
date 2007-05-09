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
	class sceUsb_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUsb_driver";
			}
		}

		#endregion

		#region State Management

		public sceUsb_driver( Kernel kernel )
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
		[BiosFunction( 0xAE5DE6AF, "sceUsbStart" )]
		int sceUsbStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2464FA0, "sceUsbStop" )]
		int sceUsbStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC21645A4, "sceUsbGetState" )]
		int sceUsbGetState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E537366, "sceUsbGetDrvList" )]
		int sceUsbGetDrvList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x112CC951, "sceUsbGetDrvState" )]
		int sceUsbGetDrvState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x586DB82C, "sceUsbActivate" )]
		int sceUsbActivate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC572A9C8, "sceUsbDeactivate" )]
		int sceUsbDeactivate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BE0E002, "sceUsbWaitState" )]
		int sceUsbWaitState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C360735, "sceUsbWaitCancel" )]
		int sceUsbWaitCancel(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D4E7198C */
