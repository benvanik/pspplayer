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
	class StdioForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "StdioForUser";
			}
		}

		#endregion

		#region State Management

		public StdioForUser( Kernel kernel )
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
		[BiosFunction( 0x172D316E, "sceKernelStdin" )]
		// SDK location: /user/pspstdio.h:35
		// SDK declaration: SceUID sceKernelStdin();
		int sceKernelStdin(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA6BAB2E9, "sceKernelStdout" )]
		// SDK location: /user/pspstdio.h:42
		// SDK declaration: SceUID sceKernelStdout();
		int sceKernelStdout(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF78BA90A, "sceKernelStderr" )]
		// SDK location: /user/pspstdio.h:49
		// SDK declaration: SceUID sceKernelStderr();
		int sceKernelStderr(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 441D2B42 */
