namespace Noxa.Emulation.Psp.Player.ServiceTools
{
	partial class PleaseWaitDialog
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
			this.animationPictureBox = new System.Windows.Forms.PictureBox();
			this.infoLabel = new System.Windows.Forms.Label();
			( ( System.ComponentModel.ISupportInitialize )( this.animationPictureBox ) ).BeginInit();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 206, 77 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// animationPictureBox
			// 
			this.animationPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.animationPictureBox.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.AeroWait;
			this.animationPictureBox.Location = new System.Drawing.Point( 130, 34 );
			this.animationPictureBox.Name = "animationPictureBox";
			this.animationPictureBox.Size = new System.Drawing.Size( 32, 32 );
			this.animationPictureBox.TabIndex = 1;
			this.animationPictureBox.TabStop = false;
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point( 12, 9 );
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size( 73, 13 );
			this.infoLabel.TabIndex = 2;
			this.infoLabel.Text = "Please Wait...";
			// 
			// PleaseWaitDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 293, 112 );
			this.Controls.Add( this.infoLabel );
			this.Controls.Add( this.animationPictureBox );
			this.Controls.Add( this.cancelButton );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PleaseWaitDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "PSP Player";
			( ( System.ComponentModel.ISupportInitialize )( this.animationPictureBox ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.PictureBox animationPictureBox;
		private System.Windows.Forms.Label infoLabel;
	}
}