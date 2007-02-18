// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <string>
#include "R4000Cache.h"

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
	return *( pblock1 + b2 );
}
#pragma managed

R4000Cache::R4000Cache()
{
	_ptrLookup = NULL;
	_syncRoot = gcnew Object();
	this->Clear( true );
}

R4000Cache::~R4000Cache()
{
	this->Clear( false );
}

void R4000Cache::Add( CodeBlock^ block )
{
	uint addr = ( ( uint )block->Address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	LOCK;
	{
		array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
		if( block0 == nullptr )
		{
			block0 = gcnew array<array<CodeBlock^>^>( 1024 );
			_lookup[ b0 ] = block0;
		}

		array<CodeBlock^>^ block1 = block0[ b1 ];
		if( block1 == nullptr )
		{
			block1 = gcnew array<CodeBlock^>( 1024 );
			block0[ b1 ] = block1;
		}

		block1[ b2 ] = block;

		// --

		int** pblock0 = *( _ptrLookup + b0 );
		if( pblock0 == NULL )
		{
			pblock0 = ( int** )malloc( 4 * 1024 );
			memset( pblock0, 0, 4 * 1024 );
			*( _ptrLookup + b0 ) = pblock0;
		}

		int* pblock1 = *( pblock0 + b1 );
		if( pblock1 == NULL )
		{
			pblock1 = ( int* )malloc( 4 * 1024 );
			memset( pblock1, 0, 4 * 1024 );
			*( pblock0 + b1 ) = pblock1;
		}

		*( pblock1 + b2 ) = ( int )block->Pointer;
	}
	UNLOCK;
}

CodeBlock^ R4000Cache::Find( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;
	uint b2 = addr & 0x3FF;

	LOCK;
	{
		array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
		if( block0 == nullptr )
		{
			UNLOCK;
			return nullptr;
		}

		array<CodeBlock^>^ block1 = block0[ b1 ];
		if( block1 == nullptr )
		{
			UNLOCK;
			return nullptr;
		}

		CodeBlock^ block = block1[ b2 ];

		UNLOCK;
		return block;
	}
}

void R4000Cache::Invalidate( int address )
{
	uint addr = ( ( uint )address ) >> 2;

	uint b0 = addr >> 20;
	uint b1 = ( addr >> 10 ) & 0x3FF;

	LOCK;
	{
		array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
		if( block0 == nullptr )
		{
			UNLOCK;
			return;
		}

		array<CodeBlock^>^ block1 = block0[ b1 ];
		if( block1 == nullptr )
		{
			UNLOCK;
			return;
		}

		// --

		int** pblock0 = *( _ptrLookup + b0 );
		int* pblock1 = *( pblock0 + b1 );

		for( int n = 0; n < block1->Length; n++ )
		{
			CodeBlock^ block = block1[ n ];
			if( block == nullptr )
				continue;
			
			// If we have gone beyond the starting address, then break out
			int upper = block->Address + block->Size;
			if( upper > address )
				break;

			// If the block contains the address, invalidate
			if( ( address >= block->Address ) &&
				( address <= upper ) )
			{
				block1[ n ] = nullptr;
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
		_lookup = nullptr;
		if( _ptrLookup != NULL )
		{
			for( int n = 0; n < 1024; n++ )
			{
				int** pb0 = *( _ptrLookup + n );
				if( pb0 != NULL )
				{
					for( int m = 0; m < 1024; m++ )
					{
						int* pb1 = *( pb0 + m );
						SAFEFREE( pb1 );
					}
					SAFEFREE( pb0 );
				}
			}
			SAFEFREE( _ptrLookup );
		}
		CCLookupTable = NULL;

		if( realloc == true )
		{
			_lookup = gcnew array<array<array<CodeBlock^>^>^>( 1024 );
			_ptrLookup = ( int*** )malloc( 4 * 1024 );
			memset( _ptrLookup, 0, 4 * 1024 );
			CCLookupTable = _ptrLookup;
		}
	}
	UNLOCK;
}
