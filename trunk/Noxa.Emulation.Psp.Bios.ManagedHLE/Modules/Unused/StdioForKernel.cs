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
	class StdioForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "StdioForKernel";
			}
		}

		#endregion

		#region State Management

		public StdioForKernel( Kernel kernel )
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
		[BiosFunction( 0x2CCF071A, "fdprintf" )]
		int fdprintf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAB439DF, "printf" )]
		int printf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F78930A, "fdputc" )]
		int fdputc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD768752A, "putchar" )]
		int putchar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36B23B8B, "fdputs" )]
		int fdputs(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD97C8CB9, "puts" )]
		int puts(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2B2A2A7, "fdgetc" )]
		int fdgetc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E338487, "getchar" )]
		int getchar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x11A5127A, "fdgets" )]
		int fdgets(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFF7E760, "gets" )]
		int gets(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x172D316E, "sceKernelStdin" )]
		int sceKernelStdin(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA6BAB2E9, "sceKernelStdout" )]
		int sceKernelStdout(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF78BA90A, "sceKernelStderr" )]
		int sceKernelStderr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98220F3E, "sceKernelStdoutReopen" )]
		int sceKernelStdoutReopen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB5380C5, "sceKernelStderrReopen" )]
		int sceKernelStderrReopen(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C20BEEFA */
