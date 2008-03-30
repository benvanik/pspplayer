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
	enum RenameTarget
	{
		Breakpoint,
		Method,
		Label,
		Bookmark,
	}
	
	partial class RenameDialog : Form
	{
		public RenameTarget Target;
		public string Value;

		public RenameDialog()
		{
			this.InitializeComponent();

			this.DialogResult = DialogResult.Cancel;
		}

		protected override void OnShown( EventArgs e )
		{
			base.OnShown( e );

			this.Text = "Rename";
			switch( this.Target )
			{
				case RenameTarget.Bookmark:
					this.Text += " Bookmark";
					break;
				case RenameTarget.Breakpoint:
					this.Text += " Breakpoint";
					break;
				case RenameTarget.Label:
					this.Text += " Label";
					break;
				case RenameTarget.Method:
					this.Text += " Method";
					break;
			}

			this.nameTextBox.Text = this.Value;
			this.nameTextBox.Focus();
			this.nameTextBox.SelectAll();
		}

		private void renameButton_Click( object sender, EventArgs e )
		{
			string value = this.nameTextBox.Text.Trim();
			if( value.Length == 0 )
			{
				SystemSounds.Exclamation.Play();
				this.nameTextBox.Focus();
				this.nameTextBox.SelectAll();
				return;
			}
			this.Value = value;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void RenameDialog_Activated( object sender, EventArgs e )
		{
			this.nameTextBox.Focus();
			this.nameTextBox.SelectAll();
		}
	}
}
