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
	[BiosModuleAlias( "sceATRAC3plus_Library" )]
	class sceAtrac3plus : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAtrac3plus";
			}
		}

		#endregion

		#region State Management

		public sceAtrac3plus( Kernel kernel )
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

		enum AtracCodec
		{
			At3Plus			= 0x00001000,
			At3				= 0x00001001,
		};

		// NEEDED:
		// 0x5d268707 » sceAtracGetStreamDataInfo
		// 0x7db31251 » sceAtracAddStreamData
		// 0x83e85ea0 » sceAtracGetSecondBufferInfo
		// 0x83bf7afd » sceAtracSetSecondBuffer
		// 0xe23e3a35 » sceAtracGetNextDecodePosition
		// 0xa2bba8be » sceAtracGetSoundSample
		// 0xca3ca3d2 » sceAtracGetBufferInfoForReseting
		// 0x644e5607 » sceAtracResetPlayPosition
		// 0xe88f759b » sceAtracGetInternalErrorInfo
		// Not sure if any of these are actually used

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1F59FDB, "sceAtracStartEntry" )]
		public int sceAtracStartEntry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5C28CC0, "sceAtracEndEntry" )]
		public int sceAtracEndEntry(){ return Module.NotImplementedReturn; }


		//allocates an Atrac ID (decoding context?)
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x780F88D1, "sceAtracGetAtracID" )]
		public int sceAtracGetAtracID(int codecType){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61EB33F5, "sceAtracReleaseAtracID" )]
		// SDK location: /atrac3/pspatrac3.h:87
		// SDK declaration: int sceAtracReleaseAtracID(int atracID);
		public int sceAtracReleaseAtracID( int atracID ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F6E26B5, "sceAtracSetHalfwayBuffer" )]
		public int sceAtracSetHalfwayBuffer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[BiosFunction( 0x0E2A73AB, "sceAtracSetData" )] [Stateless]
		// manual add
		public int sceAtracSetData( int atracID, int buf, int bufsize ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A20E7AF, "sceAtracSetDataAndGetID" )]
		// SDK location: /atrac3/pspatrac3.h:27
		// SDK declaration: int sceAtracSetDataAndGetID(void *buf, SceSize bufsize);
		public int sceAtracSetDataAndGetID( int buf, int bufsize ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FAE370E, "sceAtracSetHalfwayBufferAndGetID" )]
		public int sceAtracSetHalfwayBufferAndGetID(){ return Module.NotImplementedReturn; }

		[DontTrace]
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A8C3CD5, "sceAtracDecodeData" )]
		// SDK location: /atrac3/pspatrac3.h:43
		// SDK declaration: int sceAtracDecodeData(int atracID, u16 *outSamples, int *outN, int *outEnd, int *outRemainFrame);
		public int sceAtracDecodeData( int atracID, int outSamples, int outN, int outEnd, int outRemainFrame ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9AE849A7, "sceAtracGetRemainFrame" )]
		// SDK location: /atrac3/pspatrac3.h:55
		// SDK declaration: int sceAtracGetRemainFrame(int atracID, int *outRemainFrame);
		public int sceAtracGetRemainFrame( int atracID, int outRemainFrame ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D268707, "sceAtracGetStreamDataInfo" )]
		public int sceAtracGetStreamDataInfo(int atracID){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DB31251, "sceAtracAddStreamData" )]
		public int sceAtracAddStreamData(int atracID, int numBytesAdded){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x83E85EA0, "sceAtracGetSecondBufferInfo" )]
		public int sceAtracGetSecondBufferInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x83BF7AFD, "sceAtracSetSecondBuffer" )]
		public int sceAtracSetSecondBuffer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE23E3A35, "sceAtracGetNextDecodePosition" )]
		public int sceAtracGetNextDecodePosition(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2BBA8BE, "sceAtracGetSoundSample" )]
		public int sceAtracGetSoundSample(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31668BAA, "sceAtracGetChannel" )]
		public int sceAtracGetChannel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6A5F2F7, "sceAtracGetMaxSample" )]
		// SDK location: /atrac3/pspatrac3.h:109
		// SDK declaration: int sceAtracGetMaxSample(int atracID, int *outMax);
		public int sceAtracGetMaxSample( int atracID, int outMax ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36FAABFB, "sceAtracGetNextSample" )]
		// SDK location: /atrac3/pspatrac3.h:98
		// SDK declaration: int sceAtracGetNextSample(int atracID, int *outN);
		public int sceAtracGetNextSample( int atracID, int outN ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA554A158, "sceAtracGetBitrate" )]
		// SDK location: /atrac3/pspatrac3.h:66
		// SDK declaration: int sceAtracGetBitrate(int atracID, int *outBitrate);
		public int sceAtracGetBitrate( int atracID, int outBitrate ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFAA4F89B, "sceAtracGetLoopStatus" )]
		public int sceAtracGetLoopStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x868120B5, "sceAtracSetLoopNum" )]
		// SDK location: /atrac3/pspatrac3.h:77
		// SDK declaration: int sceAtracSetLoopNum(int atracID, int nloops);
		public int sceAtracSetLoopNum( int atracID, int nloops ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA3CA3D2, "sceAtracGetBufferInfoForReseting" )]
		public int sceAtracGetBufferInfoForReseting(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x644E5607, "sceAtracResetPlayPosition" )]
		public int sceAtracResetPlayPosition(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE88F759B, "sceAtracGetInternalErrorInfo" )]
		public int sceAtracGetInternalErrorInfo(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 99D6BDB1 */
