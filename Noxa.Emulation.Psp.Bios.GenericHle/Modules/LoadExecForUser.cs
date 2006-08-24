// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class LoadExecForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public LoadExecForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "LoadExecForUser";
			}
		}

		#endregion

		[BiosStub( 0xbd2f1094, "sceKernelLoadExec", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelLoadExec( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file
			// a1 = struct SceKernelLoadExecParam *param

			// size of the structure (16)
			// argument size (bytes)
			// argument pointer
			// ?

			// int
			return 0;
		}

		[BiosStub( 0x2ac9954b, "sceKernelExitGameWithStatus", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelExitGameWithStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = status

			Debug.WriteLine( string.Format( "sceKernelExitGameWithStatus: status = {0}", a0 ) );
			_kernel.ExitGame();

			return 0;
		}

		[BiosStub( 0x05572a5f, "sceKernelExitGame", true, 0 )]
		public int sceKernelExitGame( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return this.sceKernelExitGameWithStatus( memory, 0, 0, 0, 0, sp );
		}

		[BiosStub( 0x4ac57943, "sceKernelRegisterExitCallback", true, 1 )]
		public int sceKernelRegisterExitCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cbid

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			_kernel.Callbacks.Add( KernelCallbackName.Exit, callback );

			// int
			return 0;
		}
	}
}
