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
	class sceChkuppkg : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceChkuppkg";
			}
		}

		#endregion

		#region State Management

		public sceChkuppkg( Kernel kernel )
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
		[BiosFunction( 0xFDB62E25, "sceChkuppkg_FDB62E25" )]
		int sceChkuppkg_FDB62E25(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F8EC9C8, "sceChkuppkg_4F8EC9C8" )]
		int sceChkuppkg_4F8EC9C8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9BB329E, "sceChkuppkg_C9BB329E" )]
		int sceChkuppkg_C9BB329E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB81FD229, "sceChkuppkg_B81FD229" )]
		int sceChkuppkg_B81FD229(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEF9EB1E, "sceChkuppkg_FEF9EB1E" )]
		int sceChkuppkg_FEF9EB1E(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9662530F */
