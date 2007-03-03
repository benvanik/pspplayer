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
						// unsigned int sceGeEdramGetSize(); (/ge/pspge.h:47)
						int sceGeEdramGetSize(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE47E40E4, "sceGeEdramGetAddr" )] [Stateless]
						// void * sceGeEdramGetAddr(); (/ge/pspge.h:54)
						int sceGeEdramGetAddr(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDC93CFEF, "sceGeGetCmd" )] [Stateless]
						// unsigned int sceGeGetCmd(int cmd); (/ge/pspge.h:63)
						int sceGeGetCmd( int cmd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57C8945B, "sceGeGetMtx" )] [Stateless]
						// int sceGeGetMtx(int type, void *matrix); (/ge/pspge.h:93)
						int sceGeGetMtx( int type, int matrix ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x438A385A, "sceGeSaveContext" )] [Stateless]
						// int sceGeSaveContext(PspGeContext *context); (/ge/pspge.h:102)
						int sceGeSaveContext( int context ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0BF608FB, "sceGeRestoreContext" )] [Stateless]
						// int sceGeRestoreContext(const PspGeContext *context); (/ge/pspge.h:111)
						int sceGeRestoreContext( int context ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAB49E76A, "sceGeListEnQueue" )] [Stateless]
						// int sceGeListEnQueue(const void *list, void *stall, int cbid, void *arg); (/ge/pspge.h:124)
						int sceGeListEnQueue( int list, int stall, int cbid, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1C0D95A6, "sceGeListEnQueueHead" )] [Stateless]
						// int sceGeListEnQueueHead(const void *list, void *stall, int cbid, void *arg); (/ge/pspge.h:137)
						int sceGeListEnQueueHead( int list, int stall, int cbid, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5FB86AB0, "sceGeListDeQueue" )] [Stateless]
						// int sceGeListDeQueue(int qid); (/ge/pspge.h:146)
						int sceGeListDeQueue( int qid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE0D68148, "sceGeListUpdateStallAddr" )] [Stateless]
						// int sceGeListUpdateStallAddr(int qid, void *stall); (/ge/pspge.h:156)
						int sceGeListUpdateStallAddr( int qid, int stall ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x03444EB4, "sceGeListSync" )] [Stateless]
						// int sceGeListSync(int qid, int syncType); (/ge/pspge.h:176)
						int sceGeListSync( int qid, int syncType ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB287BD61, "sceGeDrawSync" )] [Stateless]
						// int sceGeDrawSync(int syncType); (/ge/pspge.h:185)
						int sceGeDrawSync( int syncType ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB448EC0D, "sceGeBreak" )] [Stateless]
						// manual add
						int sceGeBreak( int mode, int unknown ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4C06E472, "sceGeContinue" )] [Stateless]
						// manual add
						int sceGeContinue(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA4FC06A4, "sceGeSetCallback" )] [Stateless]
						// int sceGeSetCallback(PspGeCallbackData *cb); (/ge/pspge.h:193)
						int sceGeSetCallback( int cb ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x05DB22CE, "sceGeUnsetCallback" )] [Stateless]
						// int sceGeUnsetCallback(int cbid); (/ge/pspge.h:201)
						int sceGeUnsetCallback( int cbid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - BF91B5C0 */
