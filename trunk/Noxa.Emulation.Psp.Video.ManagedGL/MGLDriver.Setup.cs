// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define VSYNC

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Video.ManagedGL.Programs;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		private int _screenWidth = 480;
		private int _screenHeight = 272;
		private bool _needResize;

		private IntPtr _hDC;
		private IntPtr _hRC;

		private void SetupGL()
		{
			_hDC = User.GetDC( _controlHandle );
			Debug.Assert( _hDC != IntPtr.Zero );
			if( _hDC == IntPtr.Zero )
				return;

			Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();
			//ZeroMemory( &pfd, sizeof( pfd ) );
			pfd.nSize = ( short )sizeof( Gdi.PIXELFORMATDESCRIPTOR );
			pfd.nVersion = 1;
			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_TYPE_RGBA;
#if VSYNC
			//pfd.dwFlags |= Gdi.PFD_DOUBLEBUFFER;
#endif
			pfd.iPixelType = Gdi.PFD_TYPE_RGBA;
			pfd.cColorBits = 24;
			pfd.cDepthBits = 32;
			pfd.iLayerType = Gdi.PFD_MAIN_PLANE;
			int iFormat = Gdi.ChoosePixelFormat( _hDC, ref pfd );
			bool spf = Gdi.SetPixelFormat( _hDC, iFormat, ref pfd );

			Wgl.wglMakeCurrent( IntPtr.Zero, IntPtr.Zero );
			_hRC = Wgl.wglCreateContext( _hDC );
			Debug.Assert( _hRC != IntPtr.Zero );
			if( _hRC == IntPtr.Zero )
			{
				User.ReleaseDC( _controlHandle, _hDC );
				_hDC = IntPtr.Zero;
				return;
			}
			bool mc = Wgl.wglMakeCurrent( _hDC, _hRC );

			Gl.glShadeModel( Gl.GL_SMOOTH );
			Gl.glClearColor( 0.0f, 0.5f, 0.5f, 1.0f );
			Gl.glClearDepth( 0.0f );
			//Gl.glEnable( Gl.GL_DEPTH_TEST );
			Gl.glDepthFunc( Gl.GL_LEQUAL );
			Gl.glDepthRange( 1.0f, 0.0f );
			Gl.glHint( Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST );

#if !VSYNC
			Wgl.wglSwapIntervalEXT( 0 );
#endif

			this.Resize( _screenWidth, _screenHeight );

			_defaultProgram = new DefaultProgram();
			_defaultProgram.Prepare();
		}

		private void DestroyGL()
		{
			if( _defaultProgram != null )
				_defaultProgram.Dispose();
			_defaultProgram = null;

			Wgl.wglMakeCurrent( IntPtr.Zero, IntPtr.Zero );
			if( _hRC != IntPtr.Zero )
				Wgl.wglDeleteContext( _hRC );
			_hRC = IntPtr.Zero;
			if( ( _controlHandle != IntPtr.Zero ) &&
				( _hDC != IntPtr.Zero ) )
				User.ReleaseDC( _controlHandle, _hDC );
			_hDC = IntPtr.Zero;
		}

		public void Resize( int width, int height )
		{
			_screenWidth = width;
			_screenHeight = height;
			_needResize = true;
		}

		public DisplayProperties QueryDisplayProperties()
		{
			return this.Properties;
		}

		public bool SetMode( int mode, int width, int height )
		{
			this.Properties.Mode = mode;
			this.Properties.Width = width;
			this.Properties.Height = height;
			this.Resize( width, height );
			return true;
		}

		public bool SetFrameBuffer( uint bufferAddress, uint bufferSize, PixelFormat pixelFormat, BufferSyncMode bufferSyncMode )
		{
			this.Properties.BufferAddress = bufferAddress;
			this.Properties.BufferSize = bufferSize;
			this.Properties.PixelFormat = pixelFormat;
			this.Properties.SyncMode = bufferSyncMode;
			return true;
		}
	}
}
