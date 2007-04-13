// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// Describes the type of debug database.
	/// </summary>
	public enum DebugDataType
	{
		/// <summary>
		/// Full ELF debug information.
		/// </summary>
		Elf,

		/// <summary>
		/// Minimal ELF debug information, consisting only of symbols.
		/// </summary>
		Symbols,
	}

	/// <summary>
	/// Describes the state of the debugger.
	/// </summary>
	public enum DebuggerState
	{
		/// <summary>
		/// The debugger is running code.
		/// </summary>
		Running,

		/// <summary>
		/// The debugger is paused at a breakpoint or on an error.
		/// </summary>
		Paused,

		/// <summary>
		/// The debugger has handled a crash in the executable.
		/// </summary>
		Crashed,
	}

	/// <summary>
	/// The debugger instance.
	/// </summary>
	public interface IDebugger
	{
		/// <summary>
		/// The host that owns this debugger instance.
		/// </summary>
		IEmulationHost Host
		{
			get;
		}

		/// <summary>
		/// <c>true</c> when the debugger is attached to the executing instance.
		/// </summary>
		bool IsAttached
		{
			get;
		}

		/// <summary>
		/// The debugger control interface.
		/// </summary>
		IDebugControl Control
		{
			get;
		}

		/// <summary>
		/// The debugger inspection interface.
		/// </summary>
		IDebugInspector Inspector
		{
			get;
		}

		/// <summary>
		/// The hook inside the CPU that allows information gathering.
		/// </summary>
		ICpuHook CpuHook
		{
			get;
			set;
		}

		/// <summary>
		/// The hook inside the BIOS that allows information gathering.
		/// </summary>
		IBiosHook BiosHook
		{
			get;
			set;
		}

		/// <summary>
		/// Currently loaded program debug information.
		/// </summary>
		IProgramDebugData DebugData
		{
			get;
		}

		/// <summary>
		/// The state of the debugger.
		/// </summary>
		DebuggerState State
		{
			get;
		}

		/// <summary>
		/// Setup the debugger to run the given game.
		/// </summary>
		/// <param name="game">Game selected.</param>
		/// <param name="bootStream">Bootstream game is loaded from.</param>
		void SetupGame( GameInformation game, Stream bootStream );

		/// <summary>
		/// Load debug information of the given type from the target stream.
		/// </summary>
		/// <param name="dataType">The type of the debug information contained within the given stream.</param>
		/// <param name="stream">Data stream containing debug information.</param>
		/// <returns><c>true</c> if the debug information was loaded successfully; otherwise <c>false</c>.</returns>
		bool LoadDebugData( DebugDataType dataType, Stream stream );

		/// <summary>
		/// Show the debugger UI.
		/// </summary>
		void Show();

		/// <summary>
		/// Hide the debugger UI.
		/// </summary>
		void Hide();
	}
}
