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

					ref class sceHprm : public Module
					{
					public:
						sceHprm( Kernel^ kernel ) : Module( kernel ) {}
						~sceHprm(){}

						property String^ Name { virtual String^ get() override { return "sceHprm"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x208DB1BD, "sceHprmIsRemoteExist" )] [Stateless]
						// /hprm/psphprm.h:78: int sceHprmIsRemoteExist();
						int sceHprmIsRemoteExist(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7E69EDA4, "sceHprmIsHeadphoneExist" )] [Stateless]
						// /hprm/psphprm.h:71: int sceHprmIsHeadphoneExist();
						int sceHprmIsHeadphoneExist(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x219C58F1, "sceHprmIsMicrophoneExist" )] [Stateless]
						// /hprm/psphprm.h:85: int sceHprmIsMicrophoneExist();
						int sceHprmIsMicrophoneExist(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1910B327, "sceHprmPeekCurrentKey" )] [Stateless]
						// /hprm/psphprm.h:46: int sceHprmPeekCurrentKey(u32 *key);
						int sceHprmPeekCurrentKey( int key ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2BCEC83E, "sceHprmPeekLatch" )] [Stateless]
						// /hprm/psphprm.h:55: int sceHprmPeekLatch(u32 *latch);
						int sceHprmPeekLatch( int latch ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x40D2F9F0, "sceHprmReadLatch" )] [Stateless]
						// /hprm/psphprm.h:64: int sceHprmReadLatch(u32 *latch);
						int sceHprmReadLatch( int latch ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 69B3D0C9 */
