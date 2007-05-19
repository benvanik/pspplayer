// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class StatisticsViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( StatisticsViewer ) );
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.countersToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.clearToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.scrollPanel = new System.Windows.Forms.Panel();
			this.listView1 = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.valueColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.deltaColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.toolStrip1.SuspendLayout();
			this.scrollPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.countersToolStripDropDownButton,
            this.clearToolStripButton,
            this.toolStripSeparator1} );
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 261, 25 );
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// countersToolStripDropDownButton
			// 
			this.countersToolStripDropDownButton.Enabled = false;
			this.countersToolStripDropDownButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "countersToolStripDropDownButton.Image" ) ) );
			this.countersToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.countersToolStripDropDownButton.Name = "countersToolStripDropDownButton";
			this.countersToolStripDropDownButton.Size = new System.Drawing.Size( 84, 22 );
			this.countersToolStripDropDownButton.Text = "Counters";
			// 
			// clearToolStripButton
			// 
			this.clearToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.clearToolStripButton.Enabled = false;
			this.clearToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.ClearIcon;
			this.clearToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearToolStripButton.Name = "clearToolStripButton";
			this.clearToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this.clearToolStripButton.Text = "Clear";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
			// 
			// bottomPanel
			// 
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point( 0, 557 );
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size( 261, 31 );
			this.bottomPanel.TabIndex = 4;
			// 
			// scrollPanel
			// 
			this.scrollPanel.Controls.Add( this.listView1 );
			this.scrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollPanel.Location = new System.Drawing.Point( 0, 25 );
			this.scrollPanel.Name = "scrollPanel";
			this.scrollPanel.Size = new System.Drawing.Size( 261, 532 );
			this.scrollPanel.TabIndex = 5;
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.valueColumnHeader,
            this.deltaColumnHeader} );
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.InfoTip )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point( 0, 0 );
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size( 261, 532 );
			this.listView1.TabIndex = 6;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "Name";
			this.nameColumnHeader.Width = 105;
			// 
			// valueColumnHeader
			// 
			this.valueColumnHeader.Text = "Value";
			this.valueColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.valueColumnHeader.Width = 90;
			// 
			// deltaColumnHeader
			// 
			this.deltaColumnHeader.Text = "Delta";
			this.deltaColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.deltaColumnHeader.Width = 91;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// StatisticsViewer
			// 
			this.ClientSize = new System.Drawing.Size( 261, 588 );
			this.Controls.Add( this.scrollPanel );
			this.Controls.Add( this.bottomPanel );
			this.Controls.Add( this.toolStrip1 );
			this.MinimumSize = new System.Drawing.Size( 277, 0 );
			this.Name = "StatisticsViewer";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide;
			this.TabText = "Statistics";
			this.Text = "Statistics";
			this.toolStrip1.ResumeLayout( false );
			this.toolStrip1.PerformLayout();
			this.scrollPanel.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton countersToolStripDropDownButton;
		private System.Windows.Forms.ToolStripButton clearToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Panel scrollPanel;
		private System.Windows.Forms.Timer timer1;
		private Noxa.Utilities.Controls.DoubleBufferedListView listView1;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.ColumnHeader valueColumnHeader;
		private System.Windows.Forms.ColumnHeader deltaColumnHeader;
	}
}
