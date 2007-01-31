// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Noxa.Emulation.Psp.Cpu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noxa.Emulation.Psp.Utilities;

namespace Noxa.Emulation.Psp.Video.Xna
{
	partial class VideoDriver : IVideoDriver
	{
		protected ComponentParameters _params;
		protected IEmulationInstance _emulator;
		protected VideoCapabilities _caps;
		protected VideoStatistics _stats;
		protected PerformanceTimer _timer;

		protected DisplayProperties _props;
		protected DisplayProperties _currentProps;

		protected IntPtr _controlHandle;
		protected PresentationParameters _presentParams;
		protected GraphicsDevice _device;
		protected FrameBuffer _frameBuffer;
		protected SpriteBatch _spriteBatch;
		
		internal List<SwapChain> _swapChains;
		internal SwapChain _currentSwapChain;

		protected Thread _thread;
		protected AutoResetEvent _threadSync;
		protected bool _shutdown;

		protected AutoResetEvent _vblankSync;
		protected uint _vcount;

		protected List<DisplayList> _lists;
		protected AutoResetEvent _listSync;

		public VideoDriver( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_params = parameters;
			_emulator = emulator;
			_caps = new VideoCapabilities();
			_stats = new VideoStatistics();
			_timer = new PerformanceTimer();

			_props = new DisplayProperties();

			// Latch our frame buffer into the memory system
			_frameBuffer = new FrameBuffer( this );
			_swapChains = new List<SwapChain>();
			_emulator.Cpu.Memory.RegisterSegment( _frameBuffer );

			_listSync = new AutoResetEvent( false );
			_lists = new List<DisplayList>( 5 );

			_vblankSync = new AutoResetEvent( false );
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _params;
			}
		}

		public IEmulationInstance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		public Type Factory
		{
			get
			{
				return typeof( XnaVideo );
			}
		}

		public IVideoCapabilities Capabilities
		{
			get
			{
				return _caps;
			}
		}

		public IVideoStatistics Statistics
		{
			get
			{
				return _stats;
			}
		}

		public DisplayProperties Properties
		{
			get
			{
				return _props;
			}
		}

		public IntPtr ControlHandle
		{
			get
			{
				return _controlHandle;
			}
			set
			{
				_controlHandle = value;
			}
		}

		public GraphicsDevice Device
		{
			get
			{
				return _device;
			}
		}

		public AutoResetEvent Vblank
		{
			get
			{
				return _vblankSync;
			}
		}

		public uint Vcount
		{
			get
			{
				return _vcount;
			}
		}

		public SwapChain CurrentSwapChain
		{
			get
			{
				return _currentSwapChain;
			}
		}

		public void Suspend()
		{
		}

		public bool Resume()
		{
			if( _device == null )
			{
				if( this.CreateDevice() == false )
					return false;
			}

			if( _props.HasChanged == false )
				return true;

			lock( this )
			{
				Debug.WriteLine( "VideoDriver: video mode change" );

				_currentProps = _props.Clone() as DisplayProperties;
				_props.HasChanged = false;
				_currentProps.HasChanged = false;

				_frameBuffer.Reset();
			}

			return true;
		}

		public void Cleanup()
		{
			if( ( _shutdown == false ) &&
				( _thread != null ) )
			{
				_shutdown = true;

				if( _thread.Join( 500 ) == false )
					_thread.Abort();
				_thread = null;
			}

			CleanupContext();

			if( _spriteBatch != null )
				_spriteBatch.Dispose();
			_spriteBatch = null;

			if( _device != null )
				_device.Dispose();
			_device = null;

			_frameBuffer.Reset();

			_threadSync = null;
		}

		#region Device Management

		private bool CreateDevice()
		{
			_presentParams = new PresentationParameters();
			_presentParams.DeviceWindowHandle = _controlHandle;
			_presentParams.PresentationInterval = PresentInterval.One;
			_presentParams.PresentFlag = PresentFlag.LockableBackBuffer;
			_presentParams.SwapEffect = SwapEffect.Discard;
			_presentParams.IsFullScreen = false;
			_presentParams.BackBufferCount = 1;
			_presentParams.BackBufferFormat = SurfaceFormat.Bgr32;
			_presentParams.BackBufferWidth = 480;
			_presentParams.BackBufferHeight = 272;

			GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;

			_device = TryCreate( adapter, DeviceType.Hardware, CreateOptions.HardwareVertexProcessing );
			if( _device == null )
			{
				_device = TryCreate( adapter, DeviceType.Hardware, CreateOptions.MixedVertexProcessing );
				if( _device == null )
				{
					_device = TryCreate( adapter, DeviceType.Hardware, CreateOptions.SoftwareVertexProcessing );
					if( _device == null )
					{
						_device = TryCreate( adapter, DeviceType.Reference, CreateOptions.SoftwareVertexProcessing );
					}
				}
			}

			if( _device == null )
				return false;

			_spriteBatch = new SpriteBatch( _device );

			InitializeContext();

			_shutdown = false;
			_threadSync = new AutoResetEvent( true );

			_thread = new Thread( new ThreadStart( this.VideoThread ) );
			_thread.Name = "Video worker";
			_thread.Priority = ThreadPriority.BelowNormal;
			_thread.IsBackground = true;
			_thread.Start();

			return true;
		}

