// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define USE

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
	// sceSas* is actually a static wrapper library that calles the real __sceSas*
	// functions. __sceSas* have an additional parameter in front of most functions, the sasCore
	// which is normally hidden from sceSas users
	// but since __sceSas is what's imported, we have to deal with it

	// There are more layers on top of sceSas but we don't need to care
	// if we emulate sceSas properly.

	// Since only one sasCore will ever be active simultaneously (we would hope..)
	// we don't need to mess around with allocating sceCores.

	unsafe class sceSas : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSasCore";
			}
		}

		#endregion

		#region State Management

		public sceSas( Kernel kernel )
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

		private const int coreID = 0x12345678;

		private struct Voice
		{
			public bool finished;
			public int a, d, s, r;
			public int aMode, dMode, sMode, rMode; //curve shapes

			public int envelopeValue; //should be updated when mixing

			public int volumeLeft, volumeRight;
			public int volumeEffectLeft, volumeEffectRight;
			public int loopMode;

			public ulong playbackPosition; //32.32 fixed point, for example
			public ulong playbackDelta;


			public short[] unpackedData;

			public int samplePointer;
			public int sampleSize;
		}

		private Voice[] voices = new Voice[ 32 ];

		private double[ , ] vagC = new double[ 5, 2 ] { { 0.0, 0.0 },
			{   60.0 / 64.0,  0.0 },
			{  115.0 / 64.0, -52.0 / 64.0 },
			{   98.0 / 64.0, -55.0 / 64.0 },
			{  122.0 / 64.0, -60.0 / 64.0 } };

#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x42778a9f, "__sceSasInit" )]
		public int __sceSasInit()
		{
			for( int i = 0; i < 32; i++ )
			{
				voices[ i ].finished = true;
			}
			return coreID;
		}

		//This one does the mixing - it outputs 256 stereo samples (1024 bytes)
		//to the specified location.
		//This will then be played back with sceAudio functions.
		//Alternatively, we can just skip this and output directly, but any CPU effects
		//applied on top will then not be heard.
#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xa3589d81, "__sceSasCore" )]
		public int __sceSasCore( int sasCore, int outBufPtr )
		{
			return 0;
		}

#if !USE
		[NotImplemented]
		[DontTrace]
#endif
		[Stateless]
		[BiosFunction( 0x68a46b95, "__sceSasGetEndFlag" )]
		public int __sceSasGetEndFlag( int sasCore )
		{
			int f = 0;
			for( int i = 0; i < 32; i++ )
			{
				if( voices[ i ].finished )
				{
					f |= ( 1 << i );
				}
			}
			return f;
		}

#if !USE
		[NotImplemented]
#endif
		//[DontTrace]
		[Stateless]
		[BiosFunction( 0x440ca7d8, "__sceSasSetVolume" )]
		public int __sceSasSetVolume( int sasCore, int num, int l, int el, int r, int er )
		{
			//if (sasCore != coreID) ; //Do something
			voices[ num ].volumeLeft = l;
			voices[ num ].volumeRight = r;
			voices[ num ].volumeEffectLeft = el;
			voices[ num ].volumeEffectRight = er;
			return 0;
		}

#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xad84d37f, "__sceSasSetPitch" )]
		public int __sceSasSetPitch( int sasCore, int num, int pitch )
		{
			voices[ num ].playbackDelta = ( ulong )pitch; //TODO: Rather, some function of pitch
			return 0;
		}

		//This is NOT getting called, at least not by AI Go
		//It should be :(
