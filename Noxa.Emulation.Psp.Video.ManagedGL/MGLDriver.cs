// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Video.ManagedGL.Programs;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver : IVideoDriver
	{
		public readonly IEmulationInstance Emu;
		public readonly MemorySystem MemorySystem;
		private readonly ComponentParameters _parameters;
		private readonly MGLCapabilities _caps;
		public readonly DisplayProperties Properties;
		private readonly MGLStatisticsSource _statisticsSource;
		private IntPtr _controlHandle;
		private VideoCallbacks _callbacks;
		
		private bool _isSetup;
		private ulong _vcount;

		public bool IsSpeedLocked = true;
		public bool DrawWireframe;
		public bool DisableTextures;

		private MGLContext _ctx;
		private DefaultProgram _defaultProgram;

		public MGLDriver( IEmulationInstance emulator, ComponentParameters parameters )
		{
			this.Emu = emulator;
			this.MemorySystem = this.Emu.Cpu.Memory.MemorySystem;
			_parameters = parameters;
			_caps = new MGLCapabilities();
			this.Properties = new DisplayProperties();

			_statisticsSource = new MGLStatisticsSource( this );
			Diag.Instance.Counters.RegisterSource( _statisticsSource );
		}

		public Type Factory { get { return typeof( ManagedGLVideo ); } }
		public IEmulationInstance Emulator { get { return this.Emu; } }
		public ComponentParameters Parameters { get { return _parameters; } }

		public IVideoCapabilities Capabilities { get { return _caps; } }
		public IntPtr ControlHandle { get { return _controlHandle; } set { _controlHandle = value; } }
		public ulong Vcount { get { return _vcount; } }
		public IntPtr NativeInterface { get { return IntPtr.Zero; } }
		public VideoCallbacks Callbacks { get { return _callbacks; } set { _callbacks = value; } }

		public bool SpeedLocked { get { return this.IsSpeedLocked; } set { this.IsSpeedLocked = value; } }

		public void Suspend()
		{
		}

		public bool Resume()
		{
			if( _isSetup == false )
			{
				_ctx = new MGLContext();
				this.SetupGL();
				this.SetupLists();
				this.SetupWorker();
				_isSetup = true;
			}
			//if( _thread == nullptr )
			//    this->StartThread();

			//if( _props->HasChanged == false )
			//    return true;

			//Log::WriteLine( Verbosity::Normal, Feature::Video, "video mode change" );

			//_currentProps = ( DisplayProperties^ )_props->Clone();
			//_props->HasChanged = false;
			//_currentProps->HasChanged = false;

			return true;
		}

		public void Cleanup()
		{
			this.DestroyGL();
		}

		#region Screenshots

		private bool _screenshotPending;
		private AutoResetEvent _screenshotEvent = new AutoResetEvent( false );
		private Bitmap _screenshot;

		public Bitmap CaptureScreen()
		{
			_screenshotPending = true;
			if( _screenshotEvent.WaitOne( 5000, true ) == false )
				return null;
			else
			{
				Bitmap b = _screenshot;
				_screenshot = null;
				return b;
			}
		}

		internal void ReallyCaptureScreen()
		{
			Gl.glPushClientAttrib( Gl.GL_CLIENT_PIXEL_STORE_BIT );

			int[] viewport = new int[ 4 ];
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, viewport );

			int compWidth = 3;
			Gl.glPixelStorei( Gl.GL_PACK_ALIGNMENT, compWidth );
			Gl.glPixelStorei( Gl.GL_PACK_ROW_LENGTH, 0 );
			Gl.glPixelStorei( Gl.GL_PACK_SKIP_ROWS, 0 );
			Gl.glPixelStorei( Gl.GL_PACK_SKIP_PIXELS, 0 );

			Gl.glReadBuffer( Gl.GL_FRONT );

			Bitmap b;
			byte[] buffer = new byte[ compWidth * viewport[ 2 ] * viewport[ 3 ] ];
			fixed( byte* p = &buffer[ 0 ] )
			{
				Gl.glReadPixels( 0, 0, viewport[ 2 ], viewport[ 3 ], Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, new IntPtr( p ) );
				b = new Bitmap( viewport[ 2 ], viewport[ 3 ], viewport[ 2 ] * compWidth, System.Drawing.Imaging.PixelFormat.Format24bppRgb, new IntPtr( p ) );
			}
			b.RotateFlip( RotateFlipType.RotateNoneFlipY );

			Gl.glPopClientAttrib();

			_screenshot = b;
			_screenshotEvent.Set();
		}

		#endregion

		#region IDebuggable Members

		public bool SupportsDebugging { get { return false; } }
		public bool DebuggingEnabled { get { return false; } }
		public IHook DebugHook { get { return null; } }

		public void EnableDebugging()
		{
		}

		#endregion
	}
}
