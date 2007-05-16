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
	// sceSas* is actually a static wrapper library that calles the real __sceSas*
	// functions. __sceSas* have an additional parameter in front of most functions, the sasCore
	// which is normally hidden from sceSas users
	// but since __sceSas is what's imported, we have to deal with it

	// There are more layers on top of sceSas but we don't need to care
	// if we emulate sceSas properly.

	unsafe class sceSas : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSas";
			}
		}

		#endregion

		#region State Management

		public sceSas(Kernel kernel)
			: base(kernel)
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion
		

		//Don't know the difference between these two :P
		[Stateless]
		[BiosFunction(0xa3589d81, "__sceSasCore")]
		public int __sceSasCore() 
		{
			return 0;
		}

		[Stateless]
		[BiosFunction(0x42778a9f, "__sceSasInit")]
		public int __sceSasInit() 
		{
			return 0;
		}

		[Stateless]
		[BiosFunction(0x68a46b95, "__sceSasGetEndFlag")]
		public int __sceSasGetEndFlag() 
		{
			//Quick hack for now, helps puyo puyo in Potemkin
			return -1; //0xFFFFFFFF, all voices done
		}

		[Stateless]
		[BiosFunction(0x440ca7d8, "__sceSasSetVolume")]
		public int __sceSasSetVolume(int sasCore, int num, int l, int el, int r, int er) 
		{
			return 0;
		}

		[Stateless]
		[BiosFunction(0xad84d37f, "__sceSasSetPitch")]
		public int __sceSasSetPitch(int sasCore, int num, int pitch)
		{
			return 0;
		}

		[Stateless]
		[BiosFunction(0x99944089, "__sceSasSetVoice")]
		public int __sceSasSetVoice(int sasCore, int num, int vagPtr, int size, int loopmode) 
		{
			//Real VAG header is 0x30 bytes behind the vagAddr
			/*
			//VAG-Depack, hacked by bITmASTER@bigfoot.com
			//V0.1
			double f[5][2] = { { 0.0, 0.0 },
			{   60.0 / 64.0,  0.0 },
			{  115.0 / 64.0, -52.0 / 64.0 },
			{   98.0 / 64.0, -55.0 / 64.0 },
			{  122.0 / 64.0, -60.0 / 64.0 } };

			double samples[28];
			int predict_nr, shift_factor, flags;
			int i;
			int d, s;
				
			double s_1 = 0.0;
			double s_2 = 0.0;

			while( 1 ) 
			{
				predict_nr = fgetc( vag );
				shift_factor = predict_nr & 0xf;
				predict_nr >>= 4;
				flags = fgetc( vag );                           // flags
				if ( flags == 7 )
					break;              
				for ( i = 0; i < 28; i += 2 ) 
				{
					d = fgetc( vag );
					s = ( d & 0xf ) << 12;
					if ( s & 0x8000 )
						s |= 0xffff0000;
					samples[i] = (double) ( s >> shift_factor  );
					s = ( d & 0xf0 ) << 8;
					if ( s & 0x8000 )
						s |= 0xffff0000;
					samples[i+1] = (double) ( s >> shift_factor  );
				}
				for ( i = 0; i < 28; i++ ) {
					samples[i] = samples[i] + s_1 * f[predict_nr][0] + s_2 * f[predict_nr][1];
					s_2 = s_1;
					s_1 = samples[i];
					d = (int) ( samples[i] + 0.5 );
					fputc( d & 0xff, pcm );
					fputc( d >> 8, pcm );
				}
			}
			*/
			return 0; //success
		}

		[Stateless]
		[BiosFunction(0x019b25eb, "__sceSasSetADSR")]
		public int __sceSasSetADSR(int sasCore, int num, int flag, int a, int d, int s, int r)
		{
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xb7660a23, "__sceSasSetNoise")]
		public int __sceSasSetNoise() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x9ec3676a, "__sceSasSetADSRmode")]
		public int __sceSasSetADSRmode() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x5f9529f6, "__sceSasSetSL")]
		public int __sceSasSetSL() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x74ae582a, "__sceSasGetEnvelopeHeight")]
		public int __sceSasGetEnvelopeHeight() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xcbcd4f79, "__sceSasSetSimpleADSR")]
		public int __sceSasSetSimpleADSR() { return Module.NotImplementedReturn; }


		[NotImplemented]
		[Stateless]
		[BiosFunction(0xa0cf2fa4, "__sceSasSetKeyOff")]
		public int __sceSasSetKeyOff() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x76f01aca, "__sceSasSetKeyOn")]
		public int __sceSasSetKeyOn() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xf983b186, "sceSasCore_f983b186")]
		public int sceSasCore_f983b186() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xd5a229c9, "__sceSasRevEVOL")]
		public int __sceSasRevEVOL() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x33d4ab37, "__sceSasRevType")]
		public int __sceSasRevType() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x267a6dd2, "__sceSasRevParam")]
		public int __sceSasRevParam() { return Module.NotImplementedReturn; }
	
		[NotImplemented]
		[Stateless]
		[BiosFunction(0x2c8e6ab3, "__sceSasGetPauseFlag")]
		public int __sceSasGetPauseFlag() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x787d04d5, "__sceSasSetPause")]
		public int __sceSasSetPause() { return Module.NotImplementedReturn; }


		//These are probably some of __sceSasExit, __sceSasSetSimpleADSR, 
		//  __sceSasSetEffect[,Param,Type,Volume]
		//too lazy to hash and check

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x50a14dfc, "sceSasCore_50a14dfc")]
		public int sceSasCore_50a14dfc() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xa232cbe6, "sceSasCore_a232cbe6")]
		public int sceSasCore_a232cbe6() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xd5ebbbcd, "sceSasCore_d5ebbbcd")]
		public int sceSasCore_d5ebbbcd() { return Module.NotImplementedReturn; }
	}
}

