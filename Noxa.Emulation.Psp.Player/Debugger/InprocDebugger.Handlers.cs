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
		public void OnContinue( bool steppingForward )
		{
			DummyDelegate del = delegate
			{
				if( steppingForward == false )
					this.CodeTool.Disable();

				this.State = DebuggerState.Running;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnStepComplete( uint address )
		{
			DummyDelegate del = delegate
			{
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
				return;
			}

			DummyDelegate del = delegate
			{
				switch( bp.Type )
				{
					case BreakpointType.CodeExecute:
					case BreakpointType.Stepping:
						this.PC = bp.Address;
						this.JumpToAddress( NavigationTarget.Code, bp.Address, true );
						break;
					case BreakpointType.MemoryAccess:
						uint pc = this.DebugHost.CpuHook.GetCoreState( 0 ).ProgramCounter;
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
