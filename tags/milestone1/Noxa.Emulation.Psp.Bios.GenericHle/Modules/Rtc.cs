// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Rtc : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public Rtc( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceRtc";
			}
		}

		#endregion

		#region Interop

		[DllImport( "Kernel32.dll", SetLastError = false )]
		private static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceFrequency( out long lpFrequency );

		#endregion

		[BiosStubStateless]
		[BiosStub( 0xc41c2853, "sceRtcGetTickResolution", true, 0 )]
		public int sceRtcGetTickResolution( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			//long temp = 0;

			// Get timer frequency
			//if( ( QueryPerformanceFrequency( out temp ) == false ) ||
			//    ( temp == 0 ) )
			//{
			//    // Uh oh - performance counter not avail?
			//    Debug.WriteLine( "sceRtcGetTickResolution: could not query frequency of the performance counter" );
			//}

			// u32 - ticks per second
			//return ( int )( unchecked( ( uint )temp ) );
			return unchecked( ( int )TimeSpan.TicksPerSecond );
		}

		[BiosStubStateless]
		[BiosStub( 0x3f7ad767, "sceRtcGetCurrentTick", true, 1 )]
		public int sceRtcGetCurrentTick( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = u64 *tick

			//long temp = 0;
			//if( QueryPerformanceCounter( out temp ) == false )
			//	return -1;

			long temp = DateTime.Now.Ticks;

			int lower = unchecked( ( int )( ( ulong )0x00000000FFFFFFFF & ( ulong )temp ) );
			int upper = unchecked( ( int )( ( ulong )temp >> 32 ) );

			memory.WriteWord( a0, 4, upper );
			memory.WriteWord( a0 + 4, 4, lower );

			// int
			return 0;
		}

		[BiosStub( 0x029ca3b3, "sceRtc_0x029CA3B3", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x011f03c1, "sceRtc_0x011F03C1", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4cfa57b0, "sceRtcGetCurrentClock", true, 2 )]
		[BiosStubIncomplete]
		public int sceRtcGetCurrentClock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = pspTime *time
			// a1 = int tz
			
			// int
			return 0;
		}

		[BiosStub( 0xe7c27d1b, "sceRtcGetCurrentClockLocalTime", true, 1 )]
		[BiosStubIncomplete]
		public int sceRtcGetCurrentClockLocalTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = pspTime *time
			
			// int
			return 0;
		}

		[BiosStub( 0x203ceb0d, "sceRtc_0x203CEB0D", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x34885e0d, "sceRtcConvertUtcToLocalTime", true, 2 )]
		[BiosStubIncomplete]
		public int sceRtcConvertUtcToLocalTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const u64 *tickUTC
			// a1 = u64 *tickLocal
			
			// int
			return 0;
		}

		[BiosStub( 0x62685e98, "sceRtc_0x62685E98", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x779242a2, "sceRtcConvertLocalTimeToUTC", true, 2 )]
		[BiosStubIncomplete]
		public int sceRtcConvertLocalTimeToUTC( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const u64 *tickLocal
			// a1 = u64 *tickUTC
			
			// int
			return 0;
		}

		[BiosStub( 0x42307a17, "sceRtcIsLeapYear", true, 1 )]
		[BiosStubIncomplete]
		public int sceRtcIsLeapYear( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int year
			
			// int
			return 0;
		}

		[BiosStub( 0x05ef322c, "sceRtcGetDaysInMonth", true, 2 )]
		[BiosStubIncomplete]
		public int sceRtcGetDaysInMonth( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int year
			// a1 = int month
			
			// int
			return 0;
		}

		[BiosStub( 0x57726bc1, "sceRtcGetDayOfWeek", true, 3 )]
		[BiosStubIncomplete]
		public int sceRtcGetDayOfWeek( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int year
			// a1 = int month
			// a2 = int day
			
			// int
			return 0;
		}

		[BiosStub( 0x4b1b5e82, "sceRtcCheckValid", true, 1 )]
		[BiosStubIncomplete]
		public int sceRtcCheckValid( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const pspTime *date
			
			// int
			return 0;
		}

		[BiosStub( 0x3a807cc8, "sceRtcSetTime_t", false, 0 )]
		[BiosStubIncomplete]
		public int sceRtcSetTime_t( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x27c4594c, "sceRtcGetTime_t", false, 0 )]
		[BiosStubIncomplete]
		public int sceRtcGetTime_t( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf006f264, "sceRtcSetDosTime", false, 0 )]
		[BiosStubIncomplete]
		public int sceRtcSetDosTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x36075567, "sceRtcGetDosTime", false, 0 )]
		[BiosStubIncomplete]
		public int sceRtcGetDosTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
