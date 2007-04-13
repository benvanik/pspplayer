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
		protected Rectangle _rect;
		protected IntPtr[] _buffers;
		protected bool _isHalfWord;

		protected uint[] _localBuffer;
		protected byte[] _finalBuffer;

		protected bool _outstandingWrites;
		protected int _dirtyMinX;
		protected int _dirtyMaxX;
		protected int _dirtyMinY;
		protected int _dirtyMaxY;

		public FrameBuffer( VideoDriver driver )
		{
			Debug.Assert( driver != null );

			_driver = driver;
		}

		public Rectangle Dimensions
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

				_outstandingWrites = false;
				_dirtyMinX = int.MaxValue;
				_dirtyMaxY = int.MaxValue;
				_dirtyMaxX = int.MinValue;
				_dirtyMaxY = int.MinValue;

				if( _buffers != null )
				{
					for( int n = 0; n < _buffers.Length; n++ )
						Marshal.FreeHGlobal( _buffers[ n ] );
					_buffers = null;
				}

				_localBuffer = null;

				if( _driver.Device == null )
					return;

				// TODO: figure out video stuff better
				// This is the virtual buffer - 512x272, regardless of what everyone else tells us

				//_texture = new Texture( _driver.Device,
				//    //_driver.Properties.Width, _driver.Properties.Height,
				//    512, 272,
				//    1, Usage.Dynamic,
				//    //format,
				//    Format.A8R8G8B8,
				//    Pool.Default );

				_rect = new Rectangle( 0, 0, 512, 272 );

				PixelFormat pixelFormat = _driver.Properties.PixelFormat;
				_isHalfWord = ( pixelFormat == PixelFormat.Rgb565 ) || ( pixelFormat == PixelFormat.Rgba5551 ) || ( pixelFormat == PixelFormat.Rgba4444 );

				_buffers = new IntPtr[ 3 ];
				for( int n = 0; n < _buffers.Length; n++ )
					_buffers[ n ] = Marshal.AllocHGlobal( 512 * 272 * 4 );

				_localBuffer = new uint[ 512 * 272 ];
				_finalBuffer = new byte[ _localBuffer.Length * 4 ];

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
			}
		}

		public void Flush()
		{
			lock( this )
			{
				if( _outstandingWrites == false )
					return;
				_outstandingWrites = false;

				int currentBuffer = ( ( int )_driver.Properties.BufferAddress - 0x04000000 ) / 0x88000;
				if( currentBuffer < 0 )
					currentBuffer = 0;

				Surface surface = _driver.Device.GetBackBuffer( 0, 0, BackBufferType.Mono );

				_dirtyMinX = Math.Max( 0, _dirtyMinX );
				_dirtyMinY = Math.Max( 0, _dirtyMinY );
				Rectangle dirtyRect = new Rectangle( _dirtyMinX, _dirtyMinY, _dirtyMaxX - _dirtyMinX, _dirtyMaxY - _dirtyMinY );
				if( dirtyRect.Right >= 480 )
					dirtyRect.Width = 480 - dirtyRect.X;
				if( dirtyRect.Bottom >= 272 )
					dirtyRect.Height = 272 - dirtyRect.Y;
				_dirtyMinX = int.MaxValue;
				_dirtyMinY = int.MaxValue;
				_dirtyMaxX = int.MinValue;
				_dirtyMaxY = int.MinValue;

				//dirtyRect = new Rectangle( 0, 0, 480, 272 );
				int count = dirtyRect.Width * dirtyRect.Height;
				if( count <= 0 )
					return;
				//surface.GetData<uint>( dirtyRect, _localBuffer, 0, count );
				Array.Clear( _localBuffer, 0, count );

				// TODO: probably want to blend values instead of copy them

				unsafe
				{
					FastRandom r = new FastRandom();
					fixed( void* dvptr = _localBuffer )
					{
						void* svptr = _buffers[ currentBuffer ].ToPointer();

						if( _isHalfWord == false )
						{
							uint* dptr = ( uint* )dvptr;

							for( int y = 0; y < dirtyRect.Height; y++ )
							{
								uint* sptr = ( uint* )svptr + ( dirtyRect.Y + y ) * 512 + dirtyRect.X;

								for( int x = 0; x < dirtyRect.Width; x++ )
								{
									uint val = *sptr;
									*sptr = 0;
									//if( val != 0 )
									*dptr = val;
									//*dptr = *sptr;
									//*dptr = 0xFF000000 | r.NextUInt();
									dptr++;
									sptr++;
								}
							}
						}
						else
						{
							ushort* dptr = ( ushort* )dvptr;
							ushort* sptr = ( ushort* )svptr;

							for( int n = 0; n < 512 * 272; n++ )
							{
								ushort val = *sptr;
								*sptr = 0;
								if( val != 0 )
									*dptr = val;
								//*dptr = *sptr;
								dptr++;
								sptr++;
							}
						}
					}
				}

				Array.Clear( _finalBuffer, 0, _finalBuffer.Length );
				int pitch;
				using( GraphicsStream gs = surface.LockRectangle( dirtyRect, LockFlags.None, out pitch ) )
				{
					// TODO: find a way to avoid this copy
					unsafe
					{
						fixed( uint* sp = _localBuffer )
						fixed( byte* dp = _finalBuffer )
						{
							uint* spp = sp;
							for( int yy = 0; yy < dirtyRect.Height; yy++ )
							{
								uint* dpp = ( uint* )dp + ( yy * ( pitch / 4 ) ) + dirtyRect.Left;
								for( int xx = 0; xx < dirtyRect.Width; xx++ )
								{
									*dpp = *spp;
									dpp++;
									spp++;
								}
							}
						}
						//	Marshal.Copy( new IntPtr( ( int* )sp ), _finalBuffer, 0, count * 4 );
					}
					gs.Write( _finalBuffer, 0, count * 4 );
					surface.UnlockRectangle();
				}
			}
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

		protected void OnMemoryChanged( IMemorySegment segment, int address, int width, int value )
		{
			MemoryChangeDelegate handler = this.MemoryChanged;
			if( handler != null )
				handler( segment, address, width, value );
		}

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
			lock( this )
			{
				unsafe
				{
					address -= 0x04000000;

					int currentBuffer = address / 0x88000;
					void* vptr = _buffers[ currentBuffer ].ToPointer();

					address -= 0x88000 * currentBuffer;

					int xaddress = address >> 2;

					// In bytes
					int x = xaddress % _rect.Width;
					int y = xaddress / _rect.Width;

					if( _isHalfWord == false )
					{
						uint uv = ( uint )value;
						uint* ptr = ( uint* )vptr;
						ptr += ( address >> 2 ); // div by 4 because we are incrementing by words instead of bytes

						// Comes at us in RGBA, need ABGR
						//uint ut = ( uv >> 8 ) | ( ( uv & 0xFF ) << 24 ); <-- ARGB
						uint ut = ( ( uv << 8 ) & 0x00FF0000 ) | ( ( uv >> 8 ) & 0x0000FF00 ) | ( ( uv >> 24 ) & 0xFF );

						*ptr = ut;

						// 4 bytes per pixel
						//x >>= 2;
					}
					else
					{
						ushort us = ( ushort )value;
						ushort* ptr = ( ushort* )vptr;
						ptr += ( address >> 1 ); // div by 2 because we are incrementing by half words instead of bytes
						*ptr = us;

						// 2 bytes per pixel
						//x >>= 1;
					}

					if( x < _dirtyMinX )
						_dirtyMinX = x;
					if( x + 1 > _dirtyMaxX )
						_dirtyMaxX = x + 1;
					if( y < _dirtyMinY )
						_dirtyMinY = y;
					if( y + 1 > _dirtyMaxY )
						_dirtyMaxY = y + 1;
				}

				_outstandingWrites = true;
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
