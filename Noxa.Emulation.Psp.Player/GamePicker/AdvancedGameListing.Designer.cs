namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class AdvancedGameListing
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AdvancedGameListing ) );
			this.listView = new Noxa.Utilities.Controls.ImagedListView();
			this.iconColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.titleColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.regionColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.gamesImageList = new System.Windows.Forms.ImageList( this.components );
			this.filterTextBox = new System.Windows.Forms.TextBox();
			this.regionComboBox = new System.Windows.Forms.ComboBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupLabel = new System.Windows.Forms.Label();
			this.regionsImageList = new System.Windows.Forms.ImageList( this.components );
			( ( System.ComponentModel.ISupportInitialize )( this.pictureBox1 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.listView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.iconColumnHeader,
            this.titleColumnHeader,
            this.regionColumnHeader} );
			this.listView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.InfoTip )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.listView.FullRowSelect = true;
			this.listView.Location = new System.Drawing.Point( 3, 30 );
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.ShowItemToolTips = true;
			this.listView.Size = new System.Drawing.Size( 569, 331 );
			this.listView.SmallImageList = this.gamesImageList;
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.Resize += new System.EventHandler( this.listView_Resize );
			this.listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler( this.listView_ItemSelectionChanged );
			// 
			// iconColumnHeader
			// 
			this.iconColumnHeader.Text = "";
			this.iconColumnHeader.Width = 94;
			// 
			// titleColumnHeader
			// 
			this.titleColumnHeader.Text = "Title";
			this.titleColumnHeader.Width = 379;
			// 
			// regionColumnHeader
			// 
			this.regionColumnHeader.Text = "";
			this.regionColumnHeader.Width = 30;
			// 
			// gamesImageList
			// 
			this.gamesImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.gamesImageList.ImageSize = new System.Drawing.Size( 90, 50 );
			this.gamesImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// filterTextBox
			// 
			this.filterTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.filterTextBox.Location = new System.Drawing.Point( 386, 3 );
			this.filterTextBox.Name = "filterTextBox";
			this.filterTextBox.Size = new System.Drawing.Size( 121, 20 );
			this.filterTextBox.TabIndex = 2;
			this.filterTextBox.TextChanged += new System.EventHandler( this.filterTextBox_TextChanged );
			// 
			// regionComboBox
			// 
			this.regionComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.regionComboBox.Enabled = false;
			this.regionComboBox.FormattingEnabled = true;
			this.regionComboBox.Location = new System.Drawing.Point( 513, 3 );
			this.regionComboBox.Name = "regionComboBox";
			this.regionComboBox.Size = new System.Drawing.Size( 59, 21 );
			this.regionComboBox.TabIndex = 3;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.pictureBox1.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.SearchIcon;
			this.pictureBox1.Location = new System.Drawing.Point( 364, 3 );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size( 16, 20 );
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// groupLabel
			// 
			this.groupLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.groupLabel.Location = new System.Drawing.Point( 3, 6 );
			this.groupLabel.Name = "groupLabel";
			this.groupLabel.Size = new System.Drawing.Size( 101, 17 );
			this.groupLabel.TabIndex = 5;
			this.groupLabel.Text = "label1";
			// 
			// regionsImageList
			// 
			this.regionsImageList.ImageStream = ( ( System.Windows.Forms.ImageListStreamer )( resources.GetObject( "regionsImageList.ImageStream" ) ) );
			this.regionsImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.regionsImageList.Images.SetKeyName( 0, "Unknown" );
			this.regionsImageList.Images.SetKeyName( 1, "US" );
			this.regionsImageList.Images.SetKeyName( 2, "EU" );
			this.regionsImageList.Images.SetKeyName( 3, "JP" );
			this.regionsImageList.Images.SetKeyName( 4, "UK" );
			this.regionsImageList.Images.SetKeyName( 5, "KR" );
			// 
			// AdvancedGameListing
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.groupLabel );
			this.Controls.Add( this.pictureBox1 );
			this.Controls.Add( this.regionComboBox );
			this.Controls.Add( this.filterTextBox );
			this.Controls.Add( this.listView );
			this.Name = "AdvancedGameListing";
			this.Size = new System.Drawing.Size( 575, 364 );
			( ( System.ComponentModel.ISupportInitialize )( this.pictureBox1 ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Noxa.Utilities.Controls.ImagedListView listView;
		private System.Windows.Forms.TextBox filterTextBox;
		private System.Windows.Forms.ComboBox regionComboBox;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ColumnHeader iconColumnHeader;
		private System.Windows.Forms.ColumnHeader titleColumnHeader;
		private System.Windows.Forms.ColumnHeader regionColumnHeader;
		private System.Windows.Forms.Label groupLabel;
		private System.Windows.Forms.ImageList regionsImageList;
		private System.Windows.Forms.ImageList gamesImageList;

	}
}
