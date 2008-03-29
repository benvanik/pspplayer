using Noxa.Utilities.Controls;
namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CallstackTool
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
			this.callView = new Noxa.Utilities.Controls.DoubleBufferedListView();
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
			this.callView.ExtendedStyle = ( ( Noxa.Utilities.Controls.ListViewExtendedStyle )( ( ( ( Noxa.Utilities.Controls.ListViewExtendedStyle.GridLines | Noxa.Utilities.Controls.ListViewExtendedStyle.FullRowSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.BorderSelect )
						| Noxa.Utilities.Controls.ListViewExtendedStyle.DoubleBuffer ) ) );
			this.callView.Font = new System.Drawing.Font( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.callView.FullRowSelect = true;
			this.callView.GridLines = true;
			this.callView.Location = new System.Drawing.Point( 0, 0 );
			this.callView.MultiSelect = false;
			this.callView.Name = "callView";
			this.callView.Size = new System.Drawing.Size( 324, 162 );
			this.callView.TabIndex = 0;
			this.callView.UseCompatibleStateImageBehavior = false;
			this.callView.View = System.Windows.Forms.View.Details;
			this.callView.MouseDown += new System.Windows.Forms.MouseEventHandler( this.callView_MouseDown );
			// 
			// iconHeader
			// 
			this.iconHeader.Text = "";
			this.iconHeader.Width = 0;
			// 
			// nameHeader
			// 
			this.nameHeader.Text = "Name";
			this.nameHeader.Width = 165;
			// 
			// locationHeader
			// 
			this.locationHeader.Text = "Location";
			this.locationHeader.Width = 105;
			// 
			// CallstackTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 324, 162 );
			this.CloseButton = false;
			this.Controls.Add( this.callView );
			this.DockAreas = ( ( WeifenLuo.WinFormsUI.Docking.DockAreas )( ( ( ( ( WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop )
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom ) ) );
			this.Name = "CallstackTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Callstack";
			this.ResumeLayout( false );

		}

		#endregion

		private DoubleBufferedListView callView;
		private System.Windows.Forms.ColumnHeader iconHeader;
		private System.Windows.Forms.ColumnHeader nameHeader;
		private System.Windows.Forms.ColumnHeader locationHeader;
	}
}
