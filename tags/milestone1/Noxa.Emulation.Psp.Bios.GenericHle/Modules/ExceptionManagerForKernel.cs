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
	class ExceptionManagerForKernel : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public ExceptionManagerForKernel( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "ExceptionManagerForKernel";
			}
		}

		#endregion

		[BiosStub( 0x3FB264FC, "sceKernelRegisterExceptionHandler", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int exno
			// a1 = void *func

			// int
			return 0;
		}

		[BiosStub( 0x5A837AD4, "sceKernelRegisterPriorityExceptionHandler", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterPriorityExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int exno
			// a1 = int priority
			// a2 = void *func

			// int
			return 0;
		}

		[BiosStub( 0x565C0B0E, "sceKernelRegisterDefaultExceptionHandler", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterDefaultExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void *func

			// int
			return 0;
		}

		[BiosStub( 0x1AA6CFFA, "sceKernelReleaseExceptionHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xDF83875E, "sceKernelGetActiveDefaultExceptionHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetActiveDefaultExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x291FF031, "sceKernelReleaseDefaultExceptionHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseDefaultExceptionHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x15ADC862, "sceKernelRegisterNmiHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterNmiHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xB15357C9, "sceKernelReleaseNmiHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseNmiHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xFC26C354, "ExceptionManagerForKernel_0xFC26C354", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown0( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
