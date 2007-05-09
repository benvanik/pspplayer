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
	class sceCodec_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceCodec_driver";
			}
		}

		#endregion

		#region State Management

		public sceCodec_driver( Kernel kernel )
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
		[BiosFunction( 0xBD8E0977, "sceCodecInitEntry" )]
		int sceCodecInitEntry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02133959, "sceCodecStopEntry" )]
		int sceCodecStopEntry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x856E7487, "sceCodecOutputEnable" )]
		int sceCodecOutputEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x359C2B9F, "sceCodecOutputDisable" )]
		int sceCodecOutputDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC513C747, "sceCodecInputEnable" )]
		int sceCodecInputEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31B2E41E, "sceCodecInputDisable" )]
		int sceCodecInputDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x261C6EE8, "sceCodecSetOutputVolume" )]
		int sceCodecSetOutputVolume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D945509, "sceCodec_driver_6D945509" )]
		int sceCodec_driver_6D945509(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40D5C897, "sceCodec_driver_40D5C897" )]
		int sceCodec_driver_40D5C897(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFBCACF3, "sceCodecSetFrequency" )]
		int sceCodecSetFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56494D70, "sceCodec_driver_56494D70" )]
		int sceCodec_driver_56494D70(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4515AE04, "sceCodec_driver_4515AE04" )]
		int sceCodec_driver_4515AE04(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEB91526, "sceCodecSetVolumeOffset" )]
		int sceCodecSetVolumeOffset(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 0EEB132E */
