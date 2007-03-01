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

					ref class sceReg : public Module
					{
					public:
						sceReg( Kernel^ kernel ) : Module( kernel ) {}
						~sceReg(){}

						property String^ Name { virtual String^ get() override { return "sceReg"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x92E41280, "sceRegOpenRegistry" )] [Stateless]
						// /registry/pspreg.h:68: int sceRegOpenRegistry(struct RegParam *reg, int mode, REGHANDLE *h);
						int sceRegOpenRegistry( int reg, int mode, int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFA8A5739, "sceRegCloseRegistry" )] [Stateless]
						// /registry/pspreg.h:86: int sceRegCloseRegistry(REGHANDLE h);
						int sceRegCloseRegistry( int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDEDA92BF, "sceRegRemoveRegistry" )] [Stateless]
						// /registry/pspreg.h:229: int sceRegRemoveRegistry(struct RegParam *reg);
						int sceRegRemoveRegistry( int reg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1D8A762E, "sceRegOpenCategory" )] [Stateless]
						// /registry/pspreg.h:98: int sceRegOpenCategory(REGHANDLE h, const char *name, int mode, REGHANDLE *hd);
						int sceRegOpenCategory( int h, int name, int mode, int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0CAE832B, "sceRegCloseCategory" )] [Stateless]
						// /registry/pspreg.h:117: int sceRegCloseCategory(REGHANDLE hd);
						int sceRegCloseCategory( int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x39461B4D, "sceRegFlushRegistry" )] [Stateless]
						// /registry/pspreg.h:77: int sceRegFlushRegistry(REGHANDLE h);
						int sceRegFlushRegistry( int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0D69BF40, "sceRegFlushCategory" )] [Stateless]
						// /registry/pspreg.h:126: int sceRegFlushCategory(REGHANDLE hd);
						int sceRegFlushCategory( int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57641A81, "sceRegCreateKey" )] [Stateless]
						// /registry/pspreg.h:220: int sceRegCreateKey(REGHANDLE hd, const char *name, int type, SceSize size);
						int sceRegCreateKey( int hd, int name, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x17768E14, "sceRegSetKeyValue" )] [Stateless]
						// /registry/pspreg.h:187: int sceRegSetKeyValue(REGHANDLE hd, const char *name, const void *buf, SceSize size);
						int sceRegSetKeyValue( int hd, int name, int buf, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD4475AA8, "sceRegGetKeyInfo" )] [Stateless]
						// /registry/pspreg.h:139: int sceRegGetKeyInfo(REGHANDLE hd, const char *name, REGHANDLE *hk, unsigned int *type, SceSize *size);
						int sceRegGetKeyInfo( int hd, int name, int hk, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28A8E98A, "sceRegGetKeyValue" )] [Stateless]
						// /registry/pspreg.h:163: int sceRegGetKeyValue(REGHANDLE hd, REGHANDLE hk, void *buf, SceSize size);
						int sceRegGetKeyValue( int hd, int hk, int buf, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2C0DB9DD, "sceRegGetKeysNum" )] [Stateless]
						// /registry/pspreg.h:197: int sceRegGetKeysNum(REGHANDLE hd, int *num);
						int sceRegGetKeysNum( int hd, int num ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2D211135, "sceRegGetKeys" )] [Stateless]
						// /registry/pspreg.h:208: int sceRegGetKeys(REGHANDLE hd, char *buf, int num);
						int sceRegGetKeys( int hd, int buf, int num ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4CA16893, "sceRegRemoveCategory" )] [Stateless]
						// /registry/pspreg.h:108: int sceRegRemoveCategory(REGHANDLE h, const char *name);
						int sceRegRemoveCategory( int h, int name ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC5768D02, "sceRegGetKeyInfoByName" )] [Stateless]
						// /registry/pspreg.h:151: int sceRegGetKeyInfoByName(REGHANDLE hd, const char *name, unsigned int *type, SceSize *size);
						int sceRegGetKeyInfoByName( int hd, int name, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x30BE0259, "sceRegGetKeyValueByName" )] [Stateless]
						// /registry/pspreg.h:175: int sceRegGetKeyValueByName(REGHANDLE hd, const char *name, void *buf, SceSize size);
						int sceRegGetKeyValueByName( int hd, int name, int buf, int size ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 5AC010C4 */
