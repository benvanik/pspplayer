#if USEVIDEO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using DirectShowLib;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D = Microsoft.DirectX.Direct3D;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar.Helpers
{
	[ComVisible( true )]
	[ClassInterface( ClassInterfaceType.None )]
	class Vmr9Allocator : IVMRSurfaceAllocator9, IVMRImagePresenter9, IDisposable
	{
		protected const int E_FAIL = unchecked( ( int )0x80004005 );
		protected const int DxMagicNumber = -759872593;

		protected Control parentControl;

		protected AdapterDetails _adapterInfo;
		protected Device _device;
		protected Surface _renderTarget;
		protected VideoManager.VideoRenderer _renderer;

		protected Dictionary<IntPtr, Texture> _textures;
		protected Dictionary<IntPtr, Surface> _surfaces;
		protected IntPtr[] _unmanagedSurfaces;
		protected Texture _privateTexture;
		protected Surface _privateSurface;

		protected IVMRSurfaceAllocatorNotify9 _vmrSurfaceAllocatorNotify;

		public Vmr9Allocator( Device device, VideoManager.VideoRenderer renderer )
		{
			_device = device;
			_adapterInfo = D3D.Manager.Adapters[ 0 ];
			_renderTarget = _device.GetRenderTarget( 0 );

			_renderer = renderer;
		}

		~Vmr9Allocator()
		{
			Dispose();
		}

		private void DeleteSurfaces()
		{
			lock( this )
			{
				if( _privateTexture != null )
				{
					_privateTexture.Dispose();
					_privateTexture = null;
				}

				if( _privateSurface != null )
				{
					_privateSurface.Dispose();
					_privateSurface = null;
				}

				if( _textures != null )
				{
					foreach( Texture tex in _textures.Values )
						tex.Dispose();
					_textures = null;
				}

				if( _surfaces != null )
				{
					foreach( Surface surf in _surfaces.Values )
						surf.Dispose();
					_surfaces = null;
				}
			}
		}

		#region IVMRSurfaceAllocator9

		public int InitializeDevice( IntPtr dwUserID, ref VMR9AllocationInfo lpAllocInfo, ref int lpNumBuffers )
		{
			int width = 1;
			int height = 1;
			float fTU = 1.0f;
			float fTV = 1.0f;

			if( _vmrSurfaceAllocatorNotify == null )
			{
				return E_FAIL;
			}

			int hr = 0;

			try
			{
				
				//IntPtr unmanagedDevice = _device.GetObjectByValue( DxMagicNumber );
				IntPtr unmanagedDevice = _device.NativePointer;
				IntPtr hMonitor = D3D.Manager.GetAdapterMonitorHandle( _adapterInfo.Adapter );

				hr = _vmrSurfaceAllocatorNotify.SetD3DDevice( unmanagedDevice, hMonitor );
				DsError.ThrowExceptionForHR( hr );

				if( _device.Capabilities.TextureCaps.SupportsPower2 )
				{
					while( width < lpAllocInfo.dwWidth )
						width = width << 1;
					while( height < lpAllocInfo.dwHeight )
						height = height << 1;

					fTU = ( float )( lpAllocInfo.dwWidth ) / ( float )( width );
					fTV = ( float )( lpAllocInfo.dwHeight ) / ( float )( height );
					//scene.SetSrcRect( fTU, fTV );

					lpAllocInfo.dwWidth = width;
					lpAllocInfo.dwHeight = height;
				}

				// NOTE:
				// we need to make sure that we create textures because
				// surfaces can not be textured onto a primitive.
				lpAllocInfo.dwFlags |= VMR9SurfaceAllocationFlags.TextureSurface;

				DeleteSurfaces();

				_unmanagedSurfaces = new IntPtr[ lpNumBuffers ];

				hr = _vmrSurfaceAllocatorNotify.AllocateSurfaceHelper( ref lpAllocInfo, ref lpNumBuffers, _unmanagedSurfaces );

				// If we couldn't create a texture surface and 
				// the format is not an alpha format,
				// then we probably cannot create a texture.
				// So what we need to do is create a private texture
				// and copy the decoded images onto it.
				if( hr < 0 )
				{
					DeleteSurfaces();

					FourCC fcc = new FourCC( "0000" );

					// is surface YUV ?
					if( lpAllocInfo.Format > fcc.ToInt32() )
					{
						// create the private texture
						_privateTexture = new Texture(
							_device,
							lpAllocInfo.dwWidth,
							lpAllocInfo.dwHeight,
							1,
							Usage.RenderTarget,
							_adapterInfo.CurrentDisplayMode.Format,
							Pool.Default
							);

						_privateSurface = _privateTexture.GetSurfaceLevel( 0 );
					}

					lpAllocInfo.dwFlags &= ~VMR9SurfaceAllocationFlags.TextureSurface;
					lpAllocInfo.dwFlags |= VMR9SurfaceAllocationFlags.OffscreenSurface;

					_unmanagedSurfaces = new IntPtr[ lpNumBuffers ];

					hr = _vmrSurfaceAllocatorNotify.AllocateSurfaceHelper( ref lpAllocInfo, ref lpNumBuffers, _unmanagedSurfaces );
					if( hr < 0 )
						return hr;
				}
				else
				{
					_surfaces = new Dictionary<IntPtr, Surface>( _unmanagedSurfaces.Length );
					_textures = new Dictionary<IntPtr, Texture>( _unmanagedSurfaces.Length );

					for( int i = 0; i < lpNumBuffers; i++ )
					{
						Surface surf = new Surface( _unmanagedSurfaces[ i ] );
						Texture text = ( Texture )surf.GetContainer( new Guid( "85C31227-3DE5-4f00-9B3A-F11AC38C18B5" ) );
						_surfaces.Add( _unmanagedSurfaces[ i ], surf );
						_textures.Add( _unmanagedSurfaces[ i ], text );
					}
				}

				return 0;
			}
			catch( DirectXException e )
			{
				return e.ErrorCode;
			}
			catch
			{
				return E_FAIL;
			}
		}

		public int TerminateDevice( IntPtr dwID )
		{
			DeleteSurfaces();
			return 0;
		}

		public int GetSurface( IntPtr dwUserID, int SurfaceIndex, int SurfaceFlags, out IntPtr lplpSurface )
		{
			lplpSurface = IntPtr.Zero;

			if( SurfaceIndex > _unmanagedSurfaces.Length )
				return E_FAIL;

			lock( this )
			{
				lplpSurface = _unmanagedSurfaces[ SurfaceIndex ];
				Marshal.AddRef( lplpSurface );
				return 0;
			}
		}

		public int AdviseNotify( IVMRSurfaceAllocatorNotify9 lpIVMRSurfAllocNotify )
		{
			lock( this )
			{
				_vmrSurfaceAllocatorNotify = lpIVMRSurfAllocNotify;
				
				//IntPtr unmanagedDevice = _device.GetObjectByValue( DxMagicNumber );
				IntPtr unmanagedDevice = _device.NativePointer;
				IntPtr hMonitor = D3D.Manager.GetAdapterMonitorHandle( D3D.Manager.Adapters[ 0 ].Adapter );
				
				return _vmrSurfaceAllocatorNotify.SetD3DDevice( unmanagedDevice, hMonitor );
			}
		}

		#endregion

		#region IVMRImagePresenter9

		public int StartPresenting( IntPtr dwUserID )
		{
			lock( this )
			{
				if( _device == null )
					return E_FAIL;

				return 0;
			}
		}

		public int StopPresenting( IntPtr dwUserID )
		{
			return 0;
		}

		public int PresentImage( IntPtr dwUserID, ref VMR9PresentationInfo lpPresInfo )
		{
			int hr = 0;

			lock( this )
			{
				try
				{
					// if we are in the middle of the display change
					if( NeedToHandleDisplayChange() )
					{
						// NOTE: this piece of code is left as a user exercise.  
						// The D3DDevice here needs to be switched
						// to the device that is using another adapter
					}

					hr = PresentHelper( lpPresInfo );

					return hr;
				}
				catch( DirectXException e )
				{
					return e.ErrorCode;
				}
				catch
				{
					return E_FAIL;
				}
			}
		}

		private int PresentHelper( VMR9PresentationInfo lpPresInfo )
		{
			int hr = 0;

			try
			{
				_device.SetRenderTarget( 0, _renderTarget );

				if( _privateTexture != null )
				{
					Marshal.AddRef( lpPresInfo.lpSurf );
					using( Surface surface = new Surface( lpPresInfo.lpSurf ) )
					{
						_device.StretchRectangle(
							surface,
							new Rectangle( 0, 0, surface.Description.Width, surface.Description.Height ),
							_privateSurface,
							new Rectangle( 0, 0, _privateSurface.Description.Width, _privateSurface.Description.Height ),
							TextureFilter.None
							);
					}

					_renderer.Texture = _privateTexture;
					_renderer.FrameWaiting.Set();
					//hr = scene.DrawScene( _device, _privateTexture );
					if( hr < 0 )
						return hr;
				}
				else
				{
					if( _textures.ContainsKey( lpPresInfo.lpSurf ) )
					{
						_renderer.Texture = _textures[ lpPresInfo.lpSurf ];
						_renderer.FrameWaiting.Set();
						//hr = scene.DrawScene( _device, _textures[ lpPresInfo.lpSurf ] as Texture );
						if( hr < 0 )
							return hr;
					}
					else
						hr = E_FAIL;
				}

				//_device.Present();
				return 0;
			}
			catch( DirectXException e )
			{
				return e.ErrorCode;
			}
			catch
			{
				return E_FAIL;
			}
		}

		private bool NeedToHandleDisplayChange()
		{
			if( _vmrSurfaceAllocatorNotify == null )
				return false;

			IntPtr currentMonitor = D3D.Manager.GetAdapterMonitorHandle( _device.CreationParameters.AdapterOrdinal );
			IntPtr defaultMonitor = D3D.Manager.GetAdapterMonitorHandle( _adapterInfo.Adapter );

			return currentMonitor != defaultMonitor;
		}

		#endregion

		public void Dispose()
		{
			DeleteSurfaces();
		}

	}

	public class FourCC
	{
		private int fourCC = 0;

		public FourCC( string fcc )
		{
			if( fcc.Length != 4 )
				throw new ArgumentException( fcc + " is not a valid FourCC" );

			byte[] asc = Encoding.ASCII.GetBytes( fcc );

			this.fourCC = asc[ 0 ];
			this.fourCC |= asc[ 1 ] << 8;
			this.fourCC |= asc[ 2 ] << 16;
			this.fourCC |= asc[ 3 ] << 24;
		}

		public FourCC( char a, char b, char c, char d )
			: this( new string( new char[] { a, b, c, d } ) )
		{
		}

		public FourCC( int fcc )
		{
			this.fourCC = fcc;
		}

		public int ToInt32()
		{
			return this.fourCC;
		}

		public Guid ToMediaSubtype()
		{
			return new Guid( this.fourCC.ToString( "X" ) + "-0000-0010-8000-00AA00389B71" );
		}

		public static bool operator ==( FourCC fcc1, FourCC fcc2 )
		{
			return fcc1.fourCC == fcc2.fourCC;
		}

		public static bool operator !=( FourCC fcc1, FourCC fcc2 )
		{
			return fcc1.fourCC != fcc2.fourCC;
		}

		public override bool Equals( object obj )
		{
			if( !( obj is FourCC ) )
				return false;

			return ( obj as FourCC ).fourCC == this.fourCC;
		}

		public override int GetHashCode()
		{
			return this.fourCC.GetHashCode();
		}
	}
}
#endif
