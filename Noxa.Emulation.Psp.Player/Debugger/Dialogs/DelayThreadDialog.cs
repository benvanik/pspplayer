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
using System.Globalization;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player.Debugger.Dialogs
{
	partial class DelayThreadDialog : Form
	{
		public uint Time;

		public DelayThreadDialog()
		{
			this.InitializeComponent();

			this.DialogResult = DialogResult.Cancel;
			this.timeTextBox.Text = "30000";
		}

		protected override void OnShown( EventArgs e )
		{
			base.OnShown( e );

			this.timeTextBox.Focus();
			this.timeTextBox.SelectAll();
		}

		private void delayButton_Click( object sender, EventArgs e )
		{
			uint time;
			if( uint.TryParse( this.timeTextBox.Text, out time ) == true )
				this.Time = time;
			else
			{
				SystemSounds.Exclamation.Play();
				this.timeTextBox.Focus();
				this.timeTextBox.SelectAll();
				return;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void DelayThreadDialog_Activated( object sender, EventArgs e )
		{
			this.timeTextBox.Focus();
			this.timeTextBox.SelectAll();
		}
	}
}
