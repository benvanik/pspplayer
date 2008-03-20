namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class MemoryTool
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
			this.hexBox = new Be.Windows.Forms.HexBox();
			this.label1 = new System.Windows.Forms.Label();
			this.sectionComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// hexBox
			// 
			this.hexBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.hexBox.BytesPerLine = 24;
			this.hexBox.Font = new System.Drawing.Font( "Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.hexBox.LineInfoForeColor = System.Drawing.Color.Empty;
			this.hexBox.LineInfoVisible = true;
			this.hexBox.Location = new System.Drawing.Point( 12, 33 );
			this.hexBox.Name = "hexBox";
			this.hexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this.hexBox.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this.hexBox.ShadowSelectionColor = System.Drawing.SystemColors.InactiveCaption;
			this.hexBox.Size = new System.Drawing.Size( 489, 540 );
			this.hexBox.StringViewVisible = true;
			this.hexBox.TabIndex = 0;
			this.hexBox.UseFixedBytesPerLine = true;
			this.hexBox.VScrollBarVisible = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 9, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 46, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "Section:";
			// 
			// sectionComboBox
			// 
			this.sectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sectionComboBox.FormattingEnabled = true;
			this.sectionComboBox.Location = new System.Drawing.Point( 61, 6 );
			this.sectionComboBox.Name = "sectionComboBox";
			this.sectionComboBox.Size = new System.Drawing.Size( 200, 21 );
			this.sectionComboBox.TabIndex = 2;
			this.sectionComboBox.SelectedIndexChanged += new System.EventHandler( this.sectionComboBox_SelectedIndexChanged );
			// 
			// MemoryTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 513, 585 );
			this.CloseButton = false;
			this.Controls.Add( this.sectionComboBox );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.hexBox );
			this.Name = "MemoryTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.TabText = "Memory";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Be.Windows.Forms.HexBox hexBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox sectionComboBox;
	}
}
