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
	class EmuDebugger : MarshalByRefObject, IDebugger, IDebugHandler
	{
		public DebugHost Host;
		public DebugView View;
		public CodeView Code;
		public LogViewer Log;
		public StatisticsViewer Statistics;

		public BreakpointManager Breakpoints;

		public bool IsConnected;

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

			this.Breakpoints = new BreakpointManager( this );

			this.View = new DebugView( this );

			// Log is created here because it is special
			this.Log = new LogViewer( this );
		}

		public void StartConnection()
		{
			ConnectionDialog dialog = new ConnectionDialog( this );
			dialog.ShowDialog( this.View );

			if( this.IsConnected == true )
			{
				this.Code = new CodeView( this );
				this.Statistics = new StatisticsViewer( this );

				this.Code.Show( this.View.DockPanel );
				this.Log.Show( this.View.DockPanel );
				this.Statistics.Show( this.View.DockPanel );
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
			catch( Exception ex )
			{
				Debug.WriteLine( "EmuDebugger::Connect: ping failed: " + ex.ToString() );
				return false;
			}

			this.Host.Activate( this, Environment.MachineName, Environment.UserName, "RemoteDebugger 1.0" );

			this.IsConnected = true;
			return true;
		}

		#region Remoting services

		public bool SetupRemoting()
		{
			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
			BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
			serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			Hashtable clientProps = new Hashtable();
			clientProps[ "port" ] = DebugHost.ClientPort;

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
			};
			this.View.Invoke( del );
		}

		public void OnStopped()
		{
			DummyDelegate del = delegate
			{
				this.Log.OnStopped();
				this.Statistics.OnStopped();
			};
			this.View.Invoke( del );
		}

		#endregion

		#region Handlers

		public void OnStepComplete( int address )
		{
		}

		public void OnBreakpointHit( int id )
		{
		}

		public void OnEvent( Event biosEvent )
		{
		}

		public void OnError( Error error )
		{
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
	}
}
