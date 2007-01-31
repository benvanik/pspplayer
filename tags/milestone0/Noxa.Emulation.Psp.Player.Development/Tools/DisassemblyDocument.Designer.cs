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
			this.syntaxDocument = new Puzzle.SourceCode.SyntaxDocument( this.components );
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.methodsToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.codeEditorControl = new Puzzle.Windows.Forms.SyntaxBoxControl();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// syntaxDocument
			// 
			this.syntaxDocument.Lines = new string[] {
        ""};
			this.syntaxDocument.MaxUndoBufferSize = 1000;
			this.syntaxDocument.Modified = false;
			this.syntaxDocument.UndoStep = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.methodsToolStripComboBox,
            this.toolStripSeparator1} );
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 701, 25 );
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size( 52, 22 );
			this.toolStripLabel1.Text = "Method:";
			// 
			// methodsToolStripComboBox
			// 
			this.methodsToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.methodsToolStripComboBox.Name = "methodsToolStripComboBox";
			this.methodsToolStripComboBox.Size = new System.Drawing.Size( 271, 25 );
			this.methodsToolStripComboBox.Sorted = true;
			this.methodsToolStripComboBox.SelectedIndexChanged += new System.EventHandler( this.methodsToolStripComboBox_SelectedIndexChanged );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size( 61, 4 );
			// 
			// codeEditorControl
			// 
			this.codeEditorControl.ActiveView = Puzzle.Windows.Forms.ActiveView.BottomRight;
			this.codeEditorControl.AutoListPosition = null;
			this.codeEditorControl.AutoListSelectedText = "a123";
			this.codeEditorControl.AutoListVisible = false;
			this.codeEditorControl.BackColor = System.Drawing.Color.White;
			this.codeEditorControl.BorderStyle = Puzzle.Windows.Forms.BorderStyle.None;
			this.codeEditorControl.CopyAsRTF = false;
			this.codeEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeEditorControl.Document = this.syntaxDocument;
			this.codeEditorControl.FontName = "Courier new";
			this.codeEditorControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.codeEditorControl.InfoTipCount = 1;
			this.codeEditorControl.InfoTipPosition = null;
			this.codeEditorControl.InfoTipSelectedIndex = 1;
			this.codeEditorControl.InfoTipVisible = false;
			this.codeEditorControl.Location = new System.Drawing.Point( 0, 25 );
			this.codeEditorControl.LockCursorUpdate = false;
			this.codeEditorControl.Name = "codeEditorControl";
			this.codeEditorControl.ShowScopeIndicator = false;
			this.codeEditorControl.Size = new System.Drawing.Size( 701, 529 );
			this.codeEditorControl.SmoothScroll = false;
			this.codeEditorControl.SplitviewH = -4;
			this.codeEditorControl.SplitviewV = -4;
			this.codeEditorControl.TabGuideColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 233 ) ) ) ), ( ( int )( ( ( byte )( 233 ) ) ) ), ( ( int )( ( ( byte )( 233 ) ) ) ) );
			this.codeEditorControl.TabIndex = 3;
			this.codeEditorControl.Text = "codeEditorControl1";
			this.codeEditorControl.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// DisassemblyDocument
			// 
			this.ClientSize = new System.Drawing.Size( 701, 554 );
			this.CloseButton = false;
			this.Controls.Add( this.codeEditorControl );
			this.Controls.Add( this.toolStrip1 );
			this.Name = "DisassemblyDocument";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.Document;
			this.TabText = "Code";
			this.Text = "Code";
			this.toolStrip1.ResumeLayout( false );
			this.toolStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Puzzle.SourceCode.SyntaxDocument syntaxDocument;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private Puzzle.Windows.Forms.SyntaxBoxControl codeEditorControl;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripComboBox methodsToolStripComboBox;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}
