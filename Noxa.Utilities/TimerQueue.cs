// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Noxa
{
	#region Timer Instance

	/// <summary>
	/// Indicates the mode of a <see cref="Timer"/>.
	/// </summary>
	public enum TimerMode
	{
		/// <summary>
		/// Indicates that the timer will execute only once.
		/// </summary>
		OneShot = 0x00000008,
		/// <summary>
		/// Indicates that the timer will execute multiple times.
		/// </summary>
		Periodic = 0x0,
	}

	/// <summary>
	/// Indicates the thread that the <see cref="Timer"/> callback will be executed in.
	/// </summary>
	public enum TimerExecutionContext
	{
		/// <summary>
		/// Queued on a non-I/O worker thread (persistent).
		/// </summary>
		/// <remarks>
		/// This maps to <c>WT_EXECUTEINPERSISTENTTHREAD</c>.
		/// </remarks>
		WorkerThread = 0x00000080,
		/// <summary>
		/// Queued on an I/O worker thread (use only with alertable I/O events).
		/// </summary>
		IOThread = 0x00000001,
		/// <summary>
		/// Execute in the timer thread (use only with fast routines that don't block for long).
		/// </summary>
		TimerThread = 0x00000020,
	}

	/// <summary>
	/// Callback delegate for <see cref="Timer"/> handling.
	/// </summary>
	/// <param name="timer">The timer that was fired.</param>
	public delegate void TimerCallback( Timer timer );

	/// <summary>
	/// An individual timer instance in a <see cref="TimerQueue"/>.
	/// </summary>
	public class Timer
	{
		internal IntPtr Handle;
		internal int Index;

		/// <summary>
		/// The <see cref="TimerQueue"/> that owns the current timer instance.
		/// </summary>
		public readonly TimerQueue Queue;

		/// <summary>
		/// The <see cref="TimerMode"/> the timer is operating in.
		/// </summary>
		public readonly TimerMode Mode;

		/// <summary>
		/// The <see cref="TimerExecutionContext"/> the timer is executing in.
		/// </summary>
		public readonly TimerExecutionContext ExecutionContext;

		/// <summary>
		/// <c>true</c> if the timer execution is expected to take a long time.
		/// </summary>
		public readonly bool IsLongRunning;

		/// <summary>
		/// The period of the timer.
		/// </summary>
		public readonly uint Period;

		/// <summary>
		/// The initial delay time of the timer.
		/// </summary>
		public readonly uint DueTime;

		/// <summary>
		/// The delegate method that handles the timer.
		/// </summary>
		public readonly TimerCallback Callback;

		/// <summary>
		/// User-defined state information.
		/// </summary>
		public object State;

		internal Timer( TimerQueue queue, TimerMode mode, TimerExecutionContext context, bool isLongRunning, uint dueTime, uint period, TimerCallback callback )
		{
			Debug.Assert( queue != null );
			Debug.Assert( callback != null );

			Queue = queue;
			Mode = mode;
			ExecutionContext = context;
			IsLongRunning = isLongRunning;
			Period = period;
			DueTime = dueTime;
			Callback = callback;
		}
	}

	#endregion

	/// <summary>
	/// A fast timer queue.
	/// </summary>
	public class TimerQueue : IDisposable
	{
		#region Interop

		private delegate void WaitOrTimerDelegate( IntPtr param, [MarshalAs( UnmanagedType.Bool )] bool timerOrWaitFired );

		private static class NativeMethods
		{
			[Flags]
			public enum TimerQueueFlags : uint
			{
				WT_EXECUTEDEFAULT = 0x00000000,
				WT_EXECUTEINIOTHREAD = 0x00000001,
				WT_EXECUTEINUITHREAD = 0x00000002,
				WT_EXECUTEINWAITTHREAD = 0x00000004,
				WT_EXECUTEONLYONCE = 0x00000008,
				WT_EXECUTEINTIMERTHREAD = 0x00000020,
				WT_EXECUTELONGFUNCTION = 0x00000010,
				WT_EXECUTEINPERSISTENTIOTHREAD = 0x00000040,
				WT_EXECUTEINPERSISTENTTHREAD = 0x00000080,
			}

			[DllImport( "kernel32.dll", SetLastError = true )]
			public static extern IntPtr CreateTimerQueue();

			[DllImport( "kernel32.dll", SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool CreateTimerQueueTimer(
				ref IntPtr phNewTimer, IntPtr TimerQueue,
				WaitOrTimerDelegate Callback, IntPtr Parameter,
				uint DueTime, uint Period, TimerQueueFlags Flags );

			[DllImport( "kernel32.dll", SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool DeleteTimerQueueEx( IntPtr TimerQueue, IntPtr CompletionHandle );

			[DllImport( "kernel32.dll", SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool DeleteTimerQueueTimer( IntPtr TimerQueue, IntPtr Timer, IntPtr CompletionEvent );
		}

		#endregion

		/// <summary>
		/// The maximum number of timers that can be allocated at any one time.
		/// </summary>
		public const int MaximumTimers = 256;

		private IntPtr _hQueue;
		private List<Timer> _timers;
		private int _timerIndex;

		/// <summary>
		/// Initialize a new <see cref="TimerQueue"/> instance.
		/// </summary>
		public TimerQueue()
		{
			_timers = new List<Timer>( MaximumTimers );
			_timerIndex = -1;

			_hQueue = NativeMethods.CreateTimerQueue();
			Debug.Assert( _hQueue != IntPtr.Zero );
			if( _hQueue == IntPtr.Zero )
			{
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception( error, "Unable to create timer queue." );
			}
		}

		/// <summary>
		/// Dispose of the resources used by the timer queue.
		/// </summary>
		/// <remarks>
		/// This just calls <see cref="TimerQueue.Dispose"/>.
		/// </remarks>
		~TimerQueue()
		{
			this.Dispose();
		}

		/// <summary>
		/// Dispose of the resources used by the timer queue.
		/// </summary>
		/// <remarks>
		/// This method will wait until all currently running timers have stopped before returning.
		/// </remarks>
		public void Dispose()
		{
			GC.SuppressFinalize( this );

			IntPtr hQueue = _hQueue;
			_hQueue = IntPtr.Zero;
			_timerIndex = -1;
			for( int n = 0; n < _timers.Count; n++ )
				_timers[ n ] = null;

			if( hQueue != IntPtr.Zero )
			{
				// -1 indicates wait until all timers have completed
				bool deleted = NativeMethods.DeleteTimerQueueEx( hQueue, new IntPtr( -1 ) );
				Debug.Assert( deleted == true );
			}
		}

		private Timer InternalCreateTimer( TimerMode mode, TimerExecutionContext context, TimerCallback callback, uint dueTime, uint period, bool isLongRunning )
		{
			Debug.Assert( _hQueue != IntPtr.Zero );
			if( _hQueue == IntPtr.Zero )
				throw new InvalidOperationException( "The timer queue has already been disposed." );

			Debug.Assert( callback != null );
			if( callback == null )
				throw new ArgumentNullException( "callback" );
			switch( mode )
			{
				case TimerMode.OneShot:
					Debug.Assert( dueTime > 0 );
					Debug.Assert( period == 0 );
					if( dueTime <= 0 )
						throw new ArgumentOutOfRangeException( "dueTime", dueTime, "One-shot timers require a due time of 1ms or more." );
					break;
				case TimerMode.Periodic:
					Debug.Assert( period > 0 );
					if( period <= 0 )
						throw new ArgumentOutOfRangeException( "period", period, "Periodic timers require a period of 1ms or more." );
					break;
			}

			NativeMethods.TimerQueueFlags flags = ( NativeMethods.TimerQueueFlags )( ( uint )mode | ( uint )context );
			if( isLongRunning == true )
				flags |= NativeMethods.TimerQueueFlags.WT_EXECUTELONGFUNCTION;

			Timer timer = new Timer( this, mode, context, isLongRunning, dueTime, period, callback );
			int index = Interlocked.Increment( ref _timerIndex );
			timer.Index = index;
			_timers[ index ] = timer;

			IntPtr handle = IntPtr.Zero;
			bool result = NativeMethods.CreateTimerQueueTimer(
				ref handle, _hQueue,
				new WaitOrTimerDelegate( this.Callback ), new IntPtr( index ),
				dueTime, period, flags );
			Debug.Assert( result == true );
			if( result == false )
			{
				_timers[ index ] = null;
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception( error, "Unable to create timer instance." );
			}

			timer.Handle = handle;
			return timer;
		}

		/// <summary>
		/// Create a new one-shot timer that will fire at the given time.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="dueTime">The time that will elapse before the timer is executed, in milliseconds.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreateOneShotTimer( TimerCallback callback, uint dueTime )
		{
			return this.InternalCreateTimer( TimerMode.OneShot, TimerExecutionContext.WorkerThread, callback, dueTime, 0, false );
		}

		/// <summary>
		/// Create a new one-shot timer that will fire at the given time.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="dueTime">The time that will elapse before the timer is executed, in milliseconds.</param>
		/// <param name="executionContext">The thread that the callback will execute in.</param>
		/// <param name="isLongRunning"><c>true</c> if the callback execution is expected to take a long time.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreateOneShotTimer( TimerCallback callback, uint dueTime, TimerExecutionContext executionContext, bool isLongRunning )
		{
			return this.InternalCreateTimer( TimerMode.OneShot, executionContext, callback, dueTime, 0, isLongRunning );
		}

		/// <summary>
		/// Create a new periodic timer that will fire at the given interval.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="period">The time that will elapse between timer executions, in milliseconds.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreatePeriodicTimer( TimerCallback callback, uint period )
		{
			return this.InternalCreateTimer( TimerMode.Periodic, TimerExecutionContext.WorkerThread, callback, period, period, false );
		}

		/// <summary>
		/// Create a new periodic timer that will fire at the given interval.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="period">The time that will elapse between timer executions, in milliseconds.</param>
		/// <param name="executionContext">The thread that the callback will execute in.</param>
		/// <param name="isLongRunning"><c>true</c> if the callback execution is expected to take a long time.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreatePeriodicTimer( TimerCallback callback, uint period, TimerExecutionContext executionContext, bool isLongRunning )
		{
			return this.InternalCreateTimer( TimerMode.Periodic, executionContext, callback, period, period, isLongRunning );
		}

		/// <summary>
		/// Create a new periodic timer that will fire at the given interval after the <paramref name="dueTime"/> has passed.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="dueTime">The time that will elapse before the timer is first executed, in milliseconds.</param>
		/// <param name="period">The time that will elapse between timer executions, in milliseconds.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreatePeriodicTimer( TimerCallback callback, uint dueTime, uint period )
		{
			return this.InternalCreateTimer( TimerMode.Periodic, TimerExecutionContext.WorkerThread, callback, dueTime, period, false );
		}

		/// <summary>
		/// Create a new periodic timer that will fire at the given interval after the <paramref name="dueTime"/> has passed.
		/// </summary>
		/// <param name="callback">The method that will handle the timer execution.</param>
		/// <param name="dueTime">The time that will elapse before the timer is first executed, in milliseconds.</param>
		/// <param name="period">The time that will elapse between timer executions, in milliseconds.</param>
		/// <param name="executionContext">The thread that the callback will execute in.</param>
		/// <param name="isLongRunning"><c>true</c> if the callback execution is expected to take a long time.</param>
		/// <returns>A new <see cref="Timer"/> instance with the given parameters.</returns>
		public Timer CreatePeriodicTimer( TimerCallback callback, uint dueTime, uint period, TimerExecutionContext executionContext, bool isLongRunning )
		{
			return this.InternalCreateTimer( TimerMode.Periodic, executionContext, callback, dueTime, period, isLongRunning );
		}

		/// <summary>
		/// Stop the given timer from executing.
		/// </summary>
		/// <param name="timer">The <see cref="Timer"/> to stop.</param>
		/// <remarks>
		/// This method waits until the timer, if it was currently executing, completes.
		/// </remarks>
		public void StopTimer( Timer timer )
		{
			Debug.Assert( timer != null );
			if( timer == null )
				throw new ArgumentNullException( "timer" );

			// This will kill the timer if it is about to run
			_timers[ timer.Index ] = null;

			bool result = NativeMethods.DeleteTimerQueueTimer( _hQueue, timer.Handle, new IntPtr( -1 ) );
			if( result == false )
			{
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception( error );
			}
		}

		private void Callback( IntPtr param, bool timerOrWaitFired )
		{
			Timer timer = _timers[ param.ToInt32() ];
			Debug.Assert( timer != null );
			if( timer == null )
				return;

			timer.Callback( timer );
		}
	}
}
