namespace Noxa.Emulation.Psp.IO.Media.Iso.Configuration
{
	partial class MediaConfiguration
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
			this.label2 = new System.Windows.Forms.Label();
			this.pathTextBox = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add( this.browseButton );
			this.panel.Controls.Add( this.pathTextBox );
			this.panel.Controls.Add( this.label2 );
			this.panel.Size = new System.Drawing.Size( 455, 163 );
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 28, 17 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 32, 13 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Path:";
			// 
			// pathTextBox
			// 
			this.pathTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.pathTextBox.Location = new System.Drawing.Point( 66, 14 );
			this.pathTextBox.Name = "pathTextBox";
			this.pathTextBox.Size = new System.Drawing.Size( 290, 20 );
			this.pathTextBox.TabIndex = 4;
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.browseButton.Location = new System.Drawing.Point( 362, 12 );
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size( 75, 23 );
			this.browseButton.TabIndex = 5;
			this.browseButton.Text = "Browse";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler( this.BrowseButtonClick );
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Image files|*.iso|All files|*.*";
			this.openFileDialog.RestoreDirectory = true;
			this.openFileDialog.SupportMultiDottedExtensions = true;
			this.openFileDialog.Title = "Open Image";
			// 
			// MediaConfiguration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.Name = "MediaConfiguration";
			this.Size = new System.Drawing.Size( 455, 185 );
			this.panel.ResumeLayout( false );
			this.panel.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog openFileDialog;

	}
}
