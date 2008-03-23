// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class DebuggerWindow : Form
	{
		public readonly InprocDebugger Debugger;

		public DebuggerWindow()
		{
			InitializeComponent();
		}

		public DebuggerWindow( InprocDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
		}

		public DockPanel DockPanel { get { return this.dockPanel; } }

		private void exitToolsStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		#region Jump/Navigation

		private void jumpToOperandToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void jumpToAddressToolStripMenuItem_Click( object sender, EventArgs e )
		{
			NavigationTarget target = NavigationTarget.Memory;
			if( this.dockPanel.ActiveContent == this.Debugger.MemoryTool )
				target = NavigationTarget.Memory;
			else if( this.dockPanel.ActiveContent == this.Debugger.CodeTool )
				target = NavigationTarget.Code;
			this.Debugger.ShowJumpToAddressDialog( target );
		}

		private void nextLocationToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void previousLocationToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void toggleBookmarkToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void nextBookmarkToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void previousBookmarkToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void clearAllBookmarksToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		#endregion

		#region Control

		private void resumeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.DebugHost.Controller.Run();
		}

		private void breakToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.DebugHost.Controller.Break();
		}

		private void stopToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void restartToolStripMenuItem_Click( object sender, EventArgs e )
		{

		}

		private void showNextStatementToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.JumpToAddress( NavigationTarget.Code, this.Debugger.PC, true );
		}

		private void stepIntoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.DebugHost.Controller.Step();
		}

		private void stepOverToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.DebugHost.Controller.StepOver();
		}

		private void stepOutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.DebugHost.Controller.StepOut();
		}

		#endregion
	}
}
