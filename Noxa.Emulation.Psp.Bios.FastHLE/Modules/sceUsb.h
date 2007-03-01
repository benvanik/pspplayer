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

					ref class sceUsb : public Module
					{
					public:
						sceUsb( Kernel^ kernel ) : Module( kernel ) {}
						~sceUsb(){}

						property String^ Name { virtual String^ get() override { return "sceUsb"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xAE5DE6AF, "sceUsbStart" )] [Stateless]
						// int sceUsbStart(const char* driverName, int size, void *args); (/usb/pspusb.h:35)
						int sceUsbStart( int driverName, int size, int args ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC2464FA0, "sceUsbStop" )] [Stateless]
						// int sceUsbStop(const char* driverName, int size, void *args); (/usb/pspusb.h:46)
						int sceUsbStop( int driverName, int size, int args ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC21645A4, "sceUsbGetState" )] [Stateless]
						// int sceUsbGetState(); (/usb/pspusb.h:71)
						int sceUsbGetState(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4E537366, "sceUsbGetDrvList" )] [Stateless]
						// int sceUsbGetDrvList(u32 r4one, u32* r5ret, u32 r6one); (/usb/pspusb.h:83)
						int sceUsbGetDrvList( int r4one, int r5ret, int r6one ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x112CC951, "sceUsbGetDrvState" )] [Stateless]
						// int sceUsbGetDrvState(const char* driverName); (/usb/pspusb.h:80)
						int sceUsbGetDrvState( int driverName ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x586DB82C, "sceUsbActivate" )] [Stateless]
						// int sceUsbActivate(u32 pid); (/usb/pspusb.h:55)
						int sceUsbActivate( int pid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC572A9C8, "sceUsbDeactivate" )] [Stateless]
						// int sceUsbDeactivate(u32 pid); (/usb/pspusb.h:64)
						int sceUsbDeactivate( int pid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5BE0E002, "sceUsbWaitState" )] [Stateless]
						// int sceUsbWaitState(u32 state, s32 waitmode, u32 *timeout); (/usb/pspusb.h:84)
						int sceUsbWaitState( int state, int waitmode, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1C360735, "sceUsbWaitCancel" )] [Stateless]
						// int sceUsbWaitCancel(); (/usb/pspusb.h:85)
						int sceUsbWaitCancel(){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - F97B73EB */
