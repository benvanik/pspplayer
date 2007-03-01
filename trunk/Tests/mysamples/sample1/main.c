// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>

PSP_MODULE_INFO( "sample1", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR( THREAD_ATTR_USER | THREAD_ATTR_VFPU );

long long test( int a0, int a1, int a2, int a3, int t0, int t1, int t2, int t3, int sp, int 
yy )
{
	long long r = 0;
	r = 0xCDCDCDCD; // upper
	r <<= 32;
	r |= 0xFEFEFEFE; // lower

	return r;
}

void test2( int a0, int a1, long long a12, int a3, int a4 )
{
	volatile int x = 5;
}

int main( int argc, char *argv[] )
{
	//pspDebugScreenInit();
	//pspDebugScreenPrintf( "Hello World\n" );

	//long long xx = test( 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 );
	//return (long)xx;

	test2( 1, 9, 0xCDCDCDCDFEFEFEFE, 2, 3 );
	return 0;
}
