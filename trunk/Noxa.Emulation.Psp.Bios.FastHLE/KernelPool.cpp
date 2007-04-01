// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelPool.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KernelPool::KernelPool( Bios::Kernel^ kernel, int id, String^ name, KernelPartition^ partition, int attributes, uint blockSize, int blockCount )
	: KernelHandle( KernelHandleType::Pool, id )
{
	Kernel = kernel;
	ID = id;
	Name = name;
	Partition = partition;
	Attributes = attributes;
	BlockSize = blockSize;

	IsFixed = ( blockCount > 0 );
	if( IsFixed == true )
	{
		Blocks = gcnew List<KernelMemoryBlock^>( blockCount );
		FreeList = gcnew List<KernelMemoryBlock^>( blockCount );
		UsedList = gcnew List<KernelMemoryBlock^>( blockCount );

		// Allocate all blocks now
		for( int n = 0; n < blockCount; n++ )
		{
			KernelMemoryBlock^ block = Partition->Allocate( KernelAllocationType::Low, Kernel->_elfUpperBounds, BlockSize );
			Blocks->Add( block );
			FreeList->Add( block );
			block->IsFree = true;
		}
	}
	else
	{
		Blocks = gcnew List<KernelMemoryBlock^>( 1000 );
		FreeList = gcnew List<KernelMemoryBlock^>( 1000 );
		UsedList = gcnew List<KernelMemoryBlock^>( 1000 );
	}
}

void KernelPool::Cleanup()
{
	for( int n = 0; n < Blocks->Count; n++ )
		Partition->Free( Blocks[ n ] );
	Blocks->Clear();
	FreeList->Clear();
	UsedList->Clear();
}

KernelMemoryBlock^ KernelPool::Allocate()
{
	KernelMemoryBlock^ block;
	if( FreeList->Count == 0 )
	{
		// If fixed, we can't do anything
		if( IsFixed == true )
			return nullptr;
		else
		{
			// If variable, allocate a new block
			block = Partition->Allocate( KernelAllocationType::Low, Kernel->_elfUpperBounds, BlockSize );
			Blocks->Add( block );
		}
	}

	if( block == nullptr )
	{
		block = FreeList[ FreeList->Count - 1 ];
		FreeList->RemoveAt( FreeList->Count - 1 );
	}

	block->IsFree = false;

	return block;
}

void KernelPool::Free( int address )
{
	for( int n = 0; n < UsedList->Count; n++ )
	{
		if( UsedList[ n ]->Address == address )
		{
			this->Free( UsedList[ n ] );
			return;
		}
	}
	return;
}

void KernelPool::Free( KernelMemoryBlock^ block )
{
	Debug::Assert( block != nullptr );
	Debug::Assert( block->IsFree == false );

	UsedList->Remove( block );
	FreeList->Add( block );
	block->IsFree = true;
}
