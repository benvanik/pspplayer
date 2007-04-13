// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class CachedVertexBuffer
	{
		public VertexFormats Format;
		public int StrideSize;
		public VertexBuffer Buffer;

		public CachedVertexBuffer( VertexFormats format, int strideSize, VertexBuffer buffer )
		{
			Debug.Assert( format != VertexFormats.None );
			Debug.Assert( strideSize > 0 );
			Debug.Assert( buffer != null );

			Format = format;
			StrideSize = strideSize;
			Buffer = buffer;
		}
	}

	partial class VideoDriver
	{
		protected byte[] _vertexTemp = new byte[ 16 * 1000 ];
		protected int _vertexTempLength = 0;

		protected unsafe CachedVertexBuffer BuildVertexBuffer( int vertexCount, int vertexSize, byte* ptr, int length )
		{
			VertexTypes vertexType = _context.VertexType;

			bool hasPosition = false;
			VertexTypes positionMask = vertexType & VertexTypes.PositionMask;
			if( ( positionMask == VertexTypes.PositionFixed8 ) ||
				( positionMask == VertexTypes.PositionFixed16 ) ||
				( positionMask == VertexTypes.PositionFloat ) )
				hasPosition = true;

			bool hasNormal = false;
			VertexTypes normalMask = vertexType & VertexTypes.NormalMask;
			if( ( normalMask == VertexTypes.NormalFixed8 ) ||
				( normalMask == VertexTypes.NormalFixed16 ) ||
				( normalMask == VertexTypes.NormalFloat ) )
				hasNormal = true;

			bool hasTexture = false;
			VertexTypes textureType = vertexType & VertexTypes.TextureMask;
			if( ( textureType == VertexTypes.TextureFixed8 ) ||
				( textureType == VertexTypes.TextureFixed16 ) ||
				( textureType == VertexTypes.TextureFloat ) )
				hasTexture = true;

			bool hasWeight = false;
			VertexTypes weightType = vertexType & VertexTypes.WeightMask;
			if( ( weightType == VertexTypes.WeightFixed8 ) ||
				( weightType == VertexTypes.WeightFixed16 ) ||
				( weightType == VertexTypes.WeightFloat ) )
				hasWeight = true;

			bool hasColor = false;
			VertexTypes colorType = vertexType & VertexTypes.ColorMask;
			if( ( colorType == VertexTypes.ColorBGR5650 ) ||
				( colorType == VertexTypes.ColorABGR4444 ) ||
				( colorType == VertexTypes.ColorABGR5551 ) ||
				( colorType == VertexTypes.ColorABGR8888 ) )
				hasColor = true;

			bool isIndexed = false;
			VertexTypes indexType = vertexType & VertexTypes.IndexMask;
			if( ( indexType == VertexTypes.Index8 ) ||
				( indexType == VertexTypes.Index16 ) )
				isIndexed = true;

			// TODO: detect if transformed?
			bool transformed = false;

			// TODO: support indices and weights
			Debug.Assert( ( isIndexed == false ) && ( hasWeight == false ) );

			VertexFormats format = VertexFormats.None;
			int strideSize = 0;
			bool convertedOk = this.ConvertVertexFormat( transformed, hasPosition, hasNormal, hasColor, hasTexture, hasWeight, isIndexed, out format, out strideSize );
			Debug.Assert( convertedOk == true );
			if( convertedOk == false )
				return null;
			Debug.Assert( format != VertexFormats.None );
			Debug.Assert( strideSize > 0 );

			// TODO: write a cool codegen system to take the vertex structure and build an ultra-optimized reader for it, not this mess

			unsafe
			{
				byte* sourceBase = ptr;
				fixed( byte* destBase = &_vertexTemp[ 0 ] )
				{
					byte* src = sourceBase;
					byte* dest = destBase;

					float x = 0, y = 0, z = 0;
					float nx = 0, ny = 0, nz = 0;
					int c = 0;
					float tu = 0, tv = 0;

					for( int n = 0; n < vertexCount; n++ )
					{
						// Unfortunately the order required by d3d is different than what we get from the psp, so we need to
						// do this out of order and kind of nasty-like

						// PSP:
						//float skinWeight[WEIGHTS_PER_VERTEX];
						//float u,v;
						//unsigned int color;
						//float nx,ny,nz;
						//float x,y,z;

						// D3D:
						//float x, y, z;
						//float nx, ny, nz;
						//int color;
						//float tu, tv;
						//weights?

						if( hasWeight == true )
						{
							// TODO: skip over weights here
							if( weightType == VertexTypes.WeightFixed8 )
							{
							}
							else if( weightType == VertexTypes.WeightFixed16 )
							{
							}
							else if( weightType == VertexTypes.WeightFloat )
							{
							}
							Debugger.Break();
						}
						if( hasTexture == true )
						{
							if( textureType == VertexTypes.TextureFixed8 )
							{
								Debugger.Break();
							}
							else if( textureType == VertexTypes.TextureFixed16 )
							{
								short a = *( short* )src;
								src += 2;
								short b = *( short* )src;
								src += 2;
								tu = a;
								tv = b;
							}
							else if( textureType == VertexTypes.TextureFloat )
							{
								tu = *( float* )src;
								src += 4;
								tv = *( float* )src;
								src += 4;
							}
						}
						if( hasColor == true )
						{
							if( colorType == VertexTypes.ColorBGR5650 )
							{
								Debugger.Break();
							}
							else if( colorType == VertexTypes.ColorABGR4444 )
							{
								Debugger.Break();
							}
							else if( colorType == VertexTypes.ColorABGR5551 )
							{
								Debugger.Break();
							}
							else if( colorType == VertexTypes.ColorABGR8888 )
							{
								//c = ( src[ 0 ] << 24 ) | ( src[ 3 ] << 16 ) | ( src[ 2 ] << 8 ) | src[ 1 ];
								c = *( int* )src;
								c = ( c & unchecked( ( int )0xFF00FF00 ) ) | ( ( c & 0x00FF0000 ) >> 16 ) | ( ( c & 0x000000FF ) << 16 );
								src += 4;
							}
						}
						if( hasNormal == true )
						{
							if( normalMask == VertexTypes.NormalFixed8 )
							{
								Debugger.Break();
							}
							else if( normalMask == VertexTypes.NormalFixed16 )
							{
								Debugger.Break();
							}
							else if( normalMask == VertexTypes.NormalFloat )
							{
								nx = *( float* )src;
								src += 4;
								ny = *( float* )src;
								src += 4;
								nz = *( float* )src;
								src += 4;
							}
						}
						if( hasPosition == true )
						{
							if( positionMask == VertexTypes.PositionFixed8 )
							{
								Debugger.Break();
							}
							else if( positionMask == VertexTypes.PositionFixed16 )
							{
								short a = *( short* )src;
								src += 2;
								short b = *( short* )src;
								src += 2;
								short d = *( short* )src;
								src += 2;
								x = a;
								y = b;
								z = d;
							}
							else if( positionMask == VertexTypes.PositionFloat )
							{
								x = *( float* )src;
								src += 4;
								y = *( float* )src;
								src += 4;
								z = *( float* )src;
								src += 4;
							}
						}

						// Write out - ugly
						if( transformed == true )
						{
							Debug.Assert( false );
						}
						if( hasPosition == true )
						{
							*( float* )dest = x;
							dest += 4;
							*( float* )dest = y;
							dest += 4;
							*( float* )dest = z;
							dest += 4;
						}
						if( hasNormal == true )
						{
							*( float* )dest = nx;
							dest += 4;
							*( float* )dest = ny;
							dest += 4;
							*( float* )dest = nz;
							dest += 4;
						}
						if( hasColor == true )
						{
							*( int* )dest = c;
							dest += 4;
						}
						if( hasTexture == true )
						{
							*( float* )dest = tu;
							dest += 4;
							*( float* )dest = tv;
							dest += 4;
						}
						if( hasWeight == true )
						{
							Debug.Assert( false );
						}
					}
				}
			}

			_vertexTempLength = strideSize * vertexCount;
			VertexBuffer vb = new VertexBuffer( _device, _vertexTempLength, Usage.WriteOnly, format, Pool.Default );
			vb.Created += new EventHandler( VertexBufferCreated );
			VertexBufferCreated( vb, EventArgs.Empty );
			CachedVertexBuffer ret = new CachedVertexBuffer( format, strideSize, vb );
			return ret;
		}

		protected bool ConvertVertexFormat( bool transformed, bool hasPosition, bool hasNormal, bool hasColor, bool hasTexture, bool hasWeight, bool isIndexed, out VertexFormats format, out int strideSize )
		{
			format = VertexFormats.None;
			strideSize = 0;

			if( ( transformed == true ) && ( hasPosition == true ) )
				return false;

			if( transformed == true )
			{
				format |= VertexFormats.Transformed;
				strideSize += 16;
			}
			if( hasPosition == true )
			{
				format |= VertexFormats.Position;
				strideSize += 12;
			}
			if( hasNormal == true )
			{
				format |= VertexFormats.Normal;
				strideSize += 12;
			}
			if( hasColor == true )
			{
				format |= VertexFormats.Diffuse;
				strideSize += 4;
			}
			if( hasTexture == true )
			{
				format |= VertexFormats.Texture0;
				strideSize += 8;
			}
			if( hasWeight == true )
			{
				// TODO: weighting
				Debug.Assert( false );
				return false;
			}

			return true;
		}

		protected void VertexBufferCreated( object sender, EventArgs e )
		{
			try
			{
				VertexBuffer buffer = sender as VertexBuffer;
				Debug.Assert( buffer != null );
				Debug.Assert( _vertexTemp != null );
				Debug.Assert( _vertexTemp.Length > 0 );

				using( GraphicsStream gb = buffer.Lock( 0, _vertexTempLength, LockFlags.NoSystemLock ) )
				{
					gb.Write( _vertexTemp, 0, _vertexTempLength );
					buffer.Unlock();
				}
			}
			catch
			{
				Debugger.Break();
				throw;
			}
		}
	}
}
