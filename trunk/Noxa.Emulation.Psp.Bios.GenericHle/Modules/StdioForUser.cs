#define LOGNOTIMPLEMENTED

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class StdioForUser : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public StdioForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "StdioForUser";
			}
		}

		#endregion

		[BiosStub( 0x3054d478, "sceKernelStdioRead", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioRead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0cbb0571, "sceKernelStdioLseek", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioLseek( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa46785c9, "sceKernelStdioSendChar", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioSendChar( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa3b931db, "sceKernelStdioWrite", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioWrite( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9d061c19, "sceKernelStdioClose", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioClose( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x924aba61, "sceKernelStdioOpen", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStdioOpen( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x172d316e, "sceKernelStdin", true, 0 )]
		public int sceKernelStdin( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// SceUID
			return _kernel.StdIn.Uid;
		}

		[BiosStub( 0xa6bab2e9, "sceKernelStdout", true, 0 )]
		public int sceKernelStdout( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// SceUID
			return _kernel.StdOut.Uid;
		}

		[BiosStub( 0xf78ba90a, "sceKernelStderr", true, 0 )]
		public int sceKernelStderr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// SceUID
			return _kernel.StdErr.Uid;
		}
	}
}
