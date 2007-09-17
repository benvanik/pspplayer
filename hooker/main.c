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
PSP_HEAP_SIZE_KB( 12 );

int LoadHooks( const char* hooksFile );

int HookerThread( SceSize args, void *argp )
{
	const char hooksFile[] = "host0:/hooks.txt";
	if( LoadHooks( hooksFile ) <= 0 )
		return -1;

	sceKernelSleepThread();

	return 0;
}

int module_start( SceSize args, void* argp )
{
	SceUID thid = sceKernelCreateThread( "HookerThread", HookerThread, 10, 0x800, 0, NULL );
	if( thid < 0 )
	{
		printf( "Hooker: unable to create main thread" );
		return -1;
	}

	if( sceKernelStartThread( thid, args, argp ) < 0 )
	{
		printf( "Hooker: unable to start main thread" );
		return -1;
	}

	return 0;
}

int module_stop( SceSize args, void *argp )
{
	return 0;
}
