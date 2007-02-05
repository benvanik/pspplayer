// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

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
				ref class R4000Cache;
				ref class R4000GenContext;
				ref class CodeBlock;
				class R4000Generator;

				ref class R4000BlockBuilder abstract
				{
				internal:
					R4000Cpu^			_cpu;
					R4000Core^			_core;
					R4000Memory^		_memory;
					R4000Cache^			_codeCache;

					R4000GenContext^	_ctx;
					R4000Generator*		_gen;

				protected:
					virtual int InternalBuild( int address ) = 0;

				public:
					R4000BlockBuilder( R4000Cpu^ cpu, R4000Core^ core );
					~R4000BlockBuilder();

					CodeBlock^ Build( int address );
					void* BuildBounce();

					void EmitJumpBlock( int targetAddress, int sourceAddress );
				};

			}
		}
	}
}
