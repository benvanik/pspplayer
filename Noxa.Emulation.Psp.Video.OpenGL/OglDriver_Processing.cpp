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

#define COLORSWIZZLE( bgra ) bgra

__inline void WidenMatrix( float src[ 16 ], float dest[ 16 ] );
int DetermineVertexSize( int vertexType );
void DrawBuffers( OglContext* context, int primitiveType, int vertexType, int vertexCount, byte* indexBuffer );
void SetupVertexBuffers( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );
void DrawSpriteList( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );
void TextureTransfer( OglContext* context );

void SetTexture( OglContext* context, int stage );

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

		case ABE:
			// alpha blend enable
			if( argi == 0 )
				glDisable( GL_BLEND );
			else
				glEnable( GL_BLEND );
			break;
		case ATE:
			// alpha test enable
			break;
		case ZTE:
			// depth (z) test enable
			break;
		case ALPHA:
			// alpha blend
			// pspsdk: sendCommandi(223,src | (dest << 4) | (op << 8));
			// psp_doc: op | src << 4 | dest << 8
			{
				switch( ( argi >> 8 ) & 0x3 )
				{
				case 0:		// GU_ADD
					glBlendEquation( GL_FUNC_ADD );
					break;
				case 1:		// GU_SUBTRACT
					glBlendEquation( GL_FUNC_SUBTRACT );
					break;
				case 2:		// GU_REVERSE_SUBTRACT
					glBlendEquation( GL_FUNC_REVERSE_SUBTRACT );
					break;
				case 3:		// GU_MIN
					glBlendEquation( GL_MIN );
					break;
				case 4:		// GU_MAX
					glBlendEquation( GL_MAX );
					break;
				case 5:		// GU_ABS
					glBlendEquation( GL_FUNC_ADD );
					assert( false );
					break;
				}
				int src;
				switch( argi & 0xF )
				{
#ifndef _DEBUG
				default:
#endif
				case 0:		// GU_SRC_COLOR
					src = GL_SRC_COLOR;
					break;
				case 1:		// GU_ONE_MINUS_SRC_COLOR
					src = GL_ONE_MINUS_SRC_COLOR;
					break;
				case 2:		// GU_SRC_ALPHA
					src = GL_SRC_ALPHA;
					break;
				case 3:		// GU_ONE_MINUS_SRC_ALPHA
					src = GL_ONE_MINUS_SRC_ALPHA;
					break;
				case 10:	// GU_FIX
					break;
#ifdef _DEBUG
				default:
					src = GL_SRC_COLOR;
					break;
#endif
				}
				int dest;
				switch( ( argi >> 4 ) & 0xF )
				{
#ifndef _DEBUG
				default:
#endif
				case 0:		// GU_DST_COLOR
					dest = GL_DST_COLOR;
					break;
				case 1:		// GU_ONE_MINUS_DST_COLOR
					dest = GL_ONE_MINUS_DST_COLOR;
					break;
				case 2:		// GU_SRC_ALPHA
					dest = GL_SRC_ALPHA;
					break;
				case 3:		// GU_ONE_MINUS_SRC_ALPHA
					dest = GL_ONE_MINUS_SRC_ALPHA;
					break;
				case 4:		// GU_DST_ALPHA
					dest = GL_DST_ALPHA;
					break;
				case 5:		// GU_ONE_MINUS_DST_ALPHA
					dest = GL_ONE_MINUS_DST_ALPHA;
					break;
				case 10:	// GU_FIX
					break;
#ifdef _DEBUG
				default:
					dest = GL_DST_COLOR;
					break;
#endif
				}
				glBlendFunc( src, dest );
			}
			break;
		case SFIX:	// source fix color
			break;
		case DFIX:	// destination fix color
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
			vertexType = argi & 0x801FFF; // so we keep transformed bit
			break;
		case VADDR:
			vertexBufferAddress = list->Base | argi;
			break;
		case IADDR:
			indexBufferAddress = list->Base | argi;
			break;
		case PRIM:
			vertexCount = argi & 0xFFFF;
			primitiveType = ( argi >> 16 ) & 0x7;
			areSprites = ( primitiveType == 6 );
			switch( primitiveType )
			{
				case 0x0: // Points
					primitiveType = GL_POINTS;
					break;
				case 0x1: // Lines
					primitiveType = GL_LINES;
					break;
				case 0x2: // Line strips
					primitiveType = GL_LINE_STRIP;
					break;
				case 0x3: // Triangles
					primitiveType = GL_TRIANGLES;
					break;
				case 0x4: // Triangle strips
					primitiveType = GL_TRIANGLE_STRIP;
					break;
				case 0x5: // Triangle fans
					primitiveType = GL_TRIANGLE_FAN;
					break;
				// 0x6 = Sprites (2D rectangles)
			}

			{
				SetTexture( context, 0 );

				int vertexSize = DetermineVertexSize( vertexType );

				byte* ptr;
				if( ( vertexBufferAddress & MainMemoryBase ) != 0 )
					ptr = context->MainMemoryPointer + ( vertexBufferAddress - MainMemoryBase );
				else
					ptr = context->VideoMemoryPointer + ( vertexBufferAddress - FrameBufferBase );

				if( areSprites == false )
				{
					// Normal vertex list

					bool isIndexed = ( vertexType & ( VTIndex8 | VTIndex16 ) ) != 0;
					byte* iptr = 0;
					if( isIndexed == true )
					{
						if( ( indexBufferAddress & MainMemoryBase ) != 0 )
							iptr = context->MainMemoryPointer + ( indexBufferAddress - MainMemoryBase );
						else
							iptr = context->VideoMemoryPointer + ( indexBufferAddress - FrameBufferBase );
					}

					SetupVertexBuffers( context, vertexType, vertexCount, vertexSize, ptr );
					DrawBuffers( context, primitiveType, vertexType, vertexCount, iptr );
				}
				else
				{
					// Sprite list
					DrawSpriteList( context, vertexType, vertexCount, vertexSize, ptr );
				}
			}
			break;

		case TME:
			if( argi == 0 )
				glDisable( GL_TEXTURE_2D );
			else
				glEnable( GL_TEXTURE_2D );
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
			context->Textures[ 0 ].TextureID = 0;
			break;
		case TSIZE0:
			context->Textures[ 0 ].Width = 1 << ( argi & 0x000000FF );
			context->Textures[ 0 ].Height = 1 << ( ( argi >> 8 ) & 0x000000FF );
			context->Textures[ 0 ].PixelStorage = context->TextureStorageMode;
			break;

		case PMS:
			// Next 16 packets are 4x4 projection matrix
			for( int m = 0; m < 16; m++ )
			{
				argx = list->Packets[ n + m + 1 ].Argument << 8;
				matrixTemp[ m ] = *reinterpret_cast<float*>( &argx );
			}
			n += 16;
			glMatrixMode( GL_PROJECTION );
			glLoadMatrixf( matrixTemp );
			break;
		case VMS:
			// Next 12 packets are 3x4 view matrix
			for( int m = 0; m < 12; m++ )
			{
				argx = list->Packets[ n + m + 1 ].Argument << 8;
				matrixTemp[ m ] = *reinterpret_cast<float*>( &argx );
			}
			n += 12;
			WidenMatrix( matrixTemp, context->ViewMatrix );
			glMatrixMode( GL_MODELVIEW );
			glLoadMatrixf( context->ViewMatrix );
			break;
		case WMS:
			// Next 12 packets are 3x4 world matrix
			for( int m = 0; m < 12; m++ )
			{
				argx = list->Packets[ n + m + 1 ].Argument << 8;
				matrixTemp[ m ] = *reinterpret_cast<float*>( &argx );
			}
			n += 12;
			WidenMatrix( matrixTemp, context->WorldMatrix );
			glMatrixMode( GL_MODELVIEW );
			glLoadMatrixf( context->ViewMatrix );
			glMultMatrixf( context->WorldMatrix );
			break;
		case TMS:
			// Next 12 packets are 3x4 texture matrix
			for( int m = 0; m < 12; m++ )
			{
				argx = list->Packets[ n + m + 1 ].Argument << 8;
				matrixTemp[ m ] = *reinterpret_cast<float*>( &argx );
			}
			n += 12;
			// TODO: texture matrix
			break;
		case PROJ: // handled by PMS
		case VIEW: // handled by VMS
		case WORLD: // handled by WMS
		case TMATRIX: // handled by TMS
			break;

			// -- Following commands are for texture copy (to video ram)
		case TRXSBP: // Transmission Source Buffer Pointer
			context->TextureTx.SourceAddress = argi;
			break;
		case TRXSBW: // Transmission Source Buffer Width
			context->TextureTx.SourceAddress |= ( argi << 8 ) & 0xFF000000;
			context->TextureTx.SourceLineWidth = argi & 0x0000FFFF;
			break;
		case TRXDBP: // Transmission Destination Buffer Pointer
			context->TextureTx.DestinationAddress = argi;
			break;
		case TRXDBW: // Transmission Destination Buffer Width
			context->TextureTx.DestinationAddress |= ( argi << 8 ) & 0xFF000000;
			context->TextureTx.DestinationLineWidth = argi & 0x0000FFFF;
			break;
		case TRXSIZE: // Transfer Size
			context->TextureTx.Width = ( argi & 0x3FF ) + 1;
			context->TextureTx.Height = ( ( argi >> 10 ) & 0x1FF ) + 1;
			break;
		case TRXSPOS: // Transfer Source Position
			context->TextureTx.SX = argi & 0x1FF;
			context->TextureTx.SY = ( argi >> 10 ) & 0x1FF;
			break;
		case TRXDPOS: // Transfer Destination Position
			context->TextureTx.DX = argi & 0x3FF;
			context->TextureTx.DY = ( argi >> 10 ) & 0x1FF;
			break;
		case TRXKICK: // Transmission Kick
			context->TextureTx.PixelSize = ( argi & 0x1 );
			TextureTransfer( context );
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

	glDisableClientState( GL_VERTEX_ARRAY );
	glDisableClientState( GL_COLOR_ARRAY );
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
		size += 2 + 2 + 2;
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

	int weightCount = ( vertexType & VTWeightCountMask ) >> 14;
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

	int morphCount = ( vertexType & VTMorphCountMask ) >> 18;

	return size;
}

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
	bool depthTestEnabled = ( glIsEnabled( GL_DEPTH_TEST ) == GL_TRUE );
	if( depthTestEnabled == true )
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

		glEnable( GL_CULL_FACE );
	}

	// Re-enable depth testing
	if( depthTestEnabled == true )
		glEnable( GL_DEPTH_TEST );

	// Re-enable clipping
	glHint( GL_CLIP_VOLUME_CLIPPING_HINT_EXT, GL_DONT_CARE );
}

