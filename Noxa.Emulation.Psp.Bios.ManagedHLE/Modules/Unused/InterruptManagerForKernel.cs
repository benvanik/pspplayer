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
	class InterruptManagerForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "InterruptManagerForKernel";
			}
		}

		#endregion

		#region State Management

		public InterruptManagerForKernel( Kernel kernel )
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
		[BiosFunction( 0x092968F4, "sceKernelCpuSuspendIntr" )]
		int sceKernelCpuSuspendIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F10D406, "sceKernelCpuResumeIntr" )]
		int sceKernelCpuResumeIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B84732D, "sceKernelCpuResumeIntrWithSync" )]
		int sceKernelCpuResumeIntrWithSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE28C6D9, "sceKernelIsIntrContext" )]
		int sceKernelIsIntrContext(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53991063, "InterruptManagerForKernel_53991063" )]
		int InterruptManagerForKernel_53991063(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x468BC716, "sceKernelGetInterruptExitCount" )]
		int sceKernelGetInterruptExitCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43CD40EF, "ReturnToThread" )]
		int ReturnToThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x85F7766D, "SaveThreadContext" )]
		int SaveThreadContext(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02314986, "sceKernelCpuEnableIntr" )]
		int sceKernelCpuEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x00B6B0F3, "QueryInterruptManCB" )]
		int QueryInterruptManCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58DD8978, "sceKernelRegisterIntrHandler" )]
		int sceKernelRegisterIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15894D0B, "InterruptManagerForKernel_15894D0B" )]
		int InterruptManagerForKernel_15894D0B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF987B1F0, "sceKernelReleaseIntrHandler" )]
		int sceKernelReleaseIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5A15B30, "sceKernelSetIntrLevel" )]
		int sceKernelSetIntrLevel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43A7BBDC, "InterruptManagerForKernel_43A7BBDC" )]
		int InterruptManagerForKernel_43A7BBDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02475AAF, "sceKernelIsInterruptOccurred" )]
		int sceKernelIsInterruptOccurred(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D6E7305, "sceKernelEnableIntr" )]
		int sceKernelEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x750E2507, "sceKernelSuspendIntr" )]
		int sceKernelSuspendIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD774BA45, "sceKernelDisableIntr" )]
		int sceKernelDisableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x494D6D2B, "sceKernelResumeIntr" )]
		int sceKernelResumeIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CD783A1, "RegisterContextHooks" )]
		int RegisterContextHooks(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55242A8B, "ReleaseContextHooks" )]
		int ReleaseContextHooks(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27BC9A45, "UnSupportIntr" )]
		int UnSupportIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E224D66, "SupportIntr" )]
		int SupportIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x272766F8, "sceKernelRegisterDebuggerIntrHandler" )]
		int sceKernelRegisterDebuggerIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB386A459, "sceKernelReleaseDebuggerIntrHandler" )]
		int sceKernelReleaseDebuggerIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDC86B64, "sceKernelCallSubIntrHandler" )]
		int sceKernelCallSubIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6878EB6, "sceKernelGetUserIntrStack" )]
		int sceKernelGetUserIntrStack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4454E44, "sceKernelCallUserIntrHandler" )]
		int sceKernelCallUserIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA04A2B9, "sceKernelRegisterSubIntrHandler" )]
		int sceKernelRegisterSubIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD61E6961, "sceKernelReleaseSubIntrHandler" )]
		int sceKernelReleaseSubIntrHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB8E22EC, "sceKernelEnableSubIntr" )]
		int sceKernelEnableSubIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A389411, "sceKernelDisableSubIntr" )]
		int sceKernelDisableSubIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5CB5A78B, "sceKernelSuspendSubIntr" )]
		int sceKernelSuspendSubIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7860E0DC, "sceKernelResumeSubIntr" )]
		int sceKernelResumeSubIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC4374B8, "sceKernelIsSubInterruptOccurred" )]
		int sceKernelIsSubInterruptOccurred(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2E8363F, "QueryIntrHandlerInfo" )]
		int QueryIntrHandlerInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30C08374, "sceKernelGetCpuClockCounter" )]
		int sceKernelGetCpuClockCounter(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35634A64, "sceKernelGetCpuClockCounterWide" )]
		int sceKernelGetCpuClockCounterWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2DC9709B, "_sceKernelGetCpuClockCounterLow" )]
		int _sceKernelGetCpuClockCounterLow(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9E652A9, "_sceKernelGetCpuClockCounterHigh" )]
		int _sceKernelGetCpuClockCounterHigh(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FC68A56, "sceKernelSetPrimarySyscallHandler" )]
		int sceKernelSetPrimarySyscallHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4D443F3, "sceKernelRegisterSystemCallTable" )]
		int sceKernelRegisterSystemCallTable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8B61808B, "sceKernelQuerySystemCall" )]
		int sceKernelQuerySystemCall(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 0CBB21ED */
