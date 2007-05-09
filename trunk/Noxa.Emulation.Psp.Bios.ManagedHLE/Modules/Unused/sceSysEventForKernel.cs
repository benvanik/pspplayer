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
	class sceSysEventForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSysEventForKernel";
			}
		}

		#endregion

		#region State Management

		public sceSysEventForKernel( Kernel kernel )
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
		[BiosFunction( 0xAEB300AE, "sceKernelIsRegisterSysEventHandler" )]
		int sceKernelIsRegisterSysEventHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD9E4BB5, "sceKernelRegisterSysEventHandler" )]
		int sceKernelRegisterSysEventHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7D3FDCD, "sceKernelUnregisterSysEventHandler" )]
		int sceKernelUnregisterSysEventHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36331294, "sceKernelSysEventDispatch" )]
		int sceKernelSysEventDispatch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68D55505, "sceKernelReferSysEventHandler" )]
		int sceKernelReferSysEventHandler(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 63F7B99A */
