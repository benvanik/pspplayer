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

					ref class sceRtc : public Module
					{
					public:
						sceRtc( Kernel^ kernel ) : Module( kernel ) {}
						~sceRtc(){}

						property String^ Name { virtual String^ get() override { return "sceRtc"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xC41C2853, "sceRtcGetTickResolution" )] [Stateless]
						// /rtc/psprtc.h:46: u32 sceRtcGetTickResolution();
						int sceRtcGetTickResolution(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3F7AD767, "sceRtcGetCurrentTick" )] [Stateless]
						// /rtc/psprtc.h:54: int sceRtcGetCurrentTick(u64 *tick);
						int sceRtcGetCurrentTick( int tick ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4CFA57B0, "sceRtcGetCurrentClock" )] [Stateless]
						// /rtc/psprtc.h:63: int sceRtcGetCurrentClock(pspTime *time, int tz);
						int sceRtcGetCurrentClock( int time, int tz ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE7C27D1B, "sceRtcGetCurrentClockLocalTime" )] [Stateless]
						// /rtc/psprtc.h:71: int sceRtcGetCurrentClockLocalTime(pspTime *time);
						int sceRtcGetCurrentClockLocalTime( int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x34885E0D, "sceRtcConvertUtcToLocalTime" )] [Stateless]
						// /rtc/psprtc.h:80: int sceRtcConvertUtcToLocalTime(const u64* tickUTC, u64* tickLocal);
						int sceRtcConvertUtcToLocalTime( int tickUTC, int tickLocal ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x779242A2, "sceRtcConvertLocalTimeToUTC" )] [Stateless]
						// /rtc/psprtc.h:89: int sceRtcConvertLocalTimeToUTC(const u64* tickLocal, u64* tickUTC);
						int sceRtcConvertLocalTimeToUTC( int tickLocal, int tickUTC ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x42307A17, "sceRtcIsLeapYear" )] [Stateless]
						// /rtc/psprtc.h:97: int sceRtcIsLeapYear(int year);
						int sceRtcIsLeapYear( int year ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x05EF322C, "sceRtcGetDaysInMonth" )] [Stateless]
						// /rtc/psprtc.h:106: int sceRtcGetDaysInMonth(int year, int month);
						int sceRtcGetDaysInMonth( int year, int month ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57726BC1, "sceRtcGetDayOfWeek" )] [Stateless]
						// /rtc/psprtc.h:116: int sceRtcGetDayOfWeek(int year, int month, int day);
						int sceRtcGetDayOfWeek( int year, int month, int day ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4B1B5E82, "sceRtcCheckValid" )] [Stateless]
						// /rtc/psprtc.h:124: int sceRtcCheckValid(const pspTime* date);
						int sceRtcCheckValid( int date ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3A807CC8, "sceRtcSetTime_t" )] [Stateless]
						// /rtc/psprtc.h:244: int sceRtcSetTime_t(pspTime* date, const time_t time);
						int sceRtcSetTime_t( int date, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x27C4594C, "sceRtcGetTime_t" )] [Stateless]
						// /rtc/psprtc.h:245: int sceRtcGetTime_t(const pspTime* date, time_t *time);
						int sceRtcGetTime_t( int date, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF006F264, "sceRtcSetDosTime" )] [Stateless]
						// /rtc/psprtc.h:246: int sceRtcSetDosTime(pspTime* date, u32 dosTime);
						int sceRtcSetDosTime( int date, int dosTime ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x36075567, "sceRtcGetDosTime" )] [Stateless]
						// /rtc/psprtc.h:247: int sceRtcGetDosTime(pspTime* date, u32 dosTime);
						int sceRtcGetDosTime( int date, int dosTime ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7ACE4C04, "sceRtcSetWin32FileTime" )] [Stateless]
						// /rtc/psprtc.h:248: int sceRtcSetWin32FileTime(pspTime* date, u64* win32Time);
						int sceRtcSetWin32FileTime( int date, int win32Time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCF561893, "sceRtcGetWin32FileTime" )] [Stateless]
						// /rtc/psprtc.h:249: int sceRtcGetWin32FileTime(pspTime* date, u64* win32Time);
						int sceRtcGetWin32FileTime( int date, int win32Time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7ED29E40, "sceRtcSetTick" )] [Stateless]
						// /rtc/psprtc.h:133: int sceRtcSetTick(pspTime* date, const u64* tick);
						int sceRtcSetTick( int date, int tick ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6FF40ACC, "sceRtcGetTick" )] [Stateless]
						// /rtc/psprtc.h:142: int sceRtcGetTick(const pspTime* date, u64 *tick);
						int sceRtcGetTick( int date, int tick ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9ED0AE87, "sceRtcCompareTick" )] [Stateless]
						// /rtc/psprtc.h:151: int sceRtcCompareTick(const u64* tick1, const u64* tick2);
						int sceRtcCompareTick( int tick1, int tick2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x44F45E05, "sceRtcTickAddTicks" )] [Stateless]
						// /rtc/psprtc.h:161: int sceRtcTickAddTicks(u64* destTick, const u64* srcTick, u64 numTicks);
						int sceRtcTickAddTicks( int destTick, int srcTick, int numTicks ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x26D25A5D, "sceRtcTickAddMicroseconds" )] [Stateless]
						// /rtc/psprtc.h:171: int sceRtcTickAddMicroseconds(u64* destTick, const u64* srcTick, u64 numMS);
						int sceRtcTickAddMicroseconds( int destTick, int srcTick, int numMS ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF2A4AFE5, "sceRtcTickAddSeconds" )] [Stateless]
						// /rtc/psprtc.h:181: int sceRtcTickAddSeconds(u64* destTick, const u64* srcTick, u64 numSecs);
						int sceRtcTickAddSeconds( int destTick, int srcTick, int numSecs ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE6605BCA, "sceRtcTickAddMinutes" )] [Stateless]
						// /rtc/psprtc.h:191: int sceRtcTickAddMinutes(u64* destTick, const u64* srcTick, u64 numMins);
						int sceRtcTickAddMinutes( int destTick, int srcTick, int numMins ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x26D7A24A, "sceRtcTickAddHours" )] [Stateless]
						// /rtc/psprtc.h:201: int sceRtcTickAddHours(u64* destTick, const u64* srcTick, int numHours);
						int sceRtcTickAddHours( int destTick, int srcTick, int numHours ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE51B4B7A, "sceRtcTickAddDays" )] [Stateless]
						// /rtc/psprtc.h:211: int sceRtcTickAddDays(u64* destTick, const u64* srcTick, int numDays);
						int sceRtcTickAddDays( int destTick, int srcTick, int numDays ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCF3A2CA8, "sceRtcTickAddWeeks" )] [Stateless]
						// /rtc/psprtc.h:221: int sceRtcTickAddWeeks(u64* destTick, const u64* srcTick, int numWeeks);
						int sceRtcTickAddWeeks( int destTick, int srcTick, int numWeeks ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDBF74F1B, "sceRtcTickAddMonths" )] [Stateless]
						// /rtc/psprtc.h:232: int sceRtcTickAddMonths(u64* destTick, const u64* srcTick, int numMonths);
						int sceRtcTickAddMonths( int destTick, int srcTick, int numMonths ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x42842C77, "sceRtcTickAddYears" )] [Stateless]
						// /rtc/psprtc.h:242: int sceRtcTickAddYears(u64* destTick, const u64* srcTick, int numYears);
						int sceRtcTickAddYears( int destTick, int srcTick, int numYears ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDFBC5F16, "sceRtcParseDateTime" )] [Stateless]
						// /rtc/psprtc.h:251: int sceRtcParseDateTime(u64 *destTick, const char *dateString);
						int sceRtcParseDateTime( int destTick, int dateString ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 73DFBB09 */
