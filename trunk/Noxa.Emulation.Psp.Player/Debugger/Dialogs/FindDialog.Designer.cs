namespace Noxa.Emulation.Psp.Player.Debugger.Dialogs
{
	partial class FindDialog
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.findButton = new System.Windows.Forms.Button();
			this.findAllButton = new System.Windows.Forms.Button();
			this.asciiRadioButton = new System.Windows.Forms.RadioButton();
			this.asciiTextBox = new System.Windows.Forms.TextBox();
			this.binaryRadioButton = new System.Windows.Forms.RadioButton();
			this.hexBox = new Be.Windows.Forms.HexBox();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 275, 146 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// findButton
			// 
			this.findButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.findButton.Location = new System.Drawing.Point( 113, 146 );
			this.findButton.Name = "findButton";
			this.findButton.Size = new System.Drawing.Size( 75, 23 );
			this.findButton.TabIndex = 4;
			this.findButton.Text = "&Find";
			this.findButton.UseVisualStyleBackColor = true;
			this.findButton.Click += new System.EventHandler( this.findButton_Click );
			// 
			// findAllButton
			// 
			this.findAllButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.findAllButton.Location = new System.Drawing.Point( 194, 146 );
			this.findAllButton.Name = "findAllButton";
			this.findAllButton.Size = new System.Drawing.Size( 75, 23 );
			this.findAllButton.TabIndex = 5;
			this.findAllButton.Text = "Find &All";
			this.findAllButton.UseVisualStyleBackColor = true;
			this.findAllButton.Click += new System.EventHandler( this.findAllButton_Click );
			// 
			// asciiRadioButton
			// 
			this.asciiRadioButton.AutoSize = true;
			this.asciiRadioButton.Checked = true;
			this.asciiRadioButton.Location = new System.Drawing.Point( 12, 12 );
			this.asciiRadioButton.Name = "asciiRadioButton";
			this.asciiRadioButton.Size = new System.Drawing.Size( 108, 17 );
			this.asciiRadioButton.TabIndex = 0;
			this.asciiRadioButton.TabStop = true;
			this.asciiRadioButton.Text = "Find Text (ASCII):";
			this.asciiRadioButton.UseVisualStyleBackColor = true;
			this.asciiRadioButton.CheckedChanged += new System.EventHandler( this.asciiRadioButton_CheckedChanged );
			// 
			// asciiTextBox
			// 
			this.asciiTextBox.Location = new System.Drawing.Point( 12, 35 );
			this.asciiTextBox.Name = "asciiTextBox";
			this.asciiTextBox.Size = new System.Drawing.Size( 338, 20 );
			this.asciiTextBox.TabIndex = 1;
			this.asciiTextBox.TextChanged += new System.EventHandler( this.asciiTextBox_TextChanged );
			this.asciiTextBox.Enter += new System.EventHandler( this.asciiTextBox_Enter );
			// 
			// binaryRadioButton
			// 
			this.binaryRadioButton.AutoSize = true;
			this.binaryRadioButton.Location = new System.Drawing.Point( 12, 61 );
			this.binaryRadioButton.Name = "binaryRadioButton";
			this.binaryRadioButton.Size = new System.Drawing.Size( 80, 17 );
			this.binaryRadioButton.TabIndex = 2;
			this.binaryRadioButton.Text = "Find Binary:";
			this.binaryRadioButton.UseVisualStyleBackColor = true;
			this.binaryRadioButton.CheckedChanged += new System.EventHandler( this.binaryRadioButton_CheckedChanged );
			// 
			// hexBox
			// 
			this.hexBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.hexBox.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.hexBox.LineInfoForeColor = System.Drawing.Color.Empty;
			this.hexBox.Location = new System.Drawing.Point( 12, 84 );
			this.hexBox.Name = "hexBox";
			this.hexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 100 ) ) ) ), ( ( int )( ( ( byte )( 60 ) ) ) ), ( ( int )( ( ( byte )( 188 ) ) ) ), ( ( int )( ( ( byte )( 255 ) ) ) ) );
			this.hexBox.ShadowSelectionVisible = false;
			this.hexBox.Size = new System.Drawing.Size( 338, 56 );
			this.hexBox.TabIndex = 3;
			this.hexBox.UseFixedBytesPerLine = true;
			this.hexBox.Enter += new System.EventHandler( this.hexBox_Enter );
			// 
			// FindDialog
			// 
			this.AcceptButton = this.findButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 362, 181 );
			this.Controls.Add( this.hexBox );
			this.Controls.Add( this.binaryRadioButton );
			this.Controls.Add( this.asciiTextBox );
			this.Controls.Add( this.asciiRadioButton );
			this.Controls.Add( this.findAllButton );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.findButton );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Find";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button findButton;
		private System.Windows.Forms.Button findAllButton;
		private System.Windows.Forms.RadioButton asciiRadioButton;
		private System.Windows.Forms.TextBox asciiTextBox;
		private System.Windows.Forms.RadioButton binaryRadioButton;
		private Be.Windows.Forms.HexBox hexBox;
	}
}