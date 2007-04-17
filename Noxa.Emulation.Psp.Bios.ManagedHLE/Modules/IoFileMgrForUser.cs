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
	class IoFileMgrForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "IoFileMgrForUser";
			}
		}

		#endregion

		#region State Management

		public IoFileMgrForUser( Kernel kernel )
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
		[BiosFunction( 0x3251EA56, "sceIoPollAsync" )]
		// SDK location: /user/pspiofilemgr.h:419
		// SDK declaration: int sceIoPollAsync(SceUID fd, SceInt64 *res);
		public int sceIoPollAsync( int fd, long res ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE23EEC33, "sceIoWaitAsync" )]
		// SDK location: /user/pspiofilemgr.h:399
		// SDK declaration: int sceIoWaitAsync(SceUID fd, SceInt64 *res);
		public int sceIoWaitAsync( int fd, long res ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35DBD746, "sceIoWaitAsyncCB" )]
		// SDK location: /user/pspiofilemgr.h:409
		// SDK declaration: int sceIoWaitAsyncCB(SceUID fd, SceInt64 *res);
		public int sceIoWaitAsyncCB( int fd, long res ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB05F8D6, "sceIoGetAsyncStat" )]
		// SDK location: /user/pspiofilemgr.h:430
		// SDK declaration: int sceIoGetAsyncStat(SceUID fd, int poll, SceInt64 *res);
		public int sceIoGetAsyncStat( int fd, int poll, long res ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB293727F, "sceIoChangeAsyncPriority" )]
		// SDK location: /user/pspiofilemgr.h:458
		// SDK declaration: int sceIoChangeAsyncPriority(SceUID fd, int pri);
		public int sceIoChangeAsyncPriority( int fd, int pri ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA12A0514, "sceIoSetAsyncCallback" )]
		// SDK location: /user/pspiofilemgr.h:469
		// SDK declaration: int sceIoSetAsyncCallback(SceUID fd, SceUID cb, void *argp);
		public int sceIoSetAsyncCallback( int fd, int cb, int argp ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x810C4BC3, "sceIoClose" )]
		// SDK location: /user/pspiofilemgr.h:85
		// SDK declaration: int sceIoClose(SceUID fd);
		public int sceIoClose( int fd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF5940B6, "sceIoCloseAsync" )]
		// SDK location: /user/pspiofilemgr.h:93
		// SDK declaration: int sceIoCloseAsync(SceUID fd);
		public int sceIoCloseAsync( int fd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x109F50BC, "sceIoOpen" )]
		// SDK location: /user/pspiofilemgr.h:63
		// SDK declaration: SceUID sceIoOpen(const char *file, int flags, SceMode mode);
		public int sceIoOpen( int file, int flags, int mode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89AA9906, "sceIoOpenAsync" )]
		// SDK location: /user/pspiofilemgr.h:73
		// SDK declaration: SceUID sceIoOpenAsync(const char *file, int flags, SceMode mode);
		public int sceIoOpenAsync( int file, int flags, int mode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A638D83, "sceIoRead" )]
		// SDK location: /user/pspiofilemgr.h:109
		// SDK declaration: int sceIoRead(SceUID fd, void *data, SceSize size);
		public int sceIoRead( int fd, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0B5A7C2, "sceIoReadAsync" )]
		// SDK location: /user/pspiofilemgr.h:125
		// SDK declaration: int sceIoReadAsync(SceUID fd, void *data, SceSize size);
		public int sceIoReadAsync( int fd, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42EC03AC, "sceIoWrite" )]
		// SDK location: /user/pspiofilemgr.h:141
		// SDK declaration: int sceIoWrite(SceUID fd, const void *data, SceSize size);
		public int sceIoWrite( int fd, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FACAB19, "sceIoWriteAsync" )]
		// SDK location: /user/pspiofilemgr.h:152
		// SDK declaration: int sceIoWriteAsync(SceUID fd, const void *data, SceSize size);
		public int sceIoWriteAsync( int fd, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27EB27B8, "sceIoLseek" )]
		// SDK location: /user/pspiofilemgr.h:169
		// SDK declaration: SceOff sceIoLseek(SceUID fd, SceOff offset, int whence);
		public long sceIoLseek( int fd, long offset, int whence ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71B19E77, "sceIoLseekAsync" )]
		// SDK location: /user/pspiofilemgr.h:181
		// SDK declaration: int sceIoLseekAsync(SceUID fd, SceOff offset, int whence);
		public int sceIoLseekAsync( int fd, long offset, int whence ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68963324, "sceIoLseek32" )]
		// SDK location: /user/pspiofilemgr.h:198
		// SDK declaration: int sceIoLseek32(SceUID fd, int offset, int whence);
		public int sceIoLseek32( int fd, int offset, int whence ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B385D8F, "sceIoLseek32Async" )]
		// SDK location: /user/pspiofilemgr.h:210
		// SDK declaration: int sceIoLseek32Async(SceUID fd, int offset, int whence);
		public int sceIoLseek32Async( int fd, int offset, int whence ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63632449, "sceIoIoctl" )]
		// SDK location: /user/pspiofilemgr.h:368
		// SDK declaration: int sceIoIoctl(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoIoctl( int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE95A012B, "sceIoIoctlAsync" )]
		// SDK location: /user/pspiofilemgr.h:381
		// SDK declaration: int sceIoIoctlAsync(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoIoctlAsync( int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB29DDF9C, "sceIoDopen" )]
		// SDK location: /user/pspiofilemgr.h:267
		// SDK declaration: SceUID sceIoDopen(const char *dirname);
		public int sceIoDopen( int dirname ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3EB004C, "sceIoDread" )]
		// SDK location: /user/pspiofilemgr.h:280
		// SDK declaration: int sceIoDread(SceUID fd, SceIoDirent *dir);
		public int sceIoDread( int fd, int dir ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB092469, "sceIoDclose" )]
		// SDK location: /user/pspiofilemgr.h:288
		// SDK declaration: int sceIoDclose(SceUID fd);
		public int sceIoDclose( int fd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF27A9C51, "sceIoRemove" )]
		// SDK location: /user/pspiofilemgr.h:218
		// SDK declaration: int sceIoRemove(const char *file);
		public int sceIoRemove( int file ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06A70004, "sceIoMkdir" )]
		// SDK location: /user/pspiofilemgr.h:227
		// SDK declaration: int sceIoMkdir(const char *dir, SceMode mode);
		public int sceIoMkdir( int dir, int mode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1117C65F, "sceIoRmdir" )]
		// SDK location: /user/pspiofilemgr.h:235
		// SDK declaration: int sceIoRmdir(const char *path);
		public int sceIoRmdir( int path ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55F4717D, "sceIoChdir" )]
		// SDK location: /user/pspiofilemgr.h:243
		// SDK declaration: int sceIoChdir(const char *path);
		public int sceIoChdir( int path ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB96437F, "sceIoSync" )]
		// SDK location: /user/pspiofilemgr.h:389
		// SDK declaration: int sceIoSync(const char *device, unsigned int unk);
		public int sceIoSync( int device, int unk ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACE946E8, "sceIoGetstat" )]
		// SDK location: /user/pspiofilemgr.h:344
		// SDK declaration: int sceIoGetstat(const char *file, SceIoStat *stat);
		public int sceIoGetstat( int file, int stat ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8A740F4, "sceIoChstat" )]
		// SDK location: /user/pspiofilemgr.h:355
		// SDK declaration: int sceIoChstat(const char *file, SceIoStat *stat, int bits);
		public int sceIoChstat( int file, int stat, int bits ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x779103A0, "sceIoRename" )]
		// SDK location: /user/pspiofilemgr.h:252
		// SDK declaration: int sceIoRename(const char *oldname, const char *newname);
		public int sceIoRename( int oldname, int newname ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54F5FB11, "sceIoDevctl" )]
		// SDK location: /user/pspiofilemgr.h:306
		// SDK declaration: int sceIoDevctl(const char *dev, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoDevctl( int dev, int cmd, int indata, int inlen, int outdata, int outlen ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08BD7374, "sceIoGetDevType" )]
		// SDK location: /user/pspiofilemgr.h:448
		// SDK declaration: int sceIoGetDevType(SceUID fd);
		public int sceIoGetDevType( int fd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2A628C1, "sceIoAssign" )]
		// SDK location: /user/pspiofilemgr.h:325
		// SDK declaration: int sceIoAssign(const char *dev1, const char *dev2, const char *dev3, int mode, void* unk1, long unk2);
		public int sceIoAssign( int dev1, int dev2, int dev3, int mode, int unk1, int unk2 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D08A871, "sceIoUnassign" )]
		// SDK location: /user/pspiofilemgr.h:334
		// SDK declaration: int sceIoUnassign(const char *dev);
		public int sceIoUnassign( int dev ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8BC6571, "sceIoCancel" )]
		// SDK location: /user/pspiofilemgr.h:439
		// SDK declaration: int sceIoCancel(SceUID fd);
		public int sceIoCancel( int fd ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - A3F18B53 */
