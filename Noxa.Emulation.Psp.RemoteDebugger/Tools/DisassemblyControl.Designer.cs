// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class DisassemblyControl
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
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.lineContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.valueToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.copyOperandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.goToTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.showNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.runToCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.copyLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyInstructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.lineBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gutterContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.toggleBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.addBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lineContextMenuStrip.SuspendLayout();
			this.gutterContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 5000;
			this.toolTip1.InitialDelay = 250;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler( this.toolTip1_Popup );
			this.toolTip1.Draw += new System.Windows.Forms.DrawToolTipEventHandler( this.toolTip1_Draw );
			// 
			// lineContextMenuStrip
			// 
			this.lineContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.valueToolStripTextBox,
            this.toolStripSeparator5,
            this.copyOperandToolStripMenuItem,
            this.goToTargetToolStripMenuItem,
            this.toolStripSeparator4,
            this.showNextToolStripMenuItem,
            this.runToCursorToolStripMenuItem,
            this.setNextToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyLineToolStripMenuItem,
            this.copyAddressToolStripMenuItem,
            this.copyInstructionToolStripMenuItem,
            this.toolStripSeparator3,
            this.lineBreakpointToolStripMenuItem} );
			this.lineContextMenuStrip.Name = "addressContextMenuStrip";
			this.lineContextMenuStrip.Size = new System.Drawing.Size( 188, 249 );
			this.lineContextMenuStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler( this.GeneralContextClosed );
			this.lineContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler( this.lineContextMenuStrip_Opening );
			// 
			// valueToolStripTextBox
			// 
			this.valueToolStripTextBox.AutoSize = false;
			this.valueToolStripTextBox.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.valueToolStripTextBox.Name = "valueToolStripTextBox";
			this.valueToolStripTextBox.Size = new System.Drawing.Size( 100, 21 );
			this.valueToolStripTextBox.Text = "0xDEADBEEF";
			this.valueToolStripTextBox.TextChanged += new System.EventHandler( this.valueToolStripTextBox_TextChanged );
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size( 184, 6 );
			// 
			// copyOperandToolStripMenuItem
			// 
			this.copyOperandToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.CopyIcon;
			this.copyOperandToolStripMenuItem.Name = "copyOperandToolStripMenuItem";
			this.copyOperandToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.copyOperandToolStripMenuItem.Text = "Copy &Value";
			this.copyOperandToolStripMenuItem.Click += new System.EventHandler( this.copyOperandToolStripMenuItem_Click );
			// 
			// goToTargetToolStripMenuItem
			// 
			this.goToTargetToolStripMenuItem.Name = "goToTargetToolStripMenuItem";
			this.goToTargetToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.goToTargetToolStripMenuItem.Text = "&Go to Target";
			this.goToTargetToolStripMenuItem.Click += new System.EventHandler( this.goToTargetToolStripMenuItem_Click );
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size( 184, 6 );
			// 
			// showNextToolStripMenuItem
			// 
			this.showNextToolStripMenuItem.Name = "showNextToolStripMenuItem";
			this.showNextToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.showNextToolStripMenuItem.Text = "Show &Next Statement";
			this.showNextToolStripMenuItem.Click += new System.EventHandler( this.showNextToolStripMenuItem_Click );
			// 
			// runToCursorToolStripMenuItem
			// 
			this.runToCursorToolStripMenuItem.Name = "runToCursorToolStripMenuItem";
			this.runToCursorToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.runToCursorToolStripMenuItem.Text = "&Run to Cursor";
			this.runToCursorToolStripMenuItem.Click += new System.EventHandler( this.runToCursorToolStripMenuItem_Click );
			// 
			// setNextToolStripMenuItem
			// 
			this.setNextToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StatementIcon;
			this.setNextToolStripMenuItem.Name = "setNextToolStripMenuItem";
			this.setNextToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.setNextToolStripMenuItem.Text = "&Set Next Statement";
			this.setNextToolStripMenuItem.Click += new System.EventHandler( this.setNextToolStripMenuItem_Click );
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 184, 6 );
			// 
			// copyLineToolStripMenuItem
			// 
			this.copyLineToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.CopyIcon;
			this.copyLineToolStripMenuItem.Name = "copyLineToolStripMenuItem";
			this.copyLineToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.copyLineToolStripMenuItem.Text = "Copy &Line";
			this.copyLineToolStripMenuItem.Click += new System.EventHandler( this.copyLineToolStripMenuItem_Click );
			// 
			// copyAddressToolStripMenuItem
			// 
			this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
			this.copyAddressToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.copyAddressToolStripMenuItem.Text = "Copy &Address";
			this.copyAddressToolStripMenuItem.Click += new System.EventHandler( this.copyAddressToolStripMenuItem_Click );
			// 
			// copyInstructionToolStripMenuItem
			// 
			this.copyInstructionToolStripMenuItem.Name = "copyInstructionToolStripMenuItem";
			this.copyInstructionToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.copyInstructionToolStripMenuItem.Text = "Copy &Instruction";
			this.copyInstructionToolStripMenuItem.Click += new System.EventHandler( this.copyInstructionToolStripMenuItem_Click );
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size( 184, 6 );
			// 
			// lineBreakpointToolStripMenuItem
			// 
			this.lineBreakpointToolStripMenuItem.DropDown = this.gutterContextMenuStrip;
			this.lineBreakpointToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.BreakpointIcon;
			this.lineBreakpointToolStripMenuItem.Name = "lineBreakpointToolStripMenuItem";
			this.lineBreakpointToolStripMenuItem.Size = new System.Drawing.Size( 187, 22 );
			this.lineBreakpointToolStripMenuItem.Text = "&Breakpoint";
			// 
			// gutterContextMenuStrip
			// 
			this.gutterContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toggleBreakpointToolStripMenuItem,
            this.renameBreakpointToolStripMenuItem,
            this.toolStripSeparator1,
            this.addBreakpointToolStripMenuItem,
            this.removeBreakpointToolStripMenuItem} );
			this.gutterContextMenuStrip.Name = "gutterContextMenuStrip";
			this.gutterContextMenuStrip.Size = new System.Drawing.Size( 178, 98 );
			this.gutterContextMenuStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler( this.GeneralContextClosed );
			this.gutterContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler( this.gutterContextMenuStrip_Opening );
			// 
			// toggleBreakpointToolStripMenuItem
			// 
			this.toggleBreakpointToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.ToggleBreakpointIcon;
			this.toggleBreakpointToolStripMenuItem.Name = "toggleBreakpointToolStripMenuItem";
			this.toggleBreakpointToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
			this.toggleBreakpointToolStripMenuItem.Text = "&Toggle Breakpoint";
			this.toggleBreakpointToolStripMenuItem.Click += new System.EventHandler( this.toggleBreakpointToolStripMenuItem_Click );
			// 
			// renameBreakpointToolStripMenuItem
			// 
			this.renameBreakpointToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.RenameIcon;
			this.renameBreakpointToolStripMenuItem.Name = "renameBreakpointToolStripMenuItem";
			this.renameBreakpointToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
			this.renameBreakpointToolStripMenuItem.Text = "&Rename Breakpoint";
			this.renameBreakpointToolStripMenuItem.Click += new System.EventHandler( this.renameBreakpointToolStripMenuItem_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 174, 6 );
			// 
			// addBreakpointToolStripMenuItem
			// 
			this.addBreakpointToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.NewBreakpointIcon;
			this.addBreakpointToolStripMenuItem.Name = "addBreakpointToolStripMenuItem";
			this.addBreakpointToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
			this.addBreakpointToolStripMenuItem.Text = "&Add Breakpoint";
			this.addBreakpointToolStripMenuItem.Click += new System.EventHandler( this.addBreakpointToolStripMenuItem_Click );
			// 
			// removeBreakpointToolStripMenuItem
			// 
			this.removeBreakpointToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.DeleteBreakpointIcon;
			this.removeBreakpointToolStripMenuItem.Name = "removeBreakpointToolStripMenuItem";
			this.removeBreakpointToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
			this.removeBreakpointToolStripMenuItem.Text = "&Delete Breakpoint";
			this.removeBreakpointToolStripMenuItem.Click += new System.EventHandler( this.removeBreakpointToolStripMenuItem_Click );
			// 
			// DisassemblyControl
			// 
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ScrollAlwaysVisible = true;
			this.Size = new System.Drawing.Size( 120, 95 );
			this.lineContextMenuStrip.ResumeLayout( false );
			this.lineContextMenuStrip.PerformLayout();
			this.gutterContextMenuStrip.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ContextMenuStrip lineContextMenuStrip;
		private System.Windows.Forms.ContextMenuStrip gutterContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem toggleBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem addBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showNextToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runToCursorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setNextToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyInstructionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem lineBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox valueToolStripTextBox;
		private System.Windows.Forms.ToolStripMenuItem copyOperandToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem goToTargetToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
	}
}
