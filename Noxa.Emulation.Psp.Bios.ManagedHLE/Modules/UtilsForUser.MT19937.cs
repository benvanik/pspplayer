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
	unsafe partial class UtilsForUser
	{
		// MT Period parameters
		private const int N = 624;
		private const int M = 397;
		private const uint MATRIX_A = 0x9908b0df;	// constant vector a
		private const uint UPPER_MASK = 0x80000000;	// most significant w-r bits
		private const uint LOWER_MASK = 0x7fffffff;	// least significant r bits

		private static uint[] mag01 = new uint[]{ 0, MATRIX_A };

		/*typedef struct _SceKernelUtilsMt19937Context {
			unsigned int 	count;
			unsigned int 	state[624];
		} SceKernelUtilsMt19937Context;*/

		[Stateless]
		[BiosFunction( 0xE860E75E, "sceKernelUtilsMt19937Init" )]
		// SDK location: /user/psputils.h:96
		// SDK declaration: int sceKernelUtilsMt19937Init(SceKernelUtilsMt19937Context *ctx, u32 seed);
		public int sceKernelUtilsMt19937Init( int ctx, int seed )
		{
			byte* pctx = _memorySystem.Translate( ( uint )ctx );
			uint* mt = ( uint* )( pctx + 4 );
			int mti = *( ( int* )pctx );

			mt[ 0 ] = ( uint )seed & 0xffffffffU;
			for( mti = 1; mti < N; mti++ )
			{
				mt[ mti ] = ( uint )( 1812433253U * ( mt[ mti - 1 ] ^ ( mt[ mti - 1 ] >> 30 ) ) + mti );
				/* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
				/* In the previous versions, MSBs of the seed affect   */
				/* only MSBs of the array mt[].                        */
				/* 2002/01/09 modified by Makoto Matsumoto             */
				mt[ mti ] &= 0xffffffffU;
				/* for >32 bit machines */
			}

			*( ( int* )pctx ) = mti;

			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x06FB8A63, "sceKernelUtilsMt19937UInt" )]
		// SDK location: /user/psputils.h:104
		// SDK declaration: u32 sceKernelUtilsMt19937UInt(SceKernelUtilsMt19937Context *ctx);
		public int sceKernelUtilsMt19937UInt( int ctx )
		{
			byte* pctx = _memorySystem.Translate( (uint)ctx);
			uint* mt = ( uint* )( pctx + 4 );
			int mti = *( ( int* )pctx );

			// generates a random number on [0,0xffffffff]-interval
			uint y;
			/* mag01[x] = x * MATRIX_A  for x=0,1 */

			if( mti >= N )
			{
				// generate N words at one time
				int kk;

				if( mti == N + 1 )	   /* if init_genrand() has not been called, */
				{
					this.sceKernelUtilsMt19937Init( ctx, 5489 ); /* a default initial seed is used */
				}

				for( kk = 0; kk < N - M; kk++ )
				{
					y = ( mt[kk] & UPPER_MASK ) | ( mt[ kk + 1 ] & LOWER_MASK );
					mt[kk] = mt[ kk + M ] ^ ( y >> 1 ) ^ mag01[ y & 0x1UL ];
				}
				for( ; kk < N - 1; kk++ )
				{
					y = ( mt[kk] & UPPER_MASK ) | ( mt[ kk + 1 ] & LOWER_MASK );
					mt[kk] = mt[ kk + ( M - N ) ] ^ ( y >> 1 ) ^ mag01[ y & 0x1UL ];
				}
				y = ( mt[ N - 1 ] & UPPER_MASK ) | ( mt[0] & LOWER_MASK );
				mt[ N - 1 ] = mt[ M - 1 ] ^ ( y >> 1 ) ^ mag01[ y & 0x1UL ];

				mti = 0;
			}

			y = mt[ mti++ ];

			*( ( int* )pctx ) = mti;

			/* Tempering */
			y ^= (y >> 11);
			y ^= (y << 7) & 0x9d2c5680U;
			y ^= (y << 15) & 0xefc60000U;
			y ^= (y >> 18);

			return ( int )y;
		}

		// What follows is the copywright notice from:
		//  http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/MT2002/CODES/mt19937ar.c
		/* 
		   A C-program for MT19937, with initialization improved 2002/1/26.
		   Coded by Takuji Nishimura and Makoto Matsumoto.

		   Before using, initialize the state by using init_genrand(seed)  
		   or init_by_array(init_key, key_length).

		   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
		   All rights reserved.                          

		   Redistribution and use in source and binary forms, with or without
		   modification, are permitted provided that the following conditions
		   are met:

			 1. Redistributions of source code must retain the above copyright
				notice, this list of conditions and the following disclaimer.

			 2. Redistributions in binary form must reproduce the above copyright
				notice, this list of conditions and the following disclaimer in the
				documentation and/or other materials provided with the distribution.

			 3. The names of its contributors may not be used to endorse or promote 
				products derived from this software without specific prior written 
				permission.

		   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
		   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
		   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
		   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
		   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
		   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
		   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
		   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
		   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
		   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
		   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


		   Any feedback is very welcome.
		   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
		   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
		*/
	}
}
