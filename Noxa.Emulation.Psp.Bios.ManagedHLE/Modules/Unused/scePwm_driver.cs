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
	class scePwm_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePwm_driver";
			}
		}

		#endregion

		#region State Management

		public scePwm_driver( Kernel kernel )
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
		[BiosFunction( 0x68BA9CC1, "scePwmInit" )]
		int scePwmInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3169427, "scePwmEnd" )]
		int scePwmEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x526F94FC, "scePwmSuspend" )]
		int scePwmSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE449F656, "scePwmResume" )]
		int scePwmResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x850ED3D3, "scePwmStart" )]
		int scePwmStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FC555BE, "scePwmStop" )]
		int scePwmStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB6D2E36, "scePwm_driver_AB6D2E36" )]
		int scePwm_driver_AB6D2E36(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF624C1A0, "scePwm_driver_F624C1A0" )]
		int scePwm_driver_F624C1A0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93F30DAC, "scePwmIsLoading" )]
		int scePwmIsLoading(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 10E58BFC */
