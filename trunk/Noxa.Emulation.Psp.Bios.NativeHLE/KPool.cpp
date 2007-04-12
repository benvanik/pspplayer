// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include "KPool.h"
#include "KPartition.h"
#include "KMemoryBlock.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

#define VPLGROWTHSIZE	64

KPool::KPool( KPartition* partition, char* name, uint attributes, uint blockSize )
{
	assert( name != NULL );
	assert( blockSize > 0 );
	assert( partition != NULL );

	Name = _strdup( name );
	Attributes = attributes;
	BlockSize = blockSize;
	Partition = partition;

	Blocks = new LL<KMemoryBlock*>();
	UsedBlocks = new LL<KMemoryBlock*>();
	FreeBlocks = new LL<KMemoryBlock*>();
}

KPool::~KPool()
{
	SAFEFREE( Name );

	LLEntry<KMemoryBlock*>* e = Blocks->GetHead();
	while( e != NULL )
	{
		Partition->Free( e->Value );
		e = e->Next;
	}

	SAFEDELETE( Blocks );
	SAFEDELETE( UsedBlocks );
	SAFEDELETE( FreeBlocks );
}

KMemoryBlock* KPool::Allocate()
{
	if( FreeBlocks->GetCount() == 0 )
	{
		bool allocated = this->AllocateBlocks();
		assert( allocated == true );
		if( allocated == false )
			return NULL;
		assert( FreeBlocks->GetCount() > 0 );
	}

	KMemoryBlock* block = FreeBlocks->Dequeue();
	UsedBlocks->Enqueue( block );

	block->IsFree = false;

	return block;
}

void KPool::Free( int address )
{
	LLEntry<KMemoryBlock*>* e = UsedBlocks->GetHead();
	while( e != NULL )
	{
		if( e->Value->Address == address )
		{
			KMemoryBlock* block = e->Value;
			block->IsFree = true;
			UsedBlocks->Remove( e );
			FreeBlocks->Enqueue( block );
			return;
		}
		e = e->Next;
	}
}

void KPool::Free( KMemoryBlock* block )
{
	assert( block != NULL );
	assert( block->IsFree == false );

	LLEntry<KMemoryBlock*>* e = UsedBlocks->Find( block );
	assert( e != NULL );
	if( e != NULL )
	{
		block->IsFree = true;
		UsedBlocks->Remove( e );
		FreeBlocks->Enqueue( block );
	}
}

KFixedPool::KFixedPool( KPartition* partition, char* name, uint attributes, uint blockSize, int blockCount )
	: KPool( partition, name, attributes, blockSize )
{
	assert( blockCount > 0 );
	for( int n = 0; n < blockCount; n++ )
	{
		KMemoryBlock* block = partition->Allocate( KAllocLow, 0, blockSize );
		assert( block != NULL );
		Blocks->Enqueue( block );
		FreeBlocks->Enqueue( block );
	}
}

KVariablePool::KVariablePool( KPartition* partition, char* name, uint attributes, uint blockSize )
	: KPool( partition, name, attributes, blockSize )
{
}

bool KVariablePool::AllocateBlocks()
{
	for( int n = 0; n < VPLGROWTHSIZE; n++ )
	{
		KMemoryBlock* block = Partition->Allocate( KAllocLow, 0, BlockSize );
		assert( block != NULL );
		Blocks->Enqueue( block );
		FreeBlocks->Enqueue( block );
	}

	return true;
}
