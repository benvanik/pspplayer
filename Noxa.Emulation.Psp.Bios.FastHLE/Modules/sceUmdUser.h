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

					ref class sceUmdUser : public Module
					{
					public:
						sceUmdUser( Kernel^ kernel ) : Module( kernel ) {}
						~sceUmdUser(){}

						property String^ Name { virtual String^ get() override { return "sceUmdUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x46EBB729, "sceUmdCheckMedium" )] [Stateless]
						// /umd/pspumd.h:42: int sceUmdCheckMedium(int a);
						int sceUmdCheckMedium( int a ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC6183D47, "sceUmdActivate" )] [Stateless]
						// /umd/pspumd.h:66: int sceUmdActivate(int unit, const char *drive);
						int sceUmdActivate( int unit, int drive ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8EF08FCE, "sceUmdWaitDriveStat" )] [Stateless]
						// /umd/pspumd.h:75: int sceUmdWaitDriveStat(int stat);
						int sceUmdWaitDriveStat( int stat ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAEE7404D, "sceUmdRegisterUMDCallBack" )] [Stateless]
						// /umd/pspumd.h:89: int sceUmdRegisterUMDCallBack(int cbid);
						int sceUmdRegisterUMDCallBack( int cbid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 147E4A8A */