void TextureTransfer( OglContext* context )
{
	// There are two ways we can do this:
	// a) copy the bits directy to the frame buffer
	// b) copy the bits to a texture and render it as a billboard
	// I don't know which one will be faster - texture uploads are slow, but
	// frame buffer writes are too. At least with the texture method we can
	// better handle weird texture formats as we can use our texture loading
	// code to do things.

	//glFlush();

	byte* buffer;
	int sourceAddress = context->TextureTx.SourceAddress;
	if( sourceAddress & MainMemoryBase )
		buffer = context->MainMemoryPointer + ( sourceAddress - MainMemoryBase );
	else
		buffer = context->VideoMemoryPointer + ( sourceAddress - FrameBufferBase );
	// move to source position

	glPixelStorei( GL_UNPACK_ALIGNMENT, 1 );
	glPixelStorei( GL_UNPACK_ROW_LENGTH, context->TextureTx.SourceLineWidth );

	glPushAttrib( GL_ENABLE_BIT );
	glDisable( GL_TEXTURE_2D );
	glDisable( GL_DEPTH_TEST );

	glMatrixMode( GL_PROJECTION );
	glPushMatrix();
	glLoadIdentity();
	glOrtho( 0.0f, 480.0f, 0.0f, 272.0f, -1.0f, 1.0f );
	glMatrixMode( GL_MODELVIEW );
	glPushMatrix();
	glLoadIdentity();

	/*byte* b = ( byte* )malloc( context->TextureTx.Width * context->TextureTx.Height * 4 );
	memset( b, 0xCDCDCDCD, context->TextureTx.Width * context->TextureTx.Height * 4 );

	glEnable( GL_TEXTURE_2D );
	glTexImage2D( GL_TEXTURE_2D, 0, GL_RGBA,
		context->TextureTx.Width, context->TextureTx.Height,
		0, GL_RGBA, GL_UNSIGNED_BYTE,
        b );*/

	// 0 ---- 1
	// |      |
	// |      |
	// 3 ---- 2
	
	glBegin(GL_QUADS);
		glColor4ub( 255, 255, 255, 255 );
        glTexCoord3f(0, 0, 0); glVertex3f(10, 10, 0);
        glTexCoord3f(1, 0, 0); glVertex3f(100, 10, 0);
        glTexCoord3f(1, -1, 0); glVertex3f(100, 100, 0);
        glTexCoord3f(0, -1, 0); glVertex3f(10, 100, 0);
    glEnd();

	//free( b );

	//glPixelZoom( 1.0f, -1.0f );
	//glRasterPos2f( -0.5f, 0.1f );
	//glRasterPos3i( -100, 0, 1 );
	//glRasterPos2i( context->TextureTx.DX + 16, context->TextureTx.DY + 16 );
	/*glDrawPixels(
		context->TextureTx.Width, context->TextureTx.Height,
		GL_RGBA, GL_UNSIGNED_BYTE,
		buffer );*/

	//glPixelZoom( 1.0f, 1.0f );

	glPopMatrix();
	glMatrixMode( GL_PROJECTION );
	glPopMatrix();

	glPopAttrib();
}

void SetTexture( OglContext* context, int stage )
{
	OglTexture* texture = &context->Textures[ stage ];

	bool textureValid = IsTextureValid( texture );
	if( textureValid == false )
		return;

	if( texture->TextureID > 0 )
	{
		// Texture has been generated, so we just set
		//glBindTexture( GL_TEXTURE_2D, texture->TextureID );
	}
	else
	{
		// Grab and decode texture, then create in OGL
		if( GenerateTexture( context, texture ) == false )
		{
			// Failed? Not much we can do...
		}
	}
}

#pragma managed
