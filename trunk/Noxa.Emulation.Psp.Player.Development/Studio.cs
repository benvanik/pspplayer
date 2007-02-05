// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private const string MessageCaption = "PSP Player Debugger";

		private Debugger _debugger;

		private DisassemblyDocument _disasmDoc;
		private Statement _steppingStatement;

		private bool _useHex;

		public event EventHandler GlobalRefreshRequested;

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

			_debugger.StateChanged += new EventHandler( DebuggerStateChanged );

			_useHex = true;
		}

		public bool UseHex
		{
			get
			{
				return _useHex;
			}
		}

		public Debugger Debugger
		{
			get
			{
				return _debugger;
			}
		}

		public void UpdateState()
		{
		}

		#region Control

		private void continueToolStripButton_Click( object sender, EventArgs e )
		{
			CpuPane pane = new CpuPane( this );
			pane.Show( this.dockPanel );
			Method m = _debugger.DebugData.FindMethod( 0x08900000 );
			foreach( int addr in m.Instructions.Keys )
				_debugger.Control.AddBreakpoint( addr );
			this.CleanupBreakpoint();
			_debugger.Control.Run();
		}

		private void breakToolStripButton_Click( object sender, EventArgs e )
		{
			this.CleanupBreakpoint();
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
			if( _debugger.State == DebuggerState.Running )
				return;
			//this.ShowDisassembly( _debugger.h.Address );
		}

		private void stepIntoToolStripButton_Click( object sender, EventArgs e )
		{
			this.CleanupBreakpoint();
			_debugger.Control.StepInto();
		}

		private void stepOverToolStripButton_Click( object sender, EventArgs e )
		{
			this.CleanupBreakpoint();
			_debugger.Control.StepOver();
		}

		private void stepOutToolStripButton_Click( object sender, EventArgs e )
		{
			this.CleanupBreakpoint();
			_debugger.Control.StepOut();
		}

		private void hexDisplayToolStripButton_Click( object sender, EventArgs e )
		{
			_useHex = !_useHex;
			hexDisplayToolStripButton.Checked = _useHex;
			this.OnGlobalRefreshRequested();
		}

		private void OnGlobalRefreshRequested()
		{
			this.GlobalRefreshRequested( this, EventArgs.Empty );
		}

		private void CleanupBreakpoint()
		{
			if( ( _steppingStatement != null ) &&
				( _disasmDoc != null ) )
			{
				_disasmDoc.RemoveStatement( _steppingStatement );
			}
		}

		public void OnBreakpointTriggered( Breakpoint breakpoint )
		{
			switch( breakpoint.Type )
			{
				default:
				case BreakpointType.UserSet:
					this.UpdateStatusMessage( string.Format( "Stopped at breakpoint 0x{0:X8}", breakpoint.Address ) );
					break;
				case BreakpointType.Stepping:
					this.UpdateStatusMessage( string.Format( "Finished stepping at 0x{0:X8}", breakpoint.Address ) );
					break;
			}

			if( this.InvokeRequired == true )
				this.Invoke( new OnBreakpointTriggereDelegate( this.OnBreakpointTriggeredHandler ), breakpoint );
			else
				this.OnBreakpointTriggeredHandler( breakpoint );
		}

		private delegate void OnBreakpointTriggereDelegate( Breakpoint breakpoint );
		private void OnBreakpointTriggeredHandler( Breakpoint breakpoint )
		{
			_debugger.Inspector.Update( breakpoint.Address );
			if( this.ShowDisassembly( breakpoint.Address ) == false )
				return;
			_steppingStatement = _disasmDoc.AddStatement( StatementType.Current, breakpoint.Address );
		}

		#endregion

		#region Status bar

		private delegate void SetStatusLabelDelegate( ToolStripLabel label, string value );
		private void SetStatusLabel( ToolStripLabel label, string value )
		{
			label.Text = value;
		}

		private void DebuggerStateChanged( object sender, EventArgs e )
		{
			string value = _debugger.State.ToString();
			if( this.InvokeRequired == true )
				this.Invoke( new SetStatusLabelDelegate( this.SetStatusLabel ), this.stateStripStatusLabel, value );
			else
				this.SetStatusLabel( this.stateStripStatusLabel, value );
		}

		private void UpdateStatusMessage( string message )
		{
			if( this.InvokeRequired == true )
				this.Invoke( new SetStatusLabelDelegate( this.SetStatusLabel ), this.messageStripStatusLabel, message );
			else
				this.SetStatusLabel( this.messageStripStatusLabel, message );
		}

		#endregion

		#region Disassembly

		private void AlertMethodNotFound( int address )
		{
			MessageBox.Show( this,
				string.Format( "Unable to find a method corresponding to the address 0x{0:X8}. Unable to show disassemly.", address ),
				MessageCaption );
		}

		public bool ShowDisassembly( int address )
		{
			Method method = _debugger.DebugData.FindMethod( address );
			if( method == null )
			{
				this.AlertMethodNotFound( address );
				return false;
			}
			return this.ShowDisassembly( method );
		}

		public bool ShowDisassembly( Method method )
		{
			if( method == null )
				throw new ArgumentNullException( "method" );

			if( _disasmDoc != null )
			{
				_disasmDoc.DisplayMethod( method );
				_disasmDoc.Activate();
			}
			else
			{
				DisassemblyDocument doc = new DisassemblyDocument( this );
				doc.DisplayMethod( method );
				doc.Show( dockPanel );
				doc.Activate();
				_disasmDoc = doc;
			}

			return true;
		}

		#endregion
	}
}