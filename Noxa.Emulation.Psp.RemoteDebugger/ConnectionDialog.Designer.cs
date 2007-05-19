// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class ConnectionDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.machineComboBox = new System.Windows.Forms.ComboBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.connectWorker = new System.ComponentModel.BackgroundWorker();
			this.autoConnectCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 191, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Please select a machine to connect to:";
			// 
			// machineComboBox
			// 
			this.machineComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.machineComboBox.FormattingEnabled = true;
			this.machineComboBox.Location = new System.Drawing.Point( 15, 25 );
			this.machineComboBox.Name = "machineComboBox";
			this.machineComboBox.Size = new System.Drawing.Size( 276, 21 );
			this.machineComboBox.TabIndex = 1;
			this.machineComboBox.Text = "localhost";
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.browseButton.Enabled = false;
			this.browseButton.Location = new System.Drawing.Point( 297, 23 );
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size( 75, 23 );
			this.browseButton.TabIndex = 2;
			this.browseButton.Text = "&Browse";
			this.browseButton.UseVisualStyleBackColor = true;
			// 
			// startButton
			// 
			this.startButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.startButton.Location = new System.Drawing.Point( 257, 60 );
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size( 115, 23 );
			this.startButton.TabIndex = 3;
			this.startButton.Text = "&Start Debugging";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler( this.startButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 176, 60 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// connectWorker
			// 
			this.connectWorker.WorkerSupportsCancellation = true;
			this.connectWorker.DoWork += new System.ComponentModel.DoWorkEventHandler( this.connectWorker_DoWork );
			this.connectWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.connectWorker_RunWorkerCompleted );
			// 
			// autoConnectCheckBox
			// 
			this.autoConnectCheckBox.AutoSize = true;
			this.autoConnectCheckBox.Checked = true;
			this.autoConnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoConnectCheckBox.Enabled = false;
			this.autoConnectCheckBox.Location = new System.Drawing.Point( 15, 64 );
			this.autoConnectCheckBox.Name = "autoConnectCheckBox";
			this.autoConnectCheckBox.Size = new System.Drawing.Size( 118, 17 );
			this.autoConnectCheckBox.TabIndex = 5;
			this.autoConnectCheckBox.Text = "Connect on Startup";
			this.autoConnectCheckBox.UseVisualStyleBackColor = true;
			// 
			// ConnectionDialog
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 384, 95 );
			this.Controls.Add( this.autoConnectCheckBox );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.startButton );
			this.Controls.Add( this.browseButton );
			this.Controls.Add( this.machineComboBox );
			this.Controls.Add( this.label1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Remote Connection";
			this.Load += new System.EventHandler( this.ConnectionDialog_Load );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox machineComboBox;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button cancelButton;
		private System.ComponentModel.BackgroundWorker connectWorker;
		private System.Windows.Forms.CheckBox autoConnectCheckBox;
	}
}