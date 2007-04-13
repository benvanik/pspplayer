// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

#pragma unmanaged
				int QuickPointerLookup( int address );
#pragma managed

				typedef struct CodeBlock_t
				{
				public:
					int			Address;
					int			Size;
					int			InstructionCount;
					void*		Pointer;
					bool		EndsOnSyscall;
					
					int			ExecutionCount;
				} CodeBlock;

				class R4000Cache
				{
				protected:
					CodeBlock***	_lookup;
					int***			_ptrLookup;

				public:
					R4000Cache();
					~R4000Cache();

				public:
					CodeBlock* Add( int address );
					void UpdatePointer( CodeBlock* block, void* pointer );
					CodeBlock* Find( int address );
					void Invalidate( int address );
					void Clear();
					void Clear( bool realloc );
				};

			}
		}
	}
}
