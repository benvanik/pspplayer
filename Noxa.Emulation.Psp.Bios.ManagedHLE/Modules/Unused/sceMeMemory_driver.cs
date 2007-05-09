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
	class sceMeMemory_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMeMemory_driver";
			}
		}

		#endregion

		#region State Management

		public sceMeMemory_driver( Kernel kernel )
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
		[BiosFunction( 0xA177798D, "sceMeMalloc" )]
		int sceMeMalloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC86500E3, "sceMeCalloc" )]
		int sceMeCalloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AD660FA, "sceMeFree" )]
		int sceMeFree(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - FE63DE99 */
