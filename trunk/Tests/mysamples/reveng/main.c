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

PSP_MODULE_INFO( "reveng", 0, 1, 1 );
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

	while( 1 )
	{
		SceKernelSysClock t;

		unsigned us = 1;
		sceKernelUSec2SysClock( us, &t );
		pspDebugScreenPrintf( "%d -> lo: %08X hi: %08X\n", us, t.low, t.hi );

		us = 1000;
		sceKernelUSec2SysClock( us, &t );
		pspDebugScreenPrintf( "%d -> lo: %08X hi: %08X\n", us, t.low, t.hi );

		us = 1000000;
		sceKernelUSec2SysClock( us, &t );
		pspDebugScreenPrintf( "%d -> lo: %08X hi: %08X\n", us, t.low, t.hi );

		us = 1000000000;
		sceKernelUSec2SysClock( us, &t );
		pspDebugScreenPrintf( "%d -> lo: %08X hi: %08X\n", us, t.low, t.hi );

		//sceKernelGetSystemTime( &t );

		//pspDebugScreenPrintf( "sceKernelGetSystemTime lo=%08X hi=%08X\n", t.low, t.hi );

		/*t.low = 0xDEADBEEF;
		t.hi = 0xCAFEBABE;

		unsigned int lo;
		unsigned int hi;
		sceKernelSysClock2USec( &t, &lo, &hi );

		pspDebugScreenPrintf( "sceKernelSysClock2USec lo=%08X hi=%08X\n", lo, hi );

		SceKernelSysClock tt;
		unsigned us = 5538892;
		sceKernelUSec2SysClock( us, &tt );

		pspDebugScreenPrintf( "sceKernelUSec2SysClock %d -> lo=%08X hi=%08X\n", tt.low, tt.hi );*/

		//sceKernelDelayThread( 1000000 );
		while( 1 );
	}

	while( 1 );

	return 0;
}
