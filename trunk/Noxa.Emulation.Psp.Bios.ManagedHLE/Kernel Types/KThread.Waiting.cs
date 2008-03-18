// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class KThread
	{
		public void Wake()
		{
			State = KThreadState.Ready;

			WakeupCount++;

			this.AddToSchedule();
		}

		public void Wake( int returnValue )
		{
			if( this.State == KThreadState.Dead )
				return;

			if( this.State == KThreadState.WaitSuspended )
				this.State = KThreadState.Suspended;
			else if( this.State == KThreadState.Waiting )
			{
				this.State = KThreadState.Ready;
				this.Kernel.Cpu.SetContextRegister( ContextID, 2, ( uint )returnValue );
				this.WakeupCount++;
				this.AddToSchedule();
			}
		}

		public void ReleaseWait()
		{
			if( ( State == KThreadState.Waiting ) ||
				( State == KThreadState.WaitSuspended ) )
			{
				Debug.Assert( false, "implement nice way to cancel things - need to remove from waitingthreads list of whatever it's waiting on" );
				//switch( this.WaitingOn )
				//{
				//}
			}
			if( State == KThreadState.WaitSuspended )
				State = KThreadState.Suspended;
			else
				State = KThreadState.Ready;

			ReleaseCount++;
			Kernel.Cpu.SetContextRegister( ContextID, 2, 0x800201AA );

			if( State == KThreadState.Ready )
				this.AddToSchedule();
		}

		public void Suspend()
		{
			if( State == KThreadState.Waiting )
				State = KThreadState.WaitSuspended;
			else
			{
				State = KThreadState.Suspended;
				this.RemoveFromSchedule();
			}
		}

		public void Resume()
		{
			if( State == KThreadState.WaitSuspended )
				State = KThreadState.Waiting;
			else
			{
				State = KThreadState.Ready;
				this.AddToSchedule();
			}
		}

		public bool Sleep( bool canHandleCallbacks )
		{
			if( this.WakeupCount > 0 )
			{
				this.WakeupCount--;
				return false;
			}

			if( State == KThreadState.Suspended )
				State = KThreadState.WaitSuspended;
			else
				State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			WaitingOn = KThreadWait.Sleep;

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();

			return true;
		}

		private void DelayCallback( Timer timer, object state )
		{
			if( this.State == KThreadState.WaitSuspended )
				this.State = KThreadState.Suspended;
			else if( ( this.State == KThreadState.Waiting ) &&
				( this.WaitingOn == KThreadWait.Delay ) )
			{
				this.State = KThreadState.Ready;
				//Kernel.Cpu.SetContextRegister( ContextID, 2, 0 );
				this.AddToSchedule();
				this.Kernel.Schedule();
			}
		}

		public void Delay( uint waitTimeUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			// A time of zero could be valid - that may mean just delay until the next time cycle
			//Debug.Assert( waitTimeUs > 0 );

			CanHandleCallbacks = canHandleCallbacks;

			WaitingOn = KThreadWait.Delay;
			NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
			WaitTimeout = waitTimeUs * 10;	// us -> ticks

			// Install timer
			uint waitTimeMs = Math.Max( 5, waitTimeUs / 1000 );
			Kernel.AddOneShotTimer( this.DelayCallback, this, waitTimeMs );

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();
		}

		private void JoinCallback( Timer timer, object state )
		{
			this.State = KThreadState.Ready;
			this.Kernel.Cpu.SetContextRegister( ContextID, 2, unchecked( ( uint )-1 ) );

			this.AddToSchedule();
			this.Kernel.Schedule();
		}

		public void Join( KThread targetThread, uint timeoutUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			WaitingOn = KThreadWait.Join;
			if( timeoutUs > 0 )
			{
				NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
				WaitTimeout = timeoutUs * 10;	// us -> ticks
			}
			else
				WaitTimeout = 0;
			WaitHandle = targetThread;

			targetThread.ExitWaiters.Enqueue( this );

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();

			if( timeoutUs > 0 )
			{
				// Install timer
				uint timeoutMs = Math.Max( 5, timeoutUs / 1000 );
				Kernel.AddOneShotTimer( this.JoinCallback, this, timeoutMs );
			}
		}

		public const uint SCE_KERNEL_ERROR_WAIT_TIMEOUT = 0x800201A8;

		private void WaitCallback( Timer timer, object state )
		{
			// If we have not been made ready already, we wake
			if( ( this.State == KThreadState.Waiting ) ||
				( this.State == KThreadState.WaitSuspended ) )
			{
				if( this.WaitHandle is KSemaphore )
					( ( KSemaphore )this.WaitHandle ).WaitingThreads.Remove( this );
				else if( this.WaitHandle is KEvent )
					( ( KEvent )this.WaitHandle ).WaitingThreads.Remove( this );
				else if( this.WaitHandle is KPool )
					( ( KPool )this.WaitHandle ).WaitingThreads.Remove( this );

				if( this.State == KThreadState.Waiting )
				{
					this.State = KThreadState.Ready;
					this.Kernel.Cpu.SetContextRegister( ContextID, 2, SCE_KERNEL_ERROR_WAIT_TIMEOUT );

					this.AddToSchedule();
					this.Kernel.Schedule();
				}
				else
					this.State = KThreadState.Suspended;
			}
		}

		private void WaitTimeoutSetup( uint timeoutUs )
		{
			if( timeoutUs > 0 )
			{
				NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
				WaitTimeout = timeoutUs * 10;	// us -> ticks
				WaitTimeout = Math.Max( 1, WaitTimeout );

				// Install timer
				uint timeoutMs = Math.Max( 5, timeoutUs / 1000 );
				Kernel.AddOneShotTimer( this.WaitCallback, this, timeoutMs );
			}
			else
				WaitTimeout = 0;
		}

		public void Wait( KEvent ev, KWaitType waitEventMode, uint userValue, uint outAddress, uint timeoutUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			ev.WaitingThreads.Enqueue( this );

			WaitingOn = KThreadWait.Event;
			this.WaitTimeoutSetup( timeoutUs );
			WaitHandle = ev;
			WaitEventMode = waitEventMode;
			WaitArgument = userValue;
			WaitAddress = outAddress;

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();
		}

		public void Wait( KPool pool, uint pdata, uint timeoutUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			pool.WaitingThreads.Enqueue( this );

			if( pool is KVariablePool )
				WaitingOn = KThreadWait.Vpl;
			else
				WaitingOn = KThreadWait.Fpl;
			this.WaitTimeoutSetup( timeoutUs );
			WaitHandle = pool;
			WaitAddress = pdata;

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();
		}

		public void Wait( KSemaphore sema, int count, uint timeoutUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			sema.WaitingThreads.Enqueue( this );
			WaitingOn = KThreadWait.Semaphore;
			this.WaitTimeoutSetup( timeoutUs );
			WaitHandle = sema;
			WaitArgument = ( uint )count;

			if( canHandleCallbacks == true )
				this.Kernel.CheckCallbacks();

			this.Kernel.Schedule();
		}

		public void Wait( KMutex mutex, uint timeoutUs )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = false;

			mutex.WaitingThreads.Enqueue( this );
			WaitingOn = KThreadWait.Mutex;
			this.WaitTimeoutSetup( timeoutUs );
			WaitHandle = mutex;

			this.Kernel.Schedule();
		}

		public void Wait( KMessagePipe pipe, int message, int size, int outSize, int timeout, bool canHandleCallbacks )
		{
			this.State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			this.CanHandleCallbacks = canHandleCallbacks;

			pipe.WaitingThreads.Enqueue( this );

			this.WaitingOn = KThreadWait.Mpp;
			this.WaitTimeoutSetup( ( uint )timeout );
			this.WaitHandle = pipe;
			this.WaitArgument = ( uint )size;
			this.WaitAddress = ( uint )message;
			this.WaitAddressResult = ( uint )outSize;

			if (canHandleCallbacks == true)
			{
				this.Kernel.CheckCallbacks();
			}

			this.Kernel.Schedule();
		}

		public void Wait(KMessageBox box, int pmessage, int timeout, bool canHandleCallbacks)
		{
			this.State = KThreadState.Waiting;
			this.RemoveFromSchedule();
			
			this.CanHandleCallbacks = canHandleCallbacks;
			
			box.WaitingThreads.Enqueue(this);
			
			this.WaitingOn = KThreadWait.Mbx;
			this.WaitTimeoutSetup((uint)timeout);
			this.WaitHandle = box;
			this.WaitAddress = (uint)pmessage;

			if (canHandleCallbacks == true)
			{
				this.Kernel.CheckCallbacks();
			}
			
			this.Kernel.Schedule();
		}
	}
}
