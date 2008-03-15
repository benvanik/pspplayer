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
			bool clockwise = ( _ctx.Values[ ( int )VideoCommand.PFACE ] & 0x1 ) == 0;

			// We only support 4x4 patches right now
			Debug.Assert( divS == 4 );
			Debug.Assert( divT == 4 );

			uint alignmentDelta;
			int vertexSize = ( int )DetermineVertexSize( vertexType, out alignmentDelta );

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;

			byte* vertexBuffer = this.MemorySystem.Translate( vaddr );
		}

		private void DrawSpline( uint listBase, uint argi )
		{
		}

		// Bezier code adapted from NeHe lesson 28
		// http://nehe.gamedev.net/data/lessons/lesson.asp?lesson=28

		struct Point
		{
			public float x, y, z;
		}

		struct Coord
		{
			public float u, v;
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
		private static void Bernstein( float u, Point* p, out Point r )
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

		private static void Bernstein2D( float u, Coord* p, out Coord r )
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
