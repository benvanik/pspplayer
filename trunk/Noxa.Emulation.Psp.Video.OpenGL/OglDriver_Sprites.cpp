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

void DrawSpriteList( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

#pragma unmanaged

void DrawSpriteList( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr )
{
	// Sprite lists contain 2*n vertices for n sprites
	// Each sprite has 2 vertices, the first being the top left corner, and the second being
	// the bottom right
	// Since OpenGL doesn't support anything like this, we will emulate it by determining the other
	// two points and drawing a triangle list
	// A good optimization would be to find a way to do this so we can cache the results

	// I don't think these are supported
	assert( ( vertexType & VTNormalMask ) == 0 );
	assert( ( vertexType & VTWeightMask ) == 0 );

	byte* src = ptr;

	int textureType = ( vertexType & VTTextureMask );
	int colorType = ( vertexType & VTColorMask );
	int positionType = ( vertexType & VTPositionMask );
	bool transformed = ( vertexType & VTTransformedMask ) != 0;
	
	// Disable clipping
	glHint( GL_CLIP_VOLUME_CLIPPING_HINT_EXT, GL_FASTEST );

	// Disable depth testing (we place in the order we get it)
	glPushAttrib( GL_ENABLE_BIT );
	glDisable( GL_DEPTH_TEST );

	if( transformed == true )
	{
		glDisable( GL_CULL_FACE );

		glMatrixMode( GL_PROJECTION );
		glPushMatrix();
		glLoadIdentity();
		glOrtho( 0.0f, 480.0f, 272.0f, 0.0f, -1.0f, 1.0f );

		glMatrixMode( GL_MODELVIEW );
		glPushMatrix();
		glLoadIdentity();
	}

	glMatrixMode( GL_MODELVIEW );
	glPushMatrix();
	//glLoadMatrixf( context->ViewMatrix );

	glBegin( GL_QUADS );

	float vpos[ 4 ][ 3 ];
	float vtex[ 4 ][ 2 ];
	byte vclr[ 4 ][ 4 ];
	for( int n = 0; n < vertexCount / 2; n++ )
	{
		// Now on sprite n

		int m = 0;
		do
		{
			switch( textureType )
			{
			case VTTextureFixed8:
				assert( false );
				src += 2;
				break;
			case VTTextureFixed16:
				vtex[ m ][ 0 ] = *( ( short* )src );
				vtex[ m ][ 1 ] = *( ( short* )src + 1 );
				src += 4;
				break;
			case VTTextureFloat:
				vtex[ m ][ 0 ] = *( ( float* )src );
				vtex[ m ][ 1 ] = *( ( float* )src + 1 );
				src += 8;
				break;
			}

			switch( colorType )
			{
			case VTColorBGR5650:
				assert( false );
				src += 2;
				break;
			case VTColorABGR4444:
				assert( false );
				src += 2;
				break;
			case VTColorABGR5551:
				assert( false );
				src += 2;
				break;
			case VTColorABGR8888:
				*( ( int* )vclr[ m ] ) = *( ( int* )src );
				src += 4;
				break;
			}

			switch( positionType )
			{
			case VTPositionFixed8:
				assert( false );
				src += 3;
				break;
			case VTPositionFixed16:
				vpos[ m ][ 0 ] = *( ( short* )src );
				vpos[ m ][ 1 ] = *( ( short* )src + 1 );
				vpos[ m ][ 2 ] = *( ( short* )src + 2 );
				src += 6;
				break;
			case VTPositionFloat:
				vpos[ m ][ 0 ] = *( ( float* )src );
				vpos[ m ][ 1 ] = *( ( float* )src + 1 );
				vpos[ m ][ 2 ] = *( ( float* )src + 2 );
				src += 12;
				break;
			}

			// Must be word (4 byte) aligned
			if( ( vertexSize & 0x3 ) != 0 )
				src += 4 - ( vertexSize & 0x3 );

			m += 2;
		} while( m <= 2 );

		// 0 ---- 1
		// |      |
		// |      |
		// 3 ---- 2
		// Given 0 and 2, populate 1 and 3
		vpos[ 1 ][ 0 ] = vpos[ 2 ][ 0 ];	// x
		vpos[ 1 ][ 1 ] = vpos[ 0 ][ 1 ];	// y
		vpos[ 1 ][ 2 ] = vpos[ 0 ][ 2 ];	// z
		vpos[ 3 ][ 0 ] = vpos[ 0 ][ 0 ];
		vpos[ 3 ][ 1 ] = vpos[ 2 ][ 1 ];
		vpos[ 3 ][ 2 ] = vpos[ 2 ][ 2 ];
		vtex[ 1 ][ 0 ] = vtex[ 2 ][ 0 ];	// s
		vtex[ 1 ][ 1 ] = vtex[ 0 ][ 1 ];	// t
		vtex[ 3 ][ 0 ] = vtex[ 0 ][ 0 ];
		vtex[ 3 ][ 1 ] = vtex[ 2 ][ 1 ];

		*( ( int* )vclr[ 0 ] ) = *( ( int* )vclr[ 2 ] );
		*( ( int* )vclr[ 1 ] ) = *( ( int* )vclr[ 2 ] );
		*( ( int* )vclr[ 3 ] ) = *( ( int* )vclr[ 2 ] );
		
		for( m = 0; m < 4; m++ )
		{
			if( textureType != 0 )
			{
				// The texture coords are not normalized
				//glTexCoord2fv( vtex[ m ] );
				glTexCoord2f(
					vtex[ m ][ 0 ] / context->Textures[ 0 ].Width,
					vtex[ m ][ 1 ] / context->Textures[ 0 ].Height );
			}
			if( colorType != 0 )
				glColor4ubv( vclr[ m ] );
			glVertex3fv( vpos[ m ] );
		}
	}

	glEnd();

	glPopMatrix();

	if( transformed == true )
	{
		glPopMatrix();

		glMatrixMode( GL_PROJECTION );
		glPopMatrix();
	}

	// Re-enable depth testing/etc
	glPopAttrib();

	// Re-enable clipping
	glHint( GL_CLIP_VOLUME_CLIPPING_HINT_EXT, GL_DONT_CARE );
}

#pragma managed
