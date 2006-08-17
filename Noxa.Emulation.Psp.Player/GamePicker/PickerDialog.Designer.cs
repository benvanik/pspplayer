namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class PickerDialog
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
			this.components = new System.ComponentModel.Container();
			this.browseButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.playButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.whidbeyTabControl1 = new Noxa.Utilities.Controls.WhidbeyTabControl();
			this.whidbeyTabPage1 = new Noxa.Utilities.Controls.WhidbeyTabPage();
			this.graphicalHeader1 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.whidbeyTabPage2 = new Noxa.Utilities.Controls.WhidbeyTabPage();
			this.graphicalHeader2 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.whidbeyTabPage1.SuspendLayout();
			this.whidbeyTabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.browseButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.AddIcon;
			this.browseButton.Location = new System.Drawing.Point( 120, 389 );
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size( 75, 23 );
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "&Browse";
			this.browseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.toolTip1.SetToolTip( this.browseButton, "Browse for a new game to play" );
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler( this.browseButton_Click );
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.clearButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.ClearIcon;
			this.clearButton.Location = new System.Drawing.Point( 233, 389 );
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size( 26, 23 );
			this.clearButton.TabIndex = 3;
			this.toolTip1.SetToolTip( this.clearButton, "Clear the list of recently played games" );
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler( this.clearButton_Click );
			// 
			// playButton
			// 
			this.playButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.playButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StartIcon;
			this.playButton.Location = new System.Drawing.Point( 342, 389 );
			this.playButton.Name = "playButton";
			this.playButton.Size = new System.Drawing.Size( 75, 23 );
			this.playButton.TabIndex = 4;
			this.playButton.Text = "&Play";
			this.playButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.toolTip1.SetToolTip( this.playButton, "Play the selected game" );
			this.playButton.UseVisualStyleBackColor = true;
			this.playButton.Click += new System.EventHandler( this.playButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 423, 389 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.removeButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.RemoveIcon;
			this.removeButton.Location = new System.Drawing.Point( 201, 389 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 26, 23 );
			this.removeButton.TabIndex = 2;
			this.toolTip1.SetToolTip( this.removeButton, "Remove the selected game from the list" );
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler( this.removeButton_Click );
			// 
			// whidbeyTabControl1
			// 
			this.whidbeyTabControl1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.whidbeyTabControl1.Location = new System.Drawing.Point( 12, 12 );
			this.whidbeyTabControl1.Name = "whidbeyTabControl1";
			this.whidbeyTabControl1.SelectedIndex = 1;
			this.whidbeyTabControl1.SelectedTab = this.whidbeyTabPage2;
			this.whidbeyTabControl1.Size = new System.Drawing.Size( 487, 371 );
			this.whidbeyTabControl1.TabIndex = 0;
			this.whidbeyTabControl1.TabPages.Add( this.whidbeyTabPage1 );
			this.whidbeyTabControl1.TabPages.Add( this.whidbeyTabPage2 );
			// 
			// whidbeyTabPage1
			// 
			this.whidbeyTabPage1.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 243 ) ) ) ), ( ( int )( ( ( byte )( 241 ) ) ) ), ( ( int )( ( ( byte )( 230 ) ) ) ) );
			this.whidbeyTabPage1.Controls.Add( this.graphicalHeader1 );
			this.whidbeyTabPage1.Controls.Add( this.flowLayoutPanel1 );
			this.whidbeyTabPage1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.whidbeyTabPage1.Location = new System.Drawing.Point( 0, 0 );
			this.whidbeyTabPage1.Name = "whidbeyTabPage1";
			this.whidbeyTabPage1.Size = new System.Drawing.Size( 372, 357 );
			this.whidbeyTabPage1.TabIndex = 0;
			this.whidbeyTabPage1.Text = "Recent Games";
			// 
			// graphicalHeader1
			// 
			this.graphicalHeader1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.graphicalHeader1.Location = new System.Drawing.Point( 5, 3 );
			this.graphicalHeader1.Name = "graphicalHeader1";
			this.graphicalHeader1.Size = new System.Drawing.Size( 364, 22 );
			this.graphicalHeader1.TabIndex = 4;
			this.graphicalHeader1.Text = "Recently Played Games";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point( 17, 31 );
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size( 339, 311 );
			this.flowLayoutPanel1.TabIndex = 3;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// whidbeyTabPage2
			// 
			this.whidbeyTabPage2.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 243 ) ) ) ), ( ( int )( ( ( byte )( 241 ) ) ) ), ( ( int )( ( ( byte )( 230 ) ) ) ) );
			this.whidbeyTabPage2.Controls.Add( this.graphicalHeader2 );
			this.whidbeyTabPage2.Controls.Add( this.flowLayoutPanel2 );
			this.whidbeyTabPage2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.whidbeyTabPage2.Location = new System.Drawing.Point( 0, 0 );
			this.whidbeyTabPage2.Name = "whidbeyTabPage2";
			this.whidbeyTabPage2.Size = new System.Drawing.Size( 372, 357 );
			this.whidbeyTabPage2.TabIndex = 1;
			this.whidbeyTabPage2.Text = "Memory Stick";
			// 
			// graphicalHeader2
			// 
			this.graphicalHeader2.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.graphicalHeader2.Location = new System.Drawing.Point( 5, 3 );
			this.graphicalHeader2.Name = "graphicalHeader2";
			this.graphicalHeader2.Size = new System.Drawing.Size( 364, 22 );
			this.graphicalHeader2.TabIndex = 6;
			this.graphicalHeader2.Text = "Programs on the Memory Stick";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.flowLayoutPanel2.AutoScroll = true;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point( 17, 31 );
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size( 339, 311 );
			this.flowLayoutPanel2.TabIndex = 5;
			this.flowLayoutPanel2.WrapContents = false;
			// 
			// PickerDialog
			// 
			this.AcceptButton = this.playButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 510, 424 );
			this.Controls.Add( this.whidbeyTabControl1 );
			this.Controls.Add( this.removeButton );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.playButton );
			this.Controls.Add( this.clearButton );
			this.Controls.Add( this.browseButton );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size( 518, 458 );
			this.Name = "PickerDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Game Selection";
			this.whidbeyTabPage1.ResumeLayout( false );
			this.whidbeyTabPage2.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button playButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private Noxa.Utilities.Controls.WhidbeyTabControl whidbeyTabControl1;
		private Noxa.Utilities.Controls.WhidbeyTabPage whidbeyTabPage1;
		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private Noxa.Utilities.Controls.WhidbeyTabPage whidbeyTabPage2;
		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
	}
}