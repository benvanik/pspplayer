// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

using Noxa.Emulation.Psp.Audio;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Video;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.Input;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.GameTester
{
	class TestInstance : IEmulationInstance
	{
		protected TestHost _host;
		protected EmulationParameters _params;

		protected List<IComponentInstance> _instances = new List<IComponentInstance>();
		protected IAudioDriver _audio;
		protected IBios _bios;
		protected ICpu _cpu;
		protected List<IIODriver> _io = new List<IIODriver>();
		protected IInputDevice _input;
		protected IUmdDevice _umd;
		protected IMemoryStickDevice _memoryStick;
		protected IVideoDriver _video;

		protected Thread _thread;
		protected bool _isCreated = false;
		protected bool _shutDown = false;
		protected InstanceState _state = InstanceState.Idle;
		protected AutoResetEvent _stateChangeEvent = new AutoResetEvent( false );
		protected bool _switchToXmb = true;

		public TestInstance( TestHost host, EmulationParameters parameters )
		{
			Debug.Assert( host != null );
			Debug.Assert( parameters != null );

			_host = host;
			_params = parameters;
		}

		public IEmulationHost Host
		{
			get
			{
				return _host;
			}
		}

		public EmulationParameters Parameters
		{
			get
			{
				return _params;
			}
		}

		public IAudioDriver Audio
		{
			get
			{
				return _audio;
			}
		}

		public IBios Bios
		{
			get
			{
				return _bios;
			}
		}

		public ICpu Cpu
		{
			get
			{
				return _cpu;
			}
		}

		public ReadOnlyCollection<IIODriver> IO
		{
			get
			{
				return _io.AsReadOnly();
			}
		}

		public IInputDevice Input
		{
			get
			{
				return _input;
			}
		}

		public IUmdDevice Umd
		{
			get
			{
				return _umd;
			}
		}

		public IMemoryStickDevice MemoryStick
		{
			get
			{
				return _memoryStick;
			}
		}

		public IVideoDriver Video
		{
			get
			{
				return _video;
			}
		}

		public InstanceState State
		{
			get
			{
				return _state;
			}
		}

		public event EventHandler StateChanged;

		protected void OnStateChanged()
		{
			EventHandler handler = this.StateChanged;
			if( handler != null )
				handler( this, EventArgs.Empty );
		}

		private void TestComponent( List<ComponentIssue> issues, IComponent component, ComponentType type, bool required )
		{
			if( component == null )
			{
				if( required == true )
					issues.Add( new ComponentIssue( null, IssueLevel.Error,
						string.Format( "ErrorRequiredComponentNotFound", type ) ) );
				else
					issues.Add( new ComponentIssue( null, IssueLevel.Warning,
						string.Format( "ErrorOptionalComponentNotFound", type ) ) );
				return;
			}

			if( component.IsTestable == true )
			{
				IList<ComponentIssue> testResults = component.Test( _params[ component ] );
				if( testResults != null )
					issues.AddRange( testResults );
			}
		}

		public List<ComponentIssue> Test()
		{
			List<ComponentIssue> issues = new List<ComponentIssue>();
			TestComponent( issues, _params.CpuComponent, ComponentType.Cpu, true );
			TestComponent( issues, _params.BiosComponent, ComponentType.Bios, true );
			//TestComponent( issues, _params.VideoComponent, ComponentType.Video, true );
			foreach( IComponent component in _params.IOComponents )
				TestComponent( issues, component, component.Type, false );
			TestComponent( issues, _params.AudioComponent, ComponentType.Audio, false );
			return issues;
		}

		public bool Create()
		{
			Debug.Assert( _isCreated == false );
			if( _isCreated == true )
				return true;

			_shutDown = false;
			_state = InstanceState.Idle;

			// Try to create all the components
			Debug.Assert( _params.BiosComponent != null );
			Debug.Assert( _params.CpuComponent != null );
			if( _params.AudioComponent != null )
			{
				_audio = _params.AudioComponent.CreateInstance( this, _params[ _params.AudioComponent ] ) as IAudioDriver;
				_instances.Add( ( IComponentInstance )_audio );
			}
			_cpu = _params.CpuComponent.CreateInstance( this, _params[ _params.CpuComponent ] ) as ICpu;
			_instances.Add( ( IComponentInstance )_cpu );
			_bios = _params.BiosComponent.CreateInstance( this, _params[ _params.BiosComponent ] ) as IBios;
			_instances.Add( ( IComponentInstance )_bios );
			foreach( IComponent component in _params.IOComponents )
			{
				IIODriver driver = component.CreateInstance( this, _params[ component ] ) as IIODriver;
				_io.Add( driver );
				_instances.Add( ( IComponentInstance )driver );
			}
			if( _params.InputComponent != null )
			{
				_input = _params.InputComponent.CreateInstance( this, _params[ _params.InputComponent ] ) as IInputDevice;
				_instances.Add( _input );
				//_input.WindowHandle = _host.Player.Handle;
			}
			if( _params.UmdComponent != null )
			{
				_umd = _params.UmdComponent.CreateInstance( this, _params[ _params.UmdComponent ] ) as IUmdDevice;
				_instances.Add( _umd );
			}
			if( _params.MemoryStickComponent != null )
			{
				_memoryStick = _params.MemoryStickComponent.CreateInstance( this, _params[ _params.MemoryStickComponent ] ) as IMemoryStickDevice;
				_instances.Add( _memoryStick );
			}
			if( _params.VideoComponent != null )
			{
				_video = _params.VideoComponent.CreateInstance( this, _params[ _params.VideoComponent ] ) as IVideoDriver;
				//_video.ControlHandle = _host.Player.ControlHandle;
				_instances.Add( ( IComponentInstance )_video );
			}

			// Create thread
			_thread = new Thread( new ThreadStart( this.RuntimeThread ) );
			_thread.Name = "Host runtime thread";
			_thread.IsBackground = true;
			_thread.Start();

			_isCreated = true;

			return true;
		}

		public void Destroy()
		{
			Debug.Assert( _isCreated == true );
			if( _isCreated == false )
				return;

			// Destroy thread
			_shutDown = true;
			_stateChangeEvent.Set();
			_thread.Interrupt();
			_cpu.Stop();
			if( _thread.Join( 1000 ) == false )
			{
				// Failed to wait, so kill
				_thread.Abort();
			}
			while( _thread.IsAlive == true )
				Thread.Sleep( 10 );
			_thread = null;

			// Destroy all the components
			foreach( IComponentInstance component in _instances )
				component.Cleanup();
			_instances.Clear();
			_audio = null;
			_bios = null;
			_cpu = null;
			_io.Clear();
			_video = null;

			_isCreated = false;
			_state = InstanceState.Idle;
			this.OnStateChanged();
		}

		public void Start( bool debugging )
		{
			if( _isCreated == false )
				this.Create();

			switch( _state )
			{
				case InstanceState.Running:
					return;
				case InstanceState.Paused:
					this.Resume();
					return;
				case InstanceState.Debugging:
					// TODO: debugger hook on Start()
					return;
				case InstanceState.Crashed:
					// Tried to start after crashing? Should not be allowed!
					Debug.WriteLine( "Tried to Start() after crashing; calling Restart() instead" );
					this.Restart();
					return;
				case InstanceState.Ended:
					this.Restart();
					return;
			}

			if( debugging == true )
				_host.AttachDebugger();

			_state = InstanceState.Running;
			_stateChangeEvent.Set();
			this.OnStateChanged();
		}

		public void Stop()
		{
			Debug.Assert( _isCreated == true );
			if( _isCreated == false )
				return;

			_state = InstanceState.Ended;
			_stateChangeEvent.Set();
			this.OnStateChanged();

			_cpu.PrintStatistics();
			if( _video != null )
				_video.PrintStatistics();

			this.Destroy();
		}

		public void Pause()
		{
			Debug.Assert( _isCreated == true );
			if( _isCreated == false )
				return;

			if( _state != InstanceState.Running )
				return;

			_state = InstanceState.Paused;
			_stateChangeEvent.Set();
			this.OnStateChanged();
		}

		public void Resume()
		{
			Debug.Assert( _isCreated == true );
			if( _isCreated == false )
				return;

			if( _state != InstanceState.Paused )
				return;

			_state = InstanceState.Running;
			_stateChangeEvent.Set();
			this.OnStateChanged();
		}

		public void Restart()
		{
			Debug.Assert( _isCreated == true );
			if( _isCreated == false )
				return;

			this.Destroy();
			this.Create();
			this.Start( ( _host.Debugger != null ) );
		}

		public void LightReset()
		{
		}

		public delegate void DummyDelegate();

		public void SwitchToXmb()
		{
			if( _bios.Game == null )
				return;
			Debug.WriteLine( "Instance: switching to XMB" );
			_video.Cleanup();
			this.Stop();
			this.Destroy();
		}

		public LoadResults SwitchToGame( Games.GameInformation game )
		{
			Debug.WriteLine( "Instance: switching to game " + game.Parameters.Title );
			_bios.Game = game;
			return _bios.Load();
		}

		private void RuntimeThread()
		{
			try
			{
				if( _switchToXmb == true )
					this.SwitchToXmb();

				while( _shutDown == false )
				{
					switch( _state )
					{
						case InstanceState.Ended:
						case InstanceState.Idle:
						case InstanceState.Paused:
						case InstanceState.Crashed:
							_stateChangeEvent.WaitOne();
							break;
						case InstanceState.Debugging:
							// TODO: debugging runtime loop code
							break;
						case InstanceState.Running:
							Debug.Assert( _bios.Game != null );
							if( _bios.Game != null )
							{
								// Run the kernel
								_bios.Execute();
							}
							break;
					}
				}
			}
			catch( ThreadAbortException )
			{
			}
			catch( ThreadInterruptedException )
			{
			}
		}
	}
}
