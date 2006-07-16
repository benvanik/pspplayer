using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;
using Microsoft.DirectX.Direct3D;
using System.Threading;
using System.Drawing;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class VideoDriver : IVideoDriver
	{
		protected ComponentParameters _params;
		protected IEmulationInstance _emulator;

		protected DisplayProperties _props;
		protected DisplayProperties _currentProps;

		protected IntPtr _controlHandle;
		protected PresentParameters _presentParams;
		protected Device _device;
		protected FrameBuffer _frameBuffer;
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
			_frameBuffer = new FrameBuffer( this );
			_emulator.Cpu.Memory.RegisterSegment( _frameBuffer );

			_listSync = new AutoResetEvent( false );
			_lists = new List<DisplayList>( 5 );
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
			_presentParams.SwapEffect = SwapEffect.Flip;
			_presentParams.IsWindowed = true;
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

		private void VideoThread()
		{
			try
			{
				while( _shutdown == false )
				{
					lock( this )
					{
						//_device.Clear( ClearFlags.Target, Color.Red, 0, 0 );
						_device.BeginScene();

						lock( _frameBuffer )
						{
							_frameBuffer.Copy();
						}

						Rectangle fbRect = new Rectangle( 0, 0, _currentProps.Width, _currentProps.Height );
						SizeF fbSize = new SizeF( fbRect.Width, fbRect.Height );
						_sprite.Begin( SpriteFlags.None );
						_sprite.Draw2D( _frameBuffer.Texture, fbRect, fbSize, PointF.Empty, 0.0f, PointF.Empty, Color.Transparent );
						_sprite.End();

						// Display list processing
						List<DisplayList> toProcess = new List<DisplayList>( 5 );
						lock( _lists )
						{
							bool hasElements = _lists.Count > 0;

							for( int n = 0; n < _lists.Count; n++ )
							{
								DisplayList list = _lists[ n ];
								lock( list )
								{
									if( list.Ready == false )
										break;

									toProcess.Add( list );
								}
							}

							_lists.RemoveRange( 0, toProcess.Count );

							if( hasElements == true )
								_listSync.Set();
						}

						// Process lists
						for( int n = 0; n < toProcess.Count; n++ )
							Debug.WriteLine( string.Format( "VideoDriver: got a complete list with {0} packets", toProcess[ n ].Packets.Length ) );

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

					Thread.Sleep( 0 );
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
			_listSync.WaitOne();
		}

		#endregion
	}
}
