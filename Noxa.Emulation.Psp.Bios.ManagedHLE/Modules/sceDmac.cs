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
	class sceDmac : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDmac";
			}
		}

		#endregion

		#region State Management

		public sceDmac( Kernel kernel )
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

		[Stateless]
		[BiosFunction( 0x617F3FE6, "sceDmacMemcpy" )]
		// manual add
		public int sceDmacMemcpy( int dest, int source, int size )
		{
			return Module.NotImplementedReturn;
		}

	}
}
