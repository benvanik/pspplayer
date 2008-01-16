namespace Noxa.Emulation.Psp.HookerPC
{
	partial class MainDisplay
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
			this.stdoutListView = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.callListView = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// stdoutListView
			// 
			this.stdoutListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2} );
			this.stdoutListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stdoutListView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect ) ) );
			this.stdoutListView.FullRowSelect = true;
			this.stdoutListView.HideSelection = false;
			this.stdoutListView.Location = new System.Drawing.Point( 0, 0 );
			this.stdoutListView.Name = "stdoutListView";
			this.stdoutListView.Size = new System.Drawing.Size( 1217, 335 );
			this.stdoutListView.TabIndex = 1;
			this.stdoutListView.UseCompatibleStateImageBehavior = false;
			this.stdoutListView.View = System.Windows.Forms.View.Details;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.callListView );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.stdoutListView );
			this.splitContainer1.Size = new System.Drawing.Size( 1217, 678 );
			this.splitContainer1.SplitterDistance = 339;
			this.splitContainer1.TabIndex = 2;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.clearToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 1217, 24 );
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// callListView
			// 
			this.callListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1} );
			this.callListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.callListView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect ) ) );
			this.callListView.FullRowSelect = true;
			this.callListView.HideSelection = false;
			this.callListView.Location = new System.Drawing.Point( 0, 0 );
			this.callListView.Name = "callListView";
			this.callListView.Size = new System.Drawing.Size( 1217, 339 );
			this.callListView.TabIndex = 0;
			this.callListView.UseCompatibleStateImageBehavior = false;
			this.callListView.View = System.Windows.Forms.View.Details;
			// 
			// connectToolStripMenuItem
			// 
			this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
			this.connectToolStripMenuItem.Size = new System.Drawing.Size( 64, 20 );
			this.connectToolStripMenuItem.Text = "Connect";
			this.connectToolStripMenuItem.Click += new System.EventHandler( this.connectToolStripMenuItem_Click );
			// 
			// disconnectToolStripMenuItem
			// 
			this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
			this.disconnectToolStripMenuItem.Size = new System.Drawing.Size( 78, 20 );
			this.disconnectToolStripMenuItem.Text = "Disconnect";
			this.disconnectToolStripMenuItem.Click += new System.EventHandler( this.disconnectToolStripMenuItem_Click );
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size( 46, 20 );
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler( this.clearToolStripMenuItem_Click );
			// 
			// MainDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 1217, 702 );
			this.Controls.Add( this.splitContainer1 );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainDisplay";
			this.Text = "Hooker Display";
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.ResumeLayout( false );
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Noxa.Utilities.Controls.DoubleBufferedListView stdoutListView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private Noxa.Utilities.Controls.DoubleBufferedListView callListView;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
	}
}

