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

			return true;
		}
	}
}
