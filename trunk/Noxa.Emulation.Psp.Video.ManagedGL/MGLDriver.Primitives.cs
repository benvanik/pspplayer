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

		private byte[] _colorBuffer = new byte[ 1024 * 10 * 4 ];
		private byte[] _spriteBuffer = new byte[ 1024 * 10 * 16 ];

		private void DrawPrimitive( uint listBase, uint argi )
		{
			int vertexCount = ( int )( argi & 0xFFFF );
			if( vertexCount == 0 )
				return;
			int primitiveType = PrimitiveMap[ ( int )( ( argi >> 16 ) & 0x7 ) ];

			uint t = _ctx.Values[ ( int )VideoCommand.VTYPE ];
			uint boneCount = ( ( t >> 14 ) & 0x3 ) + 1;
			uint morphCount = ( t >> 18 ) & 0x3;
			if( morphCount > 0 )
				morphCount++;
			if( morphCount > 1 )
				return;
			uint vertexType = t & 0x00801FFF;
			bool isIndexed = ( vertexType & ( VertexType.Index8 | VertexType.Index16 ) ) != 0;
			bool isTransformed = ( vertexType & VertexType.TransformedMask ) != 0;
			uint alignmentDelta;
			int vertexSize = ( int )DetermineVertexSize( vertexType, out alignmentDelta );

			uint vaddr = _ctx.Values[ ( int )VideoCommand.VADDR ] | listBase;
			uint iaddr = _ctx.Values[ ( int )VideoCommand.IADDR ] | listBase;

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

			// Setup shader
			// TODO: choose the right one
			this.SetDefaultProgram( isTransformed, colorType, boneCount, morphCount );

			// Setup state
			uint arrayState = ArrayState.VertexArrayMask;
			if( normalType != 0 )
				arrayState |= ArrayState.NormalArrayMask;
			if( colorType != 0 )
				arrayState |= ArrayState.ColorArrayMask;
			if( textureType != 0 )
				arrayState |= ArrayState.TextureCoordArrayMask;
			this.EnableArrays( arrayState );

			// Real work
			fixed( byte* colorBuffer = &_colorBuffer[ 0 ] )
			fixed( byte* spriteBuffer = &_spriteBuffer[ 0 ] )
			{
				byte* vertexBuffer = this.MemorySystem.Translate( vaddr );
				if( primitiveType == 7 )
				{
					this.BuildSpriteList( vertexCount, vertexType, vertexSize, vertexBuffer, spriteBuffer );
					vertexBuffer = spriteBuffer;
					vertexCount *= 2; // adding 2x the vertices
				}

				byte* src = vertexBuffer + alignmentDelta;
				switch( weightType )
				{
					case VertexType.WeightFixed8:
						src += 1 * boneCount;
						break;
					case VertexType.WeightFixed16:
						src += 2 * boneCount;
						break;
					case VertexType.WeightFloat:
						src += 4 * boneCount;
						break;
				}
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
						//Random rr = new Random();
						//Gl.glColor4f( ( float )rr.NextDouble(), ( float )rr.NextDouble(), ( float )rr.NextDouble(), 1.0f );
						//Gl.glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );
						Gl.glColor4fv( _ctx.AmbientModelColor );
						break;
					case VertexType.ColorBGR5650:
						// TODO: nice way of doing this - in the shader?
						Debug.Assert( false );
						src += 2;
						break;
					case VertexType.ColorABGR4444:
						// TODO: nice way of doing this - in the shader?
						{
							uint* dp = ( uint* )colorBuffer;
							byte* sp = src;
							for( int n = 0; n < vertexCount; n++ )
							{
								uint entry = ( uint )*( ( ushort* )sp );
								*dp = ( ( entry & 0xF ) * 16 ) | ( ( ( ( entry >> 4 ) & 0xF ) * 16 ) << 8 ) | ( ( ( ( entry >> 8 ) & 0xF ) * 16 ) << 16 ) | ( ( ( ( entry >> 12 ) & 0xF ) * 16 ) << 24 );
								sp += vertexSize;
								dp++;
							}
							Gl.glColorPointer( 4, Gl.GL_UNSIGNED_BYTE, 0, ( IntPtr )colorBuffer );
						}
						src += 2;
						break;
					case VertexType.ColorABGR5551:
						// TODO: nice way of doing this - in the shader?
						{
							uint* dp = ( uint* )colorBuffer;
							byte* sp = src;
							for( int n = 0; n < vertexCount; n++ )
							{
								uint entry = ( uint )*( ( ushort* )sp );
								*dp = ( ( entry & 0x1 ) * 256 ) | ( ( ( ( entry >> 1 ) & 0x1F ) * 8 ) << 8 ) | ( ( ( ( entry >> 6 ) & 0x1F ) * 8 ) << 16 ) | ( ( ( ( entry >> 11 ) & 0x1F ) * 8 ) << 24 );
								sp += vertexSize;
								dp++;
							}
							Gl.glColorPointer( 4, Gl.GL_UNSIGNED_BYTE, 0, ( IntPtr )colorBuffer );
						}
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
			}
		}

		private void BuildSpriteList( int vertexCount, uint vertexType, int vertexSize, byte* vertexBuffer, byte* spriteBuffer )
		{
			// Sprite lists contain 2*n vertices for n sprites
			// Each sprite has 2 vertices, the first being the top left corner, and the second being
			// the bottom right

			// I don't think these are supported
			Debug.Assert( ( vertexType & VertexType.NormalMask ) == 0 );
			Debug.Assert( ( vertexType & VertexType.WeightMask ) == 0 );

			// TODO: REMOVE THIS
			//Gl.glPushAttrib( Gl.GL_ENABLE_BIT );
			//Gl.glDisable( Gl.GL_DEPTH_TEST );
			//Gl.glDepthMask( Gl.GL_FALSE );
			//Gl.glEnable( Gl.GL_CLAMP_TO_EDGE );
			//// ...
			//Gl.glDepthMask( Gl.GL_TRUE );
			//Gl.glPopAttrib();

			uint textureType = ( vertexType & VertexType.TextureMask );
			uint colorType = ( vertexType & VertexType.ColorMask );
			uint positionType = ( vertexType & VertexType.PositionMask );

			byte* sp = vertexBuffer;
			byte* dp = spriteBuffer;
			for( int n = 0; n < vertexCount / 2; n += 2 )
			{
				byte* sv1 = sp;
				byte* sv2 = sv1 + vertexSize;
				byte* dv1 = dp;
				byte* dv2 = dv1 + vertexSize;
				byte* dv3 = dv2 + vertexSize;
				byte* dv4 = dv3 + vertexSize;
				if( textureType > 0 )
				{
					switch( textureType )
					{
						// +0=s, +1=t
						case VertexType.TextureFixed8:
							*( ushort* )dv1 = *( ushort* )sv1;
							*( dv2 + 0 ) = *( sv2 + 0 );
							*( dv2 + 1 ) = *( sv1 + 1 );
							*( ushort* )dv3 = *( ushort* )sv2;
							*( dv4 + 0 ) = *( sv1 + 0 );
							*( dv4 + 1 ) = *( sv2 + 1 );
							sp += 2;
							dp += 2 * 4;
							break;
						case VertexType.TextureFixed16:
							*( uint* )dv1 = *( uint* )sv1;
							*( ( ushort* )dv2 + 0 ) = *( ( ushort* )sv2 + 0 );
							*( ( ushort* )dv2 + 1 ) = *( ( ushort* )sv1 + 1 );
							*( uint* )dv3 = *( uint* )sv2;
							*( ( ushort* )dv4 + 0 ) = *( ( ushort* )sv1 + 0 );
							*( ( ushort* )dv4 + 1 ) = *( ( ushort* )sv2 + 1 );
							sp += 4;
							dp += 4 * 4;
							break;
						case VertexType.TextureFloat:
							*( ulong* )dv1 = *( ulong* )sv1;
							*( ( uint* )dv2 + 0 ) = *( ( uint* )sv2 + 0 );
							*( ( uint* )dv2 + 1 ) = *( ( uint* )sv1 + 1 );
							*( ulong* )dv3 = *( ulong* )sv2;
							*( ( uint* )dv4 + 0 ) = *( ( uint* )sv1 + 0 );
							*( ( uint* )dv4 + 1 ) = *( ( uint* )sv2 + 1 );
							sp += 8;
							dp += 8 * 4;
							break;
					}
					sv1 += vertexSize * 2;
					sv2 = sv1 + vertexSize;
					dv1 += vertexSize * 4;
					dv2 = dv1 + vertexSize;
					dv3 = dv2 + vertexSize;
					dv4 = dv3 + vertexSize;
				}
				if( colorType > 0 )
				{
					switch( colorType )
					{
						case VertexType.ColorBGR5650:
						case VertexType.ColorABGR4444:
						case VertexType.ColorABGR5551:
							*( ( ushort* )dv1 ) = *( ( ushort* )sv2 );
							*( ( ushort* )dv2 ) = *( ( ushort* )sv2 );
							*( ( ushort* )dv3 ) = *( ( ushort* )sv2 );
							*( ( ushort* )dv4 ) = *( ( ushort* )sv2 );
							sp += 2;
							dp += 2 * 4;
							break;
						case VertexType.ColorABGR8888:
							*( ( uint* )dv1 ) = *( ( uint* )sv2 );
							*( ( uint* )dv2 ) = *( ( uint* )sv2 );
							*( ( uint* )dv3 ) = *( ( uint* )sv2 );
							*( ( uint* )dv4 ) = *( ( uint* )sv2 );
							sp += 4;
							dp += 4 * 4;
							break;
					}
					sv1 += vertexSize * 2;
					sv2 = sv1 + vertexSize;
					dv1 += vertexSize * 4;
					dv2 = dv1 + vertexSize;
					dv3 = dv2 + vertexSize;
					dv4 = dv3 + vertexSize;
				}
				switch( positionType )
				{
					// v1.x,v1.y,v2.z
					// v2.x,v1.y,v2.z
					// v2.x,v2.y,v2.z
					// v1.x,v2.y,v2.z
					// +0=x, +1=y, +2=z
					case VertexType.PositionFixed8:
						*( dv1 + 0 ) = *( sv1 + 0 );
						*( dv1 + 1 ) = *( sv1 + 1 );
						*( dv2 + 0 ) = *( sv2 + 0 );
						*( dv2 + 1 ) = *( sv1 + 1 );
						*( dv3 + 0 ) = *( sv2 + 0 );
						*( dv3 + 1 ) = *( sv2 + 1 );
						*( dv4 + 0 ) = *( sv1 + 0 );
						*( dv4 + 1 ) = *( sv2 + 1 );
						// Depth comes from vertex 2
						*( dv1 + 2 ) = *( dv2 + 2 ) = *( dv3 + 2 ) = *( dv4 + 2 ) = *( sv2 + 2 );
						sp += 3;
						dp += 3 * 4;
						break;
					case VertexType.PositionFixed16:
						*( ( ushort* )dv1 + 0 ) = *( ( ushort* )sv1 + 0 );
						*( ( ushort* )dv1 + 1 ) = *( ( ushort* )sv1 + 1 );
						*( ( ushort* )dv2 + 0 ) = *( ( ushort* )sv2 + 0 );
						*( ( ushort* )dv2 + 1 ) = *( ( ushort* )sv1 + 1 );
						*( ( ushort* )dv3 + 0 ) = *( ( ushort* )sv2 + 0 );
						*( ( ushort* )dv3 + 1 ) = *( ( ushort* )sv2 + 1 );
						*( ( ushort* )dv4 + 0 ) = *( ( ushort* )sv1 + 0 );
						*( ( ushort* )dv4 + 1 ) = *( ( ushort* )sv2 + 1 );
						// Depth comes from vertex 2
						*( ( ushort* )dv1 + 2 ) = *( ( ushort* )dv2 + 2 ) = *( ( ushort* )dv3 + 2 ) = *( ( ushort* )dv4 + 2 ) = *( ( ushort* )sv2 + 2 );
						sp += 6;
						dp += 6 * 4;
						break;
					case VertexType.PositionFloat:
						*( ( uint* )dv1 + 0 ) = *( ( uint* )sv1 + 0 );
						*( ( uint* )dv1 + 1 ) = *( ( uint* )sv1 + 1 );
						*( ( uint* )dv2 + 0 ) = *( ( uint* )sv2 + 0 );
						*( ( uint* )dv2 + 1 ) = *( ( uint* )sv1 + 1 );
						*( ( uint* )dv3 + 0 ) = *( ( uint* )sv2 + 0 );
						*( ( uint* )dv3 + 1 ) = *( ( uint* )sv2 + 1 );
						*( ( uint* )dv4 + 0 ) = *( ( uint* )sv1 + 0 );
						*( ( uint* )dv4 + 1 ) = *( ( uint* )sv2 + 1 );
						// Depth comes from vertex 2
						*( ( uint* )dv1 + 2 ) = *( ( uint* )dv2 + 2 ) = *( ( uint* )dv3 + 2 ) = *( ( uint* )dv4 + 2 ) = *( ( uint* )sv2 + 2 );
						sp += 12;
						dp += 12 * 4;
						break;
				}
			}
		}

		private static uint DetermineVertexSize( uint vertexType, out uint alignmentDelta )
		{
			// Kindly yoinked (with permission) from ector's DaSh

			uint positionType = ( vertexType & VertexType.PositionMask ) >> 7;
			uint normalType = ( vertexType & VertexType.NormalMask ) >> 5;
			uint textureType = ( vertexType & VertexType.TextureMask );
			uint weightCount = ( ( vertexType & VertexType.WeightCountMask ) >> 14 ) + 1;
			uint weightType = ( vertexType & VertexType.WeightMask ) >> 9;
			uint colorType = ( vertexType & VertexType.ColorMask ) >> 2;
			uint morphCount = ( vertexType & VertexType.MorphCountMask ) >> 18;

			uint size = 0;
			uint biggest = 0;
			if( weightType > 0 )
				GetVertexComponentSize( v_weightSizes, v_weightAlign, weightType, weightCount, ref biggest, ref size ); // WRONG - count?
			if( textureType > 0 )
				GetVertexComponentSize( v_textureSizes, v_textureAlign, textureType, 1, ref biggest, ref size );
			if( colorType > 0 )
				GetVertexComponentSize( v_colorSizes, v_colorAlign, colorType, 1, ref biggest, ref size );
			if( normalType > 0 )
				GetVertexComponentSize( v_normalSizes, v_normalAlign, normalType, 1, ref biggest, ref size );
			GetVertexComponentSize( v_positionSizes, v_positionAlign, positionType, 1, ref biggest, ref size );

			// do something with morph count?
			Debug.Assert( morphCount == 0 );

			uint result = MemorySystem.Align( size, biggest );
			alignmentDelta = result - size;
			return result;
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
