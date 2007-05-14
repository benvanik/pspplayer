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
	class sceNetApctl : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetApctl";
			}
		}

		#endregion

		#region State Management

		public sceNetApctl( Kernel kernel )
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

		private bool _inited = false;
		//State of the connection as the PSP sees it, should probably be an enum (Is one defined in the PSPSDK?)
		// -1: not trying to connect, not sure if this is the correct value to use but it will work.
		//1-3: Stages of connecting, 3 = DHCP'ing
		//  4: connected
		//  6: We won't use it but on OE firmware I have seen a 6 returned from sceNetApctlGetState... (danzel)
		private int _connectionState = -1;

		[Stateless]
		[BiosFunction( 0xE2F91F9B, "sceNetApctlInit" )]
		// SDK location: /net/pspnet_apctl.h:22
		// SDK declaration: public int sceNetApctlInit(int stackSize, int initPriority);
		public int sceNetApctlInit( int stackSize, int initPriority )
		{
			_inited = true;
			//How used in PSPSDK:
			//sceNetApctlInit(0x1400, 0x42)
			//Looks like we are meant to fire up a thread, for now we won't
			// as we don't need one, because we won't really connect to wifi :)
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3EDD0EC, "sceNetApctlTerm" )]
		// SDK location: /net/pspnet_apctl.h:24
		// SDK declaration: public int sceNetApctlTerm();
		public int sceNetApctlTerm()
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x2BEFDF23, "sceNetApctlGetInfo" )]
		// SDK location: /net/pspnet_apctl.h:26
		// SDK declaration: public int sceNetApctlGetInfo(int code, void *pInfo);
		public int sceNetApctlGetInfo( int code, int pInfo )
		{
			//Not well documented, code 8 = write ip as a char* to pInfo
			//Shouldn't need to implement much more for now
			switch (code)
			{
				case 8:
					_kernel.WriteString((uint)pInfo, "127.0.0.1"); //HACK :)
					return 0;
				default:
					Log.WriteLine(Verbosity.Normal, Feature.Net, "Unknown code passed to sceNetApctlGetInfo: {0}", code);
			}
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8ABADD51, "sceNetApctlAddHandler" )]
		public int sceNetApctlAddHandler()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5963991B, "sceNetApctlDelHandler" )]
		public int sceNetApctlDelHandler()
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0xCFB957C6, "sceNetApctlConnect" )]
		// SDK location: /net/pspnet_apctl.h:33
		// SDK declaration: public int sceNetApctlConnect(int connIndex);
		public int sceNetApctlConnect( int connIndex )
		{
			//FIXME: This module should have a list of APCTL's defined
			//       Then we can check against the list and return the correct value
			_connectionState = 0;
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x24FE91A1, "sceNetApctlDisconnect" )]
		// SDK location: /net/pspnet_apctl.h:35
		// SDK declaration: public int sceNetApctlDisconnect();
		public int sceNetApctlDisconnect()
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x5DEAC81B, "sceNetApctlGetState" )]
		// SDK location: /net/pspnet_apctl.h:37
		// SDK declaration: public int sceNetApctlGetState(int *pState);
		public int sceNetApctlGetState( int pState )
		{
			_memory.WriteWord(pState, 4, _connectionState);

			//Pretend-emulation of connecting :)
			//This could be done based on time passing rather than amount of calls to the function
			//will have to see if it messes up in any programs.
			if (_connectionState >= 0 && _connectionState <= 4)
				_connectionState++;

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3CF6849, "sceNetApctlScan" )]
		public int sceNetApctlScan()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C7FFA5C, "sceNetApctl_lib_0C7FFA5C" )]
		public int sceNetApctl_lib_0C7FFA5C()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x96BEB231, "sceNetApctl_lib_96BEB231" )]
		public int sceNetApctl_lib_96BEB231()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CFAB990, "sceNetApctl_lib_7CFAB990" )]
		public int sceNetApctl_lib_7CFAB990()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE11BAFAB, "sceNetApctl_lib_E11BAFAB" )]
		public int sceNetApctl_lib_E11BAFAB()
		{
			return Module.NotImplementedReturn;
		}
	}
}

/* GenerateStubsV2: auto-generated - 063D784B */
