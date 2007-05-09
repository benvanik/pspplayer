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
	class sceNetAdhocAuth_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetAdhocAuth_driver";
			}
		}

		#endregion

		#region State Management

		public sceNetAdhocAuth_driver( Kernel kernel )
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
		[BiosFunction( 0x86004235, "sceNetAdhocAuthInit" )]
		int sceNetAdhocAuthInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6074D8F1, "sceNetAdhocAuthTerm" )]
		int sceNetAdhocAuthTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89F2A732, "sceNetAdhocAuth_driver_89F2A732" )]
		int sceNetAdhocAuth_driver_89F2A732(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E6AA271, "sceNetAdhocAuth_driver_2E6AA271" )]
		int sceNetAdhocAuth_driver_2E6AA271(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CE209A3, "sceNetAdhocAuth_driver_6CE209A3" )]
		int sceNetAdhocAuth_driver_6CE209A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AD8C677, "sceNetAdhocAuth_driver_2AD8C677" )]
		int sceNetAdhocAuth_driver_2AD8C677(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD144DA6, "sceNetAdhocAuth_driver_BD144DA6" )]
		int sceNetAdhocAuth_driver_BD144DA6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F9A90B8, "sceNetAdhocAuth_driver_1F9A90B8" )]
		int sceNetAdhocAuth_driver_1F9A90B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76F26AB0, "sceNetAdhocAuth_driver_76F26AB0" )]
		int sceNetAdhocAuth_driver_76F26AB0(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 8753D0FE */
