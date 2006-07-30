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
			this.graphicalHeader1 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.browseButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.playButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.removeButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.SuspendLayout();
			// 
			// graphicalHeader1
			// 
			this.graphicalHeader1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.graphicalHeader1.Location = new System.Drawing.Point( 12, 12 );
			this.graphicalHeader1.Name = "graphicalHeader1";
			this.graphicalHeader1.Size = new System.Drawing.Size( 329, 22 );
			this.graphicalHeader1.TabIndex = 0;
			this.graphicalHeader1.Text = "Recent Games";
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.browseButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.AddIcon;
			this.browseButton.Location = new System.Drawing.Point( 12, 343 );
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
			this.clearButton.Location = new System.Drawing.Point( 125, 343 );
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size( 26, 23 );
			this.clearButton.TabIndex = 2;
			this.toolTip1.SetToolTip( this.clearButton, "Clear the list of recently played games" );
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler( this.clearButton_Click );
			// 
			// playButton
			// 
			this.playButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.playButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StartIcon;
			this.playButton.Location = new System.Drawing.Point( 185, 343 );
			this.playButton.Name = "playButton";
			this.playButton.Size = new System.Drawing.Size( 75, 23 );
			this.playButton.TabIndex = 3;
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
			this.cancelButton.Location = new System.Drawing.Point( 266, 343 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point( 12, 40 );
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size( 329, 297 );
			this.flowLayoutPanel1.TabIndex = 5;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.removeButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.RemoveIcon;
			this.removeButton.Location = new System.Drawing.Point( 93, 343 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 26, 23 );
			this.removeButton.TabIndex = 6;
			this.toolTip1.SetToolTip( this.removeButton, "Remove the selected game from the list" );
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler( this.removeButton_Click );
			// 
			// PickerDialog
			// 
			this.AcceptButton = this.playButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 353, 378 );
			this.Controls.Add( this.removeButton );
			this.Controls.Add( this.flowLayoutPanel1 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.playButton );
			this.Controls.Add( this.clearButton );
			this.Controls.Add( this.browseButton );
			this.Controls.Add( this.graphicalHeader1 );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size( 361, 412 );
			this.Name = "PickerDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Game Selection";
			this.ResumeLayout( false );

		}

		#endregion

		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader1;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button playButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}