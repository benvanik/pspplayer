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
	class scePEQ : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePEQ";
			}
		}

		#endregion

		#region State Management

		public scePEQ( Kernel kernel )
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
		[BiosFunction( 0xFC45514B, "scePEQ_FC45514B" )]
		int scePEQ_FC45514B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7EA0632, "scePEQ_F7EA0632" )]
		int scePEQ_F7EA0632(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED13C3B5, "scePEQ_ED13C3B5" )]
		int scePEQ_ED13C3B5(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 05AFA7F9 */
