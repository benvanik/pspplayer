// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>
#include <pspkernel.h>
#include <pspdisplay.h>
#include <pspthreadman.h>
#include <stdio.h>

PSP_MODULE_INFO( "vfputest", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

// Exit callback
int ExitCallback( int arg1, int arg2, void *common )
{
	sceKernelExitGame();
	return 0;
}

int CallbackThread( SceSize args, void *argp )
{
	int cbid = sceKernelCreateCallback( "Exit Callback", ExitCallback, NULL );
	sceKernelRegisterExitCallback( cbid );
	sceKernelSleepThreadCB();
	return 0;
}

int main( int argc, char *argv[] )
{
	pspDebugScreenInit();

	int thid = sceKernelCreateThread( "CallbackThread", CallbackThread, 0x11, 0xFA0, 0, 0 );
	if( thid >= 0 )
		sceKernelStartThread( thid, 0, 0 );

	

	for( ;; );

	return 0;
}
