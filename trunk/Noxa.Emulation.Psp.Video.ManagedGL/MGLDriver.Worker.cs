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
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		private TimerQueue _timerQueue = new TimerQueue();
		private Timer _vsyncTimer;

		private void SetupWorker()
		{
			// We need to ensure that vsync is always 'running'
			// For now, we just fake this timer stuff so that the vcount goes up every 16ms
			_vsyncTimer = _timerQueue.CreatePeriodicTimer( new TimerCallback( VsyncTimer ), 16, TimerExecutionContext.TimerThread, false );
		}

		private void VsyncTimer( Timer timer )
		{
			_vcount++;
		}

		#region Frame Advancement

		private bool _commaDown;
		private bool _periodDown;
		private bool _slashDown;
		private bool _decoupled;

		private void NextFrame()
		{
			// Needed?
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, 0 );

			Gl.glFlush();
			Gdi.SwapBuffers( _hDC );

			if( _screenshotPending == true )
			{
				_screenshotPending = false;
				this.ReallyCaptureScreen();
			}

			_vcount++;
			_hasFinished = true;

#if DEBUG
			bool oldPeriodDown = _periodDown;
			bool oldSlashDown = _slashDown;
			_commaDown = ( NativeMethods.GetAsyncKeyState( Keys.Oemcomma ) != 0 );
			_periodDown = ( NativeMethods.GetAsyncKeyState( Keys.OemPeriod ) != 0 );
			_slashDown = ( NativeMethods.GetAsyncKeyState( Keys.Oem2 ) != 0 );
			if( _commaDown == true )
			{
				// Draw wireframe
				//glClear( GL_COLOR_BUFFER_BIT );
				Gl.glPolygonMode( Gl.GL_FRONT_AND_BACK, Gl.GL_LINE );
				this.DrawWireframe = true;
			}
			else
			{
				// Draw solid (normal)
				Gl.glPolygonMode( Gl.GL_FRONT_AND_BACK, Gl.GL_FILL );
				this.DrawWireframe = false;
			}
			if( oldPeriodDown != _periodDown )
			{
				if( _periodDown == true )
				{
					// Unlock
					this.SetSpeedLock( false );
				}
				else
				{
					// Lock (normal)
					this.SetSpeedLock( true );
				}
				_decoupled = false;
			}
			if( oldSlashDown != _slashDown )
			{
				if( _slashDown == false )
				{
					_decoupled = !_decoupled;
					this.SetSpeedLock( !_decoupled );
				}
			}
#endif
		}

		private void SetSpeedLock( bool locked )
		{
			if( locked == true )
				this.Emu.LockSpeed();
			else
				this.Emu.UnlockSpeed();
		}

		#endregion

		private void ProcessAllLists()
		{
			for( int n = 0; n < _displayLists.Length; n++ )
			{
				DisplayList list = _displayLists[ n ];
				switch( list.State )
				{
					case DisplayListState.Ready:
					case DisplayListState.Stalled:
						this.ProcessList( list );
						break;
				}
			}
		}

		private void ProcessList( DisplayList list )
		{
			Random r = new Random();
			//Gl.glClearColor( ( float )r.NextDouble(), ( float )r.NextDouble(), ( float )r.NextDouble(), 1.0f );
			Gl.glClear( Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT );

			this.NextFrame();
			Debug.WriteLine( "finished list" );
			list.State = DisplayListState.Done;
		}
	}
}
