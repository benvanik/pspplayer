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
	partial class Kernel
	{
		public TimerQueue TimerQueue;

		private void CreateTimerQueue()
		{
			Debug.Assert( TimerQueue == null );
			TimerQueue = new TimerQueue();
		}

		private void DestroyTimerQueue()
		{
			Debug.Assert( TimerQueue != null );
			TimerQueue.Dispose();
			TimerQueue = null;
		}

		public Timer AddOneShotTimer( TimerCallback timerCallback, object state, uint dueTime )
		{
			Timer timer = TimerQueue.CreateOneShotTimer( timerCallback, dueTime, TimerExecutionContext.TimerThread, false );
			Debug.Assert( timer != null );
			timer.State = state;
			return timer;
		}

		public Timer AddPeriodicTimer( TimerCallback timerCallback, object state, uint dueTime, uint period )
		{
			Timer timer = TimerQueue.CreatePeriodicTimer( timerCallback, dueTime, period, TimerExecutionContext.TimerThread, false );
			Debug.Assert( timer != null );
			timer.State = state;
			return timer;
		}

		public void CancelTimer( Timer timer )
		{
			Debug.Assert( timer != null );
			TimerQueue.StopTimer( timer );
		}
	}
}
