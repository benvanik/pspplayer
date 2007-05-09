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
	class sceNetAdhocDownload : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetAdhocDownload";
			}
		}

		#endregion

		#region State Management

		public sceNetAdhocDownload( Kernel kernel )
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
		[BiosFunction( 0xA21FEF45, "sceNetAdhocDownloadInitServer" )]
		public int sceNetAdhocDownloadInitServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13DAB550, "sceNetAdhocDownloadCreateServer" )]
		public int sceNetAdhocDownloadCreateServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B6FB0DA, "sceNetAdhocDownloadStartServer" )]
		public int sceNetAdhocDownloadStartServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8FC359E, "sceNetAdhocDownloadReplySession" )]
		public int sceNetAdhocDownloadReplySession(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC421875C, "sceNetAdhocDownloadAbortReplySession" )]
		public int sceNetAdhocDownloadAbortReplySession(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0189004, "sceNetAdhocDownloadSend" )]
		public int sceNetAdhocDownloadSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA70FDFBE, "sceNetAdhocDownloadAbortSend" )]
		public int sceNetAdhocDownloadAbortSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF76147B1, "sceNetAdhocDownloadStopServer" )]
		public int sceNetAdhocDownloadStopServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A483F9E, "sceNetAdhocDownloadDeleteServer" )]
		public int sceNetAdhocDownloadDeleteServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x117CA01A, "sceNetAdhocDownloadTermServer" )]
		public int sceNetAdhocDownloadTermServer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3082F4E2, "sceNetAdhocDownloadInitClient" )]
		public int sceNetAdhocDownloadInitClient(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57A51DD0, "sceNetAdhocDownloadCreateClient" )]
		public int sceNetAdhocDownloadCreateClient(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x314ED31E, "sceNetAdhocDownloadStartClient" )]
		public int sceNetAdhocDownloadStartClient(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x22C2BCC6, "sceNetAdhocDownload_22C2BCC6" )]
		public int sceNetAdhocDownload_22C2BCC6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E6029F1, "sceNetAdhocDownloadRequestSession" )]
		public int sceNetAdhocDownloadRequestSession(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A5500E0, "sceNetAdhocDownloadAbortRequestSession" )]
		public int sceNetAdhocDownloadAbortRequestSession(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8846D2B0, "sceNetAdhocDownloadRecv" )]
		public int sceNetAdhocDownloadRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1AD5CC88, "sceNetAdhocDownloadAbortRecv" )]
		public int sceNetAdhocDownloadAbortRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x469F6B83, "sceNetAdhocDownloadStopClient" )]
		public int sceNetAdhocDownloadStopClient(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x378D4311, "sceNetAdhocDownloadDeleteClient" )]
		public int sceNetAdhocDownloadDeleteClient(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF1433F0, "sceNetAdhocDownloadTermClient" )]
		public int sceNetAdhocDownloadTermClient(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - F1F24751 */
