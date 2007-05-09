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
	class LoadCoreForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "LoadCoreForKernel";
			}
		}

		#endregion

		#region State Management

		public LoadCoreForKernel( Kernel kernel )
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
		[BiosFunction( 0xACE23476, "sceKernelCheckPspConfig" )]
		int sceKernelCheckPspConfig(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BE1421C, "sceKernelCheckExecFile" )]
		int sceKernelCheckExecFile(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF983EF2, "sceKernelProbeExecutableObject" )]
		int sceKernelProbeExecutableObject(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7068E6BA, "sceKernelLoadExecutableObject" )]
		int sceKernelLoadExecutableObject(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4D6FECC, "sceKernelApplyElfRelSection" )]
		int sceKernelApplyElfRelSection(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54AB2675, "sceKernelApplyPspRelSection" )]
		int sceKernelApplyPspRelSection(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2952F5AC, "sceKernelDcacheWBinvAll" )]
		int sceKernelDcacheWBinvAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8779AC6, "sceKernelIcacheClearAll" )]
		int sceKernelIcacheClearAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x99A695F0, "sceKernelRegisterLibrary" )]
		int sceKernelRegisterLibrary(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5873A31F, "sceKernelRegisterLibraryForUser" )]
		int sceKernelRegisterLibraryForUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B464512, "sceKernelReleaseLibrary" )]
		int sceKernelReleaseLibrary(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9BAF90F6, "sceKernelCanReleaseLibrary" )]
		int sceKernelCanReleaseLibrary(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E760DBA, "sceKernelLinkLibraryEntries" )]
		int sceKernelLinkLibraryEntries(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DE1F600, "sceKernelLinkLibraryEntriesForUser" )]
		int sceKernelLinkLibraryEntriesForUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDA1B09AA, "sceKernelUnLinkLibraryEntries" )]
		int sceKernelUnLinkLibraryEntries(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC99DD47A, "sceKernelQueryLoadCoreCB" )]
		int sceKernelQueryLoadCoreCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x616FCCCD, "sceKernelSetBootCallbackLevel" )]
		int sceKernelSetBootCallbackLevel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF32A2940, "sceKernelModuleFromUID" )]
		int sceKernelModuleFromUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD0F3BAC, "sceKernelCreateModule" )]
		int sceKernelCreateModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B2371C2, "sceKernelDeleteModule" )]
		int sceKernelDeleteModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7320D964, "sceKernelModuleAssign" )]
		int sceKernelModuleAssign(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44B292AB, "sceKernelAllocModule" )]
		int sceKernelAllocModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD61D4D5, "sceKernelFreeModule" )]
		int sceKernelFreeModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE7C6E76, "sceKernelRegisterModule" )]
		int sceKernelRegisterModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74CF001A, "sceKernelReleaseModule" )]
		int sceKernelReleaseModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF8A41B1, "sceKernelFindModuleByName" )]
		int sceKernelFindModuleByName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB8AE27D, "sceKernelFindModuleByAddress" )]
		int sceKernelFindModuleByAddress(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCCE4A157, "sceKernelFindModuleByUID" )]
		int sceKernelFindModuleByUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82CE54ED, "sceKernelModuleCount" )]
		int sceKernelModuleCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0584F0C, "sceKernelGetModuleList" )]
		int sceKernelGetModuleList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x929B5C69, "sceKernelGetModuleListWithAlloc" )]
		int sceKernelGetModuleListWithAlloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D8A8ACE, "sceKernelAssignModule" )]
		int sceKernelAssignModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAFF947D4, "sceKernelCreateAssignModule" )]
		int sceKernelCreateAssignModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05D915DB, "sceKernelGetModuleIdListForKernel" )]
		int sceKernelGetModuleIdListForKernel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52A86C21, "sceKernelGetModuleFromUID" )]
		int sceKernelGetModuleFromUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5248A98F, "sceKernelLoadModuleBootLoadCore" )]
		int sceKernelLoadModuleBootLoadCore(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F8E2B3ED */
