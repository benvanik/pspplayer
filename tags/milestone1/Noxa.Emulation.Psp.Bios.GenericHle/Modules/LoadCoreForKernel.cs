// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class LoadCoreForKernel : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public LoadCoreForKernel( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "LoadCoreForKernel";
			}
		}

		#endregion

		[BiosStub( 0xACE23476, "sceKernelCheckPspConfig", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCheckPspConfig( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xA6D40F56, "LoadCoreForKernel_0xA6D40F56", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown0( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7BE1421C, "sceKernelCheckExecFile", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCheckExecFile( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xBF983EF2, "sceKernelProbeExecutableObject", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelProbeExecutableObject( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7068E6BA, "sceKernelLoadExecutableObject", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelLoadExecutableObject( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xB4D6FECC, "sceKernelApplyElfRelSection", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelApplyElfRelSection( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x54AB2675, "sceKernelApplyPspRelSection", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelApplyPspRelSection( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2952F5AC, "sceKernelDcacheWBinvAll", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDcacheWBinvAll( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4440853B, "LoadCoreForKernel_0x4440853B", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xD8779AC6, "sceKernelIcacheClearAll", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelIcacheClearAll( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x99A695F0, "sceKernelRegisterLibrary", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterLibrary( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5873A31F, "sceKernelRegisterLibraryForUser", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterLibraryForUser( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0B464512, "sceKernelReleaseLibrary", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseLibrary( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9BAF90F6, "sceKernelCanReleaseLibrary", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCanReleaseLibrary( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0E760DBA, "sceKernelLinkLibraryEntries", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelLinkLibraryEntries( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0DE1F600, "sceKernelLinkLibraryEntriesForUser", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelLinkLibraryEntriesForUser( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5248A98F, "sceKernelLoadModuleBootLoadCore", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelLoadModuleBootLoadCore( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xDA1B09AA, "sceKernelUnLinkLibraryEntries", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelUnLinkLibraryEntries( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xC99DD47A, "sceKernelQueryLoadCoreCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelQueryLoadCoreCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7E63F86D, "LoadCoreForKernel_0x7E63F86D", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xB370DF29, "LoadCoreForKernel_0xB370DF29", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x616FCCCD, "sceKernelSetBootCallbackLevel", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetBootCallbackLevel( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x52A86C21, "sceKernelGetModuleFromUID", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleFromUID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xF32A2940, "sceKernelModuleFromUID", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelModuleFromUID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3BB7AC18, "LoadCoreForKernel_0x3BB7AC18", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xCD0F3BAC, "sceKernelCreateModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6B2371C2, "sceKernelDeleteModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x8D8A8ACE, "sceKernelAssignModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAssignModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7320D964, "sceKernelModuleAssign", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelModuleAssign( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xA8E2D53D, "LoadCoreForKernel_0xA8E2D53D", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xAFF947D4, "sceKernelCreateAssignModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateAssignModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x44B292AB, "sceKernelAllocModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAllocModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xBD61D4D5, "sceKernelFreeModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelFreeModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xAE7C6E76, "sceKernelRegisterModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x74CF001A, "sceKernelReleaseModule", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xCF8A41B1, "sceKernelFindModuleByName", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelFindModuleByName( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xFB8AE27D, "sceKernelFindModuleByAddress", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelFindModuleByAddress( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = unsigned int addr

			// SceModule *
			return 0;
		}

		[BiosStub( 0xCCE4A157, "sceKernelFindModuleByUID", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelFindModuleByUID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x929B5C69, "sceKernelGetModuleListWithAlloc", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleListWithAlloc( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xC0913394, "LoadCoreForKernel_0xC0913394", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x05D915DB, "sceKernelGetModuleIdListForKernel", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleIdListForKernel( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x82CE54ED, "sceKernelModuleCount", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelModuleCount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void

			// int
			return 0;
		}

		[BiosStub( 0xC0584F0C, "sceKernelGetModuleList", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleList( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int readbufsize
			// a1 = SceUID *readbuf

			// int
			return 0;
		}

		[BiosStub( 0xFA3101A4, "LoadCoreForKernel_0xFA3101A4", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown7( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
