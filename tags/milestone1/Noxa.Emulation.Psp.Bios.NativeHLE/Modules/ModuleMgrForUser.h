// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					public ref class ModuleMgrForUser : public Module
					{
					public:
						ModuleMgrForUser( IntPtr kernel ) : Module( kernel ) {}
						~ModuleMgrForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "ModuleMgrForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

						[BiosFunction( 0xD675EBB8, "sceKernelSelfStopUnloadModule" )]
						// int sceKernelSelfStopUnloadModule(int unknown, SceSize argsize, void *argp); (/user/pspmodulemgr.h:152)
						int sceKernelSelfStopUnloadModule( int unknown, int argsize, int argp );

						[BiosFunction( 0xCC1D3699, "sceKernelStopUnloadSelfModule" )]
						// int sceKernelStopUnloadSelfModule(SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:164)
						int sceKernelStopUnloadSelfModule( int argsize, int argp, int status, int option );

						[BiosFunction( 0xB7F46618, "sceKernelLoadModuleByID" )] [Stateless]
						// SceUID sceKernelLoadModuleByID(SceUID fid, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:91)
						int sceKernelLoadModuleByID( IMemory^ memory, int fid, int flags, int option );

						[BiosFunction( 0x977DE386, "sceKernelLoadModule" )] [Stateless]
						// SceUID sceKernelLoadModule(const char *path, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:68)
						int sceKernelLoadModule( IMemory^ memory, int path, int flags, int option );

						[BiosFunction( 0x50F0C1EC, "sceKernelStartModule" )]
						// int sceKernelStartModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:119)
						int sceKernelStartModule( IMemory^ memory, int modid, int argsize, int argp, int status, int option );

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x710F61B5, "sceKernelLoadModuleMs" )] [Stateless]
						// SceUID sceKernelLoadModuleMs(const char *path, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:80)
						int sceKernelLoadModuleMs( int path, int flags, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF9275D98, "sceKernelLoadModuleBufferUsbWlan" )] [Stateless]
						// SceUID sceKernelLoadModuleBufferUsbWlan(SceSize bufsize, void *buf, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:106)
						int sceKernelLoadModuleBufferUsbWlan( int bufsize, int buf, int flags, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC629AF26, "sceUtilityLoadAvModule" )] [Stateless]
						// manual add - loads avcodec.prx (or audiocodec.prx)
						int sceUtilityLoadAvModule( int module ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF7D8D092, "sceUtilityUnloadAvModule" )] [Stateless]
						// manual add
						int sceUtilityUnloadAvModule( int module ){ return NISTUBRETURN; }

						//#define PSP_NET_MODULE_COMMON 1
						//#define PSP_NET_MODULE_ADHOC 2
						//#define PSP_NET_MODULE_INET 3
						//#define PSP_NET_MODULE_PARSEURI 4
						//#define PSP_NET_MODULE_PARSEHTTP 5
						//#define PSP_NET_MODULE_HTTP 6
						//#define PSP_NET_MODULE_SSL 7
						[NotImplemented]
						[BiosFunction( 0x1579A159, "sceUtilityLoadNetModule" )] [Stateless]
						// manual add - loads one of the PSP_NET_MODULE_ prxs
						int sceUtilityLoadNetModule( int module ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x64D50C56, "sceUtilityUnloadNetModule" )] [Stateless]
						// manual add
						int sceUtilityUnloadNetModule( int module ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD1FF982A, "sceKernelStopModule" )] [Stateless]
						// int sceKernelStopModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:132)
						int sceKernelStopModule( int modid, int argsize, int argp, int status, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2E0911AA, "sceKernelUnloadModule" )] [Stateless]
						// int sceKernelUnloadModule(SceUID modid); (/user/pspmodulemgr.h:141)
						int sceKernelUnloadModule( int modid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF0A26395, "sceKernelGetModuleId" )] [Stateless]
						// manual add
						int sceKernelGetModuleId(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD8B73127, "sceKernelGetModuleIdByAddress" )] [Stateless]
						// manual add
						int sceKernelGetModuleIdByAddress( int address ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x748CBED9, "sceKernelQueryModuleInfo" )] [Stateless]
						// int sceKernelQueryModuleInfo(SceUID modid, SceKernelModuleInfo *info); (/user/pspmodulemgr.h:198)
						int sceKernelQueryModuleInfo( int modid, int info ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - E6ADC14F */
