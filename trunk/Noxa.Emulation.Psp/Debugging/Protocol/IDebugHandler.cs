// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.Protocol
{
	/// <summary>
	/// Debugger inspection control.
	/// </summary>
	public interface IDebugHandler
	{
		/// <summary>
		/// Handle a stop after a step operation.
		/// </summary>
		/// <param name="address">The address of the next instruction that will be executed.</param>
		void OnStepComplete( int address );

		/// <summary>
		/// Handle a breakpoint that has been hit.
		/// </summary>
		/// <param name="id">The breakpoint ID hit.</param>
		void OnBreakpointHit( int id );

		/// <summary>
		/// Handle an event that has been hit.
		/// </summary>
		/// <param name="biosEvent">The event that occured.</param>
		void OnEvent( Event biosEvent );

		/// <summary>
		/// Handle an error.
		/// </summary>
		/// <param name="error">The error description instance.</param>
		/// <returns><c>true</c> if the error was handled, otherwise <c>false</c>.</returns>
		bool OnError( Error error );
	}
}
