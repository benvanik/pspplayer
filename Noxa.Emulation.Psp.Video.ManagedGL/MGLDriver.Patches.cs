// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		private Point[] _lastPatchPoint = new Point[ 1024 * 10 ];
		private Coord[] _lastPatchCoord = new Coord[ 1024 * 10 ];

		private void DrawBezier( uint listBase, uint argi )
		{
			uint t = _ctx.Values[ ( int )VideoCommand.VTYPE ];
			uint vertexType = t & 0x00801FFF;
			bool isTransformed = ( vertexType & VertexType.TransformedMask ) != 0;
			bool isIndexed = ( vertexType & ( VertexType.Index8 | VertexType.Index16 ) ) != 0;
			Debug.Assert( isIndexed == false );

			t = _ctx.Values[ ( int )VideoCommand.PSUB ];
			uint divS = t & 0xFF;
			uint divT = ( t >> 8 ) & 0xFF;
			uint ucount = argi & 0xFF;
			uint vcount = ( argi >> 8 ) & 0xFF;
			bool clockwise = ( _ctx.Values[ ( int )VideoCommand.PFACE ] & 0x1 ) == 0;

			// We only support 4x4 patches right now
			Debug.Assert( ucount == 4 );
			Debug.Assert( vcount == 4 );

			uint alignmentDelta;
			int vertexSize = ( int )DetermineVertexSize( vertexType, out alignmentDelta );

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;
			byte* ptr = this.MemorySystem.Translate( vaddr );

			this.UpdateState( StateRequest.Drawing );
			if( isTransformed == true )
				this.SetState( FeatureState.CullFaceMask | FeatureState.DepthTestMask, 0 );

			// Setup textures
			if( _ctx.TexturesEnabled == true )
				this.SetTextures();

			uint positionType = ( vertexType & VertexType.PositionMask );
			uint normalType = ( vertexType & VertexType.NormalMask );
			uint textureType = ( vertexType & VertexType.TextureMask );
			uint colorType = ( vertexType & VertexType.ColorMask );
			uint weightType = ( vertexType & VertexType.WeightMask );

			Debug.Assert( normalType == 0 );
			Debug.Assert( colorType == 0 );
			Debug.Assert( weightType == 0 );

			// Setup shader
			// TODO: choose the right one
			this.SetDefaultProgram( isTransformed, colorType, 1, 0 );

			Point[][] anchors = new Point[ 4 ][] { new Point[ 4 ], new Point[ 4 ], new Point[ 4 ], new Point[ 4 ] };
			Coord[][] anchorCoords = new Coord[ 4 ][] { new Coord[ 4 ], new Coord[ 4 ], new Coord[ 4 ], new Coord[ 4 ] };

			switch( textureType )
			{
				case VertexType.TextureFixed8:
					Debug.Assert( false );
					ptr += 2;
					break;
				case VertexType.TextureFixed16:
					{
						byte* src = ( byte* )ptr;
						for( int u = 0; u < ucount; u++ )
						{
							for( int v = 0; v < vcount; v++ )
							{
								ushort* s = ( ushort* )src;
								anchorCoords[ u ][ v ] = new Coord( ( float )s[ 0 ], ( float )s[ 1 ] );
								src += vertexSize;
							}
						}
					}
					ptr += 4;
					break;
				case VertexType.TextureFloat:
					{
						byte* src = ( byte* )ptr;
						for( int u = 0; u < ucount; u++ )
						{
							for( int v = 0; v < vcount; v++ )
							{
								float* s = ( float* )src;
								anchorCoords[ u ][ v ] = new Coord( s[ 0 ], s[ 1 ] );
								src += vertexSize;
							}
						}
					}
					ptr += 8;
					break;
			}
			switch( positionType )
			{
				case VertexType.PositionFixed8:
					{
						byte* src = ( byte* )ptr;
						for( int u = 0; u < ucount; u++ )
						{
							for( int v = 0; v < vcount; v++ )
							{
								char* s = ( char* )src;
								anchors[ u ][ v ] = new Point( ( float )s[ 0 ], ( float )s[ 1 ], ( float )s[ 2 ] );
								src += vertexSize;
							}
						}
					}
					break;
				case VertexType.PositionFixed16:
					{
						byte* src = ( byte* )ptr;
						for( int u = 0; u < ucount; u++ )
						{
							for( int v = 0; v < vcount; v++ )
							{
								short* s = ( short* )src;
								anchors[ u ][ v ] = new Point( ( float )s[ 0 ], ( float )s[ 1 ], ( float )s[ 2 ] );
								src += vertexSize;
							}
						}
					}
					break;
				case VertexType.PositionFloat:
					{
						byte* src = ( byte* )ptr;
						for( int u = 0; u < ucount; u++ )
						{
							for( int v = 0; v < vcount; v++ )
							{
								float* s = ( float* )src;
								// Sometimes z is really big (65535) - this is either an error in the vfpu, fpu, or really means something on the psp
								// HACK: ignore big Z in bezier
								// For now, just pretend we didn't see it ^_^
								anchors[ u ][ v ] = new Point( s[ 0 ], s[ 1 ], ( s[ 2 ] > 60000 ) ? 1.0f : s[ 2 ] );
								src += vertexSize;
							}
						}
					}
					break;
			}

			uint udivs = divS;
			uint vdivs = divT;

			// The first derived curve (along x axis)
			Point[] temp = new Point[] { anchors[ 0 ][ 3 ], anchors[ 1 ][ 3 ], anchors[ 2 ][ 3 ], anchors[ 3 ][ 3 ] };
			Coord[] tempCoord = new Coord[] { anchorCoords[ 0 ][ 3 ], anchorCoords[ 1 ][ 3 ], anchorCoords[ 2 ][ 3 ], anchorCoords[ 3 ][ 3 ] };

			// Create the first line of points
			for( int v = 0; v <= vdivs; v++ )
			{
				float px = ( ( float )v ) / ( ( float )vdivs );				// Percent along y axis
				// use the 4 points from the derives curve to calculate the points along that curve
				Bernstein( px, temp, out _lastPatchPoint[ v ] );
				if( textureType != 0 )
					Bernstein2D( px, tempCoord, out _lastPatchCoord[ v ] );
			}

			for( int u = 1; u <= udivs; u++ )
			{
				float py = ( ( float )u ) / ( ( float )udivs );			// Percent along Y axis
				float pyold = ( ( float )u - 1.0f ) / ( ( float )udivs );	// Percent along old Y axis

				Bernstein( py, anchors[ 0 ], out temp[ 0 ] );					// Calculate new bezier points
				Bernstein( py, anchors[ 1 ], out temp[ 1 ] );
				Bernstein( py, anchors[ 2 ], out temp[ 2 ] );
				Bernstein( py, anchors[ 3 ], out temp[ 3 ] );
				if( textureType != 0 )
				{
					Bernstein2D( py, anchorCoords[ 0 ], out tempCoord[ 0 ] );	// Calculate new bezier points
					Bernstein2D( py, anchorCoords[ 1 ], out tempCoord[ 1 ] );
					Bernstein2D( py, anchorCoords[ 2 ], out tempCoord[ 2 ] );
					Bernstein2D( py, anchorCoords[ 3 ], out tempCoord[ 3 ] );
				}

				py = 1.0f - py;
				pyold = 1.0f - pyold;

				Gl.glBegin( Gl.GL_TRIANGLE_STRIP );

				if( ( this.DrawWireframe == true ) &&
					( textureType != 0 ) )
					Gl.glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );

				if( ( this.DrawWireframe == false ) &&
					( isTransformed == true ) )
					Gl.glColor4fv( _ctx.AmbientModelColor );

				for( int v = 0; v <= vdivs; v++ )
				{
					float px = ( ( float )v ) / ( ( float )vdivs );			// Percent along the X axis
					float tpx = 1.0f - px;

					if( textureType != 0 )
						Gl.glTexCoord2f( _lastPatchCoord[ v ].u, _lastPatchCoord[ v ].v );
					else
						Gl.glTexCoord2f( pyold, tpx );
					Debug.Assert( _lastPatchPoint[ v ].z < 50000.0f );
					Gl.glVertex3d( _lastPatchPoint[ v ].x, _lastPatchPoint[ v ].y, _lastPatchPoint[ v ].z );	// Old Point

					Bernstein( px, temp, out _lastPatchPoint[ v ] );						// Generate new point
					Bernstein2D( px, tempCoord, out _lastPatchCoord[ v ] );			// Generate new point
					if( textureType != 0 )
						Gl.glTexCoord2f( _lastPatchCoord[ v ].u, _lastPatchCoord[ v ].v );
					else
						Gl.glTexCoord2f( py, tpx );
					Debug.Assert( _lastPatchPoint[ v ].z < 50000.0f );
					Gl.glVertex3d( _lastPatchPoint[ v ].x, _lastPatchPoint[ v ].y, _lastPatchPoint[ v ].z );	// New Point
				}

				Gl.glEnd();
			}
		}

		private void DrawSpline( uint listBase, uint argi )
		{
		}

		// Bezier code adapted from NeHe lesson 28
		// http://nehe.gamedev.net/data/lessons/lesson.asp?lesson=28

		struct Point
		{
			public float x, y, z;
			public Point( float x, float y, float z )
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		struct Coord
		{
			public float u, v;
			public Coord( float u, float v )
			{
				this.u = u;
				this.v = v;
			}
		}

		private static void MultPoints( float c, ref Point p, out Point w )
		{
			w.x = p.x * c;
			w.y = p.y * c;
			w.z = p.z * c;
		}
		private static void MultPoints2D( float c, ref Coord p, out Coord w )
		{
			w.u = p.u * c;
			w.v = p.v * c;
		}

		// Calculates 3rd degree polynomial based on array of 4 points
		// and a single variable (u) which is generally between 0 and 1
		private static void Bernstein( float u, Point[] p, out Point r )
		{
			// TODO: replace pow
			Point a, b, c, d;
			MultPoints( ( float )Math.Pow( u, 3 ), ref p[ 0 ], out a );
			MultPoints( 3 * ( float )Math.Pow( u, 2 ) * ( 1 - u ), ref p[ 1 ], out b );
			MultPoints( 3 * u * ( float )Math.Pow( ( 1 - u ), 2 ), ref p[ 2 ], out c );
			MultPoints( ( float )Math.Pow( ( 1 - u ), 3 ), ref p[ 3 ], out d );
			r.x = ( a.x + b.x ) + ( c.x + d.x );
			r.y = ( a.y + b.y ) + ( c.y + d.y );
			r.z = ( a.z + b.z ) + ( c.z + d.z );
		}

		private static void Bernstein2D( float u, Coord[] p, out Coord r )
		{
			Coord a, b, c, d;
			MultPoints2D( ( float )Math.Pow( u, 3 ), ref p[ 0 ], out a );
			MultPoints2D( 3 * ( float )Math.Pow( u, 2 ) * ( 1 - u ), ref  p[ 1 ], out b );
			MultPoints2D( 3 * u * ( float )Math.Pow( ( 1 - u ), 2 ), ref p[ 2 ], out c );
			MultPoints2D( ( float )Math.Pow( ( 1 - u ), 3 ), ref  p[ 3 ], out d );
			r.u = ( a.u + b.u ) + ( c.u + d.u );
			r.v = ( a.v + b.v ) + ( c.v + d.v );
		}
	}
}
