// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	class GLProgram : IDisposable
	{
		public int ProgramID;
		public int VertexShaderID;
		public int FragmentShaderID;

		public bool IsDirty;

		public bool Load( string vertexShader, string fragmentShader )
		{
			this.ProgramID = Gl.glCreateProgram();
			this.VertexShaderID = Gl.glCreateShader( Gl.GL_VERTEX_SHADER );
			this.FragmentShaderID = Gl.glCreateShader( Gl.GL_FRAGMENT_SHADER );
			int vertexShaderLength = vertexShader.Length;
			int fragmentShaderLength = fragmentShader.Length;
			Gl.glShaderSource( this.VertexShaderID, 1, new string[] { vertexShader }, ref vertexShaderLength );
			Gl.glShaderSource( this.FragmentShaderID, 1, new string[] { fragmentShader }, ref fragmentShaderLength );
			return true;
		}

		public bool Prepare()
		{
			bool vertexResult = this.CompileShader( this.VertexShaderID, "Vertex Shader" );
			bool fragmentResult = this.CompileShader( this.FragmentShaderID, "Fragment Shader" );
			if( ( vertexResult == false ) || ( fragmentResult == false ) )
			{
				this.Dispose();
				return false;
			}

			Gl.glAttachShader( this.ProgramID, this.VertexShaderID );
			Gl.glAttachShader( this.ProgramID, this.FragmentShaderID );

			Gl.glLinkProgram( this.ProgramID );

			int linkResult;
			Gl.glGetProgramiv( this.ProgramID, Gl.GL_LINK_STATUS, out linkResult );
			if( linkResult == 0 )
			{
				int logLength;
				Gl.glGetProgramiv( this.ProgramID, Gl.GL_INFO_LOG_LENGTH, out logLength );
				StringBuilder sb = new StringBuilder( logLength );
				Gl.glGetProgramInfoLog( this.ProgramID, logLength, out logLength, sb );

				string escaped = sb.Replace( "{", "{{" ).Replace( "}", "}}" ).Replace( "\n", Environment.NewLine ).ToString();
				Log.WriteLine( Verbosity.Critical, Feature.Video, "GLProgram::Prepare: failed to link program: " + escaped );

				this.Dispose();
				return false;
			}

			Gl.glValidateProgram( this.ProgramID );

			int validateResult;
			Gl.glGetProgramiv( this.ProgramID, Gl.GL_VALIDATE_STATUS, out validateResult );
			if( validateResult == 0 )
			{
				int logLength;
				Gl.glGetProgramiv( this.ProgramID, Gl.GL_INFO_LOG_LENGTH, out logLength );
				StringBuilder sb = new StringBuilder( logLength );
				Gl.glGetProgramInfoLog( this.ProgramID, logLength, out logLength, sb );

				string escaped = sb.Replace( "{", "{{" ).Replace( "}", "}}" ).Replace( "\n", Environment.NewLine ).ToString();
				Log.WriteLine( Verbosity.Critical, Feature.Video, "GLProgram::Prepare: failed to validate program: " + escaped );

				this.Dispose();
				return false;
			}

			Gl.glUseProgram( this.ProgramID );
			bool result = this.OnPrepare();
			Gl.glUseProgram( 0 );
			return result;
		}

		protected virtual bool OnPrepare()
		{
			return true;
		}

		private bool CompileShader( int id, string name )
		{
			Gl.glCompileShader( id );

			int result;
			Gl.glGetShaderiv( id, Gl.GL_COMPILE_STATUS, out result );
			if( result == 0 )
			{
				int logLength;
				Gl.glGetShaderiv( id, Gl.GL_INFO_LOG_LENGTH, out logLength );
				StringBuilder sb = new StringBuilder( logLength );
				Gl.glGetShaderInfoLog( id, logLength, out logLength, sb );

				string escaped = sb.Replace( "{", "{{" ).Replace( "}", "}}" ).Replace( "\n", Environment.NewLine ).ToString();
				Log.WriteLine( Verbosity.Critical, Feature.Video, "GLProgram::CompileShader: failed to compile {0} program: " + escaped, name );

				return false;
			}
			else
				return true;
		}

		public void Dispose()
		{
			if( this.VertexShaderID > 0 )
				Gl.glDeleteShader( this.VertexShaderID );
			if( this.FragmentShaderID > 0 )
				Gl.glDeleteShader( this.FragmentShaderID );
			if( this.ProgramID > 0 )
				Gl.glDeleteProgram( this.ProgramID );
			this.VertexShaderID = 0;
			this.FragmentShaderID = 0;
			this.ProgramID = 0;
		}

		protected int GetAttribute( string name )
		{
			int id = Gl.glGetAttribLocation( this.ProgramID, name );
			if( id < 0 )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Video, "GLProgram::GetAttribute: could not find attribute {0}", name );
				//throw new ArgumentException();
				return -1;
			}
			return id;
		}

		protected int GetUniform( string name )
		{
			int id = Gl.glGetUniformLocation( this.ProgramID, name );
			if( id < 0 )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Video, "GLProgram::GetUniform: could not find uniform {0}", name );
				//throw new ArgumentException();
				return -1;
			}
			return id;
		}
	}
}
