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
	class sceReg_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceReg_driver";
			}
		}

		#endregion

		#region State Management

		public sceReg_driver( Kernel kernel )
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
		[BiosFunction( 0x98279CF1, "sceRegInit" )]
		int sceRegInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B25EDF1, "sceRegExit" )]
		int sceRegExit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92E41280, "sceRegOpenRegistry" )]
		int sceRegOpenRegistry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA8A5739, "sceRegCloseRegistry" )]
		int sceRegCloseRegistry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEDA92BF, "sceRegRemoveRegistry" )]
		int sceRegRemoveRegistry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D8A762E, "sceRegOpenCategory" )]
		int sceRegOpenCategory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0CAE832B, "sceRegCloseCategory" )]
		int sceRegCloseCategory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39461B4D, "sceRegFlushRegistry" )]
		int sceRegFlushRegistry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D69BF40, "sceRegFlushCategory" )]
		int sceRegFlushCategory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57641A81, "sceRegCreateKey" )]
		int sceRegCreateKey(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17768E14, "sceRegSetKeyValue" )]
		int sceRegSetKeyValue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4475AA8, "sceRegGetKeyInfo" )]
		int sceRegGetKeyInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28A8E98A, "sceRegGetKeyValue" )]
		int sceRegGetKeyValue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C0DB9DD, "sceRegGetKeysNum" )]
		int sceRegGetKeysNum(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D211135, "sceRegGetKeys" )]
		int sceRegGetKeys(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CA16893, "sceRegRemoveCategory" )]
		int sceRegRemoveCategory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3615BC87, "sceRegRemoveKey" )]
		int sceRegRemoveKey(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5768D02, "sceRegGetKeyInfoByName" )]
		int sceRegGetKeyInfoByName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30BE0259, "sceRegGetKeyValueByName" )]
		int sceRegGetKeyValueByName(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 309D4722 */
