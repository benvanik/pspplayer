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
	class sceChkuppkg_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceChkuppkg_driver";
			}
		}

		#endregion

		#region State Management

		public sceChkuppkg_driver( Kernel kernel )
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
		[BiosFunction( 0xE409CCC6, "sceChkuppkg_driver_E409CCC6" )]
		int sceChkuppkg_driver_E409CCC6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56481562, "sceChkuppkg_driver_56481562" )]
		int sceChkuppkg_driver_56481562(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4DB6542, "sceChkuppkg_driver_F4DB6542" )]
		int sceChkuppkg_driver_F4DB6542(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B6790C3, "sceChkuppkg_driver_7B6790C3" )]
		int sceChkuppkg_driver_7B6790C3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63F0C3D4, "sceChkuppkg_driver_63F0C3D4" )]
		int sceChkuppkg_driver_63F0C3D4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38C255EF, "sceChkuppkg_driver_38C255EF" )]
		int sceChkuppkg_driver_38C255EF(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 66DA5BD6 */
