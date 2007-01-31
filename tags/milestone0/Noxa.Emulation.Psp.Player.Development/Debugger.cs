// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.IO.Media;

namespace Noxa.Emulation.Psp.Player.Development
{
	public class Debugger : IDebugger
	{
		private IEmulationHost _host;

		private Studio _studio;
		private DebugControl _control;
		private DebugInspector _inspector;
		private IProgramDebugData _debugData;
		private DebuggerState _state;

		public event EventHandler StateChanged;

		public Debugger( IEmulationHost host )
		{
			Debug.Assert( host != null );
			if( host == null )
				throw new ArgumentNullException( "host" );

			_host = host;

			_control = new DebugControl( this );
			_inspector = new DebugInspector( this );

			// TODO: allow the user to continue without debugging info
			DebugSetup setup = new DebugSetup();
			if( setup.ShowDialog() == System.Windows.Forms.DialogResult.Cancel )
				throw new InvalidOperationException( "Unable to continue without debugging information." );

			_studio = new Studio( this );

			bool result = false;
			if( setup.UseElfDebug == true )
			{
				IMediaFolder folder = host.CurrentInstance.Bios.Kernel.Game.Folder;
				IMediaFile file = folder.FindFile( "BOOT.BIN" );
				using( Stream stream = file.OpenRead() )
					result = this.LoadDebugData( DebugDataType.Elf, stream );
			}
			else
			{
				string filename = setup.ObjdumpFilename;
				using( Stream stream = File.OpenRead( filename ) )
					result = this.LoadDebugData( DebugDataType.Objdump, stream );
			}
			Debug.Assert( result == true );
			if( result == false )
				throw new InvalidOperationException( "Could not load debugging data - cannot continue." );

			// Crazy, I know
			_host.CurrentInstance.Cpu.BreakpointTriggered += new EventHandler<BreakpointEventArgs>( CpuBreakpointTriggered );
		}

		public IEmulationHost Host
		{
			get
			{
				return _host;
			}
		}

		public bool IsAttached
		{
			get
			{
				return true;
			}
		}

		public IDebugControl Control
		{
			get
			{
				return _control;
			}
		}

		public IDebugInspector Inspector
		{
			get
			{
				return _inspector;
			}
		}

		public IProgramDebugData DebugData
		{
			get
			{
				return _debugData;
			}
		}

		public DebuggerState State
		{
			get
			{
				return _state;
			}
			internal set
			{
				if( _state != value )
				{
					_state = value;
					this.StateChanged( this, EventArgs.Empty );
				}
			}
		}

		public bool LoadDebugData( DebugDataType dataType, Stream stream )
		{
			_debugData = ProgramDebugData.Load( dataType, stream );
			return ( _debugData != null );
		}

		public void Show()
		{
			_studio.Show();
		}

		public void Hide()
		{
		}

		private void CpuBreakpointTriggered( object sender, BreakpointEventArgs e )
		{
			this.State = DebuggerState.Paused;

			_studio.OnBreakpointTriggered( e.Breakpoint );
		}
	}
}
