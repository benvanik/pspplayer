using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Drawing;
using Noxa.Emulation.Psp.Player.CrossMediaBar.Helpers;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Threading;
namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	abstract class VideoResource
	{
		public float Width;
		public float Height;

		public virtual void Draw( float x, float y )
		{
			this.Draw( x, y, Width, Height );
		}

		public abstract void Draw( float x, float y, float width, float height );
		public abstract void Cleanup();
		public abstract void Enable( Device device, Sprite sprite, Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font> fonts );
		public abstract void Disable();
	}

	class VideoManager
	{
		protected Manager _manager;
		protected IntPtr _controlHandle;
		protected Device _device;
		protected List<VideoResource> _resources = new List<VideoResource>();
		protected Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font> _fonts = new Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font>();
		protected Sprite _sprite;

		public VideoManager( Manager manager, IntPtr controlHandle )
		{
			Debug.Assert( manager != null );
			Debug.Assert( controlHandle != IntPtr.Zero );

			_manager = manager;
			_controlHandle = controlHandle;
		}

		public void Enable()
		{
			PresentParameters presentParams = new PresentParameters();
			presentParams.DeviceWindowHandle = _controlHandle;
			presentParams.IsWindowed = true;
			presentParams.PresentFlag = PresentFlag.DeviceClip;
			presentParams.SwapEffect = SwapEffect.Flip;
			presentParams.PresentationInterval = PresentInterval.Default;

			int adapter = 0;

			_device = TryCreate( _controlHandle, adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.HardwareVertexProcessing, presentParams );
			if( _device == null )
			{
				_device = TryCreate( _controlHandle, adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.MixedVertexProcessing, presentParams );
				if( _device == null )
				{
					_device = TryCreate( _controlHandle, adapter, DeviceType.Hardware, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing, presentParams );
					if( _device == null )
					{
						_device = TryCreate( _controlHandle, adapter, DeviceType.Software, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing, presentParams );
						if( _device == null )
						{
							_device = TryCreate( _controlHandle, adapter, DeviceType.Reference, CreateFlags.FpuPreserve | CreateFlags.SoftwareVertexProcessing, presentParams );
							if( _device == null )
							{
								// Wow, really can't get anything
								Debug.WriteLine( "VideoManager(): all combos tried, cannot find valid device" );
								return;
							}
						}
					}
				}
			}

			_sprite = new Sprite( _device );

			foreach( VideoResource resource in _resources )
				resource.Enable( _device, _sprite, _fonts );

			_manager.FirstRender = true;
		}

		public void Disable()
		{
			foreach( VideoResource resource in _resources )
				resource.Disable();

			_fonts.Clear();

			_sprite.Dispose();
			_sprite = null;

			_device.Dispose();
			_device = null;
		}

		public void Recreate()
		{
			foreach( VideoResource resource in _resources )
				resource.Enable( _device, _sprite, _fonts );

			_manager.FirstRender = true;
		}

		private Device TryCreate( IntPtr controlHandle, int adapter, DeviceType deviceType, CreateFlags behaviorFlags, PresentParameters presentParams )
		{
			try
			{
				return new Device( adapter, deviceType, controlHandle, behaviorFlags, presentParams );
			}
			catch( Exception ex )
			{
				Debug.WriteLine( "VideoManager.TryCreate: " + ex.ToString() );
				return null;
			}
		}

		public bool IsValid
		{
			get
			{
				return _device != null;
			}
		}

		public void Begin()
		{
			//_device.Clear( ClearFlags.Target, Color.Black, 0, 0 );

			_device.BeginScene();
			//_sprite.Begin( SpriteFlags.AlphaBlend | SpriteFlags.SortTexture );
		}

		public void End()
		{
			if( _device == null )
				return;

			//_sprite.End();
			_device.EndScene();

			try
			{
				_device.Present();
			}
			catch
			{
			}
		}

		public void Clear()
		{
			_device.Clear( ClearFlags.Target, Color.Black, 0, 0 );
			this.End();
		}

		public void Cleanup()
		{
			if( _device == null )
				return;

			foreach( VideoResource resource in _resources )
				resource.Cleanup();
			_resources.Clear();

			foreach( Microsoft.DirectX.Direct3D.Font font in _fonts.Values )
				font.Dispose();
			_fonts.Clear();

			_sprite.Dispose();

			_device.Dispose();
			_device = null;
		}

		public VideoResource CreateVideoResource( byte[] videoBytes )
		{
			VideoRenderer r = new VideoRenderer();
			r.VideoBytesSource = videoBytes;
			_resources.Add( r );
			return r;
		}

		public VideoResource CreateImageResource( Stream imageStream )
		{
			ImageRenderer r = new ImageRenderer();
			r.TextureStreamSource = imageStream;
			_resources.Add( r );
			return r;
		}

		public VideoResource CreateImageResource( Image image )
		{
			ImageRenderer r = new ImageRenderer();
			r.TextureImageSource = image;
			_resources.Add( r );
			return r;
		}

		public VideoResource CreateTextResource( System.Drawing.Font font, string text, Color color )
		{
			TextRenderer r = new TextRenderer();
			r.Text = text;
			r.Color = color;
			r.SystemFont = font;
			_resources.Add( r );
			return r;
		}

		#region Resources

		internal class VideoRenderer : VideoResource
		{
			public Vmr9Allocator Allocator;
			public Sprite Sprite;
			public Texture Texture;

			public byte[] VideoBytesSource;
			public string VideoFileSource;

			public AutoResetEvent FrameWaiting = new AutoResetEvent( false );

			private IGraphBuilder graph = null;
			private IBaseFilter filter = null;
			private IMediaControl mediaControl = null;			

			private IntPtr userId = new IntPtr( unchecked( ( int )0xACDCACDC ) );

			public override void Draw( float x, float y, float width, float height )
			{
				//Sprite.Begin( SpriteFlags.AlphaBlend | SpriteFlags.SortTexture );
				Sprite.Begin( SpriteFlags.None );
				if( Texture != null )
					Sprite.Draw2D( Texture, new Rectangle( 0, 0, ( int )Width, ( int )Height ), new SizeF( width, height ), new PointF( x, y ), Color.White );
				Sprite.End();
			}

			public override void Cleanup()
			{
				FilterState state;

				if( mediaControl != null )
				{
					do
					{
						mediaControl.Stop();
						mediaControl.GetState( 0, out state );
					} while( state != FilterState.Stopped );

					mediaControl = null;
				}

				if( Allocator != null )
				{
					Allocator.Dispose();
					Allocator = null;
				}

				if( filter != null )
				{
					Marshal.ReleaseComObject( filter );
					filter = null;
				}

				if( graph != null )
				{
					RemoveAllFilters();

					Marshal.ReleaseComObject( graph );
					graph = null;
				}
			}

			public void RemoveAllFilters()
			{
				int hr = 0;
				IEnumFilters enumFilters;
				List<IBaseFilter> filtersArray = new List<IBaseFilter>();

				hr = graph.EnumFilters( out enumFilters );
				DsError.ThrowExceptionForHR( hr );

				IBaseFilter[] filters = new IBaseFilter[ 1 ];
				int fetched;

				while( enumFilters.Next( filters.Length, filters, out fetched ) == 0 )
					filtersArray.Add( filters[ 0 ] );

				foreach( IBaseFilter filter in filtersArray )
				{
					hr = graph.RemoveFilter( filter );
					while( Marshal.ReleaseComObject( filter ) > 0 ) ;
				}
			}

			private void SetAllocatorPresenter( Device device )
			{
				int hr = 0;

				IVMRSurfaceAllocatorNotify9 vmrSurfAllocNotify = ( IVMRSurfaceAllocatorNotify9 )filter;

				try
				{
					Allocator = new Vmr9Allocator( device, this );

					hr = vmrSurfAllocNotify.AdviseSurfaceAllocator( userId, Allocator );
					DsError.ThrowExceptionForHR( hr );

					hr = Allocator.AdviseNotify( vmrSurfAllocNotify );
					DsError.ThrowExceptionForHR( hr );
				}
				catch
				{
					Allocator = null;
					throw;
				}
			}

			public override void Enable( Device device, Sprite sprite, Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font> fonts )
			{
				Sprite = sprite;
				if( ( VideoFileSource == null ) ||
					( File.Exists( VideoFileSource ) == false ) )
				{
					VideoFileSource = Path.GetTempFileName();
					File.WriteAllBytes( VideoFileSource, VideoBytesSource );
				}
				
				int hr = 0;
				try
				{
					graph = ( IGraphBuilder )new FilterGraph();
					filter = ( IBaseFilter )new VideoMixingRenderer9();

					IVMRFilterConfig9 filterConfig = ( IVMRFilterConfig9 )filter;

					hr = filterConfig.SetRenderingMode( VMR9Mode.Renderless );
					DsError.ThrowExceptionForHR( hr );

					hr = filterConfig.SetNumberOfStreams( 2 );
					DsError.ThrowExceptionForHR( hr );

					SetAllocatorPresenter( device );

					hr = graph.AddFilter( filter, "Video Mixing Renderer 9" );
					DsError.ThrowExceptionForHR( hr );

					hr = graph.RenderFile( VideoFileSource, null );
					DsError.ThrowExceptionForHR( hr );

					//Foo();

					mediaControl = ( IMediaControl )graph;
				}
				catch
				{
				}
			}

			private void Foo()
			{
				int hr = 0;
				IEnumFilters enumFilters;
				List<IBaseFilter> filtersArray = new List<IBaseFilter>();

				hr = graph.EnumFilters( out enumFilters );
				DsError.ThrowExceptionForHR( hr );

				IBaseFilter[] filters = new IBaseFilter[ 1 ];
				int fetched;

				while( enumFilters.Next( filters.Length, filters, out fetched ) == 0 )
					filtersArray.Add( filters[ 0 ] );

				foreach( IBaseFilter filter in filtersArray )
				{
					FilterInfo fi;
					filter.QueryFilterInfo( out fi );
					Debug.WriteLine( fi.achName );
				}
			}

			public override void Disable()
			{
				this.Cleanup();

				Sprite = null;
			}

			public void Play()
			{
				int hr = mediaControl.Run();
				DsError.ThrowExceptionForHR( hr );
			}

			public void Stop()
			{
				int hr = mediaControl.Stop();
				DsError.ThrowExceptionForHR( hr );
			}

			public bool IsPlaying
			{
				get
				{
					FilterState pfs;
					mediaControl.GetState( 10, out pfs );
					if( pfs == FilterState.Running )
						return true;
					else
						return false;
				}
			}
		}

		internal class ImageRenderer : VideoResource
		{
			public Sprite Sprite;
			public Texture Texture;

			public Stream TextureStreamSource;
			public Image TextureImageSource;

			public override void Draw( float x, float y, float width, float height )
			{
				if( width <= 0 )
					width = Width;
				if( height <= 0 )
					height = Height;

				Sprite.Begin( SpriteFlags.AlphaBlend | SpriteFlags.SortTexture );
				Sprite.Draw2D( Texture, new Rectangle( 0, 0, ( int )Width, ( int )Height ), new SizeF( width, height ), new PointF( x, y ), Color.White );
				Sprite.End();
			}

			public override void Cleanup()
			{
				if( Texture != null )
				{
					Texture.Dispose();
					Texture = null;
				}
			}

			public override void Enable( Device device, Sprite sprite, Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font> fonts )
			{
				Sprite = sprite;
				if( TextureStreamSource != null )
				{
					TextureStreamSource.Seek( 0, SeekOrigin.Begin );
					ImageInformation info = Texture.GetImageInformationFromStream( TextureStreamSource );
					TextureStreamSource.Seek( 0, SeekOrigin.Begin );
					//Texture = new Texture( device, TextureStreamSource );
					Texture = new Texture( device, TextureStreamSource, ( int )TextureStreamSource.Length, info.Width, info.Height, 1, Usage.None, info.Format, Pool.Managed, Filter.Linear, Filter.Linear, 0, false, null );
					Width = info.Width;
					Height = info.Height;
				}
				else
				{
					Texture = Texture.FromBitmap( device, TextureImageSource as Bitmap, Usage.None, Pool.Managed );
					Width = TextureImageSource.Width;
					Height = TextureImageSource.Height;
				}
			}

			public override void Disable()
			{
				if( Texture != null )
				{
					Texture.Dispose();
					Texture = null;
				}

				Sprite = null;
			}
		}

		internal class TextRenderer : VideoResource
		{
			public System.Drawing.Font SystemFont;
			public Microsoft.DirectX.Direct3D.Font Font;
			public string Text;
			public Color Color;

			public override void Draw( float x, float y, float width, float height )
			{
				Font.DrawString( null, Text, new Point( ( int )x, ( int )y ), Color );
			}

			public override void Cleanup()
			{
			}

			public override void Enable( Device device, Sprite sprite, Dictionary<System.Drawing.Font, Microsoft.DirectX.Direct3D.Font> fonts )
			{
				if( fonts.ContainsKey( SystemFont ) == false )
					fonts.Add( SystemFont, new Microsoft.DirectX.Direct3D.Font( device, SystemFont ) );

				Font = fonts[ SystemFont ];

				Rectangle bounds = Font.MeasureString( null, Text, DrawStringFormat.None, Color );
				Width = bounds.Width;
				Height = bounds.Height;
			}

			public override void Disable()
			{
				if( Font != null )
				{
					Font.Dispose();
					Font = null;
				}
			}
		}

		#endregion
	}
}
