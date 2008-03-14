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
using System.Collections;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		enum StateRequest
		{
			All,
			Drawing,
		}

		private static class StateField
		{
			public const int VertexArray = 0;
			public const int NormalArray = 1;
			public const int TextureCoordArray = 2;
			public const int ColorArray = 3;
		}
		private BitArray _glEnabledState = new BitArray( 32 );
		private bool _matricesValid;
		private int _currentProgramId;

		private void UpdateState( StateRequest request )
		{
			// Matrix update
			if( _matricesValid == false )
			{
				// Upload proj/view only - world goes to the shaders
				Gl.glMatrixMode( Gl.GL_PROJECTION );
				fixed( float* p = &_ctx.ProjectionMatrix[ 0 ] )
					Gl.glLoadMatrixf( ( IntPtr )p );
				Gl.glMatrixMode( Gl.GL_MODELVIEW );
				fixed( float* p = &_ctx.ViewMatrix[ 0 ] )
					Gl.glLoadMatrixf( ( IntPtr )p );

				// ?
				Gl.glMatrixMode( Gl.GL_TEXTURE );
				fixed( float* p = &_ctx.TextureMatrix[ 0 ] )
					Gl.glLoadMatrixf( ( IntPtr )p );

				_matricesValid = true;
			}
		}

		private void InvalidateMatrices()
		{
			_matricesValid = false;
		}

		private void EnableArrays( bool vertex, bool normal, bool color, bool texture )
		{
			if( _glEnabledState[ StateField.VertexArray ] != vertex )
			{
				if( vertex == true )
					Gl.glEnableClientState( Gl.GL_VERTEX_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_VERTEX_ARRAY );
				_glEnabledState[ StateField.VertexArray ] = vertex;
			}
			if( _glEnabledState[ StateField.NormalArray ] != normal )
			{
				if( normal == true )
					Gl.glEnableClientState( Gl.GL_NORMAL_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_NORMAL_ARRAY );
				_glEnabledState[ StateField.NormalArray ] = normal;
			}
			if( _glEnabledState[ StateField.TextureCoordArray ] != texture )
			{
				if( texture == true )
					Gl.glEnableClientState( Gl.GL_TEXTURE_COORD_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_TEXTURE_COORD_ARRAY );
				_glEnabledState[ StateField.TextureCoordArray ] = texture;
			}
			if( _glEnabledState[ StateField.ColorArray ] != color )
			{
				if( color == true )
					Gl.glEnableClientState( Gl.GL_COLOR_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_COLOR_ARRAY );
				_glEnabledState[ StateField.ColorArray ] = color;
			}
		}

		private void SetNoProgram()
		{
			if( _currentProgramId != 0 )
				Gl.glUseProgram( 0 );
			_currentProgramId = 0;
		}

		private void SetDefaultProgram( bool isTransformed, uint boneCount, uint morphCount )
		{
			if( _currentProgramId != _defaultProgram.ProgramID )
				Gl.glUseProgram( _defaultProgram.ProgramID );
			_currentProgramId = _defaultProgram.ProgramID;
			_defaultProgram.Setup( _ctx, isTransformed, boneCount, morphCount );
		}
	}
}
