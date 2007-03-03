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

					public ref class IoFileMgrForUser : public Module
					{
					public:
						IoFileMgrForUser( Kernel^ kernel ) : Module( kernel ) {}
						~IoFileMgrForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "IoFileMgrForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x3251EA56, "sceIoPollAsync" )] [Stateless]
						// int sceIoPollAsync(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:419)
						int sceIoPollAsync( int fd, int64 res ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE23EEC33, "sceIoWaitAsync" )] [Stateless]
						// int sceIoWaitAsync(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:399)
						int sceIoWaitAsync( int fd, int64 res ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x35DBD746, "sceIoWaitAsyncCB" )] [Stateless]
						// int sceIoWaitAsyncCB(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:409)
						int sceIoWaitAsyncCB( int fd, int64 res ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCB05F8D6, "sceIoGetAsyncStat" )] [Stateless]
						// int sceIoGetAsyncStat(SceUID fd, int poll, SceInt64 *res); (/user/pspiofilemgr.h:430)
						int sceIoGetAsyncStat( int fd, int poll, int64 res ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB293727F, "sceIoChangeAsyncPriority" )] [Stateless]
						// int sceIoChangeAsyncPriority(SceUID fd, int pri); (/user/pspiofilemgr.h:458)
						int sceIoChangeAsyncPriority( int fd, int pri ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA12A0514, "sceIoSetAsyncCallback" )] [Stateless]
						// int sceIoSetAsyncCallback(SceUID fd, SceUID cb, void *argp); (/user/pspiofilemgr.h:469)
						int sceIoSetAsyncCallback( int fd, int cb, int argp ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x810C4BC3, "sceIoClose" )] [Stateless]
						// int sceIoClose(SceUID fd); (/user/pspiofilemgr.h:85)
						int sceIoClose( int fd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFF5940B6, "sceIoCloseAsync" )] [Stateless]
						// int sceIoCloseAsync(SceUID fd); (/user/pspiofilemgr.h:93)
						int sceIoCloseAsync( int fd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x109F50BC, "sceIoOpen" )] [Stateless]
						// SceUID sceIoOpen(const char *file, int flags, SceMode mode); (/user/pspiofilemgr.h:63)
						int sceIoOpen( int file, int flags, int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x89AA9906, "sceIoOpenAsync" )] [Stateless]
						// SceUID sceIoOpenAsync(const char *file, int flags, SceMode mode); (/user/pspiofilemgr.h:73)
						int sceIoOpenAsync( int file, int flags, int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6A638D83, "sceIoRead" )] [Stateless]
						// int sceIoRead(SceUID fd, void *data, SceSize size); (/user/pspiofilemgr.h:109)
						int sceIoRead( int fd, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA0B5A7C2, "sceIoReadAsync" )] [Stateless]
						// int sceIoReadAsync(SceUID fd, void *data, SceSize size); (/user/pspiofilemgr.h:125)
						int sceIoReadAsync( int fd, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x42EC03AC, "sceIoWrite" )] [Stateless]
						// int sceIoWrite(SceUID fd, const void *data, SceSize size); (/user/pspiofilemgr.h:141)
						int sceIoWrite( int fd, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0FACAB19, "sceIoWriteAsync" )] [Stateless]
						// int sceIoWriteAsync(SceUID fd, const void *data, SceSize size); (/user/pspiofilemgr.h:152)
						int sceIoWriteAsync( int fd, int data, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x27EB27B8, "sceIoLseek" )] [Stateless]
						// SceOff sceIoLseek(SceUID fd, SceOff offset, int whence); (/user/pspiofilemgr.h:169)
						int64 sceIoLseek( int fd, int64 offset, int whence ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x71B19E77, "sceIoLseekAsync" )] [Stateless]
						// int sceIoLseekAsync(SceUID fd, SceOff offset, int whence); (/user/pspiofilemgr.h:181)
						int sceIoLseekAsync( int fd, int64 offset, int whence ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x68963324, "sceIoLseek32" )] [Stateless]
						// int sceIoLseek32(SceUID fd, int offset, int whence); (/user/pspiofilemgr.h:198)
						int sceIoLseek32( int fd, int offset, int whence ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1B385D8F, "sceIoLseek32Async" )] [Stateless]
						// int sceIoLseek32Async(SceUID fd, int offset, int whence); (/user/pspiofilemgr.h:210)
						int sceIoLseek32Async( int fd, int offset, int whence ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x63632449, "sceIoIoctl" )] [Stateless]
						// int sceIoIoctl(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:368)
						int sceIoIoctl( int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE95A012B, "sceIoIoctlAsync" )] [Stateless]
						// int sceIoIoctlAsync(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:381)
						int sceIoIoctlAsync( int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB29DDF9C, "sceIoDopen" )] [Stateless]
						// SceUID sceIoDopen(const char *dirname); (/user/pspiofilemgr.h:267)
						int sceIoDopen( int dirname ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE3EB004C, "sceIoDread" )] [Stateless]
						// int sceIoDread(SceUID fd, SceIoDirent *dir); (/user/pspiofilemgr.h:280)
						int sceIoDread( int fd, int dir ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEB092469, "sceIoDclose" )] [Stateless]
						// int sceIoDclose(SceUID fd); (/user/pspiofilemgr.h:288)
						int sceIoDclose( int fd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF27A9C51, "sceIoRemove" )] [Stateless]
						// int sceIoRemove(const char *file); (/user/pspiofilemgr.h:218)
						int sceIoRemove( int file ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x06A70004, "sceIoMkdir" )] [Stateless]
						// int sceIoMkdir(const char *dir, SceMode mode); (/user/pspiofilemgr.h:227)
						int sceIoMkdir( int dir, int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1117C65F, "sceIoRmdir" )] [Stateless]
						// int sceIoRmdir(const char *path); (/user/pspiofilemgr.h:235)
						int sceIoRmdir( int path ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x55F4717D, "sceIoChdir" )] [Stateless]
						// int sceIoChdir(const char *path); (/user/pspiofilemgr.h:243)
						int sceIoChdir( int path ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAB96437F, "sceIoSync" )] [Stateless]
						// int sceIoSync(const char *device, unsigned int unk); (/user/pspiofilemgr.h:389)
						int sceIoSync( int device, int unk ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xACE946E8, "sceIoGetstat" )] [Stateless]
						// int sceIoGetstat(const char *file, SceIoStat *stat); (/user/pspiofilemgr.h:344)
						int sceIoGetstat( int file, int stat ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB8A740F4, "sceIoChstat" )] [Stateless]
						// int sceIoChstat(const char *file, SceIoStat *stat, int bits); (/user/pspiofilemgr.h:355)
						int sceIoChstat( int file, int stat, int bits ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x779103A0, "sceIoRename" )] [Stateless]
						// int sceIoRename(const char *oldname, const char *newname); (/user/pspiofilemgr.h:252)
						int sceIoRename( int oldname, int newname ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x54F5FB11, "sceIoDevctl" )] [Stateless]
						// int sceIoDevctl(const char *dev, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:306)
						int sceIoDevctl( int dev, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x08BD7374, "sceIoGetDevType" )] [Stateless]
						// int sceIoGetDevType(SceUID fd); (/user/pspiofilemgr.h:448)
						int sceIoGetDevType( int fd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB2A628C1, "sceIoAssign" )] [Stateless]
						// int sceIoAssign(const char *dev1, const char *dev2, const char *dev3, int mode, void* unk1, long unk2); (/user/pspiofilemgr.h:325)
						int sceIoAssign( int dev1, int dev2, int dev3, int mode, int unk1, int unk2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6D08A871, "sceIoUnassign" )] [Stateless]
						// int sceIoUnassign(const char *dev); (/user/pspiofilemgr.h:334)
						int sceIoUnassign( int dev ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE8BC6571, "sceIoCancel" )] [Stateless]
						// int sceIoCancel(SceUID fd); (/user/pspiofilemgr.h:439)
						int sceIoCancel( int fd ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 7A1F74B4 */
