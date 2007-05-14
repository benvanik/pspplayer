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
using System.Net;

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

		//FIXME: Use this? Then we can return errors when they didn't init the resolver... useful I guess :|
		private bool _isInited;

		//List of
		private int _currentResolverID = 0; //Assign resolvers using this
		private Dictionary<int, int> _allocatedResolvers = new Dictionary<int, int>(); //Record allocated resolvers, rid->rid

		[Stateless]
		[BiosFunction( 0xF3370E61, "sceNetResolverInit" )]
		// SDK location: /net/pspnet_resolver.h:29
		// SDK declaration: int sceNetResolverInit();
		public int sceNetResolverInit()
		{
			_isInited = true;
			return 0;
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

		[Stateless]
		[BiosFunction( 0x244172AF, "sceNetResolverCreate" )]
		// SDK location: /net/pspnet_resolver.h:40
		// SDK declaration: int sceNetResolverCreate(int *rid, void *buf, SceSize buflen);
		public int sceNetResolverCreate( int rid, int buf, int buflen )
		{
			//We are passed in a buffer we can use to do resolving work, but we won't use it.
			// this is buf, with length buflen

			//We allocate a resolver and return the id in *rid
			//TODO: Check what numbers the psp returns, does it return a file descriptor?
			//For this implementation we will use a magic int... lol

			_allocatedResolvers.Add( _currentResolverID, _currentResolverID );
			_memory.WriteWord( rid, 4, _currentResolverID );
			_currentResolverID++;

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x94523E09, "sceNetResolverDelete" )]
		// SDK location: /net/pspnet_resolver.h:49
		// SDK declaration: int sceNetResolverDelete(int rid);
		public int sceNetResolverDelete( int rid )
		{
			if( _allocatedResolvers.ContainsKey( rid ) )
			{
				_allocatedResolvers.Remove( rid );
				return 0;
			}
			else
			{
				//FIXME: Proper return code.
				return -1;
			}
		}

		[Stateless]
		[BiosFunction( 0x224C5F44, "sceNetResolverStartNtoA" )]
		// SDK location: /net/pspnet_resolver.h:62
		// SDK declaration: int sceNetResolverStartNtoA(int rid, const char *hostname, struct in_addr* addr, unsigned int timeout, int retry);
		public int sceNetResolverStartNtoA( int rid, int hostname, int addr, int timeout, int retry )
		{
			//FIXME: Check psp for what error codes to actually return
			//FIXME: If domain fails to resolve, return error...
			//FIXME: we are ignoring timeout and retry
			if( !_allocatedResolvers.ContainsKey( rid ) )
				return -1; //FAILURE

			string HostName = _kernel.ReadString( ( uint )hostname );
			Log.WriteLine( Verbosity.Verbose, Feature.Net, "Resolving Domain {0}", HostName );

			IPHostEntry ip = Dns.GetHostEntry( HostName );
			_memory.WriteBytes( addr, ip.AddressList[ 0 ].GetAddressBytes() );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x629E2FB7, "sceNetResolverStartAtoN" )]
		// SDK location: /net/pspnet_resolver.h:76
		// SDK declaration: int sceNetResolverStartAtoN(int rid, const struct in_addr* addr, char *hostname, SceSize hostname_len, unsigned int timeout, int retry);
		public int sceNetResolverStartAtoN( int rid, int addr, int hostname, int hostname_len, int timeout, int retry )
		{
			//FIXME: return -1 if we fail.
			//FIXME: we are ignoring timeout and retry
			long iplong = ( uint )_memory.ReadWord( addr );
			IPHostEntry host = Dns.GetHostEntry( new IPAddress( iplong ) );
			_kernel.WriteString( ( uint )hostname, host.HostName );
			return 0;
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
