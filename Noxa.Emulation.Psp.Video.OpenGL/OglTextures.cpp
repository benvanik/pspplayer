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
	{ TPSIndexed16,		2,		CopyPixel,			TFAlpha,		GL_UNSIGNED_SHORT,					},
	{ TPSIndexed32,		4,		CopyPixel,			TFAlpha,		GL_UNSIGNED_INT,					},
	{ TPSDXT1,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT1_EXT,	},
	{ TPSDXT3,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT3_EXT,	},
	{ TPSDXT5,			4,		CopyPixel,			TFAlpha,		GL_COMPRESSED_RGBA_S3TC_DXT5_EXT,	},
};

void Unswizzle( const TextureFormat* format, const byte* in, byte* out, const uint width, const uint height )
{
	int rowWidth = width * format->Size;
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
}

extern void __break();
bool Noxa::Emulation::Psp::Video::GenerateTexture( OglContext* context, OglTexture* texture )
{
	uint textureId;
	glGenTextures( 1, &textureId );
	//glBindTexture( GL_TEXTURE_2D, textureId );
	texture->TextureID = textureId;

	/*static bool stop = true;
	if( stop == true )
	{
		if( texture->Height >= 272 )
			__break();
	}*/

	byte* address;
	if( ( texture->Address & VideoMemoryBase ) != 0 )
		address = context->VideoMemoryPointer + ( texture->Address - VideoMemoryBase );
	else
		address = context->MainMemoryPointer + ( texture->Address - MainMemoryBase );

	TextureFormat* format = ( TextureFormat* )&__formats[ texture->PixelStorage ];

	int size = texture->LineWidth * texture->Height * format->Size;

	byte* buffer = address;
	if( context->TexturesSwizzled == true )
	{
		buffer = ( byte* )malloc( size );
		Unswizzle( format, address, buffer, texture->Width, texture->Height );
	}

	if( format->Format != TPSABGR8888 )
	{
		// Special handling... Nasty!
		if( format->Format == TPSABGR5551 )
		{
			assert( context->TexturesSwizzled == false );
			int oldSize = size;
			short* input = ( short* )buffer;
			size = texture->Width * texture->Height * 4;
			buffer = ( byte* )malloc( size );
			format = ( TextureFormat *)&__formats[ 3 ];

			// Copy 1555 to 8888
			uint* output = ( uint* )buffer;
			for( int n = 0; n < texture->Width * texture->Height; n++ )
			{
				short spixel = *input;
				/*
				  RRRRRGGGGGBBBBBA	<- GL
				  ABBBBBGGGGGRRRRR	<- PSP
				 */
				unsigned short r = ( (spixel & 0xf800) >> 11 ) * 8;
				unsigned short g = ( (spixel & 0x07c0) >> 6 ) * 8;
				unsigned short b = ( (spixel & 0x003E) >> 1 ) * 8;
				unsigned short a = (spixel & 0x0001);

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
		}
	}

	glPixelStorei( GL_UNPACK_ALIGNMENT, format->Size );
	glPixelStorei( GL_UNPACK_ROW_LENGTH, texture->LineWidth );

	/*HANDLE f = CreateFileA( "test.raw", GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_TEMPORARY, NULL );
	int dummy1;
	WriteFile( f, ( void* )buffer, size, ( LPDWORD )&dummy1, NULL );
	CloseHandle( f );*/

	glTexImage2D( GL_TEXTURE_2D, 0, ( format->Flags & TFAlpha ) ? GL_RGBA : GL_RGB,
		texture->Width, texture->Height,
		0,
		( format->Flags & TFAlpha ) ? GL_RGBA : GL_RGB,
		format->GLFormat,
		( void* )buffer );

	if( context->TexturesSwizzled == true )
		SAFEFREE( buffer );

	return true;
}

#pragma managed
