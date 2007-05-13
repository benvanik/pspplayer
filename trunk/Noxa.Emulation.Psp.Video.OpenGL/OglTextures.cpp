// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
// A lot of the following code has been either taken from or modified from the PSPGL code at ps2dev.org
// Specifically, most comes from the following (pspgl_texobj.c):
// http://svn.ps2dev.org/filedetails.php?repname=psp&path=%2Ftrunk%2Fpspgl%2Fpspgl_texobj.c&sc=1
// ------------------------------------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#include <cmath>
#include <string>
#include <stdlib.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

#include "OglDriver.h"
#include "OglContext.h"
#include "OglTextures.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
//using namespace Noxa::Emulation::Psp::Video::Native;

#pragma unmanaged

bool Noxa::Emulation::Psp::Video::IsTextureValid( OglTexture* texture )
{
	if( ( texture->Address == 0x0 ) ||
		( texture->LineWidth == 0 ) ||
		( texture->Width == 0 ) ||
		( texture->Height == 0 ) )
		return false;

#if 1
	// This is a special case - something to do with the framebuffer being set as the texture?
	if( ( texture->Address == 0x04000000 ) &&
		( texture->LineWidth == 0x4 ) &&
		( texture->Width == 0x2 ) &&
		( texture->Height == 0x2 ) )
		return false;
#endif

	return true;
}

__inline uint lg2( uint x )
{
	switch( x )
	{
	case 1:		return 0;
	case 2:		return 1;
	case 4:		return 2;
	case 8:		return 3;
	case 16:	return 4;
	case 32:	return 5;
	case 64:	return 6;
	case 128:	return 7;
	case 256:	return 8;
	case 512:	return 9;
	case 1024:	return 10;
	case 2048:	return 11;
	case 4096:	return 12;
	case 8192:	return 13;
	case 16384:	return 14;
	case 32768:	return 15;
	case 65536:	return 16;
	}

	uint ret = -1;
	do
	{
		ret++;
		x >>= 1;
	} while( x != 0 );

	return ret;
}

static void CopyPixel( const TextureFormat* format, void* dest, const void* source, const uint width )
{
	memcpy( dest, source, width * format->Size );
}

static void CopyPixelIndexed4( const TextureFormat* format, void* dest, const void* source, const uint width )
{
	// 2 pixels/byte
	memcpy( dest, source, width / 2 );
}

const TextureFormat __formats[] = {
	// Format			Size	Copier				Flags			GL format
	{ TPSBGR5650,		2,		CopyPixel,			0,				GL_UNSIGNED_SHORT_5_6_5_REV,		},
	{ TPSABGR5551,		2,		CopyPixel,			TFAlpha,		GL_UNSIGNED_SHORT_1_5_5_5_REV,		},
	{ TPSABGR4444,		2,		CopyPixel,			TFAlpha,		GL_UNSIGNED_SHORT_4_4_4_4_REV,		},
	{ TPSABGR8888,		4,		CopyPixel,			TFAlpha,		GL_UNSIGNED_BYTE,					},
	{ TPSIndexed4,		0,		CopyPixelIndexed4,	TFAlpha,		GL_UNSIGNED_BYTE,					},
	{ TPSIndexed8,		1,		CopyPixel,			TFAlpha,		GL_UNSIGNED_BYTE,					},
	{ TPSIndexed16,		2,		CopyPixel,			TFAlpha,		GL_UNSIGNED_BYTE,					},
	{ TPSIndexed32,		4,		CopyPixel,			TFAlpha,		GL_UNSIGNED_BYTE,					},
	{ TPSDXT1,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT1_EXT,	},
	{ TPSDXT3,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT3_EXT,	},
	{ TPSDXT5,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT5_EXT,	},
};

byte* Unswizzle( const TextureFormat* format, const byte* in, byte* out, const uint width, const uint height )
{
	int rowWidth;
	if( format->Size == 0 )
		rowWidth = ( width / 2 );
	else
		rowWidth = width * format->Size;
	int pitch = ( rowWidth - 16 ) / 4;
	int bxc = rowWidth / 16;
	int byc = height / 8;

	uint* src = ( uint* )in;
	const byte* ydest = out;
	for( int by = 0; by < byc; by++ )
	{
		const byte* xdest = ydest;
		for( int bx = 0; bx < bxc; bx++ )
		{
			uint* dest = ( uint* )xdest;
			for( int n = 0; n < 8; n++ )
			{
				*( dest++ ) = *( src++ );
				*( dest++ ) = *( src++ );
				*( dest++ ) = *( src++ );
				*( dest++ ) = *( src++ );
				dest += pitch;
			}
			xdest += 16;
		}
		ydest += rowWidth * 8;
	}
	return out;
}

byte* Widen5650( const byte* in, byte* out, const uint width, const uint height )
{
	return out;
}

