// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player.Development.Tools
{
	partial class CpuPane
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
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.generalRegistersLabel = new System.Windows.Forms.TextBox();
			this.cp0TabPage = new System.Windows.Forms.TabPage();
			this.cp0RegistersLabel = new System.Windows.Forms.TextBox();
			this.fpuTabPage = new System.Windows.Forms.TabPage();
			this.vfpuTabPage = new System.Windows.Forms.TabPage();
			this.pcLabel = new System.Windows.Forms.TextBox();
			this.friendlyCheckbox = new System.Windows.Forms.CheckBox();
			this.fpuRegistersLabel = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.cp0TabPage.SuspendLayout();
			this.fpuTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 4, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 24, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "PC:";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.tabControl1.Controls.Add( this.generalTabPage );
			this.tabControl1.Controls.Add( this.cp0TabPage );
			this.tabControl1.Controls.Add( this.fpuTabPage );
			this.tabControl1.Controls.Add( this.vfpuTabPage );
			this.tabControl1.Location = new System.Drawing.Point( 4, 32 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 251, 380 );
			this.tabControl1.TabIndex = 3;
			// 
			// generalTabPage
			// 
			this.generalTabPage.Controls.Add( this.generalRegistersLabel );
			this.generalTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.Size = new System.Drawing.Size( 243, 354 );
			this.generalTabPage.TabIndex = 0;
			this.generalTabPage.Text = "General";
			this.generalTabPage.UseVisualStyleBackColor = true;
			// 
			// generalRegistersLabel
			// 
			this.generalRegistersLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.generalRegistersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generalRegistersLabel.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.generalRegistersLabel.Location = new System.Drawing.Point( 0, 0 );
			this.generalRegistersLabel.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 0 );
			this.generalRegistersLabel.Multiline = true;
			this.generalRegistersLabel.Name = "generalRegistersLabel";
			this.generalRegistersLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.generalRegistersLabel.Size = new System.Drawing.Size( 243, 354 );
			this.generalRegistersLabel.TabIndex = 0;
			this.generalRegistersLabel.WordWrap = false;
			this.generalRegistersLabel.SizeChanged += new System.EventHandler( this.generalRegistersLabel_SizeChanged );
			// 
			// cp0TabPage
			// 
			this.cp0TabPage.Controls.Add( this.cp0RegistersLabel );
			this.cp0TabPage.Location = new System.Drawing.Point( 4, 22 );
			this.cp0TabPage.Name = "cp0TabPage";
			this.cp0TabPage.Size = new System.Drawing.Size( 243, 354 );
			this.cp0TabPage.TabIndex = 3;
			this.cp0TabPage.Text = "CP0";
			this.cp0TabPage.UseVisualStyleBackColor = true;
			// 
			// cp0RegistersLabel
			// 
			this.cp0RegistersLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.cp0RegistersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cp0RegistersLabel.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.cp0RegistersLabel.Location = new System.Drawing.Point( 0, 0 );
			this.cp0RegistersLabel.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 0 );
			this.cp0RegistersLabel.Multiline = true;
			this.cp0RegistersLabel.Name = "cp0RegistersLabel";
			this.cp0RegistersLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.cp0RegistersLabel.Size = new System.Drawing.Size( 243, 354 );
			this.cp0RegistersLabel.TabIndex = 1;
			this.cp0RegistersLabel.WordWrap = false;
			// 
			// fpuTabPage
			// 
			this.fpuTabPage.Controls.Add( this.fpuRegistersLabel );
			this.fpuTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.fpuTabPage.Name = "fpuTabPage";
			this.fpuTabPage.Size = new System.Drawing.Size( 243, 354 );
			this.fpuTabPage.TabIndex = 1;
			this.fpuTabPage.Text = "FPU";
			this.fpuTabPage.UseVisualStyleBackColor = true;
			// 
			// vfpuTabPage
			// 
			this.vfpuTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.vfpuTabPage.Name = "vfpuTabPage";
			this.vfpuTabPage.Size = new System.Drawing.Size( 243, 354 );
			this.vfpuTabPage.TabIndex = 2;
			this.vfpuTabPage.Text = "VFPU";
			this.vfpuTabPage.UseVisualStyleBackColor = true;
			// 
			// pcLabel
			// 
			this.pcLabel.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.pcLabel.Location = new System.Drawing.Point( 34, 6 );
			this.pcLabel.Name = "pcLabel";
			this.pcLabel.ReadOnly = true;
			this.pcLabel.Size = new System.Drawing.Size( 78, 20 );
			this.pcLabel.TabIndex = 4;
			this.pcLabel.Text = "0x00000000";
			// 
			// friendlyCheckbox
			// 
			this.friendlyCheckbox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.friendlyCheckbox.AutoSize = true;
			this.friendlyCheckbox.Location = new System.Drawing.Point( 196, 8 );
			this.friendlyCheckbox.Name = "friendlyCheckbox";
			this.friendlyCheckbox.Size = new System.Drawing.Size( 59, 17 );
			this.friendlyCheckbox.TabIndex = 5;
			this.friendlyCheckbox.Text = "Names";
			this.friendlyCheckbox.UseVisualStyleBackColor = true;
			this.friendlyCheckbox.CheckedChanged += new System.EventHandler( this.friendlyCheckbox_CheckedChanged );
			// 
			// fpuRegistersLabel
			// 
			this.fpuRegistersLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.fpuRegistersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpuRegistersLabel.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.fpuRegistersLabel.Location = new System.Drawing.Point( 0, 0 );
			this.fpuRegistersLabel.Margin = new System.Windows.Forms.Padding( 0 );
			this.fpuRegistersLabel.Multiline = true;
			this.fpuRegistersLabel.Name = "fpuRegistersLabel";
			this.fpuRegistersLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.fpuRegistersLabel.Size = new System.Drawing.Size( 243, 354 );
			this.fpuRegistersLabel.TabIndex = 2;
			this.fpuRegistersLabel.WordWrap = false;
			// 
			// CpuPane
			// 
			this.ClientSize = new System.Drawing.Size( 258, 416 );
			this.Controls.Add( this.friendlyCheckbox );
			this.Controls.Add( this.pcLabel );
			this.Controls.Add( this.tabControl1 );
			this.Controls.Add( this.label1 );
			this.Name = "CpuPane";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.DockRight;
			this.TabText = "Registers";
			this.Text = "Registers";
			this.tabControl1.ResumeLayout( false );
			this.generalTabPage.ResumeLayout( false );
			this.generalTabPage.PerformLayout();
			this.cp0TabPage.ResumeLayout( false );
			this.cp0TabPage.PerformLayout();
			this.fpuTabPage.ResumeLayout( false );
			this.fpuTabPage.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage generalTabPage;
		private System.Windows.Forms.TabPage fpuTabPage;
		private System.Windows.Forms.TabPage vfpuTabPage;
		private System.Windows.Forms.TabPage cp0TabPage;
		private System.Windows.Forms.TextBox pcLabel;
		private System.Windows.Forms.TextBox generalRegistersLabel;
		private System.Windows.Forms.CheckBox friendlyCheckbox;
		private System.Windows.Forms.TextBox cp0RegistersLabel;
		private System.Windows.Forms.TextBox fpuRegistersLabel;

	}
}
