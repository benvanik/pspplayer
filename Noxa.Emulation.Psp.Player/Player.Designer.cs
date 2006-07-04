namespace Noxa.Emulation.Psp.Player
{
	partial class Player
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.startToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pauseToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.restartToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.configureToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.renderSurface = new System.Windows.Forms.Panel();
			this.splashPicture = new System.Windows.Forms.PictureBox();
			this.toolStrip1.SuspendLayout();
			this.renderSurface.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.splashPicture ) ).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripButton,
            this.pauseToolStripButton,
            this.stopToolStripButton,
            this.restartToolStripButton,
            this.toolStripSeparator1,
            this.configureToolStripButton} );
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 480, 25 );
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// startToolStripButton
			// 
			this.startToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StartIcon;
			this.startToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startToolStripButton.Name = "startToolStripButton";
			this.startToolStripButton.Size = new System.Drawing.Size( 51, 22 );
			this.startToolStripButton.Text = "Start";
			this.startToolStripButton.Click += new System.EventHandler( this.startToolStripButton_Click );
			// 
			// pauseToolStripButton
			// 
			this.pauseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pauseToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.PauseIcon;
			this.pauseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pauseToolStripButton.Name = "pauseToolStripButton";
			this.pauseToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.pauseToolStripButton.Text = "Pause";
			this.pauseToolStripButton.Click += new System.EventHandler( this.pauseToolStripButton_Click );
			// 
			// stopToolStripButton
			// 
			this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stopToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StopIcon;
			this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stopToolStripButton.Name = "stopToolStripButton";
			this.stopToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stopToolStripButton.Text = "Stop";
			this.stopToolStripButton.Click += new System.EventHandler( this.stopToolStripButton_Click );
			// 
			// restartToolStripButton
			// 
			this.restartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.restartToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.RestartIcon;
			this.restartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.restartToolStripButton.Name = "restartToolStripButton";
			this.restartToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.restartToolStripButton.Text = "Restart";
			this.restartToolStripButton.Click += new System.EventHandler( this.restartToolStripButton_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
			// 
			// configureToolStripButton
			// 
			this.configureToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.OptionsIcon;
			this.configureToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.configureToolStripButton.Name = "configureToolStripButton";
			this.configureToolStripButton.Size = new System.Drawing.Size( 74, 22 );
			this.configureToolStripButton.Text = "Configure";
			this.configureToolStripButton.Click += new System.EventHandler( this.configureToolStripButton_Click );
			// 
			// renderSurface
			// 
			this.renderSurface.BackColor = System.Drawing.Color.White;
			this.renderSurface.Controls.Add( this.splashPicture );
			this.renderSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderSurface.Location = new System.Drawing.Point( 0, 25 );
			this.renderSurface.Name = "renderSurface";
			this.renderSurface.Size = new System.Drawing.Size( 480, 272 );
			this.renderSurface.TabIndex = 1;
			// 
			// splashPicture
			// 
			this.splashPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.splashPicture.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.PspShake;
			this.splashPicture.Location = new System.Drawing.Point( 179, 70 );
			this.splashPicture.Name = "splashPicture";
			this.splashPicture.Size = new System.Drawing.Size( 123, 132 );
			this.splashPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.splashPicture.TabIndex = 0;
			this.splashPicture.TabStop = false;
			// 
			// Player
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 480, 297 );
			this.Controls.Add( this.renderSurface );
			this.Controls.Add( this.toolStrip1 );
			this.MinimumSize = new System.Drawing.Size( 488, 331 );
			this.Name = "Player";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PSP Player";
			this.Load += new System.EventHandler( this.Player_Load );
			this.toolStrip1.ResumeLayout( false );
			this.toolStrip1.PerformLayout();
			this.renderSurface.ResumeLayout( false );
			this.renderSurface.PerformLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.splashPicture ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton startToolStripButton;
		private System.Windows.Forms.ToolStripButton configureToolStripButton;
		private System.Windows.Forms.ToolStripButton pauseToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton stopToolStripButton;
		private System.Windows.Forms.ToolStripButton restartToolStripButton;
		private System.Windows.Forms.Panel renderSurface;
		private System.Windows.Forms.PictureBox splashPicture;
	}
}

