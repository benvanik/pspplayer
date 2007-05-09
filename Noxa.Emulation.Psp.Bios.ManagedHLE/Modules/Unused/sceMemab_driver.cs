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
	class sceMemab_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMemab_driver";
			}
		}

		#endregion

		#region State Management

		public sceMemab_driver( Kernel kernel )
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
		[BiosFunction( 0x01FD1BA0, "sceMemab_driver_01FD1BA0" )]
		int sceMemab_driver_01FD1BA0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C134D3F, "sceMemab_driver_0C134D3F" )]
		int sceMemab_driver_0C134D3F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4720EED, "sceMemab_driver_A4720EED" )]
		int sceMemab_driver_A4720EED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC598EF3, "sceMemab_driver_DC598EF3" )]
		int sceMemab_driver_DC598EF3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8DAC2343, "sceMemab_driver_8DAC2343" )]
		int sceMemab_driver_8DAC2343(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAE8A629, "sceMemab_driver_DAE8A629" )]
		int sceMemab_driver_DAE8A629(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF49BCBB, "sceMemab_driver_CF49BCBB" )]
		int sceMemab_driver_CF49BCBB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6D3ACE2, "sceMemab_driver_E6D3ACE2" )]
		int sceMemab_driver_E6D3ACE2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB54E1AA7, "sceMemab_driver_B54E1AA7" )]
		int sceMemab_driver_B54E1AA7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1D2B9B0, "sceMemab_driver_F1D2B9B0" )]
		int sceMemab_driver_F1D2B9B0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4DF566D3, "sceMemab_driver_4DF566D3" )]
		int sceMemab_driver_4DF566D3(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C03C8869 */
