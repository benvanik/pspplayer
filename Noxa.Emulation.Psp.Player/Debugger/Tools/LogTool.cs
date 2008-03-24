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
			this.InitializeComponent();
		}

		public LogTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.OutputIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );
		}

		public void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			// if log to vs
			if( false )
			{
				Debug.WriteLine( string.Format( "{0}: {1}", feature, value ) );
			}
			this.logControl.AddLine( verbosity, feature, value );
		}
	}
}
