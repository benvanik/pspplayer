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

#define L1SIZE 1024
#define L2SIZE 4096
#define L3SIZE 256
#define L2MASK 0xFFF
#define L3MASK 0xFF
#define L2SHIFT 8

//#define L1SIZE 1024
//#define L2SIZE 8192
//#define L3SIZE 128
//#define L2MASK 0x1FFF
//#define L3MASK 0x7F
//#define L2SHIFT 7

static int*** CCLookupTable;

#pragma unmanaged

void* Noxa::Emulation::Psp::Cpu::QuickPointerLookup( int address )
{
	uint addr = ( address & 0x0FFFFFFF ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;
	uint b2 = addr & L3MASK;

	int** pblock0 = *( CCLookupTable + b0 );
	if( pblock0 == NULL )
		return 0;
	int* pblock1 = *( pblock0 + b1 );
	if( pblock1 == NULL )
		return 0;
	int ret = *( pblock1 + b2 );
	if( ret == 0xCCCCCCCC )
		return 0;
	return ( void* )ret;
}

#pragma managed

R4000Cache::R4000Cache()
{
	_lookup = NULL;
	_ptrLookup = NULL;
	Version = 0;
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
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;
	uint b2 = addr & L3MASK;

	CodeBlock* block = NULL;

	LOCK;
	{
		CodeBlock** block0 = _lookup[ b0 ];
		if( block0 == NULL )
		{
//#ifdef STATISTICS
//			R4000Cpu::GlobalCpu->_stats->CodeCacheLevel2Count++;
//			R4000Cpu::GlobalCpu->_stats->CodeCacheTableSize += L2SIZE * sizeof( CodeBlock* );
//#endif
			block0 = ( CodeBlock** )calloc( L2SIZE, sizeof( CodeBlock* ) );
			_lookup[ b0 ] = block0;
		}

		CodeBlock* block1 = block0[ b1 ];
		if( block1 == NULL )
		{
//#ifdef STATISTICS
//			R4000Cpu::GlobalCpu->_stats->CodeCacheLevel3Count++;
//			R4000Cpu::GlobalCpu->_stats->CodeCacheTableSize += L3SIZE * sizeof( CodeBlock );
//#endif
			block1 = ( CodeBlock* )calloc( L3SIZE, sizeof( CodeBlock ) );
			block0[ b1 ] = block1;
		}

//#ifdef STATISTICS
//		R4000Cpu::GlobalCpu->_stats->CodeCacheBlockCount++;
//#endif

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
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;
	uint b2 = addr & L3MASK;

	LOCK;
	{
		block->Pointer = pointer;

		int** pblock0 = *( _ptrLookup + b0 );
		if( pblock0 == NULL )
		{
			pblock0 = ( int** )calloc( L2SIZE, sizeof( int* ) );
			*( _ptrLookup + b0 ) = pblock0;
		}

		int* pblock1 = *( pblock0 + b1 );
		if( pblock1 == NULL )
		{
			pblock1 = ( int* )calloc( L3SIZE, sizeof( int ) );
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
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;
	uint b2 = addr & L3MASK;

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

// Max words we search up
#define MAXCODELENGTH 400

int R4000Cache::Search( int address, CodeBlock** buffer )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;
	uint b2 = addr & L3MASK;

	LOCK;
	{
		// We need to start at where we think we are and go up, moving between lists when required

		bool hadValid = false;
		int searchCount = 0;
		int n = 0;
		while( searchCount < MAXCODELENGTH )
		{
			CodeBlock** block0 = _lookup[ b0 ];
			if( block0 == NULL )
			{
				if( b0 == 0 )
					break;
				b0--;
				continue;
			}

			CodeBlock* block1 = block0[ b1 ];
			if( block1 == NULL )
			{
				if( b1 == 0 )
				{
					b1 = L1SIZE - 1;
					if( b0 == 0 )
						break;
					b0--;
				}
				else
					b1--;
				continue;
			}

			CodeBlock* block = &block1[ b2 ];
			bool valid = ( block->Address + block->Size ) > address;
			if( valid == false )
			{
				if( ( block->Address == 0x0 ) &&
					( hadValid == true ) )
				{
					// We had previously found a valid one, but now we found a good block that is not
					// valid - we are done!
					UNLOCK;
					return n;
				}

				if( b2 == 0 )
				{
					b2 = L2SIZE - 1;
					if( b1 == 0 )
						break;
					b1--;
				}
				else
					b2--;
				searchCount++;
				continue;
			}

			// Found one!
			hadValid = true;
			buffer[ n++ ] = block;

			// -- continue trying to find more
			if( b2 == 0 )
			{
				b2 = L2SIZE - 1;
				if( b1 == 0 )
					break;
				b1--;
			}
			else
				b2--;
		}

		if( hadValid == true )
		{
			UNLOCK;
			return n;
		}
	}
	UNLOCK;
	return 0;
}

void R4000Cache::Invalidate( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> L2SHIFT ) & L2MASK;

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

		for( int n = 0; n < L3SIZE; n++ )
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
#ifdef DEBUGGING
				SAFEFREE( block->InstructionSizes );
#endif
				memset( block, 0, sizeof( CodeBlock ) );
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
			for( int n = 0; n < L1SIZE; n++ )
			{
				CodeBlock** b0 = _lookup[ n ];
				if( b0 != NULL )
				{
					for( int m = 0; m < L2SIZE; m++ )
					{
						CodeBlock* b1 = b0[ m ];
#ifdef DEBUGGING
						if( b1 != NULL )
						{
							for( int i = 0; i < L3SIZE; i++ )
								SAFEFREE( b1[ n ].InstructionSizes );
						}
#endif
						SAFEFREE( b1 );
					}
					SAFEFREE( b0 );
				}
				int** pb0 = *( _ptrLookup + n );
				if( pb0 != NULL )
				{
					for( int m = 0; m < L2SIZE; m++ )
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
			_lookup = ( CodeBlock*** )calloc( L1SIZE, sizeof( CodeBlock** ) );
			_ptrLookup = ( int*** )calloc( L1SIZE, sizeof( int** ) );
			CCLookupTable = _ptrLookup;
		}
	}
	Version++;
	UNLOCK;
}
