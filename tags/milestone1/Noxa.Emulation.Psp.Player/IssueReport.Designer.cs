// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player
{
	partial class IssueReport
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup( "Errors", System.Windows.Forms.HorizontalAlignment.Left );
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup( "Warnings", System.Windows.Forms.HorizontalAlignment.Left );
			this.listView = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList = new System.Windows.Forms.ImageList( this.components );
			this.label1 = new System.Windows.Forms.Label();
			this.dontShowWarningsCheckBox = new System.Windows.Forms.CheckBox();
			this.continueButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.continueLabel = new System.Windows.Forms.Label();
			this.continuePictureBox = new System.Windows.Forms.PictureBox();
			( ( System.ComponentModel.ISupportInitialize )( this.continuePictureBox ) ).BeginInit();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.listView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2} );
			this.listView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.OneClickActivate )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.InfoTip )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.listView.FullRowSelect = true;
			listViewGroup1.Header = "Errors";
			listViewGroup1.Name = "errorsGroup";
			listViewGroup2.Header = "Warnings";
			listViewGroup2.Name = "warningsGroup";
			this.listView.Groups.AddRange( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2} );
			this.listView.Location = new System.Drawing.Point( 12, 27 );
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.ShowItemToolTips = true;
			this.listView.Size = new System.Drawing.Size( 772, 275 );
			this.listView.SmallImageList = this.imageList;
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ItemActivate += new System.EventHandler( this.listView_ItemActivate );
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Message";
			this.columnHeader1.Width = 685;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Support";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 83;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size( 16, 16 );
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// label1
			// 
			this.label1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 772, 15 );
			this.label1.TabIndex = 1;
			this.label1.Text = "At least one problem was found with your configuration. Please see the list below" +
				" for information and links to possible solutions.";
			// 
			// dontShowWarningsCheckBox
			// 
			this.dontShowWarningsCheckBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.dontShowWarningsCheckBox.AutoSize = true;
			this.dontShowWarningsCheckBox.Location = new System.Drawing.Point( 12, 335 );
			this.dontShowWarningsCheckBox.Name = "dontShowWarningsCheckBox";
			this.dontShowWarningsCheckBox.Size = new System.Drawing.Size( 175, 17 );
			this.dontShowWarningsCheckBox.TabIndex = 2;
			this.dontShowWarningsCheckBox.Text = "Don\'t show when only warnings";
			this.dontShowWarningsCheckBox.UseVisualStyleBackColor = true;
			this.dontShowWarningsCheckBox.CheckedChanged += new System.EventHandler( this.dontShowWarningsCheckBox_CheckedChanged );
			// 
			// continueButton
			// 
			this.continueButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.continueButton.Location = new System.Drawing.Point( 709, 329 );
			this.continueButton.Name = "continueButton";
			this.continueButton.Size = new System.Drawing.Size( 75, 23 );
			this.continueButton.TabIndex = 3;
			this.continueButton.Text = "&Start";
			this.continueButton.UseVisualStyleBackColor = true;
			this.continueButton.Click += new System.EventHandler( this.continueButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 628, 329 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// continueLabel
			// 
			this.continueLabel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.continueLabel.AutoSize = true;
			this.continueLabel.Location = new System.Drawing.Point( 42, 309 );
			this.continueLabel.Name = "continueLabel";
			this.continueLabel.Size = new System.Drawing.Size( 35, 13 );
			this.continueLabel.TabIndex = 5;
			this.continueLabel.Text = "label2";
			// 
			// continuePictureBox
			// 
			this.continuePictureBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.continuePictureBox.Location = new System.Drawing.Point( 20, 308 );
			this.continuePictureBox.Name = "continuePictureBox";
			this.continuePictureBox.Size = new System.Drawing.Size( 16, 16 );
			this.continuePictureBox.TabIndex = 6;
			this.continuePictureBox.TabStop = false;
			// 
			// IssueReport
			// 
			this.AcceptButton = this.continueButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 796, 364 );
			this.Controls.Add( this.continuePictureBox );
			this.Controls.Add( this.continueLabel );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.continueButton );
			this.Controls.Add( this.dontShowWarningsCheckBox );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.listView );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size( 548, 400 );
			this.Name = "IssueReport";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Issues Detected with Configuration";
			( ( System.ComponentModel.ISupportInitialize )( this.continuePictureBox ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Noxa.Utilities.Controls.DoubleBufferedListView listView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox dontShowWarningsCheckBox;
		private System.Windows.Forms.Button continueButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label continueLabel;
		private System.Windows.Forms.PictureBox continuePictureBox;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ImageList imageList;
	}
}