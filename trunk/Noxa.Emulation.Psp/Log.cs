// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define LOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp
{
	#region Enumerations

	/// <summary>
	/// Defines the level of logging.
	/// </summary>
	public enum Verbosity
	{
		/// <summary>
		/// Only critical messages will be logged.
		/// </summary>
		Critical = 0,
		/// <summary>
		/// Critical and some other import messages will be logged.
		/// </summary>
		Normal = 1,
		/// <summary>
		/// Extra debugging info will be logged.
		/// </summary>
		Verbose = 2,
		/// <summary>
		/// Everything will be logged.
		/// </summary>
		Everything = 3,
	}

	/// <summary>
	/// Defines the feature a log message pertains to.
	/// </summary>
	public enum Feature
	{
		/// <summary>
		/// General emulation information, such as run state/etc.
		/// </summary>
		General,
		/// <summary>
		/// Information pertaining to the audio system.
		/// </summary>
		Audio,
		/// <summary>
		/// Information pertaining to the BIOS.
		/// </summary>
		Bios,
		/// <summary>
		/// Information pertaining to the CPU.
		/// </summary>
		Cpu,
		/// <summary>
		/// Information pertaining to the game/module loader.
		/// </summary>
		Loader,
		/// <summary>
		/// Information pertaining to the input system.
		/// </summary>
		Input,
		/// <summary>
		/// Information pertaining to the media (MemoryStick/UMD) systems.
		/// </summary>
		Media,
		/// <summary>
		/// Information pertaining to the networking systems.
		/// </summary>
		Net,
		/// <summary>
		/// Information pretaining to the video system.
		/// </summary>
		Video,
		/// <summary>
		/// Statistical information.
		/// </summary>
		Statistics,
		/// <summary>
		/// STDOUT/STDERR output from the game.
		/// </summary>
		Stdout,
		/// <summary>
		/// Trace of syscalls and their parameters.
		/// </summary>
		Syscall,
	}

	#endregion

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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
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
		[Conditional( "LOG" )]
		public static void WriteLine( Verbosity verbosity, Feature feature, string format, params object[] args )
		{
			Debug.Assert( Instance != null );
			Instance.WriteLine( verbosity, feature, string.Format( format, args ) );
		}
	}
}
