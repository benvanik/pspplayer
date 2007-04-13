// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					public ref class sceImpose : public Module
					{
					public:
						sceImpose( IntPtr kernel ) : Module( kernel ) {}
						~sceImpose(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceImpose"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x24FD7BCF, "sceRegOpenRegistry" )] [Stateless]
						// manual add - ptrs to storage for values
						int sceImposeGetLanguageMode( IMemory^ memory, int language, int swapButtons ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x36AA6E91, "sceRegOpenRegistry" )] [Stateless]
						// manual add
						int sceImposeSetLanguageMode( int language, int swapButtons ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x381BD9E7, "sceRegOpenRegistry" )] [Stateless]
						// manual add
						int sceImposeHomeButton(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x72189C48, "sceRegOpenRegistry" )] [Stateless]
						// manual add
						int sceImposeSetUMDPopup( int enable ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8C943191, "sceRegOpenRegistry" )] [Stateless]
						// manual add
						int sceImposeGetBatteryIconStatus( IMemory^ memory, int status0, int status1 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE0887BC8, "sceRegOpenRegistry" )] [Stateless]
						// manual add
						int sceImposeGetUMDPopup(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}
