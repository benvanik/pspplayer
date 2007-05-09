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
	class sceMeWrapper_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMeWrapper_driver";
			}
		}

		#endregion

		#region State Management

		public sceMeWrapper_driver( Kernel kernel )
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
		[BiosFunction( 0x63463F66, "sceMeWrapperInit" )]
		int sceMeWrapperInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3A1D6B2, "sceMeWrapperEnd" )]
		int sceMeWrapperEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB655AD4E, "sceMeWrapper_driver_B655AD4E" )]
		int sceMeWrapper_driver_B655AD4E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06F0236A, "sceMeWrapper_driver_06F0236A" )]
		int sceMeWrapper_driver_06F0236A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x176BBE07, "sceMeWrapper_driver_176BBE07" )]
		int sceMeWrapper_driver_176BBE07(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE2E7E89, "sceMeWrapper_driver_DE2E7E89" )]
		int sceMeWrapper_driver_DE2E7E89(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE67F4E0A, "sceMeWrapper_driver_E67F4E0A" )]
		int sceMeWrapper_driver_E67F4E0A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C6A60E1, "sceMeWrapper_driver_1C6A60E1" )]
		int sceMeWrapper_driver_1C6A60E1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC00D873B, "sceMeWrapper_driver_C00D873B" )]
		int sceMeWrapper_driver_C00D873B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEE0A97D, "sceMeWrapper_driver_FEE0A97D" )]
		int sceMeWrapper_driver_FEE0A97D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAC7C2BE, "sceMeWrapper_driver_BAC7C2BE" )]
		int sceMeWrapper_driver_BAC7C2BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39F06790, "sceMeWrapper_driver_39F06790" )]
		int sceMeWrapper_driver_39F06790(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA177798D, "sceMeMalloc" )]
		int sceMeMalloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC86500E3, "sceMeCalloc" )]
		int sceMeCalloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AD660FA, "sceMeFree" )]
		int sceMeFree(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1EA3DFD, "sceMeWrapper_driver_D1EA3DFD" )]
		int sceMeWrapper_driver_D1EA3DFD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D5F109C, "sceMeWrapper_driver_3D5F109C" )]
		int sceMeWrapper_driver_3D5F109C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04AFF68E, "sceMeRpcLock" )]
		int sceMeRpcLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB97B15D7, "sceMeRpcUnlock" )]
		int sceMeRpcUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4794C05C, "sceMeEnableFunctions" )]
		int sceMeEnableFunctions(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55F11AF6, "sceMeWrapper_driver_55F11AF6" )]
		int sceMeWrapper_driver_55F11AF6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EBBBBA3, "sceMeWrapper_driver_8EBBBBA3" )]
		int sceMeWrapper_driver_8EBBBBA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6FE3E62, "sceMeWrapper_driver_B6FE3E62" )]
		int sceMeWrapper_driver_B6FE3E62(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05B2C420, "sceMePowerControlAvcPower" )]
		int sceMePowerControlAvcPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x579ACEA9, "sceMePowerSelectAvcClock" )]
		int sceMePowerSelectAvcClock(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F86F42A4 */
