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
		[BiosFunction( 0x3054D478, "sceKernelStdioRead" )]
		public int sceKernelStdioRead()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0CBB0571, "sceKernelStdioLseek" )]
		public int sceKernelStdioLseek()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA46785C9, "sceKernelStdioSendChar" )]
		public int sceKernelStdioSendChar()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3B931DB, "sceKernelStdioWrite" )]
		public int sceKernelStdioWrite()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D061C19, "sceKernelStdioClose" )]
		public int sceKernelStdioClose()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x924ABA61, "sceKernelStdioOpen" )]
		public int sceKernelStdioOpen()
		{
			return Module.NotImplementedReturn;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x172D316E, "sceKernelStdin" )]
		// SDK location: /user/pspstdio.h:35
		// SDK declaration: SceUID sceKernelStdin();
		public int sceKernelStdin()
		{
			return ( int )_kernel.StdIn.UID;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0xA6BAB2E9, "sceKernelStdout" )]
		// SDK location: /user/pspstdio.h:42
		// SDK declaration: SceUID sceKernelStdout();
		public int sceKernelStdout()
		{
			return ( int )_kernel.StdOut.UID;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0xF78BA90A, "sceKernelStderr" )]
		// SDK location: /user/pspstdio.h:49
		// SDK declaration: SceUID sceKernelStderr();
		public int sceKernelStderr()
		{
			return ( int )_kernel.StdErr.UID;
		}
	}
}

/* GenerateStubsV2: auto-generated - 7E4C9372 */
