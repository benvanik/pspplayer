// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	public delegate void KernelTimerCallback( Timer timer, object state );

	partial class Kernel
	{
		public TimerQueue TimerQueue;
		private FastLinkedList<TimerCompletionEntry> _timerCompletionQueue;
		private object _timerSyncRoot;

		private void CreateTimerQueue()
		{
			Debug.Assert( TimerQueue == null );
			TimerQueue = new TimerQueue();
			_timerCompletionQueue = new FastLinkedList<TimerCompletionEntry>();
			_timerSyncRoot = new object();
		}

		private void DestroyTimerQueue()
		{
			lock( _timerSyncRoot )
				_timerCompletionQueue.Clear();
			Debug.Assert( TimerQueue != null );
			TimerQueue.Dispose();
			TimerQueue = null;
		}

		public Timer AddOneShotTimer( KernelTimerCallback timerCallback, object state, uint dueTime )
		{
			Timer timer = TimerQueue.CreateOneShotTimer( TimerCallback, dueTime, TimerExecutionContext.TimerThread, false );
			Debug.Assert( timer != null );
			timer.State = new object[] { timerCallback, state };
			return timer;
		}

		private void TimerCallback( Timer timer )
		{
			KernelTimerCallback realCallback = ( KernelTimerCallback )( ( object[] )timer.State )[ 0 ];
			object realState = ( ( object[] )timer.State )[ 1 ];
			lock( _timerSyncRoot )
				_timerCompletionQueue.Enqueue( new TimerCompletionEntry( timer, realCallback, realState ) );
			this.Cpu.BreakExecution();
		}

		private class TimerCompletionEntry
		{
			public readonly Timer Timer;
			public readonly KernelTimerCallback Callback;
			public readonly object State;
			public TimerCompletionEntry( Timer timer, KernelTimerCallback callback, object state )
			{
				this.Timer = timer;
				this.Callback = callback;
				this.State = state;
			}
		}

		private void HandleCompletedTimers()
		{
			LinkedListEntry<TimerCompletionEntry> e = null;
			lock( _timerSyncRoot )
			{
				if( _timerCompletionQueue.Count == 0 )
					return;
				e = _timerCompletionQueue.HeadEntry;
				_timerCompletionQueue.Clear();
			}
			while( e != null )
			{
				e.Value.Callback( e.Value.Timer, e.Value.State );
				e = e.Next;
			}
		}

		// Needs to support the queue like one shot
		//public Timer AddPeriodicTimer( TimerCallback timerCallback, object state, uint dueTime, uint period )
		//{
		//    Timer timer = TimerQueue.CreatePeriodicTimer( timerCallback, dueTime, period, TimerExecutionContext.TimerThread, false );
		//    Debug.Assert( timer != null );
		//    timer.State = state;
		//    return timer;
		//}

		public void CancelTimer( Timer timer )
		{
			Debug.Assert( timer != null );
			TimerQueue.StopTimer( timer );
		}
	}
}
