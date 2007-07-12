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
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Debugging.Protocol
{
	/// <summary>
	/// The debugger instance.
	/// </summary>
	public interface IDebugger
	{
		/// <summary>
		/// The logger instance.
		/// </summary>
		ILogger Logger
		{
			get;
		}

		/// <summary>
		/// The debugger inspection interface.
		/// </summary>
		IDebugHandler Handler
		{
			get;
		}

		/// <summary>
		/// Setup the debugger to run the given game.
		/// </summary>
		/// <param name="game">Game selected.</param>
		/// <param name="bootStream">Bootstream game is loaded from.</param>
		void OnStarted( GameInformation game, Stream bootStream );

		/// <summary>
		/// Clean up the debugger and get ready for another run.
		/// </summary>
		void OnStopped();

		/// <summary>
		/// Allocate a new unique ID for breakpoints/etc.
		/// </summary>
		/// <returns>A unique ID.</returns>
		int AllocateID();
	}
}
