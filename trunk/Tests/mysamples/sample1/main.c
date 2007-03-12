// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>
#include <zlib.h>

PSP_MODULE_INFO( "sample1", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

int main( int argc, char *argv[] )
{
	//pspDebugScreenInit();
	//pspDebugScreenPrintf( "Hello World\n" );

	int crc = crc32( 0, 0, 0 );
	char buffer[] = "hello";
	int length = 5;
	crc = crc32( crc, (void*)buffer, length );
	
	sceKernelSelfStopUnloadModule( crc, 0, 0 );
	return 0;
}
