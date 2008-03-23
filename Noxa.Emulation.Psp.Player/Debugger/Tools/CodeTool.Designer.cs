namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CodeTool
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.codeView = new Noxa.Emulation.Psp.Player.Debugger.Tools.CodeViewControl();
			this.registersControl = new Noxa.Emulation.Psp.Player.Debugger.Tools.RegistersControl();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.IsSplitterFixed = true;
			this.splitContainer.Location = new System.Drawing.Point( 4, 4 );
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add( this.codeView );
			this.splitContainer.Panel1MinSize = 300;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add( this.registersControl );
			this.splitContainer.Size = new System.Drawing.Size( 688, 503 );
			this.splitContainer.SplitterDistance = 600;
			this.splitContainer.TabIndex = 2;
			// 
			// codeView
			// 
			this.codeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeView.Location = new System.Drawing.Point( 0, 0 );
			this.codeView.Name = "codeView";
			this.codeView.Size = new System.Drawing.Size( 600, 503 );
			this.codeView.TabIndex = 2;
			this.codeView.Text = "codeView";
			this.codeView.UseHex = false;
			// 
			// registersControl
			// 
			this.registersControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.registersControl.Location = new System.Drawing.Point( 0, 0 );
			this.registersControl.Name = "registersControl";
			this.registersControl.Size = new System.Drawing.Size( 84, 503 );
			this.registersControl.TabIndex = 3;
			this.registersControl.Text = "registersControl1";
			// 
			// CodeTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 696, 511 );
			this.CloseButton = false;
			this.Controls.Add( this.splitContainer );
			this.DockAreas = ( ( WeifenLuo.WinFormsUI.Docking.DockAreas )( ( WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document ) ) );
			this.Name = "CodeTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.TabText = "Disassembly";
			this.splitContainer.Panel1.ResumeLayout( false );
			this.splitContainer.Panel2.ResumeLayout( false );
			this.splitContainer.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private CodeViewControl codeView;
		private RegistersControl registersControl;

	}
}
