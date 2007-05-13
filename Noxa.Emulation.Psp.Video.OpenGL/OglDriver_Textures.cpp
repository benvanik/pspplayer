// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#include <string>
#include <cmath>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

#include "OglDriver.h"
#include "VideoApi.h"
#include "OglContext.h"
#include "OglTextures.h"
#include "OglExtensions.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

void TextureTransfer( OglContext* context );
void SetTexture( OglContext* context, int stage );

#pragma unmanaged

void TextureTransfer( OglContext* context )
{
	// There are two ways we can do this:
	// a) copy the bits directy to the frame buffer
	// b) copy the bits to a texture and render it as a billboard
	// I don't know which one will be faster - texture uploads are slow, but
	// frame buffer writes are too. At least with the texture method we can
	// better handle weird texture formats as we can use our texture loading
	// code to do things.

	// We only support writes to the current framebuffer pointer
	//if( context->TextureTx.DestinationAddress != context->FrameBufferPointer )
	//	return;

	if( context->TextureTx.SourceAddress == 0 )
		return;

	byte* buffer = context->Memory->Translate( context->TextureTx.SourceAddress );

	glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR );
	glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR );
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP );
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP );

	glPushAttrib( GL_ENABLE_BIT );
	glDisable( GL_TEXTURE_1D );
	glDisable( GL_DEPTH_TEST );
	glDisable( GL_BLEND );
	glDisable( GL_ALPHA_TEST );
	glDisable( GL_FOG );
    glDisable( GL_LIGHTING );
    glDisable( GL_LOGIC_OP );
    glDisable( GL_STENCIL_TEST );
	glDisable( GL_CULL_FACE );
	glDepthMask( GL_FALSE );
	glDepthFunc( GL_ALWAYS );

	glMatrixMode( GL_PROJECTION );
	glPushMatrix();
	glLoadIdentity();
	glOrtho( 0.0f, 480.0f, 0.0f, 272.0f, -1.0f, 1.0f );
	glMatrixMode( GL_MODELVIEW );
	glPushMatrix();
	glLoadIdentity();

	int width = context->TextureTx.Width;
	int height = context->TextureTx.Height;
	int sx = context->TextureTx.SX;
	int sy = context->TextureTx.SY;
	int dx = context->TextureTx.DX;
	int dy = context->TextureTx.DY;
	int lineWidth = context->TextureTx.SourceLineWidth;
	int bpp = ( context->TextureTx.PixelSize == 0 ) ? 2 : 4;

	// NOTE: we only support sources of 0,0 and the width * size must = line width!
	assert( sx == 0 );
	assert( sy == 0 );
	assert( lineWidth == width );

	glPixelStorei( GL_UNPACK_ALIGNMENT, bpp );
	glPixelStorei( GL_UNPACK_ROW_LENGTH, lineWidth );

	glEnable( GL_TEXTURE_2D );
	//uint texId;
	//glGenTextures( 1, &texId );
	//glBindTexture( GL_TEXTURE_2D, texId );
	glBindTexture( GL_TEXTURE_2D, 0 );
	if( bpp == 4 )
	{
		glTexImage2D( GL_TEXTURE_2D, 0, GL_RGBA,
			width, height,
			0, GL_RGBA, GL_UNSIGNED_BYTE,
			buffer );
	}
	else
	{
		// Not supported
		glTexImage2D( GL_TEXTURE_2D, 0, GL_RGB5,
			width, height,
			0, GL_RGB, GL_UNSIGNED_BYTE,
			buffer );
	}

	// 0 ---- 1
	// |      |
	// |      |
	// 3 ---- 2
	// Note I switched around the t coords cause OGL is retarded and has a weird origin
	
	glBegin( GL_QUADS );
		glColor4ub( 255, 255, 255, 255 );
		glTexCoord2f( 0.0f, 1.0f ); glVertex2i( dx,			dy );
        glTexCoord2f( 1.0f, 1.0f ); glVertex2i( dx + width,	dy );
        glTexCoord2f( 1.0f, 0.0f ); glVertex2i( dx + width,	dy + height );
        glTexCoord2f( 0.0f, 0.0f ); glVertex2i( dx,			dy + height );
    glEnd();

	//::glDeleteTextures( 1, &texId );

	glPopMatrix();
	glMatrixMode( GL_PROJECTION );
	glPopMatrix();

	glPopAttrib();
	//glDepthMask( GL_TRUE );
}

void SetTextureModes( OglContext* context, int stage )
{
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, context->TextureWrapS );
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, context->TextureWrapT );
	glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, context->TextureFilterMin );
	glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, context->TextureFilterMag );
	//glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR );
	//glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR );
}

void SetTexture( OglContext* context, int stage )
{
	OglTexture* texture = &context->Textures[ stage ];

	if( texture->Address == 0 )
		return;

	// Check texture cache
	TextureEntry* entry = context->TextureCache->Find( texture->Address );
	if( entry != NULL )
	{
		uint* texturePointer = ( uint* )context->Memory->Translate( texture->Address );
		if( ( entry->Checksum != *texturePointer ) ||
			( entry->Width != texture->Width ) ||
			( entry->Height != texture->Height ) ||
			( entry->LineWidth != texture->LineWidth ) ||
			( entry->PixelStorage != texture->PixelStorage ) )
		{
			// Mismatch - free
			context->TextureCache->Remove( texture->Address );
			GLuint freeIds[] = { entry->TextureID };
			glDeleteTextures( 1, freeIds );
			entry = NULL;
		}
	}

	if( entry != NULL )
	{
		// Texture has been generated, so we just set
		glBindTexture( GL_TEXTURE_2D, entry->TextureID );

		// Must be done for every texture
		SetTextureModes( context, stage );

		return;
	}

	// Ensure valid
	bool textureValid = IsTextureValid( texture );
	if( textureValid == false )
		return;
	
	// Grab and decode texture, then create in OGL
	if( GenerateTexture( context, texture ) == false )
	{
		// Failed? Not much we can do...
	}

	// Must be done for every texture
	SetTextureModes( context, stage );
}

#pragma managed
