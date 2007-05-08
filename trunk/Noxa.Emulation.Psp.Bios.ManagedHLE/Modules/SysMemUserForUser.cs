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

		[Stateless]
		[BiosFunction( 0xA291F107, "sceKernelMaxFreeMemSize" )]
		// SDK location: /user/pspsysmem.h:88
		// SDK declaration: SceSize sceKernelMaxFreeMemSize();
		public int sceKernelMaxFreeMemSize()
		{
			// This is wrong?!
			return ( int )_kernel.Partitions[ 2 ].FreeSize / 2;
		}

		[Stateless]
		[BiosFunction( 0xF919F628, "sceKernelTotalFreeMemSize" )]
		// SDK location: /user/pspsysmem.h:81
		// SDK declaration: SceSize sceKernelTotalFreeMemSize();
		public int sceKernelTotalFreeMemSize()
		{
			// This is wrong!
			return ( int )_kernel.Partitions[ 2 ].FreeSize;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6581468, "sceKPartitionMaxFreeMemSize" )]
		// manual add
		public int sceKPartitionMaxFreeMemSize( int partitionid )
		{
			KPartition partition = _kernel.Partitions[ partitionid ];
			Debug.Assert( partition != null );
			if( partition == null )
				return -1;
			return ( int )partition.Size;
		}

		[Stateless]
		[BiosFunction( 0x9697CD32, "sceKPartitionTotalFreeMemSize" )]
		// manual add
		public int sceKPartitionTotalFreeMemSize( int partitionid )
		{
			KPartition partition = _kernel.Partitions[ partitionid ];
			Debug.Assert( partition != null );
			if( partition == null )
				return -1;
			return ( int )partition.FreeSize;
		}

		[Stateless]
		[BiosFunction( 0x237DBD4F, "sceKernelAllocPartitionMemory" )]
		// SDK location: /user/pspsysmem.h:56
		// SDK declaration: SceUID sceKernelAllocPartitionMemory(SceUID partitionid, const char *name, int type, SceSize size, void *addr);
		public int sceKernelAllocPartitionMemory( int partitionid, int name, int type, int size, int addr )
		{
			KPartition partition = _kernel.Partitions[ partitionid ];
			Debug.Assert( partition != null );
			if( partition == null )
				return -1;

			KMemoryBlock block = partition.Allocate( ( KAllocType )type, ( uint )addr, ( uint )size );
			Debug.Assert( block != null );
			if( block == null )
				return -1;

			if( name != 0 )
				block.Name = _kernel.ReadString( ( uint )name );
			_kernel.AddHandle( block );

			return ( int )block.UID;
		}

		[Stateless]
		[BiosFunction( 0xB6D61D02, "sceKernelFreePartitionMemory" )]
		// SDK location: /user/pspsysmem.h:65
		// SDK declaration: int sceKernelFreePartitionMemory(SceUID blockid);
		public int sceKernelFreePartitionMemory( int blockid )
		{
			KMemoryBlock block = _kernel.GetHandle<KMemoryBlock>( blockid );
			if( block == null )
				return -1;

			block.Partition.Free( block );
			_kernel.RemoveHandle( block.UID );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x9D9A5BA1, "sceKernelGetBlockHeadAddr" )]
		// SDK location: /user/pspsysmem.h:74
		// SDK declaration: void * sceKernelGetBlockHeadAddr(SceUID blockid);
		public int sceKernelGetBlockHeadAddr( int blockid )
		{
			KMemoryBlock block = _kernel.GetHandle<KMemoryBlock>( blockid );
			if( block == null )
				return -1;
			return ( int )block.Address;
		}

		[Stateless]
		[BiosFunction( 0x3FC9AE6A, "sceKernelDevkitVersion" )]
		// SDK location: /user/pspsysmem.h:104
		// SDK declaration: int sceKernelDevkitVersion();
		public int sceKernelDevkitVersion()
		{
			// 0x01000300 on v1.00 unit
			// 0x01050001 on v1.50 unit
			Debug.WriteLine( "sceKernelDevkitVersion: game requested devkit version - make sure it isn't doing something tricky!" );
			return 0x01050001;
		}

		[Stateless]
		[BiosFunction( 0xF77D77CB, "sceKernelSetCompilerVersion" )]
		// manual add - check?
		public int sceKernelSetCompilerVersion( int version )
		{
			//02080010
			Version v = new Version(
				( version >> 24 ) & 0xFF,
				( version >> 16 ) & 0xFF,
				( version >> 8 ) & 0xFF,
				version & 0xFF );
			Debug.WriteLine( string.Format( "sceKernelSetCompilerVersion: set to version {0:X8} ({1})", version, v.ToString() ) );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x7591C7DB, "sceKernelSetCompiledSdkVersion" )]
		// manual add - check?
		public int sceKernelSetCompiledSdkVersion( int version )
		{
			Version v = new Version(
				( version >> 24 ) & 0xFF,
				( version >> 16 ) & 0xFF,
				( version >> 8 ) & 0xFF,
				version & 0xFF );
			Debug.WriteLine( string.Format( "sceKernelSetCompiledSdkVersion: set to version {0:X8} ({1})", version, v.ToString() ) );
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13A5ABEF, "sceKernelPrintf" )]
		// manual add - printf( char* format, ... ) <-- right 2nd arg?
		public int sceKernelPrintf( int format, int varg ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - FEC8C0EB */