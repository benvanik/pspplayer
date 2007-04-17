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
		//[NotImplemented]
		//[Stateless]
		//[BiosFunction( 0x0E927AED, "_sceKernelReturnFromTimerHandler" )]
		//// SDK location: /user/pspthreadman.h:1447
		//// SDK declaration: void _sceKernelReturnFromTimerHandler();
		//public void _sceKernelReturnFromTimerHandler()
		//{
		//}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )]
		// SDK location: /user/pspthreadman.h:1463
		// SDK declaration: int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock);
		public int sceKernelUSec2SysClock( int usec, int clock )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )]
		// SDK location: /user/pspthreadman.h:1472
		// SDK declaration: SceInt64 sceKernelUSec2SysClockWide(unsigned int usec);
		public long sceKernelUSec2SysClockWide( int usec )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )]
		// SDK location: /user/pspthreadman.h:1483
		// SDK declaration: int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *low, unsigned int *high);
		public int sceKernelSysClock2USec( int clock, int low, int high )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )]
		// SDK location: /user/pspthreadman.h:1494
		// SDK declaration: int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *low, unsigned int *high);
		public int sceKernelSysClock2USecWide( long clock, int low, int high )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )]
		// SDK location: /user/pspthreadman.h:1503
		// SDK declaration: int sceKernelGetSystemTime(SceKernelSysClock *time);
		public int sceKernelGetSystemTime( int time )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )]
		// SDK location: /user/pspthreadman.h:1510
		// SDK declaration: SceInt64 sceKernelGetSystemTimeWide();
		public long sceKernelGetSystemTimeWide()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )]
		// SDK location: /user/pspthreadman.h:1517
		// SDK declaration: unsigned int sceKernelGetSystemTimeLow();
		public int sceKernelGetSystemTimeLow()
		{
			return Module.NotImplementedReturn;
		}
	}
}
