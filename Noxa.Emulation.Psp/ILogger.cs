// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
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

	/// <summary>
	/// Interface for runtime logging.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="value">The line to write.</param>
		void WriteLine( Verbosity verbosity, Feature feature, string value );
	}
}
