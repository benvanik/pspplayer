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
	class sceNetInet_lib : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetInet_lib";
			}
		}

		#endregion

		#region State Management

		public sceNetInet_lib( Kernel kernel )
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
		[BiosFunction( 0x6A046357, "sceNetInet_lib_6A046357" )]
		int sceNetInet_lib_6A046357(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5155EC8A, "sceNetInet_lib_5155EC8A" )]
		int sceNetInet_lib_5155EC8A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F68DB0E, "sceNetInet_lib_4F68DB0E" )]
		int sceNetInet_lib_4F68DB0E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4459EAAF, "sceNetInet_lib_4459EAAF" )]
		int sceNetInet_lib_4459EAAF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB7A7E4F, "sceNetInet_lib_EB7A7E4F" )]
		int sceNetInet_lib_EB7A7E4F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7776A492, "sceNetInet_lib_7776A492" )]
		int sceNetInet_lib_7776A492(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FE19FC4, "sceNetInet_lib_8FE19FC4" )]
		int sceNetInet_lib_8FE19FC4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FDDB0BA, "sceNetInet_lib_3FDDB0BA" )]
		int sceNetInet_lib_3FDDB0BA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6F67D14, "sceNetInet_lib_E6F67D14" )]
		int sceNetInet_lib_E6F67D14(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D905F34, "sceNetInet_lib_3D905F34" )]
		int sceNetInet_lib_3D905F34(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59561561, "sceNetInet_lib_59561561" )]
		int sceNetInet_lib_59561561(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC38FEE9, "sceNetInet_lib_DC38FEE9" )]
		int sceNetInet_lib_DC38FEE9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE9E5174, "sceNetInet_lib_DE9E5174" )]
		int sceNetInet_lib_DE9E5174(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x824CBC6D, "sceNetInet_lib_824CBC6D" )]
		int sceNetInet_lib_824CBC6D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC9D90A5, "sceNetInet_lib_AC9D90A5" )]
		int sceNetInet_lib_AC9D90A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAAF4895A, "sceNetInet_lib_AAF4895A" )]
		int sceNetInet_lib_AAF4895A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEE60F84, "sceNetInet_lib_AEE60F84" )]
		int sceNetInet_lib_AEE60F84(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3FF98EB, "sceNetInet_lib_B3FF98EB" )]
		int sceNetInet_lib_B3FF98EB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CB1D9E3, "sceNetInet_lib_7CB1D9E3" )]
		int sceNetInet_lib_7CB1D9E3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C13BE10, "sceNetInet_lib_4C13BE10" )]
		int sceNetInet_lib_4C13BE10(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7928A6D, "sceNetInet_lib_D7928A6D" )]
		int sceNetInet_lib_D7928A6D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2595064C, "sceNetInet_lib_2595064C" )]
		int sceNetInet_lib_2595064C(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 911C2A9A */
