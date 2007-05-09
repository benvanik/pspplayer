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
	class sceResmgr : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceResmgr";
			}
		}

		#endregion

		#region State Management

		public sceResmgr( Kernel kernel )
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
		[BiosFunction( 0x9DC14891, "sceResmgr_9DC14891" )]
		int sceResmgr_9DC14891(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 638F9E5A */
