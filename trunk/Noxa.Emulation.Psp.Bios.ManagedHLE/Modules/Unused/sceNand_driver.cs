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
	class sceNand_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNand_driver";
			}
		}

		#endregion

		#region State Management

		public sceNand_driver( Kernel kernel )
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
		[BiosFunction( 0xA513BB12, "sceNandInit" )]
		int sceNandInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD305870E, "sceNandEnd" )]
		int sceNandEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73A68408, "sceNandSuspend" )]
		int sceNandSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F9BBBBD, "sceNandResume" )]
		int sceNandResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84EE5D76, "sceNandSetWriteProtect" )]
		int sceNandSetWriteProtect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE4438C7, "sceNandLock" )]
		int sceNandLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41FFA822, "sceNandUnlock" )]
		int sceNandUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE41A11DE, "sceNandReadStatus" )]
		int sceNandReadStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AF7B77A, "sceNandReset" )]
		int sceNandReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCDF7610, "sceNandReadId" )]
		int sceNandReadId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89BDCA08, "sceNandReadPages" )]
		int sceNandReadPages(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8AF0AB9F, "sceNandWritePages" )]
		int sceNandWritePages(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE05AE88D, "sceNand_driver_E05AE88D" )]
		int sceNand_driver_E05AE88D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8932166A, "sceNand_driver_8932166A" )]
		int sceNand_driver_8932166A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC478C1DE, "sceNand_driver_C478C1DE" )]
		int sceNand_driver_C478C1DE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBADD5D46, "sceNand_driver_BADD5D46" )]
		int sceNand_driver_BADD5D46(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x766756EF, "sceNandReadAccess" )]
		int sceNandReadAccess(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0ADC8686, "sceNandWriteAccess" )]
		int sceNandWriteAccess(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB0A0022, "sceNandEraseBlock" )]
		int sceNandEraseBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5182C394, "sceNandReadExtraOnly" )]
		int sceNandReadExtraOnly(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF55F193, "sceNandCalcEcc" )]
		int sceNandCalcEcc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18B78661, "sceNandVerifyEcc" )]
		int sceNandVerifyEcc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB795D2ED, "sceNandCollectEcc" )]
		int sceNandCollectEcc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD897C343, "sceNandDetectChip" )]
		int sceNandDetectChip(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE9843E6, "sceNandGetPageSize" )]
		int sceNandGetPageSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB07C41D4, "sceNandGetPagesPerBlock" )]
		int sceNandGetPagesPerBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1376222, "sceNandGetTotalBlocks" )]
		int sceNandGetTotalBlocks(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x716CD2B2, "sceNandWriteBlock" )]
		int sceNandWriteBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2B021E5, "sceNandWriteBlockWithVerify" )]
		int sceNandWriteBlockWithVerify(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC32EA051, "sceNandReadBlockWithRetry" )]
		int sceNandReadBlockWithRetry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AC02755, "sceNandVerifyBlockWithRetry" )]
		int sceNandVerifyBlockWithRetry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8933B2E0, "sceNandEraseBlockWithRetry" )]
		int sceNandEraseBlockWithRetry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01F09203, "sceNandIsBadBlock" )]
		int sceNandIsBadBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC29DA136, "sceNand_driver_C29DA136" )]
		int sceNand_driver_C29DA136(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F76BC21, "sceNand_driver_3F76BC21" )]
		int sceNand_driver_3F76BC21(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEBA0E6C6, "sceNand_driver_EBA0E6C6" )]
		int sceNand_driver_EBA0E6C6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FF6081B, "sceNandDetectChipMakersBBM" )]
		int sceNandDetectChipMakersBBM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2674CFFE, "sceNandEraseAllBlock" )]
		int sceNandEraseAllBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B2AC433, "sceNandTestBlock" )]
		int sceNandTestBlock(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 6C1DB928 */
