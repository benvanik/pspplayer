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
	class sceSuspendForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSuspendForKernel";
			}
		}

		#endregion

		#region State Management

		public sceSuspendForKernel( Kernel kernel )
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
		[BiosFunction( 0xBDE686CD, "sceKernelRegisterPowerHandlers" )]
		int sceKernelRegisterPowerHandlers(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEADB1BD7, "sceKernelPowerLock" )]
		int sceKernelPowerLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB53B2147, "sceKernelPowerLockForUser" )]
		int sceKernelPowerLockForUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AEE7261, "sceKernelPowerUnlock" )]
		int sceKernelPowerUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7C928C7, "sceKernelPowerUnlockForUser" )]
		int sceKernelPowerUnlockForUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x090CCB3F, "sceKernelPowerTick" )]
		int sceKernelPowerTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98A1D061, "sceKernelPowerRebootStart" )]
		int sceKernelPowerRebootStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91A77137, "sceKernelRegisterSuspendHandler" )]
		int sceKernelRegisterSuspendHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB43D1A8C, "sceKernelRegisterResumeHandler" )]
		int sceKernelRegisterResumeHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67B59042, "sceSuspendForKernel_67B59042" )]
		int sceSuspendForKernel_67B59042(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2C9640B, "sceSuspendForKernel_B2C9640B" )]
		int sceSuspendForKernel_B2C9640B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F58B1EC, "sceKernelDispatchSuspendHandlers" )]
		int sceKernelDispatchSuspendHandlers(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AB0C6F3, "sceKernelDispatchResumeHandlers" )]
		int sceKernelDispatchResumeHandlers(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 3C353D71 */
