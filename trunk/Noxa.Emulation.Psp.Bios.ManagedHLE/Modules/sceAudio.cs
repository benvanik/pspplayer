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
	class sceAudio : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAudio";
			}
		}

		#endregion

		#region State Management

		public sceAudio( Kernel kernel )
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

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C1009B2, "sceAudioOutput" )]
		// SDK location: /audio/pspaudio.h:73
		// SDK declaration: int sceAudioOutput(int channel, int vol, void *buf);
		public int sceAudioOutput( int channel, int vol, int buf ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x136CAF51, "sceAudioOutputBlocking" )]
		// SDK location: /audio/pspaudio.h:79
		// SDK declaration: int sceAudioOutputBlocking(int channel, int vol, void *buf);
		public int sceAudioOutputBlocking( int channel, int vol, int buf ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2D56B2D, "sceAudioOutputPanned" )]
		// SDK location: /audio/pspaudio.h:85
		// SDK declaration: int sceAudioOutputPanned(int channel, int leftvol, int rightvol, void *buffer);
		public int sceAudioOutputPanned( int channel, int leftvol, int rightvol, int buffer ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13F592BC, "sceAudioOutputPannedBlocking" )]
		// SDK location: /audio/pspaudio.h:91
		// SDK declaration: int sceAudioOutputPannedBlocking(int channel, int leftvol, int rightvol, void *buffer);
		public int sceAudioOutputPannedBlocking( int channel, int leftvol, int rightvol, int buffer ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EC81C55, "sceAudioChReserve" )]
		// SDK location: /audio/pspaudio.h:61
		// SDK declaration: int sceAudioChReserve(int channel, int samplecount, int format);
		public int sceAudioChReserve( int channel, int samplecount, int format ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FC46853, "sceAudioChRelease" )]
		// SDK location: /audio/pspaudio.h:70
		// SDK declaration: int sceAudioChRelease(int channel);
		public int sceAudioChRelease( int channel ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9D97901, "sceAudioGetChannelRestLen" )]
		// SDK location: /audio/pspaudio.h:97
		// SDK declaration: int sceAudioGetChannelRestLen(int channel);
		public int sceAudioGetChannelRestLen( int channel ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB2E439E, "sceAudioSetChannelDataLen" )]
		// SDK location: /audio/pspaudio.h:103
		// SDK declaration: int sceAudioSetChannelDataLen(int channel, int samplecount);
		public int sceAudioSetChannelDataLen( int channel, int samplecount ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95FD0C2D, "sceAudioChangeChannelConfig" )]
		// SDK location: /audio/pspaudio.h:109
		// SDK declaration: int sceAudioChangeChannelConfig(int channel, int format);
		public int sceAudioChangeChannelConfig( int channel, int format ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7E1D8E7, "sceAudioChangeChannelVolume" )]
		// SDK location: /audio/pspaudio.h:115
		// SDK declaration: int sceAudioChangeChannelVolume(int channel, int leftvol, int rightvol);
		public int sceAudioChangeChannelVolume( int channel, int leftvol, int rightvol ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 7837FF0F */
