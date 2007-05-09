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
	class sceMeAudio_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMeAudio_driver";
			}
		}

		#endregion

		#region State Management

		public sceMeAudio_driver( Kernel kernel )
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
		[BiosFunction( 0xC00D873B, "sceMeAudio_driver_C00D873B" )]
		int sceMeAudio_driver_C00D873B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEE0A97D, "sceMeAudio_driver_FEE0A97D" )]
		int sceMeAudio_driver_FEE0A97D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAC7C2BE, "sceMeAudio_driver_BAC7C2BE" )]
		int sceMeAudio_driver_BAC7C2BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39F06790, "sceMeAudio_driver_39F06790" )]
		int sceMeAudio_driver_39F06790(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 31B1BBC2 */
