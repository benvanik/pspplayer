using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class KernelLibrary : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public KernelLibrary( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "Kernel_Library";
			}
		}

		#endregion

		[BiosStub( 0x092968f4, "sceKernelCpuSuspendIntr", true, 0 )]
		public int sceKernelCpuSuspendIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// We don't implement interrupts - the return would be a mask of currently enabled interrupts

			// unsigned int
			return 0;
		}

		[BiosStub( 0x5f10d406, "sceKernelCpuResumeIntr", false, 1 )]
		public int sceKernelCpuResumeIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = unsigned int flags

			// We don't implement interrupts - the param would be a mask of the interrupts that were disabled

			return 0;
		}

		[BiosStub( 0x3b84732d, "sceKernelCpuResumeIntrWithSync", false, 1 )]
		[BiosStubIncomplete]
		public int sceKernelCpuResumeIntrWithSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = unsigned int flags

			return 0;
		}

		[BiosStub( 0x47a0b729, "sceKernelIsCpuIntrSuspended", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelIsCpuIntrSuspended( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = unsigned int flags

			// int
			return 0;
		}

		[BiosStub( 0xb55249d2, "sceKernelIsCpuIntrEnable", true, 0 )]
		[BiosStubIncomplete]
		public int sceKernelIsCpuIntrEnable( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}
	}
}
