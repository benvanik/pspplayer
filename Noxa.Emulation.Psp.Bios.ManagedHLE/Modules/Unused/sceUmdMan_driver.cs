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
	class sceUmdMan_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUmdMan_driver";
			}
		}

		#endregion

		#region State Management

		public sceUmdMan_driver( Kernel kernel )
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
		[BiosFunction( 0xD27D050E, "sceUmdManInit" )]
		int sceUmdManInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D3EA203, "sceUmdManTerm" )]
		int sceUmdManTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ED141FE, "sceUmdManUMDDriveStart" )]
		int sceUmdManUMDDriveStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A302102, "sceUmdMan_driver_5A302102" )]
		int sceUmdMan_driver_5A302102(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4692D7F, "sceUmdMan_driver_B4692D7F" )]
		int sceUmdMan_driver_B4692D7F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40BEB9D1, "sceUmdManDVDDriveStart" )]
		int sceUmdManDVDDriveStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF407479, "sceUmdManDriveStop" )]
		int sceUmdManDriveStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8CFED611, "sceUmdManStart" )]
		int sceUmdManStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAD31025, "sceUmdManStop" )]
		int sceUmdManStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47E2B6D8, "sceUmdManGetUmdDrive" )]
		int sceUmdManGetUmdDrive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A77A311, "sceUmdManLPNCreateCmd" )]
		int sceUmdManLPNCreateCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB511F821, "sceUmdMan_driver_B511F821" )]
		int sceUmdMan_driver_B511F821(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x736AE133, "sceUmdMan_driver_736AE133" )]
		int sceUmdMan_driver_736AE133(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C026599, "sceUmdManLPNReadAlive" )]
		int sceUmdManLPNReadAlive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD48F9C2, "sceUmdManLPNSetVersion" )]
		int sceUmdManLPNSetVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3311B6E, "sceUmdManLPNGetVersion" )]
		int sceUmdManLPNGetVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1D7C860, "sceUmdMan_driver_C1D7C860" )]
		int sceUmdMan_driver_C1D7C860(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2368381, "sceUmdManCheckDeviceReady" )]
		int sceUmdManCheckDeviceReady(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2B0FB78, "sceUmdManGetMediaFormat" )]
		int sceUmdManGetMediaFormat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE779ECEF, "sceUmdMan_driver_E779ECEF" )]
		int sceUmdMan_driver_E779ECEF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x96527E8A, "sceUmdMan_driver_96527E8A" )]
		int sceUmdMan_driver_96527E8A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B1BF9FD, "sceUmdExecRead10Cmd" )]
		int sceUmdExecRead10Cmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18DE1880, "sceUmdExecPrefetch10Cmd" )]
		int sceUmdExecPrefetch10Cmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D44BABF, "sceUmdMan_driver_3D44BABF" )]
		int sceUmdMan_driver_3D44BABF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3F448E0, "sceUmdExecStartStopUnitCmd" )]
		int sceUmdExecStartStopUnitCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B19A313, "sceUmdExecInquiryCmd" )]
		int sceUmdExecInquiryCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CBE959B, "sceUmdMan_driver_2CBE959B" )]
		int sceUmdMan_driver_2CBE959B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A39569B, "sceUmdMan_driver_2A39569B" )]
		int sceUmdMan_driver_2A39569B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEE55E3E, "sceUmdMan_driver_CEE55E3E" )]
		int sceUmdMan_driver_CEE55E3E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE5B7EDC5, "sceUmdMan_driver_E5B7EDC5" )]
		int sceUmdMan_driver_E5B7EDC5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x65E1B97E, "sceUmdExecGetEventStatusCmd" )]
		int sceUmdExecGetEventStatusCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AA96415, "sceUmdExecReadCapacityCmd" )]
		int sceUmdExecReadCapacityCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x250E6975, "sceUmdExecSeekCmd" )]
		int sceUmdExecSeekCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A08FE9A, "sceUmdExecPreventAllowMediaCmd" )]
		int sceUmdExecPreventAllowMediaCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68577709, "sceUmdExecAllocateFromReadCmd" )]
		int sceUmdExecAllocateFromReadCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF819E17C, "sceUmdExecReadMKICmd" )]
		int sceUmdExecReadMKICmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61C32A52, "sceUmdExecSetAreaLimitCmd" )]
		int sceUmdExecSetAreaLimitCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7094E3A7, "sceUmdExecSetAccessLimitCmd" )]
		int sceUmdExecSetAccessLimitCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD31DAD7E, "sceUmdExecSetLockLengthCmd" )]
		int sceUmdExecSetLockLengthCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x108B2322, "sceUmdExecGetMediaInfoCmd" )]
		int sceUmdExecGetMediaInfoCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98345381, "sceUmdExecReportCacheCmd" )]
		int sceUmdExecReportCacheCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF88476F, "sceUmdMan_driver_BF88476F" )]
		int sceUmdMan_driver_BF88476F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x485D4925, "sceUmdExecSetStreamingCmd" )]
		int sceUmdExecSetStreamingCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73E49F8F, "sceUmdExecClearCacheInfoCmd" )]
		int sceUmdExecClearCacheInfoCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14D3381C, "sceUmdExecTestCmd" )]
		int sceUmdExecTestCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92F1CC33, "sceUmdExecAdjustDataCmd" )]
		int sceUmdExecAdjustDataCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61EB07A5, "sceUmdMan_driver_61EB07A5" )]
		int sceUmdMan_driver_61EB07A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xECA9476E, "sceUmdMan_driver_ECA9476E" )]
		int sceUmdMan_driver_ECA9476E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBEA40117, "sceUmdMan_driver_BEA40117" )]
		int sceUmdMan_driver_BEA40117(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDABDB450, "sceUmdManGpioSetup" )]
		int sceUmdManGpioSetup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA49E52E8, "sceUmdManGpioIntrStart" )]
		int sceUmdManGpioIntrStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC83EB3E2, "sceUmdManGpioIntrStop" )]
		int sceUmdManGpioIntrStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2DDE9F8, "sceUmdMan_driver_B2DDE9F8" )]
		int sceUmdMan_driver_B2DDE9F8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB989E127, "sceUmdManLeptonAliveOnOff" )]
		int sceUmdManLeptonAliveOnOff(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAF6D5838, "sceUmdManGetDriveMode" )]
		int sceUmdManGetDriveMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4581E306, "sceUmdManGetPowerStat" )]
		int sceUmdManGetPowerStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FBF6078, "sceUmdManGetPower" )]
		int sceUmdManGetPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93539196, "sceUmdMan_driver_93539196" )]
		int sceUmdMan_driver_93539196(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8D45A7B, "sceUmdMan_driver_C8D45A7B" )]
		int sceUmdMan_driver_C8D45A7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA68F200, "sceUmdManChangePowerMode" )]
		int sceUmdManChangePowerMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4AFF62D, "sceUmdMan_driver_F4AFF62D" )]
		int sceUmdMan_driver_F4AFF62D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84410A8E, "sceUmdMan_driver_84410A8E" )]
		int sceUmdMan_driver_84410A8E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39704B6E, "sceUmdMan_driver_39704B6E" )]
		int sceUmdMan_driver_39704B6E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63ACFD28, "sceUmdMan_driver_63ACFD28" )]
		int sceUmdMan_driver_63ACFD28(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEA5C857, "sceUmdMan_driver_CEA5C857" )]
		int sceUmdMan_driver_CEA5C857(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8634FFC7, "sceUmdMan_driver_8634FFC7" )]
		int sceUmdMan_driver_8634FFC7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14AAB3FC, "sceUmdManSyncState" )]
		int sceUmdManSyncState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC634697D, "sceUmdMan_driver_C634697D" )]
		int sceUmdMan_driver_C634697D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E2A680C, "sceUmdMan_driver_7E2A680C" )]
		int sceUmdMan_driver_7E2A680C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1CB85F05, "sceUmdMan_driver_1CB85F05" )]
		int sceUmdMan_driver_1CB85F05(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43255AD6, "sceUmdMan_driver_43255AD6" )]
		int sceUmdMan_driver_43255AD6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4A99D78, "sceUmdManAbort" )]
		int sceUmdManAbort(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5CFC459, "sceUmdManReset" )]
		int sceUmdManReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE192C10A, "sceUmdManGetUmdDiscInfo" )]
		int sceUmdManGetUmdDiscInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8609D1E4, "sceUmdManGetDiscInfo" )]
		int sceUmdManGetDiscInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAAA4ED91, "sceUmdManGetDiscInfo4VSH" )]
		int sceUmdManGetDiscInfo4VSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x208089C2, "sceUmdManPowerOnOff" )]
		int sceUmdManPowerOnOff(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66F94A52, "sceUmdMan_driver_66F94A52" )]
		int sceUmdMan_driver_66F94A52(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x815AA6D3, "sceUmdMan_driver_815AA6D3" )]
		int sceUmdMan_driver_815AA6D3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6BE91FC, "sceUmdMan_driver_F6BE91FC" )]
		int sceUmdMan_driver_F6BE91FC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCFEF5FE, "sceUmdMan_driver_FCFEF5FE" )]
		int sceUmdMan_driver_FCFEF5FE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x77E6C03A, "sceUmdMan_driver_77E6C03A" )]
		int sceUmdMan_driver_77E6C03A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA869CAB3, "sceUmdMan_driver_A869CAB3" )]
		int sceUmdMan_driver_A869CAB3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A437AD7, "sceUmdManRetryAdjust" )]
		int sceUmdManRetryAdjust(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7C603A2, "sceUmdMan_driver_F7C603A2" )]
		int sceUmdMan_driver_F7C603A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4EA8EA5D, "sceUmdManSPKInitConfiguration" )]
		int sceUmdManSPKInitConfiguration(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EBAEA9F, "sceUmdManSPKStartStep0" )]
		int sceUmdManSPKStartStep0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC984E1CF, "sceUmdManSPKStartStep1" )]
		int sceUmdManSPKStartStep1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0B257A7, "sceUmdManSPKStartStep2" )]
		int sceUmdManSPKStartStep2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C6BF421, "sceUmdMan_driver_4C6BF421" )]
		int sceUmdMan_driver_4C6BF421(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E7FC8F0, "sceUmdManSPKStartStep4" )]
		int sceUmdManSPKStartStep4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66CB0CC4, "sceUmdManSPKStartStep5" )]
		int sceUmdManSPKStartStep5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8359E04, "sceUmdManSPKSetupCmd" )]
		int sceUmdManSPKSetupCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73966DE9, "sceUmdManSPKStartCmd" )]
		int sceUmdManSPKStartCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD1A113A, "sceUmdManSPKStopCmd" )]
		int sceUmdManSPKStopCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23C3C3D6, "sceUmdManSPKEndCmd" )]
		int sceUmdManSPKEndCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C3D307C, "sceUmdManSPKGetMKI" )]
		int sceUmdManSPKGetMKI(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58E3718D, "sceUmdManSPKEnableIntr" )]
		int sceUmdManSPKEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08EB09C8, "sceUmdManSPKDisableIntr" )]
		int sceUmdManSPKDisableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5304CA4A, "sceUmdMan_driver_5304CA4A" )]
		int sceUmdMan_driver_5304CA4A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CB4A8D6, "sceUmdMan_driver_6CB4A8D6" )]
		int sceUmdMan_driver_6CB4A8D6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA58946CA, "sceUmdMan_driver_A58946CA" )]
		int sceUmdMan_driver_A58946CA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1D4F296, "sceUmdMan_driver_D1D4F296" )]
		int sceUmdMan_driver_D1D4F296(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0BDD5754, "sceUmdMan_driver_0BDD5754" )]
		int sceUmdMan_driver_0BDD5754(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DF2A18D, "sceUmdManSetAlarm" )]
		int sceUmdManSetAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB0A6DC55, "sceUmdManCancelAlarm" )]
		int sceUmdManCancelAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x51A9AC49, "sceUmdMan_driver_51A9AC49" )]
		int sceUmdMan_driver_51A9AC49(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43AA300A, "sceUmdMan_driver_43AA300A" )]
		int sceUmdMan_driver_43AA300A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x10089E13, "sceUmdMan_driver_10089E13" )]
		int sceUmdMan_driver_10089E13(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94A857C1, "sceUmdMan_driver_94A857C1" )]
		int sceUmdMan_driver_94A857C1(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 45B4572B */
