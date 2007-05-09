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
	class ExceptionManagerForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ExceptionManagerForKernel";
			}
		}

		#endregion

		#region State Management

		public ExceptionManagerForKernel( Kernel kernel )
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
		[BiosFunction( 0x3FB264FC, "sceKernelRegisterExceptionHandler" )]
		int sceKernelRegisterExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A837AD4, "sceKernelRegisterPriorityExceptionHandler" )]
		int sceKernelRegisterPriorityExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x565C0B0E, "sceKernelRegisterDefaultExceptionHandler" )]
		int sceKernelRegisterDefaultExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1AA6CFFA, "sceKernelReleaseExceptionHandler" )]
		int sceKernelReleaseExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF83875E, "sceKernelGetActiveDefaultExceptionHandler" )]
		int sceKernelGetActiveDefaultExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x291FF031, "sceKernelReleaseDefaultExceptionHandler" )]
		int sceKernelReleaseDefaultExceptionHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15ADC862, "sceKernelRegisterNmiHandler" )]
		int sceKernelRegisterNmiHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB15357C9, "sceKernelReleaseNmiHandler" )]
		int sceKernelReleaseNmiHandler(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4EF6CC1B */
