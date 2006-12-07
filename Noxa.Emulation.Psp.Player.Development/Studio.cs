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
using System.Diagnostics;

using Noxa.Emulation.Psp.Player.Development.Tools;
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player.Development
{
	partial class Studio : Form
	{
		private Debugger _debugger;

		private Dictionary<Method, DisassemblyDocument> _disasmDocs;

		public Studio()
		{
			InitializeComponent();
		}

		public Studio( Debugger debugger )
			: this()
		{
			Debug.Assert( debugger != null );
			if( debugger == null )
				throw new ArgumentNullException( "debugger" );
			_debugger = debugger;

			// Uses the nasty VB stuff
			DockPanel2005.VS2005Style.Extender.SetSchema( dockPanel, DockPanel2005.VS2005Style.Extender.Schema.FromBase );

			_disasmDocs = new Dictionary<Method, DisassemblyDocument>();
		}

		public void ShowMethodDisassembly( Method method )
		{
			if( _disasmDocs.ContainsKey( method ) == true )
			{
				DisassemblyDocument doc = _disasmDocs[ method ];
				doc.Activate();
			}
			else
			{
				DisassemblyDocument doc = new DisassemblyDocument( this );
				doc.DisplayMethod( method );
				_disasmDocs.Add( method, doc );
				doc.Show( dockPanel );
				doc.Activate();
			}
		}

		public void UpdateState()
		{
		}

		#region Control

		private void continueToolStripButton_Click( object sender, EventArgs e )
		{
			this.ShowMethodDisassembly( _debugger.DebugData.FindMethod( 0x08900000 ) );
			//_debugger.Control.Run();
		}

		private void breakToolStripButton_Click( object sender, EventArgs e )
		{
			_debugger.Control.Break();
		}

		private void stopToolStripButton_Click( object sender, EventArgs e )
		{
		}

		private void restartToolStripButton_Click( object sender, EventArgs e )
		{
		}

		private void showStatementToolStripButton_Click( object sender, EventArgs e )
		{
		}

		private void stepIntoToolStripButton_Click( object sender, EventArgs e )
		{
			_debugger.Control.StepInto();
		}

		private void stepOverToolStripButton_Click( object sender, EventArgs e )
		{
			_debugger.Control.StepOver();
		}

		private void stepOutToolStripButton_Click( object sender, EventArgs e )
		{
			_debugger.Control.StepOut();
		}

		private void hexDisplayToolStripButton_Click( object sender, EventArgs e )
		{

		}

		#endregion
	}
}