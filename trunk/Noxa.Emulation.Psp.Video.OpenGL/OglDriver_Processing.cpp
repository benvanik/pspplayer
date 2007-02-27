// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#include <cmath>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#pragma managed

#include <string>
#include "OglDriver.h"
#include "VideoApi.h"
#include "OglContext.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

#define COLORSWIZZLE( bgra ) bgra

__inline void WidenMatrix( float src[ 16 ], float dest[ 16 ] );
int DetermineVertexSize( int vertexType );
void DrawVertexBuffer( OglContext* context, int primitiveType, int primitiveCount, int vertexType, int vertexCount, int vertexSize, byte* ptr );

__inline bool IsTextureValid( OglTexture* texture );
void SetTexture( OglContext* context, int stage );

enum VertexType
{
	VTNone = 0x0,

	VTTextureMask = 0x3,
	VTTextureFixed8 = 0x1,
	VTTextureFixed16 = 0x2,
	VTTextureFloat = 0x3,

	VTColorMask = 0x7 << 2,
	VTColorBGR5650 = 0x4 << 2,
	VTColorABGR5551 = 0x5 << 2,
	VTColorABGR4444 = 0x6 << 2,
	VTColorABGR8888 = 0x7 << 2,

	VTNormalMask = 0x3 << 5,
	VTNormalFixed8 = 0x1 << 5,
	VTNormalFixed16 = 0x2 << 5,
	VTNormalFloat = 0x3 << 5,

	VTPositionMask = 0x3 << 7,
	VTPositionFixed8 = 0x1 << 7,
	VTPositionFixed16 = 0x2 << 7,
	VTPositionFloat = 0x3 << 7,

	VTWeightMask = 0x3 << 9,
	VTWeightFixed8 = 0x1 << 9,
	VTWeightFixed16 = 0x2 << 9,
	VTWeightFloat = 0x3 << 9,

	VTIndexMask = 0x2 << 11,
	VTIndex8 = 0x1 << 11,
	VTIndex16 = 0x2 << 11,
	//100011100
	//0-1: Texture Format (2 values ST/UV)
	//    00: Not present in vertex
	//    01: 8-bit fixed
	//    10: 16-bit fixed
	//    11: 32-bit floats
	//2-4: Color Format (1 value)
	//    000: Not present in vertex
	//    100: 16-bit BGR-5650
	//    101: 16-bit ABGR-5551
	//    110: 16-bit ABGR-4444
	//    111: 32-bit ABGR-8888
	//5-6: Normal Format (3 values XYZ)
	//    00: Not present in vertex
	//    01: 8-bit fixed
	//    10: 16-bit fixed
	//    11: 32-bit floats
	//7-8: Position Format (3 values XYZ)
	//    00: Not present in vertex
	//    01: 8-bit fixed
	//    10: 16-bit fixed
	//    11: 32-bit floats
	//9-10: Weight Format
	//    00: Not present in vertex
	//    01: 8-bit fixed
	//    10: 16-bit fixed
	//    11: 32-bit floats
	//11-12: Index Format
	//    00: Not using indices
	//    01: 8-bit
	//    10: 16-bit
	//14-16: Number of weights (Skinning)
	//    000-111: 1-8 weights
	//18-20: Number of vertices (Morphing)
	//    000-111: 1-8 vertices
	//23: Bypass Transform Pipeline
	//    0: Transformed Coordinates
	//    1: Raw Coordinates
};

enum TexturePixelStorage
{
	TPSBGR5650 = 0,
	TPSABGR5551 = 1,
	TPSABGR4444 = 2,
	TPSABGR8888 = 3,
	TPSIndexed4 = 4,
	TPSIndexed8 = 5,
	TPSIndexed16 = 6,
	TPSIndexed32 = 7,
	TPSDXT1 = 8,
	TPSDXT3 = 9,
	TPSDXT5 = 10,
};

#pragma unmanaged

