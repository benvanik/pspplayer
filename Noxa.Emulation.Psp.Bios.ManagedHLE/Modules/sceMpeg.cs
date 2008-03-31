// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//#define PRETENDVALID

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	unsafe class sceMpeg : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMpeg";
			}
		}

		#endregion

		#region State Management

		public sceMpeg( Kernel kernel )
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

#if PRETENDVALID
		public const int DummyReturn = 0;
#else
		public const int DummyReturn = Module.NotImplementedReturn;
#endif

		#region sceMpeg

		// THIS IS NOT THE REAL STRUCTURE
		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		public struct SceMpeg
		{
			public sceMpegRingbuffer* Ringbuffer;
		}

		#endregion

		#region sceMpegRingbuffer

		[StructLayout( LayoutKind.Sequential, Pack = 1, Size = 44 )]
		public struct sceMpegRingbuffer
		{
			/// <summary>
			/// (0x00) Number of packets in ring buffer.
			/// </summary>
			public int iPackets;			// 00
			/// <summary>
			/// (0x04) Number of packets that have been read?
			/// </summary>
			public int iReadPackets;
			/// <summary>
			/// (0x08) Number of packets that have been written?
			/// </summary>
			public int iWritePackets;
			/// <summary>
			/// (0x0C) Unknown, set to 0.
			/// </summary>
			public int iUnk0;
			/// <summary>
			/// (0x10) Unknown, set to 2048.
			/// </summary>
			public int iPacketSize;
			/// <summary>
			/// (0x14) Pointer to ringbuffer data.
			/// </summary>
			public int pData;
			/// <summary>
			/// (0x18) Callback that should be called in sceMpegRingbufferPut().
			/// </summary>
			public int Callback;
			/// <summary>
			/// (0x1C) Data to be passed to the callback.
			/// </summary>
			public int pCBparam;
			/// <summary>
			/// (0x20) The upper bound of the ringbuffer data.
			/// </summary>
			public int pDataUpperBound;
			/// <summary>
			/// (0x24) Semaphore.
			/// </summary>
			public uint iSemaphoreID;
			/// <summary>
			/// (0x28) Pointer to associated SceMpeg?
			/// </summary>
			public int pSceMpeg;
		};

		#endregion

		#region psmfHeader

		[StructLayout( LayoutKind.Sequential, Pack = 1, Size = 12 )]
		public struct psmfHeader
		{
			public uint magic;
			public uint version;
			public uint offset;
			public uint size;
		};

		#endregion

		private uint Swap32( uint i )
		{
			return ( ( i & 0xFF ) << 24 ) | ( ( i & 0xFF00 ) << 8 ) | ( ( i & 0xFF0000 ) >> 8 ) | ( ( i >> 24 ) & 0xFF );
		}

		[Stateless]
		[BiosFunction( 0x21FF80E4, "sceMpegQueryStreamOffset" )]
		public int sceMpegQueryStreamOffset( int mpeg, int buffer, int offset )
		{
			Debug.Assert( offset != 0 );

			psmfHeader* header = ( psmfHeader* )_memorySystem.Translate( ( uint )buffer );

			if( header->magic != 0x464D5350 ) // PSMF
			{
				return unchecked( ( int )0x806101FE );
			}

			*( ( uint* )( _memorySystem.Translate( ( uint )offset ) ) ) = Swap32( header->offset );

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceMpegQueryStreamOffset returns {0:X8}", Swap32( header->offset ) );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x611E9E11, "sceMpegQueryStreamSize" )]
		public int sceMpegQueryStreamSize( int buffer, int size )
		{
			Debug.Assert( size != 0 );

			psmfHeader* header = ( psmfHeader* )_memorySystem.Translate( ( uint )buffer );

			if( ( Swap32( header->size ) & 0x07FF ) > 0 )
			{
				*( ( uint* )( _memorySystem.Translate( ( uint )size ) ) ) = 0;
				return unchecked( ( int )0x806101FE );
			}

			*( ( uint* )( _memorySystem.Translate( ( uint )size ) ) ) = Swap32( header->size );

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceMpegQueryStreamSize returns {0:X8}", Swap32( header->size ) );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x682A619B, "sceMpegInit" )]
		public int sceMpegInit()
		{
			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x874624D6, "sceMpegFinish" )]
		public void sceMpegFinish() { }

		[Stateless]
		[BiosFunction( 0xC132E22F, "sceMpegQueryMemSize" )]
		public int sceMpegQueryMemSize( int unk )
		{
			Debug.Assert( unk == 0 );
			return 512 * 4;
		}

		[Stateless]
		[BiosFunction( 0xD8C5F121, "sceMpegCreate" )]
		public int sceMpegCreate( int mpeg, int data, int size, int ringbuffer, int framewidth, int unk1, int unk2 )
		{
			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceMpegCreate({0:X8}, {1:X8}, {2}, {3:X8}, {4}, {5}, {6})", mpeg, data, size, ringbuffer, framewidth, unk1, unk2 );
			byte* pmpeg = _memorySystem.Translate( ( uint )mpeg );
			byte* pdata = _memorySystem.Translate( ( uint )data );
			// size = size returned from sceMpegQueryMemSize
			byte* prb = _memorySystem.Translate( ( uint )ringbuffer );

			SceMpeg* sceMpeg = ( SceMpeg* )pmpeg;
			sceMpeg->Ringbuffer = ( sceMpegRingbuffer* )prb;

			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x606A4649, "sceMpegDelete" )]
		public int sceMpegDelete( int mpeg ) { return DummyReturn; }

		[Stateless]
		[BiosFunction( 0x42560F23, "sceMpegRegistStream" )]
		public int sceMpegRegistStream( int mpeg, int streamid, int unk )
		{
			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceMpegRegistStream({0:X8}, {1}, {2})", mpeg, streamid, unk );
			return 0x12345678;
		}

		[Stateless]
		[BiosFunction( 0x591A4AA2, "sceMpegUnRegistStream" )]
		public void sceMpegUnRegistStream( int mpeg, int stream ) { }

		[Stateless]
		[BiosFunction( 0xA780CF7E, "sceMpegMallocAvcEsBuf" )]
		public int sceMpegMallocAvcEsBuf( int mpeg )
		{
			return unchecked( ( int )0xBEEFD00D );
		}

		[Stateless]
		[BiosFunction( 0xCEB870B1, "sceMpegFreeAvcEsBuf" )]
		public int sceMpegFreeAvcEsBuf( int mpeg, int buffer )
		{
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xF8DCB679, "sceMpegQueryAtracEsSize" )]
		public int sceMpegQueryAtracEsSize( int mpeg, int essize, int outsize )
		{
			Debug.Assert( essize != 0 );
			Debug.Assert( outsize != 0 );

			*( ( uint* )( _memorySystem.Translate( ( uint )essize ) ) ) = 2112;
			*( ( uint* )( _memorySystem.Translate( ( uint )outsize ) ) ) = 8192;

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC02CF6B5, "sceMpegQueryPcmEsSize" )]
		public int sceMpegQueryPcmEsSize()
		{
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x167AFD9E, "sceMpegInitAu" )]
		public int sceMpegInitAu( int mpeg, int esbuffer, int au )
		{
			byte* p = _memorySystem.Translate( ( uint )au );
			for( int i = 0; i < 24; i++ )
			{
				*( ( byte* )( p + i ) ) = 0xCC;
			}

			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x234586AE, "sceMpegChangeGetAvcAuMode" )]
		public int sceMpegChangeGetAvcAuMode() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x9DCFB7EA, "sceMpegChangeGetAuMode" )]
		public int sceMpegChangeGetAuMode() { return DummyReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE246728, "sceMpegGetAvcAu" )]
		public int sceMpegGetAvcAu( int mpeg, int stream, int au, int unk )
		{
			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x8C1E027D, "sceMpegGetPcmAu" )]
		public int sceMpegGetPcmAu() { return DummyReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1CE83A7, "sceMpegGetAtracAu" )]
		public int sceMpegGetAtracAu( int mpeg, int stream, int au, int unk )
		{
			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x500F0429, "sceMpegFlushStream" )]
		public int sceMpegFlushStream() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x707B7629, "sceMpegFlushAllStream" )]
		public int sceMpegFlushAllStream() { return DummyReturn; }

		[Stateless]
		[BiosFunction( 0x0E3C2E9D, "sceMpegAvcDecode" )]
		public int sceMpegAvcDecode( int mpeg, int au, int framewidth, int buffer, int num )
		{
			Debug.Assert( num != 0 );

			byte* pmpeg = _memorySystem.Translate( ( uint )mpeg );
			SceMpeg* sceMpeg = ( SceMpeg* )pmpeg;
			byte* ptr = RingBuffer.Read(
				_memorySystem.Translate( ( uint )sceMpeg->Ringbuffer->pData ),
				sceMpeg->Ringbuffer->iPackets, sceMpeg->Ringbuffer->iPacketSize,
				ref sceMpeg->Ringbuffer->iReadPackets );

			uint pbuffer = *( ( uint* )_memorySystem.Translate( ( uint )buffer ) );
			uint* ppbuffer = ( uint* )_memorySystem.Translate( pbuffer );
			for( int n = 0; n < framewidth; n++ )
			{
				*( ppbuffer++ ) = ( uint )( 0xCCDDEEFF + n );
			}

			// 1 = decoded something, else 0
			if( num != 0 )
			{
				uint* pnum = ( uint* )_memorySystem.Translate( ( uint )num );
				*pnum = 1;
			}

			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x0F6C18D7, "sceMpegAvcDecodeDetail" )]
		public int sceMpegAvcDecodeDetail() { return DummyReturn; }

		[Stateless]
		[BiosFunction( 0xA11C7026, "sceMpegAvcDecodeMode" )]
		public int sceMpegAvcDecodeMode( int mpeg, int mode )
		{
			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceMpegAvcDecodeMode({0:X8}, {1:X8})", mpeg, mode );
			return 0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x740FCCD1, "sceMpegAvcDecodeStop" )]
		public int sceMpegAvcDecodeStop() { return DummyReturn; }

		[Stateless]
		[BiosFunction( 0x800C44DF, "sceMpegAtracDecode" )]
		public int sceMpegAtracDecode( int mpeg, int au, int buffer, int init )
		{
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xD7A29F46, "sceMpegRingbufferQueryMemSize" )]
		public int sceMpegRingbufferQueryMemSize( int packets )
		{
			return ( packets * 104 ) + ( packets * 2048 );
		}

		[Stateless]
		[BiosFunction( 0x37295ED8, "sceMpegRingbufferConstruct" )]
		public int sceMpegRingbufferConstruct( uint ringbuffer, int packets, int allocated, int size, int callback, int cbparam )
		{
			sceMpegRingbuffer* rb = ( sceMpegRingbuffer* )_memorySystem.Translate( ringbuffer );

			rb->iPackets = packets;
			rb->iReadPackets = 0;
			rb->iWritePackets = 0;
			rb->iUnk0 = packets; // using this as remaining for now
			rb->iPacketSize = 2048;
			rb->pData = allocated;
			rb->Callback = callback;
			rb->pCBparam = cbparam;
			rb->pDataUpperBound = allocated + ( packets * 104 ) + ( packets * 2048 );
			rb->iSemaphoreID = 123456;
			rb->pSceMpeg = 0;

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x13407F13, "sceMpegRingbufferDestruct" )]
		public int sceMpegRingbufferDestruct( int ringbuffer )
		{
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xB240A59E, "sceMpegRingbufferPut" )]
		public int sceMpegRingbufferPut( int ringbuffer, int packets, int available )
		{
			sceMpegRingbuffer* rb = ( sceMpegRingbuffer* )_memorySystem.Translate( ( uint )ringbuffer );

			int toWrite = Math.Min( rb->iUnk0, packets );
			if( toWrite == 0 )
			{
				// Full
				return 0;
			}

			// Write position
			byte* ptr = RingBuffer.BeginWrite( ( byte* )rb->pData, rb->iPackets, rb->iPacketSize, rb->iWritePackets );

			// int callback( ringbuffer, packets, cbparam )
			_kernel.Cpu.MarshalCall( _kernel.ActiveThread.ContextID, ( uint )rb->Callback, new uint[] { ( uint )ptr, ( uint )toWrite, ( uint )rb->pCBparam }, RingbufferPutComplete, ringbuffer );

			return toWrite;
		}

		private bool RingbufferPutComplete( int tcsId, int state, int result )
		{
			sceMpegRingbuffer* rb = ( sceMpegRingbuffer* )_memorySystem.Translate( ( uint )state );

			if( result >= 0 )
			{
				// Ok
				RingBuffer.EndWrite( ( byte* )rb->pData, rb->iPackets, rb->iPacketSize, ref rb->iWritePackets, result );
				rb->iUnk0 -= result;
			}
			else
			{
				// Failed
			}
			_kernel.Cpu.SetContextRegister( _kernel.ActiveThread.ContextID, 2, ( uint )result );

			return true;
		}

		[Stateless]
		[BiosFunction( 0xB5F6DC87, "sceMpegRingbufferAvailableSize" )]
		public int sceMpegRingbufferAvailableSize( int ringbuffer )
		{
			sceMpegRingbuffer* rb = ( sceMpegRingbuffer* )_memorySystem.Translate( ( uint )ringbuffer );
			return rb->iUnk0;
		}

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x11CAB459, "sceMpeg_11CAB459" )]
		public int sceMpeg_11CAB459() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x3C37A7A6, "sceMpeg_3C37A7A6" )]
		public int sceMpeg_3C37A7A6() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xB27711A8, "sceMpeg_B27711A8" )]
		public int sceMpeg_B27711A8() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xD4DD6E75, "sceMpeg_D4DD6E75" )]
		public int sceMpeg_D4DD6E75() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xC345DED2, "sceMpeg_C345DED2" )]
		public int sceMpeg_C345DED2() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0xCF3547A2, "sceMpegAvcDecodeDetail2" )]
		public int sceMpegAvcDecodeDetail2() { return DummyReturn; }

#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x4571CC64, "sceMpegAvcDecodeFlush" )]
		public int sceMpegAvcDecodeFlush() { return DummyReturn; }


#if !PRETENDVALID
		[NotImplemented]
#endif
		[Stateless]
		[BiosFunction( 0x988E9E12, "sceMpeg_988E9E12" )]
		public int sceMpeg_988E9E12() { return DummyReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x211A057C, "sceMpegAvcQueryYCbCrSize" )]
		public int sceMpegAvcQueryYCbCrSize() { return 0; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67179b1b, "sceMpegAvcInitYCbCr" )]
		public int sceMpegAvcInitYCbCr() { return 0; }

		[Stateless]
		[BiosFunction( 0xF0EB1125, "sceMpegAvcDecodeYCbCr" )]
		public int sceMpegAvcDecodeYCbCr() { return 0; }

		[Stateless]
		[BiosFunction( 0x31BD0272, "sceMpegAvcCsc" )]
		public int sceMpegAvcCsc() { return 0; }
	}
}

/* GenerateStubsV2: auto-generated - A5BF8D85 */
