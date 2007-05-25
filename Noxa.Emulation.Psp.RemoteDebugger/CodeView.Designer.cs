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
			this.registersToolStrip = new System.Windows.Forms.ToolStrip();
			this.disassemblyControl1 = new Noxa.Emulation.Psp.RemoteDebugger.Tools.DisassemblyControl();
			this.codeToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.hexToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add( this.registersToolStrip );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.disassemblyControl1 );
			this.splitContainer1.Panel2.Controls.Add( this.codeToolStrip );
			this.splitContainer1.Size = new System.Drawing.Size( 794, 633 );
			this.splitContainer1.SplitterDistance = 207;
			this.splitContainer1.TabIndex = 1;
			// 
			// registersToolStrip
			// 
			this.registersToolStrip.Location = new System.Drawing.Point( 0, 0 );
			this.registersToolStrip.Name = "registersToolStrip";
			this.registersToolStrip.Size = new System.Drawing.Size( 207, 25 );
			this.registersToolStrip.TabIndex = 0;
			this.registersToolStrip.Text = "Register Tools";
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
	}
}
