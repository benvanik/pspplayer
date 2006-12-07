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
using System.IO;

namespace Noxa.Emulation.Psp.Player.Development
{
	public partial class DebugSetup : Form
	{
		public DebugSetup()
		{
			InitializeComponent();
		}

		public DebugSetup( bool arg )
			: this()
		{
		}

		public string ObjdumpFilename
		{
			get
			{
				return this.objdumpFileTextBox.Text;
			}
		}

		public bool UseElfDebug
		{
			get
			{
				return this.useElfCheckBox.Checked;
			}
		}

		private void objdumpBrowseButton_Click( object sender, EventArgs e )
		{
			if( this.openFileDialog.ShowDialog( this ) == DialogResult.OK )
			{
				this.objdumpFileTextBox.Text = this.openFileDialog.FileName;
			}
		}

		private void startButton_Click( object sender, EventArgs e )
		{
			if( this.useElfCheckBox.Checked == true )
			{
			}
			else
			{
				string filename = this.objdumpFileTextBox.Text;
				if( File.Exists( filename ) == false )
				{
					MessageBox.Show( this, "File not found. Please select a file and try again.", "PSP Player" );
					return;
				}
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}