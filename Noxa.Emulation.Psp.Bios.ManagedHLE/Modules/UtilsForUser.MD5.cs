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
	partial class UtilsForUser
	{
		/*
		 *
		typedef struct _SceKernelUtilsMd5Context {
				unsigned int 	h[4];
				unsigned int 	pad;
				short unsigned int 	usRemains;
				short unsigned int 	usComputed;
				long long unsigned int 	ullTotalLen;
				unsigned char 	buf[64];
		} SceKernelUtilsMd5Context;
		*/
		//http://svn.ps2dev.org/filedetails.php?repname=psp&path=%2Ftrunk%2Fpspsdk%2Fsrc%2Fuser%2Fpsputils.h&rev=1721&sc=1

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8186A58, "sceKernelUtilsMd5Digest" )]
		// SDK location: /user/psputils.h:125
		// SDK declaration: int sceKernelUtilsMd5Digest(u8 *data, u32 size, u8 *digest);
		public int sceKernelUtilsMd5Digest( int data, int size, int digest )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E5C5086, "sceKernelUtilsMd5BlockInit" )]
		// SDK location: /user/psputils.h:142
		// SDK declaration: int sceKernelUtilsMd5BlockInit(SceKernelUtilsMd5Context *ctx);
		public int sceKernelUtilsMd5BlockInit( int ctx )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61E1E525, "sceKernelUtilsMd5BlockUpdate" )]
		// SDK location: /user/psputils.h:153
		// SDK declaration: int sceKernelUtilsMd5BlockUpdate(SceKernelUtilsMd5Context *ctx, u8 *data, u32 size);
		public int sceKernelUtilsMd5BlockUpdate( int ctx, int data, int size )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8D24E78, "sceKernelUtilsMd5BlockResult" )]
		// SDK location: /user/psputils.h:163
		// SDK declaration: int sceKernelUtilsMd5BlockResult(SceKernelUtilsMd5Context *ctx, u8 *digest);
		public int sceKernelUtilsMd5BlockResult( int ctx, int digest )
		{
			return Module.NotImplementedReturn;
		}
	}
}
