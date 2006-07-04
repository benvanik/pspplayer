#define LOGNOTIMPLEMENTED

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class SuspendForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public SuspendForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceSuspendForUser";
			}
		}

		#endregion

		[BiosStub( 0xeadb1bd7, "sceKernelPowerLock", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelPowerLock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3aee7261, "sceKernelPowerUnlock", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelPowerUnlock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x090ccb3f, "sceKernelPowerTick", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelPowerTick( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
