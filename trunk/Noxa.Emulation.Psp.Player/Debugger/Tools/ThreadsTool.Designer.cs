namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class ThreadsTool
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
			this.threadListView = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.idColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.pcColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.priorityColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.stateColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.threadContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.jumpToCurrentPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.wakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.killToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.delayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.threadContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// threadListView
			// 
			this.threadListView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.threadListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.idColumnHeader,
            this.pcColumnHeader,
            this.nameColumnHeader,
            this.priorityColumnHeader,
            this.stateColumnHeader} );
			this.threadListView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.GridLines | Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.threadListView.Font = new System.Drawing.Font( "Courier New", 8F );
			this.threadListView.FullRowSelect = true;
			this.threadListView.GridLines = true;
			this.threadListView.Location = new System.Drawing.Point( 4, 4 );
			this.threadListView.Name = "threadListView";
			this.threadListView.Size = new System.Drawing.Size( 692, 181 );
			this.threadListView.TabIndex = 0;
			this.threadListView.UseCompatibleStateImageBehavior = false;
			this.threadListView.View = System.Windows.Forms.View.Details;
			this.threadListView.MouseDown += new System.Windows.Forms.MouseEventHandler( this.threadListView_MouseDown );
			// 
			// idColumnHeader
			// 
			this.idColumnHeader.Text = "ID";
			this.idColumnHeader.Width = 55;
			// 
			// pcColumnHeader
			// 
			this.pcColumnHeader.Text = "PC";
			this.pcColumnHeader.Width = 99;
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "Name";
			this.nameColumnHeader.Width = 152;
			// 
			// priorityColumnHeader
			// 
			this.priorityColumnHeader.Text = "Pri";
			// 
			// stateColumnHeader
			// 
			this.stateColumnHeader.Text = "State";
			this.stateColumnHeader.Width = 297;
			// 
			// threadContextMenuStrip
			// 
			this.threadContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.jumpToCurrentPCToolStripMenuItem,
            this.toolStripMenuItem1,
            this.wakeToolStripMenuItem,
            this.delayToolStripMenuItem,
            this.killToolStripMenuItem} );
			this.threadContextMenuStrip.Name = "threadContextMenuStrip";
			this.threadContextMenuStrip.Size = new System.Drawing.Size( 179, 120 );
			// 
			// jumpToCurrentPCToolStripMenuItem
			// 
			this.jumpToCurrentPCToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StatementIcon;
			this.jumpToCurrentPCToolStripMenuItem.Name = "jumpToCurrentPCToolStripMenuItem";
			this.jumpToCurrentPCToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
			this.jumpToCurrentPCToolStripMenuItem.Text = "&Jump to Current PC";
			this.jumpToCurrentPCToolStripMenuItem.Click += new System.EventHandler( this.jumpToCurrentPCToolStripMenuItem_Click );
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size( 175, 6 );
			// 
			// wakeToolStripMenuItem
			// 
			this.wakeToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.PlayIcon;
			this.wakeToolStripMenuItem.Name = "wakeToolStripMenuItem";
			this.wakeToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
			this.wakeToolStripMenuItem.Text = "&Wake";
			this.wakeToolStripMenuItem.Click += new System.EventHandler( this.wakeToolStripMenuItem_Click );
			// 
			// killToolStripMenuItem
			// 
			this.killToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.StopIcon;
			this.killToolStripMenuItem.Name = "killToolStripMenuItem";
			this.killToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
			this.killToolStripMenuItem.Text = "&Kill";
			this.killToolStripMenuItem.Click += new System.EventHandler( this.killToolStripMenuItem_Click );
			// 
			// delayToolStripMenuItem
			// 
			this.delayToolStripMenuItem.Image = global::Noxa.Emulation.Psp.Player.Properties.Resources.PauseIcon;
			this.delayToolStripMenuItem.Name = "delayToolStripMenuItem";
			this.delayToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
			this.delayToolStripMenuItem.Text = "&Delay";
			this.delayToolStripMenuItem.Click += new System.EventHandler( this.delayToolStripMenuItem_Click );
			// 
			// ThreadsTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 700, 189 );
			this.CloseButton = false;
			this.Controls.Add( this.threadListView );
			this.DockAreas = ( ( WeifenLuo.WinFormsUI.Docking.DockAreas )( ( ( ( ( WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom ) ) );
			this.HideOnClose = true;
			this.Name = "ThreadsTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide;
			this.TabText = "Threads";
			this.threadContextMenuStrip.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private Noxa.Utilities.Controls.DoubleBufferedListView threadListView;
		private System.Windows.Forms.ColumnHeader idColumnHeader;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.ColumnHeader pcColumnHeader;
		private System.Windows.Forms.ColumnHeader priorityColumnHeader;
		private System.Windows.Forms.ColumnHeader stateColumnHeader;
		private System.Windows.Forms.ContextMenuStrip threadContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem jumpToCurrentPCToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem wakeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem killToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem delayToolStripMenuItem;
	}
}
