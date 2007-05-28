// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player
{
	partial class AttachDebuggerDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AttachDebuggerDialog ) );
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.attachButton = new System.Windows.Forms.Button();
			this.ignoreButton = new System.Windows.Forms.Button();
			this.quitButton = new System.Windows.Forms.Button();
			this.infoTextBox = new System.Windows.Forms.TextBox();
			this.copyButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 335, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "The emulator has requested that a debugger be attached.";
			// 
			// label2
			// 
			this.label2.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.label2.Location = new System.Drawing.Point( 12, 33 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 465, 98 );
			this.label2.TabIndex = 1;
			this.label2.Text = resources.GetString( "label2.Text" );
			// 
			// attachButton
			// 
			this.attachButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.attachButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.attachButton.Location = new System.Drawing.Point( 240, 292 );
			this.attachButton.Name = "attachButton";
			this.attachButton.Size = new System.Drawing.Size( 75, 23 );
			this.attachButton.TabIndex = 2;
			this.attachButton.Text = "&Attach";
			this.attachButton.UseVisualStyleBackColor = true;
			// 
			// ignoreButton
			// 
			this.ignoreButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.ignoreButton.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.ignoreButton.Location = new System.Drawing.Point( 321, 292 );
			this.ignoreButton.Name = "ignoreButton";
			this.ignoreButton.Size = new System.Drawing.Size( 75, 23 );
			this.ignoreButton.TabIndex = 3;
			this.ignoreButton.Text = "&Ignore";
			this.ignoreButton.UseVisualStyleBackColor = true;
			// 
			// quitButton
			// 
			this.quitButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.quitButton.Location = new System.Drawing.Point( 402, 292 );
			this.quitButton.Name = "quitButton";
			this.quitButton.Size = new System.Drawing.Size( 75, 23 );
			this.quitButton.TabIndex = 4;
			this.quitButton.Text = "&Quit";
			this.quitButton.UseVisualStyleBackColor = true;
			// 
			// infoTextBox
			// 
			this.infoTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.infoTextBox.Location = new System.Drawing.Point( 12, 100 );
			this.infoTextBox.Multiline = true;
			this.infoTextBox.Name = "infoTextBox";
			this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.infoTextBox.Size = new System.Drawing.Size( 464, 186 );
			this.infoTextBox.TabIndex = 5;
			// 
			// copyButton
			// 
			this.copyButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.copyButton.Enabled = false;
			this.copyButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.CopyIcon;
			this.copyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.copyButton.Location = new System.Drawing.Point( 15, 292 );
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size( 59, 23 );
			this.copyButton.TabIndex = 6;
			this.copyButton.Text = "&Copy";
			this.copyButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.copyButton.UseVisualStyleBackColor = true;
			this.copyButton.Click += new System.EventHandler( this.copyButton_Click );
			// 
			// AttachDebuggerDialog
			// 
			this.AcceptButton = this.attachButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.quitButton;
			this.ClientSize = new System.Drawing.Size( 488, 327 );
			this.Controls.Add( this.copyButton );
			this.Controls.Add( this.infoTextBox );
			this.Controls.Add( this.quitButton );
			this.Controls.Add( this.ignoreButton );
			this.Controls.Add( this.attachButton );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AttachDebuggerDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Attach Debugger";
			this.TopMost = true;
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button attachButton;
		private System.Windows.Forms.Button ignoreButton;
		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.TextBox infoTextBox;
		private System.Windows.Forms.Button copyButton;
	}
}