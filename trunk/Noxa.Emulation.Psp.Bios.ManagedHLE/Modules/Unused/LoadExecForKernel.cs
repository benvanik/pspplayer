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
	class LoadExecForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "LoadExecForKernel";
			}
		}

		#endregion

		#region State Management

		public LoadExecForKernel( Kernel kernel )
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
		[BiosFunction( 0xBD2F1094, "sceKernelLoadExec" )]
		int sceKernelLoadExec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AC9954B, "sceKernelExitGameWithStatus" )]
		int sceKernelExitGameWithStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05572A5F, "sceKernelExitGame" )]
		int sceKernelExitGame(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC085B9E, "sceKernelLoadExecVSHFromHost" )]
		int sceKernelLoadExecVSHFromHost(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B97BDB3, "sceKernelLoadExecVSHDisc" )]
		int sceKernelLoadExecVSHDisc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x821BE114, "sceKernelLoadExecVSHDiscUpdater" )]
		int sceKernelLoadExecVSHDiscUpdater(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x015DA036, "sceKernelLoadExecBufferVSHUsbWlan" )]
		int sceKernelLoadExecBufferVSHUsbWlan(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F41E75E, "sceKernelLoadExecBufferVSHUsbWlanDebug" )]
		int sceKernelLoadExecBufferVSHUsbWlanDebug(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31DF42BF, "sceKernelLoadExecVSHMs1" )]
		int sceKernelLoadExecVSHMs1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28D0D249, "sceKernelLoadExecVSHMs2" )]
		int sceKernelLoadExecVSHMs2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x70901231, "sceKernelLoadExecVSHMs3" )]
		int sceKernelLoadExecVSHMs3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3D5E142, "sceKernelExitVSHVSH" )]
		int sceKernelExitVSHVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62C459E1, "sceKernelLoadExecBufferVSHPlain" )]
		int sceKernelLoadExecBufferVSHPlain(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x918782E8, "sceKernelLoadExecBufferVSHFromHost" )]
		int sceKernelLoadExecBufferVSHFromHost(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB28E9B7, "sceKernelLoadExecBufferPlain0" )]
		int sceKernelLoadExecBufferPlain0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71A1D738, "sceKernelLoadExecBufferPlain" )]
		int sceKernelLoadExecBufferPlain(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D5C75BE, "sceKernelLoadExecFromHost" )]
		int sceKernelLoadExecFromHost(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AC57943, "sceKernelRegisterExitCallback" )]
		int sceKernelRegisterExitCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD9739B89, "sceKernelUnregisterExitCallback" )]
		int sceKernelUnregisterExitCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x659188E1, "sceKernelCheckExitCallback" )]
		int sceKernelCheckExitCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62A27008, "sceKernelInvokeExitCallback" )]
		int sceKernelInvokeExitCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B7C47EF, "sceKernelLoadExecVSHDiscDebug" )]
		int sceKernelLoadExecVSHDiscDebug(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9F239BC9 */
