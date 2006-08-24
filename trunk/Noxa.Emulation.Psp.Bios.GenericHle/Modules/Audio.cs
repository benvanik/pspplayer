// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Audio : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public Audio( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceAudio";
			}
		}

		#endregion

		[BiosStub( 0x8c1009b2, "sceAudioOutput", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioOutput( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x136caf51, "sceAudioOutputBlocking", true, 3 )]
		[BiosStubIncomplete]
		public int sceAudioOutputBlocking( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int vol
			// a2 = void *buf
			
			// int
			return 0;
		}

		[BiosStub( 0xe2d56b2d, "sceAudioOutputPanned", true, 4 )]
		[BiosStubIncomplete]
		public int sceAudioOutputPanned( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int leftvol
			// a2 = int rightvol
			// a3 = void *buffer
			
			// int
			return 0;
		}

		[BiosStub( 0x13f592bc, "sceAudioOutputPannedBlocking", true, 4 )]
		[BiosStubIncomplete]
		public int sceAudioOutputPannedBlocking( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int leftvol
			// a2 = int rightvol
			// a3 = void *buffer
			
			// int
			return 0;
		}

		[BiosStub( 0x5ec81c55, "sceAudioChReserve", true, 3 )]
		[BiosStubIncomplete]
		public int sceAudioChReserve( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int samplecount
			// a2 = int format
			
			// int
			return 0;
		}

		[BiosStub( 0x41efade7, "sceAudioOneshotOutput", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioOneshotOutput( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6fc46853, "sceAudioChRelease", true, 1 )]
		[BiosStubIncomplete]
		public int sceAudioChRelease( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			
			// int
			return 0;
		}

		[BiosStub( 0xe9d97901, "sceAudioGetChannelRestLen", true, 1 )]
		[BiosStubIncomplete]
		public int sceAudioGetChannelRestLen( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			
			// int
			return 0;
		}

		[BiosStub( 0xb011922f, "Audio_0xB011922F", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xcb2e439e, "sceAudioSetChannelDataLen", true, 2 )]
		[BiosStubIncomplete]
		public int sceAudioSetChannelDataLen( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int samplecount
			
			// int
			return 0;
		}

		[BiosStub( 0x95fd0c2d, "sceAudioChangeChannelConfig", true, 2 )]
		[BiosStubIncomplete]
		public int sceAudioChangeChannelConfig( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int format
			
			// int
			return 0;
		}

		[BiosStub( 0xb7e1d8e7, "sceAudioChangeChannelVolume", true, 3 )]
		[BiosStubIncomplete]
		public int sceAudioChangeChannelVolume( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int channel
			// a1 = int leftvol
			// a2 = int rightvol
			
			// int
			return 0;
		}

		[BiosStub( 0x38553111, "Audio_0x38553111", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5c37c0ae, "Audio_0x5C37C0AE", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe0727056, "Audio_0xE0727056", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x086e5895, "sceAudioInputBlocking", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioInputBlocking( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6d4bec68, "sceAudioInput", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioInput( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa708c6a6, "sceAudioGetInputLength", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioGetInputLength( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x87b2e651, "sceAudioWaitInputEnd", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioWaitInputEnd( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7de61688, "sceAudioInputInit", false, 0 )]
		[BiosStubIncomplete]
		public int sceAudioInputInit( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe926d3fb, "Audio_0xE926D3FB", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa633048e, "Audio_0xA633048E", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
