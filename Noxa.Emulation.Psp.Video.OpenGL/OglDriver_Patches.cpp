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

void DrawBezier( OglContext* context, int vertexType, int vertexSize, byte* iptr, byte* ptr, int ucount, int vcount );
//void DrawSpline( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

#pragma unmanaged

// Bezier code adapted from NeHe lesson 28
// http://nehe.gamedev.net/data/lessons/lesson.asp?lesson=28

typedef struct Point_t
{
	float x, y, z;
	Point_t()
	{
	}
	Point_t( float _x, float _y, float _z ) :
		x( _x ), y( _y ), z( _z )
	{
	}
} Point;

typedef struct Coord_t
{
	float u, v;
	Coord_t()
	{
	}
	Coord_t( float _u, float _v ) :
		u( _u ), v( _v )
	{
	}
} Coord;

Point AddPoints( Point p, Point q )
{
	p.x += q.x;		p.y += q.y;		p.z += q.z;
	return p;
}
Point MultPoints( float c, Point p )
{
	p.x *= c;		p.y *= c;		p.z *= c;
	return p;
}
Coord AddPoints2D( Coord p, Coord q )
{
	p.u += q.u;		p.v += q.v;
	return p;
}
Coord MultPoints2D( float c, Coord p )
{
	p.u *= c;		p.v *= c;
	return p;
}

// Calculates 3rd degree polynomial based on array of 4 points
// and a single variable (u) which is generally between 0 and 1
Point Bernstein( float u, Point* p )
{
	Point a = MultPoints( pow( u, 3 ), p[ 0 ] );
	Point b = MultPoints( 3 * pow( u, 2 ) * ( 1 - u ), p[ 1 ] );
	Point c = MultPoints( 3 * u * pow( ( 1 - u ), 2 ), p[ 2 ] );
	Point d = MultPoints( pow( ( 1 - u ), 3 ), p[ 3 ] );
	Point r = AddPoints( AddPoints( a, b ), AddPoints( c, d ) );
	return r;
}

Coord Bernstein2D( float u, Coord* p )
{
	Coord a = MultPoints2D( pow( u, 3 ), p[ 0 ] );
	Coord b = MultPoints2D( 3 * pow( u, 2 ) * ( 1 - u ), p[ 1 ] );
	Coord c = MultPoints2D( 3 * u * pow( ( 1 - u ), 2 ), p[ 2 ] );
	Coord d = MultPoints2D( pow( ( 1 - u ), 3 ), p[ 3 ] );
	Coord r = AddPoints2D( AddPoints2D( a, b ), AddPoints2D( c, d ) );
	return r;
}

Point _last[ 1024 * 10 ];
Coord _lastCoord[ 1024 * 10 ];

