// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// Debugger control interface.
	/// </summary>
	public interface IDebugControl
	{
		/// <summary>
		/// Get a list of breakpoints.
		/// </summary>
		Breakpoint[] Breakpoints
		{
			get;
		}

		/// <summary>
		/// Fired after a breakpoint has been added.
		/// </summary>
		event EventHandler<BreakpointEventArgs> BreakpointAdded;

		/// <summary>
		/// Fired after a breakpoint has been removed.
		/// </summary>
		event EventHandler<BreakpointEventArgs> BreakpointRemoved;

		/// <summary>
		/// Fired after a breakpoint has been toggled.
		/// </summary>
		event EventHandler<BreakpointEventArgs> BreakpointToggled;

		/// <summary>
		/// Add a new breakpoint at the given address.
		/// </summary>
		/// <param name="address">Address to add breakpoint at.</param>
		/// <returns>A new breakpoint instance or an existing instance if one had been set.</returns>
		Breakpoint AddBreakpoint( int address );
		
		/// <summary>
		/// Remove an existing breakpoint at the given address.
		/// </summary>
		/// <param name="address">Address to remove breakpoint from.</param>
		/// <returns>The existing breakpoint instance or <c>null</c> if no breakpoint was set.</returns>
		Breakpoint RemoveBreakpoint( int address );

		/// <summary>
		/// Lookup a breakpoint instance.
		/// </summary>
		/// <param name="address">Address to search for a breakpoint at.</param>
		/// <returns>The breakpoint at the given address or <c>null</c> if no breakpoint was set.</returns>
		Breakpoint FindBreakpoint( int address );

		/// <summary>
		/// Resume CPU execution.
		/// </summary>
		void Run();

		/// <summary>
		/// Break CPU execution.
		/// </summary>
		void Break();

		void RunUntil( int address );
		void Step();
		void StepInto();
		void StepOver();
		void StepOut();
	}
}
