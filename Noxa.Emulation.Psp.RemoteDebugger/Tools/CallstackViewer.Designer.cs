// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class CallstackViewer
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
			this.callView = new System.Windows.Forms.ListView();
			this.iconHeader = new System.Windows.Forms.ColumnHeader();
			this.nameHeader = new System.Windows.Forms.ColumnHeader();
			this.locationHeader = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// callView
			// 
			this.callView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.iconHeader,
            this.nameHeader,
            this.locationHeader} );
			this.callView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.callView.FullRowSelect = true;
			this.callView.GridLines = true;
			this.callView.Location = new System.Drawing.Point( 0, 0 );
			this.callView.MultiSelect = false;
			this.callView.Name = "callView";
			this.callView.Size = new System.Drawing.Size( 670, 168 );
			this.callView.TabIndex = 0;
			this.callView.UseCompatibleStateImageBehavior = false;
			this.callView.View = System.Windows.Forms.View.Details;
			// 
			// iconHeader
			// 
			this.iconHeader.Text = "";
			this.iconHeader.Width = 25;
			// 
			// nameHeader
			// 
			this.nameHeader.Text = "Name";
			this.nameHeader.Width = 500;
			// 
			// locationHeader
			// 
			this.locationHeader.Text = "Location";
			this.locationHeader.Width = 105;
			// 
			// CallstackViewer
			// 
			this.ClientSize = new System.Drawing.Size( 670, 168 );
			this.Controls.Add( this.callView );
			this.Name = "CallstackViewer";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
			this.TabText = "Call Stack";
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ListView callView;
		private System.Windows.Forms.ColumnHeader iconHeader;
		private System.Windows.Forms.ColumnHeader nameHeader;
		private System.Windows.Forms.ColumnHeader locationHeader;
	}
}
