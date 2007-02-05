// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>

PSP_MODULE_INFO( "sample1", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

int main( int argc, char *argv[] )
{
	//pspDebugScreenInit();
	//pspDebugScreenPrintf( "Hello World\n" );

	return 0;
}
