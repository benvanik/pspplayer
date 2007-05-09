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
	class UtilsForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "UtilsForKernel";
			}
		}

		#endregion

		#region State Management

		public UtilsForKernel( Kernel kernel )
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
		[BiosFunction( 0x80FE032E, "sceUtilsKernelDcacheWBinvRange" )]
		int sceUtilsKernelDcacheWBinvRange(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8186A58, "sceKernelUtilsMd5Digest" )]
		int sceKernelUtilsMd5Digest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E5C5086, "sceKernelUtilsMd5BlockInit" )]
		int sceKernelUtilsMd5BlockInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61E1E525, "sceKernelUtilsMd5BlockUpdate" )]
		int sceKernelUtilsMd5BlockUpdate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8D24E78, "sceKernelUtilsMd5BlockResult" )]
		int sceKernelUtilsMd5BlockResult(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840259F1, "sceKernelUtilsSha1Digest" )]
		int sceKernelUtilsSha1Digest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8FCD5BA, "sceKernelUtilsSha1BlockInit" )]
		int sceKernelUtilsSha1BlockInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x346F6DA8, "sceKernelUtilsSha1BlockUpdate" )]
		int sceKernelUtilsSha1BlockUpdate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x585F1C09, "sceKernelUtilsSha1BlockResult" )]
		int sceKernelUtilsSha1BlockResult(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE860E75E, "sceKernelUtilsMt19937Init" )]
		int sceKernelUtilsMt19937Init(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06FB8A63, "sceKernelUtilsMt19937UInt" )]
		int sceKernelUtilsMt19937UInt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x193D4036, "sceKernelSetGPIMask" )]
		int sceKernelSetGPIMask(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95035FEF, "sceKernelSetGPOMask" )]
		int sceKernelSetGPOMask(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37FB5C42, "sceKernelGetGPI" )]
		int sceKernelGetGPI(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6AD345D7, "sceKernelSetGPO" )]
		int sceKernelSetGPO(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B7ED3FD, "sceKernelRegisterLibcRtcFunc" )]
		int sceKernelRegisterLibcRtcFunc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6151A7C3, "sceKernelReleaseLibcRtcFunc" )]
		int sceKernelReleaseLibcRtcFunc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91E4F6A7, "sceKernelLibcClock" )]
		int sceKernelLibcClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27CC57F0, "sceKernelLibcTime" )]
		int sceKernelLibcTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71EC4271, "sceKernelLibcGettimeofday" )]
		int sceKernelLibcGettimeofday(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79D1C3FA, "sceKernelDcacheWritebackAll" )]
		int sceKernelDcacheWritebackAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB435DEC5, "sceKernelDcacheWritebackInvalidateAll" )]
		int sceKernelDcacheWritebackInvalidateAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x864A9D72, "sceKernelDcacheInvalidateAll" )]
		int sceKernelDcacheInvalidateAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EE30821, "sceKernelDcacheWritebackRange" )]
		int sceKernelDcacheWritebackRange(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34B9FA9E, "sceKernelDcacheWritebackInvalidateRange" )]
		int sceKernelDcacheWritebackInvalidateRange(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFA98062, "sceKernelDcacheInvalidateRange" )]
		int sceKernelDcacheInvalidateRange(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80001C4C, "sceKernelDcacheProbe" )]
		int sceKernelDcacheProbe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16641D70, "sceKernelDcacheReadTag" )]
		int sceKernelDcacheReadTag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x920F104A, "sceKernelIcacheInvalidateAll" )]
		int sceKernelIcacheInvalidateAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2DF770E, "sceKernelIcacheInvalidateRange" )]
		int sceKernelIcacheInvalidateRange(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FD31C9D, "sceKernelIcacheProbe" )]
		int sceKernelIcacheProbe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB05FAD0, "sceKernelIcacheReadTag" )]
		int sceKernelIcacheReadTag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78934841, "sceKernelGzipDecompress" )]
		int sceKernelGzipDecompress(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0CE3E29, "sceKernelGzipIsValid" )]
		int sceKernelGzipIsValid(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB0E9C31F, "sceKernelGzipGetInfo" )]
		int sceKernelGzipGetInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0E6BA96, "sceKernelGzipGetName" )]
		int sceKernelGzipGetName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C1FBE04, "sceKernelGzipGetComment" )]
		int sceKernelGzipGetComment(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23FFC828, "sceKernelGzipGetCompressedData" )]
		int sceKernelGzipGetCompressedData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8DB3CE6, "sceKernelDeflateDecompress" )]
		int sceKernelDeflateDecompress(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 8A559166 */