void ProcessList( OglContext* context, VideoDisplayList* list )
{
	int temp;
	float matrixTemp[ 16 ];
	float color3[ 3 ] = { 0.0f, 0.0f, 0.0f };
	float color4[ 4 ] = { 0.0f, 0.0f, 0.0f };

	bool verticesTransformed;
	int skinningWeightCount;
	int morphingVertexCount;
	int vertexType;
	int vertexBufferAddress;
	int indexBufferAddress;
	int vertexCount;
	bool areSprites;
	int primitiveType;
	int primitiveCount;

	glDisable( GL_LIGHTING );
	//glDisable( GL_CULL_FACE );

	for( int n = 0; n < list->PacketCount; n++ )
	{
		Native::VideoPacket packet = list->Packets[ n ];
		int argi = packet.Argument;
		int argx = argi << 8;
		float argf = *reinterpret_cast<float*>( &argx );

		switch( packet.Command )
		{
		case NOP:
			break;
		case CLEAR:
			if( ( argi & 0x1 ) == 0x1 )
			{
				temp = 0;
				if( ( argi & 0x100 ) != 0 )
					temp |= GL_COLOR_BUFFER_BIT; // target
				if( ( argi & 0x200 ) != 0 )
					temp |= GL_ACCUM_BUFFER_BIT | GL_STENCIL_BUFFER_BIT; // stencil/alpha
				if( ( argi & 0x400 ) != 0 )
					temp |= GL_DEPTH_BUFFER_BIT; // zbuffer
				glClear( temp );
			}
			break;
			
		case SHADE:
			if( argi == 0 )
				glShadeModel( GL_FLAT );
			else
				glShadeModel( GL_SMOOTH );
			break;
		case BCE:
			// cull enable
			if( argi == 1 )
				glEnable( GL_CULL_FACE );
			else
				glDisable( GL_CULL_FACE );
			break;
		case FFACE:
			// 0 = clockwise visible, 1 = cclockwise visible
			// or maybe the inverse?
			glFrontFace( ( argi == 1 ) ? GL_CW : GL_CCW );
			break;
		case AAE:
			// antialiasing enable
			break;

		case FGE:
			// fog enable
			if( argi == 1 )
			{
				glEnable( GL_FOG );
				//glFogi( GL_FOG_MODE, GL_EXP2 );
				//glFogf( GL_FOG_DENSITY, 0.35f );
				//glHint( GL_FOG_HINT, GL_DONT_CARE );
			}
			else
				glDisable( GL_FOG );
			break;
		case FCOL:
			// fog color
			//CONVERTCOLOR( argi, color3 );
			glFogfv( GL_FOG_COLOR, color3 );
			break;
		case FDIST:
			// fog start (float)
			glFogf( GL_FOG_START, argf );
			break;
		case FFAR:
			// fog end (float)
			glFogf( GL_FOG_END, argf );
			break;

		case ALA:
			// ambient alpha
			break;
		case ALC:
			// ambient color
			break;
		case AMA:
			// ambient material alpha
			break;
		case AMC:
			// ambient material color
			break;

		case FBP:
			temp = argi;
			break;
		case FBW:
			context->FrameBufferPointer = temp | ( ( ( uint )argi & 0x00FF0000 ) << 8 );
			context->FrameBufferWidth = ( argi & 0x0000FFFF );
			break;

		case VTYPE:
			verticesTransformed = ( argi >> 23 ) == 0;
			skinningWeightCount = ( argi >> 14 ) & 0x3;
			morphingVertexCount = ( argi >> 18 ) & 0x3;
			vertexType = argi & 0x1FFF;
			break;
		case VADDR:
			vertexBufferAddress = list->Base | argi;
			break;
		case IADDR:
			indexBufferAddress = list->Base | argi;
			break;
		case PRIM:
			vertexCount = argi & 0xFFFF;
			areSprites = false;
			primitiveCount = 0;
			switch( ( argi >> 16 ) & 0x7 )
			{
				case 0x0: // Points
					primitiveType = GL_POINTS;
					primitiveCount = vertexCount;
					break;
				case 0x1: // Lines
					primitiveType = GL_LINES;
					primitiveCount = vertexCount / 2;
					break;
				case 0x2: // Line strips
					primitiveType = GL_LINE_STRIP;
					primitiveCount = vertexCount - 1;
					break;
				case 0x3: // Triangles
					primitiveType = GL_TRIANGLES;
					primitiveCount = vertexCount / 3;
					break;
				case 0x4: // Triangle strips
					primitiveType = GL_TRIANGLE_STRIP;
					primitiveCount = vertexCount - 2;
					break;
				case 0x5: // Triangle fans
					primitiveType = GL_TRIANGLE_FAN;
					primitiveCount = vertexCount - 2;
					break;
				case 0x6: // Sprites (2D rectangles)
					areSprites = true;
					break;
			}
			if( areSprites == false )
			{
				// Tris/etc
				int vertexSize = DetermineVertexSize( vertexType );
				int offset = vertexBufferAddress - MainMemoryBase;
				assert( offset > 0 );
				byte* ptr = context->MemoryPointer + offset;
				SetTexture( context, 0 );
				DrawVertexBuffer( context, primitiveType, primitiveCount, vertexType, vertexCount, vertexSize, ptr );
			}
			else
			{
				// Sprite list
			}
			break;

		case TMODE:
			context->TexturesSwizzled = ( argi & 0x1 ) == 1 ? true : false;
			context->MipMapLevel = ( argi >> 16 ) & 0x4;
			break;
		case TPSM:
			// TexturePixelStorage (TPS*)
			context->TextureStorageMode = argi;
			break;
		case TEC:
			{
				// TODO: texture environment color
				//int color = ( packet.Argument & 0x0000FF00 ) | unchecked( ( int )0xFF000000 );
				//color |= ( ( packet.Argument & 0x00FF0000 ) >> 16 );
				//color |= ( ( packet.Argument & 0x000000FF ) << 16 );
				//_device.Material.Diffuse = Color.FromArgb( color );
			}
			break;
		case TFLT:
			{
				// bits 0-2 have minifying filter
				// bits 8-10 have magnifying filter
				switch( argi & 0x7 )
				{
				case 0x0: // Nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST );
					break;
				case 0x1: // Linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR );
					break;
				case 0x4: // Nearest; mipmap nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST );
					break;
				case 0x5: // Linear; mipmap nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST );
					break;
				case 0x6: // Nearest; mipmap linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_LINEAR );
					break;
				case 0x7: // Linear; mipmap linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR );
					break;
				}
				switch( ( argi >> 8 ) & 0x7 )
				{
				case 0x0: // Nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST );
					break;
				case 0x1: // Linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR );
					break;
				case 0x4: // Nearest; mipmap nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST_MIPMAP_NEAREST );
					break;
				case 0x5: // Linear; mipmap nearest
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR_MIPMAP_NEAREST );
					break;
				case 0x6: // Nearest; mipmap linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST_MIPMAP_LINEAR );
					break;
				case 0x7: // Linear; mipmap linear
					glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR_MIPMAP_LINEAR );
					break;
				}
			}
			break;
		case TFUNC:
			// texture function
			break;
		case TFLUSH:
			// texturesvalid = false
			break;
		case USCALE:
			// (float) should be 1
			break;
		case VSCALE:
			// (float) should be 1
			break;
		case UOFFSET:
			// (float) should be 0
			break;
		case VOFFSET:
			// (float) should be 0
			break;
		case TBP0:
			context->Textures[ 0 ].Address = argi;
			break;
		case TBW0:
			context->Textures[ 0 ].Address |= ( argi << 8 ) & 0xFF000000;
			context->Textures[ 0 ].LineWidth = argi & 0x0000FFFF;
			break;
		case TSIZE0:
			context->Textures[ 0 ].Width = power( 2, argi & 0x000000FF );
			context->Textures[ 0 ].Height = power( 2, ( argi >> 8 ) & 0x000000FF );
			break;

		case PMS:
		case VMS:
		case WMS:
		case TMS:
			temp = 0;
			break;
		case PROJ: // 4x4
			matrixTemp[ temp++ ] = argf;
			if( temp == 16 )
			{
				temp = 0;
				//context->ProjectionMatrix = matrixTemp;
				glMatrixMode( GL_PROJECTION );
				glLoadMatrixf( matrixTemp );
			}
			break;
		case VIEW: // 3x4
			matrixTemp[ temp++ ] = argf;
			if( temp == 12 )
			{
				WidenMatrix( matrixTemp, context->ViewMatrix );
				temp = 0;
				glMatrixMode( GL_MODELVIEW );
				glLoadMatrixf( context->ViewMatrix );
			}
			break;
		case WORLD: // 3x4
			matrixTemp[ temp++ ] = argf;
			assert( argf == argf );
			if( temp == 12 )
			{
				WidenMatrix( matrixTemp, context->WorldMatrix );
				temp = 0;
				//context->WorldMatrix = matrixTemp;
				glMatrixMode( GL_MODELVIEW );
				glLoadMatrixf( context->ViewMatrix );
				glMultMatrixf( context->WorldMatrix );
			}
			break;
		case TMATRIX: // 3x4
			matrixTemp[ temp++ ] = argf;
			if( temp == 12 )
			{
				temp = 0;
				//context->TextureMatrix = matrixTemp;
				//glMatrixMode( GL_TEXTURE );
				//glLoadMatrixf( matrixTemp );
			}
			break;

		case BASE:
		case FINISH:
		case END:
		case Unknown0x11:
		case JUMP:
			// Handled by display list processor
			break;

		default:
			// Unknown command
			break;
		}
	}
}

