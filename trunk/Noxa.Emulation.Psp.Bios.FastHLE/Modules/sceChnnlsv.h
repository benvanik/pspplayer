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

					ref class sceChnnlsv : public Module
					{
					public:
						sceChnnlsv( Kernel^ kernel ) : Module( kernel ) {}
						~sceChnnlsv(){}

						property String^ Name { virtual String^ get() override { return "sceChnnlsv"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0xE7833020, "sceChnnlsv_E7833020" )] [Stateless]
						// /vsh/pspchnnlsv.h:53: int sceChnnlsv_E7833020(pspChnnlsvContext1 *ctx, int mode);
						int sceChnnlsv_E7833020( int ctx, int mode ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF21A1FCA, "sceChnnlsv_F21A1FCA" )] [Stateless]
						// /vsh/pspchnnlsv.h:63: int sceChnnlsv_F21A1FCA(pspChnnlsvContext1 *ctx, unsigned char *data, int len);
						int sceChnnlsv_F21A1FCA( int ctx, int data, int len ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC4C494F8, "sceChnnlsv_C4C494F8" )] [Stateless]
						// /vsh/pspchnnlsv.h:73: int sceChnnlsv_C4C494F8(pspChnnlsvContext1 *ctx, unsigned char *hash, unsigned char *cryptkey);
						int sceChnnlsv_C4C494F8( int ctx, int hash, int cryptkey ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xABFDFC8B, "sceChnnlsv_ABFDFC8B" )] [Stateless]
						// /vsh/pspchnnlsv.h:86: int sceChnnlsv_ABFDFC8B(pspChnnlsvContext2 *ctx, int mode1, int mode2, unsigned char *hashkey, unsigned char *cipherkey);
						int sceChnnlsv_ABFDFC8B( int ctx, int mode1, int mode2, int hashkey, int cipherkey ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x850A7FA1, "sceChnnlsv_850A7FA1" )] [Stateless]
						// /vsh/pspchnnlsv.h:97: int sceChnnlsv_850A7FA1(pspChnnlsvContext2 *ctx, unsigned char *data, int len);
						int sceChnnlsv_850A7FA1( int ctx, int data, int len ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x21BE78B4, "sceChnnlsv_21BE78B4" )] [Stateless]
						// /vsh/pspchnnlsv.h:105: int sceChnnlsv_21BE78B4(pspChnnlsvContext2 *ctx);
						int sceChnnlsv_21BE78B4( int ctx ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B6611215 */
