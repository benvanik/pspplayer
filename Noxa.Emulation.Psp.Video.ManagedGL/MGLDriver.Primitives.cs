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
		private static readonly int[] PrimitiveMap = new int[] { Gl.GL_POINTS, Gl.GL_LINES, Gl.GL_LINE_STRIP, Gl.GL_TRIANGLES, Gl.GL_TRIANGLE_STRIP, Gl.GL_TRIANGLE_FAN, Gl.GL_QUADS };

		private void DrawPrimitive( uint listBase, uint argi )
		{
			int vertexCount = ( int )( argi & 0xFFFF );
			if( vertexCount == 0 )
				return;
			int primitiveType = PrimitiveMap[ ( int )( ( argi >> 16 ) & 0x7 ) ];

			uint t = _ctx.Values[ ( int )VideoCommand.VTYPE ];
			bool isTransformed = ( t >> 23 ) == 0;
			uint boneCount = ( t >> 14 ) & 0x3;
			uint morphCount = ( t >> 18 ) & 0x3;
			uint vertexType = t & 0x00801FFF;
			bool isIndexed = ( vertexType & ( VertexType.Index8 | VertexType.Index16 ) ) != 0;

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;
			uint iaddr = _ctx.Values[ ( int )VideoCommand.IADDR ] | listBase;

			this.UpdateState( StateRequest.Drawing );

			// Setup textures

			// Draw
		}

		private void DrawBezier( uint listBase, uint argi )
		{
			uint t = _ctx.Values[ ( int )VideoCommand.VTYPE ];
			bool isTransformed = ( t >> 23 ) == 0;
			uint vertexType = t & 0x00801FFF;
			bool isIndexed = ( vertexType & ( VertexType.Index8 | VertexType.Index16 ) ) != 0;

			t = _ctx.Values[ ( int )VideoCommand.PSUB ];
			uint divS = t & 0xFF;
			uint divT = ( t >> 8 ) & 0xFF;
			bool clockwise = ( _ctx.Values[ ( int )VideoCommand.PFACE ] & 0x1 ) == 0;

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;
			uint iaddr = _ctx.Values[ ( int )VideoCommand.IADDR ] | listBase;

			this.UpdateState( StateRequest.Drawing );

			// Setup textures

			// Draw
		}

		private void DrawSpline( uint listBase, uint argi )
		{
		}

		private static uint DetermineVertexSize( uint vertexType )
		{
			// Kindly yoinked (with permission) from ector's DaSh

			uint positionType = ( vertexType & VertexType.PositionMask ) >> 7;
			uint normalType = ( vertexType & VertexType.NormalMask ) >> 5;
			uint textureType = ( vertexType & VertexType.TextureMask );
			uint weightCount = ( vertexType & VertexType.WeightCountMask ) >> 14;
			uint weightType = ( vertexType & VertexType.WeightMask ) >> 9;
			uint colorType = ( vertexType & VertexType.ColorMask ) >> 2;
			uint morphCount = ( vertexType & VertexType.MorphCountMask ) >> 18;

			uint size = 0;
			uint biggest = 0;
			GetVertexComponentSize( v_weightSizes, v_weightAlign, weightType, weightCount, ref biggest, ref size ); // WRONG - count?
			GetVertexComponentSize( v_textureSizes, v_textureAlign, textureType, 1, ref biggest, ref size );
			GetVertexComponentSize( v_colorSizes, v_colorAlign, colorType, 1, ref biggest, ref size );
			GetVertexComponentSize( v_normalSizes, v_normalAlign, normalType, 1, ref biggest, ref size );
			GetVertexComponentSize( v_positionSizes, v_positionAlign, positionType, 1, ref biggest, ref size );

			// do something with morph count?
			Debug.Assert( morphCount == 0 );

			return MemorySystem.Align( size, biggest );
		}

		private static readonly uint[] v_positionSizes = new uint[] { 0, 3, 6, 12 }, v_positionAlign = new uint[] { 0, 1, 2, 4 };
		private static readonly uint[] v_normalSizes = new uint[] { 0, 3, 6, 12 }, v_normalAlign = new uint[] { 0, 1, 2, 4 };
		private static readonly uint[] v_textureSizes = new uint[] { 0, 2, 4, 8 }, v_textureAlign = new uint[] { 0, 1, 2, 4 };
		private static readonly uint[] v_weightSizes = new uint[] { 0, 1, 2, 4 }, v_weightAlign = new uint[] { 0, 1, 2, 4 };
		private static readonly uint[] v_colorSizes = new uint[] { 0, 0, 0, 0, 2, 2, 2, 4 }, v_colorAlign = new uint[] { 0, 0, 0, 0, 2, 2, 2, 4 };

		private static void GetVertexComponentSize( uint[] sizes, uint[] aligns, uint type, uint count, ref uint biggest, ref uint size )
		{
			if( sizes[ type ] != 0 )
			{
				size = MemorySystem.Align( size, aligns[ type ] );
				size += sizes[ type ] * count;
				if( aligns[ type ] > biggest )
					biggest = aligns[ type ];
			}
		}
	}
}
