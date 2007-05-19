// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class LogViewer
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.verbosityToolStripSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.criticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verboseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.everythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.saveAsTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsHtmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.debugWriteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.listView = new Noxa.Utilities.Controls.DoubleBufferedListView();
			this.columnFeature = new System.Windows.Forms.ColumnHeader();
			this.columnValue = new System.Windows.Forms.ColumnHeader();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.verbosityToolStripSplitButton,
            this.toolStripSeparator2,
            this.copyToolStripButton,
            this.saveToolStripButton,
            this.clearToolStripButton,
            this.toolStripSeparator3,
            this.debugWriteToolStripButton} );
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 977, 25 );
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size( 101, 22 );
			this.toolStripLabel2.Text = "Selected Features:";
			this.toolStripLabel2.Visible = false;
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size( 164, 22 );
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Visible = false;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
			this.toolStripSeparator1.Visible = false;
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size( 88, 22 );
			this.toolStripLabel1.Text = "Verbosity Filter:";
			// 
			// verbosityToolStripSplitButton
			// 
			this.verbosityToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.verbosityToolStripSplitButton.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.criticalToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.verboseToolStripMenuItem,
            this.everythingToolStripMenuItem} );
			this.verbosityToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.verbosityToolStripSplitButton.Name = "verbosityToolStripSplitButton";
			this.verbosityToolStripSplitButton.Size = new System.Drawing.Size( 57, 22 );
			this.verbosityToolStripSplitButton.Text = "Critical";
			// 
			// criticalToolStripMenuItem
			// 
			this.criticalToolStripMenuItem.Name = "criticalToolStripMenuItem";
			this.criticalToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.criticalToolStripMenuItem.Text = "&Critical";
			this.criticalToolStripMenuItem.Click += new System.EventHandler( this.criticalToolStripMenuItem_Click );
			// 
			// normalToolStripMenuItem
			// 
			this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
			this.normalToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.normalToolStripMenuItem.Text = "&Normal";
			this.normalToolStripMenuItem.Click += new System.EventHandler( this.normalToolStripMenuItem_Click );
			// 
			// verboseToolStripMenuItem
			// 
			this.verboseToolStripMenuItem.Name = "verboseToolStripMenuItem";
			this.verboseToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.verboseToolStripMenuItem.Text = "&Verbose";
			this.verboseToolStripMenuItem.Click += new System.EventHandler( this.verboseToolStripMenuItem_Click );
			// 
			// everythingToolStripMenuItem
			// 
			this.everythingToolStripMenuItem.Name = "everythingToolStripMenuItem";
			this.everythingToolStripMenuItem.Size = new System.Drawing.Size( 130, 22 );
			this.everythingToolStripMenuItem.Text = "&Everything";
			this.everythingToolStripMenuItem.Click += new System.EventHandler( this.everythingToolStripMenuItem_Click );
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 6, 25 );
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.CopyIcon;
			this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Size = new System.Drawing.Size( 55, 22 );
			this.copyToolStripButton.Text = "Copy";
			this.copyToolStripButton.Click += new System.EventHandler( this.copyToolStripButton_Click );
			// 
			// saveToolStripButton
			// 
			this.saveToolStripButton.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.saveAsTextToolStripMenuItem,
            this.saveAsHtmlToolStripMenuItem,
            this.saveAsXmlToolStripMenuItem} );
			this.saveToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.SaveIcon;
			this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripButton.Name = "saveToolStripButton";
			this.saveToolStripButton.Size = new System.Drawing.Size( 60, 22 );
			this.saveToolStripButton.Text = "Save";
			// 
			// saveAsTextToolStripMenuItem
			// 
			this.saveAsTextToolStripMenuItem.Name = "saveAsTextToolStripMenuItem";
			this.saveAsTextToolStripMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this.saveAsTextToolStripMenuItem.Text = "Save as &Text";
			this.saveAsTextToolStripMenuItem.Click += new System.EventHandler( this.saveAsTextToolStripMenuItem_Click );
			// 
			// saveAsHtmlToolStripMenuItem
			// 
			this.saveAsHtmlToolStripMenuItem.Enabled = false;
			this.saveAsHtmlToolStripMenuItem.Name = "saveAsHtmlToolStripMenuItem";
			this.saveAsHtmlToolStripMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this.saveAsHtmlToolStripMenuItem.Text = "Save as &HTML";
			this.saveAsHtmlToolStripMenuItem.Click += new System.EventHandler( this.saveAsHtmlToolStripMenuItem_Click );
			// 
			// saveAsXmlToolStripMenuItem
			// 
			this.saveAsXmlToolStripMenuItem.Enabled = false;
			this.saveAsXmlToolStripMenuItem.Name = "saveAsXmlToolStripMenuItem";
			this.saveAsXmlToolStripMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this.saveAsXmlToolStripMenuItem.Text = "Save as &XML";
			this.saveAsXmlToolStripMenuItem.Click += new System.EventHandler( this.saveAsXmlToolStripMenuItem_Click );
			// 
			// clearToolStripButton
			// 
			this.clearToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.ClearIcon;
			this.clearToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearToolStripButton.Name = "clearToolStripButton";
			this.clearToolStripButton.Size = new System.Drawing.Size( 54, 22 );
			this.clearToolStripButton.Text = "Clear";
			this.clearToolStripButton.Click += new System.EventHandler( this.clearToolStripButton_Click );
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size( 6, 25 );
			// 
			// debugWriteToolStripButton
			// 
			this.debugWriteToolStripButton.Image = global::Noxa.Emulation.Psp.RemoteDebugger.Properties.Resources.OutputIcon;
			this.debugWriteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.debugWriteToolStripButton.Name = "debugWriteToolStripButton";
			this.debugWriteToolStripButton.Size = new System.Drawing.Size( 115, 22 );
			this.debugWriteToolStripButton.Text = "Debug.WriteLine";
			this.debugWriteToolStripButton.Click += new System.EventHandler( this.debugWriteToolStripButton_Click );
			// 
			// listView
			// 
			this.listView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnFeature,
            this.columnValue} );
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect | Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.listView.Font = new System.Drawing.Font( "Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.listView.FullRowSelect = true;
			this.listView.Location = new System.Drawing.Point( 0, 25 );
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size( 977, 400 );
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.Resize += new System.EventHandler( this.listView_Resize );
			// 
			// columnFeature
			// 
			this.columnFeature.Text = "Feature";
			this.columnFeature.Width = 78;
			// 
			// columnValue
			// 
			this.columnValue.Text = "Message";
			this.columnValue.Width = 707;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Title = "Save Log";
			// 
			// LogViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 977, 425 );
			this.Controls.Add( this.listView );
			this.Controls.Add( this.toolStrip1 );
			this.Name = "LogViewer";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Log Viewer";
			this.Text = "Log View";
			this.toolStrip1.ResumeLayout( false );
			this.toolStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton clearToolStripButton;
		private Noxa.Utilities.Controls.DoubleBufferedListView listView;
		private System.Windows.Forms.ColumnHeader columnFeature;
		private System.Windows.Forms.ColumnHeader columnValue;
		private System.Windows.Forms.ToolStripDropDownButton verbosityToolStripSplitButton;
		private System.Windows.Forms.ToolStripMenuItem criticalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verboseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem everythingToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolStripDropDownButton saveToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem saveAsTextToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsHtmlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsXmlToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton debugWriteToolStripButton;
	}
}