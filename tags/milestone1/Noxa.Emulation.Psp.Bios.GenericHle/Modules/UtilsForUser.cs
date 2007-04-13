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

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class UtilsForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public UtilsForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "UtilsForUser";
			}
		}

		#endregion

		// NOTE: we don't do any caching, so all the invalidate stuff can be nop'ed - unfortunately
		// I don't think we can do the same thing with the reads

		[BiosStubStateless]
		[BiosStub( 0xbfa98062, "sceKernelDcacheInvalidateRange", false, 2 )]
		public int sceKernelDcacheInvalidateRange( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *p
			// a1 = unsigned int size
			
			return 0;
		}

		[BiosStub( 0xc8186a58, "sceKernelUtilsMd5Digest", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMd5Digest( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = u8 *data
			// a1 = u32 size
			// a2 = u8 *digest
			
			// int
			return 0;
		}

		[BiosStub( 0x9e5c5086, "sceKernelUtilsMd5BlockInit", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMd5BlockInit( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsMd5Context *ctx
			
			// int
			return 0;
		}

		[BiosStub( 0x61e1e525, "sceKernelUtilsMd5BlockUpdate", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMd5BlockUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsMd5Context *ctx
			// a1 = u8 *data
			// a2 = u32 size
			
			// int
			return 0;
		}

		[BiosStub( 0xb8d24e78, "sceKernelUtilsMd5BlockResult", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMd5BlockResult( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsMd5Context *ctx
			// a1 = u8 *digest
			
			// int
			return 0;
		}

		[BiosStub( 0x840259f1, "sceKernelUtilsSha1Digest", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsSha1Digest( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = u8 *data
			// a1 = u32 size
			// a2 = u8 *digest
			
			// int
			return 0;
		}

		[BiosStub( 0xf8fcd5ba, "sceKernelUtilsSha1BlockInit", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsSha1BlockInit( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsSha1Context *ctx
			
			// int
			return 0;
		}

		[BiosStub( 0x346f6da8, "sceKernelUtilsSha1BlockUpdate", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsSha1BlockUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsSha1Context *ctx
			// a1 = u8 *data
			// a2 = u32 size
			
			// int
			return 0;
		}

		[BiosStub( 0x585f1c09, "sceKernelUtilsSha1BlockResult", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsSha1BlockResult( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsSha1Context *ctx
			// a1 = u8 *digest
			
			// int
			return 0;
		}

		[BiosStub( 0xe860e75e, "sceKernelUtilsMt19937Init", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMt19937Init( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsMt19937Context *ctx
			// a1 = u32 seed
			
			// int
			return 0;
		}

		[BiosStub( 0x06fb8a63, "sceKernelUtilsMt19937UInt", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelUtilsMt19937UInt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelUtilsMt19937Context *ctx
			
			// u32
			return 0;
		}

		[BiosStub( 0x37fb5c42, "sceKernelGetGPI", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetGPI( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6ad345d7, "sceKernelSetGPO", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetGPO( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		// _CLOCKS_PER_SEC_ - 1000000 - clock ticks per second, I think (microseconds)

		[BiosStubStateless]
		[BiosStub( 0x91e4f6a7, "sceKernelLibcClock", true, 0 )]
		public int sceKernelLibcClock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// Get the processor clock used since the start of the process.

			uint tick = ( uint )Environment.TickCount - _kernel.StartTick;
			double totalus = TimeSpan.FromTicks( tick ).TotalMilliseconds / 1000.0;
			
			// clock_t (uint)
			return ( int )( ( uint )totalus );
		}

		[BiosStubStateless]
		[BiosStub( 0x27cc57f0, "sceKernelLibcTime", true, 1 )]
		public int sceKernelLibcTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = time_t *t (uint)

			// Get the time in seconds since the epoc (1st Jan 1970).

			int time = ( int )_kernel.ClockTime;
			if( a0 != 0x0 )
				memory.WriteWord( a0, 4, time );
			
			// time_t (uint)
			return time;
		}

		[BiosStubStateless]
		[BiosStub( 0x71ec4271, "sceKernelLibcGettimeofday", true, 2 )]
		public int sceKernelLibcGettimeofday( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = struct timeval *tp
			// a1 = struct timezone *tzp

			// timeval: int sec, int usec
			// timezone:
			//	int tz_minuteswest - This is the number of minutes west of UTC.
			//	int tz_dsttime - If nonzero, Daylight Saving Time applies during some part of the year.
			// timezone is supposedly obsolete? unused? good!

			if( a0 != 0x0 )
			{
				// usec = 1000000 per sec
				uint time = _kernel.ClockTime;
				uint tsec = time / 1000000;
				uint tusec = time % 1000000;
				memory.WriteWord( a0, 4, ( int )tsec );
				memory.WriteWord( a0 + 4, 4, ( int )tusec );
			}
			else
				return -1;

			if( a1 != 0x0 )
			{
				int minutesWest = ( int )TimeZone.CurrentTimeZone.GetUtcOffset( DateTime.Today ).TotalMinutes;
				if( minutesWest < 0 )
				{
					// We wrap, so we don't return negative offsets
					minutesWest = -minutesWest + ( 12 * 60 );
				}
				int dst = DateTime.Today.IsDaylightSavingTime() == true ? 1 : 0;
				memory.WriteWord( a1, 4, minutesWest );
				memory.WriteWord( a1 + 4, 4, dst );
			}
			
			// int
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0x79d1c3fa, "sceKernelDcacheWritebackAll", false, 1 )]
		public int sceKernelDcacheWritebackAll( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void
			
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0xb435dec5, "sceKernelDcacheWritebackInvalidateAll", false, 1 )]
		public int sceKernelDcacheWritebackInvalidateAll( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void
			
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0x3ee30821, "sceKernelDcacheWritebackRange", false, 2 )]
		public int sceKernelDcacheWritebackRange( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *p
			// a1 = unsigned int size
			
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0x34b9fa9e, "sceKernelDcacheWritebackInvalidateRange", false, 2 )]
		public int sceKernelDcacheWritebackInvalidateRange( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *p
			// a1 = unsigned int size
			
			return 0;
		}

		[BiosStub( 0x80001c4c, "sceKernelDcacheProbe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDcacheProbe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x77dff087, "UtilsForUser_0x77DFF087", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x16641d70, "sceKernelDcacheReadTag", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDcacheReadTag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4fd31c9d, "sceKernelIcacheProbe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelIcacheProbe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfb05fad0, "sceKernelIcacheReadTag", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelIcacheReadTag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0x920f104a, "sceKernelIcacheInvalidateAll", false, 0 )]
		public int sceKernelIcacheInvalidateAll( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStubStateless]
		[BiosStub( 0xc2df770e, "sceKernelIcacheInvalidateRange", false, 0 )]
		public int sceKernelIcacheInvalidateRange( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
