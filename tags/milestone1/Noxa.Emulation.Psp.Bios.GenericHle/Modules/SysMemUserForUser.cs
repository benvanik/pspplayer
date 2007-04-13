// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class SysMemUserForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public SysMemUserForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "SysMemUserForUser";
			}
		}

		#endregion

		[BiosStub( 0xa291f107, "sceKernelMaxFreeMemSize", true, 0 )]
		public int sceKernelMaxFreeMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			uint size = 0;
			for( int n = 0; n < _kernel.Partitions.Length; n++ )
				size += _kernel.Partitions[ n ].Size;

			// SceSize
			return ( int )size;
		}

		[BiosStub( 0xf919f628, "sceKernelTotalFreeMemSize", true, 0 )]
		public int sceKernelTotalFreeMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			uint size = 0;
			for( int n = 0; n < _kernel.Partitions.Length; n++ )
				size += _kernel.Partitions[ n ].FreeSize;
		
			// SceSize
			return ( int )size;
		}

		[BiosStub( 0x13a5abef, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown0( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa6848df8, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe6581468, "sceKernelPartitionMaxFreeMemSize", true, 1 )]
		public int sceKernelPartitionMaxFreeMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int pid

			KernelPartition partition = _kernel.Partitions[ a0 ];
			
			// SceSize
			return ( int )partition.Size;
		}

		[BiosStub( 0x9697cd32, "sceKernelPartitionTotalFreeMemSize", true, 1 )]
		public int sceKernelPartitionTotalFreeMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int pid

			KernelPartition partition = _kernel.Partitions[ a0 ];
			
			// SceSize
			return ( int )partition.FreeSize;
		}

		[BiosStub( 0x237dbd4f, "sceKernelAllocPartitionMemory", true, 5 )]
		public int sceKernelAllocPartitionMemory( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID partitionid
			// a1 = const char *name
			// a2 = int type
			// a3 = SceSize size
			// sp[0] = void *addr
			int a4 = memory.ReadWord( sp + 0 );

			KernelPartition partition = _kernel.Partitions[ a0 ];

			KernelAllocationType allocType;
			switch( a2 )
			{
				case 0: // Low (allocate at lowest)
					allocType = KernelAllocationType.Low;
					break;
				case 1: // High (allocate at highest)
					allocType = KernelAllocationType.High;
					break;
				default:
				case 2: // Addr (allocate at specified address)
					allocType = KernelAllocationType.SpecificAddress;
					break;
			}
			KernelMemoryBlock block = partition.Allocate( allocType, ( uint )a4, ( uint )a3 );
			block.Name = Kernel.ReadString( memory, a1 );

			_kernel.AddHandle( block );
			
			// SceUID
			return block.Uid;
		}

		[BiosStub( 0xb6d61d02, "sceKernelFreePartitionMemory", true, 1 )]
		public int sceKernelFreePartitionMemory( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID blockid

			KernelMemoryBlock block = _kernel.GetHandle( a0 ) as KernelMemoryBlock;
			if( block == null )
				return -1;

			block.Partition.Free( block );
			_kernel.RemoveHandle( block );
			
			// int
			return 0;
		}

		[BiosStub( 0x9d9a5ba1, "sceKernelGetBlockHeadAddr", true, 1 )]
		public int sceKernelGetBlockHeadAddr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID blockid

			KernelMemoryBlock block = _kernel.GetHandle( a0 ) as KernelMemoryBlock;
			if( block == null )
				return -1;
			
			// void *
			return ( int )block.Address;
		}

		[BiosStub( 0xfc114573, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7591c7db, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf77d77cb, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3fc9ae6a, "sceKernelDevkitVersion", true, 0 )]
		public int sceKernelDevkitVersion( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// 0x01000300 on v1.00 unit
			// 0x01050001 on v1.50 unit

			Debug.WriteLine( "SysMemUserForUser::sceKernelDevkitVersion: game requested devkit version - make sure it isn't doing something tricky!" );

			// int
			return 0x01050001;
		}
	}
}
