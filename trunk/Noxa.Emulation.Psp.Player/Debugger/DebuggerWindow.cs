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
		public DockPanel DockPanel { get { return this.dockPanel; } }

		public DebuggerWindow()
		{
			InitializeComponent();
		}

		public DebuggerWindow( InprocDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
			this.Debugger.StateChanged += new EventHandler( Debugger_StateChanged );
		}

		private void Debugger_StateChanged( object sender, EventArgs e )
		{
			bool isRunning = ( this.Debugger.State == DebuggerState.Running );
			this.resumeToolStripButton.Enabled = !isRunning;
			this.resumeToolStripMenuItem.Enabled = !isRunning;
			this.breakToolStripButton.Enabled = isRunning;
			this.breakToolStripMenuItem.Enabled = isRunning;
			this.stopToolStripButton.Enabled = false;
			this.stopToolStripMenuItem.Enabled = false;
			this.restartToolStripButton.Enabled = false;
			this.restartToolStripMenuItem.Enabled = false;

			this.showNextStatementToolStripMenuItem.Enabled = !isRunning;
			this.showStatementToolStripButton.Enabled = !isRunning;
			this.stepIntoToolStripButton.Enabled = !isRunning;
			this.stepIntoToolStripMenuItem.Enabled = !isRunning;
			this.stepOverToolStripButton.Enabled = !isRunning;
			this.stepOverToolStripMenuItem.Enabled = !isRunning;
			this.stepOutToolStripButton.Enabled = !isRunning;
			this.stepOutToolStripMenuItem.Enabled = !isRunning;
			
			this.editMenu.Enabled = !isRunning;
			this.jumpMenu.Enabled = !isRunning;
			this.searchMenu.Enabled = !isRunning;
		}

		public void SetStatusText( string text )
		{
			this.toolStripStatusLabel.Text = text;
		}

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

		private void jumpToMethodToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.ShowJumpToMethodDialog();
		}

		private void nextLocationToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.CodeTool.NavigateForward();
		}

		private void previousLocationToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.CodeTool.NavigateBack();
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
