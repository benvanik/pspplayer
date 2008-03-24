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
		private bool _vsyncWaiting;

		private void SetupWorker()
		{
			// We need to ensure that vsync is always 'running'
			// For now, we just fake this timer stuff so that the vcount goes up every 16ms
			// HACK: longer duration so the cpu isn't pegged firing interrupts
			_vsyncTimer = _timerQueue.CreatePeriodicTimer( new TimerCallback( VsyncTimer ), 32, TimerExecutionContext.TimerThread, false );
		}

		private void VsyncTimer( Timer timer )
		{
			_vcount++;
			_vsyncWaiting = true;

			// HACK: this should happen in NextFrame()
			//lock( this )
			//	this.Emu.Cpu.SetPendingInterrupt( 30 );
		}

		#region Frame Advancement

		private bool _commaDown;
		private bool _periodDown;
		private bool _slashDown;
		private bool _decoupled;

		private void NextFrame()
		{
			MGLStatistics.ProcessedFrames++;

			Gl.glFlush();
			Wgl.wglSwapBuffers( _hDC );
			//Gdi.SwapBuffersFast( _hDC );

			if( _screenshotPending == true )
			{
				_screenshotPending = false;
				this.ReallyCaptureScreen();
			}

			//_vcount++;
			_vsyncWaiting = false;
			_hasFinished = true;
			// TODO: do interrupt here?

			if( _needResize == true )
			{
				Gl.glViewport( 0, 0, _screenWidth, _screenHeight );
				Random r = new Random();
				Gl.glClearColor( ( float )r.NextDouble(), ( float )r.NextDouble(), ( float )r.NextDouble(), 1.0f );
				//Gl.glClearColor( 0.5f, 0.0f, 0.5f, 1.0f );
				Gl.glClear( Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT );
				_needResize = false;
			}
			Gl.glClear( Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT );

#if DEBUG
			bool oldPeriodDown = _periodDown;
			bool oldSlashDown = _slashDown;
			_commaDown = ( NativeMethods.GetAsyncKeyState( Keys.Oemcomma ) != 0 );
			_periodDown = ( NativeMethods.GetAsyncKeyState( Keys.OemPeriod ) != 0 );
			_slashDown = ( NativeMethods.GetAsyncKeyState( Keys.Oem2 ) != 0 );
			if( _commaDown == true )
			{
				// Draw wireframe
				this.DrawWireframe = true;
			}
			else
			{
				// Draw solid (normal)
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

		private static readonly int[] DepthFuncMap = new int[] { Gl.GL_NEVER, Gl.GL_ALWAYS, Gl.GL_EQUAL, Gl.GL_NOTEQUAL, Gl.GL_LESS, Gl.GL_LEQUAL, Gl.GL_GREATER, Gl.GL_GEQUAL };
		private static readonly int[] AlphaTestMap = new int[] { Gl.GL_NEVER, Gl.GL_ALWAYS, Gl.GL_EQUAL, Gl.GL_NOTEQUAL, Gl.GL_LESS, Gl.GL_LEQUAL, Gl.GL_GREATER, Gl.GL_GEQUAL };
		private static readonly int[] AlphaBlendEquMap = new int[] { Gl.GL_FUNC_ADD, Gl.GL_FUNC_SUBTRACT, Gl.GL_FUNC_REVERSE_SUBTRACT, Gl.GL_MIN, Gl.GL_MAX, Gl.GL_FUNC_ADD }; // TODO: last is really GU_ABS - somehow implement?
		private static readonly int[] AlphaBlendSrcMap = new int[] { Gl.GL_DST_COLOR, Gl.GL_ONE_MINUS_DST_COLOR, Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA, Gl.GL_DST_ALPHA, Gl.GL_ONE_MINUS_DST_ALPHA,
			Gl.GL_SRC_ALPHA /*x2*/, Gl.GL_ONE_MINUS_SRC_ALPHA /*x2*/, Gl.GL_DST_ALPHA /*x2*/, Gl.GL_ONE_MINUS_DST_ALPHA /*x2*/,
			Gl.GL_SRC_ALPHA /*GU_FIX*/ };
		private static readonly int[] AlphaBlendDestMap = new int[] { Gl.GL_SRC_COLOR, Gl.GL_ONE_MINUS_SRC_COLOR, Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA, Gl.GL_DST_ALPHA, Gl.GL_ONE_MINUS_DST_ALPHA,
			Gl.GL_SRC_ALPHA /*x2*/, Gl.GL_ONE_MINUS_SRC_ALPHA /*x2*/, Gl.GL_DST_ALPHA /*x2*/, Gl.GL_ONE_MINUS_DST_ALPHA /*x2*/,
			Gl.GL_ONE_MINUS_SRC_ALPHA /*GU_FIX*/ };
		private static readonly int[] TextureFilterMap = new int[] { Gl.GL_NEAREST, Gl.GL_LINEAR, Gl.GL_LINEAR /*undef*/, Gl.GL_LINEAR /*undef*/, Gl.GL_NEAREST_MIPMAP_NEAREST, Gl.GL_LINEAR_MIPMAP_NEAREST, Gl.GL_NEAREST_MIPMAP_LINEAR, Gl.GL_LINEAR_MIPMAP_LINEAR };
		private static readonly int[] TextureFunctionMap = new int[] { Gl.GL_MODULATE, Gl.GL_DECAL, Gl.GL_BLEND, Gl.GL_REPLACE, Gl.GL_ADD };
		private static readonly int[] TextureWrapMap = new int[] { Gl.GL_REPEAT, Gl.GL_CLAMP };

		private void ProcessList( DisplayList list )
		{
			if( _vsyncWaiting == true )
				this.NextFrame();

			// Lists are processed until they stall or CMD_END
			// CMD_FINISH means that the drawing is done and the frame can change (I think)
			// NOTE: I don't support signals yet - a CMD_SIGNAL is always followed by a CMD_END - it's ignored!
			// NOTE: CMD_SIGNAL can be used to simulate a jump, call, or ret - I DO support these

			//MGLStatistics.Print();

			uint* stallAddress = ( list.StallAddress > 0 ) ? ( uint* )this.MemorySystem.Translate( list.StallAddress ) : null;

			if( this.DrawWireframe == true )
				Gl.glPolygonMode( Gl.GL_FRONT_AND_BACK, Gl.GL_LINE );
			else
				Gl.glPolygonMode( Gl.GL_FRONT_AND_BACK, Gl.GL_FILL );

			bool listDone = false;
			bool didRealDrawing = false;
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
				int i;
				uint* pp;
				float[] vector4 = new float[ 4 ];

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
					case VideoCommand.ORIGINADDR:
						continue;
					case VideoCommand.OFFSETADDR:
						continue;

					// -- Termination ---------------------------------------------------
					case VideoCommand.FINISH:
						if( didRealDrawing == true )
						{
							//Debug.WriteLine( "finished list" );
							//this.NextFrame();
							//Gl.glFlush();
						}
						list.State = DisplayListState.Done;
						if( list.CallbackID > 0 )
						{
							if( _callbacks.FinishFunction != 0 )
							{
								Log.WriteLine( Verbosity.Verbose, Feature.Video, "calling GE finish callback at 0x{0:X8} with arg 0x{1:X8}", _callbacks.FinishFunction, _callbacks.FinishArgument );
								this.Emu.Cpu.MarshalCall( -1, _callbacks.FinishFunction, new uint[] { ( uint )list.ID, _callbacks.FinishArgument }, null, 0 );
							}
						}
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
									Log.WriteLine( Verbosity.Critical, Feature.Video, "would have issued a signal" );
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
							//x = 0;
							//if( ( argi & 0x100 ) != 0 )
							//{
							//    x |= Gl.GL_COLOR_BUFFER_BIT; // target
							//}
							//if( ( argi & 0x200 ) != 0 )
							//    x |= Gl.GL_ACCUM_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT; // stencil/alpha
							//if( ( argi & 0x400 ) != 0 )
							//    x |= Gl.GL_DEPTH_BUFFER_BIT; // zbuffer

							//Gl.glDepthMask( ( data >> 10 ) & 1 ); // Update Z?
							Gl.glDepthMask( 0 );
							int colMask = ( int )( ( argi >> 8 ) & 1 );
							int alphaMask = ( int )( ( argi >> 9 ) & 1 );
							Gl.glColorMask( colMask, colMask, colMask, alphaMask );
							Gl.glDisable( Gl.GL_BLEND );
							//if( ( argi & 0x100 ) != 0 )
							//Gl.glClear( Gl.GL_COLOR_BUFFER_BIT );
						}
						else
						{
							Gl.glDepthMask( 1 );
							Gl.glColorMask( 1, 1, 1, 1 );
							Gl.glEnable( Gl.GL_BLEND );
						}
						continue;
					case VideoCommand.SHADE:
					case VideoCommand.BCE:
						this.SetState( FeatureState.CullFaceMask, ( argi == 1 ) ? FeatureState.CullFaceMask : 0 );
						continue;
					case VideoCommand.FFACE:
						Gl.glFrontFace( ( argi == 1 ) ? Gl.GL_CW : Gl.GL_CCW );
						continue;
					case VideoCommand.AAE:
						// TODO: antialiasing
						continue;
					case VideoCommand.FBP:
					case VideoCommand.FBW:
						continue;

					// -- Alpha Testing -------------------------------------------------
					case VideoCommand.ATE:
						if( this.DrawWireframe == true )
							this.SetState( FeatureState.AlphaTestMask, 0 );
						else
							this.SetState( FeatureState.AlphaTestMask, ( argi == 1 ) ? FeatureState.AlphaTestMask : 0 );
						continue;
					case VideoCommand.ATST:
						argf = ( ( argi >> 8 ) & 0xFF ) / 255.0f;
						if( argf > 0.0f )
							Gl.glAlphaFunc( AlphaTestMap[ argi & 0xFF ], argf );
						// maybe disable if invalid?
						// @param mask - Specifies the mask that both values are ANDed with before comparison.
						x = ( argi >> 16 ) & 0xFF;
						Debug.Assert( ( x == 0x0 ) || ( x == 0xFF ) );
						continue;

					// -- Depth Testing -------------------------------------------------
					case VideoCommand.ZTE:
						this.SetState( FeatureState.DepthTestMask, ( argi == 1 ) ? FeatureState.DepthTestMask : 0 );
						continue;
					case VideoCommand.ZTST:
						Gl.glDepthFunc( DepthFuncMap[ argi ] );
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

					// -- Alpha Blending ------------------------------------------------
					case VideoCommand.ABE:
						this.SetState( FeatureState.AlphaBlendMask, ( argi == 1 ) ? FeatureState.AlphaBlendMask : 0 );
						continue;
					case VideoCommand.ALPHA:
						Gl.glBlendEquation( AlphaBlendEquMap[ ( argi >> 8 ) & 0x3 ] );
						Gl.glBlendFunc( AlphaBlendSrcMap[ argi & 0xF ], AlphaBlendDestMap[ ( argi >> 4 ) & 0xF ] );
						continue;
					case VideoCommand.SFIX:
					case VideoCommand.DFIX:
						continue;

					// -- Viewport/Scissor ----------------------------------------------
					case VideoCommand.SCISSOR1:
					case VideoCommand.SCISSOR2:
						continue;

					// -- Fog -----------------------------------------------------------
					case VideoCommand.FGE:
						this.SetState( FeatureState.FogMask, ( argi == 1 ) ? FeatureState.FogMask : 0 );
						continue;
					case VideoCommand.FCOL:
						vector4[ 0 ] = ( int )( argi & 0xFF ) / 255.0f;
						vector4[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						vector4[ 2 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						vector4[ 3 ] = 1.0f;
						Gl.glFogfv( Gl.GL_FOG_COLOR, vector4 );
						continue;
					case VideoCommand.FFAR:
						_ctx.FogEnd = argf;
						continue;
					case VideoCommand.FDIST:
						_ctx.FogDepth = argf;
						// We get f precalculated, so need to derive start
						{
							if( ( _ctx.FogEnd != 0.0 ) &&
								( _ctx.FogDepth != 0.0 ) )
							{
								float end = _ctx.FogEnd;
								float start = end - ( 1 / argf );
								Gl.glFogf( Gl.GL_FOG_START, start );
								Gl.glFogf( Gl.GL_FOG_END, end );
							}
						}
						continue;

					// -- Lighting ------------------------------------------------------
					case VideoCommand.LTE:
					case VideoCommand.LTE0:
					case VideoCommand.LTE1:
					case VideoCommand.LTE2:
					case VideoCommand.LTE3:
						continue;

					case VideoCommand.ALA:
					case VideoCommand.ALC:
						continue;

					// -- Materials -----------------------------------------------------
					// AMA
					// TODO: ensure AMC always follows AMA
					case VideoCommand.AMC: // Ambient (alpha from AMA)
						_ctx.AmbientModelColor[ 0 ] = ( int )( argi & 0xFF ) / 255.0f;
						_ctx.AmbientModelColor[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						_ctx.AmbientModelColor[ 2 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						_ctx.AmbientModelColor[ 3 ] = ( int )( _ctx.Values[ ( int )VideoCommand.AMA ] & 0xFF ) / 255.0f;
						Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, _ctx.AmbientModelColor );
						continue;
					case VideoCommand.DMC: // Diffuse
						vector4[ 0 ] = ( int )( argi & 0xFF ) / 255.0f;
						vector4[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						vector4[ 2 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						vector4[ 3 ] = 1.0f;
						Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, vector4 );
						continue;
					case VideoCommand.SMC: // Specular
						vector4[ 0 ] = ( int )( argi & 0xFF ) / 255.0f;
						vector4[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						vector4[ 2 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						vector4[ 3 ] = 1.0f;
						Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, vector4 );
						continue;
					case VideoCommand.EMC: // Emissive
						vector4[ 0 ] = ( int )( argi & 0xFF ) / 255.0f;
						vector4[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						vector4[ 2 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						vector4[ 3 ] = 1.0f;
						Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, vector4 );
						continue;

					// -- Primitive Drawing ---------------------------------------------
					// VTYPE, VADDR, IADDR
					case VideoCommand.PRIM:
						// Ignore clear mode
						//if( ( _ctx.Values[ ( int )VideoCommand.CLEAR ] & 0x1 ) > 0 )
						//	continue;
						//Gl.glFlush();
						didRealDrawing = true;
						this.DrawPrimitive( list.Base, argi );
						continue;

					// -- Patches/Splines -----------------------------------------------
					// PSUB, PFACE
					case VideoCommand.BEZIER:
						didRealDrawing = true;
						this.DrawBezier( list.Base, argi );
						continue;
					case VideoCommand.SPLINE:
						didRealDrawing = true;
						this.DrawSpline( list.Base, argi );
						continue;

					// -- Textures ------------------------------------------------------
					// TPSM
					case VideoCommand.TME:
						this.SetState( FeatureState.TexturesMask, ( argi == 1 ) ? FeatureState.TexturesMask : 0 );
						continue;
					case VideoCommand.TFLUSH:
					case VideoCommand.TSYNC:
						// Something?
						continue;
					case VideoCommand.TMODE:
						_ctx.TexturesSwizzled = ( ( argi & 0x1 ) == 1 ) ? true : false;
						_ctx.MipMapLevel = ( int )( ( argi >> 16 ) & 0x4 );
						continue;
					case VideoCommand.TFLT:
						// TODO: Mipmapping - the & 0x1 limits everything to normal, non-mipmapped textures
						i = TextureFilterMap[ ( int )( ( argi & 0x7 ) & 0x1 ) ];
						_ctx.TextureMinFilter = i;
						Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, i );
						Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, TextureFilterMap[ ( int )( ( ( argi >> 8 ) & 0x7 ) & 0x1 ) ] );
						continue;
					case VideoCommand.TWRAP:
						x = argi & 0x1;
						y = ( argi >> 8 ) & 0x1;
						this.SetState( FeatureState.ClampToEdgeMask, ( ( x | y ) > 0 ) ? FeatureState.ClampToEdgeMask : 0 );
						Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, TextureWrapMap[ ( int )x ] );
						Gl.glTexParameteri( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, TextureWrapMap[ ( int )y ] );
						continue;
					case VideoCommand.TFUNC:
						Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, TextureFunctionMap[ ( int )( argi & 0x7 ) ] );
						continue;
					case VideoCommand.TEC:
						vector4[ 0 ] = ( int )( ( argi >> 16 ) & 0xFF ) / 255.0f;
						vector4[ 1 ] = ( int )( ( argi >> 8 ) & 0xFF ) / 255.0f;
						vector4[ 2 ] = ( int )( argi & 0xFF ) / 255.0f;
						vector4[ 3 ] = 1.0f;
						Gl.glTexEnvfv( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_COLOR, vector4 );
						continue;
					case VideoCommand.USCALE:
						_ctx.TextureScaleS = argf;
						continue;
					case VideoCommand.VSCALE:
						_ctx.TextureScaleT = argf;
						continue;
					case VideoCommand.UOFFSET:
						_ctx.TextureOffsetS = argf;
						continue;
					case VideoCommand.VOFFSET:
						_ctx.TextureOffsetT = argf;
						continue;
					case VideoCommand.TBP0:
					case VideoCommand.TBP1:
					case VideoCommand.TBP2:
					case VideoCommand.TBP3:
					case VideoCommand.TBP4:
					case VideoCommand.TBP5:
					case VideoCommand.TBP6:
					case VideoCommand.TBP7:
						i = ( int )cmd - ( int )VideoCommand.TBP0;
						_ctx.Textures[ i ].Address = ( _ctx.Textures[ i ].Address & 0xFF000000 ) | argi;
						continue;
					case VideoCommand.TBW0:
					case VideoCommand.TBW1:
					case VideoCommand.TBW2:
					case VideoCommand.TBW3:
					case VideoCommand.TBW4:
					case VideoCommand.TBW5:
					case VideoCommand.TBW6:
					case VideoCommand.TBW7:
						i = ( int )cmd - ( int )VideoCommand.TBW0;
						_ctx.Textures[ i ].Address = ( ( argi << 8 ) & 0xFF000000 ) | ( _ctx.Textures[ i ].Address & 0x00FFFFFF );
						_ctx.Textures[ i ].LineWidth = argi & 0x0000FFFF;
						continue;
					case VideoCommand.TSIZE0:
					case VideoCommand.TSIZE1:
					case VideoCommand.TSIZE2:
					case VideoCommand.TSIZE3:
					case VideoCommand.TSIZE4:
					case VideoCommand.TSIZE5:
					case VideoCommand.TSIZE6:
					case VideoCommand.TSIZE7:
						i = ( int )cmd - ( int )VideoCommand.TSIZE0;
						_ctx.Textures[ i ].Width = ( uint )( 1 << ( int )( argi & 0x000000FF ) );
						_ctx.Textures[ i ].Height = ( uint )( 1 << ( int )( ( argi >> 8 ) & 0x000000FF ) );
						_ctx.Textures[ i ].PixelStorage = ( TexturePixelStorage )( _ctx.Values[ ( int )VideoCommand.TPSM ] & 0xF );
						continue;

					// -- CLUT ----------------------------------------------------------
					// CBP, CBPH, CMODE
					case VideoCommand.CLOAD:
						x = ( ( _ctx.Values[ ( int )VideoCommand.CBPH ] << 8 ) & 0xFF000000 ) | _ctx.Values[ ( int )VideoCommand.CBP ];
						y = _ctx.Values[ ( int )VideoCommand.CMODE ];
						_ctx.Clut.Load( x, y, argi );
						continue;

					// -- Texture Transfer ----------------------------------------------
					// TRXSBP, TRXSBW, TRXDBP, TRXDBW, TRXSIZE, TRXSPOS, TRXDPOS
					case VideoCommand.TRXKICK:
						didRealDrawing = true;
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
