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

extern uint _commandCounts[ 256 ];

#define COLORSWIZZLE( bgra ) bgra

__inline void WidenMatrix( float src[ 16 ], float dest[ 16 ] );
int DetermineVertexSize( int vertexType );

// OglDriver_VertexLists
extern void DrawBuffers( OglContext* context, int primitiveType, int vertexType, int vertexCount, byte* indexBuffer );
extern void SetupVertexBuffers( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

// OglDriver_Sprites
extern void DrawSpriteList( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

// OglDriver_Textures
extern void TextureTransfer( OglContext* context );
extern void SetTexture( OglContext* context, int stage );

#pragma unmanaged

void ProcessList( OglContext* context, VideoDisplayList* list )
{
	int temp;
	float matrixTemp[ 16 ];
	float color3[ 3 ] = { 0.0f, 0.0f, 0.0f };
	float color4[ 4 ] = { 0.0f, 0.0f, 0.0f, 0.0f };

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

#ifdef STATISTICS
		_commandCounts[ packet.Command ]++;
#endif

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

		case ATE:
			// alpha test enable
			if( argi == 0 )
				glDisable( GL_ALPHA_TEST );
			else
				glEnable( GL_ALPHA_TEST );
			break;
		case ATST:
			switch( argi & 0xFF )
			{
			default:
			case 0:
				temp = GL_NEVER;
				break;
			case 1:
				temp = GL_ALWAYS;
				break;
			case 2:
				temp = GL_EQUAL;
				break;
			case 3:
				temp = GL_NOTEQUAL;
				break;
			case 4:
				temp = GL_LESS;
				break;
			case 5:
				temp = GL_LEQUAL;
				break;
			case 6:
				temp = GL_GREATER;
				break;
			case 7:
				temp = GL_GEQUAL;
				break;
			}
			glAlphaFunc( temp, ( ( argi >> 8 ) & 0xFF ) / 255.0f );
			// @param mask - Specifies the mask that both values are ANDed with before comparison.
			//mask = ( argi >> 16 ) & 0xFF;
			break;

		case ZTE:
			// depth (z) test enable
			if( argi == 0 )
				glDisable( GL_DEPTH_TEST );
			else
				glEnable( GL_DEPTH_TEST );
			break;
		case ZTST:
			/*
			  *   - GU_NEVER - No pixels pass the depth-test
			  *   - GU_ALWAYS - All pixels pass the depth-test
			  *   - GU_EQUAL - Pixels that match the depth-test pass
			  *   - GU_NOTEQUAL - Pixels that doesn't match the depth-test pass
			  *   - GU_LESS - Pixels that are less in depth passes
			  *   - GU_LEQUAL - Pixels that are less or equal in depth passes
			  *   - GU_GREATER - Pixels that are greater in depth passes
			  *   - GU_GEQUAL - Pixels that are greater or equal passes
			  */
			switch( argi )
			{
			default:
			case 0:
				temp = GL_NEVER;
				break;
			case 1:
				temp = GL_ALWAYS;
				break;
			case 2:
				temp = GL_EQUAL;
				break;
			case 3:
				temp = GL_NOTEQUAL;
				break;
			case 4:
				temp = GL_LESS;
				break;
			case 5:
				temp = GL_LEQUAL;
				break;
			case 6:
				temp = GL_GREATER;
				break;
			case 7:
				temp = GL_GEQUAL;
				break;
			}
			glDepthFunc( temp );
			break;

		case ABE:
			// alpha blend enable
			if( argi == 0 )
				glDisable( GL_BLEND );
			else
				glEnable( GL_BLEND );
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
				default:
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
					assert( false );
					break;
				}
				int dest;
				switch( ( argi >> 4 ) & 0xF )
				{
				default:
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
					assert( false );
					break;
				}
				glBlendFunc( src, dest );
			}
			break;
		case SFIX:	// source fix color
			break;
		case DFIX:	// destination fix color
			break;

		case SCISSOR1:	// scissor start
			context->Scissor[ 0 ] = argi & 0x3FF;
			context->Scissor[ 1 ] = ( ( argi >> 10 ) & 0x3FF );
			break;
		case SCISSOR2:	// scissor end - I think this always follows a start
			context->Scissor[ 2 ] = ( argi & 0x3FF ) + 1;
			context->Scissor[ 3 ] = ( ( argi >> 10 ) & 0x3FF ) + 1;
			if( ( context->Scissor[ 0 ] == 0 ) &&
				( context->Scissor[ 1 ] == 0 ) &&
				( context->Scissor[ 2 ] == 480 ) &&
				( context->Scissor[ 3 ] == 272 ) )
				glDisable( GL_SCISSOR_TEST );
			else
			{
				// We are given x1,y1 x2,y2, NOT width,height!
				glEnable( GL_SCISSOR_TEST );
				glScissor(
					context->Scissor[ 0 ], 272 - context->Scissor[ 3 ],
					context->Scissor[ 2 ] - context->Scissor[ 0 ], context->Scissor[ 3 ] - context->Scissor[ 1 ] );
			}
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
					ptr = context->VideoMemoryPointer + ( vertexBufferAddress - VideoMemoryBase );

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
							iptr = context->VideoMemoryPointer + ( indexBufferAddress - VideoMemoryBase );
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
		case TSYNC:
			//SetTexture( context, 0 );
			break;
		case TMODE:
			context->TexturesSwizzled = ( argi & 0x1 ) == 1 ? true : false;
			context->MipMapLevel = ( argi >> 16 ) & 0x4;
			break;
		case TPSM:
			// TexturePixelStorage (TPS*)
			context->TextureStorageMode = argi;
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
		//case TWRAP:
		//	switch( argi & 0xFF )
		//	{
		//	case 0: // GU_REPEAT
		//		glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT );
		//		break;
		//	case 1:	// GU_CLAMP
		//		glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP );
		//		break;
		//	}
		//	switch( ( argi >> 8 ) & 0xFF )
		//	{
		//	case 0: // GU_REPEAT
		//		glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT );
		//		break;
		//	case 1:	// GU_CLAMP
		//		glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP );
		//		break;
		//	}
		//	break;
		case TFUNC:
			// texture function
			/*
			  *   - GU_TFX_MODULATE - Cv=Ct*Cf TCC_RGB: Av=Af TCC_RGBA: Av=At*Af
			  *   - GU_TFX_DECAL - TCC_RGB: Cv=Ct,Av=Af TCC_RGBA: Cv=Cf*(1-At)+Ct*At Av=Af
			  *   - GU_TFX_BLEND - Cv=(Cf*(1-Ct))+(Cc*Ct) TCC_RGB: Av=Af TCC_RGBA: Av=At*Af
			  *   - GU_TFX_REPLACE - Cv=Ct TCC_RGB: Av=Af TCC_RGBA: Av=At
			  *   - GU_TFX_ADD - Cv=Cf+Ct TCC_RGB: Av=Af TCC_RGBA: Av=At*Af
			  *
			  * tcc is GU_TCC_RGB or GU_TCC_RGBA
			  */
			switch( argi & 0xFF )
			{
			default:
			case 0:
				temp = GL_MODULATE;
				break;
			case 1:
				temp = GL_DECAL;
				break;
			case 2:
				temp = GL_BLEND;
				break;
			case 3:
				temp = GL_REPLACE;
				break;
			case 4:
				// I think this works
				temp = GL_ADD;
				break;
			}
			glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, temp );
			break;
		case TEC:
			color4[ 0 ] = ( ( argi >> 16 ) & 0xFF ) / 255.0f;
			color4[ 1 ] = ( ( argi >> 8 ) & 0xFF ) / 255.0f;
			color4[ 2 ] = ( argi & 0xFF ) / 255.0f;
			color4[ 3 ] = 0;
			glTexEnvfv( GL_TEXTURE_ENV, GL_TEXTURE_ENV_COLOR, color4 );
			break;
		case TFLUSH:
			// texturesvalid = false
			context->Textures[ 0 ].TextureID = 0;
			break;
		case USCALE:
			// (float) should be 1
			//assert( argf == 1.0f );
			break;
		case VSCALE:
			// (float) should be 1
			//assert( argf == 1.0f );
			break;
		case UOFFSET:
			// (float) should be 0
			//assert( argf == 0.0f );
			break;
		case VOFFSET:
			// (float) should be 0
			//assert( argf == 0.0f );
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
			context->Textures[ 0 ].TextureID = 0;
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

#pragma managed
