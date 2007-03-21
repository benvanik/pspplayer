// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <string>
#include "R4000Cache.h"

#ifdef STATISTICS
#include "R4000Cpu.h"
#endif

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#if MULTITHREADED
#define LOCK	Monitor::Enter( _syncRoot )
#define UNLOCK	UNLOCK;
#else
#define LOCK
#define UNLOCK
#endif

#define BLOCKSIZE 1024

static int*** CCLookupTable;

#pragma unmanaged
int Noxa::Emulation::Psp::Cpu::QuickPointerLookup( int address )
{
	uint addr = address >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	int** pblock0 = *( CCLookupTable + b0 );
	if( pblock0 == NULL )
		return 0;
	int* pblock1 = *( pblock0 + b1 );
	if( pblock1 == NULL )
		return 0;
	int ret = *( pblock1 + b2 );
	if( ret == 0xCCCCCCCC )
		return 0;
	return ret;
}
#pragma managed

R4000Cache::R4000Cache()
{
	_lookup = NULL;
	_ptrLookup = NULL;
	this->Clear( true );
}

R4000Cache::~R4000Cache()
{
	this->Clear( false );
}

CodeBlock* R4000Cache::Add( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	CodeBlock* block = NULL;

	LOCK;
	{
		CodeBlock** block0 = _lookup[ b0 ];
		if( block0 == NULL )
		{
#ifdef STATISTICS
			R4000Cpu::GlobalCpu->_stats->CodeCacheLevel2Count++;
			R4000Cpu::GlobalCpu->_stats->CodeCacheTableSize += BLOCKSIZE * sizeof( CodeBlock* );
#endif
			block0 = ( CodeBlock** )calloc( BLOCKSIZE, sizeof( CodeBlock* ) );
			_lookup[ b0 ] = block0;
		}

		CodeBlock* block1 = block0[ b1 ];
		if( block1 == NULL )
		{
#ifdef STATISTICS
			R4000Cpu::GlobalCpu->_stats->CodeCacheLevel3Count++;
			R4000Cpu::GlobalCpu->_stats->CodeCacheTableSize += BLOCKSIZE * sizeof( CodeBlock );
#endif
			block1 = ( CodeBlock* )calloc( BLOCKSIZE, sizeof( CodeBlock ) );
			block0[ b1 ] = block1;
		}

#ifdef STATISTICS
		R4000Cpu::GlobalCpu->_stats->CodeCacheBlockCount++;
#endif

		block = &block1[ b2 ];
		block->Address = address;
	}
	UNLOCK;

	return block;
}

void R4000Cache::UpdatePointer( CodeBlock* block, void* pointer )
{
	uint addr = ( ( uint )block->Address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	LOCK;
	{
		block->Pointer = pointer;

		int** pblock0 = *( _ptrLookup + b0 );
		if( pblock0 == NULL )
		{
			pblock0 = ( int** )calloc( BLOCKSIZE, sizeof( int* ) );
			*( _ptrLookup + b0 ) = pblock0;
		}

		int* pblock1 = *( pblock0 + b1 );
		if( pblock1 == NULL )
		{
			pblock1 = ( int* )calloc( BLOCKSIZE, sizeof( int ) );
			*( pblock0 + b1 ) = pblock1;
		}

		*( pblock1 + b2 ) = ( int )pointer;
	}
	UNLOCK;
}

CodeBlock* R4000Cache::Find( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	LOCK;
	{
		CodeBlock** block0 = _lookup[ b0 ];
		if( block0 == NULL )
		{
			UNLOCK;
			return NULL;
		}

		CodeBlock* block1 = block0[ b1 ];
		if( block1 == NULL )
		{
			UNLOCK;
			return NULL;
		}

		CodeBlock* block = &block1[ b2 ];

		UNLOCK;
		if( block->Address == address )
			return block;
		else
			return NULL;
	}
}

void R4000Cache::Invalidate( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;

	LOCK;
	{
		CodeBlock** block0 = _lookup[ b0 ];
		if( block0 == NULL )
		{
			UNLOCK;
			return;
		}

		CodeBlock* block1 = block0[ b1 ];
		if( block1 == NULL )
		{
			UNLOCK;
			return;
		}

		// --

		int** pblock0 = *( _ptrLookup + b0 );
		int* pblock1 = *( pblock0 + b1 );

		for( int n = 0; n < BLOCKSIZE; n++ )
		{
			CodeBlock* block = &block1[ n ];
			if( block->Address == NULL )
				continue;
			
			// If we have gone beyond the starting address, then break out
			int upper = block->Address + block->Size;
			if( upper > address )
				break;

			// If the block contains the address, invalidate
			if( ( address >= block->Address ) &&
				( address <= upper ) )
			{
				memset( &block1[ n ], 0, sizeof( CodeBlock ) );
				*( pblock1 + n ) = NULL;
			}
		}
	}
	UNLOCK;
}

void R4000Cache::Clear()
{
	this->Clear( true );
}

void R4000Cache::Clear( bool realloc )
{
	LOCK;
	{
		if( _lookup != NULL )
		{
			for( int n = 0; n < BLOCKSIZE; n++ )
			{
				CodeBlock** b0 = _lookup[ n ];
				if( b0 != NULL )
				{
					for( int m = 0; m < BLOCKSIZE; m++ )
					{
						CodeBlock* b1 = b0[ m ];
						SAFEFREE( b1 );
					}
					SAFEFREE( b0 );
				}
				int** pb0 = *( _ptrLookup + n );
				if( pb0 != NULL )
				{
					for( int m = 0; m < BLOCKSIZE; m++ )
					{
						int* pb1 = *( pb0 + m );
						SAFEFREE( pb1 );
					}
					SAFEFREE( pb0 );
				}
			}
			SAFEFREE( _lookup );
			SAFEFREE( _ptrLookup );
		}
		CCLookupTable = NULL;

		if( realloc == true )
		{
			_lookup = ( CodeBlock*** )calloc( BLOCKSIZE, sizeof( CodeBlock** ) );
			_ptrLookup = ( int*** )calloc( BLOCKSIZE, sizeof( int** ) );
			CCLookupTable = _ptrLookup;
		}
	}
	UNLOCK;
}
