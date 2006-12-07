// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Player.Development.Tools
{
	partial class DisassemblyDocument
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
			this.codeEditorControl = new Puzzle.Windows.Forms.SyntaxBoxControl();
			this.syntaxDocument = new Puzzle.SourceCode.SyntaxDocument( this.components );
			this.SuspendLayout();
			// 
			// codeEditorControl
			// 
			this.codeEditorControl.ActiveView = Puzzle.Windows.Forms.ActiveView.BottomRight;
			this.codeEditorControl.AutoListPosition = null;
			this.codeEditorControl.AutoListSelectedText = "";
			this.codeEditorControl.AutoListVisible = false;
			this.codeEditorControl.CopyAsRTF = false;
			this.codeEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeEditorControl.Document = this.syntaxDocument;
			this.codeEditorControl.FontName = "Courier New";
			this.codeEditorControl.InfoTipCount = 1;
			this.codeEditorControl.InfoTipPosition = null;
			this.codeEditorControl.InfoTipSelectedIndex = 0;
			this.codeEditorControl.InfoTipVisible = false;
			this.codeEditorControl.Location = new System.Drawing.Point( 0, 0 );
			this.codeEditorControl.LockCursorUpdate = false;
			this.codeEditorControl.Name = "codeEditorControl";
			this.codeEditorControl.ShowScopeIndicator = false;
			this.codeEditorControl.Size = new System.Drawing.Size( 701, 554 );
			this.codeEditorControl.SmoothScroll = false;
			this.codeEditorControl.SplitviewH = -4;
			this.codeEditorControl.SplitviewV = -4;
			this.codeEditorControl.TabGuideColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 233 ) ) ) ), ( ( int )( ( ( byte )( 233 ) ) ) ), ( ( int )( ( ( byte )( 233 ) ) ) ) );
			this.codeEditorControl.TabIndex = 0;
			this.codeEditorControl.Text = "codeEditorControl1";
			this.codeEditorControl.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// syntaxDocument
			// 
			this.syntaxDocument.Lines = new string[] {
        ""};
			this.syntaxDocument.MaxUndoBufferSize = 1000;
			this.syntaxDocument.Modified = false;
			this.syntaxDocument.UndoStep = 0;
			// 
			// CodeDocument
			// 
			this.ClientSize = new System.Drawing.Size( 701, 554 );
			this.CloseButton = false;
			this.Controls.Add( this.codeEditorControl );
			this.Name = "DisassemblyDocument";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.Document;
			this.TabText = "Code";
			this.Text = "Code";
			this.ResumeLayout( false );

		}

		#endregion

		private Puzzle.Windows.Forms.SyntaxBoxControl codeEditorControl;
		private Puzzle.SourceCode.SyntaxDocument syntaxDocument;
	}
}
