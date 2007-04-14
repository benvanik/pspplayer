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
	class sceUsbstorBoot : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUsbstorBoot";
			}
		}

		#endregion

		#region State Management

		public sceUsbstorBoot( Kernel kernel )
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
		[BiosFunction( 0xE58818A8, "sceUsbstorBootSetCapacity" )]
		// SDK location: /usbstor/pspusbstor.h:50
		// SDK declaration: int sceUsbstorBootSetCapacity(u32 size);
		int sceUsbstorBootSetCapacity( int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x594BBF95, "sceUsbstorBootSetLoadAddr" )]
		// SDK location: /usbstor/pspusbstor.h:55
		// SDK declaration: int sceUsbstorBootSetLoadAddr(u32 addr);
		int sceUsbstorBootSetLoadAddr( int addr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D865ECD, "sceUsbstorBootGetDataSize" )]
		// SDK location: /usbstor/pspusbstor.h:54
		// SDK declaration: int sceUsbstorBootGetDataSize();
		int sceUsbstorBootGetDataSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA1119F0D, "sceUsbstorBootSetStatus" )]
		// SDK location: /usbstor/pspusbstor.h:56
		// SDK declaration: int sceUsbstorBootSetStatus(u32 status);
		int sceUsbstorBootSetStatus( int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F080078, "sceUsbstorBootRegisterNotify" )]
		// SDK location: /usbstor/pspusbstor.h:28
		// SDK declaration: int sceUsbstorBootRegisterNotify(u32 eventFlag);
		int sceUsbstorBootRegisterNotify( int eventFlag ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA55C9E16, "sceUsbstorBootUnregisterNotify" )]
		// SDK location: /usbstor/pspusbstor.h:37
		// SDK declaration: int sceUsbstorBootUnregisterNotify(u32 eventFlag);
		int sceUsbstorBootUnregisterNotify( int eventFlag ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 63E4F54A */
