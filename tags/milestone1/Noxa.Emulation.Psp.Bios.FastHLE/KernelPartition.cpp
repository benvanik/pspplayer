// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelPartition.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KernelPartition::KernelPartition( Bios::Kernel^ kernel, int id, uint baseAddress, uint size )
{
	Kernel = kernel;
	ID = id;
	BaseAddress = baseAddress;
	Size = size;
	Top = baseAddress + ( uint )size;
	Bottom = baseAddress;
	FreeSize = Size;

	Blocks = gcnew List<KernelMemoryBlock^>( 1024 );
	FreeList = gcnew List<KernelMemoryBlock^>( 1024 );

	KernelMemoryBlock^ emptyBlock = gcnew KernelMemoryBlock( -1 );
	emptyBlock->IsFree = true;
	emptyBlock->Address = baseAddress;
	emptyBlock->Size = size;
	emptyBlock->Partition = this;
	Blocks->Add( emptyBlock );

	FreeList->Add( emptyBlock );
}

KernelMemoryBlock^ KernelPartition::Allocate( KernelAllocationType type, uint address, uint size )
{
	KernelMemoryBlock^ newBlock = nullptr;

	if( type == KernelAllocationType::SpecificAddress )
	{
		for( int n = 0; n < Blocks->Count; n++ )
		{
			KernelMemoryBlock^ block = Blocks[ n ];

			if( block->IsFree == false )
				continue;
			if( ( address >= block->Address ) &&
				( address < block->UpperBound ) )
			{
				// Split
				newBlock = this->SplitBlock( block, address, size );
				if( block->Size <= 0 )
				{
					// Free space dead
					Blocks->Remove( block );
					FreeList->Remove( block );
				}
				break;
			}
		}
	}
	else if( type == KernelAllocationType::Low )
	{
		for( int n = 0; n < Blocks->Count; n++ )
		{
			KernelMemoryBlock^ block = Blocks[ n ];

			if( block->IsFree == false )
				continue;
			
			// If we specified a lower limit for this block, keep searching
			if( ( address != 0 ) &&
				( ( address < block->Address ) ||
				  ( address > block->UpperBound ) ) )
				  continue;

			newBlock = this->SplitBlock( block, ( address != 0 ) ? address : block->Address, size );
			if( block->Size <= 0 )
			{
				// Free space dead
				Blocks->Remove( block );
				FreeList->Remove( block );
			}
			break;
		}
	}
	else
	{
		for( int n = Blocks->Count - 1; n >= 0; n-- )
		{
			KernelMemoryBlock^ block = Blocks[ n ];

			if( block->IsFree == false )
				continue;
			newBlock = this->SplitBlock( block, ( uint )( ( block->UpperBound ) - size ), size );
			if( block->Size <= 0 )
			{
				// Free space dead
				Blocks->Remove( block );
				FreeList->Remove( block );
			}
			break;
		}
	}

	FreeSize -= size;
	return newBlock;
}

KernelMemoryBlock^ KernelPartition::SplitBlock( KernelMemoryBlock^ block, uint address, uint size )
{
	KernelMemoryBlock^ newBlock = gcnew KernelMemoryBlock( Kernel->AllocateID() );
	newBlock->IsFree = false;
	newBlock->Partition = block->Partition;
	newBlock->Address = address;
	newBlock->Size = size;
	
	if( address == block->Address )
	{
		// Bottom up - put right before free and shift free up
		block->Address = address + size;
		block->Size -= size;

		Blocks->Insert( Blocks->IndexOf( block ), newBlock );
	}
	else if( address == block->UpperBound - size )
	{
		// Top down - put right after free and shift free down
		block->Size -= size;

		Blocks->Insert( Blocks->IndexOf( block ) + 1, newBlock );
	}
	else
	{
		// Middle - need to split and add new + subfree
		uint originalSize = block->Size;
		block->Size = newBlock->Address - block->Address;

		KernelMemoryBlock^ freeBlock = gcnew KernelMemoryBlock( -1 );
		freeBlock->IsFree = true;
		freeBlock->Partition = block->Partition;
		freeBlock->Address = newBlock->Address + newBlock->Size;
		freeBlock->Size = originalSize - block->Size - newBlock->Size;

		int index = Blocks->IndexOf( block );
		Blocks->Insert( index + 1, newBlock );

		if( freeBlock->Size > 0 )
		{
			Blocks->Insert( index + 2, newBlock );
			FreeList->Add( freeBlock );

			FreeList->Sort( gcnew Comparison<KernelMemoryBlock^>( this, &KernelPartition::BlockCompare ) );
		}
	}

	return newBlock;
}

void KernelPartition::Free( KernelMemoryBlock^ block )
{
	int index = Blocks->IndexOf( block );
	block->IsFree = true;
	block->ID = -1;

	FreeSize += block->Size;
	FreeList->Add( block );

	// Coalesce
	if( index > 0 )
	{
		KernelMemoryBlock^ target = Blocks[ index - 1 ];
		if( target->IsFree == true )
		{
			Blocks->RemoveAt( index );
			FreeList->Remove( block );
			index = index - 1;

			target->Size += block->Size;
		}
	}
	if( index < Blocks->Count - 1 )
	{
		KernelMemoryBlock^ target = Blocks[ index + 1 ];
		if( target->IsFree == true )
		{
			Blocks->RemoveAt( index + 1 );
			FreeList->Remove( target );

			Blocks[ index ]->Size += target->Size;
		}
	}

	FreeList->Sort( gcnew Comparison<KernelMemoryBlock^>( this, &KernelPartition::BlockCompare ) );
}

int KernelPartition::BlockCompare( KernelMemoryBlock^ a, KernelMemoryBlock^ b )
{
	if( a->Address < b->Address )
		return -1;
	else if( a->Address == b->Address )
		return 0;
	else
		return 1;
}
