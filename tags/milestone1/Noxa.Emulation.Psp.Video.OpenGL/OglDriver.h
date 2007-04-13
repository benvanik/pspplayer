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
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				ref class OpenGLVideo;
				
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
					OglStatistics^				_stats;
					DateTime					_startTime;

					void*						_nativeInterface;

					void*						_handle;
					void*						_hDC;
					void*						_hRC;

					OglContext*					_context;

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

					property uint OglDriver::Vcount
					{
						virtual uint get();
					}

					property IVideoCapabilities^ Capabilities
					{
						virtual IVideoCapabilities^ get()
						{
							return _caps;
						}
					}

					property IVideoStatistics^ Statistics
					{
						virtual IVideoStatistics^ get()
						{
							return _stats;
						}
					}

					property IntPtr NativeInterface
					{
						virtual IntPtr get()
						{
							return IntPtr::IntPtr( _nativeInterface );
						}
					}

					OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~OglDriver();

					virtual void Suspend();
					virtual bool Resume();

					virtual void Cleanup();

					virtual DisplayList^ FindDisplayList( int displayListId );
					virtual bool Enqueue( DisplayList^ displayList, bool immediate );
					virtual void Abort( int displayListId );
					virtual void Sync( DisplayList^ displayList );
					virtual void Sync();

					virtual void PrintStatistics();

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
