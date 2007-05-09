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
	class sceLed_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLed_driver";
			}
		}

		#endregion

		#region State Management

		public sceLed_driver( Kernel kernel )
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
		[BiosFunction( 0xB0B6A883, "sceLedInit" )]
		int sceLedInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8542C48, "sceLedEnd" )]
		int sceLedEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE91D3A4, "sceLedSuspend" )]
		int sceLedSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA13B3D38, "sceLedResume" )]
		int sceLedResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA24BE03, "sceLedSetMode" )]
		int sceLedSetMode(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F7264464 */
