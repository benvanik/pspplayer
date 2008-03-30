namespace Noxa.Emulation.Psp.Player.Debugger.Dialogs
{
	partial class DelayThreadDialog
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
			this.delayButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.timeTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// delayButton
			// 
			this.delayButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.delayButton.Location = new System.Drawing.Point( 103, 51 );
			this.delayButton.Name = "delayButton";
			this.delayButton.Size = new System.Drawing.Size( 75, 23 );
			this.delayButton.TabIndex = 2;
			this.delayButton.Text = "&Delay";
			this.delayButton.UseVisualStyleBackColor = true;
			this.delayButton.Click += new System.EventHandler( this.delayButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 184, 51 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 175, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter time to delay the thread in ms:";
			// 
			// timeTextBox
			// 
			this.timeTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.timeTextBox.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.timeTextBox.Location = new System.Drawing.Point( 15, 25 );
			this.timeTextBox.Name = "timeTextBox";
			this.timeTextBox.Size = new System.Drawing.Size( 244, 20 );
			this.timeTextBox.TabIndex = 1;
			// 
			// DelayThreadDialog
			// 
			this.AcceptButton = this.delayButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 271, 86 );
			this.Controls.Add( this.timeTextBox );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.delayButton );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DelayThreadDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Delay Thread";
			this.Activated += new System.EventHandler( this.DelayThreadDialog_Activated );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button delayButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox timeTextBox;
	}
}