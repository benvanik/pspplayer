// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "OglCapabilities.h"
#include "OglStatistics.h"

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
					bool						_shutdown;

				internal:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					DisplayProperties^			_props;
					DisplayProperties^			_currentProps;
					OglCapabilities^			_caps;
					OglStatistics^				_stats;

					void*						_nativeInterface;

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
							return IntPtr::Zero;
						}
						virtual void set( IntPtr value )
						{
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

				protected:
					void FillNativeInterface();

					void StartThread();
					void StopThread();

				internal:
					void WorkerThread();
				};

			}
		}
	}
}
