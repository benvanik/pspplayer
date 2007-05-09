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
	class sceAudiocodec_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAudiocodec_driver";
			}
		}

		#endregion

		#region State Management

		public sceAudiocodec_driver( Kernel kernel )
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
		[BiosFunction( 0x62518B7E, "sceAudiocodecStartEntry" )]
		int sceAudiocodecStartEntry(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 6F9F90FA */
