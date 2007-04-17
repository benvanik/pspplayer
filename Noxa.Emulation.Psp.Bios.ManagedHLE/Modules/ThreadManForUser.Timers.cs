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
	partial class ThreadManForUser
	{
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6652B8CA, "sceKernelSetAlarm" )]
		// SDK location: /user/pspthreadman.h:915
		// SDK declaration: SceUID sceKernelSetAlarm(SceUInt clock, SceKernelAlarmHandler handler, void *common);
		public int sceKernelSetAlarm( int clock, int handler, int common )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2C25152, "sceKernelSetSysClockAlarm" )]
		// SDK location: /user/pspthreadman.h:926
		// SDK declaration: SceUID sceKernelSetSysClockAlarm(SceKernelSysClock *clock, SceKernelAlarmHandler handler, void *common);
		public int sceKernelSetSysClockAlarm( int clock, int handler, int common )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E65B999, "sceKernelCancelAlarm" )]
		// SDK location: /user/pspthreadman.h:935
		// SDK declaration: int sceKernelCancelAlarm(SceUID alarmid);
		public int sceKernelCancelAlarm( int alarmid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAA3F564, "sceKernelReferAlarmStatus" )]
		// SDK location: /user/pspthreadman.h:945
		// SDK declaration: int sceKernelReferAlarmStatus(SceUID alarmid, SceKernelAlarmInfo *info);
		public int sceKernelReferAlarmStatus( int alarmid, int info )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20FFF560, "sceKernelCreateVTimer" )]
		// SDK location: /user/pspthreadman.h:1531
		// SDK declaration: SceUID sceKernelCreateVTimer(const char *name, struct SceKernelVTimerOptParam *opt);
		public int sceKernelCreateVTimer( int name, int opt )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328F9E52, "sceKernelDeleteVTimer" )]
		// SDK location: /user/pspthreadman.h:1540
		// SDK declaration: int sceKernelDeleteVTimer(SceUID uid);
		public int sceKernelDeleteVTimer( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3A59970, "sceKernelGetVTimerBase" )]
		// SDK location: /user/pspthreadman.h:1550
		// SDK declaration: int sceKernelGetVTimerBase(SceUID uid, SceKernelSysClock *base);
		public int sceKernelGetVTimerBase( int uid, int clockBase )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7C18B77, "sceKernelGetVTimerBaseWide" )]
		// SDK location: /user/pspthreadman.h:1559
		// SDK declaration: SceInt64 sceKernelGetVTimerBaseWide(SceUID uid);
		public long sceKernelGetVTimerBaseWide( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x034A921F, "sceKernelGetVTimerTime" )]
		// SDK location: /user/pspthreadman.h:1569
		// SDK declaration: int sceKernelGetVTimerTime(SceUID uid, SceKernelSysClock *time);
		public int sceKernelGetVTimerTime( int uid, int time )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0B3FFD2, "sceKernelGetVTimerTimeWide" )]
		// SDK location: /user/pspthreadman.h:1578
		// SDK declaration: SceInt64 sceKernelGetVTimerTimeWide(SceUID uid);
		public long sceKernelGetVTimerTimeWide( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x542AD630, "sceKernelSetVTimerTime" )]
		// SDK location: /user/pspthreadman.h:1588
		// SDK declaration: int sceKernelSetVTimerTime(SceUID uid, SceKernelSysClock *time);
		public int sceKernelSetVTimerTime( int uid, int time )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB6425C3, "sceKernelSetVTimerTimeWide" )]
		// SDK location: /user/pspthreadman.h:1598
		// SDK declaration: SceInt64 sceKernelSetVTimerTimeWide(SceUID uid, SceInt64 time);
		public long sceKernelSetVTimerTimeWide( int uid, long time )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC68D9437, "sceKernelStartVTimer" )]
		// SDK location: /user/pspthreadman.h:1607
		// SDK declaration: int sceKernelStartVTimer(SceUID uid);
		public int sceKernelStartVTimer( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0AEEE87, "sceKernelStopVTimer" )]
		// SDK location: /user/pspthreadman.h:1616
		// SDK declaration: int sceKernelStopVTimer(SceUID uid);
		public int sceKernelStopVTimer( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8B299AE, "sceKernelSetVTimerHandler" )]
		// SDK location: /user/pspthreadman.h:1631
		// SDK declaration: int sceKernelSetVTimerHandler(SceUID uid, SceKernelSysClock *time, SceKernelVTimerHandler handler, void *common);
		public int sceKernelSetVTimerHandler( int uid, int time, int handler, int common )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53B00E9A, "sceKernelSetVTimerHandlerWide" )]
		// SDK location: /user/pspthreadman.h:1643
		// SDK declaration: int sceKernelSetVTimerHandlerWide(SceUID uid, SceInt64 time, SceKernelVTimerHandlerWide handler, void *common);
		public int sceKernelSetVTimerHandlerWide( int uid, long time, int handler, int common )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2D615EF, "sceKernelCancelVTimerHandler" )]
		// SDK location: /user/pspthreadman.h:1652
		// SDK declaration: int sceKernelCancelVTimerHandler(SceUID uid);
		public int sceKernelCancelVTimerHandler( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F32BEAA, "sceKernelReferVTimerStatus" )]
		// SDK location: /user/pspthreadman.h:1673
		// SDK declaration: int sceKernelReferVTimerStatus(SceUID uid, SceKernelVTimerInfo *info);
		public int sceKernelReferVTimerStatus( int uid, int info )
		{
			return Module.NotImplementedReturn;
		}
	}
}