byte* Widen5551( const byte* in, byte* out, const uint width, const uint height )
{
	// Copy 1555 to 8888
	short* input = ( short* )in;
	uint* output = ( uint* )out;
	for( uint n = 0; n < width * height; n++ )
	{
		short spixel = *input;
		/*
		  RRRRRGGGGGBBBBBA	<- GL
		  ABBBBBGGGGGRRRRR	<- PSP
		 */
		unsigned short r = ( ( spixel & 0xF800 ) >> 11 ) * 8;
		unsigned short g = ( ( spixel & 0x07C0 ) >> 6 ) * 8;
		unsigned short b = ( ( spixel & 0x003E ) >> 1 ) * 8;
		unsigned short a = ( spixel & 0x0001 );

		uint dpixel = a ? 0x000000FF : 0x0;
		dpixel |= ( r << 24 ) | ( g << 16 ) | ( b << 8 );
		/*dpixel |=
			( 8 * ( spixel & 0x1F ) ) |
			( 8 * ( spixel >> 5 ) & 0x1F ) |
			( 8 * ( spixel >> 10 ) & 0x1F );*/
		*output = dpixel;

		input++;
		output++;
	}
	return out;
}

byte* Widen4444( const byte* in, byte* out, const uint width, const uint height )
{
	return out;
}

__inline uint ClutLookup( const OglContext* context, uint index )
{
	int finalIndex = ( ( context->ClutStart + index ) >> context->ClutShift ) & context->ClutMask;

	if( context->ClutFormat == 0x3 )
	{
		// 32-bit ABGR 8888
		uint entry = ( ( uint* )context->ClutTable )[ finalIndex ];
		// Supposedly the intrinsic does BSWAP in release build
		//return _byteswap_ulong( entry );
		return entry;
	}
	else
	{
		// This may not need to swap!
		ushort entry = ( ( ushort* )context->ClutTable )[ finalIndex ];
		switch( context->ClutFormat )
		{
		case 0x0:
			// 16-bit BGR 5650
			return
				( ( entry & 0x1F ) * 8 ) |
				( ( ( ( entry >> 5 ) & 0x3F ) * 4 ) << 8 ) |
				( ( ( ( entry >> 11 ) & 0x1F ) * 8 ) << 16 ) |
				( 0xFF << 24 );
			break;
		case 0x1:
			// 16-bit ABGR 5551
			return
				( ( entry & 0x1F ) * 8 ) |
				( ( ( ( entry >> 5 ) & 0x1F ) * 8 ) << 8 ) |
				( ( ( ( entry >> 10 ) & 0x1F ) * 8 ) << 16 ) |
				( ( ( entry >> 15 ) * 255 ) << 24 );
			break;
		case 0x2:
			// 16-bit ABGR 4444
			return
				( ( entry & 0xF ) * 16 ) |
				( ( ( ( entry >> 4 ) & 0xF ) * 16 ) << 8 ) |
				( ( ( ( entry >> 8 ) & 0xF ) * 16 ) << 16 ) |
				( ( ( ( entry >> 12 ) & 0xF ) * 16 ) << 24 );
		}
	}

	return 0;
}

byte* Decode4( const OglContext* context, const byte* in, byte* out, const uint width, const uint height, const uint lineWidth )
{
	// Tricky, as each byte contains 2 indices (4 bits each)
	byte* input = ( byte* )in;
	uint* output = ( uint* )out;
	int diff = ( width - lineWidth ) / 2;
	assert( diff >= 0 );
	for( uint y = 0; y < height; y++ )
	{
		for( uint x = 0; x < width; x++ )
		{
			byte index = *input;
			if( x & 0x1 )
			{
				index >>= 4;
				input++; // only advance after reading both
			}
			else
				index &= 0x0F;
			output[ x ] = ClutLookup( context, index );
		}
		input -= width / 2;
		input += lineWidth / 2;
		output += width;
	}
	return out;
}

byte* Decode8( const OglContext* context, const byte* in, byte* out, const uint width, const uint height, const uint lineWidth )
{
	byte* input = ( byte* )in;
	uint* output = ( uint* )out;
	for( uint y = 0; y < height; y++ )
	{
		for( uint x = 0; x < width; x++ )
		{
			byte index = input[ x ];
			output[ x ] = ClutLookup( context, index );
		}
		input += lineWidth;
		output += width;
	}
	return out;
}

byte* Decode16( const OglContext* context, const byte* in, byte* out, const uint width, const uint height, const uint lineWidth )
{
	ushort* input = ( ushort* )in;
	uint* output = ( uint* )out;
	for( uint y = 0; y < height; y++ )
	{
		for( uint x = 0; x < width; x++ )
		{
			ushort index = input[ x ];
			output[ x ] = ClutLookup( context, index );
		}
		input += lineWidth;
		output += width;
	}
	return out;
}

