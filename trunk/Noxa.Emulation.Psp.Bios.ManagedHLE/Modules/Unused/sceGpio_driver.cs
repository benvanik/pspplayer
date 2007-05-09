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
	class sceGpio_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceGpio_driver";
			}
		}

		#endregion

		#region State Management

		public sceGpio_driver( Kernel kernel )
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
		[BiosFunction( 0xEABDB328, "sceGpioInit" )]
		int sceGpioInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A992B24, "sceGpioEnd" )]
		int sceGpioEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17DAA8C2, "sceGpioSuspend" )]
		int sceGpioSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x64CD4536, "sceGpioResume" )]
		int sceGpioResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4250D44A, "sceGpioPortRead" )]
		int sceGpioPortRead(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x310F0CCF, "sceGpioPortSet" )]
		int sceGpioPortSet(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x103C3EB2, "sceGpioPortClear" )]
		int sceGpioPortClear(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95135905, "sceGpio_driver_95135905" )]
		int sceGpio_driver_95135905(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x317D9D2C, "sceGpioSetPortMode" )]
		int sceGpioSetPortMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA8BE2EA, "sceGpioGetPortMode" )]
		int sceGpioGetPortMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37C8DADC, "sceGpioSetIntrMode" )]
		int sceGpioSetIntrMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF856CE46, "sceGpioGetIntrMode" )]
		int sceGpioGetIntrMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x785206CD, "sceGpioEnableIntr" )]
		int sceGpioEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95D7F3B8, "sceGpioDisableIntr" )]
		int sceGpioDisableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31F34AE6, "sceGpioQueryIntr" )]
		int sceGpioQueryIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE77D1D0, "sceGpioAcquireIntr" )]
		int sceGpioAcquireIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6928224, "sceGpio_driver_C6928224" )]
		int sceGpio_driver_C6928224(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B38B826, "sceGpio_driver_6B38B826" )]
		int sceGpio_driver_6B38B826(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F6D3446F */
