// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class FastHLE;
				ref class Module;
				ref class Kernel;

				ref class FastBios : public IBios
				{
				private:
					IEmulationInstance^					_emulator;
					ComponentParameters^				_parameters;
					List<Module^>^						_moduleList;
					Dictionary<String^, Module^>^		_modules;
					List<BiosFunction^>^				_functionList;
					Dictionary<uint, BiosFunction^>^	_functions;

				internal:
					Kernel^								_kernel;

				public:

					FastBios( IEmulationInstance^ emulator, ComponentParameters^ parameters );

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
							return FastHLE::typeid;
						}
					}

					property IKernel^ Kernel
					{
						virtual IKernel^ get()
						{
							return ( IKernel^ )_kernel;
						}
					}

					property array<IModule^>^ Modules
					{
						virtual array<IModule^>^ get()
						{
							return ( array<IModule^>^ )_moduleList->ToArray();
						}
					}

					property array<BiosFunction^>^ Functions
					{
						virtual array<BiosFunction^>^ get()
						{
							return _functionList->ToArray();
						}
					}

					virtual void Cleanup();

					void ClearModules();
					void StartModules();
					void StopModules();

					virtual IModule^ FindModule( String^ name )
					{
						if( _modules->ContainsKey( name ) == true )
							return ( IModule^ )_modules[ name ];
						return nullptr;
					}

					virtual BiosFunction^ FindFunction( uint nid )
					{
						if( _functions->ContainsKey( nid ) == true )
							return _functions[ nid ];
						return nullptr;
					}

					virtual void RegisterFunction( BiosFunction^ function );
					virtual void UnregisterFunction( uint nid );

				private:
					void GatherFunctions();

				};

			}
		}
	}
}
