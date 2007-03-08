// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceRtc.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

#define TICKSPERUS		10
#define TICKSPERSEC		10000000
#define TICKSPERMIN		600000000
#define TICKSPERHOUR	36000000000
#define TICKSPERDAY		864000000000
#define TICKSPERWEEK	6048000000000
#define TICKSPERMONTH	24192000000000		// 4 weeks/month?
#define TICKSPERYEAR	314496000000000		// 52 weeks/year?

int sceRtcGetTickResolutionN();
int sceRtcGetCurrentTickN( LARGE_INTEGER* tick );

void* sceRtc::QueryNativePointer( uint nid )
{
	switch( nid )
	{
	case 0xC41C2853:
		return &sceRtcGetTickResolutionN;
	case 0x3F7AD767:
		return &sceRtcGetCurrentTickN;
	};

	return 0;
}

// u32 sceRtcGetTickResolution(); (/rtc/psprtc.h:46)
int sceRtc::sceRtcGetTickResolution()
{
	LARGE_INTEGER frequency;
	QueryPerformanceFrequency( &frequency );
	return ( uint )frequency.LowPart;
}

#pragma unmanaged
int sceRtcGetTickResolutionN()
{
	LARGE_INTEGER frequency;
	QueryPerformanceFrequency( &frequency );
	return ( uint )frequency.LowPart;
}
#pragma managed

// int sceRtcGetCurrentTick(u64 *tick); (/rtc/psprtc.h:54)
int sceRtc::sceRtcGetCurrentTick( IMemory^ memory, int tick )
{
	LARGE_INTEGER li;
	QueryPerformanceCounter( &li );
	memory->WriteWord( tick, 4, li.LowPart );
	memory->WriteWord( tick + 4, 4, li.HighPart );
	return 0;
}

#pragma unmanaged
int sceRtcGetCurrentTickN( LARGE_INTEGER* tick )
{
	QueryPerformanceCounter( tick );
	return 0;
}
#pragma managed

// int sceRtcGetCurrentClock(pspTime *time, int tz); (/rtc/psprtc.h:63)
int sceRtc::sceRtcGetCurrentClock( IMemory^ memory, int time, int tz ){ return NISTUBRETURN; }

// int sceRtcGetCurrentClockLocalTime(pspTime *time); (/rtc/psprtc.h:71)
int sceRtc::sceRtcGetCurrentClockLocalTime( IMemory^ memory, int time ){ return NISTUBRETURN; }

// int sceRtcConvertUtcToLocalTime(const u64* tickUTC, u64* tickLocal); (/rtc/psprtc.h:80)
int sceRtc::sceRtcConvertUtcToLocalTime( IMemory^ memory, int tickUTC, int tickLocal ){ return NISTUBRETURN; }

// int sceRtcConvertLocalTimeToUTC(const u64* tickLocal, u64* tickUTC); (/rtc/psprtc.h:89)
int sceRtc::sceRtcConvertLocalTimeToUTC( IMemory^ memory, int tickLocal, int tickUTC ){ return NISTUBRETURN; }

// int sceRtcIsLeapYear(int year); (/rtc/psprtc.h:97)
int sceRtc::sceRtcIsLeapYear( int year ){ return NISTUBRETURN; }

// int sceRtcGetDaysInMonth(int year, int month); (/rtc/psprtc.h:106)
int sceRtc::sceRtcGetDaysInMonth( int year, int month ){ return NISTUBRETURN; }

// int sceRtcGetDayOfWeek(int year, int month, int day); (/rtc/psprtc.h:116)
int sceRtc::sceRtcGetDayOfWeek( int year, int month, int day ){ return NISTUBRETURN; }

// int sceRtcCheckValid(const pspTime* date); (/rtc/psprtc.h:124)
int sceRtc::sceRtcCheckValid( IMemory^ memory, int date ){ return NISTUBRETURN; }

// int sceRtcSetTime_t(pspTime* date, const time_t time); (/rtc/psprtc.h:244)
int sceRtc::sceRtcSetTime_t( IMemory^ memory, int date, int time ){ return NISTUBRETURN; }

// int sceRtcGetTime_t(const pspTime* date, time_t *time); (/rtc/psprtc.h:245)
int sceRtc::sceRtcGetTime_t( IMemory^ memory, int date, int time ){ return NISTUBRETURN; }

