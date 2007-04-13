// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Event arguments for <see cref="Breakpoint"/> events.
	/// </summary>
	public class BreakpointEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="Breakpoint"/> the event is about.
		/// </summary>
		public readonly Breakpoint Breakpoint;

		/// <summary>
		/// Initializes a new <see cref="BreakpointEventArgs"/> instance with the given parameters.
		/// </summary>
		/// <param name="breakpoint">The <see cref="Breakpoint"/> the event is about.</param>
		public BreakpointEventArgs( Breakpoint breakpoint )
		{
			Debug.Assert( breakpoint != null );
			if( breakpoint == null )
				throw new ArgumentNullException( "breakpoint" );
			this.Breakpoint = breakpoint;
		}
	}
}