// TODO: a faster widen matrix 3x4->4x4
__inline void WidenMatrix( float src[ 16 ], float dest[ 16 ] )
{
	dest[0] = src[0];
	dest[1] = src[1];
	dest[2] = src[2];
	dest[3] = 0.0f;
	dest[4] = src[3];
	dest[5] = src[4];
	dest[6] = src[5];
	dest[7] = 0.0f;
	dest[8] = src[6];
	dest[9] = src[7];
	dest[10] = src[8];
	dest[11] = 0.0f;
	dest[12] = src[9];
	dest[13] = src[10];
	dest[14] = src[11];
	dest[15] = 1.0f;
}

int DetermineVertexSize( int vertexType )
{
	int size = 0;

	int positionMask = vertexType & VTPositionMask;
	if( positionMask == VTPositionFixed8 )
		size += 1 + 1 + 1;
	else if( positionMask == VTPositionFixed16 )
		//size += 2 + 2 + 2;
		size += 4 + 4 + 4;
	else if( positionMask == VTPositionFloat )
		size += 4 + 4 + 4;

	int normalMask = vertexType & VTNormalMask;
	if( normalMask == VTNormalFixed8 )
		size += 1 + 1 + 1;
	else if( normalMask == VTNormalFixed16 )
		size += 2 + 4 + 4;
	else if( normalMask == VTNormalFloat )
		size += 4 + 4 + 4;

	int textureType = vertexType & VTTextureMask;
	if( textureType == VTTextureFixed8 )
		size += 1 + 1;
	else if( textureType == VTTextureFixed16 )
		size += 2 + 2;
	else if( textureType == VTTextureFloat )
		size += 4 + 4;

	// TODO: figure out weight format
	int weightType = vertexType & VTWeightMask;
	if( weightType == VTWeightFixed8 )
		size += 1;
	else if( weightType == VTWeightFixed16 )
		size += 2;
	else if( weightType == VTWeightFloat )
		size += 4;

	int colorType = vertexType & VTColorMask;
	if( colorType == VTColorBGR5650 )
		size += 2;
	else if( colorType == VTColorABGR4444 )
		size += 2;
	else if( colorType == VTColorABGR5551 )
		size += 2;
	else if( colorType == VTColorABGR8888 )
		size += 4;

	int indexType = vertexType & VTIndexMask;
	if( indexType == VTIndex8 )
		size += 1;
	else if( indexType == VTIndex16 )
		size += 2;

	return size;
}

