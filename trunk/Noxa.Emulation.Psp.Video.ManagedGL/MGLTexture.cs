// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tao.OpenGl;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe class MGLTexture
	{
		public TexturePixelStorage PixelStorage;
		public uint Address;
		public uint LineWidth;
		public uint Width;
		public uint Height;

		public int TextureID;

		public uint Checksum;
		public uint Cookie;
		public uint CookieOriginal;

		// If PixelStorage is IndexedN
		public uint ClutPointer;
		public uint ClutChecksum;

		#region TextureFormats

		private class TextureFormat
		{
			public readonly TexturePixelStorage Format;
			public readonly uint Size;
			public readonly uint Flags;
			public readonly int GLFormat;
			public TextureFormat( TexturePixelStorage format, uint size, uint flags, int glFormat )
			{
				this.Format = format;
				this.Size = size;
				this.Flags = flags;
				this.GLFormat = glFormat;
			}
		}

		private static readonly TextureFormat[] TextureFormats = new TextureFormat[]{
			new TextureFormat( TexturePixelStorage.BGR5650,		2, 0, Gl.GL_UNSIGNED_SHORT_5_6_5_REV ),
			new TextureFormat( TexturePixelStorage.ABGR5551,	2, 1, Gl.GL_UNSIGNED_SHORT_1_5_5_5_REV ),
			new TextureFormat( TexturePixelStorage.ABGR4444,	2, 1, Gl.GL_UNSIGNED_SHORT_4_4_4_4_REV ),
			new TextureFormat( TexturePixelStorage.ABGR8888,	4, 1, Gl.GL_UNSIGNED_BYTE ),
			new TextureFormat( TexturePixelStorage.Indexed4,	0, 1, Gl.GL_UNSIGNED_BYTE ),
			new TextureFormat( TexturePixelStorage.Indexed8,	1, 1, Gl.GL_UNSIGNED_BYTE ),
			new TextureFormat( TexturePixelStorage.Indexed16,	2, 1, Gl.GL_UNSIGNED_BYTE ),
			new TextureFormat( TexturePixelStorage.Indexed32,	4, 1, Gl.GL_UNSIGNED_BYTE ),
			new TextureFormat( TexturePixelStorage.DXT1,		4, 1, Gl.GL_COMPRESSED_RGBA_S3TC_DXT1_EXT ),
			new TextureFormat( TexturePixelStorage.DXT3,		4, 1, Gl.GL_COMPRESSED_RGBA_S3TC_DXT3_EXT ),
			new TextureFormat( TexturePixelStorage.DXT5,		4, 1, Gl.GL_COMPRESSED_RGBA_S3TC_DXT5_EXT ),
		};

		#endregion

		private static int _tt = 0;
		public static MGLTexture LoadTexture( MGLDriver driver, MGLContext ctx, MGLTextureInfo info, uint checksum )
		{
			uint width = info.Width;
			uint lineWidth = info.LineWidth;
			uint height = info.Height;

			MGLTexture texture = new MGLTexture();
			texture.PixelStorage = info.PixelStorage;
			texture.Address = info.Address;
			texture.LineWidth = lineWidth;
			texture.Width = width;
			texture.Height = height;
			texture.Checksum = checksum;
			texture.ClutPointer = ctx.Clut.Pointer;
			texture.ClutChecksum = ctx.Clut.Checksum;

			int textureId;
			//Gl.glGenTextures( 1, out textureId );
			//Gl.glBindTexture( Gl.GL_TEXTURE_2D, textureId );
			//texture.TextureID = textureId;
			texture.TextureID = _tt++;

			byte* address = driver.MemorySystem.Translate( info.Address );
			TextureFormat format = TextureFormats[ ( int )info.PixelStorage ];
			uint size = lineWidth * height * format.Size;

			fixed( byte* unswizzleBuffer = &_unswizzleBuffer[ 0 ] )
			fixed( byte* decodeBuffer = &_decodeBuffer[ 0 ] )
			{
				bool needRowLength = false;
				byte* buffer = address;
				if( ctx.TexturesSwizzled == true )
					buffer = Unswizzle( format, buffer, unswizzleBuffer, lineWidth, height );

				switch( texture.PixelStorage )
				{
					case TexturePixelStorage.BGR5650:
						ColorOperations.DecodeBGR5650( buffer, decodeBuffer, lineWidth * height );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.ABGR5551:
						ColorOperations.DecodeABGR5551( buffer, decodeBuffer, lineWidth * height );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.ABGR4444:
						ColorOperations.DecodeABGR4444( buffer, decodeBuffer, lineWidth * height );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.ABGR8888:
						// Pass through
						needRowLength = true;
						break;
					case TexturePixelStorage.Indexed4:
						ctx.Clut.Decode4( buffer, decodeBuffer, width, height, lineWidth );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.Indexed8:
						ctx.Clut.Decode8( buffer, decodeBuffer, width, height, lineWidth );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.Indexed16:
						ctx.Clut.Decode16( buffer, decodeBuffer, width, height, lineWidth );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.Indexed32:
						ctx.Clut.Decode32( buffer, decodeBuffer, width, height, lineWidth );
						buffer = decodeBuffer;
						break;
					case TexturePixelStorage.DXT1:
					case TexturePixelStorage.DXT3:
					case TexturePixelStorage.DXT5:
						// Not yet implemented
						Debug.Assert( false );
						break;
				}

				// TODO: eliminate these
				//Gl.glPixelStorei( Gl.GL_UNPACK_ALIGNMENT, 4 );
				//if( needRowLength == true )
				//    Gl.glPixelStorei( Gl.GL_UNPACK_ROW_LENGTH, ( int )lineWidth );
				//else
				//    Gl.glPixelStorei( Gl.GL_UNPACK_ROW_LENGTH, ( int )width );

				Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8,
					( int )width, ( int )height,
					0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE,
					( IntPtr )buffer );

				// Set cookie
				//texture.CookieOriginal = *( ( uint* )address );
				//texture.Cookie = ( uint )textureId;
				//*( ( uint* )address ) = ( uint )textureId;
			}

			return texture;
		}

		private static byte[] _unswizzleBuffer = new byte[ 512 * 512 * 4 ];
		private static byte[] _decodeBuffer = new byte[ 512 * 512 * 4 ];

		// For Indexed4 only
		//static void CopyPixel( const TextureFormat* format, void* dest, const void* source, const uint width )
		//{
		//    memcpy( dest, source, width * format->Size );
		//}
		// All others
		//static void CopyPixelIndexed4( const TextureFormat* format, void* dest, const void* source, const uint width )
		//{
		//    // 2 pixels/byte
		//    memcpy( dest, source, width / 2 );
		//}

		private const uint ChecksumSpacing = 4;
		public static uint CalculateChecksum( byte* address, uint width, uint height, TexturePixelStorage format )
		{
			TextureFormat textureFormat = TextureFormats[ ( int )format ];
			uint size = textureFormat.Size;
			if( size == 0 )
			{
				size = 1;
				width /= 2;
			}

			uint checksum = 0;
			uint stride = width * size;

			// Offset a bit
			address += stride / 2;

			stride *= ChecksumSpacing;
			for( int n = 0; n < ( height / ChecksumSpacing ); n++ )
			{
				checksum += *( ( uint* )address );
				address += stride;
			}

			return checksum;
		}

		private static byte* Unswizzle( TextureFormat format, byte* pin, byte* pout, uint width, uint height )
		{
			uint rowWidth;
			if( format.Size == 0 )
				rowWidth = ( width / 2 );
			else
				rowWidth = width * format.Size;
			uint pitch = ( rowWidth - 16 ) / 4;
			uint bxc = rowWidth / 16;
			uint byc = height / 8;
			uint* src = ( uint* )pin;
			byte* ydest = pout;
			for( int by = 0; by < byc; by++ )
			{
				byte* xdest = ydest;
				for( int bx = 0; bx < bxc; bx++ )
				{
					uint* dest = ( uint* )xdest;
					for( int n = 0; n < 8; n++ )
					{
						*( dest + 0 ) = *( src + 0 );
						*( dest + 1 ) = *( src + 1 );
						*( dest + 2 ) = *( src + 2 );
						*( dest + 3 ) = *( src + 3 );
						src += 4;
						dest += 4 + pitch;
					}
					xdest += 16;
				}
				ydest += rowWidth * 8;
			}
			return pout;
		}
	}
}
