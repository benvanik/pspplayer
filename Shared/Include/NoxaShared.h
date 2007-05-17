// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <assert.h>
//#include "gcref.h"

// This must be in project settings!!
//#define _WIN32_WINNT	0x0502

typedef unsigned char byte;
typedef unsigned short ushort;
typedef unsigned int uint;
typedef long long int64;
typedef unsigned long long uint64;
typedef unsigned short word;
typedef unsigned int dword;
typedef unsigned long long qword;
typedef void* ptr;

typedef union SysClock_u
{
	struct
	{
		uint	LowPart;
		int		HighPart;
	};
	long long	QuadPart;
} SysClock;

#define SAFEFREE( x ) { if( x != NULL ) free( ( void* )x ); x = NULL; }
#define SAFEDELETE( x ) { if( x != NULL ) delete x; x = NULL; }
#define SAFEDELETEA( x ) { if ( x != NULL ) delete[] x; x = NULL; }

#define MIN2( a, b ) ( a < b ) ? a : b

// Stupid standard...
#pragma unmanaged
__inline int power( int base, int exponent )
{
	if( base == 2 )
		return 1 << exponent;
	if( exponent == 0 )
		return 1;
	else if( exponent == 1 )
		return base;
	else
		return base * power( base, exponent - 1 );
}
#pragma managed

// Memory ---------------------------------------------------------------------
// These are some useful constants dealing with memory addresses
#define MainMemoryMask			0xF7000000
#define MainMemoryBase			0x08000000
#define MainMemorySize			0x01FFFFFF
#define MainMemoryBound			( MainMemoryBase + MainMemorySize )
#define ScratchPadBase			0x00010000
#define ScratchPadSize			0x00003FFF
#define ScratchPadBound			( ScratchPadBase + ScratchPadSize )
#define VideoMemoryBase			0x04000000
#define VideoMemorySize			0x001FFFFF
#define VideoMemoryBound		( VideoMemoryBase + VideoMemorySize )

// If defined, the scratch pad memory will be supported - if it's not, things may be faster
//#define SUPPORTSCRATCHPAD

#pragma unmanaged
namespace Noxa { namespace Emulation { namespace Psp {
typedef struct NativeMemorySystem_t
{
	byte*	MainMemory;
	byte*	VideoMemory;
	byte*	ScratchPad;

	__inline byte* Translate( int guestAddress )
	{
		guestAddress &= 0x3FFFFFFF;
		if( ( guestAddress & MainMemoryBase ) != 0 )
		{
			assert( ( guestAddress >= MainMemoryBase ) && ( guestAddress < MainMemoryBound ) );
			return ( MainMemory + ( guestAddress - MainMemoryBase ) );
		}
		else if( ( guestAddress & VideoMemoryBase ) != 0 )
		{
			// Shadow memory?
			guestAddress &= 0x041FFFFF;

			assert( ( guestAddress >= VideoMemoryBase ) && ( guestAddress < VideoMemoryBound ) );
			return ( VideoMemory + ( guestAddress - VideoMemoryBase ) );
		}
#ifdef SUPPORTSCRATCHPAD
		else if( ( guestAddress & ScratchPadBase ) != 0 )
		{
			assert( ( guestAddress >= ScratchPadBase ) && ( guestAddress < ScratchPadBound ) );
			return ( ScratchPad + ( guestAddress - ScratchPadBase ) );
		}
#endif
		else
		{
			assert( false );
			return 0;
		}
	}
} NativeMemorySystem;
} } }
#pragma managed
