namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class GameEntry
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
			this.components = new System.ComponentModel.Container();
			this.gamePicture = new System.Windows.Forms.PictureBox();
			this.infoLabel = new System.Windows.Forms.Label();
			this.titleLabel = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			( ( System.ComponentModel.ISupportInitialize )( this.gamePicture ) ).BeginInit();
			this.SuspendLayout();
			// 
			// gamePicture
			// 
			this.gamePicture.BackColor = System.Drawing.Color.Transparent;
			this.gamePicture.Location = new System.Drawing.Point( 3, 3 );
			this.gamePicture.Name = "gamePicture";
			this.gamePicture.Size = new System.Drawing.Size( 75, 41 );
			this.gamePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.gamePicture.TabIndex = 0;
			this.gamePicture.TabStop = false;
			this.gamePicture.DoubleClick += new System.EventHandler( this.gamePicture_DoubleClick );
			this.gamePicture.Click += new System.EventHandler( this.gamePicture_Click );
			// 
			// infoLabel
			// 
			this.infoLabel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.infoLabel.BackColor = System.Drawing.Color.Transparent;
			this.infoLabel.Location = new System.Drawing.Point( 84, 26 );
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size( 171, 13 );
			this.infoLabel.TabIndex = 2;
			this.infoLabel.Text = "label1";
			this.infoLabel.DoubleClick += new System.EventHandler( this.infoLabel_DoubleClick );
			this.infoLabel.Click += new System.EventHandler( this.infoLabel_Click );
			// 
			// titleLabel
			// 
			this.titleLabel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.titleLabel.BackColor = System.Drawing.Color.Transparent;
			this.titleLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.titleLabel.Location = new System.Drawing.Point( 84, 6 );
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size( 171, 20 );
			this.titleLabel.TabIndex = 4;
			this.titleLabel.Text = "label1";
			this.titleLabel.DoubleClick += new System.EventHandler( this.titleLabel_DoubleClick );
			this.titleLabel.Click += new System.EventHandler( this.titleLabel_Click );
			// 
			// GameEntry
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.titleLabel );
			this.Controls.Add( this.infoLabel );
			this.Controls.Add( this.gamePicture );
			this.Name = "GameEntry";
			this.Size = new System.Drawing.Size( 258, 48 );
			( ( System.ComponentModel.ISupportInitialize )( this.gamePicture ) ).EndInit();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.PictureBox gamePicture;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
