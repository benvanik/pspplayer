// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Debugging.Protocol;
using Noxa.Emulation.Psp.Debugging.Statistics;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// The local debugging host.
	/// </summary>
	public class DebugHost : MarshalByRefObject
	{
		#region Constants

		/// <summary>
		/// The port the <see cref="DebugHost"/> listens on.
		/// </summary>
		public const int ServerPort = 30001;
		/// <summary>
		/// The port the <see cref="IDebugger"/> listens on.
		/// </summary>
		public const int ClientPort = 30002;

		#endregion

		private bool _isAttached;
		private IDebugger _client;
		private IHook[] _hooks;
		private ManualResetEvent _attachedEvent;

		/// <summary>
		/// The emulator instance that owns the debugger.
		/// </summary>
		public readonly IEmulationHost Emulator;

		/// <summary>
		/// A collection of counters.
		/// </summary>
		public readonly CounterSink Counters;

		/// <summary>
		/// Currently loaded program debug information.
		/// </summary>
		public IDebugDatabase Database;

		/// <summary>
		/// The debug controller implementation.
		/// </summary>
		public IDebugController Controller;

		/// <summary>
		/// A list of <see cref="IHook"/> instances currently active.
		/// </summary>
		public IHook[] Hooks
		{
			get
			{
				return _hooks;
			}
		}

		/// <summary>
		/// The BIOS debug hook, if enabled/supported.
		/// </summary>
		public IBiosHook BiosHook;
		/// <summary>
		/// The CPU debug hook, if enabled/supported.
		/// </summary>
		public ICpuHook CpuHook;
		/// <summary>
		/// The video debug hook, if enabled/supported.
		/// </summary>
		public IVideoHook VideoHook;

		/// <summary>
		/// Description of the host.
		/// </summary>
		public readonly string HostString;

		/// <summary>
		/// Fired when a debugger is attached.
		/// </summary>
		public event EventHandler DebuggerAttached;

		/// <summary>
		/// Initializes a new <see cref="DebugHost"/> instance with the given parameters.
		/// </summary>
		/// <param name="emulationHost">The emulator host instance.</param>
		public DebugHost( IEmulationHost emulationHost )
		{
			Debug.Assert( emulationHost != null );
			this.Emulator = emulationHost;

			this.Counters = new CounterSink();

			_attachedEvent = new ManualResetEvent( false );

			this.HostString = string.Format( "{0}@{1}", Environment.UserName, Environment.MachineName );

			bool setupOk = this.SetupRemoting();
			Debug.Assert( setupOk == true );
		}

		/// <summary>
		/// <c>true</c> if the debugger is attached.
		/// </summary>
		public bool IsAttached
		{
			get
			{
				return _isAttached;
			}
		}

		/// <summary>
		/// The current debugger client.
		/// </summary>
		public IDebugger Client
		{
			get
			{
				return _client;
			}
		}

		#region Setup

		/// <summary>
		/// Simple network tester method.
		/// </summary>
		public void Ping()
		{
		}

		/// <summary>
		/// Activate a remote debugger.
		/// </summary>
		/// <param name="client">The client connecting.</param>
		/// <param name="machineName">The machine name connecting.</param>
		/// <param name="userName">The current user.</param>
		/// <param name="message">A client-defined message.</param>
		/// <returns><c>true</c> if the debugger is allowed to proceed.</returns>
		public bool Activate( IDebugger client, string machineName, string userName, string message )
		{
			Debug.Assert( client != null );

			Debug.Assert( _isAttached == false );
			if( _isAttached == true )
				return false;

			_isAttached = true;

			_client = client;

			// Redirect logger
			Log.Instance = _client.Logger;

			// Say hello
			Log.WriteLine( Verbosity.Normal, Feature.General, "Debugger {0}@{1} connected: {2}", userName, machineName, message );

			if( this.DebuggerAttached != null )
				DebuggerAttached( this, EventArgs.Empty );

			_attachedEvent.Set();

			return true;
		}

		/// <summary>
		/// Wait until a debugger has attached itself.
		/// </summary>
		public void WaitUntilAttached()
		{
			if( _isAttached == true )
				return;
			_attachedEvent.WaitOne();
		}

		/// <summary>
		/// Setup the debugger after an instance has been started.
		/// </summary>
		public void OnInstanceStarted()
		{
			Emulator.AttachDebugger();

			IEmulationInstance emu = this.Emulator.CurrentInstance;
			List<IHook> hooks = new List<IHook>();
			foreach( IComponentInstance instance in emu.Components )
			{
				IDebuggable debuggable = instance as IDebuggable;
				if( debuggable == null )
					continue;
				if( debuggable.SupportsDebugging == false )
				{
					Debug.WriteLine( string.Format( "Debugger: {0} does not support debugging", instance.Factory.Name ) );
					continue;
				}
				debuggable.EnableDebugging();
				hooks.Add( debuggable.DebugHook );

				if( debuggable.DebugHook is IBiosHook )
					this.BiosHook = ( IBiosHook )debuggable.DebugHook;
				else if( debuggable.DebugHook is ICpuHook )
					this.CpuHook = ( ICpuHook )debuggable.DebugHook;
				else if( debuggable.DebugHook is IVideoHook )
					this.VideoHook = ( IVideoHook )debuggable.DebugHook;
			}
			_hooks = hooks.ToArray();

			this.Controller = emu.Cpu.DebugController;

			if( _isAttached == false )
				return;

			_client.OnStarted( emu.Bios.Game, emu.Bios.BootStream );
		}

		/// <summary>
		/// Cleanup the debugger after an instance has been destroyed.
		/// </summary>
		public void OnInstanceStopped()
		{
			_hooks = null;
			this.BiosHook = null;
			this.CpuHook = null;
			this.VideoHook = null;

			if( _isAttached == false )
				return;

			_client.OnStopped();
		}

		#endregion

		#region Remoting services

		private bool SetupRemoting()
		{
			BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
			serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();

			Hashtable serverProps = new Hashtable();
			serverProps[ "port" ] = ServerPort;
			serverProps[ "timeout" ] = Timeout.Infinite;

			try
			{
				TcpChannel channel = new TcpChannel( serverProps, clientProvider, serverProvider );
				ChannelServices.RegisterChannel( channel, false );

				RemotingServices.Marshal( this, "DebugHost" );
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Remoting support: prevents the GC from getting this object.
		/// </summary>
		/// <returns><c>null</c>.</returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}

		#endregion
	}
}