void DrawVertexBuffer( OglContext* context, int primitiveType, int primitiveCount, int vertexType, int vertexCount, int vertexSize, byte* ptr )
{
	int positionType = ( vertexType & VTPositionMask );
	int normalType = ( vertexType & VTNormalMask );
	int textureType = ( vertexType & VTTextureMask );
	int weightType = ( vertexType & VTWeightMask );
	int colorType = ( vertexType & VTColorMask );
	int indexType = ( vertexType & VTIndexMask );
	bool transformed = false; // ??

	// DO NOT SUPPORT INDICES AND WEIGHTS OR TRANSFORMED

	// PSP comes in a crazy order:
	//float skinWeight[WEIGHTS_PER_VERTEX];
	//float u,v;
	//unsigned int color;
	//float nx,ny,nz;
	//float x,y,z;

	glBegin( primitiveType );

	byte* src = ptr;
	for( int n = 0; n < vertexCount; n++ )
	{
		/*switch( weightType )
		{
		default:
			break;
		}*/

		switch( textureType )
		{
		case VTTextureFixed8:
			src += 2;
			break;
		case VTTextureFixed16:
			glTexCoord2sv( ( short* )src );
			src += 4;
			break;
		case VTTextureFloat:
			glTexCoord2fv( ( float* )src );
			src += 8;
			break;
		}

		switch( colorType )
		{
		case VTColorBGR5650:
			src += 2;
			break;
		case VTColorABGR4444:
			src += 2;
			break;
		case VTColorABGR5551:
			src += 2;
			break;
		case VTColorABGR8888:
			glColor3ubv( src );
			src += 4;
			break;
		}

		switch( normalType )
		{
		case VTNormalFixed8:
			glNormal3bv( ( GLbyte* )src );
			src += 3;
			break;
		case VTNormalFixed16:
			glNormal3sv( ( short* )src );
			src += 6;
			break;
		case VTNormalFloat:
			glNormal3fv( ( float* )src );
			src += 12;
			break;
		}

		switch( positionType )
		{
		case VTPositionFixed8:
			src += 3;
			break;
		case VTPositionFixed16:
			glVertex3sv( ( short* )src );
			src += 6;
			break;
		case VTPositionFloat:
			//float x = *( ( float* )src );
			//float y = *( ( float* )src + 1 );
			//float z = *( ( float* )src + 2 );
			glVertex3fv( ( float* )src );
			src += 12;
			break;
		}
	}

	glEnd();
}

