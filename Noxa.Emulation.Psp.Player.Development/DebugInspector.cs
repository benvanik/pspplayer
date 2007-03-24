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

namespace Noxa.Emulation.Psp.Player.Development
{
	class DebugInspector : IDebugInspector
	{
		private Debugger _debugger;

		public DebugInspector( Debugger debugger )
		{
			Debug.Assert( debugger != null );
			if( debugger == null )
				throw new ArgumentNullException( "debugger" );
			_debugger = debugger;
		}

		public void OnStepComplete( int address )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void OnBreakpointHit( int address )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void OnCpuError( int address, CpuError error )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void OnBiosError( int address, BiosError error )
		{
			throw new Exception( "The method or operation is not implemented." );
		}
	}
}
