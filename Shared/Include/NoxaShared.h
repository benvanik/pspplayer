// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

typedef unsigned char byte;
typedef unsigned short ushort;
typedef unsigned int uint;
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
