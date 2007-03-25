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
}

void SetupVertexBuffers( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr )
{
	bool transformed = ( vertexType & VTTransformedMask ) != 0;

	// DO NOT SUPPORT WEIGHTS OR TRANSFORMED

	// PSP comes in this order:
	//float skinWeight[WEIGHTS_PER_VERTEX];
	//float u,v;
	//unsigned int color;
	//float nx,ny,nz;
	//float x,y,z;

	// Must be word (4 byte) aligned - if it's not, there will be padding we need to skip
	if( ( vertexSize & 0x3 ) != 0 )
		vertexSize += 4 - ( vertexSize & 0x3 );

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

	if( format != 0 )
	{
		// Something we support - issue an interleaved array
		glInterleavedArrays( format, vertexSize, ptr );
	}
	else
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

		switch( textureType )
		{
		case VTTextureFixed8:
			assert( false );
			src += 2;
			break;
		case VTTextureFixed16:
			glTexCoordPointer( 2, GL_SHORT, vertexSize, src );
			src += 4;
			break;
		case VTTextureFloat:
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
			assert( false );
			src += 2;
			break;
		case VTColorABGR5551:
			assert( false );
			src += 2;
			break;
		case VTColorABGR8888:
			glColorPointer( 4, GL_UNSIGNED_BYTE, vertexSize, src );
			src += 4;
			break;
		}

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
			assert( false );
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
