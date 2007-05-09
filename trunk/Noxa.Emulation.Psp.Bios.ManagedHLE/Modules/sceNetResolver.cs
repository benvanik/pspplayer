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
	class sceNetResolver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetResolver";
			}
		}

		#endregion

		#region State Management

		public sceNetResolver( Kernel kernel )
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
		[BiosFunction( 0xF3370E61, "sceNetResolverInit" )]
		// SDK location: /net/pspnet_resolver.h:29
		// SDK declaration: int sceNetResolverInit();
		public int sceNetResolverInit()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6138194A, "sceNetResolverTerm" )]
		// SDK location: /net/pspnet_resolver.h:92
		// SDK declaration: int sceNetResolverTerm();
		public int sceNetResolverTerm()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x244172AF, "sceNetResolverCreate" )]
		// SDK location: /net/pspnet_resolver.h:40
		// SDK declaration: int sceNetResolverCreate(int *rid, void *buf, SceSize buflen);
		public int sceNetResolverCreate( int rid, int buf, int buflen )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94523E09, "sceNetResolverDelete" )]
		// SDK location: /net/pspnet_resolver.h:49
		// SDK declaration: int sceNetResolverDelete(int rid);
		public int sceNetResolverDelete( int rid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x224C5F44, "sceNetResolverStartNtoA" )]
		// SDK location: /net/pspnet_resolver.h:62
		// SDK declaration: int sceNetResolverStartNtoA(int rid, const char *hostname, struct in_addr* addr, unsigned int timeout, int retry);
		public int sceNetResolverStartNtoA( int rid, int hostname, int addr, int timeout, int retry )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x629E2FB7, "sceNetResolverStartAtoN" )]
		// SDK location: /net/pspnet_resolver.h:76
		// SDK declaration: int sceNetResolverStartAtoN(int rid, const struct in_addr* addr, char *hostname, SceSize hostname_len, unsigned int timeout, int retry);
		public int sceNetResolverStartAtoN( int rid, int addr, int hostname, int hostname_len, int timeout, int retry )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x808F6063, "sceNetResolverStop" )]
		// SDK location: /net/pspnet_resolver.h:85
		// SDK declaration: int sceNetResolverStop(int rid);
		public int sceNetResolverStop( int rid )
		{
			return Module.NotImplementedReturn;
		}
	}
}

/* GenerateStubsV2: auto-generated - 07F64734 */
