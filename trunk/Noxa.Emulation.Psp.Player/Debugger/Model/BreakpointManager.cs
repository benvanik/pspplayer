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
using Noxa.Emulation.Psp.Player.Debugger.UserData;

namespace Noxa.Emulation.Psp.Player.Debugger.Model
{
	class BreakpointManager
	{
		public readonly InprocDebugger Debugger;

		private List<Breakpoint> _breakpoints;
		private Dictionary<int, Breakpoint> _breakpointLookup;
		private Dictionary<uint, Breakpoint> _addressBreakpointLookup;
		private Dictionary<BiosFunctionToken, Breakpoint> _biosBreakpointLookup;
		private bool _suspendSave;

		public BreakpointManager( InprocDebugger debugger )
		{
			this.Debugger = debugger;

			_breakpoints = new List<Breakpoint>( 100 );
			_breakpointLookup = new Dictionary<int, Breakpoint>( 100 );
			_addressBreakpointLookup = new Dictionary<uint, Breakpoint>( 100 );
			_biosBreakpointLookup = new Dictionary<BiosFunctionToken, Breakpoint>( 100 );
		}

		public Breakpoint[] Breakpoints { get { return _breakpoints.ToArray(); } }

		public event EventHandler<BreakpointEventArgs> Added;
		public event EventHandler<BreakpointEventArgs> Removed;
		public event EventHandler<BreakpointEventArgs> Toggled;

		public void Load()
		{
			// Remove all and readd - slow?
			_suspendSave = true;
			foreach( Breakpoint bp in _breakpoints )
				this.Remove( bp );
			foreach( BreakpointInfo info in this.Debugger.UserData.Breakpoints.Infos )
			{
				Breakpoint bp = new Breakpoint( this.Debugger.AllocateID(), info.Type, info.Address );
				bp.Name = info.Name;
				bp.AccessType = info.AccessType;
				bp.Mode = info.Mode;
				bp.Enabled = info.Enabled;
				this.Add( bp );
			}
			_suspendSave = false;
		}

		public void Save()
		{
			if( _suspendSave == true )
				return;
			this.Debugger.UserData.Breakpoints.Infos.Clear();
			foreach( Breakpoint bp in _breakpoints )
			{
				BreakpointInfo info = new BreakpointInfo();
				info.Type = bp.Type;
				info.Address = bp.Address;
				info.Name = bp.Name;
				info.AccessType = bp.AccessType;
				info.Mode = bp.Mode;
				info.Enabled = bp.Enabled;
				this.Debugger.UserData.Breakpoints.Infos.Add( info );
			}
			this.Debugger.UserData.Save();
		}

		public void Update()
		{
			foreach( Breakpoint breakpoint in _breakpoints )
			{
				MethodBody body = this.Debugger.CodeCache[ breakpoint.Address ];
				if( body != null )
				{
					foreach( Instruction instruction in body.Instructions )
					{
						if( instruction.Address == breakpoint.Address )
						{
							Debug.Assert( instruction.Breakpoint == null );
							instruction.Breakpoint = breakpoint;
							break;
						}
					}
				}
			}
		}

		public void Add( Breakpoint breakpoint )
		{
			_breakpoints.Add( breakpoint );
			_breakpointLookup.Add( breakpoint.ID, breakpoint );
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
					_addressBreakpointLookup.Add( breakpoint.Address, breakpoint );
					{
						MethodBody body = this.Debugger.CodeCache[ breakpoint.Address ];
						if( body != null )
						{
							foreach( Instruction instruction in body.Instructions )
							{
								if( instruction.Address == breakpoint.Address )
								{
									Debug.Assert( instruction.Breakpoint == null );
									instruction.Breakpoint = breakpoint;
									break;
								}
							}
						}
					}
					break;
				case BreakpointType.MemoryAccess:
					_addressBreakpointLookup.Add( breakpoint.Address, breakpoint );
					break;
				case BreakpointType.BiosFunction:
					_biosBreakpointLookup.Add( breakpoint.Function, breakpoint );
					break;
			}
			this.OnBreakpointAdded( breakpoint );
			this.Save();
		}

		public void Remove( Breakpoint breakpoint )
		{
			_breakpoints.Remove( breakpoint );
			_breakpointLookup.Remove( breakpoint.ID );
			switch( breakpoint.Type )
			{
				case BreakpointType.CodeExecute:
					_addressBreakpointLookup.Remove( breakpoint.Address );
					{
						MethodBody body = this.Debugger.CodeCache[ breakpoint.Address ];
						if( body != null )
						{
							foreach( Instruction instruction in body.Instructions )
							{
								if( instruction.Address == breakpoint.Address )
								{
									Debug.Assert( instruction.Breakpoint == breakpoint );
									instruction.Breakpoint = null;
									break;
								}
							}
						}
					}
					break;
				case BreakpointType.MemoryAccess:
					_addressBreakpointLookup.Remove( breakpoint.Address );
					break;
				case BreakpointType.BiosFunction:
					_biosBreakpointLookup.Remove( breakpoint.Function );
					break;
			}
			this.OnBreakpointRemoved( breakpoint );
			this.Save();
		}

		public Breakpoint this[ int id ]
		{
			get
			{
				if( _breakpoints.Count < 50 )
				{
					foreach( Breakpoint bp in _breakpoints )
					{
						if( bp.ID == id )
							return bp;
					}
					return null;
				}
				else
				{
					Breakpoint breakpoint;
					if( _breakpointLookup.TryGetValue( id, out breakpoint ) == true )
						return breakpoint;
					else
						return null;
				}
			}
		}

		public Breakpoint Find( uint address )
		{
			if( _breakpoints.Count < 50 )
			{
				foreach( Breakpoint bp in _breakpoints )
				{
					if( bp.Address == address )
						return bp;
				}
				return null;
			}
			else
			{
				Breakpoint breakpoint;
				if( _addressBreakpointLookup.TryGetValue( address, out breakpoint ) == true )
					return breakpoint;
				else
					return null;
			}
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
			this.Debugger.DebugHost.CpuHook.AddBreakpoint( breakpoint );
			if( this.Added != null )
				this.Added( this, new BreakpointEventArgs( breakpoint ) );
		}

		private void OnBreakpointRemoved( Breakpoint breakpoint )
		{
			this.Debugger.DebugHost.CpuHook.RemoveBreakpoint( breakpoint.ID );
			if( this.Removed != null )
				this.Removed( this, new BreakpointEventArgs( breakpoint ) );
		}

		public void ToggleBreakpoint( int id )
		{
			Breakpoint breakpoint = this[ id ];
			Debug.Assert( breakpoint != null );
			if( breakpoint == null )
				return;
			this.ToggleBreakpoint( breakpoint );
		}
		
		public void ToggleBreakpoint( Breakpoint breakpoint )
		{
			bool old = breakpoint.Enabled;
			Breakpoint dummy = new Breakpoint( breakpoint.ID, breakpoint.Type, breakpoint.Address );
			dummy.Mode = breakpoint.Mode;
			dummy.Enabled = !old;
			this.OnBreakpointToggled( breakpoint, dummy );
			this.Save();
		}

		internal void OnBreakpointToggled( Breakpoint breakpoint, Breakpoint changed )
		{
			this.Debugger.DebugHost.CpuHook.UpdateBreakpoint( changed );
			if( this.Toggled != null )
				this.Toggled( this, new BreakpointEventArgs( breakpoint ) );
		}
	}
}
