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

					ref class sceUsbstorBoot : public Module
					{
					public:
						sceUsbstorBoot( Kernel^ kernel ) : Module( kernel ) {}
						~sceUsbstorBoot(){}

						property String^ Name { virtual String^ get() override { return "sceUsbstorBoot"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xE58818A8, "sceUsbstorBootSetCapacity" )] [Stateless]
						// int sceUsbstorBootSetCapacity(u32 size); (/usbstor/pspusbstor.h:50)
						int sceUsbstorBootSetCapacity( int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x594BBF95, "sceUsbstorBootSetLoadAddr" )] [Stateless]
						// int sceUsbstorBootSetLoadAddr(u32 addr); (/usbstor/pspusbstor.h:55)
						int sceUsbstorBootSetLoadAddr( int addr ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6D865ECD, "sceUsbstorBootGetDataSize" )] [Stateless]
						// int sceUsbstorBootGetDataSize(); (/usbstor/pspusbstor.h:54)
						int sceUsbstorBootGetDataSize(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA1119F0D, "sceUsbstorBootSetStatus" )] [Stateless]
						// int sceUsbstorBootSetStatus(u32 status); (/usbstor/pspusbstor.h:56)
						int sceUsbstorBootSetStatus( int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1F080078, "sceUsbstorBootRegisterNotify" )] [Stateless]
						// int sceUsbstorBootRegisterNotify(u32 eventFlag); (/usbstor/pspusbstor.h:28)
						int sceUsbstorBootRegisterNotify( int eventFlag ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA55C9E16, "sceUsbstorBootUnregisterNotify" )] [Stateless]
						// int sceUsbstorBootUnregisterNotify(u32 eventFlag); (/usbstor/pspusbstor.h:37)
						int sceUsbstorBootUnregisterNotify( int eventFlag ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 13D1DE64 */
