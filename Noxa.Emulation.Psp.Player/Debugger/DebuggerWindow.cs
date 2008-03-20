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
using WeifenLuo.WinFormsUI.Docking;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class DebuggerWindow : Form
	{
		public readonly InprocDebugger Debugger;

		public DebuggerWindow()
		{
			InitializeComponent();
		}

		public DebuggerWindow( InprocDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
		}

		public DockPanel DockPanel { get { return this.dockPanel; } }

		private void exitToolsStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}
