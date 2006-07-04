namespace Noxa.Emulation.Psp.Configuration
{
	partial class ConfigurationBase
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.notificationPanel = new System.Windows.Forms.Panel();
			this.panel = new System.Windows.Forms.Panel();
			this.notificationLabel = new System.Windows.Forms.Label();
			this.notificationIcon = new System.Windows.Forms.PictureBox();
			this.notificationPanel.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.notificationIcon ) ).BeginInit();
			this.SuspendLayout();
			// 
			// notificationPanel
			// 
			this.notificationPanel.Controls.Add( this.notificationIcon );
			this.notificationPanel.Controls.Add( this.notificationLabel );
			this.notificationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.notificationPanel.Location = new System.Drawing.Point( 0, 437 );
			this.notificationPanel.Name = "notificationPanel";
			this.notificationPanel.Size = new System.Drawing.Size( 547, 22 );
			this.notificationPanel.TabIndex = 1;
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point( 0, 0 );
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size( 547, 437 );
			this.panel.TabIndex = 2;
			// 
			// notificationLabel
			// 
			this.notificationLabel.AutoSize = true;
			this.notificationLabel.Location = new System.Drawing.Point( 25, 4 );
			this.notificationLabel.Name = "notificationLabel";
			this.notificationLabel.Size = new System.Drawing.Size( 149, 13 );
			this.notificationLabel.TabIndex = 0;
			this.notificationLabel.Text = "A warning message goes here";
			this.notificationLabel.Visible = false;
			// 
			// notificationIcon
			// 
			this.notificationIcon.Location = new System.Drawing.Point( 3, 3 );
			this.notificationIcon.Name = "notificationIcon";
			this.notificationIcon.Size = new System.Drawing.Size( 16, 16 );
			this.notificationIcon.TabIndex = 1;
			this.notificationIcon.TabStop = false;
			this.notificationIcon.Visible = false;
			// 
			// ConfigurationBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.panel );
			this.Controls.Add( this.notificationPanel );
			this.Name = "ConfigurationBase";
			this.Size = new System.Drawing.Size( 547, 459 );
			this.notificationPanel.ResumeLayout( false );
			this.notificationPanel.PerformLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.notificationIcon ) ).EndInit();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel notificationPanel;
		protected System.Windows.Forms.Panel panel;
		private System.Windows.Forms.PictureBox notificationIcon;
		private System.Windows.Forms.Label notificationLabel;
	}
}
