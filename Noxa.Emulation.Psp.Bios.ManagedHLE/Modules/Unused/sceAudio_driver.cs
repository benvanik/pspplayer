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
	class sceAudio_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAudio_driver";
			}
		}

		#endregion

		#region State Management

		public sceAudio_driver( Kernel kernel )
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
		[BiosFunction( 0x80F1F7E0, "sceAudioInit" )]
		int sceAudioInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x210567F7, "sceAudioEnd" )]
		int sceAudioEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2BEAA6C, "sceAudioSetFrequency" )]
		int sceAudioSetFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB61595C0, "sceAudioLoopbackTest" )]
		int sceAudioLoopbackTest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x927AC32B, "sceAudioSetVolumeOffset" )]
		int sceAudioSetVolumeOffset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C1009B2, "sceAudioOutput" )]
		int sceAudioOutput(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x136CAF51, "sceAudioOutputBlocking" )]
		int sceAudioOutputBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2D56B2D, "sceAudioOutputPanned" )]
		int sceAudioOutputPanned(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13F592BC, "sceAudioOutputPannedBlocking" )]
		int sceAudioOutputPannedBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EC81C55, "sceAudioChReserve" )]
		int sceAudioChReserve(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41EFADE7, "sceAudioOneshotOutput" )]
		int sceAudioOneshotOutput(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FC46853, "sceAudioChRelease" )]
		int sceAudioChRelease(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9D97901, "sceAudioGetChannelRestLen" )]
		int sceAudioGetChannelRestLen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB2E439E, "sceAudioSetChannelDataLen" )]
		int sceAudioSetChannelDataLen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95FD0C2D, "sceAudioChangeChannelConfig" )]
		int sceAudioChangeChannelConfig(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7E1D8E7, "sceAudioChangeChannelVolume" )]
		int sceAudioChangeChannelVolume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38553111, "sceAudio_driver_38553111" )]
		int sceAudio_driver_38553111(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C37C0AE, "sceAudio_driver_5C37C0AE" )]
		int sceAudio_driver_5C37C0AE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0727056, "sceAudio_driver_E0727056" )]
		int sceAudio_driver_E0727056(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x086E5895, "sceAudioInputBlocking" )]
		int sceAudioInputBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D4BEC68, "sceAudioInput" )]
		int sceAudioInput(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA708C6A6, "sceAudioGetInputLength" )]
		int sceAudioGetInputLength(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87B2E651, "sceAudioWaitInputEnd" )]
		int sceAudioWaitInputEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DE61688, "sceAudioInputInit" )]
		int sceAudioInputInit(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F8ACCC37 */
