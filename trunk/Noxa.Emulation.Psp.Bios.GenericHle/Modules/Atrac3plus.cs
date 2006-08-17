using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Atrac3plus : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public Atrac3plus( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceAtrac3plus";
			}
		}

		#endregion

		[BiosStub( 0xd1f59fdb, "sceAtracStartEntry", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracStartEntry( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd5c28cc0, "sceAtracEndEntry", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracEndEntry( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x780f88d1, "sceAtracGetAtracID", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetAtracID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x61eb33f5, "sceAtracReleaseAtracID", true, 1 )]
		[BiosStubIncomplete]
		public int sceAtracReleaseAtracID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID

			// int
			return 0;
		}

		[BiosStub( 0x0e2a73ab, "sceAtracSetData", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracSetData( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3f6e26b5, "sceAtracSetHalfwayBuffer", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracSetHalfwayBuffer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7a20e7af, "sceAtracSetDataAndGetID", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracSetDataAndGetID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void *buf
			// a1 = SceSize bufsize

			// int
			return 0;
		}

		[BiosStub( 0x0fae370e, "sceAtracSetHalfwayBufferAndGetID", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracSetHalfwayBufferAndGetID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6a8c3cd5, "sceAtracDecodeData", true, 5 )]
		[BiosStubIncomplete]
		public int sceAtracDecodeData( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = u16 *outSamples
			// a2 = int *outN
			// a3 = int *outEnd
			// sp[0] = int *outRemainFrame
			int a4 = memory.ReadWord( sp + 0 );

			// int
			return 0;
		}

		[BiosStub( 0x9ae849a7, "sceAtracGetRemainFrame", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracGetRemainFrame( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = int *outRemainFrame

			// int
			return 0;
		}

		[BiosStub( 0x5d268707, "sceAtracGetStreamDataInfo", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetStreamDataInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7db31251, "sceAtracAddStreamData", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracAddStreamData( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x83e85ea0, "sceAtracGetSecondBufferInfo", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetSecondBufferInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x83bf7afd, "sceAtracSetSecondBuffer", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracSetSecondBuffer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe23e3a35, "sceAtracGetNextDecodePosition", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetNextDecodePosition( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa2bba8be, "sceAtracGetSoundSample", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetSoundSample( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x31668baa, "sceAtracGetChannel", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetChannel( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd6a5f2f7, "sceAtracGetMaxSample", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracGetMaxSample( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = int *outMax

			// int
			return 0;
		}

		[BiosStub( 0x36faabfb, "sceAtracGetNextSample", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracGetNextSample( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = int *outN

			// int
			return 0;
		}

		[BiosStub( 0xa554a158, "sceAtracGetBitrate", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracGetBitrate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = int *outBitrate

			// int
			return 0;
		}

		[BiosStub( 0xfaa4f89b, "sceAtracGetLoopStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetLoopStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x868120b5, "sceAtracSetLoopNum", true, 2 )]
		[BiosStubIncomplete]
		public int sceAtracSetLoopNum( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int atracID
			// a1 = int nloops

			// int
			return 0;
		}

		[BiosStub( 0xca3ca3d2, "sceAtracGetBufferInfoForReseting", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetBufferInfoForReseting( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x644e5607, "sceAtracResetPlayPosition", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracResetPlayPosition( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe88f759b, "sceAtracGetInternalErrorInfo", false, 0 )]
		[BiosStubIncomplete]
		public int sceAtracGetInternalErrorInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
