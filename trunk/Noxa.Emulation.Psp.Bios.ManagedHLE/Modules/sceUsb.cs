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
	class sceUsb : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUsb";
			}
		}

		#endregion

		#region State Management

		public sceUsb( Kernel kernel )
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
		// SDK location: /usb/pspusb.h:35
		// SDK declaration: int sceUsbStart(const char* driverName, int size, void *args);
		int sceUsbStart( int driverName, int size, int args ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2464FA0, "sceUsbStop" )]
		// SDK location: /usb/pspusb.h:46
		// SDK declaration: int sceUsbStop(const char* driverName, int size, void *args);
		int sceUsbStop( int driverName, int size, int args ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC21645A4, "sceUsbGetState" )]
		// SDK location: /usb/pspusb.h:71
		// SDK declaration: int sceUsbGetState();
		int sceUsbGetState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E537366, "sceUsbGetDrvList" )]
		// SDK location: /usb/pspusb.h:83
		// SDK declaration: int sceUsbGetDrvList(u32 r4one, u32* r5ret, u32 r6one);
		int sceUsbGetDrvList( int r4one, int r5ret, int r6one ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x112CC951, "sceUsbGetDrvState" )]
		// SDK location: /usb/pspusb.h:80
		// SDK declaration: int sceUsbGetDrvState(const char* driverName);
		int sceUsbGetDrvState( int driverName ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x586DB82C, "sceUsbActivate" )]
		// SDK location: /usb/pspusb.h:55
		// SDK declaration: int sceUsbActivate(u32 pid);
		int sceUsbActivate( int pid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC572A9C8, "sceUsbDeactivate" )]
		// SDK location: /usb/pspusb.h:64
		// SDK declaration: int sceUsbDeactivate(u32 pid);
		int sceUsbDeactivate( int pid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BE0E002, "sceUsbWaitState" )]
		// SDK location: /usb/pspusb.h:84
		// SDK declaration: int sceUsbWaitState(u32 state, s32 waitmode, u32 *timeout);
		int sceUsbWaitState( int state, int waitmode, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C360735, "sceUsbWaitCancel" )]
		// SDK location: /usb/pspusb.h:85
		// SDK declaration: int sceUsbWaitCancel();
		int sceUsbWaitCancel(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 324626AF */
