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

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		private void TransferTexture( uint argi )
		{
			// pixelSize: 0 = 16 bit, 1 = 32 bit
			uint pixelSize = argi & 0x1;
			uint t;

			t = _ctx.Values[ ( int )VideoCommand.TRXSBW ];
			uint sourceAddress = ( _ctx.Values[ ( int )VideoCommand.TRXSBP ] & 0x0000FFF0 ) | ( ( t << 8 ) & 0xFF000000 );
			int sourceLineWidth = ( int )( t & 0x0000FFF8 );
			t = _ctx.Values[ ( int )VideoCommand.TRXDBW ];
			uint destinationAddress = ( _ctx.Values[ ( int )VideoCommand.TRXDBP ] & 0x0000FFF0 ) | ( ( t << 8 ) & 0xFF000000 );
			int destinationLineWidth = ( int )( t & 0x0000FFF8 );

			// Happens sometimes
			if( sourceAddress == 0 )
				return;

			t = _ctx.Values[ ( int )VideoCommand.TRXSIZE ];
			int width = ( int )( ( t & 0x3FF ) + 1 );
			int height = ( int )( ( ( t >> 10 ) & 0x1FF ) + 1 );

			t = _ctx.Values[ ( int )VideoCommand.TRXSPOS ];
			int sx = ( int )( t & 0x1FF );
			int sy = ( int )( ( t >> 10 ) & 0x1FF );
			t = _ctx.Values[ ( int )VideoCommand.TRXDPOS ];
			int dx = ( int )( t & 0x1FF );
			int dy = ( int )( ( t >> 10 ) & 0x1FF );

			// NOTE: we only support sources of 0,0 and the width * size must = line width!
			Debug.Assert( sx == 0 );
			Debug.Assert( sy == 0 );
			Debug.Assert( sourceLineWidth == width );

			int bpp = ( pixelSize == 0 ) ? 2 : 4;
			byte* buffer = this.MemorySystem.Translate( sourceAddress );

			Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR );
			Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR );
			Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP );
			Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP );

			Gl.glPushAttrib( Gl.GL_ENABLE_BIT );
			Gl.glDisable( Gl.GL_TEXTURE_1D );
			Gl.glDisable( Gl.GL_DEPTH_TEST );
			Gl.glDisable( Gl.GL_BLEND );
			Gl.glDisable( Gl.GL_ALPHA_TEST );
			Gl.glDisable( Gl.GL_FOG );
			Gl.glDisable( Gl.GL_LIGHTING );
			Gl.glDisable( Gl.GL_LOGIC_OP );
			Gl.glDisable( Gl.GL_STENCIL_TEST );
			Gl.glDisable( Gl.GL_CULL_FACE );
			Gl.glDepthMask( Gl.GL_FALSE );
			Gl.glDepthFunc( Gl.GL_ALWAYS );

			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix();
			Gl.glLoadIdentity();
			Gl.glOrtho( 0.0f, 480.0f, 0.0f, 272.0f, -1.0f, 1.0f );
			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix();
			Gl.glLoadIdentity();

			Gl.glPixelStorei( Gl.GL_UNPACK_ALIGNMENT, bpp );
			Gl.glPixelStorei( Gl.GL_UNPACK_ROW_LENGTH, sourceLineWidth );

			Gl.glEnable( Gl.GL_TEXTURE_2D );
			//uint texId;
			//Gl.glGenTextures( 1, &texId );
			//Gl.glBindTexture( Gl.GL_TEXTURE_2D, texId );
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, 0 );
			if( bpp == 4 )
			{
				Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA,
					width, height,
					0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE,
					new IntPtr( buffer ) );
			}
			else
			{
				// Not supported
				Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB5,
					width, height,
					0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE,
					new IntPtr( buffer ) );
			}

			// 0 ---- 1
			// |      |
			// |      |
			// 3 ---- 2
			// Note I switched around the t coords cause OGL is retarded and has a weird origin

			Gl.glBegin( Gl.GL_QUADS );
			Gl.glColor4ub( 255, 255, 255, 255 );
			Gl.glTexCoord2f( 0.0f, 1.0f );
			Gl.glVertex2i( dx, dy );
			Gl.glTexCoord2f( 1.0f, 1.0f );
			Gl.glVertex2i( dx + width, dy );
			Gl.glTexCoord2f( 1.0f, 0.0f );
			Gl.glVertex2i( dx + width, dy + height );
			Gl.glTexCoord2f( 0.0f, 0.0f );
			Gl.glVertex2i( dx, dy + height );
			Gl.glEnd();

			//Gl.glDeleteTextures( 1, &texId );

			Gl.glPopMatrix();
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPopMatrix();

			Gl.glPopAttrib();
			//Gl.glDepthMask( Gl.GL_TRUE );
		}
	}
}
