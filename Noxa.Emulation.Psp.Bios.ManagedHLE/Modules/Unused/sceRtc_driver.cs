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
	class sceRtc_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceRtc_driver";
			}
		}

		#endregion

		#region State Management

		public sceRtc_driver( Kernel kernel )
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
		[BiosFunction( 0x912BEE56, "sceRtcInit" )]
		int sceRtcInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE27DE2F, "sceRtcEnd" )]
		int sceRtcEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CC2797E, "sceRtcSuspend" )]
		int sceRtcSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x48D07D70, "sceRtcResume" )]
		int sceRtcResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0B5571C, "sceRtcSynchronize" )]
		int sceRtcSynchronize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x759937C5, "sceRtcSetConf" )]
		int sceRtcSetConf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC41C2853, "sceRtcGetTickResolution" )]
		int sceRtcGetTickResolution(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9763C138, "sceRtcSetCurrentTick" )]
		int sceRtcSetCurrentTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17C26C00, "sceRtc_driver_17C26C00" )]
		int sceRtc_driver_17C26C00(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F7AD767, "sceRtcGetCurrentTick" )]
		int sceRtcGetCurrentTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB44BDAED, "sceRtc_driver_B44BDAED" )]
		int sceRtc_driver_B44BDAED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x029CA3B3, "sceRtc_driver_029CA3B3" )]
		int sceRtc_driver_029CA3B3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CFA57B0, "sceRtcGetCurrentClock" )]
		int sceRtcGetCurrentClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7C27D1B, "sceRtcGetCurrentClockLocalTime" )]
		int sceRtcGetCurrentClockLocalTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34885E0D, "sceRtcConvertUtcToLocalTime" )]
		int sceRtcConvertUtcToLocalTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x779242A2, "sceRtcConvertLocalTimeToUTC" )]
		int sceRtcConvertLocalTimeToUTC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D1FBED3, "sceRtcSetAlarmTick" )]
		int sceRtcSetAlarmTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2DDBEB5, "sceRtcGetAlarmTick" )]
		int sceRtcGetAlarmTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42307A17, "sceRtcIsLeapYear" )]
		int sceRtcIsLeapYear(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05EF322C, "sceRtcGetDaysInMonth" )]
		int sceRtcGetDaysInMonth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57726BC1, "sceRtcGetDayOfWeek" )]
		int sceRtcGetDayOfWeek(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B1B5E82, "sceRtcCheckValid" )]
		int sceRtcCheckValid(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A807CC8, "sceRtcSetTime_t" )]
		int sceRtcSetTime_t(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27C4594C, "sceRtcGetTime_t" )]
		int sceRtcGetTime_t(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF006F264, "sceRtcSetDosTime" )]
		int sceRtcSetDosTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36075567, "sceRtcGetDosTime" )]
		int sceRtcGetDosTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ACE4C04, "sceRtcSetWin32FileTime" )]
		int sceRtcSetWin32FileTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF561893, "sceRtcGetWin32FileTime" )]
		int sceRtcGetWin32FileTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ED29E40, "sceRtcSetTick" )]
		int sceRtcSetTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FF40ACC, "sceRtcGetTick" )]
		int sceRtcGetTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9ED0AE87, "sceRtcCompareTick" )]
		int sceRtcCompareTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44F45E05, "sceRtcTickAddTicks" )]
		int sceRtcTickAddTicks(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26D25A5D, "sceRtcTickAddMicroseconds" )]
		int sceRtcTickAddMicroseconds(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF2A4AFE5, "sceRtcTickAddSeconds" )]
		int sceRtcTickAddSeconds(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6605BCA, "sceRtcTickAddMinutes" )]
		int sceRtcTickAddMinutes(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26D7A24A, "sceRtcTickAddHours" )]
		int sceRtcTickAddHours(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE51B4B7A, "sceRtcTickAddDays" )]
		int sceRtcTickAddDays(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF3A2CA8, "sceRtcTickAddWeeks" )]
		int sceRtcTickAddWeeks(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBF74F1B, "sceRtcTickAddMonths" )]
		int sceRtcTickAddMonths(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42842C77, "sceRtcTickAddYears" )]
		int sceRtcTickAddYears(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC663B3B9, "sceRtcFormatRFC2822" )]
		int sceRtcFormatRFC2822(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DE6711B, "sceRtcFormatRFC2822LocalTime" )]
		int sceRtcFormatRFC2822LocalTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0498FB3C, "sceRtcFormatRFC3339" )]
		int sceRtcFormatRFC3339(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27F98543, "sceRtcFormatRFC3339LocalTime" )]
		int sceRtcFormatRFC3339LocalTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFBC5F16, "sceRtcParseDateTime" )]
		int sceRtcParseDateTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28E1E988, "sceRtcParseRFC3339" )]
		int sceRtcParseRFC3339(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F9DBF3C0 */
