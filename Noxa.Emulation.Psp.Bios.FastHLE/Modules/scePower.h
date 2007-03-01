// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					ref class scePower : public Module
					{
					public:
						scePower( Kernel^ kernel ) : Module( kernel ) {}
						~scePower(){}

						property String^ Name { virtual String^ get() override { return "scePower"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xEFD3C963, "scePowerTick" )] [Stateless]
						// /power/psppower.h:198: int scePowerTick(int unknown);
						int scePowerTick( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEDC13FE5, "scePowerGetIdleTimer" )] [Stateless]
						// /power/psppower.h:204: int scePowerGetIdleTimer();
						int scePowerGetIdleTimer(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7F30B3B1, "scePowerIdleTimerEnable" )] [Stateless]
						// /power/psppower.h:211: int scePowerIdleTimerEnable(int unknown);
						int scePowerIdleTimerEnable( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x972CE941, "scePowerIdleTimerDisable" )] [Stateless]
						// /power/psppower.h:218: int scePowerIdleTimerDisable(int unknown);
						int scePowerIdleTimerDisable( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x87440F5E, "scePowerIsPowerOnline" )] [Stateless]
						// /power/psppower.h:66: int scePowerIsPowerOnline();
						int scePowerIsPowerOnline(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0AFD0D8B, "scePowerIsBatteryExist" )] [Stateless]
						// /power/psppower.h:71: int scePowerIsBatteryExist();
						int scePowerIsBatteryExist(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1E490401, "scePowerIsBatteryCharging" )] [Stateless]
						// /power/psppower.h:76: int scePowerIsBatteryCharging();
						int scePowerIsBatteryCharging(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB4432BC8, "scePowerGetBatteryChargingStatus" )] [Stateless]
						// /power/psppower.h:81: int scePowerGetBatteryChargingStatus();
						int scePowerGetBatteryChargingStatus(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD3075926, "scePowerIsLowBattery" )] [Stateless]
						// /power/psppower.h:86: int scePowerIsLowBattery();
						int scePowerIsLowBattery(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2085D15D, "scePowerGetBatteryLifePercent" )] [Stateless]
						// /power/psppower.h:92: int scePowerGetBatteryLifePercent();
						int scePowerGetBatteryLifePercent(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8EFB3FA2, "scePowerGetBatteryLifeTime" )] [Stateless]
						// /power/psppower.h:97: int scePowerGetBatteryLifeTime();
						int scePowerGetBatteryLifeTime(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28E12023, "scePowerGetBatteryTemp" )] [Stateless]
						// /power/psppower.h:102: int scePowerGetBatteryTemp();
						int scePowerGetBatteryTemp(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x862AE1A6, "scePowerGetBatteryElec" )] [Stateless]
						// /power/psppower.h:107: int scePowerGetBatteryElec();
						int scePowerGetBatteryElec(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x483CE86B, "scePowerGetBatteryVolt" )] [Stateless]
						// /power/psppower.h:112: int scePowerGetBatteryVolt();
						int scePowerGetBatteryVolt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD6D016EF, "scePowerLock" )] [Stateless]
						// /power/psppower.h:183: int scePowerLock(int unknown);
						int scePowerLock( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCA3D34C1, "scePowerUnlock" )] [Stateless]
						// /power/psppower.h:190: int scePowerUnlock(int unknown);
						int scePowerUnlock( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2B7C7CF4, "scePowerRequestStandby" )] [Stateless]
						// /power/psppower.h:225: int scePowerRequestStandby();
						int scePowerRequestStandby(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAC32C9CC, "scePowerRequestSuspend" )] [Stateless]
						// /power/psppower.h:232: int scePowerRequestSuspend();
						int scePowerRequestSuspend(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x04B7766E, "scePowerRegisterCallback" )] [Stateless]
						// /power/psppower.h:61: int scePowerRegisterCallback(int slot, SceUID cbid);
						int scePowerRegisterCallback( int slot, int cbid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x843FBF43, "scePowerSetCpuClockFrequency" )] [Stateless]
						// /power/psppower.h:118: int scePowerSetCpuClockFrequency(int cpufreq);
						int scePowerSetCpuClockFrequency( int cpufreq ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB8D7B3FB, "scePowerSetBusClockFrequency" )] [Stateless]
						// /power/psppower.h:124: int scePowerSetBusClockFrequency(int busfreq);
						int scePowerSetBusClockFrequency( int busfreq ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFEE03A2F, "scePowerGetCpuClockFrequency" )] [Stateless]
						// /power/psppower.h:130: int scePowerGetCpuClockFrequency();
						int scePowerGetCpuClockFrequency(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x478FE6F5, "scePowerGetBusClockFrequency" )] [Stateless]
						// /power/psppower.h:148: int scePowerGetBusClockFrequency();
						int scePowerGetBusClockFrequency(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFDB5BFE9, "scePowerGetCpuClockFrequencyInt" )] [Stateless]
						// /power/psppower.h:136: int scePowerGetCpuClockFrequencyInt();
						int scePowerGetCpuClockFrequencyInt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBD681969, "scePowerGetBusClockFrequencyInt" )] [Stateless]
						// /power/psppower.h:154: int scePowerGetBusClockFrequencyInt();
						int scePowerGetBusClockFrequencyInt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB1A52C83, "scePowerGetCpuClockFrequencyFloat" )] [Stateless]
						// /power/psppower.h:142: float scePowerGetCpuClockFrequencyFloat();
						int scePowerGetCpuClockFrequencyFloat(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9BADB3EB, "scePowerGetBusClockFrequencyFloat" )] [Stateless]
						// /power/psppower.h:160: float scePowerGetBusClockFrequencyFloat();
						int scePowerGetBusClockFrequencyFloat(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x737486F2, "scePowerSetClockFrequency" )] [Stateless]
						// /power/psppower.h:173: int scePowerSetClockFrequency(int cpufreq, int ramfreq, int busfreq);
						int scePowerSetClockFrequency( int cpufreq, int ramfreq, int busfreq ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 27D0BD44 */
