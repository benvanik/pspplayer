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
	class sceSsl : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSsl";
			}
		}

		#endregion

		#region State Management

		public sceSsl( Kernel kernel )
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
		[BiosFunction( 0x957ECBE2, "sceSslInit" )]
		public int sceSslInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x191CDEFF, "sceSslEnd" )]
		public int sceSslEnd(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 5A1EB240 */
