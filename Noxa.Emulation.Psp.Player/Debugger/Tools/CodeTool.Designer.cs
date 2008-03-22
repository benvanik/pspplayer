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
			this.disassemblyControl = new Noxa.Emulation.Psp.Player.Debugger.Tools.DisassemblyControl();
			this.codeView = new Noxa.Emulation.Psp.Player.Debugger.Tools.CodeViewControl();
			this.SuspendLayout();
			// 
			// disassemblyControl
			// 
			this.disassemblyControl.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.disassemblyControl.DisplayHex = true;
			this.disassemblyControl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.disassemblyControl.FormattingEnabled = true;
			this.disassemblyControl.Location = new System.Drawing.Point( 12, 12 );
			this.disassemblyControl.Name = "disassemblyControl";
			this.disassemblyControl.ScrollAlwaysVisible = true;
			this.disassemblyControl.Size = new System.Drawing.Size( 672, 186 );
			this.disassemblyControl.TabIndex = 0;
			// 
			// codeViewControl1
			// 
			this.codeView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.codeView.Location = new System.Drawing.Point( 12, 204 );
			this.codeView.Name = "codeViewControl1";
			this.codeView.Size = new System.Drawing.Size( 672, 295 );
			this.codeView.TabIndex = 1;
			this.codeView.Text = "codeView";
			// 
			// CodeTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 696, 511 );
			this.CloseButton = false;
			this.Controls.Add( this.codeView );
			this.Controls.Add( this.disassemblyControl );
			this.DockAreas = ( ( WeifenLuo.WinFormsUI.Docking.DockAreas )( ( WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document ) ) );
			this.Name = "CodeTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.TabText = "Disassembly";
			this.ResumeLayout( false );

		}

		#endregion

		private DisassemblyControl disassemblyControl;
		private CodeViewControl codeView;
	}
}