void DrawBezier( OglContext* context, int vertexType, int vertexSize, byte* iptr, byte* ptr, int ucount, int vcount )
{
	// We only support 4x4 patches right now
	assert( ucount == 4 );
	assert( vcount == 4 );

	bool transformed = ( vertexType & VTTransformedMask ) != 0;

	int positionType = ( vertexType & VTPositionMask );
	int normalType = ( vertexType & VTNormalMask );
	int textureType = ( vertexType & VTTextureMask );
	int colorType = ( vertexType & VTColorMask );
	int weightType = ( vertexType & VTWeightMask );

	assert( normalType == 0 );
	assert( colorType == 0 );
	assert( weightType == 0 );

	int textureWidth = context->Textures[ 0 ].Width;
	int textureHeight = context->Textures[ 0 ].Height;
	float uoffset = context->TextureOffset[ 0 ];
	float voffset = context->TextureOffset[ 1 ];
	float uscale = context->TextureScale[ 0 ];
	float vscale = context->TextureScale[ 1 ];

	Point anchors[ 4 ][ 4 ];
	Coord anchorCoords[ 4 ][ 4 ];

	switch( textureType )
	{
	case VTTextureFixed8:
		assert( false );
		ptr += 2;
		break;
	case VTTextureFixed16:
		{
			byte* src = ( byte* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					ushort *s = ( ushort* )src;
					if( transformed == true )
						anchorCoords[ u ][ v ] = Coord(
							( float )s[ 0 ] / textureWidth, ( float )s[ 1 ] / textureHeight );
					else
						anchorCoords[ u ][ v ] = Coord(
							( ( float )s[ 0 ] * uscale ) + uoffset, ( ( float )s[ 1 ] * vscale ) + voffset );
					src += vertexSize;
				}
			}
		}
		ptr += 4;
		break;
	case VTTextureFloat:
		{
			byte* src = ( byte* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					float *s = ( float* )src;
					if( transformed == true )
						anchorCoords[ u ][ v ] = Coord(
							s[ 0 ] / textureWidth, s[ 1 ] / textureHeight );
					else
						anchorCoords[ u ][ v ] = Coord(
							( s[ 0 ] * uscale ) + uoffset, ( s[ 1 ] * vscale ) + voffset );
					src += vertexSize;
				}
			}
		}
		ptr += 8;
		break;
	}
	switch( positionType )
	{
	case VTPositionFixed8:
		{
			byte* src = ( byte* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					char *s = ( char* )src;
					anchors[ u ][ v ] = Point( ( float )s[ 0 ], ( float )s[ 1 ], ( float )s[ 2 ] );
					src += vertexSize;
				}
			}
		}
		break;
	case VTPositionFixed16:
		{
			byte* src = ( byte* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					short *s = ( short* )src;
					anchors[ u ][ v ] = Point( ( float )s[ 0 ], ( float )s[ 1 ], ( float )s[ 2 ] );
					src += vertexSize;
				}
			}
		}
		break;
	case VTPositionFloat:
		{
			byte* src = ( byte* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					float *s = ( float* )src;
					// Sometimes z is really big (65535) - this is either an error in the vfpu, fpu, or really means something on the psp
					// HACK: ignore big Z in bezier
					// For now, just pretend we didn't see it ^_^
					anchors[ u ][ v ] = Point( s[ 0 ], s[ 1 ], ( s[ 2 ] > 60000 ) ? 1.0f : s[ 2 ] );
					src += vertexSize;
				}
			}
		}
		break;
	}
	
	int udivs = context->PatchDivS;
	int vdivs = context->PatchDivT;

	// The first derived curve (along x axis)
	Point temp[ 4 ];
	temp[ 0 ] = anchors[ 0 ][ 3 ];
	temp[ 1 ] = anchors[ 1 ][ 3 ];
	temp[ 2 ] = anchors[ 2 ][ 3 ];
	temp[ 3 ] = anchors[ 3 ][ 3 ];
	Coord tempCoord[ 4 ];
	tempCoord[ 0 ] = anchorCoords[ 0 ][ 3 ];
	tempCoord[ 1 ] = anchorCoords[ 1 ][ 3 ];
	tempCoord[ 2 ] = anchorCoords[ 2 ][ 3 ];
	tempCoord[ 3 ] = anchorCoords[ 3 ][ 3 ];

	// Create the first line of points
	for( int v = 0; v <= vdivs; v++ )
	{
		float px = ( ( float )v ) / ( ( float )vdivs );				// Percent along y axis
		// use the 4 points from the derives curve to calculate the points along that curve
		_last[ v ] = Bernstein( px, temp );
		if( textureType != 0 )
			_lastCoord[ v ] = Bernstein2D( px, tempCoord );
	}

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

	for( int u = 1; u <= udivs; u++ )
	{
		float py	= ( ( float )u ) / ( ( float )udivs );			// Percent along Y axis
		float pyold	= ( ( float )u - 1.0f ) / ( ( float )udivs );	// Percent along old Y axis

		temp[ 0 ] = Bernstein( py, anchors[ 0 ] );					// Calculate new bezier points
		temp[ 1 ] = Bernstein( py, anchors[ 1 ] );
		temp[ 2 ] = Bernstein( py, anchors[ 2 ] );
		temp[ 3 ] = Bernstein( py, anchors[ 3 ] );
		if( textureType != 0 )
		{
			tempCoord[ 0 ] = Bernstein2D( py, anchorCoords[ 0 ] );	// Calculate new bezier points
			tempCoord[ 1 ] = Bernstein2D( py, anchorCoords[ 1 ] );
			tempCoord[ 2 ] = Bernstein2D( py, anchorCoords[ 2 ] );
			tempCoord[ 3 ] = Bernstein2D( py, anchorCoords[ 3 ] );
		}

		py = ( ( 1.0f - py ) * uscale ) + uoffset;
		pyold = ( ( 1.0f - pyold ) * uscale ) + uoffset;

		glBegin( GL_TRIANGLE_STRIP );

		if( ( context->WireframeEnabled == true ) &&
			( textureType != 0 ) )
			glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );

		if( ( context->WireframeEnabled == false ) &&
			( transformed == true ) )
			glColor4fv( context->AmbientMaterial );

		for( int v = 0; v <= vdivs; v++ )
		{
			float px = ( ( float )v ) / ( ( float )vdivs );			// Percent along the X axis
			float tpx = ( ( 1.0f - px ) * vscale ) + voffset;

			if( textureType != 0 )
				glTexCoord2f( _lastCoord[ v ].u, _lastCoord[ v ].v );
			else
				glTexCoord2f( pyold, tpx );
			assert( _last[ v ].z < 50000.0f );
			glVertex3d( _last[ v ].x, _last[ v ].y, _last[ v ].z );	// Old Point

			_last[ v ] = Bernstein( px, temp );						// Generate new point
			_lastCoord[ v ] = Bernstein2D( px, tempCoord );			// Generate new point
			if( textureType != 0 )
				glTexCoord2f( _lastCoord[ v ].u, _lastCoord[ v ].v );
			else
				glTexCoord2f( py, tpx );
			assert( _last[ v ].z < 50000.0f );
			glVertex3d( _last[ v ].x, _last[ v ].y, _last[ v ].z );	// New Point
		}

		glEnd();
	}

	if( transformed == true )
	{
		glPopMatrix();
		glMatrixMode( GL_PROJECTION );
		glPopMatrix();
		glPopAttrib();
	}
}

#pragma managed
