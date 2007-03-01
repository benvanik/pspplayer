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

					ref class SysMemUserForUser : public Module
					{
					public:
						SysMemUserForUser( Kernel^ kernel ) : Module( kernel ) {}
						~SysMemUserForUser(){}

						property String^ Name { virtual String^ get() override { return "SysMemUserForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xA291F107, "sceKernelMaxFreeMemSize" )] [Stateless]
						// /user/pspsysmem.h:88: SceSize sceKernelMaxFreeMemSize();
						int sceKernelMaxFreeMemSize(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF919F628, "sceKernelTotalFreeMemSize" )] [Stateless]
						// /user/pspsysmem.h:81: SceSize sceKernelTotalFreeMemSize();
						int sceKernelTotalFreeMemSize(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x237DBD4F, "sceKernelAllocPartitionMemory" )] [Stateless]
						// /user/pspsysmem.h:56: SceUID sceKernelAllocPartitionMemory(SceUID partitionid, const char *name, int type, SceSize size, void *addr);
						int sceKernelAllocPartitionMemory( int partitionid, int name, int type, int size, int addr ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB6D61D02, "sceKernelFreePartitionMemory" )] [Stateless]
						// /user/pspsysmem.h:65: int sceKernelFreePartitionMemory(SceUID blockid);
						int sceKernelFreePartitionMemory( int blockid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9D9A5BA1, "sceKernelGetBlockHeadAddr" )] [Stateless]
						// /user/pspsysmem.h:74: void * sceKernelGetBlockHeadAddr(SceUID blockid);
						int sceKernelGetBlockHeadAddr( int blockid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3FC9AE6A, "sceKernelDevkitVersion" )] [Stateless]
						// /user/pspsysmem.h:104: int sceKernelDevkitVersion();
						int sceKernelDevkitVersion(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 781FEFFF */
