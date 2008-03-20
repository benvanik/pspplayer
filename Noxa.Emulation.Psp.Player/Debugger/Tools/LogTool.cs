// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class LogTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool, ILogger
	{
		public LogTool()
		{
			InitializeComponent();
		}

		public LogTool( InprocDebugger debugger )
			: base( debugger )
		{
			InitializeComponent();
		}

		public void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			Debug.WriteLine( string.Format( "{0}: {1}", feature, value ) );
		}
	}
}
