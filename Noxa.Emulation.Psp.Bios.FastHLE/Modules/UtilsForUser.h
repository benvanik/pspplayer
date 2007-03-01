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

					ref class UtilsForUser : public Module
					{
					public:
						UtilsForUser( Kernel^ kernel ) : Module( kernel ) {}
						~UtilsForUser(){}

						property String^ Name { virtual String^ get() override { return "UtilsForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xBFA98062, "sceKernelDcacheInvalidateRange" )] [Stateless]
						// void sceKernelDcacheInvalidateRange(const void *p, unsigned int size); (/user/psputils.h:73)
						void sceKernelDcacheInvalidateRange( int p, int size ){}

						[NotImplemented]
						[BiosFunction( 0xC8186A58, "sceKernelUtilsMd5Digest" )] [Stateless]
						// int sceKernelUtilsMd5Digest(u8 *data, u32 size, u8 *digest); (/user/psputils.h:125)
						int sceKernelUtilsMd5Digest( int data, int size, int digest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9E5C5086, "sceKernelUtilsMd5BlockInit" )] [Stateless]
						// int sceKernelUtilsMd5BlockInit(SceKernelUtilsMd5Context *ctx); (/user/psputils.h:142)
						int sceKernelUtilsMd5BlockInit( int ctx ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x61E1E525, "sceKernelUtilsMd5BlockUpdate" )] [Stateless]
						// int sceKernelUtilsMd5BlockUpdate(SceKernelUtilsMd5Context *ctx, u8 *data, u32 size); (/user/psputils.h:153)
						int sceKernelUtilsMd5BlockUpdate( int ctx, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB8D24E78, "sceKernelUtilsMd5BlockResult" )] [Stateless]
						// int sceKernelUtilsMd5BlockResult(SceKernelUtilsMd5Context *ctx, u8 *digest); (/user/psputils.h:163)
						int sceKernelUtilsMd5BlockResult( int ctx, int digest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x840259F1, "sceKernelUtilsSha1Digest" )] [Stateless]
						// int sceKernelUtilsSha1Digest(u8 *data, u32 size, u8 *digest); (/user/psputils.h:183)
						int sceKernelUtilsSha1Digest( int data, int size, int digest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF8FCD5BA, "sceKernelUtilsSha1BlockInit" )] [Stateless]
						// int sceKernelUtilsSha1BlockInit(SceKernelUtilsSha1Context *ctx); (/user/psputils.h:201)
						int sceKernelUtilsSha1BlockInit( int ctx ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x346F6DA8, "sceKernelUtilsSha1BlockUpdate" )] [Stateless]
						// int sceKernelUtilsSha1BlockUpdate(SceKernelUtilsSha1Context *ctx, u8 *data, u32 size); (/user/psputils.h:212)
						int sceKernelUtilsSha1BlockUpdate( int ctx, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x585F1C09, "sceKernelUtilsSha1BlockResult" )] [Stateless]
						// int sceKernelUtilsSha1BlockResult(SceKernelUtilsSha1Context *ctx, u8 *digest); (/user/psputils.h:222)
						int sceKernelUtilsSha1BlockResult( int ctx, int digest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE860E75E, "sceKernelUtilsMt19937Init" )] [Stateless]
						// int sceKernelUtilsMt19937Init(SceKernelUtilsMt19937Context *ctx, u32 seed); (/user/psputils.h:96)
						int sceKernelUtilsMt19937Init( int ctx, int seed ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x06FB8A63, "sceKernelUtilsMt19937UInt" )] [Stateless]
						// u32 sceKernelUtilsMt19937UInt(SceKernelUtilsMt19937Context *ctx); (/user/psputils.h:104)
						int sceKernelUtilsMt19937UInt( int ctx ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x91E4F6A7, "sceKernelLibcClock" )] [Stateless]
						// clock_t sceKernelLibcClock(); (/user/psputils.h:43)
						int sceKernelLibcClock(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x27CC57F0, "sceKernelLibcTime" )] [Stateless]
						// time_t sceKernelLibcTime(time_t *t); (/user/psputils.h:38)
						int sceKernelLibcTime( int t ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x71EC4271, "sceKernelLibcGettimeofday" )] [Stateless]
						// int sceKernelLibcGettimeofday(struct timeval *tp, struct timezone *tzp); (/user/psputils.h:48)
						int sceKernelLibcGettimeofday( int tp, int tzp ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x79D1C3FA, "sceKernelDcacheWritebackAll" )] [Stateless]
						// void sceKernelDcacheWritebackAll(); (/user/psputils.h:53)
						void sceKernelDcacheWritebackAll(){}

						[NotImplemented]
						[BiosFunction( 0xB435DEC5, "sceKernelDcacheWritebackInvalidateAll" )] [Stateless]
						// void sceKernelDcacheWritebackInvalidateAll(); (/user/psputils.h:58)
						void sceKernelDcacheWritebackInvalidateAll(){}

						[NotImplemented]
						[BiosFunction( 0x3EE30821, "sceKernelDcacheWritebackRange" )] [Stateless]
						// void sceKernelDcacheWritebackRange(const void *p, unsigned int size); (/user/psputils.h:63)
						void sceKernelDcacheWritebackRange( int p, int size ){}

						[NotImplemented]
						[BiosFunction( 0x34B9FA9E, "sceKernelDcacheWritebackInvalidateRange" )] [Stateless]
						// void sceKernelDcacheWritebackInvalidateRange(const void *p, unsigned int size); (/user/psputils.h:68)
						void sceKernelDcacheWritebackInvalidateRange( int p, int size ){}

						[NotImplemented]
						[BiosFunction( 0x80001C4C, "sceKernelDcacheProbe" )] [Stateless]
						// int sceKernelDcacheProbe(void *addr); (/kernel/psputilsforkernel.h:43)
						int sceKernelDcacheProbe( int addr ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4FD31C9D, "sceKernelIcacheProbe" )] [Stateless]
						// int sceKernelIcacheProbe(const void *addr); (/kernel/psputilsforkernel.h:63)
						int sceKernelIcacheProbe( int addr ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 2BA95326 */
