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
	public class NullDebugger : IDebugger
	{
		private IEmulationHost _host;

		public NullDebugger( IEmulationHost host )
		{
			_host = host;
		}

		public IEmulationHost Host
		{
			get
			{
				return _host;
			}
		}

		public bool IsAttached
		{
			get
			{
				return false;
			}
		}

		public IDebugControl Control
		{
			get
			{
				return null;
			}
		}

		public IDebugInspector Inspector
		{
			get
			{
				return null;
			}
		}

		public IProgramDebugData DebugData
		{
			get
			{
				return null;
			}
		}

		public DebuggerState State
		{
			get
			{
				return DebuggerState.Running;
			}
		}

		public bool LoadDebugData( DebugDataType dataType, System.IO.Stream stream )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Show()
		{
		}

		public void Hide()
		{
		}
	}
}