__inline bool IsTextureValid( OglTexture* texture )
{
	if( ( texture->Address == 0x0 ) ||
		( texture->LineWidth == 0 ) ||
		( texture->Width == 0 ) ||
		( texture->Height == 0 ) )
		return false;

#if 0
	// This is a special case - something to do with the framebuffer being set as the texture?
	if( ( texture->Address == 0x04000000 ) &&
		( texture->LineWidth == 0x4 ) &&
		( texture->Width == 0x2 ) &&
		( texture->Height == 0x2 ) )
		return false;
#endif

	return true;
}

void Unswizzle( byte* address, int size, int bpp )
{
}

void SetTexture( OglContext* context, int stage )
{
	OglTexture* texture = &context->Textures[ stage ];

	bool textureValid = IsTextureValid( texture );
	if( textureValid == false )
		return;

	if( context->TexturesEnabled == false )
	{
		glEnable( GL_TEXTURE_2D );
		context->TexturesEnabled = true;
	}

	if( texture->TextureID > 0 )
	{
		// Texture has been generated, so we just set
		//glBindTexture( GL_TEXTURE_2D, texture->TextureID );
	}
	else
	{
		// Grab and decode texture, then create in OGL

		uint textureId;
		glGenTextures( 1, &textureId );
		glBindTexture( GL_TEXTURE_2D, textureId );
		texture->TextureID = textureId;
		
		glPixelStorei( GL_UNPACK_ALIGNMENT, 1 );
		glPixelStorei( GL_UNPACK_ROW_LENGTH, texture->LineWidth );

		byte* address = context->MemoryPointer + ( texture->Address - MainMemoryBase );

		int bpp = 4;
		int size = texture->LineWidth * texture->Height * bpp;

		if( context->TexturesSwizzled == true )
		{
			Unswizzle( address, size, bpp );
		}

		HANDLE f = CreateFileA( "test.raw", GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_TEMPORARY, NULL );
		int dummy1;
		WriteFile( f, ( void* )address, size, ( LPDWORD )&dummy1, NULL );
		CloseHandle( f );

		glTexImage2D( GL_TEXTURE_2D, 0, 3,
			texture->Width, texture->Height,
			0, GL_RGBA, GL_UNSIGNED_BYTE,
			( void* )address );

		// ??
		glTexEnvf( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL );
	}
}

#pragma managed
