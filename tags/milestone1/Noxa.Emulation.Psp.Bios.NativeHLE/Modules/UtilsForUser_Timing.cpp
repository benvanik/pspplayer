// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <time.h>

#include "UtilsForUser.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// _CLOCKS_PER_SEC_ - 1000000 - clock ticks per second, I think (microseconds)

// clock_t sceKernelLibcClock(); (/user/psputils.h:43)
int UtilsForUser::sceKernelLibcClock()
{
	// Get the processor clock used since the start of the process.

	//uint tick = ( uint )Environment::TickCount - _kernel->StartTick;
	//double totalus = TimeSpan::FromTicks( tick ).TotalMilliseconds / 1000.0;
	//return ( int )( ( uint )totalus );

	clock_t c = clock();
	return ( int )c;
}

// time_t sceKernelLibcTime(time_t *t); (/user/psputils.h:38)
int UtilsForUser::sceKernelLibcTime( IMemory^ memory, int t )
{
	// Get the time in seconds since the epoc (1st Jan 1970).

	time_t tm = time( NULL );
	uint tm32 = ( uint )tm;
	if( t != 0x0 )
	{
		int* pt = ( int* )MSI( memory )->Translate( t );
		*pt = tm32;
	}

	return tm32;
}

// int sceKernelLibcGettimeofday(struct timeval *tp, struct timezone *tzp); (/user/psputils.h:48)
int UtilsForUser::sceKernelLibcGettimeofday( IMemory^ memory, int tp, int tzp )
{
	// timeval: int sec, int usec
	// timezone:
	//	int tz_minuteswest - This is the number of minutes west of UTC.
	//	int tz_dsttime - If nonzero, Daylight Saving Time applies during some part of the year.
	// timezone is supposedly obsolete? unused? good!

	if( tp != 0x0 )
	{
		// usec = 1000000 per sec
		int64 time = _kernel->GetClockTime();
		uint tsec = ( uint )( time / 1000000 );
		uint tusec = ( uint )( time % 1000000 );

		int* ptp = ( int* )MSI( memory )->Translate( tp );
		*ptp = ( int )tsec;
		*( ptp + 1 ) = ( int )tusec;
	}
	else
		return -1;

	if( tzp != 0x0 )
	{
		int minutesWest = ( int )TimeZone::CurrentTimeZone->GetUtcOffset( DateTime::Today ).TotalMinutes;
		if( minutesWest < 0 )
		{
			// We wrap, so we don't return negative offsets
			minutesWest = -minutesWest + ( 12 * 60 );
		}
		int dst = DateTime::Today.IsDaylightSavingTime() == true ? 1 : 0;

		int* ptzp = ( int* )MSI( memory )->Translate( tzp );
		*ptzp = minutesWest;
		*( ptzp + 1 ) = dst;
	}
	
	return 0;
}
