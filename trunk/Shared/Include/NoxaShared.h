// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

typedef unsigned char byte;
typedef unsigned short ushort;
typedef unsigned int uint;
typedef void* ptr;

#define SAFEFREE( x ) { if( x != NULL ) free( x ); x = NULL; }
#define SAFEDELETE( x ) { if( x != NULL ) delete x; x = NULL; }
