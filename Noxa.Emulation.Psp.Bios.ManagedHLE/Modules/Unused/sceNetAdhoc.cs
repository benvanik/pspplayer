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
	class sceNetAdhoc : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetAdhoc";
			}
		}

		#endregion

		#region State Management

		public sceNetAdhoc( Kernel kernel )
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
		[BiosFunction( 0xE1D621D7, "sceNetAdhocInit" )]
		public int sceNetAdhocInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA62C6F57, "sceNetAdhocTerm" )]
		public int sceNetAdhocTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A662D6B, "sceNetAdhocPollSocket" )]
		public int sceNetAdhocPollSocket(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73BFD52D, "sceNetAdhocSetSocketAlert" )]
		public int sceNetAdhocSetSocketAlert(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D2CE199, "sceNetAdhocGetSocketAlert" )]
		public int sceNetAdhocGetSocketAlert(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6F92741B, "sceNetAdhocPdpCreate" )]
		public int sceNetAdhocPdpCreate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABED3790, "sceNetAdhocPdpSend" )]
		public int sceNetAdhocPdpSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFE53E03, "sceNetAdhocPdpRecv" )]
		public int sceNetAdhocPdpRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F27BB5E, "sceNetAdhocPdpDelete" )]
		public int sceNetAdhocPdpDelete(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7C1FC57, "sceNetAdhocGetPdpStat" )]
		public int sceNetAdhocGetPdpStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x877F6D66, "sceNetAdhocPtpOpen" )]
		public int sceNetAdhocPtpOpen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC6FC07B, "sceNetAdhocPtpConnect" )]
		public int sceNetAdhocPtpConnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE08BDAC1, "sceNetAdhocPtpListen" )]
		public int sceNetAdhocPtpListen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DF81198, "sceNetAdhocPtpAccept" )]
		public int sceNetAdhocPtpAccept(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4DA4C788, "sceNetAdhocPtpSend" )]
		public int sceNetAdhocPtpSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8BEA2B3E, "sceNetAdhocPtpRecv" )]
		public int sceNetAdhocPtpRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9AC2EEAC, "sceNetAdhocPtpFlush" )]
		public int sceNetAdhocPtpFlush(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x157E6225, "sceNetAdhocPtpClose" )]
		public int sceNetAdhocPtpClose(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9685118, "sceNetAdhocGetPtpStat" )]
		public int sceNetAdhocGetPtpStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F75C338, "sceNetAdhocGameModeCreateMaster" )]
		public int sceNetAdhocGameModeCreateMaster(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3278AB0C, "sceNetAdhocGameModeCreateReplica" )]
		public int sceNetAdhocGameModeCreateReplica(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98C204C8, "sceNetAdhocGameModeUpdateMaster" )]
		public int sceNetAdhocGameModeUpdateMaster(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA324B4E, "sceNetAdhocGameModeUpdateReplica" )]
		public int sceNetAdhocGameModeUpdateReplica(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0229362, "sceNetAdhocGameModeDeleteMaster" )]
		public int sceNetAdhocGameModeDeleteMaster(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B2228E9, "sceNetAdhocGameModeDeleteReplica" )]
		public int sceNetAdhocGameModeDeleteReplica(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 62C61AA7 */
