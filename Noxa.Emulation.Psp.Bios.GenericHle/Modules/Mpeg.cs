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
	class Mpeg : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public Mpeg( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceMpeg";
			}
		}

		#endregion

		[BiosStub( 0x21ff80e4, "sceMpegQueryStreamOffset", true, 3 )]
		[BiosStubIncomplete]
		public int sceMpegQueryStreamOffset( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = ScePVoid pBuffer
			// a2 = SceInt32 *iOffset
			
			// int
			return 0;
		}

		[BiosStub( 0x611e9e11, "sceMpegQueryStreamSize", true, 2 )]
		[BiosStubIncomplete]
		public int sceMpegQueryStreamSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = ScePVoid pBuffer
			// a1 = SceInt32 *iSize
			
			// int
			return 0;
		}

		[BiosStub( 0x682a619b, "sceMpegInit", true, 0 )]
		[BiosStubIncomplete]
		public int sceMpegInit( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x874624d6, "sceMpegFinish", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegFinish( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc132e22f, "sceMpegQueryMemSize", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegQueryMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int iUnk
			
			// int
			return 0;
		}

		[BiosStub( 0xd8c5f121, "sceMpegCreate", true, 7 )]
		[BiosStubIncomplete]
		public int sceMpegCreate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = ScePVoid pData
			// a2 = SceInt32 iSize
			// a3 = SceMpegRingbuffer *Ringbuffer
			// sp[0] = SceInt32 iFrameWidth
			// sp[4] = SceInt32 iUnk1
			// sp[8] = SceInt32 iUnk2
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );
			int a6 = memory.ReadWord( sp + 8 );
			
			// SceInt32
			return 0;
		}

		[BiosStub( 0x606a4649, "sceMpegDelete", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegDelete( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			
			// SceVoid
			return 0;
		}

		[BiosStub( 0x42560f23, "sceMpegRegistStream", true, 3 )]
		[BiosStubIncomplete]
		public int sceMpegRegistStream( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceInt32 iStreamID
			// a2 = SceInt32 iUnk
			
			// SceMpegStream *
			return 0;
		}

		[BiosStub( 0x591a4aa2, "sceMpegUnRegistStream", false, 2 )]
		[BiosStubIncomplete]
		public int sceMpegUnRegistStream( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg Mpeg
			// a1 = SceMpegStream *pStream
			
			return 0;
		}

		[BiosStub( 0xa780cf7e, "sceMpegMallocAvcEsBuf", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegMallocAvcEsBuf( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			
			// ScePVoid
			return 0;
		}

		[BiosStub( 0xceb870b1, "sceMpegFreeAvcEsBuf", false, 2 )]
		[BiosStubIncomplete]
		public int sceMpegFreeAvcEsBuf( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = ScePVoid pBuf
			
			return 0;
		}

		[BiosStub( 0xf8dcb679, "sceMpegQueryAtracEsSize", true, 3 )]
		[BiosStubIncomplete]
		public int sceMpegQueryAtracEsSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceInt32 *iEsSize
			// a2 = SceInt32 *iOutSize
			
			// int
			return 0;
		}

		[BiosStub( 0xc02cf6b5, "sceMpegQueryPcmEsSize", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegQueryPcmEsSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x167afd9e, "sceMpegInitAu", true, 3 )]
		[BiosStubIncomplete]
		public int sceMpegInitAu( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = ScePVoid pEsBuffer
			// a2 = SceMpegAu *pAu
			
			// int
			return 0;
		}

		[BiosStub( 0x234586ae, "sceMpegChangeGetAvcAuMode", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegChangeGetAvcAuMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9dcfb7ea, "sceMpegChangeGetAuMode", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegChangeGetAuMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfe246728, "sceMpegGetAvcAu", true, 4 )]
		[BiosStubIncomplete]
		public int sceMpegGetAvcAu( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceMpegStream *pStream
			// a2 = SceMpegAu *pAu
			// a3 = SceInt32 *iUnk
			
			// int
			return 0;
		}

		[BiosStub( 0x8c1e027d, "sceMpegGetPcmAu", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegGetPcmAu( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe1ce83a7, "sceMpegGetAtracAu", true, 4 )]
		[BiosStubIncomplete]
		public int sceMpegGetAtracAu( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceMpegStream *pStream
			// a2 = SceMpegAu *pAu
			// a3 = ScePVoid pUnk
			
			// int
			return 0;
		}

		[BiosStub( 0x500f0429, "sceMpegFlushStream", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegFlushStream( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x707b7629, "sceMpegFlushAllStream", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegFlushAllStream( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			
			// int
			return 0;
		}

		[BiosStub( 0x0e3c2e9d, "sceMpegAvcDecode", true, 5 )]
		[BiosStubIncomplete]
		public int sceMpegAvcDecode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceMpegAu *pAu
			// a2 = SceInt32 iFrameWidth
			// a3 = ScePVoid pBuffer
			// sp[0] = SceInt32 *iInit
			int a4 = memory.ReadWord( sp + 0 );
			
			// int
			return 0;
		}

		[BiosStub( 0x0f6c18d7, "sceMpegAvcDecodeDetail", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegAvcDecodeDetail( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa11c7026, "sceMpegAvcDecodeMode", true, 2 )]
		[BiosStubIncomplete]
		public int sceMpegAvcDecodeMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceMpegAvcMode *pMode
			
			// int
			return 0;
		}

		[BiosStub( 0x740fccd1, "sceMpegAvcDecodeStop", true, 4 )]
		[BiosStubIncomplete]
		public int sceMpegAvcDecodeStop( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceInt32 iFrameWidth
			// a2 = ScePVoid pBuffer
			// a3 = SceInt32 *iStatus
			
			// int
			return 0;
		}

		[BiosStub( 0x4571cc64, "sceMpeg_0x4571CC64", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x800c44df, "sceMpegAtracDecode", true, 4 )]
		[BiosStubIncomplete]
		public int sceMpegAtracDecode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpeg *Mpeg
			// a1 = SceMpegAu *pAu
			// a2 = ScePVoid pBuffer
			// a3 = SceInt32 iInit
			
			// int
			return 0;
		}

		[BiosStub( 0x211a057c, "sceMpeg_0x211A057C", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd7a29f46, "sceMpegRingbufferQueryMemSize", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegRingbufferQueryMemSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceInt32 iPackets
			
			// int
			return 0;
		}

		[BiosStub( 0x37295ed8, "sceMpegRingbufferConstruct", true, 6 )]
		[BiosStubIncomplete]
		public int sceMpegRingbufferConstruct( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpegRingbuffer *Ringbuffer
			// a1 = SceInt32 iPackets
			// a2 = ScePVoid pData
			// a3 = SceInt32 iSize
			// sp[0] = sceMpegRingbufferCB Callback
			// sp[4] = ScePVoid pCBparam
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );
			
			// int
			return 0;
		}

		[BiosStub( 0x67179b1b, "sceMpeg_0x67179B1B", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x13407f13, "sceMpegRingbufferDestruct", false, 1 )]
		[BiosStubIncomplete]
		public int sceMpegRingbufferDestruct( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpegRingbuffer *Ringbuffer
			
			return 0;
		}

		[BiosStub( 0xf0eb1125, "sceMpeg_0xF0EB1125", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf2930c9c, "sceMpeg_0xF2930C9C", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb240a59e, "sceMpegRingbufferPut", true, 3 )]
		[BiosStubIncomplete]
		public int sceMpegRingbufferPut( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpegRingbuffer *Ringbuffer
			// a1 = SceInt32 iNumPackets
			// a2 = SceInt32 iAvailable
			
			// int
			return 0;
		}

		[BiosStub( 0xb5f6dc87, "sceMpegRingbufferAvailableSize", true, 1 )]
		[BiosStubIncomplete]
		public int sceMpegRingbufferAvailableSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceMpegRingbuffer *Ringbuffer
			
			// int
			return 0;
		}

		[BiosStub( 0x31bd0272, "sceMpeg_0x31BD0272", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x11cab459, "sceMpeg_0x11CAB459", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown7( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3c37a7a6, "sceMpeg_0x3C37A7A6", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown8( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb27711a8, "sceMpeg_0xB27711A8", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown9( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd4dd6e75, "sceMpeg_0xD4DD6E75", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown10( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc345ded2, "sceMpeg_0xC345DED2", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown11( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xcf3547a2, "sceMpegAvcDecodeDetail2", false, 0 )]
		[BiosStubIncomplete]
		public int sceMpegAvcDecodeDetail2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x988e9e12, "sceMpeg_0x988E9E12", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown12( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc2f02cdd, "sceMpeg_0xC2F02CDD", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown13( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdbb60658, "sceMpeg_0xDBB60658", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown14( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xab0e9556, "sceMpeg_0xAB0E9556", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown15( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x921fcccf, "sceMpeg_0x921FCCCF", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown16( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x11f95cf1, "sceMpeg_0x11F95CF1", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown17( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6f314410, "sceMpeg_0x6F314410", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown18( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
