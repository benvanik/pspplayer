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
	class sceWlanDrv : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceWlanDrv";
			}
		}

		#endregion

		#region State Management

		public sceWlanDrv( Kernel kernel )
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
		[BiosFunction( 0x93440B11, "sceWlanDevIsPowerOn" )]
		// SDK location: /wlan/pspwlan.h:24
		// SDK declaration: int sceWlanDevIsPowerOn();
		public int sceWlanDevIsPowerOn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7763699, "sceWlanGetSwitchState" )]
		// SDK location: /wlan/pspwlan.h:31
		// SDK declaration: int sceWlanGetSwitchState();
		public int sceWlanGetSwitchState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C622081, "sceWlanGetEtherAddr" )]
		// SDK location: /wlan/pspwlan.h:40
		// SDK declaration: int sceWlanGetEtherAddr(u8 *etherAddr);
		public int sceWlanGetEtherAddr( int etherAddr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x482CAE9A, "sceWlanDevAttach" )]
		// SDK location: /wlan/pspwlan.h:47
		// SDK declaration: int sceWlanDevAttach();
		public int sceWlanDevAttach(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9A8CAB7, "sceWlanDevDetach" )]
		// SDK location: /wlan/pspwlan.h:54
		// SDK declaration: int sceWlanDevDetach();
		public int sceWlanDevDetach(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - CCF4999B */
