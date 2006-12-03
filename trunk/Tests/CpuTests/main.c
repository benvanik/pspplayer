#include <pspkernel.h>
#include <pspdebug.h>

#define printf pspDebugScreenPrintf

/* Define the module info section */
PSP_MODULE_INFO("CPU Tests", 0, 1, 1);

/* Define the main thread's attribute value (optional) */
PSP_MAIN_THREAD_ATTR(THREAD_ATTR_USER | THREAD_ATTR_VFPU);

#define BEGINTEST(n) pspDebugScreenPrintf( "Test %d: ", n );
#define ENDTEST printf( "Passed\n" );

int main(int argc, char *argv[])
{
	pspDebugScreenInit();

	BEGINTEST(1);
	{
		volatile int x = 5;
		volatile int y = 5;
		volatile int z = x * y;
		printf( "%d", z );
		if( z != 25 )
			goto failed;
	}
	ENDTEST;

	BEGINTEST(2);
	{
		volatile int x = 25;
		volatile int y = 5;
		volatile int w = 3;
		volatile int z = x / y;
		volatile int t = x % w;
		printf( "%d %%: %d", z, t );
		if( ( z != 5 ) || ( t != 1 ) )
			goto failed;
	}
	ENDTEST;

	printf( "\n\nDone" );
	while(1);
	return 0;

failed:
	printf( "Failed!\r\n" );
	while(1);

	return 0;
}
