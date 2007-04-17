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
	class SysMemUserForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "SysMemUserForUser";
			}
		}

		#endregion

		#region State Management

		public SysMemUserForUser( Kernel kernel )
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
		[BiosFunction( 0xA291F107, "sceKernelMaxFreeMemSize" )]
		// SDK location: /user/pspsysmem.h:88
		// SDK declaration: SceSize sceKernelMaxFreeMemSize();
		public int sceKernelMaxFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF919F628, "sceKernelTotalFreeMemSize" )]
		// SDK location: /user/pspsysmem.h:81
		// SDK declaration: SceSize sceKernelTotalFreeMemSize();
		public int sceKernelTotalFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6581468, "sceKPartitionMaxFreeMemSize" )]
		// manual add
		public int sceKPartitionMaxFreeMemSize( int partitionid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9697CD32, "sceKPartitionTotalFreeMemSize" )]
		// manual add
		public int sceKPartitionTotalFreeMemSize( int partitionid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x237DBD4F, "sceKernelAllocPartitionMemory" )]
		// SDK location: /user/pspsysmem.h:56
		// SDK declaration: SceUID sceKernelAllocPartitionMemory(SceUID partitionid, const char *name, int type, SceSize size, void *addr);
		public int sceKernelAllocPartitionMemory( int partitionid, int name, int type, int size, int addr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6D61D02, "sceKernelFreePartitionMemory" )]
		// SDK location: /user/pspsysmem.h:65
		// SDK declaration: int sceKernelFreePartitionMemory(SceUID blockid);
		public int sceKernelFreePartitionMemory( int blockid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D9A5BA1, "sceKernelGetBlockHeadAddr" )]
		// SDK location: /user/pspsysmem.h:74
		// SDK declaration: void * sceKernelGetBlockHeadAddr(SceUID blockid);
		public int sceKernelGetBlockHeadAddr( int blockid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FC9AE6A, "sceKernelDevkitVersion" )]
		// SDK location: /user/pspsysmem.h:104
		// SDK declaration: int sceKernelDevkitVersion();
		public int sceKernelDevkitVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF77D77CB, "sceKernelSetCompilerVersion" )]
		// manual add - check?
		public int sceKernelSetCompilerVersion( int version ){ return 0; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7591C7DB, "sceKernelSetCompiledSdkVersion" )]
		// manual add - check?
		public int sceKernelSetCompiledSdkVersion( int version ){ return 0; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13A5ABEF, "sceKernelPrintf" )]
		// manual add - printf( char* format, ... ) <-- right 2nd arg?
		public int sceKernelPrintf( int format, int varg ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - FEC8C0EB */
