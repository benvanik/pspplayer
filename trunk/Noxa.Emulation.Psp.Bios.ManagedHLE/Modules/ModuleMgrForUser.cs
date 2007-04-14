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
	class ModuleMgrForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ModuleMgrForUser";
			}
		}

		#endregion

		#region State Management

		public ModuleMgrForUser( Kernel kernel )
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
		[BiosFunction( 0xB7F46618, "sceKernelLoadModuleByID" )]
		// SDK location: /user/pspmodulemgr.h:91
		// SDK declaration: SceUID sceKernelLoadModuleByID(SceUID fid, int flags, SceKernelLMOption *option);
		int sceKernelLoadModuleByID( int fid, int flags, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x977DE386, "sceKernelLoadModule" )]
		// SDK location: /user/pspmodulemgr.h:68
		// SDK declaration: SceUID sceKernelLoadModule(const char *path, int flags, SceKernelLMOption *option);
		int sceKernelLoadModule( int path, int flags, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x710F61B5, "sceKernelLoadModuleMs" )]
		// SDK location: /user/pspmodulemgr.h:80
		// SDK declaration: SceUID sceKernelLoadModuleMs(const char *path, int flags, SceKernelLMOption *option);
		int sceKernelLoadModuleMs( int path, int flags, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9275D98, "sceKernelLoadModuleBufferUsbWlan" )]
		// SDK location: /user/pspmodulemgr.h:106
		// SDK declaration: SceUID sceKernelLoadModuleBufferUsbWlan(SceSize bufsize, void *buf, int flags, SceKernelLMOption *option);
		int sceKernelLoadModuleBufferUsbWlan( int bufsize, int buf, int flags, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50F0C1EC, "sceKernelStartModule" )]
		// SDK location: /user/pspmodulemgr.h:119
		// SDK declaration: int sceKernelStartModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		int sceKernelStartModule( int modid, int argsize, int argp, int status, int option ){ return Module.NotImplementedReturn; }

		[Stateless]
		[NotImplemented]
		[BiosFunction( 0xC629AF26, "sceUtilityLoadAvModule" )]
		// manual add - loads avcodec.prx (or audiocodec.prx)
		int sceUtilityLoadAvModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[NotImplemented]
		[BiosFunction( 0xF7D8D092, "sceUtilityUnloadAvModule" )]
		// manual add
		int sceUtilityUnloadAvModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		//#define PSP_NET_MODULE_COMMON 1
		//#define PSP_NET_MODULE_ADHOC 2
		//#define PSP_NET_MODULE_INET 3
		//#define PSP_NET_MODULE_PARSEURI 4
		//#define PSP_NET_MODULE_PARSEHTTP 5
		//#define PSP_NET_MODULE_HTTP 6
		//#define PSP_NET_MODULE_SSL 7
		[Stateless]
		[NotImplemented]
		[BiosFunction( 0x1579A159, "sceUtilityLoadNetModule" )]
		// manual add - loads one of the PSP_NET_MODULE_ prxs
		int sceUtilityLoadNetModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[NotImplemented]
		[BiosFunction( 0x64D50C56, "sceUtilityUnloadNetModule" )]
		// manual add
		int sceUtilityUnloadNetModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1FF982A, "sceKernelStopModule" )]
		// SDK location: /user/pspmodulemgr.h:132
		// SDK declaration: int sceKernelStopModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		int sceKernelStopModule( int modid, int argsize, int argp, int status, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E0911AA, "sceKernelUnloadModule" )]
		// SDK location: /user/pspmodulemgr.h:141
		// SDK declaration: int sceKernelUnloadModule(SceUID modid);
		int sceKernelUnloadModule( int modid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0A26395, "sceKernelGetModuleId" )]
		// manual add
		int sceKernelGetModuleId()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8B73127, "sceKernelGetModuleIdByAddress" )]
		// manual add
		int sceKernelGetModuleIdByAddress( int address )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD675EBB8, "sceKernelSelfStopUnloadModule" )]
		// SDK location: /user/pspmodulemgr.h:152
		// SDK declaration: int sceKernelSelfStopUnloadModule(int unknown, SceSize argsize, void *argp);
		int sceKernelSelfStopUnloadModule( int unknown, int argsize, int argp ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC1D3699, "sceKernelStopUnloadSelfModule" )]
		// SDK location: /user/pspmodulemgr.h:164
		// SDK declaration: int sceKernelStopUnloadSelfModule(SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		int sceKernelStopUnloadSelfModule( int argsize, int argp, int status, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x748CBED9, "sceKernelQueryModuleInfo" )]
		// SDK location: /user/pspmodulemgr.h:198
		// SDK declaration: int sceKernelQueryModuleInfo(SceUID modid, SceKernelModuleInfo *info);
		int sceKernelQueryModuleInfo( int modid, int info ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4E257708 */
