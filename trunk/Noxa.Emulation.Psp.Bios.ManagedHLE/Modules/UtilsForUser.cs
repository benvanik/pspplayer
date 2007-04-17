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
	class UtilsForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "UtilsForUser";
			}
		}

		#endregion

		#region State Management

		public UtilsForUser( Kernel kernel )
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
		[BiosFunction( 0xBFA98062, "sceKernelDcacheInvalidateRange" )]
		// SDK location: /user/psputils.h:73
		// SDK declaration: void sceKernelDcacheInvalidateRange(const void *p, unsigned int size);
		public void sceKernelDcacheInvalidateRange( int p, int size ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8186A58, "sceKernelUtilsMd5Digest" )]
		// SDK location: /user/psputils.h:125
		// SDK declaration: int sceKernelUtilsMd5Digest(u8 *data, u32 size, u8 *digest);
		public int sceKernelUtilsMd5Digest( int data, int size, int digest ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E5C5086, "sceKernelUtilsMd5BlockInit" )]
		// SDK location: /user/psputils.h:142
		// SDK declaration: int sceKernelUtilsMd5BlockInit(SceKernelUtilsMd5Context *ctx);
		public int sceKernelUtilsMd5BlockInit( int ctx ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61E1E525, "sceKernelUtilsMd5BlockUpdate" )]
		// SDK location: /user/psputils.h:153
		// SDK declaration: int sceKernelUtilsMd5BlockUpdate(SceKernelUtilsMd5Context *ctx, u8 *data, u32 size);
		public int sceKernelUtilsMd5BlockUpdate( int ctx, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8D24E78, "sceKernelUtilsMd5BlockResult" )]
		// SDK location: /user/psputils.h:163
		// SDK declaration: int sceKernelUtilsMd5BlockResult(SceKernelUtilsMd5Context *ctx, u8 *digest);
		public int sceKernelUtilsMd5BlockResult( int ctx, int digest ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840259F1, "sceKernelUtilsSha1Digest" )]
		// SDK location: /user/psputils.h:183
		// SDK declaration: int sceKernelUtilsSha1Digest(u8 *data, u32 size, u8 *digest);
		public int sceKernelUtilsSha1Digest( int data, int size, int digest ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8FCD5BA, "sceKernelUtilsSha1BlockInit" )]
		// SDK location: /user/psputils.h:201
		// SDK declaration: int sceKernelUtilsSha1BlockInit(SceKernelUtilsSha1Context *ctx);
		public int sceKernelUtilsSha1BlockInit( int ctx ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x346F6DA8, "sceKernelUtilsSha1BlockUpdate" )]
		// SDK location: /user/psputils.h:212
		// SDK declaration: int sceKernelUtilsSha1BlockUpdate(SceKernelUtilsSha1Context *ctx, u8 *data, u32 size);
		public int sceKernelUtilsSha1BlockUpdate( int ctx, int data, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x585F1C09, "sceKernelUtilsSha1BlockResult" )]
		// SDK location: /user/psputils.h:222
		// SDK declaration: int sceKernelUtilsSha1BlockResult(SceKernelUtilsSha1Context *ctx, u8 *digest);
		public int sceKernelUtilsSha1BlockResult( int ctx, int digest ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE860E75E, "sceKernelUtilsMt19937Init" )]
		// SDK location: /user/psputils.h:96
		// SDK declaration: int sceKernelUtilsMt19937Init(SceKernelUtilsMt19937Context *ctx, u32 seed);
		public int sceKernelUtilsMt19937Init( int ctx, int seed ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06FB8A63, "sceKernelUtilsMt19937UInt" )]
		// SDK location: /user/psputils.h:104
		// SDK declaration: u32 sceKernelUtilsMt19937UInt(SceKernelUtilsMt19937Context *ctx);
		public int sceKernelUtilsMt19937UInt( int ctx ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91E4F6A7, "sceKernelLibcClock" )]
		// SDK location: /user/psputils.h:43
		// SDK declaration: clock_t sceKernelLibcClock();
		public int sceKernelLibcClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27CC57F0, "sceKernelLibcTime" )]
		// SDK location: /user/psputils.h:38
		// SDK declaration: time_t sceKernelLibcTime(time_t *t);
		public int sceKernelLibcTime( int t ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71EC4271, "sceKernelLibcGettimeofday" )]
		// SDK location: /user/psputils.h:48
		// SDK declaration: int sceKernelLibcGettimeofday(struct timeval *tp, struct timezone *tzp);
		public int sceKernelLibcGettimeofday( int tp, int tzp ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79D1C3FA, "sceKernelDcacheWritebackAll" )]
		// SDK location: /user/psputils.h:53
		// SDK declaration: void sceKernelDcacheWritebackAll();
		public void sceKernelDcacheWritebackAll(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB435DEC5, "sceKernelDcacheWritebackInvalidateAll" )]
		// SDK location: /user/psputils.h:58
		// SDK declaration: void sceKernelDcacheWritebackInvalidateAll();
		public void sceKernelDcacheWritebackInvalidateAll(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EE30821, "sceKernelDcacheWritebackRange" )]
		// SDK location: /user/psputils.h:63
		// SDK declaration: void sceKernelDcacheWritebackRange(const void *p, unsigned int size);
		public void sceKernelDcacheWritebackRange( int p, int size ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34B9FA9E, "sceKernelDcacheWritebackInvalidateRange" )]
		// SDK location: /user/psputils.h:68
		// SDK declaration: void sceKernelDcacheWritebackInvalidateRange(const void *p, unsigned int size);
		public void sceKernelDcacheWritebackInvalidateRange( int p, int size ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80001C4C, "sceKernelDcacheProbe" )]
		// SDK location: /kernel/psputilsforkernel.h:43
		// SDK declaration: int sceKernelDcacheProbe(void *addr);
		public int sceKernelDcacheProbe( int addr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FD31C9D, "sceKernelIcacheProbe" )]
		// SDK location: /kernel/psputilsforkernel.h:63
		// SDK declaration: int sceKernelIcacheProbe(const void *addr);
		public int sceKernelIcacheProbe( int addr ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F3D204AF */
