// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
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

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class ConnectionDialog : Form
	{
		public EmuDebugger Debugger;

		public ConnectionDialog()
		{
			InitializeComponent();
		}

		public ConnectionDialog( EmuDebugger debugger )
			: this()
		{
			this.Icon = Properties.Resources.MainIcon;
			this.Debugger = debugger;

			this.cancelButton.Enabled = false;
		}

		private void ConnectionDialog_Load( object sender, EventArgs e )
		{
			if( this.autoConnectCheckBox.Checked == true )
			{
				this.startButton_Click( this, EventArgs.Empty );
			}
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			connectWorker.CancelAsync();
		}

		private void startButton_Click( object sender, EventArgs e )
		{
			string host = this.machineComboBox.Text;
			if( host.Length == 0 )
			{
				MessageBox.Show( this, "Please enter a machine name to connect to; if you don't know what\nto do, enter 'localhost'.", "Machine Name Error" );
				return;
			}

			this.machineComboBox.Enabled = false;
			this.startButton.Enabled = false;
			this.cancelButton.Enabled = true;

			this.Cursor = Cursors.WaitCursor;

			connectWorker.RunWorkerAsync( host );
		}

		private void connectWorker_DoWork( object sender, DoWorkEventArgs e )
		{
			string host = e.Argument as string;

			while( connectWorker.CancellationPending == false )
			{
				try
				{
					if( this.Debugger.Connect( host ) == false )
						continue;
					else
						return;
				}
				catch( Exception ex )
				{
					Debug.WriteLine( ex.ToString() );
				}
			}
		}

		private void connectWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		{
			if( e.Cancelled == true )
			{
				// Cancelled
				this.machineComboBox.Enabled = true;
				this.startButton.Enabled = true;
				this.cancelButton.Enabled = false;
				this.Cursor = Cursors.Default;
			}
			else
			{
				if( e.Error != null )
				{
					// Exception hit
					DialogResult ret = MessageBox.Show( this, "An error occured while connecting to the remote debugger.", "Connection Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error );
					if( ret == DialogResult.Retry )
						this.startButton_Click( this, EventArgs.Empty );
					else
					{
						this.machineComboBox.Enabled = true;
						this.startButton.Enabled = true;
						this.cancelButton.Enabled = false;
						this.Cursor = Cursors.Default;
					}
				}
				else
				{
					// Ok
					this.Cursor = Cursors.Default;
					this.Close();
				}
			}
		}
	}
}