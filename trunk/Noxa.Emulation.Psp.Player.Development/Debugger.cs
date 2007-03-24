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
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Media;
using Noxa.Emulation.Psp.Games;

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

		private ICpuHook _cpuHook;
		private IBiosHook _biosHook;

		public event EventHandler StateChanged;

		public Debugger( IEmulationHost host )
		{
			Debug.Assert( host != null );
			if( host == null )
				throw new ArgumentNullException( "host" );

			_host = host;

			_control = new DebugControl( this );
			_inspector = new DebugInspector( this );

			_studio = new Studio( this );
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

		public ICpuHook CpuHook
		{
			get
			{
				return _cpuHook;
			}
			set
			{
				_cpuHook = value;
			}
		}

		public IBiosHook BiosHook
		{
			get
			{
				return _biosHook;
			}
			set
			{
				_biosHook = value;
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

		public void SetupGame( GameInformation game, Stream bootStream )
		{
			// Attempt to load symbols from game information
			bool debugInfoLoaded = false;
			if( bootStream != null )
				debugInfoLoaded = this.LoadDebugData( DebugDataType.Symbols, bootStream );

			// If nothing loaded, give the user a choice
			bool skipLoadInfo = debugInfoLoaded;
			DebugSetup setup = null;
			if( debugInfoLoaded == false )
			{
				setup = new DebugSetup();
				if( setup.ShowDialog() == System.Windows.Forms.DialogResult.Cancel )
					skipLoadInfo = true;
			}

			if( skipLoadInfo == false )
			{
				bool result = false;
				if( setup.UseElfDebug == true )
				{
					IMediaFolder folder = game.Folder;
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
	}
}
