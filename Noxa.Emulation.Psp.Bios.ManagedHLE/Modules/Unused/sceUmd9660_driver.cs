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
	class sceUmd9660_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUmd9660_driver";
			}
		}

		#endregion

		#region State Management

		public sceUmd9660_driver( Kernel kernel )
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
		[BiosFunction( 0x85F6776E, "sceUmd9660Init" )]
		int sceUmd9660Init(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7565881, "sceUmd9660Term" )]
		int sceUmd9660Term(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26523A78, "sceUmd9660_driver_26523A78" )]
		int sceUmd9660_driver_26523A78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC6F6D8D, "sceUmd9660GetUnitNum" )]
		int sceUmd9660GetUnitNum(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0EC10F9F, "sceUmd9660GetDrive" )]
		int sceUmd9660GetDrive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB1AC979, "sceUmd9660_driver_DB1AC979" )]
		int sceUmd9660_driver_DB1AC979(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44EF600C, "sceUmd9660_driver_44EF600C" )]
		int sceUmd9660_driver_44EF600C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7CD9CE8, "sceUmd9660_driver_C7CD9CE8" )]
		int sceUmd9660_driver_C7CD9CE8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C6C3F4C, "sceUmd9660_driver_2C6C3F4C" )]
		int sceUmd9660_driver_2C6C3F4C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF42C0AEE, "sceUmd9660_driver_F42C0AEE" )]
		int sceUmd9660_driver_F42C0AEE(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 3D30E1CB */
