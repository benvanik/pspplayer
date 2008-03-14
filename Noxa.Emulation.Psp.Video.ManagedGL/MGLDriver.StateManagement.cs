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

		private BitArray _glEnabledState = new BitArray( 32 );
		private bool _matricesValid;

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
	}
}
