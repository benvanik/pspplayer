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

Point _last[ 1024 * 10 ];

void DrawBezier( OglContext* context, int vertexType, int vertexSize, byte* iptr, byte* ptr, int ucount, int vcount )
{
	// We only support 4x4 patches right now
	assert( ucount == 4 );
	assert( vcount == 4 );

	int positionType = ( vertexType & VTPositionMask );
	int normalType = ( vertexType & VTNormalMask );
	int textureType = ( vertexType & VTTextureMask );
	int colorType = ( vertexType & VTColorMask );
	int weightType = ( vertexType & VTWeightMask );

	assert( normalType == 0 );
	assert( textureType == 0 );
	assert( colorType == 0 );
	assert( weightType == 0 );

	float uoffset = context->TextureOffset[ 0 ];
	float voffset = context->TextureOffset[ 1 ];
	float uscale = context->TextureScale[ 0 ];
	float vscale = context->TextureScale[ 1 ];

	Point anchors[ 4 ][ 4 ];
	switch( positionType )
	{
	case VTPositionFixed8:
		{
			char* src = ( char* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					anchors[ u ][ v ] = Point( ( float )src[ 0 ], ( float )src[ 1 ], ( float )src[ 2 ] );
					src += 3;
				}
			}
		}
		break;
	case VTPositionFixed16:
		{
			short* src = ( short* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					anchors[ u ][ v ] = Point( ( float )src[ 0 ], ( float )src[ 1 ], ( float )src[ 2 ] );
					src += 3;
				}
			}
		}
		break;
	case VTPositionFloat:
		{
			float* src = ( float* )ptr;
			for( int u = 0; u < ucount; u++ )
			{
				for( int v = 0; v < vcount; v++ )
				{
					anchors[ u ][ v ] = Point( src[ 0 ], src[ 1 ], src[ 2 ] );
					src += 3;
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

	// Create the first line of points
	for( int v = 0; v <= vdivs; v++ )
	{
		float px = ( ( float )v ) / ( ( float )vdivs );				// Percent along y axis
		// use the 4 points from the derives curve to calculate the points along that curve
		_last[ v ] = Bernstein( px, temp );
	}

	for( int u = 1; u <= udivs; u++ )
	{
		float py	= ( ( float )u ) / ( ( float )udivs );			// Percent along Y axis
		float pyold	= ( ( float )u - 1.0f ) / ( ( float)udivs );	// Percent along old Y axis

		temp[ 0 ] = Bernstein( py, anchors[ 0 ] );					// Calculate new bezier points
		temp[ 1 ] = Bernstein( py, anchors[ 1 ] );
		temp[ 2 ] = Bernstein( py, anchors[ 2 ] );
		temp[ 3 ] = Bernstein( py, anchors[ 3 ] );

		py = ( ( 1.0f - py ) * uscale ) + uoffset;
		pyold = ( ( 1.0f - pyold ) * uscale ) + uoffset;

		glBegin( GL_TRIANGLE_STRIP );

		for( int v = 0; v <= vdivs; v++ )
		{
			float px = ( ( float )v ) / ( ( float )vdivs );			// Percent along the X axis
			float tpx = ( ( 1.0f - px ) * vscale ) + voffset;

			glTexCoord2f( pyold, tpx );								// Apply the old texture coords
			glVertex3d( _last[ v ].x, _last[ v ].y, _last[ v ].z );	// Old Point

			_last[ v ] = Bernstein( px, temp );						// Generate new point
			glTexCoord2f( py, tpx );								// Apply the new texture coords
			glVertex3d( _last[ v ].x, _last[ v ].y, _last[ v ].z );	// New Point
		}

		glEnd();
	}
}

#pragma managed
