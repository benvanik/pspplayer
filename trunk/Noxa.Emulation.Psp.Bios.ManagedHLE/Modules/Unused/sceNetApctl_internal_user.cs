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
	class sceNetApctl_internal_user : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetApctl_internal_user";
			}
		}

		#endregion

		#region State Management

		public sceNetApctl_internal_user( Kernel kernel )
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
		[BiosFunction( 0xE9B2E5E6, "sceNetApctlScanUser" )]
		int sceNetApctlScanUser(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6BDDCB8C, "sceNetApctl_internal_user_6BDDCB8C" )]
		int sceNetApctl_internal_user_6BDDCB8C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04776994, "sceNetApctl_internal_user_04776994" )]
		int sceNetApctl_internal_user_04776994(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F902CC33 */
