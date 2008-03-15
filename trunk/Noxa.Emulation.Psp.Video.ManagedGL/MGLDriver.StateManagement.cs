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

		private static class FeatureState
		{
			public const int CullFace = 0;
			public const int DepthTest = 1;
			public const int AlphaTest = 2;
			public const int Fog = 3;
			public const int AlphaBlend = 4;

			public const uint CullFaceMask = 0x1 << CullFace;
			public const uint DepthTestMask = 0x1 << DepthTest;
			public const uint AlphaTestMask = 0x1 << AlphaTest;
			public const uint FogMask = 0x1 << Fog;
			public const uint AlphaBlendMask = 0x1 << AlphaBlend;
		}

		private static class ArrayState
		{
			public const int VertexArray = 0;
			public const int NormalArray = 1;
			public const int TextureCoordArray = 2;
			public const int ColorArray = 3;

			public const uint VertexArrayMask = 0x1 << VertexArray;
			public const uint NormalArrayMask = 0x1 << NormalArray;
			public const uint TextureCoordArrayMask = 0x1 << TextureCoordArray;
			public const uint ColorArrayMask = 0x1 << ColorArray;
		}

		private uint _featureStateValue;
		private uint _arrayStateValue;

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

			// TODO: Do this for all programs
			_defaultProgram.IsDirty = true;
		}

		private void SetState( uint mask, uint values )
		{
			uint diff = ( _featureStateValue & mask ) ^ values;
			if( diff == 0 )
				return;

			if( ( diff & FeatureState.CullFaceMask ) != 0 )
			{
				if( ( values & FeatureState.CullFaceMask ) != 0 )
					Gl.glEnable( Gl.GL_CULL_FACE );
				else
					Gl.glDisable( Gl.GL_CULL_FACE );
			}
			if( ( diff & FeatureState.DepthTestMask ) != 0 )
			{
				if( ( values & FeatureState.DepthTestMask ) != 0 )
					Gl.glEnable( Gl.GL_DEPTH_TEST );
				else
					Gl.glDisable( Gl.GL_DEPTH_TEST );
			}
			if( ( diff & FeatureState.AlphaTestMask ) != 0 )
			{
				if( ( values & FeatureState.AlphaTestMask ) != 0 )
					Gl.glEnable( Gl.GL_ALPHA_TEST );
				else
					Gl.glDisable( Gl.GL_ALPHA_TEST );
			}
			if( ( diff & FeatureState.FogMask ) != 0 )
			{
				if( ( values & FeatureState.FogMask ) != 0 )
					Gl.glEnable( Gl.GL_FOG );
				else
					Gl.glDisable( Gl.GL_FOG );
			}
			if( ( diff & FeatureState.AlphaBlendMask ) != 0 )
			{
				if( ( values & FeatureState.AlphaBlendMask ) != 0 )
					Gl.glEnable( Gl.GL_BLEND );
				else
					Gl.glDisable( Gl.GL_BLEND );
			}
			
			_featureStateValue = ( _featureStateValue & ~mask ) | values;
		}

		private void EnableArrays( uint values )
		{
			if( ( _arrayStateValue & ArrayState.VertexArrayMask ) != ( values & ArrayState.VertexArrayMask ) )
			{
				if( ( values & ArrayState.VertexArrayMask ) != 0 )
					Gl.glEnableClientState( Gl.GL_VERTEX_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_VERTEX_ARRAY );
			}
			if( ( _arrayStateValue & ArrayState.NormalArrayMask ) != ( values & ArrayState.NormalArrayMask ) )
			{
				if( ( values & ArrayState.NormalArrayMask ) != 0 )
					Gl.glEnableClientState( Gl.GL_NORMAL_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_NORMAL_ARRAY );
			}
			if( ( _arrayStateValue & ArrayState.TextureCoordArrayMask ) != ( values & ArrayState.TextureCoordArrayMask ) )
			{
				if( ( values & ArrayState.TextureCoordArrayMask ) != 0 )
					Gl.glEnableClientState( Gl.GL_TEXTURE_COORD_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_TEXTURE_COORD_ARRAY );
			}
			if( ( _arrayStateValue & ArrayState.ColorArrayMask ) != ( values & ArrayState.ColorArrayMask ) )
			{
				if( ( values & ArrayState.ColorArrayMask ) != 0 )
					Gl.glEnableClientState( Gl.GL_COLOR_ARRAY );
				else
					Gl.glDisableClientState( Gl.GL_COLOR_ARRAY );
			}
			_arrayStateValue = values;
		}

		private void SetNoProgram()
		{
			if( _currentProgramId != 0 )
				Gl.glUseProgram( 0 );
			_currentProgramId = 0;
		}

		private void SetDefaultProgram( bool isTransformed, uint colorType, uint boneCount, uint morphCount )
		{
			if( _currentProgramId != _defaultProgram.ProgramID )
				Gl.glUseProgram( _defaultProgram.ProgramID );
			_currentProgramId = _defaultProgram.ProgramID;
			_defaultProgram.Setup( _ctx, isTransformed, colorType, boneCount, morphCount );
		}
	}
}
