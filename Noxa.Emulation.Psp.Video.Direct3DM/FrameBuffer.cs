using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using System.IO;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class FrameBuffer : IMemorySegment
	{
		protected VideoDriver _driver;
		protected Texture _texture;
		protected Rectangle _rect;
		protected IntPtr _buffer;
		protected bool _isHalfWord;

		public FrameBuffer( VideoDriver driver )
		{
			Debug.Assert( driver != null );

			_driver = driver;
		}

		public Texture Texture
		{
			get
			{
				return _texture;
			}
		}

		public Rectangle TextureRectangle
		{
			get
			{
				return _rect;
			}
		}

		public void Reset()
		{
			lock( this )
			{
				Format format;
				switch( _driver.Properties.PixelFormat )
				{
					case PixelFormat.Rgb565:
						format = Format.R5G6B5;
						break;
					case PixelFormat.Rgba4444:
						format = Format.A4R4G4B4;
						break;
					case PixelFormat.Rgba5551:
						format = Format.A1R5G5B5;
						break;
					case PixelFormat.Rgba8888:
					default:
						format = Format.A8R8G8B8;
						break;
				}

				if( _texture != null )
					_texture.Dispose();
				_texture = null;

				if( _buffer != null )
					Marshal.FreeHGlobal( _buffer );

				if( _driver.Device == null )
				{
					_texture = null;
					_buffer = IntPtr.Zero;
					return;
				}

				// TODO: figure out video stuff better
				// This is the virtual buffer - 512x272, regardless of what everyone else tells us

				_texture = new Texture( _driver.Device,
					//_driver.Properties.Width, _driver.Properties.Height,
					512, 272,
					1, Usage.Dynamic,
					//format,
					Format.A8R8G8B8,
					Pool.Default );

				_rect = new Rectangle( 0, 0, 512, 272 );

				PixelFormat pixelFormat = _driver.Properties.PixelFormat;
				_isHalfWord = ( pixelFormat == PixelFormat.Rgb565 ) || ( pixelFormat == PixelFormat.Rgba5551 ) || ( pixelFormat == PixelFormat.Rgba4444 );

				_buffer = Marshal.AllocHGlobal( 512 * 272 * 4 );
				unsafe
				{
					if( _isHalfWord == false )
					{
						uint* ptr = ( uint* )_buffer.ToPointer();
						for( int n = 0; n < 512 * 272; n++ )
						{
							*ptr = 0xFF4488CC;
							ptr++;
						}
					}
					else
					{
						ushort* ptr = ( ushort* )_buffer.ToPointer();
						for( int n = 0; n < 512 * 272; n++ )
						{
							*ptr = 0xFFFF;
							ptr++;
						}
					}
				}

				//GraphicsBuffer<int> buffer = _texture.Lock<int>( 0, null, LockFlags.None );
				//buffer.AllocateNew( _rect.Width * _rect.Height * 4 );
				//_texture.Unlock( 0 );
			}
		}

		public void Copy()
		{
			GraphicsBuffer<int> buffer;
			try
			{
				buffer = _texture.Lock<int>( 0, null, LockFlags.None );
			}
			catch
			{
				return;
			}

			//FastRandom r = new FastRandom( Environment.TickCount );

			unsafe
			{
				void* dvptr = buffer.DataBufferPointer;
				void* svptr = _buffer.ToPointer();

				if( _isHalfWord == false )
				{
					uint* dptr = ( uint* )dvptr;
					uint* sptr = ( uint* )svptr;

					for( int n = 0; n < 512 * 272; n++ )
					{
						*dptr = *sptr;
						//*dptr = 0xFF000000 | r.NextUInt();
						dptr++;
						sptr++;
					}
				}
				else
				{
					ushort* dptr = ( ushort* )dvptr;
					ushort* sptr = ( ushort* )svptr;

					for( int n = 0; n < 512 * 272; n++ )
					{
						*dptr = *sptr;
						dptr++;
						sptr++;
					}
				}
			}

			_texture.Unlock( 0 );
			
			//_texture.Save( "a." + DateTime.Now.Millisecond.ToString() + ".bmp", ImageFileFormat.Bitmap );
		}

		#region IMemorySegment Members

		public MemoryType MemoryType
		{
			get
			{
				return MemoryType.HardwareMapped;
			}
		}

		public string Name
		{
			get
			{
				return "Frame Buffer";
			}
		}

		public int BaseAddress
		{
			get
			{
				return 0x04000000;
			}
		}

		public int Length
		{
			get
			{
				return 0x001FFFFF;
			}
		}

		public event MemoryChangeDelegate MemoryChanged;

		public int ReadWord( int address )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public byte[] ReadBytes( int address, int count )
		{
			throw new NotImplementedException();
		}

		public int ReadStream( int address, Stream destination, int count )
		{
			throw new NotImplementedException();
		}

		public void WriteWord( int address, int width, int value )
		{
			unsafe
			{
				void* vptr = _buffer.ToPointer();

				if( _isHalfWord == false )
				{
					uint uv = ( uint )value;
					uint* ptr = ( uint* )vptr;
					ptr += ( ( address - 0x04000000 ) >> 2 ); // div by 4 because we are incrementing by words instead of bytes
					
					// Comes at us in RGBA, need ARGB
					uint ut = ( uv >> 8 ) | ( ( uv & 0xFF ) << 24 );

					*ptr = ut;
				}
				else
				{
					ushort us = ( ushort )value;
					ushort* ptr = ( ushort* )vptr;
					ptr += ( ( address - 0x04000000 ) >> 1 ); // div by 2 because we are incrementing by half words instead of bytes
					*ptr = us;
				}

				if( value != 0 )
				{
					int x = 6;
				}
			}
		}

		public void WriteBytes( int address, byte[] bytes )
		{
			throw new NotImplementedException();
		}

		public void WriteBytes( int address, byte[] bytes, int index, int count )
		{
			throw new NotImplementedException();
		}

		public void WriteStream( int address, Stream source, int count )
		{
			throw new NotImplementedException();
		}

		public void Load( System.IO.Stream stream )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Load( string fileName )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Save( System.IO.Stream stream )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Save( string fileName )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion
	}
}
