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
	class sceIplUpdate : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceIplUpdate";
			}
		}

		#endregion

		#region State Management

		public sceIplUpdate( Kernel kernel )
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
		[BiosFunction( 0xEE7EB563, "sceIplUpdateSetIpl" )]
		int sceIplUpdateSetIpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26093B04, "sceIplUpdateClearIpl" )]
		int sceIplUpdateClearIpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x446B75E3, "sceIplUpdateUpdateIpl" )]
		int sceIplUpdateUpdateIpl(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 1A6EB692 */
