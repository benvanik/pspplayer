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
						// int sceRegOpenRegistry(struct RegParam *reg, int mode, REGHANDLE *h); (/registry/pspreg.h:68)
						int sceRegOpenRegistry( int reg, int mode, int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFA8A5739, "sceRegCloseRegistry" )] [Stateless]
						// int sceRegCloseRegistry(REGHANDLE h); (/registry/pspreg.h:86)
						int sceRegCloseRegistry( int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDEDA92BF, "sceRegRemoveRegistry" )] [Stateless]
						// int sceRegRemoveRegistry(struct RegParam *reg); (/registry/pspreg.h:229)
						int sceRegRemoveRegistry( int reg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1D8A762E, "sceRegOpenCategory" )] [Stateless]
						// int sceRegOpenCategory(REGHANDLE h, const char *name, int mode, REGHANDLE *hd); (/registry/pspreg.h:98)
						int sceRegOpenCategory( int h, int name, int mode, int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0CAE832B, "sceRegCloseCategory" )] [Stateless]
						// int sceRegCloseCategory(REGHANDLE hd); (/registry/pspreg.h:117)
						int sceRegCloseCategory( int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x39461B4D, "sceRegFlushRegistry" )] [Stateless]
						// int sceRegFlushRegistry(REGHANDLE h); (/registry/pspreg.h:77)
						int sceRegFlushRegistry( int h ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0D69BF40, "sceRegFlushCategory" )] [Stateless]
						// int sceRegFlushCategory(REGHANDLE hd); (/registry/pspreg.h:126)
						int sceRegFlushCategory( int hd ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57641A81, "sceRegCreateKey" )] [Stateless]
						// int sceRegCreateKey(REGHANDLE hd, const char *name, int type, SceSize size); (/registry/pspreg.h:220)
						int sceRegCreateKey( int hd, int name, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x17768E14, "sceRegSetKeyValue" )] [Stateless]
						// int sceRegSetKeyValue(REGHANDLE hd, const char *name, const void *buf, SceSize size); (/registry/pspreg.h:187)
						int sceRegSetKeyValue( int hd, int name, int buf, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD4475AA8, "sceRegGetKeyInfo" )] [Stateless]
						// int sceRegGetKeyInfo(REGHANDLE hd, const char *name, REGHANDLE *hk, unsigned int *type, SceSize *size); (/registry/pspreg.h:139)
						int sceRegGetKeyInfo( int hd, int name, int hk, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28A8E98A, "sceRegGetKeyValue" )] [Stateless]
						// int sceRegGetKeyValue(REGHANDLE hd, REGHANDLE hk, void *buf, SceSize size); (/registry/pspreg.h:163)
						int sceRegGetKeyValue( int hd, int hk, int buf, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2C0DB9DD, "sceRegGetKeysNum" )] [Stateless]
						// int sceRegGetKeysNum(REGHANDLE hd, int *num); (/registry/pspreg.h:197)
						int sceRegGetKeysNum( int hd, int num ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2D211135, "sceRegGetKeys" )] [Stateless]
						// int sceRegGetKeys(REGHANDLE hd, char *buf, int num); (/registry/pspreg.h:208)
						int sceRegGetKeys( int hd, int buf, int num ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4CA16893, "sceRegRemoveCategory" )] [Stateless]
						// int sceRegRemoveCategory(REGHANDLE h, const char *name); (/registry/pspreg.h:108)
						int sceRegRemoveCategory( int h, int name ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC5768D02, "sceRegGetKeyInfoByName" )] [Stateless]
						// int sceRegGetKeyInfoByName(REGHANDLE hd, const char *name, unsigned int *type, SceSize *size); (/registry/pspreg.h:151)
						int sceRegGetKeyInfoByName( int hd, int name, int type, int size ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x30BE0259, "sceRegGetKeyValueByName" )] [Stateless]
						// int sceRegGetKeyValueByName(REGHANDLE hd, const char *name, void *buf, SceSize size); (/registry/pspreg.h:175)
						int sceRegGetKeyValueByName( int hd, int name, int buf, int size ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - 188528B0 */
