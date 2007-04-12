// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include "KPartition.h"
#include "KMemoryBlock.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KPartition::KPartition( Bios::Kernel* kernel, uint baseAddress, uint size )
{
	Kernel = kernel;
	BaseAddress = baseAddress;
	Size = size;
	Top = baseAddress + ( uint )size;
	Bottom = baseAddress;
	FreeSize = Size;

	Blocks = new LL<KMemoryBlock*>();
	FreeList = new LL<KMemoryBlock*>();

	KMemoryBlock* emptyBlock = new KMemoryBlock( this, baseAddress, size );
	Blocks->Enqueue( emptyBlock );
	FreeList->Enqueue( emptyBlock );
}

KPartition::~KPartition()
{
	LLEntry<KMemoryBlock*>* e = Blocks->GetHead();
	while( e != NULL )
	{
		SAFEDELETE( e->Value );
		e = e->Next;
	}

	SAFEDELETE( Blocks );
	SAFEDELETE( FreeList );
}

KMemoryBlock* KPartition::Allocate( KAllocType type, uint address, uint size )
{
	KMemoryBlock* newBlock = NULL;

	// Round size up to the next word
	if( size % 4 != 0 )
		size += 4 - ( size % 4 );

	switch( type )
	{
	case KAllocSpecific:
		{
			assert( address != 0 );
			LLEntry<KMemoryBlock*>* e = FreeList->GetHead();
			while( e != NULL )
			{
				KMemoryBlock* freeBlock = e->Value;

				if( ( address >= freeBlock->Address ) &&
					( address < freeBlock->UpperBound ) )
				{
					newBlock = this->SplitBlock( freeBlock, address, size );
					break;
				}

				e = e->Next;
			}
		}
		break;
	case KAllocLow:
		{
			KMemoryBlock* targetBlock = NULL;
			if( address != 0 )
			{
				// If we specified a lower limit, find the first block that fits and use that
				LLEntry<KMemoryBlock*>* e = FreeList->GetHead();
				while( e != NULL )
				{
					KMemoryBlock* freeBlock = e->Value;

					if( ( address != 0 ) &&
						( ( address < freeBlock->Address ) ||
						  ( address > freeBlock->UpperBound ) ) )
					{
						e = e->Next;
						continue;
					}

					// This block is good enough
					targetBlock = freeBlock;
					break;
				}
			}
			else
			{
				// No lower limit - pick first free block
				targetBlock = FreeList->PeekHead();
			}
			assert( targetBlock != NULL );
			newBlock = this->SplitBlock(
				targetBlock,
				( address != 0 ) ? address : targetBlock->Address,
				size );
		}
		break;
	case KAllocHigh:
		{
			assert( address == 0 );
			KMemoryBlock* targetBlock = FreeList->PeekTail();
			assert( targetBlock != NULL );
			newBlock = this->SplitBlock(
				targetBlock,
				targetBlock->UpperBound - size,
				size );
		}
		break;
	}

	if( newBlock != NULL )
		FreeSize -= size;
	return newBlock;
}

void KPartition::Free( KMemoryBlock* block )
{
	assert( block != NULL );
	assert( block->IsFree == false );

	block->UID = -1;
	block->IsFree = true;

	FreeSize += block->Size;

	LLEntry<KMemoryBlock*>* blockEntry = Blocks->Find( block );

	// Attempt to coalesce in to previous and/or next blocks
	KMemoryBlock* merged = NULL;
	LLEntry<KMemoryBlock*>* prev = blockEntry->Previous;
	if( ( prev != NULL ) &&
		( prev->Value->IsFree == true ) )
	{
		// Merge in to previous (kill us)
		Blocks->Remove( blockEntry );
		prev->Value->Size += block->Size;
		merged = prev->Value;
		
		SAFEDELETE( block );
	}
	LLEntry<KMemoryBlock*>* next = blockEntry->Next;
	if( ( next != NULL ) &&
		( next->Value->IsFree == true ) )
	{
		// Merge next in to us (kill them)
		// Take in to account whether or not we already merged
		if( merged == NULL )
			merged = block;
		KMemoryBlock* nextBlock = next->Value;
		Blocks->Remove( next );
		FreeList->Remove( nextBlock );
		merged->Size += nextBlock->Size;

		SAFEDELETE( nextBlock );
	}

	if( merged == NULL )
	{
		// Couldn't merge - putting in free list
		this->AddToFreeList( block );
	}
}

void KPartition::AddToFreeList( KMemoryBlock* block )
{
	// Insert in to free list at the right place
	if( FreeList->GetCount() == 0 )
	{
		FreeList->Enqueue( block );
	}
	else
	{
		bool inserted = false;
		LLEntry<KMemoryBlock*>* e = FreeList->GetHead();
		while( e != NULL )
		{
			if( e->Value->Address > block->Address )
			{
				// Found next block - insert before
				FreeList->InsertBefore( block, e );
				inserted = true;
				break;
			}
			e = e->Next;
		}
		if( inserted == false )
			FreeList->Enqueue( block );
	}
}

KMemoryBlock* KPartition::SplitBlock( KMemoryBlock* block, uint address, uint size )
{
	assert( size > 0 );
	KMemoryBlock* newBlock = new KMemoryBlock( this, address, size );
	newBlock->IsFree = false;

	LLEntry<KMemoryBlock*>* blockEntry = Blocks->Find( block );
	assert( blockEntry != NULL );

	if( address == block->Address )
	{
		// Bottom up - put right before free and shift free up
		block->Address += size;
		block->Size -= size;

		Blocks->InsertBefore( newBlock, blockEntry );
	}
	else if( address == block->UpperBound - size )
	{
		// Top down - put right after free and shift free down
		block->Size -= size;

		Blocks->InsertAfter( newBlock, blockEntry );
	}
	else
	{
		// Middle - need a real split
		uint originalSize = block->Size;
		block->Size = newBlock->Address - block->Address;
		if( block->Size == 0 )
		{
			// Special case of block replacing block
			Blocks->InsertAfter( newBlock, blockEntry );

			// Block will be removed below
		}
		else
		{
			uint freeAddress = newBlock->Address + newBlock->Size;
			uint freeSize = originalSize - block->Size - newBlock->Size;

			KMemoryBlock* freeBlock = new KMemoryBlock( this, freeAddress, freeSize );

			// Add free space after start block if needed
			if( freeSize > 0 )
			{
				LLEntry<KMemoryBlock*>* blockEntryFree = FreeList->Find( block );
				Blocks->InsertAfter( freeBlock, blockEntry );
				FreeList->InsertAfter( freeBlock, blockEntryFree );
			}

			// Add after the block (but before the freeBlock if there was one)
			Blocks->InsertAfter( newBlock, blockEntry );
		}
	}

	// Remove old block if dead
	if( block->Size == 0 )
	{
		Blocks->Remove( blockEntry );
		FreeList->Remove( block );

		SAFEDELETE( block );
	}

	return newBlock;
}
