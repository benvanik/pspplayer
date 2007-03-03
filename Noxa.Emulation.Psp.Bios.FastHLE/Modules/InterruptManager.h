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

					public ref class InterruptManager : public Module
					{
					public:
						InterruptManager( Kernel^ kernel ) : Module( kernel ) {}
						~InterruptManager(){}

					public:
						property String^ Name { virtual String^ get() override { return "InterruptManager"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xCA04A2B9, "sceKernelRegisterSubIntrHandler" )] [Stateless]
						// int sceKernelRegisterSubIntrHandler(int intno, int no, void *handler, void *arg); (/user/pspintrman.h:119)
						int sceKernelRegisterSubIntrHandler( int intno, int no, int handler, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD61E6961, "sceKernelReleaseSubIntrHandler" )] [Stateless]
						// int sceKernelReleaseSubIntrHandler(int intno, int no); (/user/pspintrman.h:129)
						int sceKernelReleaseSubIntrHandler( int intno, int no ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFB8E22EC, "sceKernelEnableSubIntr" )] [Stateless]
						// int sceKernelEnableSubIntr(int intno, int no); (/user/pspintrman.h:139)
						int sceKernelEnableSubIntr( int intno, int no ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8A389411, "sceKernelDisableSubIntr" )] [Stateless]
						// int sceKernelDisableSubIntr(int intno, int no); (/user/pspintrman.h:149)
						int sceKernelDisableSubIntr( int intno, int no ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD2E8363F, "QueryIntrHandlerInfo" )] [Stateless]
						// int QueryIntrHandlerInfo(SceUID intr_code, SceUID sub_intr_code, PspIntrHandlerOptionParam *data); (/user/pspintrman.h:170)
						int QueryIntrHandlerInfo( int intr_code, int sub_intr_code, int data ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - FC5FA31B */
