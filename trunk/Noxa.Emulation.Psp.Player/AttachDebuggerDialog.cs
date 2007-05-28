// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player
{
	partial class AttachDebuggerDialog : Form
	{
		public AttachDebuggerDialog()
		{
			InitializeComponent();

			this.DialogResult = DialogResult.Cancel;
		}

		public AttachDebuggerDialog( string message )
			: this()
		{
			this.Icon = Icon.FromHandle( ( ( Bitmap )Properties.Resources.ErrorIcon ).GetHicon() );

			this.infoTextBox.Text = message;
		}

		private void copyButton_Click( object sender, EventArgs e )
		{
			// Need STAThread
			//Clipboard.Clear();
			//Clipboard.SetText( this.infoTextBox.Text );
		}
	}
}