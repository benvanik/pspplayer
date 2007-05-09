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
	class IoFileMgrForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "IoFileMgrForKernel";
			}
		}

		#endregion

		#region State Management

		public IoFileMgrForKernel( Kernel kernel )
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
		int sceIoPollAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE23EEC33, "sceIoWaitAsync" )]
		int sceIoWaitAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35DBD746, "sceIoWaitAsyncCB" )]
		int sceIoWaitAsyncCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB05F8D6, "sceIoGetAsyncStat" )]
		int sceIoGetAsyncStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB293727F, "sceIoChangeAsyncPriority" )]
		int sceIoChangeAsyncPriority(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA12A0514, "sceIoSetAsyncCallback" )]
		int sceIoSetAsyncCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x810C4BC3, "sceIoClose" )]
		int sceIoClose(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF5940B6, "sceIoCloseAsync" )]
		int sceIoCloseAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA905B705, "sceIoCloseAll" )]
		int sceIoCloseAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x109F50BC, "sceIoOpen" )]
		int sceIoOpen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89AA9906, "sceIoOpenAsync" )]
		int sceIoOpenAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3C54E908, "sceIoReopen" )]
		int sceIoReopen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A638D83, "sceIoRead" )]
		int sceIoRead(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0B5A7C2, "sceIoReadAsync" )]
		int sceIoReadAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42EC03AC, "sceIoWrite" )]
		int sceIoWrite(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FACAB19, "sceIoWriteAsync" )]
		int sceIoWriteAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27EB27B8, "sceIoLseek" )]
		int sceIoLseek(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71B19E77, "sceIoLseekAsync" )]
		int sceIoLseekAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68963324, "sceIoLseek32" )]
		int sceIoLseek32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B385D8F, "sceIoLseek32Async" )]
		int sceIoLseek32Async(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63632449, "sceIoIoctl" )]
		int sceIoIoctl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE95A012B, "sceIoIoctlAsync" )]
		int sceIoIoctlAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB29DDF9C, "sceIoDopen" )]
		int sceIoDopen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3EB004C, "sceIoDread" )]
		int sceIoDread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB092469, "sceIoDclose" )]
		int sceIoDclose(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF27A9C51, "sceIoRemove" )]
		int sceIoRemove(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06A70004, "sceIoMkdir" )]
		int sceIoMkdir(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1117C65F, "sceIoRmdir" )]
		int sceIoRmdir(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55F4717D, "sceIoChdir" )]
		int sceIoChdir(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB96437F, "sceIoSync" )]
		int sceIoSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACE946E8, "sceIoGetstat" )]
		int sceIoGetstat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8A740F4, "sceIoChstat" )]
		int sceIoChstat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x779103A0, "sceIoRename" )]
		int sceIoRename(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54F5FB11, "sceIoDevctl" )]
		int sceIoDevctl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08BD7374, "sceIoGetDevType" )]
		int sceIoGetDevType(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2A628C1, "sceIoAssign" )]
		int sceIoAssign(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D08A871, "sceIoUnassign" )]
		int sceIoUnassign(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x411106BA, "sceIoGetThreadCwd" )]
		int sceIoGetThreadCwd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB0A151F, "sceIoChangeThreadCwd" )]
		int sceIoChangeThreadCwd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8BC6571, "sceIoCancel" )]
		int sceIoCancel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8E982A74, "sceIoAddDrv" )]
		int sceIoAddDrv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7F35804, "sceIoDelDrv" )]
		int sceIoDelDrv(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 3865EF70 */
