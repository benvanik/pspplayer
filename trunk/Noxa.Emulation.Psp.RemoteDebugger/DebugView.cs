// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
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
		}

		private void DebugView_Load( object sender, EventArgs e )
		{
			this.Debugger.StartConnection();
		}

		#region Window controls

		private void codeViewToolStripButton_Click( object sender, EventArgs e )
		{
			this.Debugger.Code.Show( this.dockPanel1 );
		}

		private void memoryToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void registersToolStripButton_Click( object sender, EventArgs e )
		{

		}

		private void callstackToolStripButton_Click( object sender, EventArgs e )
		{

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
	}
}