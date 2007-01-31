namespace Noxa.Emulation.Psp.Player.ServiceTools
{
	partial class SubmitGame
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
			this.submitButton = new System.Windows.Forms.Button();
			this.graphicalHeader1 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.graphicalHeader2 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.discIdLabel = new System.Windows.Forms.Label();
			this.regionLabel = new System.Windows.Forms.Label();
			this.titleLabel = new System.Windows.Forms.Label();
			this.firmwareLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.iconPictureBox = new System.Windows.Forms.PictureBox();
			this.addButton = new System.Windows.Forms.Button();
			this.titleView = new System.Windows.Forms.DataGridView();
			this.TitleColumn = new System.Windows.Forms.DataGridViewLinkColumn();
			this.WebsiteColumn = new System.Windows.Forms.DataGridViewLinkColumn();
			this.GoogleColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.refreshButton = new System.Windows.Forms.Button();
			( ( System.ComponentModel.ISupportInitialize )( this.iconPictureBox ) ).BeginInit();
			( ( System.ComponentModel.ISupportInitialize )( this.titleView ) ).BeginInit();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 437, 589 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler( this.cancelButton_Click );
			// 
			// submitButton
			// 
			this.submitButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.submitButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StartIcon;
			this.submitButton.Location = new System.Drawing.Point( 356, 589 );
			this.submitButton.Name = "submitButton";
			this.submitButton.Size = new System.Drawing.Size( 75, 23 );
			this.submitButton.TabIndex = 6;
			this.submitButton.Text = "&Submit";
			this.submitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.submitButton.UseVisualStyleBackColor = true;
			this.submitButton.Click += new System.EventHandler( this.submitButton_Click );
			// 
			// graphicalHeader1
			// 
			this.graphicalHeader1.Location = new System.Drawing.Point( 12, 12 );
			this.graphicalHeader1.Name = "graphicalHeader1";
			this.graphicalHeader1.Size = new System.Drawing.Size( 525, 22 );
			this.graphicalHeader1.TabIndex = 8;
			this.graphicalHeader1.Text = "Game Information";
			// 
			// graphicalHeader2
			// 
			this.graphicalHeader2.Location = new System.Drawing.Point( 12, 123 );
			this.graphicalHeader2.Name = "graphicalHeader2";
			this.graphicalHeader2.Size = new System.Drawing.Size( 525, 22 );
			this.graphicalHeader2.TabIndex = 9;
			this.graphicalHeader2.Text = "Game Title";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point( 31, 148 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 489, 33 );
			this.label1.TabIndex = 10;
			this.label1.Text = "Please select the title that describes this game. Keep in mind that this title ma" +
				"y differ from the region-specific title of the game. If you can\'t find a match, " +
				"click \'Add as New Game\' to proceed.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 31, 37 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 45, 13 );
			this.label2.TabIndex = 11;
			this.label2.Text = "Disc ID:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 32, 50 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 44, 13 );
			this.label3.TabIndex = 12;
			this.label3.Text = "Region:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 46, 63 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 30, 13 );
			this.label4.TabIndex = 13;
			this.label4.Text = "Title:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point( 24, 76 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 52, 13 );
			this.label5.TabIndex = 14;
			this.label5.Text = "Firmware:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point( 31, 89 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 45, 13 );
			this.label6.TabIndex = 15;
			this.label6.Text = "Version:";
			// 
			// discIdLabel
			// 
			this.discIdLabel.Location = new System.Drawing.Point( 82, 37 );
			this.discIdLabel.Name = "discIdLabel";
			this.discIdLabel.Size = new System.Drawing.Size( 352, 13 );
			this.discIdLabel.TabIndex = 16;
			this.discIdLabel.Text = "[value]";
			// 
			// regionLabel
			// 
			this.regionLabel.Location = new System.Drawing.Point( 82, 50 );
			this.regionLabel.Name = "regionLabel";
			this.regionLabel.Size = new System.Drawing.Size( 352, 13 );
			this.regionLabel.TabIndex = 17;
			this.regionLabel.Text = "[value]";
			// 
			// titleLabel
			// 
			this.titleLabel.Location = new System.Drawing.Point( 82, 63 );
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size( 352, 13 );
			this.titleLabel.TabIndex = 18;
			this.titleLabel.Text = "[value]";
			// 
			// firmwareLabel
			// 
			this.firmwareLabel.Location = new System.Drawing.Point( 82, 76 );
			this.firmwareLabel.Name = "firmwareLabel";
			this.firmwareLabel.Size = new System.Drawing.Size( 352, 13 );
			this.firmwareLabel.TabIndex = 19;
			this.firmwareLabel.Text = "[value]";
			// 
			// versionLabel
			// 
			this.versionLabel.Location = new System.Drawing.Point( 82, 89 );
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size( 352, 13 );
			this.versionLabel.TabIndex = 20;
			this.versionLabel.Text = "[value]";
			// 
			// iconPictureBox
			// 
			this.iconPictureBox.Location = new System.Drawing.Point( 368, 37 );
			this.iconPictureBox.Name = "iconPictureBox";
			this.iconPictureBox.Size = new System.Drawing.Size( 144, 80 );
			this.iconPictureBox.TabIndex = 21;
			this.iconPictureBox.TabStop = false;
			// 
			// addButton
			// 
			this.addButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.addButton.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.AddIcon;
			this.addButton.Location = new System.Drawing.Point( 217, 589 );
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size( 133, 23 );
			this.addButton.TabIndex = 22;
			this.addButton.Text = "Add as &New Game";
			this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler( this.addButton_Click );
			// 
			// titleView
			// 
			this.titleView.AllowUserToAddRows = false;
			this.titleView.AllowUserToDeleteRows = false;
			this.titleView.AllowUserToResizeRows = false;
			this.titleView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.titleView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.titleView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.titleView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.titleView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.titleView.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.TitleColumn,
            this.WebsiteColumn,
            this.GoogleColumn} );
			this.titleView.GridColor = System.Drawing.SystemColors.ControlLight;
			this.titleView.Location = new System.Drawing.Point( 34, 184 );
			this.titleView.MultiSelect = false;
			this.titleView.Name = "titleView";
			this.titleView.ReadOnly = true;
			this.titleView.RowHeadersVisible = false;
			this.titleView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.titleView.Size = new System.Drawing.Size( 478, 387 );
			this.titleView.TabIndex = 25;
			this.titleView.SelectionChanged += new System.EventHandler( this.titleView_SelectionChanged );
			this.titleView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler( this.titleView_CellContentClick );
			// 
			// TitleColumn
			// 
			this.TitleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.TitleColumn.HeaderText = "Title";
			this.TitleColumn.Name = "TitleColumn";
			this.TitleColumn.ReadOnly = true;
			this.TitleColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.TitleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// WebsiteColumn
			// 
			this.WebsiteColumn.HeaderText = "";
			this.WebsiteColumn.Name = "WebsiteColumn";
			this.WebsiteColumn.ReadOnly = true;
			this.WebsiteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.WebsiteColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.WebsiteColumn.Text = "View Website";
			this.WebsiteColumn.ToolTipText = "View Associated Website";
			this.WebsiteColumn.UseColumnTextForLinkValue = true;
			// 
			// GoogleColumn
			// 
			this.GoogleColumn.HeaderText = "";
			this.GoogleColumn.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.OptionsIcon;
			this.GoogleColumn.Name = "GoogleColumn";
			this.GoogleColumn.ReadOnly = true;
			this.GoogleColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.GoogleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.GoogleColumn.ToolTipText = "Search on Google";
			this.GoogleColumn.Width = 20;
			// 
			// refreshButton
			// 
			this.refreshButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.refreshButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.refreshButton.Enabled = false;
			this.refreshButton.Location = new System.Drawing.Point( 35, 589 );
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size( 75, 23 );
			this.refreshButton.TabIndex = 26;
			this.refreshButton.Text = "&Refresh";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler( this.refreshButton_Click );
			// 
			// SubmitGame
			// 
			this.AcceptButton = this.submitButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 549, 624 );
			this.Controls.Add( this.refreshButton );
			this.Controls.Add( this.titleView );
			this.Controls.Add( this.addButton );
			this.Controls.Add( this.iconPictureBox );
			this.Controls.Add( this.versionLabel );
			this.Controls.Add( this.firmwareLabel );
			this.Controls.Add( this.titleLabel );
			this.Controls.Add( this.regionLabel );
			this.Controls.Add( this.discIdLabel );
			this.Controls.Add( this.label6 );
			this.Controls.Add( this.label5 );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.graphicalHeader2 );
			this.Controls.Add( this.graphicalHeader1 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.submitButton );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size( 565, 500 );
			this.Name = "SubmitGame";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Submit Game";
			( ( System.ComponentModel.ISupportInitialize )( this.iconPictureBox ) ).EndInit();
			( ( System.ComponentModel.ISupportInitialize )( this.titleView ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button submitButton;
		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader1;
		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label discIdLabel;
		private System.Windows.Forms.Label regionLabel;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label firmwareLabel;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.PictureBox iconPictureBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.DataGridView titleView;
		private System.Windows.Forms.DataGridViewLinkColumn TitleColumn;
		private System.Windows.Forms.DataGridViewLinkColumn WebsiteColumn;
		private System.Windows.Forms.DataGridViewImageColumn GoogleColumn;
		private System.Windows.Forms.Button refreshButton;
	}
}