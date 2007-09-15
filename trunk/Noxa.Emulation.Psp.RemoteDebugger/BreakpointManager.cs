// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.RemoteDebugger.Model;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	class BreakpointManager
	{
		public EmuDebugger Debugger;

		private List<Breakpoint> _breakpoints;
		private Dictionary<int, Breakpoint> _breakpointLookup;
		private Dictionary<uint, Breakpoint> _addressBreakpointLookup;
		private Dictionary<BiosFunctionToken, Breakpoint> _biosBreakpointLookup;

		public BreakpointManager( EmuDebugger debugger )
		{
			this.Debugger = debugger;

			_breakpoints = new List<Breakpoint>( 100 );
			_breakpointLookup = new Dictionary<int, Breakpoint>( 100 );
			_addressBreakpointLookup = new Dictionary<uint, Breakpoint>( 100 );
			_biosBreakpointLookup = new Dictionary<BiosFunctionToken, Breakpoint>( 100 );
		}

		public Breakpoint[] Breakpoints
		{
			get
			{
				return _breakpoints.ToArray();
			}
		}

		public event EventHandler<BreakpointEventArgs> Added;
		public event EventHandler<BreakpointEventArgs> Removed;
		public event EventHandler<BreakpointEventArgs> Toggled;

		public void Add( Breakpoint breakpoint )
		{
			_breakpointLookup.Add( breakpoint.ID, breakpoint );
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
				case BreakpointType.MemoryAccess:
					_addressBreakpointLookup.Add( breakpoint.Address, breakpoint );
					break;
				case BreakpointType.BiosFunction:
					_biosBreakpointLookup.Add( breakpoint.Function, breakpoint );
					break;
			}
			this.OnBreakpointAdded( breakpoint );
		}

		public void Remove( Breakpoint breakpoint )
		{
			_breakpointLookup.Remove( breakpoint.ID );
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
				case BreakpointType.MemoryAccess:
					_addressBreakpointLookup.Remove( breakpoint.Address );
					break;
				case BreakpointType.BiosFunction:
					_biosBreakpointLookup.Remove( breakpoint.Function );
					break;
			}
			this.OnBreakpointRemoved( breakpoint );
		}

		public Breakpoint this[ int id ]
		{
			get
			{
				Breakpoint breakpoint;
				if( _breakpointLookup.TryGetValue( id, out breakpoint ) == true )
					return breakpoint;
				else
					return null;
			}
		}

		public Breakpoint Find( uint address )
		{
			Breakpoint breakpoint;
			if( _addressBreakpointLookup.TryGetValue( address, out breakpoint ) == true )
				return breakpoint;
			else
				return null;
		}

		public Breakpoint Find( BiosFunctionToken function )
		{
			Breakpoint breakpoint;
			if( _biosBreakpointLookup.TryGetValue( function, out breakpoint ) == true )
				return breakpoint;
			else
				return null;
		}

		private void OnBreakpointAdded( Breakpoint breakpoint )
		{
			this.Debugger.Host.CpuHook.AddBreakpoint( breakpoint );
			if( this.Added != null )
				this.Added( this, new BreakpointEventArgs( breakpoint ) );
		}

		private void OnBreakpointRemoved( Breakpoint breakpoint )
		{
			this.Debugger.Host.CpuHook.RemoveBreakpoint( breakpoint.ID );
			if( this.Removed != null )
				this.Removed( this, new BreakpointEventArgs( breakpoint ) );
		}

		public void ToggleBreakpoint( int id )
		{
			Breakpoint breakpoint = this[ id ];
			Debug.Assert( breakpoint != null );
			bool old = breakpoint.Enabled;
			breakpoint.Enabled = !old;
			this.OnBreakpointToggled( breakpoint );
		}

		internal void OnBreakpointToggled( Breakpoint breakpoint )
		{
			this.Debugger.Host.CpuHook.UpdateBreakpoint( breakpoint );
			if( this.Toggled != null )
				this.Toggled( this, new BreakpointEventArgs( breakpoint ) );
		}
	}
}
