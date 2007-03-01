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
typedef void* ptr;

#define SAFEFREE( x ) { if( x != NULL ) free( x ); x = NULL; }
#define SAFEDELETE( x ) { if( x != NULL ) delete x; x = NULL; }

// Stupid standard...
__inline int power( int base, int exponent )
{
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
#define FrameBufferBase			0x04000000
#define FrameBufferSize			0x001FFFFF
#define FrameBufferBound		( FrameBufferBase + FrameBufferSize )
