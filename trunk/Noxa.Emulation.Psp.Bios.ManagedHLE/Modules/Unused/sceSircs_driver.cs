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
	class sceSircs_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSircs_driver";
			}
		}

		#endregion

		#region State Management

		public sceSircs_driver( Kernel kernel )
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
		[BiosFunction( 0x62411801, "sceSircsInit" )]
		int sceSircsInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x19155A2F, "sceSircsEnd" )]
		int sceSircsEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71EEF62D, "sceSircsSend" )]
		int sceSircsSend(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 1965F40D */
