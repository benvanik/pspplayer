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
	class ModuleMgrForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ModuleMgrForKernel";
			}
		}

		#endregion

		#region State Management

		public ModuleMgrForKernel( Kernel kernel )
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
		[BiosFunction( 0xABE84F8A, "sceKernelLoadModuleBufferWithApitype" )]
		int sceKernelLoadModuleBufferWithApitype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA889C07, "sceKernelLoadModuleBuffer" )]
		int sceKernelLoadModuleBuffer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7F46618, "sceKernelLoadModuleByID" )]
		int sceKernelLoadModuleByID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x437214AE, "sceKernelLoadModuleWithApitype" )]
		int sceKernelLoadModuleWithApitype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x977DE386, "sceKernelLoadModule" )]
		int sceKernelLoadModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x710F61B5, "sceKernelLoadModuleMs" )]
		int sceKernelLoadModuleMs(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91B87FAE, "sceKernelLoadModuleVSHByID" )]
		int sceKernelLoadModuleVSHByID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4370E7C, "sceKernelLoadModuleVSH" )]
		int sceKernelLoadModuleVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23425E93, "sceKernelLoadModuleVSHPlain" )]
		int sceKernelLoadModuleVSHPlain(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9275D98, "sceKernelLoadModuleBufferUsbWlan" )]
		int sceKernelLoadModuleBufferUsbWlan(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0CAC59E, "sceKernelLoadModuleBufferVSH" )]
		int sceKernelLoadModuleBufferVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50F0C1EC, "sceKernelStartModule" )]
		int sceKernelStartModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1FF982A, "sceKernelStopModule" )]
		int sceKernelStopModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E0911AA, "sceKernelUnloadModule" )]
		int sceKernelUnloadModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD675EBB8, "sceKernelSelfStopUnloadModule" )]
		int sceKernelSelfStopUnloadModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC1D3699, "sceKernelStopUnloadSelfModule" )]
		int sceKernelStopUnloadSelfModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04B7BD22, "sceKernelSearchModuleByName" )]
		int sceKernelSearchModuleByName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54D9E02E, "sceKernelSearchModuleByAddress" )]
		int sceKernelSearchModuleByAddress(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x748CBED9, "sceKernelQueryModuleInfo" )]
		int sceKernelQueryModuleInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F0CC575, "sceKernelRebootBeforeForUser" )]
		int sceKernelRebootBeforeForUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB49FFB9E, "sceKernelRebootBeforeForKernel" )]
		int sceKernelRebootBeforeForKernel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x644395E2, "sceKernelGetModuleIdList" )]
		int sceKernelGetModuleIdList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8B73127, "sceKernelGetModuleIdByAddress" )]
		int sceKernelGetModuleIdByAddress(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0A26395, "sceKernelGetModuleId" )]
		int sceKernelGetModuleId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA6E8C1F5, "sceKernelRebootPhaseForKernel" )]
		int sceKernelRebootPhaseForKernel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB77DC5A, "sceKernelLoadModuleBootInitConfig" )]
		int sceKernelLoadModuleBootInitConfig(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 930169F0 */
