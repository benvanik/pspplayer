// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

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
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;

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
							return nullptr;
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

					property uint Vcount
					{
						virtual uint get()
						{
							return 0;
						}
					}

					property IVideoCapabilities^ Capabilities
					{
						virtual IVideoCapabilities^ get()
						{
							return nullptr;
						}
					}

					property IVideoStatistics^ Statistics
					{
						virtual IVideoStatistics^ get()
						{
							return nullptr;
						}
					}

					OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~OglDriver();

					virtual void Suspend();
					virtual bool Resume();

					virtual void Cleanup();

					virtual DisplayList^ FindDisplayList( int displayListId ){ return nullptr; }
					virtual bool Enqueue( DisplayList^ displayList, bool immediate ){ return false; }
					virtual void Abort( int displayListId ){}
					virtual void Sync( DisplayList^ displayList ){}
					virtual void Sync(){}
				};

			}
		}
	}
}
