// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class CodeView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( CodeView ) );
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.registersListView = new System.Windows.Forms.ListView();
			this.registerNameHeader = new System.Windows.Forms.ColumnHeader();
			this.registerValuePrettyHeader = new System.Windows.Forms.ColumnHeader();
			this.registerValueRawHeader = new System.Windows.Forms.ColumnHeader();
			this.pcLabel = new System.Windows.Forms.Label();
			this.registersToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.generalRegistersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fPURegistersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vFPURegistersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disassemblyControl1 = new Noxa.Emulation.Psp.RemoteDebugger.Tools.DisassemblyControl();
			this.codeToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.hexToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.vfpuDisplay = new Noxa.Emulation.Psp.RemoteDebugger.Tools.VfpuDisplay();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.registersToolStrip.SuspendLayout();
			this.codeToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.textBox1 );
			this.splitContainer1.Panel1.Controls.Add( this.registersListView );
			this.splitContainer1.Panel1.Controls.Add( this.pcLabel );
			this.splitContainer1.Panel1.Controls.Add( this.registersToolStrip );
			this.splitContainer1.Panel1.Controls.Add( this.vfpuDisplay );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.disassemblyControl1 );
			this.splitContainer1.Panel2.Controls.Add( this.codeToolStrip );
			this.splitContainer1.Size = new System.Drawing.Size( 794, 633 );
			this.splitContainer1.SplitterDistance = 207;
			this.splitContainer1.TabIndex = 1;
			// 
			// registersListView
			// 
			this.registersListView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.registersListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.registerNameHeader,
            this.registerValuePrettyHeader,
            this.registerValueRawHeader} );
			this.registersListView.Font = new System.Drawing.Font( "Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.registersListView.FullRowSelect = true;
			this.registersListView.GridLines = true;
			this.registersListView.LabelEdit = true;
			this.registersListView.LabelWrap = false;
			this.registersListView.Location = new System.Drawing.Point( 3, 48 );
			this.registersListView.Name = "registersListView";
			this.registersListView.Size = new System.Drawing.Size( 201, 582 );
			this.registersListView.TabIndex = 3;
			this.registersListView.UseCompatibleStateImageBehavior = false;
			this.registersListView.View = System.Windows.Forms.View.Details;
			// 
			// registerNameHeader
			// 
			this.registerNameHeader.Text = "";
			this.registerNameHeader.Width = 49;
			// 
			// registerValuePrettyHeader
			// 
			this.registerValuePrettyHeader.Text = "Value";
			this.registerValuePrettyHeader.Width = 73;
			// 
			// registerValueRawHeader
			// 
			this.registerValueRawHeader.Text = "";
			// 
			// pcLabel
			// 
			this.pcLabel.Font = new System.Drawing.Font( "Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.pcLabel.Location = new System.Drawing.Point( 82, 29 );
			this.pcLabel.Name = "pcLabel";
			this.pcLabel.Size = new System.Drawing.Size( 30, 15 );
			this.pcLabel.TabIndex = 2;
			this.pcLabel.Text = "PC:";
			this.pcLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// registersToolStrip
			// 
			this.registersToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1} );
			this.registersToolStrip.Location = new System.Drawing.Point( 0, 0 );
			this.registersToolStrip.Name = "registersToolStrip";
			this.registersToolStrip.Size = new System.Drawing.Size( 207, 25 );
			this.registersToolStrip.TabIndex = 0;
			this.registersToolStrip.Text = "Register Tools";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButton1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.generalRegistersToolStripMenuItem,
            this.fPURegistersToolStripMenuItem,
            this.vFPURegistersToolStripMenuItem} );
			this.toolStripSplitButton1.Image = ( ( System.Drawing.Image )( resources.GetObject( "toolStripSplitButton1.Image" ) ) );
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size( 45, 22 );
			this.toolStripSplitButton1.Text = "GPR";
			// 
			// generalRegistersToolStripMenuItem
			// 
			this.generalRegistersToolStripMenuItem.Name = "generalRegistersToolStripMenuItem";
			this.generalRegistersToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1 ) ) );
			this.generalRegistersToolStripMenuItem.Size = new System.Drawing.Size( 204, 22 );
			this.generalRegistersToolStripMenuItem.Text = "&General Registers";
			// 
			// fPURegistersToolStripMenuItem
			// 
			this.fPURegistersToolStripMenuItem.Name = "fPURegistersToolStripMenuItem";
			this.fPURegistersToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2 ) ) );
			this.fPURegistersToolStripMenuItem.Size = new System.Drawing.Size( 204, 22 );
			this.fPURegistersToolStripMenuItem.Text = "&FPU Registers";
			// 
			// vFPURegistersToolStripMenuItem
			// 
			this.vFPURegistersToolStripMenuItem.Name = "vFPURegistersToolStripMenuItem";
			this.vFPURegistersToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3 ) ) );
			this.vFPURegistersToolStripMenuItem.Size = new System.Drawing.Size( 204, 22 );
			this.vFPURegistersToolStripMenuItem.Text = "&VFPU Registers";
			// 
			// disassemblyControl1
			// 
			this.disassemblyControl1.DisplayHex = true;
			this.disassemblyControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.disassemblyControl1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.disassemblyControl1.FormattingEnabled = true;
			this.disassemblyControl1.Location = new System.Drawing.Point( 0, 25 );
			this.disassemblyControl1.Name = "disassemblyControl1";
			this.disassemblyControl1.ScrollAlwaysVisible = true;
			this.disassemblyControl1.Size = new System.Drawing.Size( 583, 602 );
			this.disassemblyControl1.TabIndex = 2;
			// 
			// codeToolStrip
			// 
			this.codeToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripComboBox1,
            this.toolStripLabel1,
            this.hexToolStripButton} );
			this.codeToolStrip.Location = new System.Drawing.Point( 0, 0 );
			this.codeToolStrip.Name = "codeToolStrip";
			this.codeToolStrip.Size = new System.Drawing.Size( 583, 25 );
			this.codeToolStrip.TabIndex = 1;
			this.codeToolStrip.Text = "Code Tools";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ( ( System.Drawing.Image )( resources.GetObject( "toolStripButton1.Image" ) ) );
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size( 23, 22 );
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// toolStripComboBox1
			// 
			this.toolStripComboBox1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripComboBox1.Name = "toolStripComboBox1";
			this.toolStripComboBox1.Size = new System.Drawing.Size( 121, 25 );
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size( 48, 22 );
			this.toolStripLabel1.Text = "Browse:";
			// 
			// hexToolStripButton
			// 
			this.hexToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.hexToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hexToolStripButton.Name = "hexToolStripButton";
			this.hexToolStripButton.Size = new System.Drawing.Size( 31, 22 );
			this.hexToolStripButton.Text = "Hex";
			this.hexToolStripButton.Click += new System.EventHandler( this.hexToolStripButton_Click );
			// 
			// vfpuDisplay
			// 
			this.vfpuDisplay.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.vfpuDisplay.Location = new System.Drawing.Point( 3, 48 );
			this.vfpuDisplay.Name = "vfpuDisplay";
			this.vfpuDisplay.Size = new System.Drawing.Size( 201, 582 );
			this.vfpuDisplay.TabIndex = 4;
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font( "Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.textBox1.Location = new System.Drawing.Point( 118, 28 );
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size( 86, 18 );
			this.textBox1.TabIndex = 5;
			this.textBox1.Text = "0x00000000";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// CodeView
			// 
			this.ClientSize = new System.Drawing.Size( 794, 633 );
			this.CloseButton = false;
			this.Controls.Add( this.splitContainer1 );
			this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
			this.HideOnClose = true;
			this.Name = "CodeView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.TabText = "Code View";
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout( false );
			this.registersToolStrip.ResumeLayout( false );
			this.registersToolStrip.PerformLayout();
			this.codeToolStrip.ResumeLayout( false );
			this.codeToolStrip.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip registersToolStrip;
		private Noxa.Emulation.Psp.RemoteDebugger.Tools.DisassemblyControl disassemblyControl1;
		private System.Windows.Forms.ToolStrip codeToolStrip;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton hexToolStripButton;
		private System.Windows.Forms.Label pcLabel;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem generalRegistersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fPURegistersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vFPURegistersToolStripMenuItem;
		private System.Windows.Forms.ListView registersListView;
		private System.Windows.Forms.ColumnHeader registerNameHeader;
		private System.Windows.Forms.ColumnHeader registerValuePrettyHeader;
		private System.Windows.Forms.ColumnHeader registerValueRawHeader;
		private Noxa.Emulation.Psp.RemoteDebugger.Tools.VfpuDisplay vfpuDisplay;
		private System.Windows.Forms.TextBox textBox1;
	}
}
