// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// When defined, the framebuffer will be replaced with a null operator, essentially
//#define DUMMYFRAMEBUFFER

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

using Noxa.Emulation.Psp.Cpu;
using Microsoft.DirectX.Direct3D;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	partial class VideoDriver : IVideoDriver
	{
		protected ComponentParameters _params;
		protected IEmulationInstance _emulator;

		protected DisplayProperties _props;
		protected DisplayProperties _currentProps;

		protected IntPtr _controlHandle;
		protected PresentParameters _presentParams;
		protected Device _device;
#if DUMMYFRAMEBUFFER
		protected DummyFrameBuffer _frameBuffer;
#else
		protected FrameBuffer _frameBuffer;
#endif
		protected Sprite _sprite;

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

			_props = new DisplayProperties();

			// Latch our frame buffer into the memory system
#if DUMMYFRAMEBUFFER
			_frameBuffer = new DummyFrameBuffer( this );
#else
			_frameBuffer = new FrameBuffer( this );
#endif
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
				return typeof( Direct3DMVideo );
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

		public Device Device
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

		public IVideoCapabilities Capabilities
		{
			get
			{
				return null;
			}
		}

		public IVideoStatistics Statistics
		{
			get
			{
				return null;
			}
		}

		public IntPtr NativeInterface
		{
			get
			{
				return IntPtr.Zero;
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

			if( _sprite != null )
				_sprite.Dispose();
			_sprite = null;

			if( _device != null )
				_device.Dispose();
			_device = null;

			_frameBuffer.Reset();

			_threadSync = null;
		}

		#region Device Management

		private bool CreateDevice()
		{
			_presentParams = new PresentParameters();
			_presentParams.PresentationInterval = PresentInterval.One;
			_presentParams.PresentFlag = PresentFlag.LockableBackBuffer;
			_presentParams.SwapEffect = SwapEffect.Copy;
			_presentParams.Windowed = true;
			_presentParams.BackBufferFormat = Format.A8R8G8B8;

			int adapter = 0;
			_device = TryCreate( adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.HardwareVertexProcessing );
			if( _device == null )
			{
				_device = TryCreate( adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.MixedVertexProcessing );
				if( _device == null )
				{
					_device = TryCreate( adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing );
					if( _device == null )
					{
						_device = TryCreate( adapter, DeviceType.Software, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing );
						if( _device == null )
						{
							_device = TryCreate( adapter, DeviceType.Reference, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing );
						}
					}
				}
			}

			if( _device == null )
				return false;

			_sprite = new Sprite( _device );

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

		private Device TryCreate( int adapter, DeviceType deviceType, CreateFlags createFlags )
		{
			try
			{
				return new Device( adapter, deviceType, _controlHandle, createFlags, _presentParams );
			}
			catch
			{
				return null;
			}
		}

		#endregion

		#region Render Loop

		private List<DisplayList> _toProcess = new List<DisplayList>( 5 );

		private void VideoThread()
		{
			try
			{
				while( _shutdown == false )
				{
					lock( this )
					{
						if( _context.ClearFlags != ( ClearFlags )0 )
							_device.Clear( _context.ClearFlags, _context.ClearColor, _context.ClearZDepth, _context.ClearStencil );
						_device.BeginScene();

						// Display list processing
						
						//if( _lists.Count > 0 )
						//	_listSync.WaitOne();
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
}
