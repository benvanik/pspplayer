// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

typedef unsigned char byte;
typedef unsigned int uint;

#define SAFEFREE( x ) { if( x != NULL ) free( x ); x = NULL; }