// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Debugging.Protocol;
using Noxa.Emulation.Psp.Debugging.Statistics;
using Noxa.Emulation.Psp.Games;

using Noxa.Emulation.Psp.RemoteDebugger.Tools;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	enum DebuggerState
	{
		/// <summary>
		/// The debugger is not attached.
		/// </summary>
		Detached,
		/// <summary>
		/// The debugger is idle, waiting to start.
		/// </summary>
		Idle,
		/// <summary>
		/// The debugger is running code.
		/// </summary>
		Running,
		/// <summary>
		/// The debugger is stopped in the middle of execution.
		/// </summary>
		Broken,
	}

	class EmuDebugger : MarshalByRefObject, IDebugger, IDebugHandler
	{
		public DebugHost Host;
		public DebugView View;
		public CodeView Code;
		public LogViewer Log;
		public StatisticsViewer Statistics;
		public CallstackViewer CallStack;

		public BreakpointManager Breakpoints;

		public bool IsConnected;
		public bool CallStacksEnabled;

		private DebuggerState _state = DebuggerState.Detached;
		public event EventHandler StateChanged;
		public DebuggerState State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		public ILogger Logger
		{
			get
			{
				return this.Log;
			}
		}

		public IDebugHandler Handler
		{
			get
			{
				return this;
			}
		}

		public EmuDebugger()
		{
			bool setupOk = this.SetupRemoting();
			Debug.Assert( setupOk == true );

			this.CallStacksEnabled = true;

			this.Breakpoints = new BreakpointManager( this );

			this.View = new DebugView( this );

			// Log is created here because it is special
			this.Log = new LogViewer( this );

			this.OnStateChanged();
		}

		public void StartConnection()
		{
			ConnectionDialog dialog = new ConnectionDialog( this );
			dialog.ShowDialog( this.View );

			if( this.IsConnected == true )
			{
				this.Code = new CodeView( this );
				this.Statistics = new StatisticsViewer( this );
				this.CallStack = new CallstackViewer( this );

				this.Code.Show( this.View.DockPanel );
				this.Log.Show( this.View.DockPanel );
				this.Statistics.Show( this.View.DockPanel );
				if( this.CallStacksEnabled == true )
					this.CallStack.Show( this.View.DockPanel );
			}
			else
				Environment.Exit( 0 );
		}

		public bool Connect( string host )
		{
			this.Host = ( DebugHost )Activator.GetObject( typeof( DebugHost ), string.Format( "tcp://{0}:{1}/DebugHost", host, DebugHost.ServerPort ) );
			try
			{
				this.Host.Ping();
			}
			catch( Exception )
			{
				Debug.WriteLine( "EmuDebugger::Connect: ping failed" );// + ex.ToString() );
				return false;
			}

			this.Host.Activate( this, Environment.MachineName, Environment.UserName, "RemoteDebugger 1.0" );

			this.IsConnected = true;
			return true;
		}

		private int _ids = 100;
		public int AllocateID()
		{
			return Interlocked.Increment( ref _ids );
		}

		#region Remoting services

		public bool SetupRemoting()
		{
			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
			BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
			serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			Hashtable clientProps = new Hashtable();
			clientProps[ "port" ] = DebugHost.ClientPort;
			clientProps[ "timeout" ] = Timeout.Infinite;

			TcpChannel chan = new TcpChannel( clientProps, clientProvider, serverProvider );
			ChannelServices.RegisterChannel( chan, false );

			return true;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		#endregion

		#region Debugger State

		private delegate void DummyDelegate();

		public void OnStarted( GameInformation game, Stream bootStream )
		{
			// Need to marshal all UI calls to the proper thread
			DummyDelegate del = delegate
			{
				this.Log.OnStarted();
				this.Statistics.OnStarted();

				this.View.SetStatusText( Verbosity.Normal, "Connection to emulator {0} established.", this.Host.HostString );

				this.State = DebuggerState.Running;
				this.OnStateChanged();
			};
			this.View.Invoke( del );

			// TEST
			Breakpoint bp = new Breakpoint( this.AllocateID(), BreakpointType.CodeExecute, 0x08900334 );
			this.Breakpoints.Add( bp );
		}

		public void OnStopped()
		{
			DummyDelegate del = delegate
			{
				this.Log.OnStopped();
				this.Statistics.OnStopped();

				this.State = DebuggerState.Detached;
				this.OnStateChanged();
			};
			this.View.Invoke( del );
		}

		#endregion

		#region Handlers

		private Frame[] GetCallstack()
		{
			if( this.CallStacksEnabled == true )
				return this.Host.CpuHook.GetCallstack();
			else
				return null;
		}

		private delegate void ShowSourceViewDelegate( uint address );
		private void ShowSourceView( uint address )
		{
			ShowSourceViewDelegate del = delegate
			{
				this.Code.BringToFront();
				this.Code.Activate();
				this.Code.Show( this.View.DockPanel );

				// Jump in code
				this.Code.SetAddress( address );

				if( this.CallStacksEnabled == true )
				{
					Frame[] stack = this.GetCallstack();
					this.CallStack.SetCallstack( stack );
				}

				this.State = DebuggerState.Broken;
				this.OnStateChanged();
			};
			this.View.Invoke( del, address );
		}

		public void OnContinue()
		{
			DummyDelegate del = delegate
			{
				this.Code.Disable();

				this.State = DebuggerState.Running;
				this.OnStateChanged();
			};
			this.View.Invoke( del );
		}

		public void OnStepComplete( uint address )
		{
			this.ShowSourceView( address );
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
			this.ShowSourceView( bp.Address );
		}

		public void OnEvent( Event biosEvent )
		{
			Frame[] stack = this.GetCallstack();
			Debugger.Break();
		}

		public bool OnError( Error error )
		{
			this.ShowSourceView( error.PC );
			Debugger.Break();

			return true;
		}

		#endregion

		internal void OnConnectionLost()
		{
			if( this.IsConnected == false )
				return;
			this.IsConnected = false;

			this.OnStopped();

			this.View.SetStatusText( Verbosity.Critical, "Connection to emulator lost!" );
		}

		private void OnStateChanged()
		{
			if( this.StateChanged != null )
				this.StateChanged( this, EventArgs.Empty );
		}
	}
}
