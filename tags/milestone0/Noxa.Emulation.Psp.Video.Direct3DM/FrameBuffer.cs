// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//#define DEBUGPATTERN

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using Noxa.Emulation.Psp.Cpu;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class FrameBuffer : IMemorySegment
	{
		// Framebuffer memory actually has 3 banks, I think (0x1FFFFF / 0x88000 = 3)
		// May be set by the software to go to any of them

		protected VideoDriver _driver;
		protected Texture _texture;
		protected Rectangle _rect;
		protected IntPtr[] _buffers;
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
				//Format format;
				//switch( _driver.Properties.PixelFormat )
				//{
				//    case PixelFormat.Rgb565:
				//        format = Format.R5G6B5;
				//        break;
				//    case PixelFormat.Rgba4444:
				//        format = Format.A4R4G4B4;
				//        break;
				//    case PixelFormat.Rgba5551:
				//        format = Format.A1R5G5B5;
				//        break;
				//    case PixelFormat.Rgba8888:
				//    default:
				//        format = Format.A8R8G8B8;
				//        break;
				//}

				if( _texture != null )
					_texture.Dispose();
				_texture = null;

				if( _buffers != null )
				{
					for( int n = 0; n < _buffers.Length; n++ )
						Marshal.FreeHGlobal( _buffers[ n ] );
					_buffers = null;
				}

				if( _driver.Device == null )
				{
					_texture = null;
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

				_buffers = new IntPtr[ 3 ];
				for( int n = 0; n < _buffers.Length; n++ )
					_buffers[ n ] = Marshal.AllocHGlobal( 512 * 272 * 4 );

				// Ideally we wouldn't have to do this
				for( int n = 0; n < _buffers.Length; n++ )
				{
					unsafe
					{
						if( _isHalfWord == false )
						{
							uint* ptr = ( uint* )_buffers[ n ].ToPointer();
							for( int m = 0; m < 512 * 272; m++ )
							{
#if DEBUGPATTERN
								*ptr = 0xFF4488CC;
#else
								*ptr = 0xFF000000;
#endif
								ptr++;
							}
						}
						else
						{
							ushort* ptr = ( ushort* )_buffers[ n ].ToPointer();
							for( int m = 0; m < 512 * 272; m++ )
							{
#if DEBUGPATTERN
								*ptr = 0xFFFF;
#else
								*ptr = 0x0000;
#endif
								ptr++;
							}
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
			int currentBuffer = ( ( int )_driver.Properties.BufferAddress - 0x04000000 ) / 0x88000;
			if( currentBuffer < 0 )
				currentBuffer = 0;

			GraphicsStream buffer;
			try
			{
				buffer = _texture.LockRectangle( 0, LockFlags.None );
			}
			catch
			{
				return;
			}

			//FastRandom r = new FastRandom( Environment.TickCount );

			unsafe
			{
				void* dvptr = buffer.InternalDataPointer;
				void* svptr = _buffers[ currentBuffer ].ToPointer();
				Debug.Assert( new IntPtr( dvptr ) != IntPtr.Zero );

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

			_texture.UnlockRectangle( 0 );
			
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
			throw new NotImplementedException();
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
				address -= 0x04000000;

				int currentBuffer = address / 0x88000;
				void* vptr = _buffers[ currentBuffer ].ToPointer();

				if( _isHalfWord == false )
				{
					uint uv = ( uint )value;
					uint* ptr = ( uint* )vptr;
					ptr += ( ( address - 0x88000 * currentBuffer ) >> 2 ); // div by 4 because we are incrementing by words instead of bytes
					
					// Comes at us in RGBA, need ARGB
					uint ut = ( uv >> 8 ) | ( ( uv & 0xFF ) << 24 );

					*ptr = ut;
				}
				else
				{
					ushort us = ( ushort )value;
					ushort* ptr = ( ushort* )vptr;
					ptr += ( ( address - 0x88000 * currentBuffer ) >> 1 ); // div by 2 because we are incrementing by half words instead of bytes
					*ptr = us;
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
			throw new NotImplementedException();
		}

		public void Load( string fileName )
		{
			throw new NotImplementedException();
		}

		public void Save( System.IO.Stream stream )
		{
			throw new NotImplementedException();
		}

		public void Save( string fileName )
		{
			throw new NotImplementedException();
		}

		public uint GetMemoryHash( int address, int count, uint prime )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
