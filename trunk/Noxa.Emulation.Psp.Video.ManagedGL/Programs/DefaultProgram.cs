// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tao.OpenGl;

namespace Noxa.Emulation.Psp.Video.ManagedGL.Programs
{
	unsafe class DefaultProgram : GLProgram
	{
		private int _vert_worldMatrix;

		private int _vert_isTransformed;
		private int _vert_isRaw;

		private int _vert_textureSize;
		private int _vert_textureOffset;
		private int _vert_textureScale;

		private int _vert_isSkinned;
		private int _vert_boneMatrices;
		private int _vert_boneWeights03;
		private int _vert_boneWeights47;

		private int _vert_morphCount;
		private int _vert_morphWeights;

		private int _vert_lightingEnabled;
		private int _vert_lightEnabled;
		private int _vert_lightParameters;
		private int _vert_lightPositions;
		private int _vert_lightDirections;
		private int _vert_lightColors;

		private int _frag_textureEnabled;
		private int _frag_texture;
		private int _frag_textureFormat;

		// Cache of values... glUniform isn't *fast*, so cache anything we expect to change infrequently
		private bool _isTransformed;

		public DefaultProgram()
		{
			string vertexShader = File.ReadAllText( "Shaders/Default.vert" );
			string fragmentShader = File.ReadAllText( "Shaders/Default.frag" );
			bool loaded = this.Load( vertexShader, fragmentShader );
			Debug.Assert( loaded == true );
			if( loaded == false )
				return;
		}

		protected override bool OnPrepare()
		{
			_vert_worldMatrix = this.GetUniform( "worldMatrix" );

			_vert_isTransformed = this.GetUniform( "isTransformed" );
			_vert_isRaw = this.GetUniform( "isRaw" );

			_vert_textureSize = this.GetUniform( "textureSize" );
			_vert_textureOffset = this.GetUniform( "textureOffset" );
			_vert_textureScale = this.GetUniform( "textureScale" );

			_vert_isSkinned = this.GetUniform( "isSkinned" );
			_vert_boneMatrices = this.GetUniform( "boneMatrices" );
			_vert_boneWeights03 = this.GetAttribute( "boneWeights03" );
			_vert_boneWeights47 = this.GetAttribute( "boneWeights47" );

			_vert_morphCount = this.GetUniform( "morphCount" );
			_vert_morphWeights = this.GetUniform( "morphWeights" );

			_vert_lightingEnabled = this.GetUniform( "lightingEnabled" );
			_vert_lightEnabled = this.GetUniform( "lightEnabled" );
			_vert_lightParameters = this.GetUniform( "lightParameters" );
			_vert_lightPositions = this.GetUniform( "lightPositions" );
			_vert_lightDirections = this.GetUniform( "lightDirections" );
			_vert_lightColors = this.GetUniform( "lightColors" );

			_frag_textureEnabled = this.GetUniform( "textureEnabled" );
			_frag_texture = this.GetUniform( "texture" );
			_frag_textureFormat = this.GetUniform( "textureFormat" );

			// Defaults
			Gl.glUniform4f( _vert_isTransformed, 0.0f, 0.0f, 0.0f, 0.0f );
			Gl.glUniform4f( _vert_isRaw, 1.0f, 1.0f, 1.0f, 1.0f );

			return true;
		}

		public void Setup( MGLContext ctx, bool isTransformed, uint colorType, uint boneCount, uint morphCount )
		{
			if( this.IsDirty == true )
			{
				fixed( float* p = &ctx.WorldMatrix[ 0 ] )
					Gl.glUniformMatrix4fv( _vert_worldMatrix, 1, 0, ( IntPtr )p );

				if( ctx.TexturesEnabled == true )
				{
					Gl.glUniform2f( _vert_textureSize, ctx.Textures[ 0 ].Width, ctx.Textures[ 0 ].Height ); // TODO
					Gl.glUniform2f( _vert_textureOffset, ctx.TextureOffsetS, ctx.TextureOffsetT );
					Gl.glUniform2f( _vert_textureScale, ctx.TextureScaleS, ctx.TextureScaleT );

					Gl.glUniform1i( _frag_textureEnabled, 1 );
					Gl.glUniform1i( _frag_textureFormat, 0 ); // TODO
				}
				else
					Gl.glUniform1i( _frag_textureEnabled, 0 );

				this.IsDirty = false;
			}

			// Cached because most games render a bunch of transformed then a bunch of raw
			if( _isTransformed != isTransformed )
			{
				if( isTransformed == true )
				{
					Gl.glUniform4f( _vert_isTransformed, 1.0f, 1.0f, 1.0f, 1.0f );
					Gl.glUniform4f( _vert_isRaw, 0.0f, 0.0f, 0.0f, 0.0f );
				}
				else
				{
					Gl.glUniform4f( _vert_isTransformed, 0.0f, 0.0f, 0.0f, 0.0f );
					Gl.glUniform4f( _vert_isRaw, 1.0f, 1.0f, 1.0f, 1.0f );
				}
				_isTransformed = isTransformed;
			}
		}
	}
}
