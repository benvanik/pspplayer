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
			State = KThreadState.Ready;
			Kernel.Cpu.SetContextRegister( ContextID, 4, ( uint )returnValue );

			WakeupCount++;

			this.AddToSchedule();
		}

		public void ReleaseWait()
		{
			State = KThreadState.Ready;

			ReleaseCount++;

			this.AddToSchedule();
		}

		public void Suspend()
		{
			Suspended = true;

			this.RemoveFromSchedule();
		}

		public void Resume()
		{
			Suspended = false;

			this.AddToSchedule();
		}

		public void Sleep( bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			WaitingOn = KThreadWait.Sleep;
		}

		private void DelayCallback( Timer timer )
		{
			State = KThreadState.Ready;
			ReturnValue = 0;

			this.AddToSchedule();
			
			// We cannot schedule here - in a weird thread
			Kernel.Cpu.BreakExecution();
		}

		public void Delay( uint waitTimeUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			Debug.Assert( waitTimeUs > 0 );

			CanHandleCallbacks = canHandleCallbacks;

			WaitingOn = KThreadWait.Delay;
			NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
			WaitTimeout = waitTimeUs * 10;	// us -> ticks

			// Install timer
			Kernel.AddOneShotTimer( new TimerCallback( this.DelayCallback ), this, waitTimeUs / 1000 );
		}

		private void JoinCallback( Timer timer )
		{
			State = KThreadState.Ready;
			ReturnValue = -1;

			this.AddToSchedule();
			
			// We cannot schedule here - in a weird thread
			Kernel.Cpu.BreakExecution();
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

				// Install timer
				Kernel.AddOneShotTimer( new TimerCallback( this.JoinCallback ), this, timeoutUs / 1000 );
			}
			else
				WaitTimeout = 0;
			WaitHandle = targetThread;

			targetThread.ExitWaiters.Enqueue( this );
		}

		private void WaitCallback( Timer timer )
		{
			// If we have not been made ready already, we wake
			if( State == KThreadState.Waiting )
			{
				State = KThreadState.Ready;
				Kernel.Cpu.SetContextRegister( ContextID, 4, unchecked( ( uint )-1 ) );

				this.AddToSchedule();

				// We cannot schedule here - in a weird thread
				Kernel.Cpu.BreakExecution();
			}
		}

		public void Wait( KEvent ev, KWaitType waitEventMode, uint userValue, uint outAddress, uint timeoutUs, bool canHandleCallbacks )
		{
			State = KThreadState.Waiting;
			this.RemoveFromSchedule();

			CanHandleCallbacks = canHandleCallbacks;

			ev.WaitingThreads.Enqueue( this );

			WaitingOn = KThreadWait.Event;
			if( timeoutUs > 0 )
			{
				NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
				WaitTimeout = timeoutUs * 10;	// us -> ticks

				// Install timer
				Kernel.AddOneShotTimer( new TimerCallback( this.WaitCallback ), this, timeoutUs / 1000 );
			}
			else
				WaitTimeout = 0;
			WaitHandle = ev;
			WaitEventMode = waitEventMode;
			WaitArgument = userValue;
			WaitAddress = outAddress;
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
			if( timeoutUs > 0 )
			{
				NativeMethods.QueryPerformanceCounter( out WaitTimestamp );
				WaitTimeout = timeoutUs * 10;	// us -> ticks

				// Install timer
				Kernel.AddOneShotTimer( new TimerCallback( this.WaitCallback ), this, timeoutUs / 1000 );
			}
			else
				WaitTimeout = 0;
			WaitHandle = pool;
			WaitAddress = pdata;
		}
	}
}