#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x99944089, "__sceSasSetVoice" )]
		public int __sceSasSetVoice( int sasCore, int num, int vagPtr, int size, int loopmode )
		{
			//Real VAG header is 0x30 bytes behind the vagAddr
			//The existance of this header can probably not be relied on 
			//since FMOD docs http://www.fmod.org/docs/tutorials/psp.htm 
			//indicate that it loads all the samples contiguosly

			//Regardless, the info in the parameters to this function should be enough
			//I haven't been able to find any decent documentation of the VAG header anyway

			//VAG is 3.5:1 compression
			if( vagPtr == voices[ num ].samplePointer && size == voices[ num ].sampleSize )
			{
				//We got the same sample as last time - no need to do anything
				return 0; //all OK
			}

			voices[ num ].unpackedData = new short[ size ];
			voices[ num ].samplePointer = vagPtr;
			voices[ num ].sampleSize = size;
			voices[ num ].loopMode = loopmode;

			byte* data = _memorySystem.Translate( ( uint )vagPtr );

			int dataPtr = 0;
			int outPtr = 0;

			//based on VAG-Depack, hacked by bITmASTER@bigfoot.com, V0.1
			double[] samples = new double[ 28 ];
			while( outPtr < size )  //for safety only ; should be possible to be while (true) as well, due to the end flag
			{
				int predict_nr = data[ dataPtr++ ];
				int shift_factor = predict_nr & 0xf;
				predict_nr >>= 4;
				int flags = data[ dataPtr++ ];                           // flags
				if( flags == 7 )
					break;
				//Read raw samples
				double s_1 = 0.0;
				double s_2 = 0.0;
				for( int i = 0; i < 28; i += 2 )
				{
					int d = data[ dataPtr++ ];
					int s = ( int )( short )( ( d & 0xf ) << 12 );
					samples[ i ] = ( double )( s >> shift_factor );
					s = ( int )( short )( ( d & 0xf0 ) << 8 );
					samples[ i + 1 ] = ( double )( s >> shift_factor );
				}
				//Adjust them (ADPCM) and convert them to our 16-bit voice data
				for( int i = 0; i < 28; i++ )
				{
					samples[ i ] = samples[ i ] + s_1 * vagC[ predict_nr, 0 ] + s_2 * vagC[ predict_nr, 1 ];
					s_2 = s_1;
					s_1 = samples[ i ];
					int d = ( int )( samples[ i ] + 0.5 );
					if( d > 32767 )
						d = 32767;
					if( d < -32768 )
						d = 32768;
					voices[ num ].unpackedData[ outPtr++ ] = ( short )d;
				}
			}
			return 0; //success
		}

#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x019b25eb, "__sceSasSetADSR" )]
		public int __sceSasSetADSR( int sasCore, int num, int flag, int a, int d, int s, int r )
		{
			voices[ num ].a = a;
			voices[ num ].d = d;
			voices[ num ].s = s;
			voices[ num ].r = r;
			return 0;
		}

#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x74ae582a, "__sceSasGetEnvelopeHeight" )]
		public int __sceSasGetEnvelopeHeight( int sasCore, int num )
		{
			return voices[ num ].envelopeValue; //max : 0x40000000
		}

#if !USE
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x9ec3676a, "__sceSasSetADSRmode" )]
		public int __sceSasSetADSRmode( int sasCore, int num, int aMode, int dMode, int sMode, int rMode )
		{
			voices[ num ].aMode = aMode;
			voices[ num ].dMode = dMode;
			voices[ num ].sMode = sMode;
			voices[ num ].rMode = rMode;
			return 0;
		}

		//Start a voice
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76f01aca, "__sceSasSetKeyOn" )]
		public int __sceSasSetKeyOn( int sasCore, int num )
		{
			return Module.NotImplementedReturn;
		}

		//Stop a voice
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xa0cf2fa4, "__sceSasSetKeyOff" )]
		public int __sceSasSetKeyOff( int sasCore, int num )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xb7660a23, "__sceSasSetNoise" )]
		public int __sceSasSetNoise()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5f9529f6, "__sceSasSetSL" )]
		public int __sceSasSetSL()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xcbcd4f79, "__sceSasSetSimpleADSR" )]
		public int __sceSasSetSimpleADSR()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xd5a229c9, "__sceSasRevEVOL" )]
		public int __sceSasRevEVOL()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33d4ab37, "__sceSasRevType" )]
		public int __sceSasRevType()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x267a6dd2, "__sceSasRevParam" )]
		public int __sceSasRevParam()
		{
			return Module.NotImplementedReturn;
		}

		//[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2c8e6ab3, "__sceSasGetPauseFlag" )]
		public int __sceSasGetPauseFlag()
		{
			//return Module.NotImplementedReturn;
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x787d04d5, "__sceSasSetPause" )]
		public int __sceSasSetPause()
		{
			return Module.NotImplementedReturn;
		}


		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50a14dfc, "sceSasCore_50a14dfc" )]
		public int sceSasCore_50a14dfc()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xa232cbe6, "sceSasCore_a232cbe6" )]
		public int sceSasCore_a232cbe6()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xd5ebbbcd, "sceSasCore_d5ebbbcd" )]
		public int sceSasCore_d5ebbbcd()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xf983b186, "sceSasCore_f983b186" )]
		public int sceSasCore_f983b186()
		{
			return Module.NotImplementedReturn;
		}
	}
}

