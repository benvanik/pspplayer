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

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	class BreakpointManager
	{
		public EmuDebugger Debugger;

		private List<Breakpoint> _breakpoints;
		private Dictionary<int, Breakpoint> _breakpointLookup;
		private Dictionary<uint, Breakpoint> _addressBreakpointLookup;
		private Dictionary<BiosFunction, Breakpoint> _biosBreakpointLookup;

		public BreakpointManager( EmuDebugger debugger )
		{
			this.Debugger = debugger;

			_breakpoints = new List<Breakpoint>( 100 );
			_breakpointLookup = new Dictionary<int, Breakpoint>( 100 );
			_addressBreakpointLookup = new Dictionary<uint, Breakpoint>( 100 );
			_biosBreakpointLookup = new Dictionary<BiosFunction, Breakpoint>( 100 );
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

		public Breakpoint Add( BreakpointType type, uint address )
		{
			Breakpoint breakpoint = new Breakpoint( this, type, address );
			_breakpointLookup.Add( breakpoint.ID, breakpoint );
			_addressBreakpointLookup.Add( address, breakpoint );
			this.OnBreakpointAdded( breakpoint );
			return breakpoint;
		}

		public Breakpoint Add( BiosFunction function )
		{
			Breakpoint breakpoint = new Breakpoint( this, function );
			_breakpointLookup.Add( breakpoint.ID, breakpoint );
			_biosBreakpointLookup.Add( function, breakpoint );
			this.OnBreakpointAdded( breakpoint );
			return breakpoint;
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

		public Breakpoint Find( BiosFunction function )
		{
			Breakpoint breakpoint;
			if( _biosBreakpointLookup.TryGetValue( function, out breakpoint ) == true )
				return breakpoint;
			else
				return null;
		}

		private void OnBreakpointAdded( Breakpoint breakpoint )
		{
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
					this.Debugger.Host.CpuHook.AddCodeBreakpoint( breakpoint.ID, breakpoint.Address );
					break;
				case BreakpointType.MemoryAccess:
					this.Debugger.Host.CpuHook.AddMemoryBreakpoint( breakpoint.ID, breakpoint.Address, breakpoint.AccessType );
					break;
				case BreakpointType.BiosFunction:
					this.Debugger.Host.CpuHook.AddCodeBreakpoint( breakpoint.ID, breakpoint.Function.StubAddress );
					break;
			}
			if( this.Added != null )
				this.Added( this, new BreakpointEventArgs( breakpoint ) );
		}

		private void OnBreakpointRemoved( Breakpoint breakpoint )
		{
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
				case BreakpointType.BiosFunction:
					this.Debugger.Host.CpuHook.RemoveCodeBreakpoint( breakpoint.ID );
					break;
				case BreakpointType.MemoryAccess:
					this.Debugger.Host.CpuHook.RemoveMemoryBreakpoint( breakpoint.ID );
					break;
			}
			if( this.Removed != null )
				this.Removed( this, new BreakpointEventArgs( breakpoint ) );
		}

		internal void OnBreakpointToggled( Breakpoint breakpoint )
		{
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
				case BreakpointType.BiosFunction:
					this.Debugger.Host.CpuHook.SetCodeBreakpointState( breakpoint.ID, breakpoint.Enabled );
					break;
				case BreakpointType.MemoryAccess:
					this.Debugger.Host.CpuHook.SetMemoryBreakpointState( breakpoint.ID, breakpoint.Enabled );
					break;
			}
			if( this.Toggled != null )
				this.Toggled( this, new BreakpointEventArgs( breakpoint ) );
		}
	}
}
