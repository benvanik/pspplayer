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
	class sceVshBridge_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVshBridge_driver";
			}
		}

		#endregion

		#region State Management

		public sceVshBridge_driver( Kernel kernel )
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
		[BiosFunction( 0x39B14120, "_vshVshBridgeStart" )]
		int _vshVshBridgeStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92A3B940, "_vshVshBridgeStop" )]
		int _vshVshBridgeStop(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 6CA3922F */
