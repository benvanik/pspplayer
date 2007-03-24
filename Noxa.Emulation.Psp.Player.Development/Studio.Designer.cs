// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player.Development
{
	partial class Studio
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Studio ) );
			this.menuStrip = new System.Windows.Forms.MenuStrip();
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
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.messageStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.stateStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.dockPanel = new WeifenLuo.WinFormsUI.DockPanel();
			this.controlToolStrip = new System.Windows.Forms.ToolStrip();
			this.continueToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.breakToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.restartToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.showStatementToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepIntoToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepOverToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.stepOutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.hexDisplayToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.registersToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.memoryToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.callstackToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.threadsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.osToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.breakpointsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.watchesToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.controlToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem} );
			this.menuStrip.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size( 841, 24 );
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
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
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.optionsToolStripMenuItem.Text = "&Options";
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
			this.contentsToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.HelpContentsIcon;
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.HelpIndexIcon;
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
			// statusStrip
			// 
			this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.messageStripStatusLabel,
            this.stateStripStatusLabel} );
			this.statusStrip.Location = new System.Drawing.Point( 0, 636 );
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size( 841, 22 );
			this.statusStrip.TabIndex = 3;
			this.statusStrip.Text = "statusStrip1";
			// 
			// messageStripStatusLabel
			// 
			this.messageStripStatusLabel.Name = "messageStripStatusLabel";
			this.messageStripStatusLabel.Size = new System.Drawing.Size( 708, 17 );
			this.messageStripStatusLabel.Spring = true;
			this.messageStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// stateStripStatusLabel
			// 
			this.stateStripStatusLabel.AutoSize = false;
			this.stateStripStatusLabel.Name = "stateStripStatusLabel";
			this.stateStripStatusLabel.Size = new System.Drawing.Size( 118, 17 );
			this.stateStripStatusLabel.Text = "Debugging";
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add( this.dockPanel );
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size( 841, 587 );
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size( 841, 612 );
			this.toolStripContainer1.TabIndex = 4;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.controlToolStrip );
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.DocumentStyles.DockingWindow;
			this.dockPanel.Font = new System.Drawing.Font( "Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World );
			this.dockPanel.Location = new System.Drawing.Point( 0, 0 );
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.ShowDocumentIcon = true;
			this.dockPanel.Size = new System.Drawing.Size( 841, 587 );
			this.dockPanel.TabIndex = 0;
			// 
			// controlToolStrip
			// 
			this.controlToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.controlToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.continueToolStripButton,
            this.breakToolStripButton,
            this.stopToolStripButton,
            this.restartToolStripButton,
            this.toolStripSeparator6,
            this.showStatementToolStripButton,
            this.stepIntoToolStripButton,
            this.stepOverToolStripButton,
            this.stepOutToolStripButton,
            this.toolStripSeparator7,
            this.hexDisplayToolStripButton,
            this.toolStripSeparator8,
            this.registersToolStripButton,
            this.memoryToolStripButton,
            this.callstackToolStripButton,
            this.threadsToolStripButton,
            this.osToolStripButton,
            this.toolStripSeparator9,
            this.breakpointsToolStripButton,
            this.watchesToolStripButton} );
			this.controlToolStrip.Location = new System.Drawing.Point( 3, 0 );
			this.controlToolStrip.Name = "controlToolStrip";
			this.controlToolStrip.Size = new System.Drawing.Size( 443, 25 );
			this.controlToolStrip.TabIndex = 1;
			// 
			// continueToolStripButton
			// 
			this.continueToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.continueToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.PlayIcon;
			this.continueToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.continueToolStripButton.Name = "continueToolStripButton";
			this.continueToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.continueToolStripButton.Text = "Continue";
			this.continueToolStripButton.Click += new System.EventHandler( this.continueToolStripButton_Click );
			// 
			// breakToolStripButton
			// 
			this.breakToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.breakToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.PauseIcon;
			this.breakToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.breakToolStripButton.Name = "breakToolStripButton";
			this.breakToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.breakToolStripButton.Text = "Break All";
			this.breakToolStripButton.Click += new System.EventHandler( this.breakToolStripButton_Click );
			// 
			// stopToolStripButton
			// 
			this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stopToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.StopIcon;
			this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stopToolStripButton.Name = "stopToolStripButton";
			this.stopToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stopToolStripButton.Text = "Stop Debugging";
			this.stopToolStripButton.Click += new System.EventHandler( this.stopToolStripButton_Click );
			// 
			// restartToolStripButton
			// 
			this.restartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.restartToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.RestartIcon;
			this.restartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.restartToolStripButton.Name = "restartToolStripButton";
			this.restartToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.restartToolStripButton.Text = "Restart";
			this.restartToolStripButton.Click += new System.EventHandler( this.restartToolStripButton_Click );
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size( 6, 25 );
			// 
			// showStatementToolStripButton
			// 
			this.showStatementToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showStatementToolStripButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "showStatementToolStripButton.Image" ) ) );
			this.showStatementToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showStatementToolStripButton.Name = "showStatementToolStripButton";
			this.showStatementToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.showStatementToolStripButton.Text = "Show Next Statement";
			this.showStatementToolStripButton.Click += new System.EventHandler( this.showStatementToolStripButton_Click );
			// 
			// stepIntoToolStripButton
			// 
			this.stepIntoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepIntoToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.StepIntoIcon;
			this.stepIntoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepIntoToolStripButton.Name = "stepIntoToolStripButton";
			this.stepIntoToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepIntoToolStripButton.Text = "Step Into";
			this.stepIntoToolStripButton.Click += new System.EventHandler( this.stepIntoToolStripButton_Click );
			// 
			// stepOverToolStripButton
			// 
			this.stepOverToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepOverToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.StepOverIcon;
			this.stepOverToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepOverToolStripButton.Name = "stepOverToolStripButton";
			this.stepOverToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepOverToolStripButton.Text = "Step Over";
			this.stepOverToolStripButton.Click += new System.EventHandler( this.stepOverToolStripButton_Click );
			// 
			// stepOutToolStripButton
			// 
			this.stepOutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stepOutToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.StepOutIcon;
			this.stepOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stepOutToolStripButton.Name = "stepOutToolStripButton";
			this.stepOutToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.stepOutToolStripButton.Text = "Step Out";
			this.stepOutToolStripButton.Click += new System.EventHandler( this.stepOutToolStripButton_Click );
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size( 6, 25 );
			// 
			// hexDisplayToolStripButton
			// 
			this.hexDisplayToolStripButton.Checked = true;
			this.hexDisplayToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.hexDisplayToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.hexDisplayToolStripButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "hexDisplayToolStripButton.Image" ) ) );
			this.hexDisplayToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hexDisplayToolStripButton.Name = "hexDisplayToolStripButton";
			this.hexDisplayToolStripButton.Size = new System.Drawing.Size( 31, 22 );
			this.hexDisplayToolStripButton.Text = "Hex";
			this.hexDisplayToolStripButton.Click += new System.EventHandler( this.hexDisplayToolStripButton_Click );
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size( 6, 25 );
			// 
			// registersToolStripButton
			// 
			this.registersToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.registersToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.RegistersIcon;
			this.registersToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.registersToolStripButton.Name = "registersToolStripButton";
			this.registersToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.registersToolStripButton.ToolTipText = "Registers";
			this.registersToolStripButton.Click += new System.EventHandler( this.registersToolStripButton_Click );
			// 
			// memoryToolStripButton
			// 
			this.memoryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.memoryToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.MemoryIcon;
			this.memoryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.memoryToolStripButton.Name = "memoryToolStripButton";
			this.memoryToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.memoryToolStripButton.Text = "memoryToolStripButton";
			this.memoryToolStripButton.ToolTipText = "Memory Viewer";
			this.memoryToolStripButton.Click += new System.EventHandler( this.memoryToolStripButton_Click );
			// 
			// callstackToolStripButton
			// 
			this.callstackToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.callstackToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.CallstackIcon;
			this.callstackToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.callstackToolStripButton.Name = "callstackToolStripButton";
			this.callstackToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.callstackToolStripButton.Text = "callstackToolStripButton";
			this.callstackToolStripButton.ToolTipText = "Callstack";
			this.callstackToolStripButton.Click += new System.EventHandler( this.callstackToolStripButton_Click );
			// 
			// threadsToolStripButton
			// 
			this.threadsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.threadsToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.ThreadsIcon;
			this.threadsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.threadsToolStripButton.Name = "threadsToolStripButton";
			this.threadsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.threadsToolStripButton.Text = "threadsToolStripButton";
			this.threadsToolStripButton.ToolTipText = "Threads";
			this.threadsToolStripButton.Click += new System.EventHandler( this.threadsToolStripButton_Click );
			// 
			// osToolStripButton
			// 
			this.osToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.osToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.OsIcon;
			this.osToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.osToolStripButton.Name = "osToolStripButton";
			this.osToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.osToolStripButton.Text = "osToolStripButton";
			this.osToolStripButton.ToolTipText = "BIOS Inspector";
			this.osToolStripButton.Click += new System.EventHandler( this.osToolStripButton_Click );
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size( 6, 25 );
			// 
			// breakpointsToolStripButton
			// 
			this.breakpointsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.breakpointsToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.BreakpointsIcon;
			this.breakpointsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.breakpointsToolStripButton.Name = "breakpointsToolStripButton";
			this.breakpointsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.breakpointsToolStripButton.Text = "breakpointsToolStripButton";
			this.breakpointsToolStripButton.ToolTipText = "Breakpoints";
			this.breakpointsToolStripButton.Click += new System.EventHandler( this.breakpointsToolStripButton_Click );
			// 
			// watchesToolStripButton
			// 
			this.watchesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.watchesToolStripButton.Image = global::Noxa.Emulation.Psp.Player.Development.Properties.Resources.WatchesIcon;
			this.watchesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.watchesToolStripButton.Name = "watchesToolStripButton";
			this.watchesToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.watchesToolStripButton.Text = "watchesToolStripButton";
			this.watchesToolStripButton.ToolTipText = "Watches";
			this.watchesToolStripButton.Click += new System.EventHandler( this.watchesToolStripButton_Click );
			// 
			// Studio
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 841, 658 );
			this.Controls.Add( this.toolStripContainer1 );
			this.Controls.Add( this.statusStrip );
			this.Controls.Add( this.menuStrip );
			this.MainMenuStrip = this.menuStrip;
			this.Name = "Studio";
			this.Text = "Studio";
			this.menuStrip.ResumeLayout( false );
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout( false );
			this.statusStrip.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout( false );
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout( false );
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout( false );
			this.toolStripContainer1.PerformLayout();
			this.controlToolStrip.ResumeLayout( false );
			this.controlToolStrip.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
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
		private System.Windows.Forms.ToolStrip controlToolStrip;
		private System.Windows.Forms.ToolStripButton continueToolStripButton;
		private System.Windows.Forms.ToolStripButton breakToolStripButton;
		private System.Windows.Forms.ToolStripButton stopToolStripButton;
		private System.Windows.Forms.ToolStripButton restartToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton showStatementToolStripButton;
		private System.Windows.Forms.ToolStripButton stepIntoToolStripButton;
		private System.Windows.Forms.ToolStripButton stepOverToolStripButton;
		private System.Windows.Forms.ToolStripButton stepOutToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton hexDisplayToolStripButton;
		private WeifenLuo.WinFormsUI.DockPanel dockPanel;
		private System.Windows.Forms.ToolStripStatusLabel messageStripStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel stateStripStatusLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripButton registersToolStripButton;
		private System.Windows.Forms.ToolStripButton memoryToolStripButton;
		private System.Windows.Forms.ToolStripButton callstackToolStripButton;
		private System.Windows.Forms.ToolStripButton threadsToolStripButton;
		private System.Windows.Forms.ToolStripButton breakpointsToolStripButton;
		private System.Windows.Forms.ToolStripButton watchesToolStripButton;
		private System.Windows.Forms.ToolStripButton osToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
	}
}