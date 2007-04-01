// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

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

#define SAFEFREE( x ) { if( x != NULL ) free( x ); x = NULL; }
#define SAFEDELETE( x ) { if( x != NULL ) delete x; x = NULL; }

#define MIN2( a, b ) ( a < b ) ? a : b

// Stupid standard...
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

// Memory ---------------------------------------------------------------------
// These are some useful constants dealing with memory addresses
#define MainMemoryBase			0x08000000
#define MainMemorySize			0x01FFFFFF
#define MainMemoryBound			( MainMemoryBase + MainMemorySize )
#define ScratchPadBase			0x00010000
#define ScratchPadSize			0x00003FFF
#define ScratchPadBound			( ScratchPadBase + ScratchPadSize )
#define VideoMemoryBase			0x04000000
#define VideoMemorySize			0x001FFFFF
#define VideoMemoryBound		( VideoMemoryBase + VideoMemorySize )
