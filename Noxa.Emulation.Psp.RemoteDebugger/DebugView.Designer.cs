// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class DebugView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DebugView ) );
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.breakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.showNextStatementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.codeViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.callstackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.biosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.threadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.watchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.windowsToolStrip = new System.Windows.Forms.ToolStrip();
			this.codeViewToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.memoryToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.callstackToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.biosToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.threadsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.breakpointsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.watchesToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.statisticsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.logViewToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.controlToolStrip = new System.Windows.Forms.ToolStrip();
			this.resumeToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.breakToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.restartToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.showStatementToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepIntoToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepOverToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepOutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.hexToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.locationToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.threadToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.frameToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.windowsToolStrip.SuspendLayout();
			this.controlToolStrip.SuspendLayout();
			this.locationToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 873, 24 );
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem} );
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size( 37, 20 );
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "newToolStripMenuItem.Image" ) ) );
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N ) ) );
			this.newToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "openToolStripMenuItem.Image" ) ) );
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O ) ) );
			this.openToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size( 143, 6 );
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "saveToolStripMenuItem.Image" ) ) );
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S ) ) );
			this.saveToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.saveAsToolStripMenuItem.Text = "Save &As";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 143, 6 );
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "printToolStripMenuItem.Image" ) ) );
			this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P ) ) );
			this.printToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.printToolStripMenuItem.Text = "&Print";
			// 
			// printPreviewToolStripMenuItem
			// 
			this.printPreviewToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "printPreviewToolStripMenuItem.Image" ) ) );
			this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
			this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 143, 6 );
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 146, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem} );
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size( 39, 20 );
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z ) ) );
			this.undoToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.undoToolStripMenuItem.Text = "&Undo";
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y ) ) );
			this.redoToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.redoToolStripMenuItem.Text = "&Redo";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size( 141, 6 );
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "cutToolStripMenuItem.Image" ) ) );
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X ) ) );
			this.cutToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.cutToolStripMenuItem.Text = "Cu&t";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "copyToolStripMenuItem.Image" ) ) );
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C ) ) );
			this.copyToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.copyToolStripMenuItem.Text = "&Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ( ( System.Drawing.Image )( resources.GetObject( "pasteToolStripMenuItem.Image" ) ) );
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V ) ) );
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.pasteToolStripMenuItem.Text = "&Paste";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size( 141, 6 );
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size( 144, 22 );
			this.selectAllToolStripMenuItem.Text = "Select &All";
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.resumeToolStripMenuItem,
            this.breakToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.toolStripMenuItem3,
            this.showNextStatementToolStripMenuItem,
            this.stepIntoToolStripMenuItem,
            this.stepOverToolStripMenuItem,
            this.stepOutToolStripMenuItem} );
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size( 54, 20 );
			this.debugToolStripMenuItem.Text = "&Debug";
			// 
			// resumeToolStripMenuItem
			// 
			this.resumeToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.PlayIcon;
			this.resumeToolStripMenuItem.Name = "resumeToolStripMenuItem";
			this.resumeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.resumeToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.resumeToolStripMenuItem.Text = "&Resume";
			this.resumeToolStripMenuItem.Click += new System.EventHandler( this.resumeToolStripMenuItem_Click );
			// 
			// breakToolStripMenuItem
			// 
			this.breakToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.PauseIcon;
			this.breakToolStripMenuItem.Name = "breakToolStripMenuItem";
			this.breakToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt )
						| System.Windows.Forms.Keys.B ) ) );
			this.breakToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.breakToolStripMenuItem.Text = "&Break";
			this.breakToolStripMenuItem.Click += new System.EventHandler( this.breakToolStripMenuItem_Click );
			// 
			// stopToolStripMenuItem
			// 
			this.stopToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StopIcon;
			this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
			this.stopToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5 ) ) );
			this.stopToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.stopToolStripMenuItem.Text = "&Stop";
			this.stopToolStripMenuItem.Click += new System.EventHandler( this.stopToolStripMenuItem_Click );
			// 
			// restartToolStripMenuItem
			// 
			this.restartToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.RestartIcon;
			this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
			this.restartToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( ( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift )
						| System.Windows.Forms.Keys.F5 ) ) );
			this.restartToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.restartToolStripMenuItem.Text = "R&estart";
			this.restartToolStripMenuItem.Click += new System.EventHandler( this.restartToolStripMenuItem_Click );
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size( 203, 6 );
			// 
			// showNextStatementToolStripMenuItem
			// 
			this.showNextStatementToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StatementIcon;
			this.showNextStatementToolStripMenuItem.Name = "showNextStatementToolStripMenuItem";
			this.showNextStatementToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.showNextStatementToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.showNextStatementToolStripMenuItem.Text = "Show &Next Statement";
			this.showNextStatementToolStripMenuItem.Click += new System.EventHandler( this.showNextStatementToolStripMenuItem_Click );
			// 
			// stepIntoToolStripMenuItem
			// 
			this.stepIntoToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepIntoIcon;
			this.stepIntoToolStripMenuItem.Name = "stepIntoToolStripMenuItem";
			this.stepIntoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.stepIntoToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.stepIntoToolStripMenuItem.Text = "Step &Into";
			this.stepIntoToolStripMenuItem.Click += new System.EventHandler( this.stepIntoToolStripMenuItem_Click );
			// 
			// stepOverToolStripMenuItem
			// 
			this.stepOverToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepOverIcon;
			this.stepOverToolStripMenuItem.Name = "stepOverToolStripMenuItem";
			this.stepOverToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.stepOverToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.stepOverToolStripMenuItem.Text = "Step &Over";
			this.stepOverToolStripMenuItem.Click += new System.EventHandler( this.stepOverToolStripMenuItem_Click );
			// 
			// stepOutToolStripMenuItem
			// 
			this.stepOutToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepOutIcon;
			this.stepOutToolStripMenuItem.Name = "stepOutToolStripMenuItem";
			this.stepOutToolStripMenuItem.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11 ) ) );
			this.stepOutToolStripMenuItem.Size = new System.Drawing.Size( 206, 22 );
			this.stepOutToolStripMenuItem.Text = "Step Ou&t";
			this.stepOutToolStripMenuItem.Click += new System.EventHandler( this.stepOutToolStripMenuItem_Click );
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem} );
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size( 48, 20 );
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// customizeToolStripMenuItem
			// 
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.customizeToolStripMenuItem.Text = "&Customize";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OptionsIcon;
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// windowToolStripMenuItem
			// 
			this.windowToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.codeViewToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.callstackToolStripMenuItem,
            this.toolStripMenuItem2,
            this.biosToolStripMenuItem,
            this.threadsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.breakpointsToolStripMenuItem,
            this.watchesToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.logViewToolStripMenuItem} );
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			this.windowToolStripMenuItem.Size = new System.Drawing.Size( 63, 20 );
			this.windowToolStripMenuItem.Text = "&Window";
			// 
			// codeViewToolStripMenuItem
			// 
			this.codeViewToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.DisassemblyIcon;
			this.codeViewToolStripMenuItem.Name = "codeViewToolStripMenuItem";
			this.codeViewToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.codeViewToolStripMenuItem.Text = "&Code View";
			this.codeViewToolStripMenuItem.Click += new System.EventHandler( this.codeViewToolStripButton_Click );
			// 
			// memoryToolStripMenuItem
			// 
			this.memoryToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.MemoryIcon;
			this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
			this.memoryToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.memoryToolStripMenuItem.Text = "&Memory";
			this.memoryToolStripMenuItem.Click += new System.EventHandler( this.memoryToolStripButton_Click );
			// 
			// callstackToolStripMenuItem
			// 
			this.callstackToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.CallstackIcon;
			this.callstackToolStripMenuItem.Name = "callstackToolStripMenuItem";
			this.callstackToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.callstackToolStripMenuItem.Text = "&Callstack";
			this.callstackToolStripMenuItem.Click += new System.EventHandler( this.callstackToolStripButton_Click );
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size( 148, 6 );
			// 
			// biosToolStripMenuItem
			// 
			this.biosToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OsIcon;
			this.biosToolStripMenuItem.Name = "biosToolStripMenuItem";
			this.biosToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.biosToolStripMenuItem.Text = "&BIOS Inspector";
			this.biosToolStripMenuItem.Click += new System.EventHandler( this.biosToolStripButton_Click );
			// 
			// threadsToolStripMenuItem
			// 
			this.threadsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.ThreadsIcon;
			this.threadsToolStripMenuItem.Name = "threadsToolStripMenuItem";
			this.threadsToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.threadsToolStripMenuItem.Text = "&Threads";
			this.threadsToolStripMenuItem.Click += new System.EventHandler( this.threadsToolStripButton_Click );
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size( 148, 6 );
			// 
			// breakpointsToolStripMenuItem
			// 
			this.breakpointsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.BreakpointsIcon;
			this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
			this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.breakpointsToolStripMenuItem.Text = "B&reakpoints";
			this.breakpointsToolStripMenuItem.Click += new System.EventHandler( this.breakpointsToolStripButton_Click );
			// 
			// watchesToolStripMenuItem
			// 
			this.watchesToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.WatchesIcon;
			this.watchesToolStripMenuItem.Name = "watchesToolStripMenuItem";
			this.watchesToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.watchesToolStripMenuItem.Text = "&Watches";
			this.watchesToolStripMenuItem.Click += new System.EventHandler( this.watchesToolStripButton_Click );
			// 
			// statisticsToolStripMenuItem
			// 
			this.statisticsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StatisticsIcon;
			this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
			this.statisticsToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.statisticsToolStripMenuItem.Text = "&Statistics";
			this.statisticsToolStripMenuItem.Click += new System.EventHandler( this.statisticsToolStripButton_Click );
			// 
			// logViewToolStripMenuItem
			// 
			this.logViewToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OutputIcon;
			this.logViewToolStripMenuItem.Name = "logViewToolStripMenuItem";
			this.logViewToolStripMenuItem.Size = new System.Drawing.Size( 151, 22 );
			this.logViewToolStripMenuItem.Text = "&Log View";
			this.logViewToolStripMenuItem.Click += new System.EventHandler( this.logViewToolStripButton_Click );
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem} );
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.HelpContentsIcon;
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.HelpIndexIcon;
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size( 119, 6 );
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1} );
			this.statusStrip1.Location = new System.Drawing.Point( 0, 642 );
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size( 873, 22 );
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size( 39, 17 );
			this.toolStripStatusLabel1.Text = "Ready";
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add( this.dockPanel1 );
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size( 873, 568 );
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size( 873, 618 );
			this.toolStripContainer1.TabIndex = 2;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.windowsToolStrip );
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.controlToolStrip );
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.locationToolStrip );
			// 
			// dockPanel1
			// 
			this.dockPanel1.ActiveAutoHideContent = null;
			this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dockPanel1.Font = new System.Drawing.Font( "Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World );
			this.dockPanel1.Location = new System.Drawing.Point( 0, 0 );
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.Size = new System.Drawing.Size( 873, 568 );
			this.dockPanel1.TabIndex = 0;
			// 
			// windowsToolStrip
			// 
			this.windowsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.windowsToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.codeViewToolStripButton,
            this.memoryToolStripButton,
            this.callstackToolStripButton,
            this.toolStripSeparator9,
            this.biosToolStripButton,
            this.threadsToolStripButton,
            this.toolStripSeparator10,
            this.breakpointsToolStripButton,
            this.watchesToolStripButton,
            this.statisticsToolStripButton,
            this.logViewToolStripButton} );
			this.windowsToolStrip.Location = new System.Drawing.Point( 3, 0 );
			this.windowsToolStrip.Name = "windowsToolStrip";
			this.windowsToolStrip.Size = new System.Drawing.Size( 231, 25 );
			this.windowsToolStrip.TabIndex = 1;
			// 
			// codeViewToolStripButton
			// 
			this.codeViewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.codeViewToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.DisassemblyIcon;
			this.codeViewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.codeViewToolStripButton.Name = "codeViewToolStripButton";
			this.codeViewToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.codeViewToolStripButton.Text = "Code View";
			this.codeViewToolStripButton.Click += new System.EventHandler( this.codeViewToolStripButton_Click );
			// 
			// memoryToolStripButton
			// 
			this.memoryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.memoryToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.MemoryIcon;
			this.memoryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.memoryToolStripButton.Name = "memoryToolStripButton";
			this.memoryToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.memoryToolStripButton.Text = "Memory";
			this.memoryToolStripButton.Click += new System.EventHandler( this.memoryToolStripButton_Click );
			// 
			// callstackToolStripButton
			// 
			this.callstackToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.callstackToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.CallstackIcon;
			this.callstackToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.callstackToolStripButton.Name = "callstackToolStripButton";
			this.callstackToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.callstackToolStripButton.Text = "Callstack";
			this.callstackToolStripButton.Click += new System.EventHandler( this.callstackToolStripButton_Click );
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size( 6, 25 );
			// 
			// biosToolStripButton
			// 
			this.biosToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.biosToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OsIcon;
			this.biosToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.biosToolStripButton.Name = "biosToolStripButton";
			this.biosToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.biosToolStripButton.Text = "BIOS Information";
			this.biosToolStripButton.Click += new System.EventHandler( this.biosToolStripButton_Click );
			// 
			// threadsToolStripButton
			// 
			this.threadsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.threadsToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.ThreadsIcon;
			this.threadsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.threadsToolStripButton.Name = "threadsToolStripButton";
			this.threadsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.threadsToolStripButton.Text = "Threads";
			this.threadsToolStripButton.Click += new System.EventHandler( this.threadsToolStripButton_Click );
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size( 6, 25 );
			// 
			// breakpointsToolStripButton
			// 
			this.breakpointsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.breakpointsToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.BreakpointsIcon;
			this.breakpointsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.breakpointsToolStripButton.Name = "breakpointsToolStripButton";
			this.breakpointsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.breakpointsToolStripButton.Text = "Breakpoints";
			this.breakpointsToolStripButton.Click += new System.EventHandler( this.breakpointsToolStripButton_Click );
			// 
			// watchesToolStripButton
			// 
			this.watchesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.watchesToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.WatchesIcon;
			this.watchesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.watchesToolStripButton.Name = "watchesToolStripButton";
			this.watchesToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.watchesToolStripButton.Text = "Watches";
			this.watchesToolStripButton.Click += new System.EventHandler( this.watchesToolStripButton_Click );
			// 
			// statisticsToolStripButton
			// 
			this.statisticsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.statisticsToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StatisticsIcon;
			this.statisticsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.statisticsToolStripButton.Name = "statisticsToolStripButton";
			this.statisticsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.statisticsToolStripButton.Text = "Statistics";
			this.statisticsToolStripButton.Click += new System.EventHandler( this.statisticsToolStripButton_Click );
			// 
			// logViewToolStripButton
			// 
			this.logViewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.logViewToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OutputIcon;
			this.logViewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.logViewToolStripButton.Name = "logViewToolStripButton";
			this.logViewToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.logViewToolStripButton.Text = "Log View";
			this.logViewToolStripButton.Click += new System.EventHandler( this.logViewToolStripButton_Click );
			// 
			// controlToolStrip
			// 
			this.controlToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.controlToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.resumeToolStripButton,
            this.breakToolStripButton,
            this.stopToolStripButton,
            this.restartToolStripButton,
            this.toolStripSeparator6,
            this.showStatementToolStripButton,
            this.stepIntoToolStripButton,
            this.stepOverToolStripButton,
            this.stepOutToolStripButton,
            this.toolStripSeparator7,
            this.hexToolStripButton} );
			this.controlToolStrip.Location = new System.Drawing.Point( 3, 25 );
			this.controlToolStrip.Name = "controlToolStrip";
			this.controlToolStrip.Size = new System.Drawing.Size( 239, 25 );
			this.controlToolStrip.TabIndex = 0;
			// 
			// resumeToolStripButton
			// 
			this.resumeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.resumeToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.PlayIcon;
			this.resumeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.resumeToolStripButton.Name = "resumeToolStripButton";
			this.resumeToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.resumeToolStripButton.Text = "Resume";
			this.resumeToolStripButton.ToolTipText = "Resume Execution";
			this.resumeToolStripButton.Click += new System.EventHandler( this.resumeToolStripMenuItem_Click );
			// 
			// breakToolStripButton
			// 
			this.breakToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.breakToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.PauseIcon;
			this.breakToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.breakToolStripButton.Name = "breakToolStripButton";
			this.breakToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.breakToolStripButton.Text = "Break";
			this.breakToolStripButton.ToolTipText = "Break Execution";
			this.breakToolStripButton.Click += new System.EventHandler( this.breakToolStripMenuItem_Click );
			// 
			// stopToolStripButton
			// 
			this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stopToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StopIcon;
			this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stopToolStripButton.Name = "stopToolStripButton";
			this.stopToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stopToolStripButton.Text = "Stop";
			this.stopToolStripButton.ToolTipText = "Stop Execution";
			this.stopToolStripButton.Click += new System.EventHandler( this.stopToolStripMenuItem_Click );
			// 
			// restartToolStripButton
			// 
			this.restartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.restartToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.RestartIcon;
			this.restartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.restartToolStripButton.Name = "restartToolStripButton";
			this.restartToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.restartToolStripButton.Text = "Restart";
			this.restartToolStripButton.Click += new System.EventHandler( this.restartToolStripMenuItem_Click );
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size( 6, 25 );
			// 
			// showStatementToolStripButton
			// 
			this.showStatementToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showStatementToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StatementIcon;
			this.showStatementToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showStatementToolStripButton.Name = "showStatementToolStripButton";
			this.showStatementToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.showStatementToolStripButton.Text = "Show Next Statement";
			this.showStatementToolStripButton.Click += new System.EventHandler( this.showNextStatementToolStripMenuItem_Click );
			// 
			// stepIntoToolStripButton
			// 
			this.stepIntoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepIntoToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepIntoIcon;
			this.stepIntoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepIntoToolStripButton.Name = "stepIntoToolStripButton";
			this.stepIntoToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepIntoToolStripButton.Text = "Step Into";
			this.stepIntoToolStripButton.Click += new System.EventHandler( this.stepIntoToolStripMenuItem_Click );
			// 
			// stepOverToolStripButton
			// 
			this.stepOverToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepOverToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepOverIcon;
			this.stepOverToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepOverToolStripButton.Name = "stepOverToolStripButton";
			this.stepOverToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepOverToolStripButton.Text = "Step Over";
			this.stepOverToolStripButton.Click += new System.EventHandler( this.stepOverToolStripMenuItem_Click );
			// 
			// stepOutToolStripButton
			// 
			this.stepOutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepOutToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.StepOutIcon;
			this.stepOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepOutToolStripButton.Name = "stepOutToolStripButton";
			this.stepOutToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepOutToolStripButton.Text = "Step Out";
			this.stepOutToolStripButton.Click += new System.EventHandler( this.stepOutToolStripMenuItem_Click );
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size( 6, 25 );
			// 
			// hexToolStripButton
			// 
			this.hexToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.hexToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hexToolStripButton.Name = "hexToolStripButton";
			this.hexToolStripButton.Size = new System.Drawing.Size( 31, 22 );
			this.hexToolStripButton.Text = "Hex";
			this.hexToolStripButton.ToolTipText = "Show Numbers in Hexidecimal";
			// 
			// locationToolStrip
			// 
			this.locationToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.locationToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.threadToolStripComboBox,
            this.toolStripLabel2,
            this.frameToolStripComboBox} );
			this.locationToolStrip.Location = new System.Drawing.Point( 242, 25 );
			this.locationToolStrip.Name = "locationToolStrip";
			this.locationToolStrip.Size = new System.Drawing.Size( 348, 25 );
			this.locationToolStrip.TabIndex = 2;
			this.locationToolStrip.Visible = false;
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size( 47, 22 );
			this.toolStripLabel1.Text = "Thread:";
			// 
			// threadToolStripComboBox
			// 
			this.threadToolStripComboBox.Name = "threadToolStripComboBox";
			this.threadToolStripComboBox.Size = new System.Drawing.Size( 121, 25 );
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size( 43, 22 );
			this.toolStripLabel2.Text = "Frame:";
			// 
			// frameToolStripComboBox
			// 
			this.frameToolStripComboBox.Name = "frameToolStripComboBox";
			this.frameToolStripComboBox.Size = new System.Drawing.Size( 121, 25 );
			// 
			// DebugView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 873, 664 );
			this.Controls.Add( this.toolStripContainer1 );
			this.Controls.Add( this.statusStrip1 );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "DebugView";
			this.Text = "PSP Player Debugger";
			this.Load += new System.EventHandler( this.DebugView_Load );
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout( false );
			this.statusStrip1.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout( false );
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout( false );
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout( false );
			this.toolStripContainer1.PerformLayout();
			this.windowsToolStrip.ResumeLayout( false );
			this.windowsToolStrip.PerformLayout();
			this.controlToolStrip.ResumeLayout( false );
			this.controlToolStrip.PerformLayout();
			this.locationToolStrip.ResumeLayout( false );
			this.locationToolStrip.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStrip windowsToolStrip;
		private System.Windows.Forms.ToolStripButton codeViewToolStripButton;
		private System.Windows.Forms.ToolStripButton memoryToolStripButton;
		private System.Windows.Forms.ToolStripButton biosToolStripButton;
		private System.Windows.Forms.ToolStripButton threadsToolStripButton;
		private System.Windows.Forms.ToolStripButton callstackToolStripButton;
		private System.Windows.Forms.ToolStripButton breakpointsToolStripButton;
		private System.Windows.Forms.ToolStripButton watchesToolStripButton;
		private System.Windows.Forms.ToolStripButton logViewToolStripButton;
		private System.Windows.Forms.ToolStripButton statisticsToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem codeViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem callstackToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem biosToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem threadsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem watchesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ToolStrip controlToolStrip;
		private System.Windows.Forms.ToolStrip locationToolStrip;
		private System.Windows.Forms.ToolStripButton resumeToolStripButton;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripComboBox threadToolStripComboBox;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripComboBox frameToolStripComboBox;
		private System.Windows.Forms.ToolStripButton breakToolStripButton;
		private System.Windows.Forms.ToolStripButton stopToolStripButton;
		private System.Windows.Forms.ToolStripButton restartToolStripButton;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton showStatementToolStripButton;
		private System.Windows.Forms.ToolStripButton stepIntoToolStripButton;
		private System.Windows.Forms.ToolStripButton stepOverToolStripButton;
		private System.Windows.Forms.ToolStripButton stepOutToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton hexToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem resumeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem breakToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem showNextStatementToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepIntoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepOverToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepOutToolStripMenuItem;
	}
}

