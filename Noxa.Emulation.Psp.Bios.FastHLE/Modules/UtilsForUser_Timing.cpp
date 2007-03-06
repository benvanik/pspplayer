// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

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

	uint tick = ( uint )Environment::TickCount - _kernel->StartTick;
	double totalus = TimeSpan::FromTicks( tick ).TotalMilliseconds / 1000.0;
	
	// clock_t (uint)
	return ( int )( ( uint )totalus );
}

// time_t sceKernelLibcTime(time_t *t); (/user/psputils.h:38)
int UtilsForUser::sceKernelLibcTime( IMemory^ memory, int t )
{
	// Get the time in seconds since the epoc (1st Jan 1970).

	int time = ( int )_kernel->ClockTime;
	if( t != 0x0 )
		memory->WriteWord( t, 4, time );

	return time;
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
		uint time = _kernel->ClockTime;
		uint tsec = time / 1000000;
		uint tusec = time % 1000000;
		memory->WriteWord( tp, 4, ( int )tsec );
		memory->WriteWord( tp + 4, 4, ( int )tusec );
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
		memory->WriteWord( tzp, 4, minutesWest );
		memory->WriteWord( tzp + 4, 4, dst );
	}
	
	return 0;
}
