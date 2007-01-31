// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Player.Development
{
	class BasicBreakpoint : Breakpoint
	{
		protected DebugControl _control;

		public BasicBreakpoint( DebugControl control, BreakpointType type, int address )
			: base( type, address )
		{
			Debug.Assert( control != null );
			if( control == null )
				throw new ArgumentNullException( "control" );
			_control = control;
		}

		protected override void OnEnabledChanged()
		{
			_control.OnBreakpointToggled( this );
			base.OnEnabledChanged();
		}
	}
}
