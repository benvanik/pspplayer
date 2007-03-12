// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>
#include <stdio.h>
#include <zlib.h>

PSP_MODULE_INFO( "sample1", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

int main( int argc, char *argv[] )
{
	//pspDebugScreenInit();
	//pspDebugScreenPrintf( "Hello World\n" );

	volatile int x = -10;
	fprintf( stderr, "%d", x );

	//volatile unsigned long long x = -10;
	//volatile unsigned long long y = 2;
	//unsigned long long z = __udivdi3( x, y );
	//sceKernelSelfStopUnloadModule( ( int )z, 0, 0 );

	//int crc = crc32( 0, 0, 0 );
	//char buffer[] = "hello";
	//int length = 5;
	//crc = crc32( crc, (void*)buffer, length );
	//
	//sceKernelSelfStopUnloadModule( crc, 0, 0 );
	return 0;
}
