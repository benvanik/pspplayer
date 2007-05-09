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
	class scePower_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePower_driver";
			}
		}

		#endregion

		#region State Management

		public scePower_driver( Kernel kernel )
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
		[BiosFunction( 0x9CE06934, "scePowerInit" )]
		int scePowerInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD5BB433, "scePowerEnd" )]
		int scePowerEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D2CA84B, "scePowerWlanActivate" )]
		int scePowerWlanActivate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23BB0A60, "scePowerWlanDeactivate" )]
		int scePowerWlanDeactivate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B51FE2F, "scePower_driver_2B51FE2F" )]
		int scePower_driver_2B51FE2F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x442BFBAC, "scePower_driver_442BFBAC" )]
		int scePower_driver_442BFBAC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8685403, "scePower_driver_E8685403" )]
		int scePower_driver_E8685403(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEFD3C963, "scePowerTick" )]
		int scePowerTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDC13FE5, "scePowerGetIdleTimer" )]
		int scePowerGetIdleTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1BA2FCAE, "scePowerSetIdleCallback" )]
		int scePowerSetIdleCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F30B3B1, "scePowerIdleTimerEnable" )]
		int scePowerIdleTimerEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x972CE941, "scePowerIdleTimerDisable" )]
		int scePowerIdleTimerDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27F3292C, "scePowerBatteryUpdateInfo" )]
		int scePowerBatteryUpdateInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8E4E204, "scePower_driver_E8E4E204" )]
		int scePower_driver_E8E4E204(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB999184C, "scePowerGetLowBatteryCapacity" )]
		int scePowerGetLowBatteryCapacity(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x166922EC, "scePower_driver_166922EC" )]
		int scePower_driver_166922EC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDD3D4DAC, "scePower_driver_DD3D4DAC" )]
		int scePower_driver_DD3D4DAC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87440F5E, "scePowerIsPowerOnline" )]
		int scePowerIsPowerOnline(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AFD0D8B, "scePowerIsBatteryExist" )]
		int scePowerIsBatteryExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E490401, "scePowerIsBatteryCharging" )]
		int scePowerIsBatteryCharging(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4432BC8, "scePowerGetBatteryChargingStatus" )]
		int scePowerGetBatteryChargingStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3075926, "scePowerIsLowBattery" )]
		int scePowerIsLowBattery(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78A1A796, "scePower_driver_78A1A796" )]
		int scePower_driver_78A1A796(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94F5A53F, "scePowerGetBatteryRemainCapacity" )]
		int scePowerGetBatteryRemainCapacity(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD18A0FF, "scePower_driver_FD18A0FF" )]
		int scePower_driver_FD18A0FF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2085D15D, "scePowerGetBatteryLifePercent" )]
		int scePowerGetBatteryLifePercent(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EFB3FA2, "scePowerGetBatteryLifeTime" )]
		int scePowerGetBatteryLifeTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28E12023, "scePowerGetBatteryTemp" )]
		int scePowerGetBatteryTemp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x862AE1A6, "scePowerGetBatteryElec" )]
		int scePowerGetBatteryElec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x483CE86B, "scePowerGetBatteryVolt" )]
		int scePowerGetBatteryVolt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23436A4A, "scePower_driver_23436A4A" )]
		int scePower_driver_23436A4A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0CD21B1F, "scePower_driver_0CD21B1F" )]
		int scePower_driver_0CD21B1F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x165CE085, "scePower_driver_165CE085" )]
		int scePower_driver_165CE085(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6D016EF, "scePowerLock" )]
		int scePowerLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA3D34C1, "scePowerUnlock" )]
		int scePowerUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79DB9421, "scePowerRebootStart" )]
		int scePowerRebootStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB62C9CF, "scePowerCancelRequest" )]
		int scePowerCancelRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7FA406DD, "scePowerIsRequest" )]
		int scePowerIsRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B7C7CF4, "scePowerRequestStandby" )]
		int scePowerRequestStandby(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC32C9CC, "scePowerRequestSuspend" )]
		int scePowerRequestSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2875994B, "scePower_driver_2875994B" )]
		int scePower_driver_2875994B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3951AF53, "scePowerEncodeUBattery" )]
		int scePowerEncodeUBattery(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0074EF9B, "scePowerGetResumeCount" )]
		int scePowerGetResumeCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF535D928, "scePower_driver_F535D928" )]
		int scePower_driver_F535D928(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04B7766E, "scePowerRegisterCallback" )]
		int scePowerRegisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFA8BAF8, "scePowerUnregisterCallback" )]
		int scePowerUnregisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB9D28DD, "scePowerUnregitserCallback" )]
		int scePowerUnregitserCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD24E6BEB, "scePower_driver_D24E6BEB" )]
		int scePower_driver_D24E6BEB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35B7662E, "scePowerGetSectionDescriptionEntry" )]
		int scePowerGetSectionDescriptionEntry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9B4DEA1, "scePowerLimitPllClock" )]
		int scePowerLimitPllClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x843FBF43, "scePowerSetCpuClockFrequency" )]
		int scePowerSetCpuClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8D7B3FB, "scePowerSetBusClockFrequency" )]
		int scePowerSetBusClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEE03A2F, "scePowerGetCpuClockFrequency" )]
		int scePowerGetCpuClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x478FE6F5, "scePowerGetBusClockFrequency" )]
		int scePowerGetBusClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDB5BFE9, "scePowerGetCpuClockFrequencyInt" )]
		int scePowerGetCpuClockFrequencyInt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD681969, "scePowerGetBusClockFrequencyInt" )]
		int scePowerGetBusClockFrequencyInt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1A52C83, "scePowerGetCpuClockFrequencyFloat" )]
		int scePowerGetCpuClockFrequencyFloat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9BADB3EB, "scePowerGetBusClockFrequencyFloat" )]
		int scePowerGetBusClockFrequencyFloat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x737486F2, "scePowerSetClockFrequency" )]
		int scePowerSetClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0B7A95D, "scePower_driver_E0B7A95D" )]
		int scePower_driver_E0B7A95D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC23AC778, "scePower_driver_C23AC778" )]
		int scePower_driver_C23AC778(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x23C31FFE, "scePower_driver_23C31FFE" )]
		int scePower_driver_23C31FFE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA97A599, "scePower_driver_FA97A599" )]
		int scePower_driver_FA97A599(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3EDD801, "scePower_driver_B3EDD801" )]
		int scePower_driver_B3EDD801(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C7A4FBE5 */
