// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class InprocDebugger
	{
		private void DisableAll()
		{
			this.CodeTool.Disable();
			this.CallstackTool.Clear();
			this.ThreadsTool.Clear();
		}

		public void OnContinue( bool steppingForward )
		{
			DummyDelegate del = delegate
			{
				if( steppingForward == false )
					this.DisableAll();

				this.SetStatusText( "Running..." );

				this.State = DebuggerState.Running;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnStepComplete( uint address )
		{
			DummyDelegate del = delegate
			{
				this.SetStatusText( "Step completed, now at 0x{0:X8}", address );

				this.PC = address;
				this.JumpToAddress( NavigationTarget.Code, address, true );

				this.State = DebuggerState.Broken;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnBreakpointHit( int id )
		{
			Breakpoint bp = this.Breakpoints[ id ];
			Debug.Assert( bp != null );
			if( bp == null )
			{
				// Not found?
				this.SetStatusText( "Breakpoint hit; ERROR BP NOT FOUND" );
				return;
			}

			DummyDelegate del = delegate
			{
				switch( bp.Type )
				{
					case BreakpointType.CodeExecute:
						this.SetStatusText( "Breakpoint hit; now at 0x{0:X8}", bp.Address );
						this.PC = bp.Address;
						this.JumpToAddress( NavigationTarget.Code, bp.Address, true );
						break;
					case BreakpointType.Stepping:
						this.SetStatusText( "Step completed; now at 0x{0:X8}", bp.Address );
						this.PC = bp.Address;
						this.JumpToAddress( NavigationTarget.Code, bp.Address, true );
						break;
					case BreakpointType.MemoryAccess:
						uint pc = this.DebugHost.CpuHook.GetCoreState( 0 ).ProgramCounter;
						this.SetStatusText( "Breakpoint hit; memory access at 0x{0:X8} of 0x{1:X8}", pc, bp.Address );
						this.PC = pc;
						this.JumpToAddress( NavigationTarget.Memory, bp.Address, true );
						this.JumpToAddress( NavigationTarget.Code, pc, true );
						break;
				}

				this.State = DebuggerState.Broken;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnEvent( Event biosEvent )
		{
			Frame[] frames = this.DebugHost.CpuHook.GetCallstack();
			System.Diagnostics.Debugger.Break();
		}

		public bool OnError( Error error )
		{
			DummyDelegate del = delegate
			{
				this.SetStatusText( "Error hit at 0x{0:X8}: {1}", error.PC, error.Message );

				this.PC = error.PC;
				this.JumpToAddress( NavigationTarget.Code, error.PC, true );

				this.State = DebuggerState.Broken;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );

			return true;
		}
	}
}
