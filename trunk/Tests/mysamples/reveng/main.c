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

PSP_MODULE_INFO( "reveng", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

extern void sceKernelSetCompiledSdkVersion( uint a );
extern void sceKernelSetCompilerVersion( uint a );

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

int DummyCallback( int arg1, int arg2, void* arg )
{
	pspDebugScreenPrintf( "callback: arg1: %X, arg2: %X, arg3: %X", arg1, arg2, (int)arg );

	return 0;
}

int main( int argc, char *argv[] )
{
	sceKernelSetCompiledSdkVersion(0x02070110);
	sceKernelSetCompilerVersion(0x00030306);

	pspDebugScreenInit();

	char name[] = "hello";

	uint freeBefore = sceKernelTotalFreeMemSize();
	SceUID uid = sceKernelAllocPartitionMemory(0x00000002, name, 0x00000003, 0x00019000, 0x00001000);
	uint freeAfter = sceKernelTotalFreeMemSize();

	uint lower = sceKernelGetBlockHeadAddr( uid );

	pspDebugScreenPrintf( "uid=%d, lower=%X", uid, lower );
	pspDebugScreenPrintf( "before=%d, after=%d, delta=%d", freeBefore, freeAfter, freeAfter - freeBefore );


	int thid = sceKernelCreateThread( "CallbackThread", CallbackThread, 0x11, 0xFA0, 0, 0 );
	if( thid >= 0 )
		sceKernelStartThread( thid, 0, 0 );

	//int cbid = sceKernelCreateCallback( "Dummy Callback", DummyCallback, NULL );

	//int  sceKernelCreateFpl (const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt) 
	//SceKernelFplOptParam opts;
	//opts.size = sizeof( SceKernelFplOptParam );
	//int id = sceKernelCreateFpl( "Fpl1", 2, 0, 0x0145D000, 1, NULL );

	// 0x00900010 -> 0890E400
	// 0x0145D000 -> 0890E400
	// = next aligned address after program memory

	/*void* ptr;
	int ret = sceKernelAllocateFpl( id, &ptr, 0 );
	if( ret >= 0 )
	{
		pspDebugScreenPrintf( "allocated ok - got ptr %X (ret=%X)", (int)ptr, ret );
	}
	else
	{
		pspDebugScreenPrintf( "failed alloc - got ret=%X", ret );
	}

	sceKernelSleepThreadCB();*/

	//unsigned int buf[5];
	//unsigned int *pbuf = buf;
	//sceIoDevctl("ms0:", 0x02425818, &pbuf, sizeof(pbuf), 0, 0);
	//pspDebugScreenPrintf( "%d %d %d %d %d", buf[ 0 ], buf[ 1 ], buf[ 2 ], buf[ 3 ], buf[ 4 ] );
	//fprintf( stdout, "%d %d %d %d %d\n", buf[ 0 ], buf[ 1 ], buf[ 2 ], buf[ 3 ], buf[ 4 ] );
	// 124958 42083 41968 512 64

	//uint x = sceKernelLoadModule("libbase64.prx", 0, NULL); 
	//uint y = sceKernelLoadModule("libbase64.prx", 0, NULL); 
	//pspDebugScreenPrintf( "%X %X", x, y );

	//sceKernelSleepThreadCB();
	for( ;; );

	return 0;
}
