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

					public ref class sceUmdUser : public Module
					{
					public:
						sceUmdUser( Kernel^ kernel ) : Module( kernel ) {}
						~sceUmdUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "sceUmdUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[BiosFunction( 0x46EBB729, "sceUmdCheckMedium" )] [Stateless]
						// int sceUmdCheckMedium(); (/umd/pspumd.h:42)
						int sceUmdCheckMedium();

						[BiosFunction( 0xC6183D47, "sceUmdActivate" )] [Stateless]
						// int sceUmdActivate(int unit, const char *drive); (/umd/pspumd.h:66)
						int sceUmdActivate( IMemory^ memory, int unit, int drive );

						[BiosFunction( 0xE83742BA, "sceUmdDeactivate" )] [Stateless]
						// manual add
						int sceUmdDeactivate( IMemory^ memory, int unit, int drive );

						[BiosFunction( 0x6B4A146C, "sceUmdGetDriveStat" )] [Stateless]
						// manual add
						int sceUmdGetDriveStat();

						[BiosFunction( 0x20628E6F, "sceUmdGetErrorStat" )] [Stateless]
						// manual add
						int sceUmdGetErrorStat();

						[BiosFunction( 0x8EF08FCE, "sceUmdWaitDriveStat" )] [Stateless]
						// int sceUmdWaitDriveStat(int stat); (/umd/pspumd.h:75)
						int sceUmdWaitDriveStat( int stat );

						[NotImplemented]
						[BiosFunction( 0x56202973, "sceUmdWaitDriveStatWithTimer" )] [Stateless]
						// manual add - params not right?
						int sceUmdWaitDriveStatWithTimer( int stat );

						[BiosFunction( 0x4A9E5E29, "sceUmdWaitDriveStatCB" )] [Stateless]
						// manual add
						int sceUmdWaitDriveStatCB( int stat );

						[BiosFunction( 0xAEE7404D, "sceUmdRegisterUMDCallBack" )] [Stateless]
						// int sceUmdRegisterUMDCallBack(int cbid); (/umd/pspumd.h:89)
						int sceUmdRegisterUMDCallBack( int cbid );

						[BiosFunction( 0xBD2BDE07, "sceUmdUnRegisterUMDCallBack" )] [Stateless]
						// manual add
						int sceUmdUnRegisterUMDCallBack( int cbid );

						[BiosFunction( 0x340B7686, "sceUmdGetDiscInfo" )] [Stateless]
						// manual add
						int sceUmdGetDiscInfo( IMemory^ memory, int discInfo );

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 5085FAD2 */
