// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Singleton logging helper.
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// The global logger instance.
		/// </summary>
		public static ILogger Instance;

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="value">The line to write.</param>
		public static void WriteLine( Feature feature, string value )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( Verbosity.Normal, feature, value );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		public static void WriteLine( Feature feature, string format, object arg0 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( Verbosity.Normal, feature, string.Format( format, arg0 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		public static void WriteLine( Feature feature, string format, object arg0, object arg1 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( Verbosity.Normal, feature, string.Format( format, arg0, arg1 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <param name="arg2">The third object to format.</param>
		public static void WriteLine( Feature feature, string format, object arg0, object arg1, object arg2 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( Verbosity.Normal, feature, string.Format( format, arg0, arg1, arg2 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="args">An object array containing items to format.</param>
		public static void WriteLine( Feature feature, string format, params object[] args )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( Verbosity.Normal, feature, string.Format( format, args ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="value">The line to write.</param>
		public static void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, value );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		public static void WriteLine( Verbosity verbosity, Feature feature, string format, object arg0 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, string.Format( format, arg0 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		public static void WriteLine( Verbosity verbosity, Feature feature, string format, object arg0, object arg1 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, string.Format( format, arg0, arg1 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <param name="arg2">The third object to format.</param>
		public static void WriteLine( Verbosity verbosity, Feature feature, string format, object arg0, object arg1, object arg2 )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, string.Format( format, arg0, arg1, arg2 ) );
		}

		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="format">A <see cref="String"/> containg one or more format items.</param>
		/// <param name="args">An object array containing items to format.</param>
		public static void WriteLine( Verbosity verbosity, Feature feature, string format, params object[] args )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, string.Format( format, args ) );
		}
	}
}
