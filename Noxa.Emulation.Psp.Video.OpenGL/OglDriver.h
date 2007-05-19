// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "OglCapabilities.h"
#include "OglStatistics.h"
#include "OglContext.h"

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::Hooks;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				extern bool _speedLocked;
				extern bool _debugEnabled;
				extern bool _screenshotPending;

				ref class OpenGLVideo;
				ref class OglHook;
				
				ref class OglDriver : IVideoDriver
				{
				public:
					static OglDriver^			GlobalDriver;

				protected:
					Thread^						_thread;
					AutoResetEvent^				_threadSync;

				internal:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					DisplayProperties^			_props;
					DisplayProperties^			_currentProps;
					OglCapabilities^			_caps;
					OglHook^					_hook;
					OglStatistics^				_stats;
					DateTime					_startTime;

					void*						_nativeInterface;

					void*						_handle;
					void*						_hDC;
					void*						_hRC;

					OglContext*					_context;

					int							_screenWidth;
					int							_screenHeight;

					AutoResetEvent^				_screenshotEvent;
					Drawing::Bitmap^			_screenshot;

				public:

					property ComponentParameters^ Parameters
					{
						virtual ComponentParameters^ get()
						{
							return _params;
						}
					}

					property IEmulationInstance^ Emulator
					{
						virtual IEmulationInstance^ get()
						{
							return _emu;
						}
					}

					property Type^ Factory
					{
						virtual Type^ get()
						{
							return OpenGLVideo::typeid;
						}
					}

					property DisplayProperties^ Properties
					{
						virtual DisplayProperties^ get()
						{
							return _props;
						}
					}

					property IntPtr ControlHandle
					{
						virtual IntPtr get()
						{
							return IntPtr::IntPtr( ( void* )_handle );
						}
						virtual void set( IntPtr value )
						{
							_handle = ( void* )value.ToPointer();
						}
					}

					property AutoResetEvent^ Vblank
					{
						virtual AutoResetEvent^ get()
						{
							return nullptr;
						}
					}

					property uint64 OglDriver::Vcount
					{
						virtual uint64 get();
					}

					property IVideoCapabilities^ Capabilities
					{
						virtual IVideoCapabilities^ get()
						{
							return _caps;
						}
					}

					property IntPtr NativeInterface
					{
						virtual IntPtr get()
						{
							return IntPtr::IntPtr( _nativeInterface );
						}
					}

					property bool SpeedLocked
					{
						virtual bool get()
						{
							return _speedLocked;
						}
						virtual void set( bool value )
						{
							_speedLocked = value;
						}
					}

					OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~OglDriver();

					virtual void Resize( int width, int height );

					virtual void Suspend();
					virtual bool Resume();

					virtual Drawing::Bitmap^ CaptureScreen();

					virtual void Cleanup();

					property IHook^ DebugHook
					{
						virtual IHook^ get()
						{
							return ( IHook^ )_hook;
						}
					}
					property bool SupportsDebugging
					{
						virtual bool get();
					}
					property bool DebuggingEnabled
					{
						virtual bool get();
					}
					virtual void EnableDebugging();

				protected:
					void SetupNativeInterface();
					void DestroyNativeInterface();

					void StartThread();
					void StopThread();
					void SetupOpenGL();
					void DestroyOpenGL();

				internal:
					void WorkerThread();
				};

			}
		}
	}
}
