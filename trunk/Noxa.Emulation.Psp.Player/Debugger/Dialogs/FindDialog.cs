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
using Be.Windows.Forms;

namespace Noxa.Emulation.Psp.Player.Debugger.Dialogs
{
	enum FindAction
	{
		FindFirst,
		FindNext,
		FindAll,
	}

	enum FindMode
	{
		Ascii,
		Binary,
	}

	partial class FindDialog : Form
	{
		public FindAction Action = FindAction.FindFirst;
		public FindMode Mode = FindMode.Ascii;
		public string FindText = string.Empty;
		public byte[] FindBytes = new byte[ 0 ];

		private bool _didChange;

		public FindDialog()
		{
			this.InitializeComponent();

			this.DialogResult = DialogResult.Cancel;

			this.hexBox.ByteProvider = new DynamicByteProvider( new ByteCollection() );
			this.hexBox.ByteProvider.Changed += new EventHandler( ByteProvider_Changed );

			_didChange = true;
		}

		protected override void OnShown( EventArgs e )
		{
			base.OnShown( e );

			switch( this.Mode )
			{
				case FindMode.Ascii:
					this.asciiTextBox.Focus();
					this.asciiTextBox.SelectAll();
					break;
				case FindMode.Binary:
					this.hexBox.Focus();
					this.hexBox.SelectionStart = 0;
					break;
			}
			_didChange = false;
		}

		private void ByteProvider_Changed( object sender, EventArgs e )
		{
			_didChange = true;
		}

		private void asciiTextBox_TextChanged( object sender, EventArgs e )
		{
			_didChange = true;
		}

		private void asciiTextBox_Enter( object sender, EventArgs e )
		{
			this.asciiRadioButton.Checked = true;
			_didChange = true;
		}

		private void hexBox_Enter( object sender, EventArgs e )
		{
			this.binaryRadioButton.Checked = true;
			_didChange = true;
		}

		private void asciiRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			this.Mode = FindMode.Ascii;
			_didChange = true;
		}

		private void binaryRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			this.Mode = FindMode.Binary;
			_didChange = true;
		}

		private bool GetState()
		{
			switch( this.Mode )
			{
				case FindMode.Ascii:
					this.FindText = this.asciiTextBox.Text;
					if( this.FindText.Length == 0 )
						return false;
					break;
				case FindMode.Binary:
					this.FindBytes = ( ( DynamicByteProvider )this.hexBox.ByteProvider ).Bytes.GetBytes();
					if( this.FindBytes.Length == 0 )
						return false;
					break;
			}
			return true;
		}

		private void findButton_Click( object sender, EventArgs e )
		{
			this.Action = ( _didChange == true ) ? FindAction.FindFirst : FindAction.FindNext;
			if( this.GetState() == true )
			{
				this.DialogResult = DialogResult.OK;
			}
			else
				this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void findAllButton_Click( object sender, EventArgs e )
		{
			this.Action = FindAction.FindAll;
			if( this.GetState() == true )
			{
				this.DialogResult = DialogResult.OK;
			}
			else
				this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
