// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000Cache.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;
				ref class R4000Core;
				ref class R4000Memory;
				ref class R4000GenContext;
				class R4000Generator;

				ref class R4000BlockBuilder abstract
				{
				internal:
					R4000Cpu^			_cpu;
					R4000Core^			_core;
					R4000Memory^		_memory;
					R4000Cache*			_codeCache;

					R4000GenContext^	_ctx;
					R4000Generator*		_gen;

				protected:
					virtual int InternalBuild( int startAddress, CodeBlock* block ) = 0;

				public:
					R4000BlockBuilder( R4000Cpu^ cpu, R4000Core^ core );
					~R4000BlockBuilder();

					void EmitTrace( int address, int code );
					void EmitDebug( int address, int code, char* codeString );

					CodeBlock* Build( int address );
					void* BuildBounce();

					void EmitJumpBlock( int targetAddress );
					void EmitJumpBlockEbx();
				};

			}
		}
	}
}
