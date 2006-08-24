// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define LOGNOTIMPLEMENTED

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class ModuleMgrForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public ModuleMgrForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "ModuleMgrForUser";
			}
		}

		#endregion

		[BiosStub( 0xb7f46618, "sceKernelLoadModuleByID", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelLoadModuleByID( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fid
			// a1 = int flags
			// a2 = SceKernelLMOption *option

			// SceUID
			return 0;
		}

		[BiosStub( 0x977de386, "sceKernelLoadModule", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelLoadModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *path
			// a1 = int flags
			// a2 = SceKernelLMOption *option

			// SceUID
			return 0;
		}

		[BiosStub( 0x710f61b5, "sceKernelLoadModuleMs", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelLoadModuleMs( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *path
			// a1 = int flags
			// a2 = SceKernelLMOption *option

			// SceUID
			return 0;
		}

		[BiosStub( 0xf9275d98, "sceKernelLoadModuleBufferUsbWlan", true, 4 )]
		[BiosStubIncomplete]
		public int sceKernelLoadModuleBufferUsbWlan( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceSize bufsize
			// a1 = void *buf
			// a2 = int flags
			// a3 = SceKernelLMOption *option

			// SceUID
			return 0;
		}

		[BiosStub( 0x50f0c1ec, "sceKernelStartModule", true, 5 )]
		[BiosStubIncomplete]
		public int sceKernelStartModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID modid
			// a1 = SceSize argsize
			// a2 = void *argp
			// a3 = int *status
			// sp[0] = SceKernelSMOption *option
			int a4 = memory.ReadWord( sp + 0 );

			// int
			return 0;
		}

		[BiosStub( 0xd1ff982a, "sceKernelStopModule", true, 5 )]
		[BiosStubIncomplete]
		public int sceKernelStopModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID modid
			// a1 = SceSize argsize
			// a2 = void *argp
			// a3 = int *status
			// sp[0] = SceKernelSMOption *option
			int a4 = memory.ReadWord( sp + 0 );

			// int
			return 0;
		}

		[BiosStub( 0x2e0911aa, "sceKernelUnloadModule", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelUnloadModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID modid

			// int
			return 0;
		}

		[BiosStub( 0xd675ebb8, "sceKernelSelfStopUnloadModule", true, 3 )]
		public int sceKernelSelfStopUnloadModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown
			// a1 = SceSize argsize
			// a2 = void *argp

			// Piggy back onto the other function... great names!
			return this.sceKernelStopUnloadSelfModule( memory, a1, a2, 0, 0, sp );

			// int
			//return 0;
		}

		[BiosStub( 0xcc1d3699, "sceKernelStopUnloadSelfModule", true, 4 )]
		public int sceKernelStopUnloadSelfModule( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceSize argsize
			// a1 = void *argp
			// a2 = int *status
			// a3 = SceKernelSMOption *option

			// Don't care
			_kernel.ExitGame();

			// int
			return 0;
		}

		[BiosStub( 0x748cbed9, "sceKernelQueryModuleInfo", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelQueryModuleInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID modid
			// a1 = SceKernelModuleInfo *info

			// int
			return 0;
		}

		[BiosStub( 0xf0a26395, "sceKernelGetModuleId", true, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleId( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// The UID of the module calling the function on success, otherwise < 0 
			return 0;
		}

		[BiosStub( 0xd8b73127, "sceKernelGetModuleIdByAddress", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelGetModuleIdByAddress( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = Address somewhere within the module

			// The UID of the module on success, otherwise < 0
			return 0;
		}
	}
}
