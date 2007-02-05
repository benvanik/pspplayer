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

		private Stack<CallstackFrame> _callstack;

		public DebugInspector( Debugger debugger )
		{
			Debug.Assert( debugger != null );
			if( debugger == null )
				throw new ArgumentNullException( "debugger" );
			_debugger = debugger;

			_callstack = new Stack<CallstackFrame>();
		}

		public void Update( int newAddress )
		{
			Debug.WriteLine( string.Format( "updated: {0:X8}", newAddress ) );
		}

		#region Callstacks

		public CallstackFrame[] Callstack
		{
			get
			{
				return _callstack.ToArray();
			}
		}

		public void PushCall( int address, string name )
		{
			BasicCallstackFrame frame = new BasicCallstackFrame( address, name );

			_callstack.Push( frame );
		}

		public void PopCall()
		{
			if( _callstack.Count == 0 )
			{
				Debug.WriteLine( "DebugInspector: callstack pop count mismatch" );
				return;
			}

			_callstack.Pop();
		}

		#endregion
	}
}
