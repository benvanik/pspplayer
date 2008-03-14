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

		private uint[] _colorBuffer = new uint[ 1024 * 10 ];
		private float[] _textureCoordBuffer = new float[ 1024 * 10 ];

		private void DrawPrimitive( uint listBase, uint argi )
		{
			int vertexCount = ( int )( argi & 0xFFFF );
			if( vertexCount == 0 )
				return;
			int primitiveType = PrimitiveMap[ ( int )( ( argi >> 16 ) & 0x7 ) ];
			if( primitiveType == 7 )
				return;

			Gl.glDisable( Gl.GL_CULL_FACE );
			Gl.glDisable( Gl.GL_ALPHA_TEST );
			Gl.glDisable( Gl.GL_DEPTH_TEST );
			Gl.glDisable( Gl.GL_BLEND );
			//Gl.glPushAttrib( Gl.GL_ENABLE_BIT );
			Gl.glDisable( Gl.GL_CULL_FACE );
			Gl.glDisable( Gl.GL_LIGHTING );

			uint t = _ctx.Values[ ( int )VideoCommand.VTYPE ];
			uint boneCount = ( t >> 14 ) & 0x3;
			uint morphCount = ( t >> 18 ) & 0x3;
			uint vertexType = t & 0x00801FFF;
			bool isIndexed = ( vertexType & ( VertexType.Index8 | VertexType.Index16 ) ) != 0;
			bool isTransformed = ( vertexType & VertexType.TransformedMask ) != 0;
			int vertexSize = ( int )DetermineVertexSize( vertexType );

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;
			uint iaddr = _ctx.Values[ ( int )VideoCommand.IADDR ] | listBase;

			this.UpdateState( StateRequest.Drawing );

			// Setup textures

			uint positionType = ( vertexType & VertexType.PositionMask );
			uint normalType = ( vertexType & VertexType.NormalMask );
			uint textureType = ( vertexType & VertexType.TextureMask );
			uint colorType = ( vertexType & VertexType.ColorMask );
			uint weightType = ( vertexType & VertexType.WeightMask );

			// Setup shader
			// TODO: choose the right one
			this.SetDefaultProgram( isTransformed, boneCount, morphCount );

			// Setup state
			this.EnableArrays( true, ( normalType != 0 ), ( colorType != 0 ), ( textureType != 0 ) );

			byte* vertexBuffer = this.MemorySystem.Translate( vaddr );
			byte* src = vertexBuffer;
			switch( textureType )
			{
				case VertexType.TextureFixed8:
					Debug.Assert( false );
					src += 2;
					break;
				case VertexType.TextureFixed16:
					Gl.glTexCoordPointer( 2, Gl.GL_SHORT, vertexSize, ( IntPtr )src );
					src += 4;
					break;
				case VertexType.TextureFloat:
					Gl.glTexCoordPointer( 4, Gl.GL_FLOAT, vertexSize, ( IntPtr )src );
					src += 8;
					break;
			}
			switch( colorType )
			{
				case 0:
					Gl.glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );
					break;
				case VertexType.ColorBGR5650:
					// TODO: nice way of doing this - in the shader?
					Debug.Assert( false );
					src += 2;
					break;
				case VertexType.ColorABGR4444:
					// TODO: nice way of doing this - in the shader?
					Gl.glColor4f( 1.0f, 0.0f, 1.0f, 1.0f );
					src += 2;
					break;
				case VertexType.ColorABGR5551:
					// TODO: nice way of doing this - in the shader?
					Gl.glColor4f( 1.0f, 1.0f, 0.0f, 1.0f );
					src += 2;
					break;
				case VertexType.ColorABGR8888:
					Gl.glColorPointer( 4, Gl.GL_UNSIGNED_BYTE, vertexSize, ( IntPtr )src );
					src += 4;
					break;
			}
			switch( normalType )
			{
				case VertexType.NormalFixed8:
					Gl.glNormalPointer( Gl.GL_BYTE, vertexSize, ( IntPtr )src );
					src += 3;
					break;
				case VertexType.NormalFixed16:
					Gl.glNormalPointer( Gl.GL_SHORT, vertexSize, ( IntPtr )src );
					src += 6;
					break;
				case VertexType.NormalFloat:
					Gl.glNormalPointer( Gl.GL_FLOAT, vertexSize, ( IntPtr )src );
					src += 12;
					break;
			}
			switch( positionType )
			{
				case VertexType.PositionFixed8:
					Debug.Assert( false );
					Gl.glVertexPointer( 3, Gl.GL_BYTE, vertexSize, ( IntPtr )src ); // This may not work!
					src += 3;
					break;
				case VertexType.PositionFixed16:
					Gl.glVertexPointer( 3, Gl.GL_SHORT, vertexSize, ( IntPtr )src );
					src += 6;
					break;
				case VertexType.PositionFloat:
					Gl.glVertexPointer( 3, Gl.GL_FLOAT, vertexSize, ( IntPtr )src );
					src += 12;
					break;
			}

			// TODO: Ortho projection if isTransformed
			if( isTransformed == true )
			{
				Gl.glPushAttrib( Gl.GL_ENABLE_BIT );
				Gl.glDisable( Gl.GL_CULL_FACE );
			}

			if( isIndexed == false )
			{
				Gl.glDrawArrays( primitiveType, 0, vertexCount );
			}
			else
			{
				byte* indexBuffer = this.MemorySystem.Translate( iaddr );
				if( ( vertexType & VertexType.Index8 ) != 0 )
					Gl.glDrawElements( primitiveType, vertexCount, Gl.GL_UNSIGNED_BYTE, ( IntPtr )indexBuffer );
				else if( ( vertexType & VertexType.Index16 ) != 0 )
					Gl.glDrawElements( primitiveType, vertexCount, Gl.GL_UNSIGNED_SHORT, ( IntPtr )indexBuffer );
			}

			// TODO: restore matrices
			if( isTransformed == true )
			{
				Gl.glPopAttrib();
			}
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
