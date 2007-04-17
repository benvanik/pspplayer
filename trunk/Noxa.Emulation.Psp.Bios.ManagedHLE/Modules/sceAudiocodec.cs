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
	class sceAudiocodec : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAudiocodec";
			}
		}

		#endregion

		#region State Management

		public sceAudiocodec( Kernel kernel )
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

		// Look here for implementation usage
		// http://perso.orange.fr/franck.charlet/AT3Replay.zip

		/*
		typedef tag_WFEXT_AT3PLUS { 
		u16   wFormatTag;      //+00   0xFFFE 
		u16   nChannels;      //+02   eg. 2 
		u32   nSamplesPerSec;      //+04   eg. 44100 
		u32   nAvgBytesPerSec;   //+08   eg. 16020 (~125 kbps) 
		u16   nBlockAlign      //+0C   eg. 0x02E8 
		u16   wBitsPerSample      //+0E   eg. 0 
		u16   cbSize;         //+10   0x0022 
		u16   wValidBitsPerSample;   //+12   0x0800 
		u32   dwChannelMask;      //+14   0x0003 (left | right) 
		GUID   SubFormat;      //+18   BF AA 23 E9 58 CB 71 44 A1 19 FF FA 01 E4 CE 62 
		u16   field_28;      //+28   eg. 0x0001 
		u8   field_2A[2];      //+2A   eg. 28 5C 
		u8   field_2C[8];      //+2C   eg. 0... 
		} WFEXT_AT3PLUS;         //=34 

		typedef tag_WFEXT_AT3 { 
		u16   wFormatTag;      //+00   0x0270 
		u16   nChannels;      //+02   eg. 2 
		u32   nSamplesPerSec;      //+04   eg. 44100 
		u32   nAvgBytesPerSec;   //+08   eg. 16537 (~129 kbps) 
		u16   nBlockAlign      //+0C   eg. 0x0180 
		u16   wBitsPerSample      //+0E   eg. 0 
		u16   cbSize;         //+10   0x000E 
		u16   _1;         //+12   eg. 0x0001 
		u32   _field_14;      //+14   eg. 0x00001000 
		u16   chmask;         //+18   eg. 0x0000 
		u16   chmask_EQ;      //+1A   eg. 0x0000 
		u16   _field_1C_1;      //+1C   eg. 0x0001 
		u16   field_1E;      //+1E   eg. 0x0000 
		} WFEXT_AT3;            //=20 

		0x1000 is atrac3plus 
		0x1001 is atrac3 
		0x1002 is mp3 
		0x1003 is aac 
		0x1004 might be wma in newer ME code (haven't checked)
		*/

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D3F790C, "sceAudiocodecCheckNeedMem" )]
		// manual add
		public int sceAudiocodecCheckNeedMem( int Buffer, int Type )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B37EB1D, "sceAudiocodecInit" )]
		// SDK location: /audio/pspaudiocodec.h:18
		// SDK declaration: int sceAudiocodecInit(unsigned long *Buffer, int Type);
		public int sceAudiocodecInit( int Buffer, int Type ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x70A703F8, "sceAudiocodecDecode" )]
		// SDK location: /audio/pspaudiocodec.h:19
		// SDK declaration: int sceAudiocodecDecode(unsigned long *Buffer, int Type);
		public int sceAudiocodecDecode( int Buffer, int Type ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A20A200, "sceAudiocodecGetEDRAM" )]
		// SDK location: /audio/pspaudiocodec.h:20
		// SDK declaration: int sceAudiocodecGetEDRAM(unsigned long *Buffer, int Type);
		public int sceAudiocodecGetEDRAM( int Buffer, int Type ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29681260, "sceAudiocodecReleaseEDRAM" )]
		// SDK location: /audio/pspaudiocodec.h:21
		// SDK declaration: int sceAudiocodecReleaseEDRAM(unsigned long *Buffer);
		public int sceAudiocodecReleaseEDRAM( int Buffer ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 8E961E73 */
