// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugData;

namespace Noxa.Emulation.Psp.Debugging
{
	#pragma warning disable 1591

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

		public ICpuHook CpuHook
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IBiosHook BiosHook
		{
			get
			{
				return null;
			}
			set
			{
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

		public void SetupGame( Noxa.Emulation.Psp.Games.GameInformation game, System.IO.Stream bootStream )
		{
			throw new Exception( "The method or operation is not implemented." );
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

	#pragma warning restore
}
