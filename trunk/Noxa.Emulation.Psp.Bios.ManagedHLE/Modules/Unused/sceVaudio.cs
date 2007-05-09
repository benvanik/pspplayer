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
	class sceVaudio : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVaudio";
			}
		}

		#endregion

		#region State Management

		public sceVaudio( Kernel kernel )
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
		[BiosFunction( 0x8986295E, "sceVaudioOutputBlocking" )]
		public int sceVaudioOutputBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03B6807D, "sceVaudioChReserve" )]
		public int sceVaudioChReserve(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67585DFD, "sceVaudioChRelease" )]
		public int sceVaudioChRelease(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x346FBE94, "sceVaudio_346FBE94" )]
		public int sceVaudio_346FBE94(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - A744FCEC */
