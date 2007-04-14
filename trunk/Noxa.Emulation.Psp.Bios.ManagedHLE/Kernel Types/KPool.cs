// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	abstract class KPool : KHandle, IDisposable
	{
		public string Name;
		public uint Attributes;
		public uint BlockSize;
		public KPartition Partition;

		public FastLinkedList<KMemoryBlock> Blocks;
		public FastLinkedList<KMemoryBlock> UsedBlocks;
		public FastLinkedList<KMemoryBlock> FreeBlocks;

		public FastLinkedList<KThread> WaitingThreads;

		public KPool( KPartition partition, string name, uint attributes, uint blockSize )
		{
			Debug.Assert( partition != null );
			Debug.Assert( name != null );
			Debug.Assert( blockSize > 0 );

			Name = name;
			Attributes = attributes;
			BlockSize = blockSize;
			Partition = partition;

			Blocks = new FastLinkedList<KMemoryBlock>();
			UsedBlocks = new FastLinkedList<KMemoryBlock>();
			FreeBlocks = new FastLinkedList<KMemoryBlock>();

			WaitingThreads = new FastLinkedList<KThread>();
		}

		~KPool()
		{
			this.Dispose();
		}
		
		public void Dispose()
		{
			GC.SuppressFinalize( this );

			LinkedListEntry<KMemoryBlock> e = Blocks.HeadEntry;
			while( e != null )
			{
				Partition.Free( e.Value );
				e = e.Next;
			}
			Blocks.Clear();
		}

		public KMemoryBlock Allocate()
		{
			if( FreeBlocks.Count == 0 )
			{
				bool allocated = this.AllocateBlocks();
				// Allocation will fail if we are fixed and out of blocks!
				if( allocated == false )
					return null;
				Debug.Assert( FreeBlocks.Count > 0 );
			}

			KMemoryBlock block = FreeBlocks.Dequeue();
			UsedBlocks.Enqueue( block );
			return block;
		}

		private void WakeWaiter()
		{
			KThread waiter = WaitingThreads.Dequeue();
			if( waiter == null )
				return;
			waiter.Wake();
		}

		public void Free( int address )
		{
			LinkedListEntry<KMemoryBlock> e = UsedBlocks.HeadEntry;
			while( e != null )
			{
				if( e.Value.Address == address )
				{
					UsedBlocks.Remove( e );
					FreeBlocks.Enqueue( e.Value );
					this.WakeWaiter();
					return;
				}
				e = e.Next;
			}
		}

		public void Free( KMemoryBlock block )
		{
			Debug.Assert( block != null );

			LinkedListEntry<KMemoryBlock> e = UsedBlocks.Find( block );
			Debug.Assert( e != null );
			if( e != null )
			{
				UsedBlocks.Remove( e );
				FreeBlocks.Enqueue( block );
				this.WakeWaiter();
			}
		}

		protected virtual bool AllocateBlocks()
		{
			return false;
		}
	}

	class KFixedPool : KPool
	{
		public KFixedPool( KPartition partition, string name, uint attributes, uint blockSize, int blockCount )
			: base( partition, name, attributes, blockSize )
		{
			Debug.Assert( blockCount > 0 );
			for( int n = 0; n < blockCount; n++ )
			{
				KMemoryBlock block = partition.Allocate( KAllocType.Low, 0, blockSize );
				Debug.Assert( block != null );
				Blocks.Enqueue( block );
				FreeBlocks.Enqueue( block );
			}
		}
	}

	class KVariablePool : KPool
	{
		public const int GrowthCount = 64;

		public KVariablePool( KPartition partition, string name, uint attributes, uint blockSize )
			: base( partition, name, attributes, blockSize )
		{
		}

		protected override bool AllocateBlocks()
		{
			for( int n = 0; n < GrowthCount; n++ )
			{
				KMemoryBlock block = Partition.Allocate( KAllocType.Low, 0, BlockSize );
				Debug.Assert( block != null );
				Blocks.Enqueue( block );
				FreeBlocks.Enqueue( block );
			}
			return true;
		}
	}
}
