// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Noxa
{
	/// <summary>
	/// Utility class for dealing with native OS events.
	/// </summary>
	public static class NativeEvent
	{
		#region Interop

		private static class NativeMethods
		{
			[DllImport( "kernel32.dll" )]
			public static extern IntPtr CreateEvent( IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName );

			[DllImport( "kernel32.dll", SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool CloseHandle( IntPtr hObject );

			[DllImport( "kernel32.dll" )]
			public static extern bool SetEvent( IntPtr hEvent );

			[DllImport( "kernel32.dll" )]
			public static extern bool ResetEvent( IntPtr hEvent );

			[DllImport( "kernel32.dll" )]
			public static extern bool PulseEvent( IntPtr hEvent );

			[DllImport( "kernel32.dll", SetLastError = true, ExactSpelling = true )]
			public static extern uint WaitForSingleObject( IntPtr handle, uint milliseconds );
		}

		#endregion

		/// <summary>
		/// Create a new native event with the given properties.
		/// </summary>
		/// <param name="autoReset"><c>true</c> to automatically unsignal the event after signalling.</param>
		/// <param name="initialState">The initial signal state of the event.</param>
		/// <returns>The handle of the native event or <c>IntPtr.Zero</c> if creation failed.</returns>
		public static IntPtr CreateEvent( bool autoReset, bool initialState )
		{
			IntPtr handle = NativeMethods.CreateEvent( IntPtr.Zero, !autoReset, initialState, null );
			Debug.Assert( handle != IntPtr.Zero );
			return handle;
		}

		/// <summary>
		/// Delete a previous created native event.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		public static void DeleteEvent( IntPtr handle )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.CloseHandle( handle );
		}

		/// <summary>
		/// Signal a native event. If it is an auto-reset event, one waiter is woken and the event is unset;
		/// otherwise all waiters are woken and the event remains signalled.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		public static void Set( IntPtr handle )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.SetEvent( handle );
		}

		/// <summary>
		/// Unset a signalled native event.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		public static void Reset( IntPtr handle )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.ResetEvent( handle );
		}

		/// <summary>
		/// Set and unset a native event. If it is an auto-reset event, one waiter is woken; otherwise all waiters
		/// are woken.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		public static void Pulse( IntPtr handle )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.PulseEvent( handle );
		}

		/// <summary>
		/// Wait on the given native event until it is set/pulsed.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		public static void Wait( IntPtr handle )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.WaitForSingleObject( handle, 0xFFFFFFFF );
		}

		/// <summary>
		/// Wait on the given native event until it is set/pulsed or the timeout elapses.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		/// <param name="timeoutMs">The time, in milliseconds, to wait until aborting the wait.</param>
		/// <returns><c>true</c> if the event was signalled before the timeout elapsed.</returns>
		public static bool Wait( IntPtr handle, uint timeoutMs )
		{
			Debug.Assert( handle != IntPtr.Zero );
			if( handle == IntPtr.Zero )
				return false;
			uint ret = NativeMethods.WaitForSingleObject( handle, timeoutMs );
			if( ret == 0 )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Create an <see cref="AutoResetEvent"/> wrapper around the given native event.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		/// <returns>An <see cref="AutoResetEvent"/> wrapping the given <paramref name="handle"/>.</returns>
		public static AutoResetEvent GetAutoResetEvent( IntPtr handle )
		{
			AutoResetEvent ev = new AutoResetEvent( false );
			ev.Close();
			GC.ReRegisterForFinalize( ev );
			ev.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle( handle, false );
			return ev;
		}

		/// <summary>
		/// Create a <see cref="ManualResetEvent"/> wrapper around the given native event.
		/// </summary>
		/// <param name="handle">The handle of the native event.</param>
		/// <returns>A <see cref="ManualResetEvent"/> wrapping the given <paramref name="handle"/>.</returns>
		public static ManualResetEvent GetManualResetEvent( IntPtr handle )
		{
			ManualResetEvent ev = new ManualResetEvent( false );
			ev.Close();
			GC.ReRegisterForFinalize( ev );
			ev.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle( handle, false );
			return ev;
		}
	}
}
