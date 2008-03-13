namespace Noxa.Emulation.Psp.Video.ManagedGL.Configuration
{
	partial class VideoConfiguration
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
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add( this.checkBox1 );
			this.panel.Size = new System.Drawing.Size( 455, 163 );
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.Description = "Select a root folder for the device.";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point( 12, 13 );
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size( 111, 17 );
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Some setting here";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// VideoConfiguration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.Name = "VideoConfiguration";
			this.Size = new System.Drawing.Size( 455, 185 );
			this.panel.ResumeLayout( false );
			this.panel.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.CheckBox checkBox1;

	}
}
