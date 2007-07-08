// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class DebugView : Form
	{
		public EmuDebugger Debugger;

		public DockPanel DockPanel
		{
			get
			{
				return dockPanel1;
			}
		}

		public DebugView()
		{
			InitializeComponent();
		}

		public DebugView( EmuDebugger debugger )
			: this()
		{
			this.Icon = Properties.Resources.MainIcon;
			this.Debugger = debugger;
			
			// Toolstrips are broken - need to remove/readd to get on the same line
			this.toolStripContainer1.TopToolStripPanel.Controls.Clear();
			this.controlToolStrip.Location = Point.Empty;
			this.windowsToolStrip.Location = Point.Empty;
			this.locationToolStrip.Location = Point.Empty;
			// Add in reverse order
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.locationToolStrip );
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.windowsToolStrip );
			this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.controlToolStrip );

			this.Debugger.StateChanged += new EventHandler( Debugger_StateChanged );
		}

		private void Debugger_StateChanged( object sender, EventArgs e )
		{
			Debug.WriteLine( "debug state changed: " + this.Debugger.State.ToString() );

			this.SetDebugMenu();
		}

		private void DebugView_Load( object sender, EventArgs e )
		{
			this.Debugger.StartConnection();
		}

		#region Debug Menu

		private void SetDebugMenu()
		{
			switch( this.Debugger.State )
			{
				case DebuggerState.Idle:
					this.resumeToolStripMenuItem.Enabled = true;
					this.breakToolStripMenuItem.Enabled = false;
					this.stopToolStripMenuItem.Enabled = false;
					this.restartToolStripMenuItem.Enabled = false;
					this.showNextStatementToolStripMenuItem.Enabled = false;
					this.stepIntoToolStripMenuItem.Enabled = false;
					this.stepOutToolStripMenuItem.Enabled = false;
					this.stepOverToolStripMenuItem.Enabled = false;
					break;
				case DebuggerState.Running:
					this.resumeToolStripMenuItem.Enabled = false;
					this.breakToolStripMenuItem.Enabled = true;
					this.stopToolStripMenuItem.Enabled = true;
					this.restartToolStripMenuItem.Enabled = true;
					this.showNextStatementToolStripMenuItem.Enabled = false;
					this.stepIntoToolStripMenuItem.Enabled = false;
					this.stepOutToolStripMenuItem.Enabled = false;
					this.stepOverToolStripMenuItem.Enabled = false;
					break;
				case DebuggerState.Broken:
					this.resumeToolStripMenuItem.Enabled = true;
					this.breakToolStripMenuItem.Enabled = false;
					this.stopToolStripMenuItem.Enabled = true;
					this.restartToolStripMenuItem.Enabled = true;
					this.showNextStatementToolStripMenuItem.Enabled = true;
					this.stepIntoToolStripMenuItem.Enabled = true;
					this.stepOutToolStripMenuItem.Enabled = true;
					this.stepOverToolStripMenuItem.Enabled = true;
					break;
				case DebuggerState.Detached:
					this.resumeToolStripMenuItem.Enabled = false;
					this.breakToolStripMenuItem.Enabled = false;
					this.stopToolStripMenuItem.Enabled = false;
					this.restartToolStripMenuItem.Enabled = false;
					this.showNextStatementToolStripMenuItem.Enabled = false;
					this.stepIntoToolStripMenuItem.Enabled = false;
					this.stepOutToolStripMenuItem.Enabled = false;
					this.stepOverToolStripMenuItem.Enabled = false;
					break;
			}

			this.resumeToolStripButton.Enabled = this.resumeToolStripMenuItem.Enabled;
			this.breakToolStripButton.Enabled = this.breakToolStripMenuItem.Enabled;
			this.stopToolStripButton.Enabled = this.stopToolStripMenuItem.Enabled;
			this.restartToolStripButton.Enabled = this.restartToolStripMenuItem.Enabled;
			this.showStatementToolStripButton.Enabled = this.showNextStatementToolStripMenuItem.Enabled;
			this.stepIntoToolStripButton.Enabled = this.stepIntoToolStripMenuItem.Enabled;
			this.stepOutToolStripButton.Enabled = this.stepOutToolStripMenuItem.Enabled;
			this.stepOverToolStripButton.Enabled = this.stepOverToolStripMenuItem.Enabled;
		}

		private void resumeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Host.Controller.Run();
		}

		private void breakToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Host.Controller.Break();
		}

		private void stopToolStripMenuItem_Click( object sender, EventArgs e )
		{
			// TODO: STOP ??
		}

		private void restartToolStripMenuItem_Click( object sender, EventArgs e )
		{
			// TODO: RESTART ??
		}

		private void showNextStatementToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Code.ShowNextStatement();
		}

		private void stepIntoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Host.Controller.Step();
		}

		private void stepOverToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Host.Controller.StepOver();
		}

		private void stepOutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Debugger.Host.Controller.StepOut();
		}

		#endregion

		#region Window Menu

		private void codeViewToolStripButton_Click( object sender, EventArgs e )
		{
			this.Debugger.Code.Show( this.dockPanel1 );
		}

		private void memoryToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void callstackToolStripButton_Click( object sender, EventArgs e )
		{
			this.Debugger.CallStack.Show( this.dockPanel1 );
		}

		private void biosToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void threadsToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void breakpointsToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void watchesToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void statisticsToolStripButton_Click( object sender, EventArgs e )
		{
			this.Debugger.Statistics.Show( this.dockPanel1 );
		}

		private void logViewToolStripButton_Click( object sender, EventArgs e )
		{
			this.Debugger.Log.Show( this.dockPanel1 );
		}

		#endregion

		#region Statusbar

		private delegate void StatusUpdate( Verbosity verbosity, string text );

		internal void SetStatusText( Verbosity verbosity, string text )
		{
			StatusUpdate del = delegate
			{
				this.toolStripStatusLabel1.Text = text;
				switch( verbosity )
				{
					case Verbosity.Critical:
						this.toolStripStatusLabel1.ForeColor = Color.Red;
						break;
					default:
					case Verbosity.Normal:
						this.toolStripStatusLabel1.ForeColor = SystemColors.ControlText;
						break;
					case Verbosity.Verbose:
						this.toolStripStatusLabel1.ForeColor = Color.Green;
						break;
					case Verbosity.Everything:
						this.toolStripStatusLabel1.ForeColor = Color.Blue;
						break;
				}
			};
			if( this.InvokeRequired == true )
				this.Invoke( del, verbosity, text );
			else
				del( verbosity, text );
		}

		internal void SetStatusText( Verbosity verbosity, string format, params object[] args )
		{
			this.SetStatusText( verbosity, string.Format( format, args ) );
		}

		#endregion
	}
}