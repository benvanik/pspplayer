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
	class sceMSAudio_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMSAudio_driver";
			}
		}

		#endregion

		#region State Management

		public sceMSAudio_driver( Kernel kernel )
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
		[BiosFunction( 0x757F4FD3, "sceMSAudioInit" )]
		int sceMSAudioInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3872FBE5, "sceMSAudioEnd" )]
		int sceMSAudioEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x107D83D4, "sceMSAudioAuth" )]
		int sceMSAudioAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8B25D38, "sceMSAudio_driver_E8B25D38" )]
		int sceMSAudio_driver_E8B25D38(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47F7DAFC, "sceMSAudioCheckICV" )]
		int sceMSAudioCheckICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B953C1A, "sceMSAudio_driver_7B953C1A" )]
		int sceMSAudio_driver_7B953C1A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA18A1DF6, "sceMSAudio_driver_A18A1DF6" )]
		int sceMSAudio_driver_A18A1DF6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80BD1592, "sceMSAudioDeauth" )]
		int sceMSAudioDeauth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D1C9867, "sceMSAudio_driver_5D1C9867" )]
		int sceMSAudio_driver_5D1C9867(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38178F2F, "sceMSAudio_driver_38178F2F" )]
		int sceMSAudio_driver_38178F2F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67E58C07, "sceMSAudio_driver_67E58C07" )]
		int sceMSAudio_driver_67E58C07(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x135F2225, "sceMSAudio_driver_135F2225" )]
		int sceMSAudio_driver_135F2225(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x22DA9981, "sceMSAudio_driver_22DA9981" )]
		int sceMSAudio_driver_22DA9981(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E37E51D, "sceMSAudio_driver_9E37E51D" )]
		int sceMSAudio_driver_9E37E51D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86666DA6, "sceMSAudioInvalidateICV" )]
		int sceMSAudioInvalidateICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE7BC654, "sceMSAudio_driver_FE7BC654" )]
		int sceMSAudio_driver_FE7BC654(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66F19CA3, "sceMSAudio_driver_66F19CA3" )]
		int sceMSAudio_driver_66F19CA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x383DEC75, "sceMSAudioGetInitialEKB" )]
		int sceMSAudioGetInitialEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1B87E96, "sceMSAudioFormatICV" )]
		int sceMSAudioFormatICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x60577DBD, "sceMSAudioGetICVInfo" )]
		int sceMSAudioGetICVInfo(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 74C96A60 */
