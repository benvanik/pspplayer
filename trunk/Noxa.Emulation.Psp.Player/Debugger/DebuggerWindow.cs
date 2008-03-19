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

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class DebuggerWindow : Form
	{
		public DebuggerWindow()
		{
			InitializeComponent();
		}

		private void ExitToolsStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		private void CutToolStripMenuItem_Click( object sender, EventArgs e )
		{
		}

		private void CopyToolStripMenuItem_Click( object sender, EventArgs e )
		{
		}

		private void PasteToolStripMenuItem_Click( object sender, EventArgs e )
		{
		}
	}
}
