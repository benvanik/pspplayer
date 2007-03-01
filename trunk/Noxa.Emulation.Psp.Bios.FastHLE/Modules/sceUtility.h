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

					ref class sceUtility : public Module
					{
					public:
						sceUtility( Kernel^ kernel ) : Module( kernel ) {}
						~sceUtility(){}

						property String^ Name { virtual String^ get() override { return "sceUtility"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x50C4CD57, "sceUtilitySavedataInitStart" )] [Stateless]
						// /utility/psputility_savedata.h:97: int sceUtilitySavedataInitStart(SceUtilitySavedataParam * params);
						int sceUtilitySavedataInitStart( int params ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9790B33C, "sceUtilitySavedataShutdownStart" )] [Stateless]
						// /utility/psputility_savedata.h:117: int sceUtilitySavedataShutdownStart();
						int sceUtilitySavedataShutdownStart(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD4B95FFB, "sceUtilitySavedataUpdate" )] [Stateless]
						// /utility/psputility_savedata.h:124: void sceUtilitySavedataUpdate(int unknown);
						void sceUtilitySavedataUpdate( int unknown ){}

						[NotImplemented]
						[BiosFunction( 0x8874DBE0, "sceUtilitySavedataGetStatus" )] [Stateless]
						// /utility/psputility_savedata.h:107: int sceUtilitySavedataGetStatus();
						int sceUtilitySavedataGetStatus(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2AD8E239, "sceUtilityMsgDialogInitStart" )] [Stateless]
						// /utility/psputility_msgdialog.h:50: int sceUtilityMsgDialogInitStart(SceUtilityMsgDialogParams *params);
						int sceUtilityMsgDialogInitStart( int params ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x67AF3428, "sceUtilityMsgDialogShutdownStart" )] [Stateless]
						// /utility/psputility_msgdialog.h:57: void sceUtilityMsgDialogShutdownStart();
						void sceUtilityMsgDialogShutdownStart(){}

						[NotImplemented]
						[BiosFunction( 0x95FC253B, "sceUtilityMsgDialogUpdate" )] [Stateless]
						// /utility/psputility_msgdialog.h:73: void sceUtilityMsgDialogUpdate(int n);
						void sceUtilityMsgDialogUpdate( int n ){}

						[NotImplemented]
						[BiosFunction( 0x9A1C91D7, "sceUtilityMsgDialogGetStatus" )] [Stateless]
						// /utility/psputility_msgdialog.h:66: int sceUtilityMsgDialogGetStatus();
						int sceUtilityMsgDialogGetStatus(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF6269B82, "sceUtilityOskInitStart" )] [Stateless]
						// /utility/psputility_osk.h:86: int sceUtilityOskInitStart(SceUtilityOskParams* params);
						int sceUtilityOskInitStart( int params ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3DFAEBA9, "sceUtilityOskShutdownStart" )] [Stateless]
						// /utility/psputility_osk.h:92: int sceUtilityOskShutdownStart();
						int sceUtilityOskShutdownStart(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4B85C861, "sceUtilityOskUpdate" )] [Stateless]
						// /utility/psputility_osk.h:99: int sceUtilityOskUpdate(int n);
						int sceUtilityOskUpdate( int n ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF3F76017, "sceUtilityOskGetStatus" )] [Stateless]
						// /utility/psputility_osk.h:106: int sceUtilityOskGetStatus();
						int sceUtilityOskGetStatus(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x45C18506, "sceUtilitySetSystemParamInt" )] [Stateless]
						// /utility/psputility_sysparam.h:104: int sceUtilitySetSystemParamInt(int id, int value);
						int sceUtilitySetSystemParamInt( int id, int value ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x41E30674, "sceUtilitySetSystemParamString" )] [Stateless]
						// /utility/psputility_sysparam.h:113: int sceUtilitySetSystemParamString(int id, const char *str);
						int sceUtilitySetSystemParamString( int id, int str ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA5DA2406, "sceUtilityGetSystemParamInt" )] [Stateless]
						// /utility/psputility_sysparam.h:122: int sceUtilityGetSystemParamInt( int id, int *value );
						int sceUtilityGetSystemParamInt( int id, int value ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x34B78343, "sceUtilityGetSystemParamString" )] [Stateless]
						// /utility/psputility_sysparam.h:132: int sceUtilityGetSystemParamString(int id, char *str, int len);
						int sceUtilityGetSystemParamString( int id, int str, int len ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5EEE6548, "sceUtilityCheckNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:60: int sceUtilityCheckNetParam(int id);
						int sceUtilityCheckNetParam( int id ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x434D4B3A, "sceUtilityGetNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:71: int sceUtilityGetNetParam(int conf, int param, netData *data);
						int sceUtilityGetNetParam( int conf, int param, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x072DEBF2, "sceUtilityCreateNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:81: int sceUtilityCreateNetParam(int conf);
						int sceUtilityCreateNetParam( int conf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9CE50172, "sceUtilityDeleteNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:111: int sceUtilityDeleteNetParam(int conf);
						int sceUtilityDeleteNetParam( int conf ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFB0C4840, "sceUtilityCopyNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:102: int sceUtilityCopyNetParam(int src, int dest);
						int sceUtilityCopyNetParam( int src, int dest ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFC4516F3, "sceUtilitySetNetParam" )] [Stateless]
						// /utility/psputility_netparam.h:92: int sceUtilitySetNetParam(int param, const void *val);
						int sceUtilitySetNetParam( int param, int val ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 70AC8FBF */
