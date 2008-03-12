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
	enum KAllocType
	{
		/// <summary>
		/// Allocate at lowest address possible.
		/// </summary>
		Low = 0,
		/// <summary>
		/// Allocate at highest address possible.
		/// </summary>
		High = 1,
		/// <summary>
		/// Allocate at the specified address.
		/// </summary>
		Specific = 2,
		/// <summary>
		/// Allocate at lowest address possible, aligned.
		/// </summary>
		LowAligned = 3,
		/// <summary>
		/// Allocate at highest address possible, aligned.
		/// </summary>
		HighAligned = 4,
	}

	class KPartition
	{
		public Kernel Kernel;

		public uint BaseAddress;
		public uint Size;
		public uint UpperBound;
		public uint FreeSize;

		public FastLinkedList<KMemoryBlock> Blocks;
		public FastLinkedList<KMemoryBlock> FreeList;

		public KPartition( Kernel kernel, uint baseAddress, uint size )
		{
			Kernel = kernel;

			BaseAddress = baseAddress;
			Size = size;
			UpperBound = baseAddress + size;
			FreeSize = size;

			Blocks = new FastLinkedList<KMemoryBlock>();
			FreeList = new FastLinkedList<KMemoryBlock>();

			// Initial empty block
			KMemoryBlock block = new KMemoryBlock( this, baseAddress, size, true );
			Blocks.Enqueue( block );
			FreeList.Enqueue( block );
		}

		public KMemoryBlock Allocate( string name, KAllocType type, uint address, uint size )
		{
			KMemoryBlock newBlock = null;

			// Round size up to the next word
			//if( ( size & 0x3 ) != 0 )
			//    size += 4 - ( size & 0x3 );

			if( ( type == KAllocType.LowAligned ) ||
				( type == KAllocType.HighAligned ) )
			{
				// TODO: align at 'address' (like 4096, etc)
				address = 0;

				// Other logic is the same
				if( type == KAllocType.LowAligned )
					type = KAllocType.Low;
				else if( type == KAllocType.HighAligned )
					type = KAllocType.High;
			}

			// Quick check to see if we have the space free
			Debug.Assert( FreeSize >= size );
			if( FreeSize < size )
				return null;

			switch( type )
			{
				case KAllocType.Specific:
					{
						Debug.Assert( address != 0 );
						LinkedListEntry<KMemoryBlock> e = FreeList.HeadEntry;
						while( e != null )
						{
							if( ( address >= e.Value.Address ) &&
								( address < e.Value.UpperBound ) )
							{
								newBlock = this.SplitBlock( e.Value, address, size );
								break;
							}
							e = e.Next;
						}
					}
					break;
				case KAllocType.Low:
					{
						KMemoryBlock targetBlock = null;
						uint maxContig = 0;
						if( address != 0 )
						{
							// Specified lower limit, find the first block that fits
							LinkedListEntry<KMemoryBlock> e = FreeList.HeadEntry;
							while( e != null )
							{
								if( ( address >= e.Value.Address ) &&
									( address < e.Value.UpperBound ) )
								{
									targetBlock = e.Value;
									break;
								}
								maxContig = Math.Max( maxContig, e.Value.Size );
								e = e.Next;
							}
						}
						else
						{
							// No lower limit - pick first free block that fits
							LinkedListEntry<KMemoryBlock> e = FreeList.HeadEntry;
							while( e != null )
							{
								if( e.Value.Size >= size )
								{
									targetBlock = e.Value;
									break;
								}
								maxContig = Math.Max( maxContig, e.Value.Size );
								e = e.Next;
							}
						}
						Debug.Assert( targetBlock != null );
						if( targetBlock == null )
						{
							// Try again with a smaller size
							Log.WriteLine( Verbosity.Critical, Feature.Bios, "KPartition::Allocate could not find enough space" );
							//return this.Allocate( KAllocType.Maximum, 0, maxContig );
							return null;
						}
						Debug.Assert( targetBlock.Size >= size );
						if( targetBlock.Size < size )
							return null;
						newBlock = this.SplitBlock( targetBlock,
							( address != 0 ) ? address : targetBlock.Address,
							size );
					}
					break;
				case KAllocType.High:
					{
						Debug.Assert( address == 0 );
						KMemoryBlock targetBlock = null;
						LinkedListEntry<KMemoryBlock> e = FreeList.TailEntry;
						while( e != null )
						{
							if( e.Value.Size >= size )
							{
								targetBlock = e.Value;
								break;
							}
							e = e.Previous;
						}
						Debug.Assert( targetBlock != null );
						if( targetBlock == null )
						{
							// Try again with a smaller size
							Log.WriteLine( Verbosity.Critical, Feature.Bios, "KPartition::Allocate could not find enough space" );
							//return this.Allocate( KAllocType.Maximum, 0, maxContig );
							return null;
						}
						Debug.Assert( ( int )targetBlock.UpperBound - ( int )size >= 0 );
						newBlock = this.SplitBlock( targetBlock, targetBlock.UpperBound - size, size );
					}
					break;
			}

			if( newBlock != null )
			{
				newBlock.Name = name;
				newBlock.IsFree = false;
				FreeSize -= size;
			}
			this.Kernel.PrintMemoryInfo();
			return newBlock;
		}

		public void Free( KMemoryBlock block )
		{
			Debug.Assert( block != null );
			Debug.Assert( block.IsFree == false );

			block.UID = 0;
			block.IsFree = true;
			FreeSize += block.Size;

			LinkedListEntry<KMemoryBlock> entry = Blocks.Find( block );
			Debug.Assert( entry != null );

			// Attempt to coalesce in to previous and or next blocks
			KMemoryBlock merged = null;
			LinkedListEntry<KMemoryBlock> prev = entry.Previous;
			LinkedListEntry<KMemoryBlock> next = entry.Next;
			if( ( prev != null ) &&
				( prev.Value.IsFree == true ) )
			{
				// Merge in to previous (kill us)
				Blocks.Remove( entry );
				prev.Value.Size += block.Size;
				merged = prev.Value;
			}
			if( ( next != null ) &&
				( next.Value.IsFree == true ) )
			{
				// Merge next in to us (kill them)
				// This takes in to account whether or not we already merged
				KMemoryBlock nextBlock = next.Value;
				Blocks.Remove( next );
				FreeList.Remove( nextBlock );
				if( merged == null )
					block.Size += nextBlock.Size;
				else
					merged.Size += nextBlock.Size;
			}

			if( merged == null )
			{
				// Didn't merge - put back in free list
				this.AddToFreeList( block );
			}
		}

		private void AddToFreeList( KMemoryBlock block )
		{
			// Inserts in to free list at the right place
			if( FreeList.Count == 0 )
				FreeList.Enqueue( block );
			else
			{
				LinkedListEntry<KMemoryBlock> e = FreeList.HeadEntry;
				while( e != null )
				{
					if( e.Value.Address > block.Address )
					{
						// Found next block - insert before
						FreeList.InsertBefore( block, e );
						return;
					}
					e = e.Next;
				}
				// Didn't find - add to tail
				FreeList.Enqueue( block );
			}
		}

		private KMemoryBlock SplitBlock( KMemoryBlock block, uint address, uint size )
		{
			Debug.Assert( size > 0 );

			KMemoryBlock newBlock = new KMemoryBlock( this, address, size, false );

			LinkedListEntry<KMemoryBlock> blockEntry = Blocks.Find( block );
			Debug.Assert( blockEntry != null );

			if( address == block.Address )
			{
				// Bottom up - put right before free and shift free up
				block.Address += size;
				block.Size -= size;

				Blocks.InsertBefore( newBlock, blockEntry );
			}
			else if( address == block.UpperBound - size )
			{
				// Top down - put right after and free shift free down
				block.Size -= size;

				Blocks.InsertAfter( newBlock, blockEntry );
			}
			else
			{
				// Middle - need a real split
				uint originalSize = block.Size;
				block.Size = newBlock.Address - block.Address;
				if( block.Size == 0 )
				{
					// Special case of block replacing block
					Blocks.InsertAfter( newBlock, blockEntry );
					// block will be removed below
				}
				else
				{
					uint freeAddress = newBlock.Address + newBlock.Size;
					uint freeSize = originalSize - block.Size - newBlock.Size;

					KMemoryBlock freeBlock = new KMemoryBlock( this, freeAddress, freeSize, true );

					// Add free space after start block if needed
					if( freeSize > 0 )
					{
						LinkedListEntry<KMemoryBlock> blockEntryFree = FreeList.Find( block );
						Blocks.InsertAfter( freeBlock, blockEntry );
						FreeList.InsertAfter( freeBlock, blockEntryFree );
					}

					// Add after the block (but before the freeBlock if there was one)
					Blocks.InsertAfter( newBlock, blockEntry );
				}
			}

			Debug.Assert( block.Size >= 0 );

			// Remove old block if dead
			if( block.Size == 0 )
			{
				Blocks.Remove( blockEntry );
				FreeList.Remove( block );
			}

			return newBlock;
		}
	}
}