// int sceRtcSetDosTime(pspTime* date, u32 dosTime); (/rtc/psprtc.h:246)
int sceRtc::sceRtcSetDosTime( IMemory^ memory, int date, int dosTime ){ return NISTUBRETURN; }

// int sceRtcGetDosTime(pspTime* date, u32 dosTime); (/rtc/psprtc.h:247)
int sceRtc::sceRtcGetDosTime( IMemory^ memory, int date, int dosTime ){ return NISTUBRETURN; }

// int sceRtcSetWin32FileTime(pspTime* date, u64* win32Time); (/rtc/psprtc.h:248)
int sceRtc::sceRtcSetWin32FileTime( IMemory^ memory, int date, int win32Time ){ return NISTUBRETURN; }

// int sceRtcGetWin32FileTime(pspTime* date, u64* win32Time); (/rtc/psprtc.h:249)
int sceRtc::sceRtcGetWin32FileTime( IMemory^ memory, int date, int win32Time ){ return NISTUBRETURN; }

// int sceRtcSetTick(pspTime* date, const u64* tick); (/rtc/psprtc.h:133)
int sceRtc::sceRtcSetTick( IMemory^ memory, int date, int tick ){ return NISTUBRETURN; }

// int sceRtcGetTick(const pspTime* date, u64 *tick); (/rtc/psprtc.h:142)
int sceRtc::sceRtcGetTick( IMemory^ memory, int date, int tick ){ return NISTUBRETURN; }

// int sceRtcCompareTick(const u64* tick1, const u64* tick2); (/rtc/psprtc.h:151)
int sceRtc::sceRtcCompareTick( IMemory^ memory, int tick1, int tick2 ){ return NISTUBRETURN; }

// int sceRtcTickAddTicks(u64* destTick, const u64* srcTick, u64 numTicks); (/rtc/psprtc.h:161)
int sceRtc::sceRtcTickAddTicks( IMemory^ memory, int destTick, int srcTick, int numTicks )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numTicks;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddMicroseconds(u64* destTick, const u64* srcTick, u64 numMS); (/rtc/psprtc.h:171)
int sceRtc::sceRtcTickAddMicroseconds( IMemory^ memory, int destTick, int srcTick, int numMS )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numMS * TICKSPERUS;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddSeconds(u64* destTick, const u64* srcTick, u64 numSecs); (/rtc/psprtc.h:181)
int sceRtc::sceRtcTickAddSeconds( IMemory^ memory, int destTick, int srcTick, int numSecs )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numSecs * TICKSPERSEC;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddMinutes(u64* destTick, const u64* srcTick, u64 numMins); (/rtc/psprtc.h:191)
int sceRtc::sceRtcTickAddMinutes( IMemory^ memory, int destTick, int srcTick, int numMins )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numMins * TICKSPERMIN;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddHours(u64* destTick, const u64* srcTick, int numHours); (/rtc/psprtc.h:201)
int sceRtc::sceRtcTickAddHours( IMemory^ memory, int destTick, int srcTick, int numHours )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numHours * TICKSPERHOUR;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddDays(u64* destTick, const u64* srcTick, int numDays); (/rtc/psprtc.h:211)
int sceRtc::sceRtcTickAddDays( IMemory^ memory, int destTick, int srcTick, int numDays )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numDays * TICKSPERDAY;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddWeeks(u64* destTick, const u64* srcTick, int numWeeks); (/rtc/psprtc.h:221)
int sceRtc::sceRtcTickAddWeeks( IMemory^ memory, int destTick, int srcTick, int numWeeks )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numWeeks * TICKSPERWEEK;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddMonths(u64* destTick, const u64* srcTick, int numMonths); (/rtc/psprtc.h:232)
int sceRtc::sceRtcTickAddMonths( IMemory^ memory, int destTick, int srcTick, int numMonths )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numMonths * TICKSPERMONTH;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcTickAddYears(u64* destTick, const u64* srcTick, int numYears); (/rtc/psprtc.h:242)
int sceRtc::sceRtcTickAddYears( IMemory^ memory, int destTick, int srcTick, int numYears )
{
	int64 st = memory->ReadDoubleWord( srcTick );
	st += numYears * TICKSPERYEAR;
	memory->WriteDoubleWord( destTick, st );

	return 0;
}

// int sceRtcParseDateTime(u64 *destTick, const char *dateString); (/rtc/psprtc.h:251)
int sceRtc::sceRtcParseDateTime( IMemory^ memory, int destTick, int dateString ){ return NISTUBRETURN; }
