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
	partial class IoFileMgrForUser
	{
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63632449, "sceIoIoctl" )]
		// SDK location: /user/pspiofilemgr.h:368
		// SDK declaration: int sceIoIoctl(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoIoctl( int fd, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE95A012B, "sceIoIoctlAsync" )]
		// SDK location: /user/pspiofilemgr.h:381
		// SDK declaration: int sceIoIoctlAsync(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoIoctlAsync( int fd, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54F5FB11, "sceIoDevctl" )]
		// SDK location: /user/pspiofilemgr.h:306
		// SDK declaration: int sceIoDevctl(const char *dev, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoDevctl( int dev, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08BD7374, "sceIoGetDevType" )]
		// SDK location: /user/pspiofilemgr.h:448
		// SDK declaration: int sceIoGetDevType(SceUID fd);
		public int sceIoGetDevType( int fd )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2A628C1, "sceIoAssign" )]
		// SDK location: /user/pspiofilemgr.h:325
		// SDK declaration: int sceIoAssign(const char *dev1, const char *dev2, const char *dev3, int mode, void* unk1, long unk2);
		public int sceIoAssign( int dev1, int dev2, int dev3, int mode, int unk1, int unk2 )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D08A871, "sceIoUnassign" )]
		// SDK location: /user/pspiofilemgr.h:334
		// SDK declaration: int sceIoUnassign(const char *dev);
		public int sceIoUnassign( int dev )
		{
			return Module.NotImplementedReturn;
		}
	}
}
