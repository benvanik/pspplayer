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
		[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )]
		// SDK location: /user/pspthreadman.h:244
		// SDK declaration: int sceKernelSleepThread();
		public int sceKernelSleepThread()
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			if( thread.Sleep( false ) == true )
				_kernel.Schedule();

			return 0;
		}

		[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )]
		// SDK location: /user/pspthreadman.h:255
		// SDK declaration: int sceKernelSleepThreadCB();
		public int sceKernelSleepThreadCB()
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			if( thread.Sleep( true ) == true )
				_kernel.Schedule();

			return 0;
		}

		[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:264
		// SDK declaration: int sceKernelWakeupThread(SceUID thid);
		public int sceKernelWakeupThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			// Perhaps we shouldn't schedule here?
			thread.Wake( 0 );
			if( thread.State == KThreadState.Ready )
				_kernel.Schedule();

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:273
		// SDK declaration: int sceKernelCancelWakeupThread(SceUID thid);
		public int sceKernelCancelWakeupThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			// Cancel wakeup not supported - perhaps we shouldn't cs in sceKernelWakeupThread?
			if( thread.WakeupCount > 0 )
			{
				thread.WakeupCount = 0;
				return ( int )thread.WakeupCount;
			}
			else
				return 0;
		}

		[Stateless]
		[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )]
		// SDK location: /user/pspthreadman.h:282
		// SDK declaration: int sceKernelSuspendThread(SceUID thid);
		public int sceKernelSuspendThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			thread.Suspend();
			//_kernel.Schedule();

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )]
		// SDK location: /user/pspthreadman.h:291
		// SDK declaration: int sceKernelResumeThread(SceUID thid);
		public int sceKernelResumeThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			thread.Resume();
			//_kernel.Schedule();

			return 0;
		}

		[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )]
		// SDK location: /user/pspthreadman.h:301
		// SDK declaration: int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEnd( int thid, int timeout )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			KThread targetThread = _kernel.GetHandle<KThread>( thid );
			if( targetThread == null )
				return -1;

			// If already stopped, return
			if( ( targetThread.State == KThreadState.Dead ) ||
				( targetThread.State == KThreadState.Stopped ) )
				return 0;

			uint timeoutUs = 0;
			if( timeout != 0 )
			{
				unsafe
				{
					uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
					timeoutUs = *ptimeout;
				}
			}

			thread.Join( targetThread, timeoutUs, false );
			_kernel.Schedule();

			return 0;
		}

		[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )]
		// SDK location: /user/pspthreadman.h:311
		// SDK declaration: int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEndCB( int thid, int timeout )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			KThread targetThread = _kernel.GetHandle<KThread>( thid );
			if( targetThread == null )
				return -1;

			// If already stopped, return
			if( ( targetThread.State == KThreadState.Dead ) ||
				( targetThread.State == KThreadState.Stopped ) )
				return 0;

			uint timeoutUs = 0;
			if( timeout != 0 )
			{
				unsafe
				{
					uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
					timeoutUs = *ptimeout;
				}
			}

			thread.Join( targetThread, timeoutUs, true );
			_kernel.Schedule();

			return 0;
		}

		[DontTrace]
		[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )]
		// SDK location: /user/pspthreadman.h:323
		// SDK declaration: int sceKernelDelayThread(SceUInt delay);
		public int sceKernelDelayThread( int delay )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Delay( ( uint )delay, false );
			_kernel.Schedule();

			return 0;
		}

		[DontTrace]
		[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )]
		// SDK location: /user/pspthreadman.h:335
		// SDK declaration: int sceKernelDelayThreadCB(SceUInt delay);
		public int sceKernelDelayThreadCB( int delay )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Delay( ( uint )delay, true );
			_kernel.Schedule();

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )]
		// SDK location: /user/pspthreadman.h:344
		// SDK declaration: int sceKernelDelaySysClockThread(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThread( int delay )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )]
		// SDK location: /user/pspthreadman.h:354
		// SDK declaration: int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThreadCB( int delay )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )]
		// SDK location: /user/pspthreadman.h:399
		// SDK declaration: int sceKernelReleaseWaitThread(SceUID thid);
		public int sceKernelReleaseWaitThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;
			if( ( thread.State == KThreadState.Waiting ) ||
				( thread.State == KThreadState.WaitSuspended ) )
			{
				thread.ReleaseWait();
				_kernel.Schedule();
				return 0;
			}
			else
				return unchecked( ( int )0x800201A6 );
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )]
		// SDK location: /user/pspthreadman.h:227
		// SDK declaration: int sceKernelSuspendDispatchThread();
		public int sceKernelSuspendDispatchThread()
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )]
		// SDK location: /user/pspthreadman.h:237
		// SDK declaration: int sceKernelResumeDispatchThread(int state);
		public int sceKernelResumeDispatchThread( int state )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return Module.NotImplementedReturn;
		}
	}
}
