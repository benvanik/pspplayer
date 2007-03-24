using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;

using Noxa.Emulation.Psp.Player.Development.Model;

namespace Noxa.Emulation.Psp.Player.Development
{
	class DebugControl : IDebugControl
	{
		private Debugger _debugger;
		private ICpu _cpu;

		private List<Breakpoint> _breakpoints;
		private Dictionary<int, Breakpoint> _breakpointLookup;

		public event EventHandler<BreakpointEventArgs> BreakpointAdded;
		public event EventHandler<BreakpointEventArgs> BreakpointRemoved;
		public event EventHandler<BreakpointEventArgs> BreakpointToggled;

		public DebugControl( Debugger debugger )
		{
			Debug.Assert( debugger != null );
			if( debugger == null )
				throw new ArgumentNullException( "debugger" );
			_debugger = debugger;

			_breakpoints = new List<Breakpoint>();
			_breakpointLookup = new Dictionary<int, Breakpoint>();

			_cpu = _debugger.Host.CurrentInstance.Cpu;
			Debug.Assert( _cpu != null );
		}

		#region Breakpoints

		public Breakpoint[] Breakpoints
		{
			get
			{
				return _breakpoints.ToArray();
			}
		}

		public Breakpoint AddBreakpoint( int address )
		{
			if( _breakpointLookup.ContainsKey( address ) == true )
			{
				Breakpoint existing = _breakpointLookup[ address ];
				existing.Enabled = true;
				return existing;
			}

			BasicBreakpoint breakpoint = new BasicBreakpoint( this, BreakpointType.UserSet, address );
			_breakpoints.Add( breakpoint );
			_breakpointLookup.Add( address, breakpoint );

			this.OnBreakpointAdded( breakpoint );

			return breakpoint;
		}

		public Breakpoint AddSteppingBreakpoint( int address )
		{
			// TODO: redo things so that adding a stepping bp does not change real bp's enabled state

			if( _breakpointLookup.ContainsKey( address ) == true )
			{
				Breakpoint existing = _breakpointLookup[ address ];
				existing.Enabled = true;
				return null;
			}

			BasicBreakpoint breakpoint = new BasicBreakpoint( this, BreakpointType.Stepping, address );
			_breakpoints.Add( breakpoint );
			_breakpointLookup.Add( address, breakpoint );

			this.OnBreakpointAdded( breakpoint );

			return breakpoint;
		}

		public Breakpoint RemoveBreakpoint( int address )
		{
			if( _breakpointLookup.ContainsKey( address ) == false )
				return null;

			Breakpoint breakpoint = _breakpointLookup[ address ];
			_breakpoints.Remove( breakpoint );
			_breakpointLookup.Remove( address );

			this.OnBreakpointRemoved( breakpoint );

			return breakpoint;
		}

		public Breakpoint FindBreakpoint( int address )
		{
			if( _breakpointLookup.ContainsKey( address ) == true )
				return _breakpointLookup[ address ];
			else
				return null;
		}

		private void OnBreakpointAdded( BasicBreakpoint breakpoint )
		{
			this.BreakpointAdded( this, new BreakpointEventArgs( breakpoint ) );
		}

		private void OnBreakpointRemoved( Breakpoint breakpoint )
		{
			this.BreakpointRemoved( this, new BreakpointEventArgs( breakpoint ) );
		}

		internal void OnBreakpointToggled( Breakpoint breakpoint )
		{
			this.BreakpointToggled( this, new BreakpointEventArgs( breakpoint ) );
		}

		#endregion

		public void Run()
		{
			_debugger.State = DebuggerState.Running;
			_cpu.Resume();
		}

		public void RunUntil( int address )
		{
			_debugger.State = DebuggerState.Running;
			_cpu.Resume();
		}

		public void Step()
		{
			_debugger.State = DebuggerState.Running;
			_cpu.Resume();
		}

		public void StepInto()
		{
			this.Step();
		}

		public void StepOver()
		{
			//_debugger.State = DebuggerState.Stepping;
			//_cpu.ExecutionMode = ExecutionMode.RunUntil;
			//_cpu.ExecutionParameter = _cpu[ 0 ].ProgramCounter + 4;
			//_cpu.Resume();
		}

		public void StepOut()
		{
			throw new NotImplementedException();
		}

		public void Break()
		{
			_cpu.Break();
		}
	}
}
