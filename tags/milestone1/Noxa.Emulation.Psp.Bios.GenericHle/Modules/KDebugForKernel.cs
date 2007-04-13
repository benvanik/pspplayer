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
	class KDebugForKernel : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public KDebugForKernel( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "KDebugForKernel";
			}
		}

		#endregion

		[BiosStub( 0xE7A3874D, "sceKernelRegisterAssertHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterAssertHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x23906FB2, "KDebugForKernel_0x23906FB2", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown0( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2FF4E9F9, "sceKernelAssert", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAssert( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9B868276, "sceKernelGetDebugPutchar", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelGetDebugPutchar( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void

			// PspDebugPutChar
			return 0;
		}

		[BiosStub( 0xE146606D, "sceKernelRegisterDebugPutchar", false, 1 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterDebugPutchar( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = PspDebugPutChar func

			return 0;
		}

		[BiosStub( 0x7CEB2C09, "sceKernelRegisterKprintfHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterKprintfHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xAFB8FC80, "KDebugForKernel_0xAFB8FC80", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x84F370BC, "Kprintf", false, 2 )]
		[BiosStubIncomplete]
		public int Kprintf( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *format
			// a1 = 

			return 0;
		}

		[BiosStub( 0x5CE9838B, "sceKernelDebugWrite", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDebugWrite( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x66253C4E, "sceKernelRegisterDebugWrite", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterDebugWrite( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xDBB5597F, "sceKernelDebugRead", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDebugRead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xE6554FDA, "sceKernelRegisterDebugRead", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterDebugRead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xB9C643C9, "sceKernelDebugEcho", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDebugEcho( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7D1C74F0, "sceKernelDebugEchoSet", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDebugEchoSet( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x8B041DFB, "KDebugForKernel_0x8B041DFB", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x24C32559, "KDebugForKernel_0x24C32559", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xD636B827, "sceKernelRemoveByDebugSection", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRemoveByDebugSection( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5282DD5E, "KDebugForKernel_0x5282DD5E", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xEE75658D, "KDebugForKernel_0xEE75658D", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9F8703E4, "KDebugForKernel_0x9F8703E4", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x333DCEC7, "KDebugForKernel_0x333DCEC7", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown7( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x428A8DA3, "KDebugForKernel_0x428A8DA3", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown8( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xE892D9A1, "KDebugForKernel_0xE892D9A1", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown9( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xFFD2F2B9, "KDebugForKernel_0xFFD2F2B9", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown10( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xA126F497, "KDebugForKernel_0xA126F497", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown11( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xB7251823, "sceKernelAcceptMbogoSig", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAcceptMbogoSig( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x72E6B3B8, "KDebugForKernel_0x72E6B3B8", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown12( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xA216AE06, "KDebugForKernel_0xA216AE06", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown13( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
