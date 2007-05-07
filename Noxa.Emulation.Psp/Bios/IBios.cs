// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// BIOS/Operating System.
	/// </summary>
	public interface IBios : IComponentInstance
	{
		/// <summary>
		/// The <see cref="ILoader"/> instance that controls loading of games and modules.
		/// </summary>
		ILoader Loader
		{
			get;
		}

		/// <summary>
		/// A list of modules contained within the BIOS.
		/// </summary>
		BiosModule[] Modules
		{
			get;
		}

		/// <summary>
		/// A list of functions contained within the BIOS.
		/// </summary>
		BiosFunction[] Functions
		{
			get;
		}

		/// <summary>
		/// Find a <see cref="BiosModule"/> by name.
		/// </summary>
		/// <param name="name">The name of the module to look for.</param>
		/// <returns>The <see cref="BiosModule"/> corresponding to the given <paramref name="name"/> or <c>null</c> if it was not found.</returns>
		BiosModule FindModule( string name );

		/// <summary>
		/// Find a <see cref="BiosFunction"/> by NID.
		/// </summary>
		/// <param name="nid">The unique ID to look for.</param>
		/// <returns>The <see cref="BiosFunction"/> corresponding to the given <paramref name="nid"/> or <c>null</c> if it was not found.</returns>
		BiosFunction FindFunction( uint nid );

		/// <summary>
		/// The current game loaded by the BIOS.
		/// </summary>
		GameInformation Game
		{
			get;
			set;
		}

		/// <summary>
		/// The boot stream of the current game loaded by the BIOS.
		/// </summary>
		Stream BootStream
		{
			get;
			set;
		}

		/// <summary>
		/// Load the current game.
		/// </summary>
		/// <returns>The <see cref="LoadResults"/> of the loader.</returns>
		/// <remarks>
		/// If no game is currently set, this method will block until one is.
		/// </remarks>
		LoadResults Load();

		/// <summary>
		/// Wait until the game is loaded.
		/// </summary>
		void WaitUntilLoaded();

		/// <summary>
		/// Run the BIOS scheduler and execute code.
		/// </summary>
		void Execute();

		#region Debugging

		/// <summary>
		/// <c>true</c> if a debugger has been attached.
		/// </summary>
		bool DebuggingEnabled
		{
			get;
		}

		/// <summary>
		/// The currently attached debugger.
		/// </summary>
		IDebugger Debugger
		{
			get;
		}

		/// <summary>
		/// The debug hook that allows for inspection.
		/// </summary>
		IBiosHook DebugHook
		{
			get;
		}

		/// <summary>
		/// Enable debugging support.
		/// </summary>
		/// <param name="debugger">The <see cref="IDebugger"/> instance to attach to.</param>
		void EnableDebugging( IDebugger debugger );

		#endregion
	}
}
