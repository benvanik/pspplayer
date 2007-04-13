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

					public ref class SysMemUserForUser : public Module
					{
					public:
						SysMemUserForUser( Kernel^ kernel ) : Module( kernel ) {}
						~SysMemUserForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "SysMemUserForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

						// If these get called frequently we should optimize them (more bookkeeping in the kernel)

						[BiosFunction( 0xA291F107, "sceKernelMaxFreeMemSize" )] [Stateless]
						// SceSize sceKernelMaxFreeMemSize(); (/user/pspsysmem.h:88)
						int sceKernelMaxFreeMemSize();

						[BiosFunction( 0xF919F628, "sceKernelTotalFreeMemSize" )] [Stateless]
						// SceSize sceKernelTotalFreeMemSize(); (/user/pspsysmem.h:81)
						int sceKernelTotalFreeMemSize();

						[BiosFunction( 0xE6581468, "sceKernelPartitionMaxFreeMemSize" )] [Stateless]
						// manual add
						int sceKernelPartitionMaxFreeMemSize( int partitionid );

						[BiosFunction( 0x9697CD32, "sceKernelPartitionTotalFreeMemSize" )] [Stateless]
						// manual add
						int sceKernelPartitionTotalFreeMemSize( int partitionid );

						[BiosFunction( 0x237DBD4F, "sceKernelAllocPartitionMemory" )] [Stateless]
						// SceUID sceKernelAllocPartitionMemory(SceUID partitionid, const char *name, int type, SceSize size, void *addr); (/user/pspsysmem.h:56)
						int sceKernelAllocPartitionMemory( IMemory^ memory, int partitionid, int name, int type, int size, int addr );

						[BiosFunction( 0xB6D61D02, "sceKernelFreePartitionMemory" )] [Stateless]
						// int sceKernelFreePartitionMemory(SceUID blockid); (/user/pspsysmem.h:65)
						int sceKernelFreePartitionMemory( int blockid );

						[BiosFunction( 0x9D9A5BA1, "sceKernelGetBlockHeadAddr" )] [Stateless]
						// void * sceKernelGetBlockHeadAddr(SceUID blockid); (/user/pspsysmem.h:74)
						int sceKernelGetBlockHeadAddr( int blockid );

						[BiosFunction( 0x3FC9AE6A, "sceKernelDevkitVersion" )] [Stateless]
						// int sceKernelDevkitVersion(); (/user/pspsysmem.h:104)
						int sceKernelDevkitVersion();

					public: // ------ Stubbed calls ------

						[BiosFunction( 0xF77D77CB, "sceKernelSetCompilerVersion" )] [Stateless]
						// manual add - check?
						int sceKernelSetCompilerVersion( int version ){ return 0; }

						[BiosFunction( 0x7591C7DB, "sceKernelSetCompiledSdkVersion" )] [Stateless]
						// manual add - check?
						int sceKernelSetCompiledSdkVersion( int version ){ return 0; }

						[NotImplemented]
						[BiosFunction( 0x13A5ABEF, "sceKernelPrintf" )] [Stateless]
						// manual add - printf( char* format, ... ) <-- right 2nd arg?
						int sceKernelPrintf( IMemory^ memory, int format, int varg ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - D35D6873 */
