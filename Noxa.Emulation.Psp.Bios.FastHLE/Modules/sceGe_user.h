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

					ref class sceGe_user : public Module
					{
					public:
						sceGe_user( Kernel^ kernel ) : Module( kernel ) {}
						~sceGe_user(){}

						property String^ Name { virtual String^ get() override { return "sceGe_user"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x1F6752AD, "sceGeEdramGetSize" )] [Stateless]
						// /ge/pspge.h:47: unsigned int sceGeEdramGetSize();
						int sceGeEdramGetSize(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE47E40E4, "sceGeEdramGetAddr" )] [Stateless]
						// /ge/pspge.h:54: void * sceGeEdramGetAddr();
						int sceGeEdramGetAddr(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDC93CFEF, "sceGeGetCmd" )] [Stateless]
						// /ge/pspge.h:63: unsigned int sceGeGetCmd(int cmd);
						int sceGeGetCmd( int cmd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57C8945B, "sceGeGetMtx" )] [Stateless]
						// /ge/pspge.h:93: int sceGeGetMtx(int type, void *matrix);
						int sceGeGetMtx( int type, int matrix ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x438A385A, "sceGeSaveContext" )] [Stateless]
						// /ge/pspge.h:102: int sceGeSaveContext(PspGeContext *context);
						int sceGeSaveContext( int context ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0BF608FB, "sceGeRestoreContext" )] [Stateless]
						// /ge/pspge.h:111: int sceGeRestoreContext(const PspGeContext *context);
						int sceGeRestoreContext( int context ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAB49E76A, "sceGeListEnQueue" )] [Stateless]
						// /ge/pspge.h:124: int sceGeListEnQueue(const void *list, void *stall, int cbid, void *arg);
						int sceGeListEnQueue( int list, int stall, int cbid, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1C0D95A6, "sceGeListEnQueueHead" )] [Stateless]
						// /ge/pspge.h:137: int sceGeListEnQueueHead(const void *list, void *stall, int cbid, void *arg);
						int sceGeListEnQueueHead( int list, int stall, int cbid, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5FB86AB0, "sceGeListDeQueue" )] [Stateless]
						// /ge/pspge.h:146: int sceGeListDeQueue(int qid);
						int sceGeListDeQueue( int qid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE0D68148, "sceGeListUpdateStallAddr" )] [Stateless]
						// /ge/pspge.h:156: int sceGeListUpdateStallAddr(int qid, void *stall);
						int sceGeListUpdateStallAddr( int qid, int stall ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x03444EB4, "sceGeListSync" )] [Stateless]
						// /ge/pspge.h:176: int sceGeListSync(int qid, int syncType);
						int sceGeListSync( int qid, int syncType ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB287BD61, "sceGeDrawSync" )] [Stateless]
						// /ge/pspge.h:185: int sceGeDrawSync(int syncType);
						int sceGeDrawSync( int syncType ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA4FC06A4, "sceGeSetCallback" )] [Stateless]
						// /ge/pspge.h:193: int sceGeSetCallback(PspGeCallbackData *cb);
						int sceGeSetCallback( int cb ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x05DB22CE, "sceGeUnsetCallback" )] [Stateless]
						// /ge/pspge.h:201: int sceGeUnsetCallback(int cbid);
						int sceGeUnsetCallback( int cbid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 50569958 */
