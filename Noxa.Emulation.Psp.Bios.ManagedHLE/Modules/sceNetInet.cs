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
	class sceNetInet : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetInet";
			}
		}

		#endregion

		#region State Management

		public sceNetInet( Kernel kernel )
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

		private bool _isInited;

		[Stateless]
		[BiosFunction( 0x17943399, "sceNetInetInit" )]
		// SDK location: /net/pspnet_inet.h:22
		// SDK declaration: public int sceNetInetInit();
		public int sceNetInetInit()
		{
			//FIXME: Should this care wether sceNet has been inited?
			_isInited = true;
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9ED66B9, "sceNetInetTerm" )]
		// SDK location: /net/pspnet_inet.h:23
		// SDK declaration: public int sceNetInetTerm();
		public int sceNetInetTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB094E1B, "sceNetInetAccept" )]
		public int sceNetInetAccept(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A33F9AE, "sceNetInetBind" )]
		public int sceNetInetBind(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D7284EA, "sceNetInetClose" )]
		// SDK location: /net/pspnet_inet.h:39
		// SDK declaration: public int sceNetInetClose(int s);
		public int sceNetInetClose( int s ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x805502DD, "sceNetInetCloseWithRST" )]
		public int sceNetInetCloseWithRST(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x410B34AA, "sceNetInetConnect" )]
		public int sceNetInetConnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE247B6D6, "sceNetInetGetpeername" )]
		public int sceNetInetGetpeername(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x162E6FD5, "sceNetInetGetsockname" )]
		public int sceNetInetGetsockname(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A114C7C, "sceNetInetGetsockopt" )]
		public int sceNetInetGetsockopt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD10A1A7A, "sceNetInetListen" )]
		public int sceNetInetListen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFAABB1DD, "sceNetInetPoll" )]
		public int sceNetInetPoll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDA85C99, "sceNetInetRecv" )]
		public int sceNetInetRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC91142E4, "sceNetInetRecvfrom" )]
		public int sceNetInetRecvfrom(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEECE61D2, "sceNetInetRecvmsg" )]
		public int sceNetInetRecvmsg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BE8D595, "sceNetInetSelect" )]
		public int sceNetInetSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AA671BC, "sceNetInetSend" )]
		public int sceNetInetSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05038FC7, "sceNetInetSendto" )]
		public int sceNetInetSendto(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x774E36F4, "sceNetInetSendmsg" )]
		public int sceNetInetSendmsg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FE71FE7, "sceNetInetSetsockopt" )]
		public int sceNetInetSetsockopt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CFE4E56, "sceNetInetShutdown" )]
		public int sceNetInetShutdown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8B7B220F, "sceNetInetSocket" )]
		public int sceNetInetSocket(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80A21ABD, "sceNetInetSocketAbort" )]
		public int sceNetInetSocketAbort(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBABE411, "sceNetInetGetErrno" )]
		// SDK location: /net/pspnet_inet.h:40
		// SDK declaration: public int sceNetInetGetErrno();
		public int sceNetInetGetErrno(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3888AD4, "sceNetInetGetTcpcbstat" )]
		public int sceNetInetGetTcpcbstat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39B0C7D3, "sceNetInetGetUdpcbstat" )]
		public int sceNetInetGetUdpcbstat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB75D5B0A, "sceNetInetInetAddr" )]
		// SDK location: ??
		// SDK declaration: in_addr_t sceNetInetInetAddr(const char *ip);
		public int sceNetInetInetAddr( int ip ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1BDF5D13, "sceNetInetInetAton" )]
		// SDK location: ??
		// SDK declaration: public int sceNetInetInetAton(const char *ip, struct in_addr *in);
		public int sceNetInetInetAton( int ip, int pin ){ return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0xD0792666, "sceNetInetInetNtop" )]
		// SDK location: ??
		// SDK declaration: const char* sceNetInetInetNtop(int af, const void *src, char *dst, socklen_t cnt);
		public int sceNetInetInetNtop( int af, int src, int dst, int cnt )
		{
			//Create the string, write it into memory.
			//we should limit the string length to cnt-1 chars, but who cares.
			uint ipnum = ( uint )( _memory.ReadWord( src ) );
			string ip = +( ipnum & 0xFF ) +
				"." + ( ( ipnum >> 8 ) & 0xFF ) +
				"." + ( ( ipnum >> 16 ) & 0xFF ) +
				"." + ( ipnum >> 24 );

			_kernel.WriteString( ( uint )dst, ip );
			return dst;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE30B8C19, "sceNetInetInetPton" )]
		// SDK location: ??
		// SDK declaration: public int sceNetInetInetPton(int af, const char *src, void *dst);
		public int sceNetInetInetPton( int af, int src, int dst ){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - C49D9AB7 */
