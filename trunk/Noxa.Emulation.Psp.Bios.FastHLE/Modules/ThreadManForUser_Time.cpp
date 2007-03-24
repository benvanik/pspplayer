// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <time.h>

#include "ThreadManForUser.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

extern int64 _startTick;

// These can all be native
//0x0E927AED _sceKernelReturnFromTimerHandler
//0x110DEC9A sceKernelUSec2SysClock
//0xC8CD158C sceKernelUSec2SysClockWide
//0xBA6B92E2 sceKernelSysClock2USec
//0xE1619D7C sceKernelSysClock2USecWide
//0xDB738F35 sceKernelGetSystemTime
//0x82BC5777 sceKernelGetSystemTimeWide
//0x369ED59D sceKernelGetSystemTimeLow

// SysTime is low, high

// void _sceKernelReturnFromTimerHandler(); (/user/pspthreadman.h:1447)
void ThreadManForUser::_sceKernelReturnFromTimerHandler(){}

// int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock); (/user/pspthreadman.h:1463)
int ThreadManForUser::sceKernelUSec2SysClock( IMemory^ memory, int usec, int clock )
{
	// sysclock is us count, so this is easy
	memory->WriteWord( clock, 4, usec );
	memory->WriteWord( clock + 4, 4, 0 );

	return 0;
}

// SceInt64 sceKernelUSec2SysClockWide(unsigned int usec); (/user/pspthreadman.h:1472)
int64 ThreadManForUser::sceKernelUSec2SysClockWide( int usec )
{
	// sysclock is us count, so this is easy
	return ( int64 )( uint )usec;
}

// int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *sec, unsigned int *usec); (/user/pspthreadman.h:1483)
int ThreadManForUser::sceKernelSysClock2USec( IMemory^ memory, int clock, int sec, int usec )
{
	// sysclock is us count - we need to get seconds + usec
	int clo = memory->ReadWord( clock );
	int chi = memory->ReadWord( clock + 4 );
	int64 c = memory->ReadDoubleWord( clock );

	memory->WriteWord( sec, 4, ( int )( c / 1000000 ) );
	memory->WriteWord( usec, 4, ( int )( c % 1000000 ) );
	
	return 0;
}

// int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *sec, unsigned int *usec); (/user/pspthreadman.h:1494)
int ThreadManForUser::sceKernelSysClock2USecWide( IMemory^ memory, int64 clock, int sec, int usec )
{
	memory->WriteWord( sec, 4, ( int )( clock / 1000000 ) );
	memory->WriteWord( usec, 4, ( int )( clock % 1000000 ) );
	
	return 0;
}

// int sceKernelGetSystemTime(SceKernelSysClock *time); (/user/pspthreadman.h:1503)
int ThreadManForUser::sceKernelGetSystemTime( IMemory^ memory, int time )
{
	// microseconds since game started
	int64 ticks = DateTime::Now.Ticks - _kernel->StartTick;
	// 10000000 ticks/sec - 1000000 us/sec
	ticks /= 10;

	int lo = ( int )ticks;
	int hi = ( int )( ticks >> 32 );

	memory->WriteWord( time, 4, lo );
	memory->WriteWord( time + 4, 4, hi );

	return 0;
}

// SceInt64 sceKernelGetSystemTimeWide(); (/user/pspthreadman.h:1510)
int64 ThreadManForUser::sceKernelGetSystemTimeWide()
{
	// microseconds since game started
	int64 ticks = DateTime::Now.Ticks - _kernel->StartTick;
	return ticks / 10;
}

#pragma unmanaged
int sceKernelGetSystemTimeLowN()
{
	ULARGE_INTEGER time;
	GetSystemTimeAsFileTime( ( FILETIME* )&time );
	return ( int )( ( time.QuadPart - _startTick ) / 10 );

}
#pragma managed

// unsigned int sceKernelGetSystemTimeLow(); (/user/pspthreadman.h:1517)
int ThreadManForUser::sceKernelGetSystemTimeLow()
{
	// microseconds since game started
	int64 ticks = DateTime::Now.Ticks - _kernel->StartTick;
	ticks /= 10;
	return ( int )ticks;
}
