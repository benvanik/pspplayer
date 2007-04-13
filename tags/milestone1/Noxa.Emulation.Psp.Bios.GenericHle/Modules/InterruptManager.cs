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
	class InterruptManager : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public InterruptManager( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "InterruptManager";
			}
		}

		#endregion

		[BiosStub( 0xeee43f47, "sceKernelRegisterUserSpaceIntrStack", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterUserSpaceIntrStack( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xca04a2b9, "sceKernelRegisterSubIntrHandler", true, 4 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterSubIntrHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int intno
			// a1 = int no
			// a2 = void *handler
			// a3 = void *arg

			// int
			return 0;
		}

		[BiosStub( 0xd61e6961, "sceKernelReleaseSubIntrHandler", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseSubIntrHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int intno
			// a1 = int no

			// int
			return 0;
		}

		[BiosStub( 0xfb8e22ec, "sceKernelEnableSubIntr", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelEnableSubIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int intno
			// a1 = int no

			// int
			return 0;
		}

		[BiosStub( 0x8a389411, "sceKernelDisableSubIntr", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelDisableSubIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int intno
			// a1 = int no

			// int
			return 0;
		}

		[BiosStub( 0x5cb5a78b, "sceKernelSuspendSubIntr", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSuspendSubIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7860e0dc, "sceKernelResumeSubIntr", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelResumeSubIntr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfc4374b8, "sceKernelIsSubInterruptOccurred", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelIsSubInterruptOccurred( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd2e8363f, "QueryIntrHandlerInfo", false, 0 )]
		[BiosStubIncomplete]
		public int QueryIntrHandlerInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
