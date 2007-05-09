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
	class sceSemawm : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSemawm";
			}
		}

		#endregion

		#region State Management

		public sceSemawm( Kernel kernel )
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
		[BiosFunction( 0x77C42467, "sceSemawm_77C42467" )]
		int sceSemawm_77C42467(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x51E51869, "sceSemawm_51E51869" )]
		int sceSemawm_51E51869(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C268535, "sceSemawm_4C268535" )]
		int sceSemawm_4C268535(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B6C204D, "sceSemawm_5B6C204D" )]
		int sceSemawm_5B6C204D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FEA0A45, "sceSemawm_6FEA0A45" )]
		int sceSemawm_6FEA0A45(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - E2E12DF7 */
