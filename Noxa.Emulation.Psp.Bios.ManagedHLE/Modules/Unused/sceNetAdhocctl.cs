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
	class sceNetAdhocctl : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetAdhocctl";
			}
		}

		#endregion

		#region State Management

		public sceNetAdhocctl( Kernel kernel )
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
		[BiosFunction( 0xE26F226E, "sceNetAdhocctlInit" )]
		public int sceNetAdhocctlInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D689E13, "sceNetAdhocctlTerm" )]
		public int sceNetAdhocctlTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AD043ED, "sceNetAdhocctlConnect" )]
		public int sceNetAdhocctlConnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC0635C1, "sceNetAdhocctlCreate" )]
		public int sceNetAdhocctlCreate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E7F79C9, "sceNetAdhocctlJoin" )]
		public int sceNetAdhocctlJoin(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08FFF7A0, "sceNetAdhocctlScan" )]
		public int sceNetAdhocctlScan(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34401D65, "sceNetAdhocctlDisconnect" )]
		public int sceNetAdhocctlDisconnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20B317A0, "sceNetAdhocctlAddHandler" )]
		public int sceNetAdhocctlAddHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6402490B, "sceNetAdhocctlDelHandler" )]
		public int sceNetAdhocctlDelHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75ECD386, "sceNetAdhocctlGetState" )]
		public int sceNetAdhocctlGetState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x362CBE8F, "sceNetAdhocctlGetAdhocId" )]
		public int sceNetAdhocctlGetAdhocId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE162CB14, "sceNetAdhocctlGetPeerList" )]
		public int sceNetAdhocctlGetPeerList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x99560ABE, "sceNetAdhocctlGetAddrByName" )]
		public int sceNetAdhocctlGetAddrByName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8916C003, "sceNetAdhocctlGetNameByAddr" )]
		public int sceNetAdhocctlGetNameByAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDED9D28E, "sceNetAdhocctlGetParameter" )]
		public int sceNetAdhocctlGetParameter(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81AEE1BE, "sceNetAdhocctlGetScanInfo" )]
		public int sceNetAdhocctlGetScanInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA5C055CE, "sceNetAdhocctlCreateEnterGameMode" )]
		public int sceNetAdhocctlCreateEnterGameMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FF89745, "sceNetAdhocctlJoinEnterGameMode" )]
		public int sceNetAdhocctlJoinEnterGameMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF8E084D, "sceNetAdhocctlExitGameMode" )]
		public int sceNetAdhocctlExitGameMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A014CE0, "sceNetAdhocctlGetGameModeInfo" )]
		public int sceNetAdhocctlGetGameModeInfo(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 9BEEB1A6 */
