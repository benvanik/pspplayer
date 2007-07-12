// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Singleton debug helper.
	/// </summary>
	public static class Diag
	{
		/// <summary>
		/// The local debugger instance.
		/// </summary>
		public static DebugHost Instance;

		/// <summary>
		/// <c>true</c> if the debugger is attached.
		/// </summary>
		public static bool IsAttached
		{
			get
			{
				if( Instance == null )
					return false;
				return Instance.IsAttached;
			}
		}

		/// <summary>
		/// Ensure that the debugger is attached; if it's not, a dialog will be presented to the user.
		/// </summary>
		/// <param name="message">Information displayed to the user about why the debugger is being requested.</param>
		/// <returns><c>true</c> if the debugger is attached, otherwise <c>false</c>.</returns>
		public static bool EnsureAttached( string message )
		{
			if( IsAttached == false )
			{
				// We should at least have a debug host!
				Debug.Assert( Instance != null );

				// Ask the user what to do
				if( Instance.Emulator.AskForDebugger( message ) == false )
				{
					// They said no... fuck em
					return false;
				}

				// Wait until the debugger attaches
				Instance.WaitUntilAttached();
			}

			return true;
		}

		/// <summary>
		/// Throw an error to the debugger.
		/// </summary>
		/// <param name="error">The <see cref="Error"/> to throw.</param>
		/// <returns><c>true</c> if the debugger has handled the error, <c>false</c> to continue on if it was ignored.</returns>
		public static bool ThrowError( Error error )
		{
			if( EnsureAttached( error.ToString() ) == false )
				return false;
			return Instance.Client.Handler.OnError( error );
		}

		/// <summary>
		/// Attempt to extract the callstack if frame tracking is enabled.
		/// </summary>
		/// <returns>The callstack or <c>null</c> if frame tracking is disabled.</returns>
		public static Frame[] GetCallstack()
		{
			return Instance.CpuHook.GetCallstack();
		}
	}
}
