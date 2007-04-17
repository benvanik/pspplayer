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
		[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )]
		// SDK location: /user/pspthreadman.h:244
		// SDK declaration: int sceKernelSleepThread();
		public int sceKernelSleepThread()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )]
		// SDK location: /user/pspthreadman.h:255
		// SDK declaration: int sceKernelSleepThreadCB();
		public int sceKernelSleepThreadCB()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:264
		// SDK declaration: int sceKernelWakeupThread(SceUID thid);
		public int sceKernelWakeupThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:273
		// SDK declaration: int sceKernelCancelWakeupThread(SceUID thid);
		public int sceKernelCancelWakeupThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )]
		// SDK location: /user/pspthreadman.h:282
		// SDK declaration: int sceKernelSuspendThread(SceUID thid);
		public int sceKernelSuspendThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )]
		// SDK location: /user/pspthreadman.h:291
		// SDK declaration: int sceKernelResumeThread(SceUID thid);
		public int sceKernelResumeThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )]
		// SDK location: /user/pspthreadman.h:301
		// SDK declaration: int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEnd( int thid, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )]
		// SDK location: /user/pspthreadman.h:311
		// SDK declaration: int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEndCB( int thid, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )]
		// SDK location: /user/pspthreadman.h:323
		// SDK declaration: int sceKernelDelayThread(SceUInt delay);
		public int sceKernelDelayThread( int delay )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )]
		// SDK location: /user/pspthreadman.h:335
		// SDK declaration: int sceKernelDelayThreadCB(SceUInt delay);
		public int sceKernelDelayThreadCB( int delay )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )]
		// SDK location: /user/pspthreadman.h:344
		// SDK declaration: int sceKernelDelaySysClockThread(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThread( int delay )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )]
		// SDK location: /user/pspthreadman.h:354
		// SDK declaration: int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThreadCB( int delay )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )]
		// SDK location: /user/pspthreadman.h:399
		// SDK declaration: int sceKernelReleaseWaitThread(SceUID thid);
		public int sceKernelReleaseWaitThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )]
		// SDK location: /user/pspthreadman.h:227
		// SDK declaration: int sceKernelSuspendDispatchThread();
		public int sceKernelSuspendDispatchThread()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )]
		// SDK location: /user/pspthreadman.h:237
		// SDK declaration: int sceKernelResumeDispatchThread(int state);
		public int sceKernelResumeDispatchThread( int state )
		{
			return Module.NotImplementedReturn;
		}
	}
}
