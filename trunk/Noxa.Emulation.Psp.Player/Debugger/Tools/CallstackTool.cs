// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CallstackTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		public CallstackTool()
		{
			InitializeComponent();
		}

		public CallstackTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();
		}

		public void RefreshCallstack()
		{
			Frame[] frames = this.Debugger.DebugHost.CpuHook.GetCallstack();
			System.Diagnostics.Debug.WriteLine( "CALLSTACK:" );
			foreach( Frame frame in frames )
				System.Diagnostics.Debug.WriteLine( string.Format( "   0x{0:X8} {1}", frame.Address, frame.Type ) );
		}
	}
}
