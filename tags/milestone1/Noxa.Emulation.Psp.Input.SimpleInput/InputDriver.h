// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Input {

				ref class SimpleInput;
				
				ref class InputDriver : public IInputDevice
				{
				public:
					static InputDriver^			GlobalDriver;

				internal:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;

					void*						_nativeInterface;

					void*						_handle;

					PadButtons					_buttons;
					int							_analogX;
					int							_analogY;

					// XInput
					int							_padIndex;
					bool						_isConnected;

					// DInput
					bool						_dinputSetup;

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
							return SimpleInput::typeid;
						}
					}

					property IntPtr WindowHandle
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

					property bool IsConnected
					{
						virtual bool get();
					}

					property int AnalogX
					{
						virtual int get();
					}

					property int AnalogY
					{
						virtual int get();
					}

					property PadButtons Buttons
					{
						virtual PadButtons get();
					}

					property IntPtr NativeInterface
					{
						virtual IntPtr get()
						{
							return IntPtr::IntPtr( _nativeInterface );
						}
					}

					InputDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~InputDriver();

					virtual void Suspend(){}
					virtual bool Resume(){ return true; }

					virtual void Cleanup();

					virtual void Poll();

				protected:
					//void SetupNativeInterface();
					//void DestroyNativeInterface();
				};

			}
		}
	}
}
