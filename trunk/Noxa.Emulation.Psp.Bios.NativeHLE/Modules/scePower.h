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

					public ref class scePower : public Module
					{
					public:
						scePower( IntPtr kernel ) : Module( kernel ) {}
						~scePower(){}

					public:
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
						// int scePowerTick(int unknown); (/power/psppower.h:198)
						int scePowerTick( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEDC13FE5, "scePowerGetIdleTimer" )] [Stateless]
						// int scePowerGetIdleTimer(); (/power/psppower.h:204)
						int scePowerGetIdleTimer(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7F30B3B1, "scePowerIdleTimerEnable" )] [Stateless]
						// int scePowerIdleTimerEnable(int unknown); (/power/psppower.h:211)
						int scePowerIdleTimerEnable( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x972CE941, "scePowerIdleTimerDisable" )] [Stateless]
						// int scePowerIdleTimerDisable(int unknown); (/power/psppower.h:218)
						int scePowerIdleTimerDisable( int unknown ){ return NISTUBRETURN; }

						[BiosFunction( 0x87440F5E, "scePowerIsPowerOnline" )] [Stateless]
						// int scePowerIsPowerOnline(); (/power/psppower.h:66)
						int scePowerIsPowerOnline(){ return 1; }

						[BiosFunction( 0x0AFD0D8B, "scePowerIsBatteryExist" )] [Stateless]
						// int scePowerIsBatteryExist(); (/power/psppower.h:71)
						int scePowerIsBatteryExist(){ return 1; }

						[BiosFunction( 0x1E490401, "scePowerIsBatteryCharging" )] [Stateless]
						// int scePowerIsBatteryCharging(); (/power/psppower.h:76)
						int scePowerIsBatteryCharging(){ return 0; }

						[NotImplemented]
						[BiosFunction( 0xB4432BC8, "scePowerGetBatteryChargingStatus" )] [Stateless]
						// int scePowerGetBatteryChargingStatus(); (/power/psppower.h:81)
						int scePowerGetBatteryChargingStatus(){ return NISTUBRETURN; }

						[BiosFunction( 0xD3075926, "scePowerIsLowBattery" )] [Stateless]
						// int scePowerIsLowBattery(); (/power/psppower.h:86)
						int scePowerIsLowBattery(){ return 0; }

						[BiosFunction( 0x2085D15D, "scePowerGetBatteryLifePercent" )] [Stateless]
						// int scePowerGetBatteryLifePercent(); (/power/psppower.h:92)
						int scePowerGetBatteryLifePercent(){ return 100; }

						[NotImplemented]
						[BiosFunction( 0x8EFB3FA2, "scePowerGetBatteryLifeTime" )] [Stateless]
						// int scePowerGetBatteryLifeTime(); (/power/psppower.h:97)
						int scePowerGetBatteryLifeTime(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28E12023, "scePowerGetBatteryTemp" )] [Stateless]
						// int scePowerGetBatteryTemp(); (/power/psppower.h:102)
						int scePowerGetBatteryTemp(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x862AE1A6, "scePowerGetBatteryElec" )] [Stateless]
						// int scePowerGetBatteryElec(); (/power/psppower.h:107)
						int scePowerGetBatteryElec(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x483CE86B, "scePowerGetBatteryVolt" )] [Stateless]
						// int scePowerGetBatteryVolt(); (/power/psppower.h:112)
						int scePowerGetBatteryVolt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD6D016EF, "scePowerLock" )] [Stateless]
						// int scePowerLock(int unknown); (/power/psppower.h:183)
						int scePowerLock( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCA3D34C1, "scePowerUnlock" )] [Stateless]
						// int scePowerUnlock(int unknown); (/power/psppower.h:190)
						int scePowerUnlock( int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2B7C7CF4, "scePowerRequestStandby" )] [Stateless]
						// int scePowerRequestStandby(); (/power/psppower.h:225)
						int scePowerRequestStandby(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAC32C9CC, "scePowerRequestSuspend" )] [Stateless]
						// int scePowerRequestSuspend(); (/power/psppower.h:232)
						int scePowerRequestSuspend(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x04B7766E, "scePowerRegisterCallback" )] [Stateless]
						// int scePowerRegisterCallback(int slot, SceUID cbid); (/power/psppower.h:61)
						int scePowerRegisterCallback( int slot, int cbid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDFA8BAF8, "scePowerUnregisterCallback" )] [Stateless]
						// manual add
						int scePowerUnregisterCallback( int slot ){ return NISTUBRETURN; }

						[BiosFunction( 0x843FBF43, "scePowerSetCpuClockFrequency" )] [Stateless]
						// int scePowerSetCpuClockFrequency(int cpufreq); (/power/psppower.h:118)
						int scePowerSetCpuClockFrequency( int cpufreq ){ return 0; }

						[BiosFunction( 0xB8D7B3FB, "scePowerSetBusClockFrequency" )] [Stateless]
						// int scePowerSetBusClockFrequency(int busfreq); (/power/psppower.h:124)
						int scePowerSetBusClockFrequency( int busfreq ){ return 0; }

						[NotImplemented]
						[BiosFunction( 0xFEE03A2F, "scePowerGetCpuClockFrequency" )] [Stateless]
						// int scePowerGetCpuClockFrequency(); (/power/psppower.h:130)
						int scePowerGetCpuClockFrequency(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x478FE6F5, "scePowerGetBusClockFrequency" )] [Stateless]
						// int scePowerGetBusClockFrequency(); (/power/psppower.h:148)
						int scePowerGetBusClockFrequency(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFDB5BFE9, "scePowerGetCpuClockFrequencyInt" )] [Stateless]
						// int scePowerGetCpuClockFrequencyInt(); (/power/psppower.h:136)
						int scePowerGetCpuClockFrequencyInt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBD681969, "scePowerGetBusClockFrequencyInt" )] [Stateless]
						// int scePowerGetBusClockFrequencyInt(); (/power/psppower.h:154)
						int scePowerGetBusClockFrequencyInt(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB1A52C83, "scePowerGetCpuClockFrequencyFloat" )] [Stateless]
						// float scePowerGetCpuClockFrequencyFloat(); (/power/psppower.h:142)
						int scePowerGetCpuClockFrequencyFloat(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9BADB3EB, "scePowerGetBusClockFrequencyFloat" )] [Stateless]
						// float scePowerGetBusClockFrequencyFloat(); (/power/psppower.h:160)
						int scePowerGetBusClockFrequencyFloat(){ return NISTUBRETURN; }

						[BiosFunction( 0x737486F2, "scePowerSetClockFrequency" )] [Stateless]
						// int scePowerSetClockFrequency(int cpufreq, int ramfreq, int busfreq); (/power/psppower.h:173)
						int scePowerSetClockFrequency( int cpufreq, int ramfreq, int busfreq ){ return 0; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - AB26FEBE */
