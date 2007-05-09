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
	class sceHttp : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceHttp";
			}
		}

		#endregion

		#region State Management

		public sceHttp( Kernel kernel )
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
		[BiosFunction( 0xAB1ABE07, "sceHttpInit" )]
		public int sceHttpInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1C8945E, "sceHttpEnd" )]
		public int sceHttpEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B1F1F36, "sceHttpCreateTemplate" )]
		public int sceHttpCreateTemplate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCF8C055, "sceHttpDeleteTemplate" )]
		public int sceHttpDeleteTemplate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EEFD953, "sceHttpCreateConnection" )]
		public int sceHttpCreateConnection(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDF8ECB9, "sceHttpCreateConnectionWithURL" )]
		public int sceHttpCreateConnectionWithURL(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5152773B, "sceHttpDeleteConnection" )]
		public int sceHttpDeleteConnection(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47347B50, "sceHttpCreateRequest" )]
		public int sceHttpCreateRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB509B09E, "sceHttpCreateRequestWithURL" )]
		public int sceHttpCreateRequestWithURL(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA5512E01, "sceHttpDeleteRequest" )]
		public int sceHttpDeleteRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47940436, "sceHttpSetResolveTimeOut" )]
		public int sceHttpSetResolveTimeOut(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8ACD1F73, "sceHttpSetConnectTimeOut" )]
		public int sceHttpSetConnectTimeOut(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9988172D, "sceHttpSetSendTimeOut" )]
		public int sceHttpSetSendTimeOut(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F0FC3E3, "sceHttpSetRecvTimeOut" )]
		public int sceHttpSetRecvTimeOut(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78A0D3EC, "sceHttpEnableKeepAlive" )]
		public int sceHttpEnableKeepAlive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7EF2559, "sceHttpDisableKeepAlive" )]
		public int sceHttpDisableKeepAlive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0809C831, "sceHttpEnableRedirect" )]
		public int sceHttpEnableRedirect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A0EBB69, "sceHttpDisableRedirect" )]
		public int sceHttpDisableRedirect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FC5F10D, "sceHttpEnableAuth" )]
		public int sceHttpEnableAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE948FEE, "sceHttpDisableAuth" )]
		public int sceHttpDisableAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DAFA58F, "sceHttpEnableCookie" )]
		public int sceHttpEnableCookie(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B12ABFB, "sceHttpDisableCookie" )]
		public int sceHttpDisableCookie(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA6800C34, "sceHttpInitCache" )]
		public int sceHttpInitCache(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78B54C09, "sceHttpEndCache" )]
		public int sceHttpEndCache(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59E6D16F, "sceHttpEnableCache" )]
		public int sceHttpEnableCache(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCCBD167A, "sceHttpDisableCache" )]
		public int sceHttpDisableCache(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB70706F, "sceHttpSendRequest" )]
		public int sceHttpSendRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC10B6BD9, "sceHttpAbortRequest" )]
		public int sceHttpAbortRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0282A3BD, "sceHttpGetContentLength" )]
		public int sceHttpGetContentLength(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CC7D78F, "sceHttpGetStatusCode" )]
		public int sceHttpGetStatusCode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB266CCF, "sceHttpGetAllHeader" )]
		public int sceHttpGetAllHeader(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDEEB999, "sceHttpReadData" )]
		public int sceHttpReadData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF49934F6, "sceHttpSetMallocFunction" )]
		public int sceHttpSetMallocFunction(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A6C3296, "sceHttpSetAuthInfoCB" )]
		public int sceHttpSetAuthInfoCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15540184, "sceHttpDeleteHeader" )]
		public int sceHttpDeleteHeader(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EABA285, "sceHttpAddExtraHeader" )]
		public int sceHttpAddExtraHeader(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0F46C62, "sceHttpSetProxy" )]
		public int sceHttpSetProxy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD70D4847, "sceHttpGetProxy" )]
		public int sceHttpGetProxy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD081EC8F, "sceHttpGetNetworkErrno" )]
		public int sceHttpGetNetworkErrno(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE4D21302, "sceHttpsInit" )]
		public int sceHttpsInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68AB0F86, "sceHttpsInitWithPath" )]
		public int sceHttpsInitWithPath(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87797BDD, "sceHttpsLoadDefaultCert" )]
		public int sceHttpsLoadDefaultCert(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9D8EB63, "sceHttpsEnd" )]
		public int sceHttpsEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAC31BF1, "sceHttpsEnableOption" )]
		public int sceHttpsEnableOption(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3FAF831, "sceHttpsDisableOption" )]
		public int sceHttpsDisableOption(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 0A27A37B */
