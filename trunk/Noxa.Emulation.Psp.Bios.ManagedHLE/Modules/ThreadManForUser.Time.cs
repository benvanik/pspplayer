// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	partial class ThreadManForUser
	{
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

		//[NotImplemented]
		//[Stateless]
		//[BiosFunction( 0x0E927AED, "_sceKernelReturnFromTimerHandler" )]
		//// SDK location: /user/pspthreadman.h:1447
		//// SDK declaration: void _sceKernelReturnFromTimerHandler();
		//public void _sceKernelReturnFromTimerHandler()
		//{
		//}

		[Stateless]
		[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )]
		// SDK location: /user/pspthreadman.h:1463
		// SDK declaration: int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock);
		public int sceKernelUSec2SysClock( int usec, int clock )
		{
			// sysclock is us count, so this is easy
			unsafe
			{
				int* p = ( int* )_memorySystem.Translate( ( uint )clock );
				*p = usec;
				*( p + 1 ) = 0;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )]
		// SDK location: /user/pspthreadman.h:1472
		// SDK declaration: SceInt64 sceKernelUSec2SysClockWide(unsigned int usec);
		public long sceKernelUSec2SysClockWide( int usec )
		{
			return ( long )( ulong )( uint )usec;
		}

		[Stateless]
		[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )]
		// SDK location: /user/pspthreadman.h:1483
		// SDK declaration: int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *low, unsigned int *high);
		public int sceKernelSysClock2USec( int clock, int low, int high )
		{
			unsafe
			{
				ulong c = *( ulong* )_memorySystem.Translate( ( uint )clock );
				uint* ps = ( uint* )_memorySystem.Translate( ( uint )low );
				uint* pus = ( uint* )_memorySystem.Translate( ( uint )high );
				*ps = ( uint )( c / 1000000 );
				*pus = ( uint )( c % 1000000 );
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )]
		// SDK location: /user/pspthreadman.h:1494
		// SDK declaration: int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *low, unsigned int *high);
		public int sceKernelSysClock2USecWide( long clock, int low, int high )
		{
			unsafe
			{
				uint* ps = ( uint* )_memorySystem.Translate( ( uint )low );
				uint* pus = ( uint* )_memorySystem.Translate( ( uint )high );
				*ps = ( uint )( clock / 1000000 );
				*pus = ( uint )( clock % 1000000 );
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )]
		// SDK location: /user/pspthreadman.h:1503
		// SDK declaration: int sceKernelGetSystemTime(SceKernelSysClock *time);
		public int sceKernelGetSystemTime( int time )
		{
			long currentTick;
			NativeMethods.QueryPerformanceCounter( out currentTick );
			// Do ticks/sec to sec/usec
			currentTick = ( currentTick - _kernel.StartTick ) / ( _kernel.TickFrequency / 1000000 );

			unsafe
			{
				long* ptime = ( long* )_memorySystem.Translate( ( uint )time );
				*ptime = currentTick;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )]
		// SDK location: /user/pspthreadman.h:1510
		// SDK declaration: SceInt64 sceKernelGetSystemTimeWide();
		public long sceKernelGetSystemTimeWide()
		{
			long currentTick;
			NativeMethods.QueryPerformanceCounter( out currentTick );
			// Do ticks/sec to sec/usec
			return ( currentTick - _kernel.StartTick ) / ( _kernel.TickFrequency / 1000000 );
		}

		[Stateless]
		[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )]
		// SDK location: /user/pspthreadman.h:1517
		// SDK declaration: unsigned int sceKernelGetSystemTimeLow();
		public int sceKernelGetSystemTimeLow()
		{
			long currentTick;
			NativeMethods.QueryPerformanceCounter( out currentTick );
			// Do ticks/sec to sec/usec
			return ( int )( ( currentTick - _kernel.StartTick ) / ( _kernel.TickFrequency / 1000000 ) );
		}
	}
}
