// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Debugging
{
	public class BreakpointEventArgs : EventArgs
	{
		protected Breakpoint _breakpoint;

		public BreakpointEventArgs( Breakpoint breakpoint )
		{
			Debug.Assert( breakpoint != null );
			if( breakpoint == null )
				throw new ArgumentNullException( "breakpoint" );
			_breakpoint = breakpoint;
		}

		public Breakpoint Breakpoint
		{
			get
			{
				return _breakpoint;
			}
		}
	}
}
