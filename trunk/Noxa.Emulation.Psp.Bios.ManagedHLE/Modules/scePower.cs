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
	class scePower : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePower";
			}
		}

		#endregion

		#region State Management

		public scePower( Kernel kernel )
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
		[BiosFunction( 0xEFD3C963, "scePowerTick" )]
		// SDK location: /power/psppower.h:200
		// SDK declaration: int scePowerTick(int unknown);
		int scePowerTick( int unknown ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDC13FE5, "scePowerGetIdleTimer" )]
		// SDK location: /power/psppower.h:206
		// SDK declaration: int scePowerGetIdleTimer();
		int scePowerGetIdleTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F30B3B1, "scePowerIdleTimerEnable" )]
		// SDK location: /power/psppower.h:213
		// SDK declaration: int scePowerIdleTimerEnable(int unknown);
		int scePowerIdleTimerEnable( int unknown ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x972CE941, "scePowerIdleTimerDisable" )]
		// SDK location: /power/psppower.h:220
		// SDK declaration: int scePowerIdleTimerDisable(int unknown);
		int scePowerIdleTimerDisable( int unknown ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87440F5E, "scePowerIsPowerOnline" )]
		// SDK location: /power/psppower.h:66
		// SDK declaration: int scePowerIsPowerOnline();
		int scePowerIsPowerOnline(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AFD0D8B, "scePowerIsBatteryExist" )]
		// SDK location: /power/psppower.h:71
		// SDK declaration: int scePowerIsBatteryExist();
		int scePowerIsBatteryExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E490401, "scePowerIsBatteryCharging" )]
		// SDK location: /power/psppower.h:76
		// SDK declaration: int scePowerIsBatteryCharging();
		int scePowerIsBatteryCharging(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4432BC8, "scePowerGetBatteryChargingStatus" )]
		// SDK location: /power/psppower.h:81
		// SDK declaration: int scePowerGetBatteryChargingStatus();
		int scePowerGetBatteryChargingStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3075926, "scePowerIsLowBattery" )]
		// SDK location: /power/psppower.h:86
		// SDK declaration: int scePowerIsLowBattery();
		int scePowerIsLowBattery(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2085D15D, "scePowerGetBatteryLifePercent" )]
		// SDK location: /power/psppower.h:92
		// SDK declaration: int scePowerGetBatteryLifePercent();
		int scePowerGetBatteryLifePercent(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EFB3FA2, "scePowerGetBatteryLifeTime" )]
		// SDK location: /power/psppower.h:97
		// SDK declaration: int scePowerGetBatteryLifeTime();
		int scePowerGetBatteryLifeTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28E12023, "scePowerGetBatteryTemp" )]
		// SDK location: /power/psppower.h:102
		// SDK declaration: int scePowerGetBatteryTemp();
		int scePowerGetBatteryTemp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x862AE1A6, "scePowerGetBatteryElec" )]
		// SDK location: /power/psppower.h:107
		// SDK declaration: int scePowerGetBatteryElec();
		int scePowerGetBatteryElec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x483CE86B, "scePowerGetBatteryVolt" )]
		// SDK location: /power/psppower.h:112
		// SDK declaration: int scePowerGetBatteryVolt();
		int scePowerGetBatteryVolt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6D016EF, "scePowerLock" )]
		// SDK location: /power/psppower.h:185
		// SDK declaration: int scePowerLock(int unknown);
		int scePowerLock( int unknown ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA3D34C1, "scePowerUnlock" )]
		// SDK location: /power/psppower.h:192
		// SDK declaration: int scePowerUnlock(int unknown);
		int scePowerUnlock( int unknown ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B7C7CF4, "scePowerRequestStandby" )]
		// SDK location: /power/psppower.h:227
		// SDK declaration: int scePowerRequestStandby();
		int scePowerRequestStandby(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC32C9CC, "scePowerRequestSuspend" )]
		// SDK location: /power/psppower.h:234
		// SDK declaration: int scePowerRequestSuspend();
		int scePowerRequestSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04B7766E, "scePowerRegisterCallback" )]
		// SDK location: /power/psppower.h:61
		// SDK declaration: int scePowerRegisterCallback(int slot, SceUID cbid);
		int scePowerRegisterCallback( int slot, int cbid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFA8BAF8, "scePowerUnregisterCallback" )]
		// manual add
		int scePowerUnregisterCallback( int slot )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x843FBF43, "scePowerSetCpuClockFrequency" )]
		// SDK location: /power/psppower.h:118
		// SDK declaration: int scePowerSetCpuClockFrequency(int cpufreq);
		int scePowerSetCpuClockFrequency( int cpufreq ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8D7B3FB, "scePowerSetBusClockFrequency" )]
		// SDK location: /power/psppower.h:124
		// SDK declaration: int scePowerSetBusClockFrequency(int busfreq);
		int scePowerSetBusClockFrequency( int busfreq ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEE03A2F, "scePowerGetCpuClockFrequency" )]
		// SDK location: /power/psppower.h:130
		// SDK declaration: int scePowerGetCpuClockFrequency();
		int scePowerGetCpuClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x478FE6F5, "scePowerGetBusClockFrequency" )]
		// SDK location: /power/psppower.h:148
		// SDK declaration: int scePowerGetBusClockFrequency();
		int scePowerGetBusClockFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDB5BFE9, "scePowerGetCpuClockFrequencyInt" )]
		// SDK location: /power/psppower.h:136
		// SDK declaration: int scePowerGetCpuClockFrequencyInt();
		int scePowerGetCpuClockFrequencyInt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD681969, "scePowerGetBusClockFrequencyInt" )]
		// SDK location: /power/psppower.h:154
		// SDK declaration: int scePowerGetBusClockFrequencyInt();
		int scePowerGetBusClockFrequencyInt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1A52C83, "scePowerGetCpuClockFrequencyFloat" )]
		// SDK location: /power/psppower.h:142
		// SDK declaration: float scePowerGetCpuClockFrequencyFloat();
		int scePowerGetCpuClockFrequencyFloat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9BADB3EB, "scePowerGetBusClockFrequencyFloat" )]
		// SDK location: /power/psppower.h:160
		// SDK declaration: float scePowerGetBusClockFrequencyFloat();
		int scePowerGetBusClockFrequencyFloat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x737486F2, "scePowerSetClockFrequency" )]
		// SDK location: /power/psppower.h:175
		// SDK declaration: int scePowerSetClockFrequency(int pllfreq, int cpufreq, int busfreq);
		int scePowerSetClockFrequency( int pllfreq, int cpufreq, int busfreq ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 08E9C0D8 */
