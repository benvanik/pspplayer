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
	partial class JumpToAddressDialog : Form
	{
		public uint Address;

		public JumpToAddressDialog()
		{
			this.InitializeComponent();

			this.DialogResult = DialogResult.Cancel;
			this.addressTextBox.Text = this.Address.ToString( "X8" );
		}

		private void jumpButton_Click( object sender, EventArgs e )
		{
			uint address;
			if( uint.TryParse( this.addressTextBox.Text, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address ) == true )
				this.Address = address;
			else
			{
				SystemSounds.Exclamation.Play();
				this.addressTextBox.Focus();
				this.addressTextBox.SelectAll();
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

		private void JumpToAddressDialog_Activated( object sender, EventArgs e )
		{
			this.addressTextBox.Focus();
			this.addressTextBox.SelectAll();
		}
	}
}
