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
				//if( steppingForward == false )
				//	this.Code.Disable();

				this.State = DebuggerState.Running;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnStepComplete( uint address )
		{
			DummyDelegate del = delegate
			{
				this.ShowSourceView( address );

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
				this.ShowSourceView( bp.Address );

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
				this.ShowSourceView( error.PC );

				this.State = DebuggerState.Broken;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );

			return true;
		}

		private void ShowSourceView( uint address )
		{
			this.CodeTool.Show( this.Window.DockPanel );
			this.CodeTool.BringToFront();
			this.CodeTool.Activate();

			// Jump in code
			//this.CodeTool.SetAddress( address );

			this.CallstackTool.RefreshCallstack();
		}
	}
}
