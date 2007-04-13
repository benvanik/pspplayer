// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player.Development
{
	partial class DebugSetup
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
			this.label2 = new System.Windows.Forms.Label();
			this.objdumpFileTextBox = new System.Windows.Forms.TextBox();
			this.objdumpBrowseButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.useElfCheckBox = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 169, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "THIS IS A TEMPORARY DIALOG";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 12, 41 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 80, 13 );
			this.label2.TabIndex = 1;
			this.label2.Text = "objdump report:";
			// 
			// objdumpFileTextBox
			// 
			this.objdumpFileTextBox.Location = new System.Drawing.Point( 98, 38 );
			this.objdumpFileTextBox.Name = "objdumpFileTextBox";
			this.objdumpFileTextBox.Size = new System.Drawing.Size( 293, 20 );
			this.objdumpFileTextBox.TabIndex = 2;
			// 
			// objdumpBrowseButton
			// 
			this.objdumpBrowseButton.Location = new System.Drawing.Point( 397, 36 );
			this.objdumpBrowseButton.Name = "objdumpBrowseButton";
			this.objdumpBrowseButton.Size = new System.Drawing.Size( 75, 23 );
			this.objdumpBrowseButton.TabIndex = 3;
			this.objdumpBrowseButton.Text = "Browse";
			this.objdumpBrowseButton.UseVisualStyleBackColor = true;
			this.objdumpBrowseButton.Click += new System.EventHandler( this.objdumpBrowseButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 397, 210 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// startButton
			// 
			this.startButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.startButton.Location = new System.Drawing.Point( 281, 210 );
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size( 110, 23 );
			this.startButton.TabIndex = 5;
			this.startButton.Text = "&Start Debugging";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler( this.startButton_Click );
			// 
			// useElfCheckBox
			// 
			this.useElfCheckBox.AutoSize = true;
			this.useElfCheckBox.Location = new System.Drawing.Point( 98, 103 );
			this.useElfCheckBox.Name = "useElfCheckBox";
			this.useElfCheckBox.Size = new System.Drawing.Size( 293, 17 );
			this.useElfCheckBox.TabIndex = 7;
			this.useElfCheckBox.Text = "Use embedded debug info in ELF (only for custom or PB)";
			this.useElfCheckBox.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point( 98, 64 );
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size( 333, 20 );
			this.textBox1.TabIndex = 8;
			this.textBox1.Text = "psp-objdump -d --adjust-vma=0x08900000 BOOT.BIN > BOOT.dis.txt";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// DebugSetup
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 484, 245 );
			this.Controls.Add( this.textBox1 );
			this.Controls.Add( this.useElfCheckBox );
			this.Controls.Add( this.startButton );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.objdumpBrowseButton );
			this.Controls.Add( this.objdumpFileTextBox );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DebugSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Debug Setup";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox objdumpFileTextBox;
		private System.Windows.Forms.Button objdumpBrowseButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.CheckBox useElfCheckBox;
		private System.Windows.Forms.TextBox textBox1;
	}
}