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
	unsafe partial class UtilsForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "UtilsForUser";
			}
		}

		#endregion

		#region State Management

		public UtilsForUser( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		#region Clock

		[Stateless]
		[BiosFunction( 0x91E4F6A7, "sceKernelLibcClock" )]
		// SDK location: /user/psputils.h:43
		// SDK declaration: clock_t sceKernelLibcClock();
		public int sceKernelLibcClock()
		{
			// Get the processor clock used since the start of the process.

			long currentTick;
			NativeMethods.QueryPerformanceCounter( out currentTick );
			// ticks/sec * sec/usec
			currentTick = ( currentTick - _kernel.StartTick ) / ( _kernel.TickFrequency / 1000000 );
			return ( int )currentTick;
		}

		[Stateless]
		[BiosFunction( 0x27CC57F0, "sceKernelLibcTime" )]
		// SDK location: /user/psputils.h:38
		// SDK declaration: time_t sceKernelLibcTime(time_t *t);
		public int sceKernelLibcTime( int t )
		{
			// Get the time in seconds since the epoc (1st Jan 1970).

			// usec = 1000000 per sec
			long time = _kernel.GetClockTime();
			uint tsec = ( uint )( time / 1000000 );
			if( t != 0x0 )
			{
				uint* pt = ( uint* )_memorySystem.Translate( ( uint )t );
				*pt = tsec;
			}

			return ( int )tsec;
		}

		[Stateless]
		[BiosFunction( 0x71EC4271, "sceKernelLibcGettimeofday" )]
		// SDK location: /user/psputils.h:48
		// SDK declaration: int sceKernelLibcGettimeofday(struct timeval *tp, struct timezone *tzp);
		public int sceKernelLibcGettimeofday( int tp, int tzp )
		{
			// timeval: int sec, int usec
			// timezone:
			//	int tz_minuteswest - This is the number of minutes west of UTC.
			//	int tz_dsttime - If nonzero, Daylight Saving Time applies during some part of the year.
			// timezone is supposedly obsolete? unused? good!

			if( tp != 0x0 )
			{
				// usec = 1000000 per sec
				long time = _kernel.GetClockTime();
				uint tsec = ( uint )( time / 1000000 );
				uint tusec = ( uint )( time % 1000000 );

				int* ptp = ( int* )_memorySystem.Translate( ( uint )tp );
				*ptp = ( int )tsec;
				*( ptp + 1 ) = ( int )tusec;
			}
			else
				return -1;

			if( tzp != 0x0 )
			{
				int minutesWest = ( int )TimeZone.CurrentTimeZone.GetUtcOffset( DateTime.Today ).TotalMinutes;
				if( minutesWest < 0 )
				{
					// We wrap, so we don't return negative offsets
					minutesWest = -minutesWest + ( 12 * 60 );
				}
				int dst = DateTime.Today.IsDaylightSavingTime() == true ? 1 : 0;

				int* ptzp = ( int* )_memorySystem.Translate( ( uint )tzp );
				*ptzp = minutesWest;
				*( ptzp + 1 ) = dst;
			}
			
			return 0;
		}

		#endregion

		#region Cache

		[Stateless]
		[BiosFunction( 0xBFA98062, "sceKernelDcacheInvalidateRange" )]
		// SDK location: /user/psputils.h:73
		// SDK declaration: void sceKernelDcacheInvalidateRange(const void *p, unsigned int size);
		public void sceKernelDcacheInvalidateRange( int p, int size )
		{
		}

		[Stateless]
		[BiosFunction( 0x79D1C3FA, "sceKernelDcacheWritebackAll" )]
		// SDK location: /user/psputils.h:53
		// SDK declaration: void sceKernelDcacheWritebackAll();
		public void sceKernelDcacheWritebackAll(){}

		[Stateless]
		[BiosFunction( 0xB435DEC5, "sceKernelDcacheWritebackInvalidateAll" )]
		// SDK location: /user/psputils.h:58
		// SDK declaration: void sceKernelDcacheWritebackInvalidateAll();
		public void sceKernelDcacheWritebackInvalidateAll(){}

		[Stateless]
		[BiosFunction( 0x3EE30821, "sceKernelDcacheWritebackRange" )]
		// SDK location: /user/psputils.h:63
		// SDK declaration: void sceKernelDcacheWritebackRange(const void *p, unsigned int size);
		public void sceKernelDcacheWritebackRange( int p, int size ){}

		[Stateless]
		[BiosFunction( 0x34B9FA9E, "sceKernelDcacheWritebackInvalidateRange" )]
		// SDK location: /user/psputils.h:68
		// SDK declaration: void sceKernelDcacheWritebackInvalidateRange(const void *p, unsigned int size);
		public void sceKernelDcacheWritebackInvalidateRange( int p, int size ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80001C4C, "sceKernelDcacheProbe" )]
		// SDK location: /kernel/psputilsforkernel.h:43
		// SDK declaration: int sceKernelDcacheProbe(void *addr);
		public int sceKernelDcacheProbe( int addr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FD31C9D, "sceKernelIcacheProbe" )]
		// SDK location: /kernel/psputilsforkernel.h:63
		// SDK declaration: int sceKernelIcacheProbe(const void *addr);
		public int sceKernelIcacheProbe( int addr ){ return Module.NotImplementedReturn; }

		#endregion
	}
}

/* GenerateStubsV2: auto-generated - F3D204AF */
