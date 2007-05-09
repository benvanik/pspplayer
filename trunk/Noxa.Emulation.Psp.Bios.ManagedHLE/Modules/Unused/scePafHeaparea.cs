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
	class scePafHeaparea : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePafHeaparea";
			}
		}

		#endregion

		#region State Management

		public scePafHeaparea( Kernel kernel )
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
		[BiosFunction( 0xF50AAE41, "scePafHeaparea_F50AAE41" )]
		int scePafHeaparea_F50AAE41(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACCE25B2, "scePafHeaparea_ACCE25B2" )]
		int scePafHeaparea_ACCE25B2(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 0310B154 */
