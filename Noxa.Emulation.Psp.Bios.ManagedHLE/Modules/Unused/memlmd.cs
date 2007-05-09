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
	class memlmd : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "memlmd";
			}
		}

		#endregion

		#region State Management

		public memlmd( Kernel kernel )
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
		[BiosFunction( 0x8BDB1A3E, "sceUtilsGetLoadModuleABLength" )]
		int sceUtilsGetLoadModuleABLength(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x185F0A2A, "memlmd_185F0A2A" )]
		int memlmd_185F0A2A(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - A4F52D68 */
