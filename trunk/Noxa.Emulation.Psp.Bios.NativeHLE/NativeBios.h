// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Debugging;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class NativeHLE;
				class Kernel;

				ref class NativeBios : public IBios
				{
				internal:
					IEmulationInstance^					_emulator;
					ComponentParameters^				_parameters;
					IDebugger^							_debugger;

					List<BiosModule^>^					_metaModuleList;
					Dictionary<String^, BiosModule^>^	_metaModules;
					List<Module^>						_moduleList;
					List<BiosFunction^>^				_functionList;
					Dictionary<uint, BiosFunction^>^	_functions;

					List<Module^>						_modules;

				internal:
					Loader^								Loader;
					Kernel*								Kernel;

				public:
					NativeBios( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~NativeBios();

					property ComponentParameters^ Parameters
					{
						virtual ComponentParameters^ get()
						{
							return _parameters;
						}
					}

					property IEmulationInstance^ Emulator
					{
						virtual IEmulationInstance^ get()
						{
							return _emulator;
						}
					}

					property Type^ Factory
					{
						virtual Type^ get()
						{
							return NativeHLE::typeid;
						}
					}

					property array<BiosModule^>^ Modules
					{
						virtual array<BiosModule^>^ get()
						{
							return ( array<BiosModule^>^ )_moduleList->ToArray();
						}
					}

					property array<BiosFunction^>^ Functions
					{
						virtual array<BiosFunction^>^ get()
						{
							return _functionList->ToArray();
						}
					}

					property bool DebuggingEnabled
					{
						virtual bool get()
						{
							return ( _debugger != nullptr );
						}
					}
					
					property IDebugger^ Debugger
					{
						virtual IDebugger^ get()
						{
							return _debugger;
						}
					}

					property IBiosHook^ DebugHook
					{
						virtual IBiosHook^ get()
						{
							return nullptr;
						}
					}

					virtual void EnableDebugging( IDebugger^ debugger );

					virtual void Cleanup();

					virtual BiosModule^ FindModule( String^ name );
					virtual BiosFunction^ FindFunction( uint nid );

				internal:
					void ClearModules();
					void StartModules();
					void StopModules();

				private:
					void GatherFunctions();
					void RegisterFunction( BiosFunction^ function );

				};

			}
		}
	}
}
