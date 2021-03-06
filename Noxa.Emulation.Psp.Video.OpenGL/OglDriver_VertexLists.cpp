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

void DrawBuffers( OglContext* context, int primitiveType, int vertexType, int vertexCount, byte* indexBuffer );
void SetupVertexBuffers( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

#pragma unmanaged

void DrawBuffers( OglContext* context, int primitiveType, int vertexType, int vertexCount, byte* indexBuffer )
{
	bool transformed = ( vertexType & VTTransformedMask ) != 0;

	if( transformed == true )
	{
		glPushAttrib( GL_ENABLE_BIT );
		glDisable( GL_CULL_FACE );

		glMatrixMode( GL_PROJECTION );
		glPushMatrix();
		glLoadIdentity();
		glOrtho( 0.0f, 480.0f, 272.0f, 0.0f, -1.0f, 1.0f );

		glMatrixMode( GL_MODELVIEW );
		glPushMatrix();
		glLoadIdentity();
	}

	if( indexBuffer == NULL )
	{
		glDrawArrays( primitiveType, 0, vertexCount );
	}
	else
	{
		if( ( vertexType & VTIndex8 ) != 0 )
			glDrawElements( primitiveType, vertexCount, GL_UNSIGNED_BYTE, indexBuffer );
		else if( ( vertexType & VTIndex16 ) != 0 )
			glDrawElements( primitiveType, vertexCount, GL_UNSIGNED_SHORT, indexBuffer );
	}

	if( transformed == true )
	{
		glPopMatrix();

		glMatrixMode( GL_PROJECTION );
		glPopMatrix();

		glPopAttrib();
	}
}

uint _colorBuffer[ 1024 * 10 ];
float _textureCoordBuffer[ 1024 * 10 ];

void SetupVertexBuffers( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr )
{
	// DO NOT SUPPORT WEIGHTS/MORPHING

	// PSP comes in this order:
	//float skinWeight[WEIGHTS_PER_VERTEX];
	//float u,v;
	//unsigned int color;
	//float nx,ny,nz;
	//float x,y,z;

	bool transformed = ( vertexType & VTTransformedMask ) != 0;
	bool crazyTextures =
		( context->TextureOffset[ 0 ] != 0.0f ) ||
		( context->TextureOffset[ 1 ] != 0.0f ) ||
		( context->TextureScale[ 0 ] != 1.0f ) ||
		( context->TextureScale[ 1 ] != 1.0f );

	GLenum format = 0;

	int careMasks = VTPositionMask | VTNormalMask | VTTextureMask | VTColorMask;
	int careType = vertexType & careMasks;

	// Always assume V3F set
	if( careType == ( VTTextureFloat | VTNormalFloat | VTPositionFloat ) )
		format = GL_T2F_N3F_V3F;
	else if( careType == ( VTTextureFloat | VTColorABGR8888 | VTPositionFloat ) )
		format = GL_T2F_C4UB_V3F;
	else if( careType == ( VTColorABGR8888 | VTPositionFloat ) )
		format = GL_C4UB_V3F;
	else if( careType == ( VTTextureFloat | VTPositionFloat ) )
		format = GL_T2F_V3F;
	else if( careType == ( VTNormalFloat | VTPositionFloat ) )
		format = GL_N3F_V3F;
	else if( careType == VTPositionFloat )
		format = GL_V3F;

	// We can only support interleaved arrays if we are not transformed
	//if( ( format != 0 ) && ( transformed == false ) && ( crazyTextures == false ) )
	//{
	//	// Something we support - issue an interleaved array
	//	glInterleavedArrays( format, vertexSize, ptr );
	//}
	//else
	{
		// Interleaved unsupported - use separate arrays

		int positionType = ( vertexType & VTPositionMask );
		int normalType = ( vertexType & VTNormalMask );
		int textureType = ( vertexType & VTTextureMask );
		int colorType = ( vertexType & VTColorMask );
		int weightType = ( vertexType & VTWeightMask );

		if( positionType != 0 )
			glEnableClientState( GL_VERTEX_ARRAY );
		else
			glDisableClientState( GL_VERTEX_ARRAY );
		if( normalType != 0 )
			glEnableClientState( GL_NORMAL_ARRAY );
		else
			glDisableClientState( GL_NORMAL_ARRAY );
		if( textureType != 0 )
			glEnableClientState( GL_TEXTURE_COORD_ARRAY );
		else
			glDisableClientState( GL_TEXTURE_COORD_ARRAY );
		if( colorType != 0 )
			glEnableClientState( GL_COLOR_ARRAY );
		else
			glDisableClientState( GL_COLOR_ARRAY );

		byte* src = ptr;

		float uoffset = context->TextureOffset[ 0 ];
		float voffset = context->TextureOffset[ 1 ];
		float uscale = context->TextureScale[ 0 ];
		float vscale = context->TextureScale[ 1 ];

		switch( textureType )
		{
		case VTTextureFixed8:
			assert( false );
			src += 2;
			break;
		case VTTextureFixed16:
			if( transformed == true )
			{
				byte* sp = src;
				float* dp = _textureCoordBuffer;
				int textureWidth = context->Textures[ 0 ].Width;
				int textureHeight = context->Textures[ 0 ].Height;
				for( int n = 0; n < vertexCount; n++ )
				{
					ushort* usp = ( ushort* )sp;
					dp[ 0 ] = ( ( float )usp[ 0 ] / textureWidth );
					dp[ 1 ] = ( ( float )usp[ 1 ] / textureHeight );
					dp += 2;
					sp += vertexSize;
				}
				glTexCoordPointer( 2, GL_FLOAT, 0, _textureCoordBuffer );
			}
			else if( crazyTextures == true )
			{
				byte* sp = src;
				float* dp = _textureCoordBuffer;
				for( int n = 0; n < vertexCount; n++ )
				{
					ushort* usp = ( ushort* )sp;
					dp[ 0 ] = ( ( float )usp[ 0 ] * uscale ) + uoffset;
					dp[ 1 ] = ( ( float )usp[ 1 ] * vscale ) + voffset;
					dp += 2;
					sp += vertexSize;
				}
				glTexCoordPointer( 2, GL_FLOAT, 0, _textureCoordBuffer );
			}
			else
				glTexCoordPointer( 2, GL_SHORT, vertexSize, src );
			src += 4;
			break;
		case VTTextureFloat:
			if( transformed == true )
			{
				byte* sp = src;
				float* dp = _textureCoordBuffer;
				int textureWidth = context->Textures[ 0 ].Width;
				int textureHeight = context->Textures[ 0 ].Height;
				for( int n = 0; n < vertexCount; n++ )
				{
					float* usp = ( float* )sp;
					dp[ 0 ] = ( usp[ 0 ] / textureWidth );
					dp[ 1 ] = ( usp[ 1 ] / textureHeight );
					dp += 2;
					sp += vertexSize;
				}
				glTexCoordPointer( 2, GL_FLOAT, 0, _textureCoordBuffer );
			}
			else if( crazyTextures == true )
			{
				byte* sp = src;
				float* dp = _textureCoordBuffer;
				for( int n = 0; n < vertexCount; n++ )
				{
					float* usp = ( float* )sp;
					dp[ 0 ] = ( usp[ 0 ] * uscale ) + uoffset;
					dp[ 1 ] = ( usp[ 1 ] * vscale ) + voffset;
					dp += 2;
					sp += vertexSize;
				}
				glTexCoordPointer( 2, GL_FLOAT, 0, _textureCoordBuffer );
			}
			else
				glTexCoordPointer( 2, GL_FLOAT, vertexSize, src );
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
			{
				byte* sp = src;
				uint* dp = _colorBuffer;
				for( int n = 0; n < vertexCount; n++ )
				{
					uint entry = ( uint )*( ( ushort* )sp );
					*dp =	( ( entry & 0xF ) * 16 ) |
							( ( ( ( entry >> 4 ) & 0xF ) * 16 ) << 8 ) |
							( ( ( ( entry >> 8 ) & 0xF ) * 16 ) << 16 ) |
							( ( ( ( entry >> 12 ) & 0xF ) * 16 ) << 24 );
					sp += vertexSize;
					dp += 1;
				}
				glColorPointer( 4, GL_UNSIGNED_BYTE, 0, _colorBuffer );
				//glColor4f( 0.0f, 0.0f, 1.0f, 1.0f );
			}
			src += 2;
			break;
		case VTColorABGR5551:
			{
				byte* sp = src;
				uint* dp = _colorBuffer;
				for( int n = 0; n < vertexCount; n++ )
				{
					uint entry = ( uint )*( ( ushort* )sp );
					*dp =	( ( entry & 0x1 ) * 256 ) |
							( ( ( ( entry >> 1 ) & 0x1F ) * 8 ) << 8 ) |
							( ( ( ( entry >> 6 ) & 0x1F ) * 8 ) << 16 ) |
							( ( ( ( entry >> 11 ) & 0x1F ) * 8 ) << 24 );
					sp += vertexSize;
					dp += 1;
				}
				glColorPointer( 4, GL_UNSIGNED_BYTE, 0, _colorBuffer );
				//glColor4f( 0.0f, 1.0f, 0.0f, 1.0f );
			}
			src += 2;
			break;
		case VTColorABGR8888:
			glColorPointer( 4, GL_UNSIGNED_BYTE, vertexSize, src );
			src += 4;
			break;
		case 0:
			glColor4fv( context->AmbientMaterial );
			//if( context->AmbientMaterial[ 0 ] == context->AmbientMaterial[ 1 ] == context->AmbientMaterial[ 2 ] == 0.0f )
			//	glColor4f( 1.0f, 1.0f, 1.0f, context->AmbientMaterial[ 3 ] );
			//else
			//	glColor4fv( context->AmbientMaterial );
			//glColor4f( 1.0f, 0.0f, 0.0f, 1.0f );
			break;
		}

		if( ( context->WireframeEnabled == true ) &&
			( colorType == 0 ) )
			glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );

		switch( normalType )
		{
		case VTNormalFixed8:
			glNormalPointer( GL_BYTE, vertexSize, src );
			src += 3;
			break;
		case VTNormalFixed16:
			glNormalPointer( GL_SHORT, vertexSize, src );
			src += 6;
			break;
		case VTNormalFloat:
			glNormalPointer( GL_FLOAT, vertexSize, src );
			src += 12;
			break;
		}

		switch( positionType )
		{
		case VTPositionFixed8:
			glVertexPointer( 3, GL_BYTE, vertexSize, src );	// THIS MAY NOT WORK!!!
			src += 3;
			break;
		case VTPositionFixed16:
			glVertexPointer( 3, GL_SHORT, vertexSize, src );
			src += 6;
			break;
		case VTPositionFloat:
			glVertexPointer( 3, GL_FLOAT, vertexSize, src );
			src += 12;
			break;
		}
	}
}

#pragma managed
