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
	class sceLcdc_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLcdc_driver";
			}
		}

		#endregion

		#region State Management

		public sceLcdc_driver( Kernel kernel )
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
		[BiosFunction( 0xB55500A3, "sceLcdcInit" )]
		int sceLcdcInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E8E8774, "sceLcdcEnd" )]
		int sceLcdcEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDCD51769, "sceLcdcSuspend" )]
		int sceLcdcSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6F10C77, "sceLcdcResume" )]
		int sceLcdcResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA182B32C, "sceLcdcEnable" )]
		int sceLcdcEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0032C3D, "sceLcdcDisable" )]
		int sceLcdcDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73A3A01D, "sceLcdcCheckMode" )]
		int sceLcdcCheckMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0BC2B5E2, "sceLcdcSetMode" )]
		int sceLcdcSetMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2E70DA6, "sceLcdcReset" )]
		int sceLcdcReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x965D1633, "sceLcdc_driver_965D1633" )]
		int sceLcdc_driver_965D1633(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3BA421A, "sceLcdc_driver_B3BA421A" )]
		int sceLcdc_driver_B3BA421A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3107417C, "sceLcdc_driver_3107417C" )]
		int sceLcdc_driver_3107417C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x551C5BC3, "sceLcdc_driver_551C5BC3" )]
		int sceLcdc_driver_551C5BC3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE5A8859, "sceLcdc_driver_FE5A8859" )]
		int sceLcdc_driver_FE5A8859(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BDC15C8, "sceLcdc_driver_7BDC15C8" )]
		int sceLcdc_driver_7BDC15C8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE08B076B, "sceLcdc_driver_E08B076B" )]
		int sceLcdc_driver_E08B076B(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 2DDA9F63 */
