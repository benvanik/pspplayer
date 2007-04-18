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
		typedef struct _SceKernelUtilsSha1Context {
			unsigned int 	h[5];
			short unsigned int 	usRemains;
			short unsigned int 	usComputed;
			long long unsigned int 	ullTotalLen;
			unsigned char 	buf[64];
		} SceKernelUtilsSha1Context;
		 */

		//http://svn.ps2dev.org/filedetails.php?repname=psp&path=%2Ftrunk%2Fpspsdk%2Fsrc%2Fuser%2Fpsputils.h&rev=1721&sc=1

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840259F1, "sceKernelUtilsSha1Digest" )]
		// SDK location: /user/psputils.h:183
		// SDK declaration: int sceKernelUtilsSha1Digest(u8 *data, u32 size, u8 *digest);
		public int sceKernelUtilsSha1Digest( int data, int size, int digest )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8FCD5BA, "sceKernelUtilsSha1BlockInit" )]
		// SDK location: /user/psputils.h:201
		// SDK declaration: int sceKernelUtilsSha1BlockInit(SceKernelUtilsSha1Context *ctx);
		public int sceKernelUtilsSha1BlockInit( int ctx )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x346F6DA8, "sceKernelUtilsSha1BlockUpdate" )]
		// SDK location: /user/psputils.h:212
		// SDK declaration: int sceKernelUtilsSha1BlockUpdate(SceKernelUtilsSha1Context *ctx, u8 *data, u32 size);
		public int sceKernelUtilsSha1BlockUpdate( int ctx, int data, int size )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x585F1C09, "sceKernelUtilsSha1BlockResult" )]
		// SDK location: /user/psputils.h:222
		// SDK declaration: int sceKernelUtilsSha1BlockResult(SceKernelUtilsSha1Context *ctx, u8 *digest);
		public int sceKernelUtilsSha1BlockResult( int ctx, int digest )
		{
			return Module.NotImplementedReturn;
		}
	}
}
