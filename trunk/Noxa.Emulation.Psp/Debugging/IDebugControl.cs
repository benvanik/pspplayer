// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging
{
	public interface IDebugControl
	{
		Breakpoint[] Breakpoints
		{
			get;
		}

		event EventHandler<BreakpointEventArgs> BreakpointAdded;
		event EventHandler<BreakpointEventArgs> BreakpointRemoved;
		event EventHandler<BreakpointEventArgs> BreakpointToggled;

		Breakpoint AddBreakpoint( int address );
		Breakpoint RemoveBreakpoint( int address );
		Breakpoint FindBreakpoint( int address );

		void Run();
		void RunUntil( int address );
		void Step();
		void StepInto();
		void StepOver();
		void StepOut();

		void Break();
	}
}
