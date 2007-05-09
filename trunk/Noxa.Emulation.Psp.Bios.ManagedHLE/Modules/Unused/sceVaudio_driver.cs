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
	class sceVaudio_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVaudio_driver";
			}
		}

		#endregion

		#region State Management

		public sceVaudio_driver( Kernel kernel )
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
		[BiosFunction( 0x5A7435B7, "sceVaudioInit" )]
		int sceVaudioInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72749E84, "sceVaudioEnd" )]
		int sceVaudioEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8986295E, "sceVaudioOutputBlocking" )]
		int sceVaudioOutputBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03B6807D, "sceVaudioChReserve" )]
		int sceVaudioChReserve(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67585DFD, "sceVaudioChRelease" )]
		int sceVaudioChRelease(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x346FBE94, "sceVaudio_driver_346FBE94" )]
		int sceVaudio_driver_346FBE94(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82EF2F9D, "sceVaudio_driver_82EF2F9D" )]
		int sceVaudio_driver_82EF2F9D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDCC18F25, "sceVaudio_driver_DCC18F25" )]
		int sceVaudio_driver_DCC18F25(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - ED846A13 */