		private GraphicsDevice TryCreate( GraphicsAdapter adapter, DeviceType deviceType, CreateOptions creationOptions )
		{
			try
			{
				return new GraphicsDevice( adapter, deviceType, this.ControlHandle, creationOptions, _presentParams );
			}
			catch
			{
				return null;
			}
		}

		#endregion

		#region Swap Chains

		#endregion

		#region Render Loop

		private List<DisplayList> _toProcess = new List<DisplayList>( 5 );

		private void VideoThread()
		{
			try
			{
				while( _shutdown == false )
				{
					double startTime = _timer.Elapsed;
					//lock( this )
					{
						//if( _context.ClearOptions != ( ClearOptions )0 )
						//	_device.Clear( _context.ClearOptions, _context.ClearColor, _context.ClearZDepth, _context.ClearStencil );
						//_device.Clear( Color.Black );
						_device.BeginScene();

						// -- Display list processing --

						if( _lists.Count > 0 )
							_listSync.WaitOne();
						lock( _lists )
						{
						    for( int n = 0; n < _lists.Count; n++ )
						    {
						        DisplayList list = _lists[ n ];
						        lock( list )
						        {
						            if( list.Ready == false )
						                break;

						            _toProcess.Add( list );
						        }
						    }

						    _lists.RemoveRange( 0, _toProcess.Count );
						}

						// Process lists
						for( int n = 0; n < _toProcess.Count; n++ )
							ParseList( _toProcess[ n ] );

						_toProcess.Clear();

						_frameBuffer.Flush();

						_device.EndScene();
						try
						{
							if( _currentSwapChain != null )
								_currentSwapChain.Present();
							else
								_device.Present();
						}
						catch
						{
						}

						if( _vblankSync != null )
							_vblankSync.Set();
						_vcount++;
					}

					//Thread.Sleep( 0 );

					_stats.TimeElapsed += ( _timer.Elapsed - startTime );
					_stats.FrameCount++;
					if( _stats.TimeElapsed > 1.0 )
					{
						_stats._framesPerSecond = ( int )( _stats.FrameCount / _stats.TimeElapsed );
						_stats.TimeElapsed -= 1.0;
						_stats.FrameCount = 0;

						Debug.WriteLine( string.Format( "FPS: {0}", _stats.FramesPerSecond ) );
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

		#endregion

		#region Display Lists

		public DisplayList FindDisplayList( int displayListId )
		{
			lock( _lists )
			{
				for( int n = 0; n < _lists.Count; n++ )
				{
					if( _lists[ n ].ID == displayListId )
						return _lists[ n ];
				}
			}

			return null;
		}

		public bool Enqueue( DisplayList displayList, bool immediate )
		{
			lock( _lists )
			{
				int id = 0;
				for( int n = 0; n < _lists.Count; n++ )
				{
					if( _lists[ n ].ID > id )
						id = _lists[ n ].ID;
				}

				displayList.ID = id;

				if( immediate == true )
					_lists.Insert( 0, displayList );
				else
					_lists.Add( displayList );
			}

			return true;
		}

		public void Abort( int displayListId )
		{
			lock( _lists )
			{
				DisplayList list = FindDisplayList( displayListId );
				if( list == null )
					return;

				_lists.Remove( list );
			}
		}

		public void Sync( DisplayList displayList )
		{
		}

		public void Sync()
		{
			// This means that everything we have in there is ready to draw
			// We could wait on _listSync, but that slows everything down
			// This is really a frame-skip option
			_listSync.Set();
			//Debug.WriteLine( string.Format( "Would Sync(), {0} outstanding lists", _lists.Count ) );
			//lock( _lists )
			//	_lists.Clear();
		}

		#endregion
	}

	class VideoCapabilities : IVideoCapabilities
	{
		public VideoStatisticsCapabilities SupportedStatistics
		{
			get
			{
				return VideoStatisticsCapabilities.FramesPerSecond;
			}
		}
	}

	class VideoStatistics : IVideoStatistics
	{
		internal int _framesPerSecond;
		public int FrameCount;
		public double TimeElapsed;

		public int FramesPerSecond
		{
			get
			{
				return _framesPerSecond;
			}
		}
	}
}
