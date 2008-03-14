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
			MGLStatistics.ProcessedFrames++;

			if( _needResize == true )
			{
				Gl.glViewport( 0, 0, _screenWidth, _screenHeight );
				Gl.glClearColor( 0.5f, 0.0f, 0.5f, 1.0f );
				Gl.glClear( Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT );
				_needResize = false;
			}

			Gl.glFlush();
			Wgl.wglSwapBuffers( _hDC );
			//Gdi.SwapBuffersFast( _hDC );

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
			//Gl.glClear( Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT );

			// Lists are processed until they stall or CMD_END
			// CMD_FINISH means that the drawing is done and the frame can change (I think)
			// NOTE: I don't support signals yet - a CMD_SIGNAL is always followed by a CMD_END - it's ignored!
			// NOTE: CMD_SIGNAL can be used to simulate a jump, call, or ret - I DO support these

			//MGLStatistics.Print();

			uint* stallAddress = ( list.StallAddress > 0 ) ? ( uint* )this.MemorySystem.Translate( list.StallAddress ) : null;

			bool listDone = false;
			uint commandCount = 0;
			while( listDone == false )
			{
				// Check for stall
				if( list.Pointer == stallAddress )
				{
					MGLStatistics.StallCount++;
					list.State = DisplayListState.Stalled;
					listDone = true;
					break;
				}

				uint p = *list.Pointer;
				VideoCommand cmd = ( VideoCommand )( ( byte )( p >> 24 ) );
				uint argi = p & 0x00FFFFFF;
				uint argx = argi << 8;
				float argf = *( ( float* )( ( uint* )( &argx ) ) );

				uint x, y;
				uint* pp;

				// Next packet
				list.Pointer++;
				commandCount++;

				MGLStatistics.CommandCounts[ ( int )cmd ]++;
				_ctx.Values[ ( int )cmd ] = argi;

				switch( cmd )
				{
					// -- General -------------------------------------------------------
					case VideoCommand.BASE:
						list.Base = argx;
						continue;

					// -- Termination ---------------------------------------------------
					case VideoCommand.FINISH:
						this.NextFrame();
						Debug.WriteLine( "finished list" );
						list.State = DisplayListState.Done;
						continue;
					case VideoCommand.END:
						listDone = true;
						continue;
					case VideoCommand.Unknown0x11:
						// ?
						continue;

					// -- Flow control --------------------------------------------------
					case VideoCommand.JUMP:
						list.Pointer = ( uint* )this.MemorySystem.Translate( ( argi | list.Base ) & 0xFFFFFFFC );
						continue;
					case VideoCommand.CALL:
						list.Stack[ list.StackIndex++ ] = list.Pointer;
						list.Pointer = ( uint* )this.MemorySystem.Translate( ( argi | list.Base ) & 0xFFFFFFFC );
						continue;
					case VideoCommand.RET:
						list.Pointer = list.Stack[ --list.StackIndex ];
						continue;

					// -- Signals -------------------------------------------------------
					case VideoCommand.SIGNAL:
						{
							// Signal is ALWAYS followed by END - if not, we are dead
							MGLStatistics.SignalCount++;
							uint next = *list.Pointer;
							list.Pointer++;
							x = argi & 0x0000FFFF;
							y = next & 0x0000FFFF;
							switch( argi >> 16 )
							{
								case 0x01:
								case 0x02:
								case 0x03:
									break;
								case 0x10: // JUMP
									y |= x << 16;
									list.Pointer = ( uint* )this.MemorySystem.Translate( y & 0xFFFFFFFC );
									break;
								case 0x11: // CALL
									y |= x << 16;
									list.Stack[ list.StackIndex++ ] = list.Pointer;
									list.Pointer = ( uint* )this.MemorySystem.Translate( y & 0xFFFFFFFC );
									break;
								case 0x12: // RET
									list.Pointer = list.Stack[ --list.StackIndex ];
									break;
							}
						}
						continue;

					// -- General State -------------------------------------------------
					case VideoCommand.CLEAR:
						if( ( argi & 0x1 ) == 0x1 )
						{
							x = 0;
							if( ( argi & 0x100 ) != 0 )
								x |= Gl.GL_COLOR_BUFFER_BIT; // target
							if( ( argi & 0x200 ) != 0 )
								x |= Gl.GL_ACCUM_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT; // stencil/alpha
							if( ( argi & 0x400 ) != 0 )
								x |= Gl.GL_DEPTH_BUFFER_BIT; // zbuffer
							Gl.glClear( ( int )x );
						}
						continue;
					case VideoCommand.SHADE:
					case VideoCommand.BCE:
					case VideoCommand.FFACE:
					case VideoCommand.AAE:
						continue;
					case VideoCommand.FBP:
					case VideoCommand.FBW:
						continue;

					// -- Alpha Testing -------------------------------------------------
					case VideoCommand.ATE:
					case VideoCommand.ATST:
						continue;

					// -- Depth Testing -------------------------------------------------
					case VideoCommand.ZTE:
					case VideoCommand.ZTST:
						continue;
					case VideoCommand.NEARZ:
						_ctx.NearZ = ( float )( int )( ( short )( ushort )argi );
						continue;
					case VideoCommand.FARZ:
						if( _ctx.NearZ > argi )
						{
							_ctx.FarZ = _ctx.NearZ;
							_ctx.NearZ = ( float )( int )( ( short )( ushort )argi );
						}
						else
							_ctx.FarZ = ( float )( int )( ( short )( ushort )argi );
						Gl.glDepthRange( _ctx.NearZ, _ctx.FarZ );
						continue;

					// -- Alpha Testing -------------------------------------------------
					case VideoCommand.ABE:
					case VideoCommand.ALPHA:
					case VideoCommand.SFIX:
					case VideoCommand.DFIX:
						continue;

					// -- Viewport/Scissor ----------------------------------------------
					case VideoCommand.SCISSOR1:
					case VideoCommand.SCISSOR2:
						continue;

					// -- Fog -----------------------------------------------------------
					case VideoCommand.FGE:
					case VideoCommand.FCOL:
					case VideoCommand.FFAR:
					case VideoCommand.FDIST:
						continue;

					// -- Lighting/Materials --------------------------------------------
					case VideoCommand.LTE:
					case VideoCommand.LTE0:
					case VideoCommand.LTE1:
					case VideoCommand.LTE2:
					case VideoCommand.LTE3:
						continue;

					case VideoCommand.ALA:
					case VideoCommand.ALC:
					case VideoCommand.AMA:
					case VideoCommand.AMC:
						continue;

					// -- Primitive Drawing ---------------------------------------------
					// VTYPE, VADDR, IADDR
					case VideoCommand.PRIM:
						this.DrawPrimitive( list.Base, argi );
						continue;

					// -- Patches/Splines -----------------------------------------------
					// PSUB, PFACE
					case VideoCommand.BEZIER:
						this.DrawBezier( list.Base, argi );
						continue;
					case VideoCommand.SPLINE:
						this.DrawSpline( list.Base, argi );
						continue;

					// -- Textures ------------------------------------------------------
					case VideoCommand.TME:
					case VideoCommand.TSYNC:
					case VideoCommand.TMODE:
					case VideoCommand.TPSM:
					case VideoCommand.TFLT:
					case VideoCommand.TWRAP:
					case VideoCommand.TFUNC:
					case VideoCommand.TEC:
					case VideoCommand.TFLUSH:
					case VideoCommand.USCALE:
					case VideoCommand.VSCALE:
					case VideoCommand.UOFFSET:
					case VideoCommand.VOFFSET:
						//case VideoCommand.TBPN:
						//case VideoCommand.TBWN:
						//case VideoCommand.TSIZEN:
						continue;

					// -- CLUT ----------------------------------------------------------

					// -- Texture Transfer ----------------------------------------------
					// TRXSBP, TRXSBW, TRXDBP, TRXDBW, TRXSIZE, TRXSPOS, TRXDPOS
					case VideoCommand.TRXKICK:
						this.TransferTexture( argi );
						continue;

					// -- Matrices ------------------------------------------------------
					// PROJ, VIEW, WORLD, TMATRIX
					case VideoCommand.PMS:
						pp = list.Pointer;
						for( int n = 0; n < 16; n++ )
						{
							argx = ( *pp & 0x00FFFFFF ) << 8;
							_ctx.ProjectionMatrix[ n ] = *( ( float* )( ( uint* )( &argx ) ) );
							pp++;
						}
						list.Pointer += 16;
						this.InvalidateMatrices();
						continue;
					case VideoCommand.VMS:
						MGLUtilities.ReadMatrix3x4( list.Pointer, _ctx.ViewMatrix );
						list.Pointer += 12;
						this.InvalidateMatrices();
						continue;
					case VideoCommand.WMS:
						MGLUtilities.ReadMatrix3x4( list.Pointer, _ctx.WorldMatrix );
						list.Pointer += 12;
						this.InvalidateMatrices();
						continue;
					case VideoCommand.TMS:
						MGLUtilities.ReadMatrix3x4( list.Pointer, _ctx.TextureMatrix );
						list.Pointer += 12;
						this.InvalidateMatrices();
						continue;
				}
			}

			MGLStatistics.DisplayListsProcessed++;
			MGLStatistics.CommandsProcessed += commandCount;
		}
	}
}