byte* Decode32( const OglContext* context, const byte* in, byte* out, const uint width, const uint height, const uint lineWidth )
{
	uint* input = ( uint* )in;
	uint* output = ( uint* )out;
	for( uint y = 0; y < height; y++ )
	{
		for( uint x = 0; x < width; x++ )
		{
			uint index = input[ x ];
			output[ x ] = ClutLookup( context, index );
		}
		input += lineWidth;
		output += width;
	}
	return out;
}

// TODO: Free texture buffers
byte* _unswizzleBuffer = NULL;
byte* _decodeBuffer = NULL;

extern void __break();
bool Noxa::Emulation::Psp::Video::GenerateTexture( OglContext* context, OglTexture* texture )
{
	uint textureId;
	glGenTextures( 1, &textureId );
	glBindTexture( GL_TEXTURE_2D, textureId );

	TextureEntry* entry = new TextureEntry();
	entry->Address = texture->Address;
	entry->Width = texture->Width;
	entry->Height = texture->Height;
	entry->LineWidth = texture->LineWidth;
	entry->PixelStorage = texture->PixelStorage;
	entry->TextureID = textureId;
	context->TextureCache->Add( texture->Address, entry );

	if( _unswizzleBuffer == NULL )
		_unswizzleBuffer = ( byte* )malloc( 512 * 512 * 4 );
	if( _decodeBuffer == NULL )
		_decodeBuffer = ( byte* )malloc( 512 * 512 * 4 );

	byte* address = context->Memory->Translate( texture->Address );
	TextureFormat* format = ( TextureFormat* )&__formats[ texture->PixelStorage ];
	int size = texture->LineWidth * texture->Height * format->Size;

	entry->Checksum = *( ( uint* )address );

	byte* buffer = address;
	if( context->TexturesSwizzled == true )
	{
		// Not sure if this is right, but it works for Puzzle Bobble
		if( format->Size == 0 )
			buffer = Unswizzle( format, buffer, _unswizzleBuffer, texture->LineWidth, texture->Height );
		else
			buffer = Unswizzle( format, buffer, _unswizzleBuffer, texture->Width, texture->Height );
	}

	// buffer now contains an unswizzled texture - may need to un-CLUT it, or convert colors

	int width = texture->Width;
	bool needRowLength = false;

	switch( format->Format )
	{
	case TPSBGR5650:
		//buffer = Widen5650( buffer, _decodeBuffer, texture->Width, texture->Height );
		//format = ( TextureFormat* )&__formats[ 3 ];
		assert( false );
		break;
	case TPSABGR5551:
		buffer = Widen5551( buffer, _decodeBuffer, texture->Width, texture->Height );
		format = ( TextureFormat* )&__formats[ 3 ];
		break;
	case TPSABGR4444:
		//buffer = Widen4444( buffer, _decodeBuffer, texture->Width, texture->Height );
		//format = ( TextureFormat* )&__formats[ 3 ];
		assert( false );
		break;
	case TPSABGR8888:
		// Pass through
		needRowLength = true;
		break;
	case TPSIndexed4:
		buffer = Decode4( context, buffer, _decodeBuffer, texture->Width, texture->Height, texture->LineWidth );
		format = ( TextureFormat* )&__formats[ 3 ];
		break;
	case TPSIndexed8:
		buffer = Decode8( context, buffer, _decodeBuffer, texture->Width, texture->Height, texture->LineWidth );
		format = ( TextureFormat* )&__formats[ 3 ];
		break;
	case TPSIndexed16:
		buffer = Decode16( context, buffer, _decodeBuffer, texture->Width, texture->Height, texture->LineWidth );
		format = ( TextureFormat* )&__formats[ 3 ];
		break;
	case TPSIndexed32:
		buffer = Decode32( context, buffer, _decodeBuffer, texture->Width, texture->Height, texture->LineWidth );
		format = ( TextureFormat* )&__formats[ 3 ];
		break;
	case TPSDXT1:
	case TPSDXT3:
	case TPSDXT5:
		// Not supported
		assert( false );
		break;
	}

	glPixelStorei( GL_UNPACK_ALIGNMENT, format->Size );
	if( needRowLength == true )
		glPixelStorei( GL_UNPACK_ROW_LENGTH, texture->LineWidth );

#ifdef _DEBUG
	static bool write = false;
	if( write == true )
	{
		size = texture->Width * texture->Height * format->Size;
		HANDLE f = CreateFileA( "test.raw", GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_TEMPORARY, NULL );
		int dummy1;
		WriteFile( f, ( void* )buffer, size, ( LPDWORD )&dummy1, NULL );
		CloseHandle( f );
	}
#endif

	glTexImage2D( GL_TEXTURE_2D, 0, ( format->Flags & TFAlpha ) ? GL_RGBA8 : GL_RGB8,
		width, texture->Height,
		0,
		( format->Flags & TFAlpha ) ? GL_RGBA : GL_RGB,
		format->GLFormat,
		( void* )buffer );

	return true;
}

#pragma managed
