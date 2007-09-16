// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2007 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>
#include <pspkdebug.h>
#include <pspsdk.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

PSP_MODULE_INFO( "hooker", PSP_MODULE_KERNEL, 1, 1 );
PSP_MAIN_THREAD_ATTR( 0 );
PSP_HEAP_SIZE_KB( 12 );

int LoadHooks( const char* hooksFile );

int main( int argc, char* argv[] )
{
	pspDebugScreenInit();

	const char hooksFile[] = "hooks.txt";
	if( LoadHooks( hooksFile ) <= 0 )
		return -1;

	sceKernelSleepThread();

	return 0;
}
