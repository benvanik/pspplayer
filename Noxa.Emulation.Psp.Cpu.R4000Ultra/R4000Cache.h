// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "DebugOptions.h"

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

#pragma unmanaged
				void* QuickPointerLookup( int address );
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

#ifdef DEBUGGING
					// This is a list of instruction expansion sizes
					// For ex, Sizes[5]=36 means that the instruction at address + 5 is 36 bytes in x86
					ushort*		InstructionSizes;
					// The size of the preamble, in bytes - after this begins the instructions
					byte		PreambleSize;
#endif
				} CodeBlock;

				class R4000Cache
				{
				protected:
					CodeBlock***	_lookup;
					int***			_ptrLookup;

				public:
					R4000Cache();
					~R4000Cache();

					int				Version;

				public:
					CodeBlock* Add( int address );
					void UpdatePointer( CodeBlock* block, void* pointer );
					
					// Finds the code block that starts at the given address
					CodeBlock* Find( int address );

					// Finds the code blocks that contains the given address
					int Search( int address, CodeBlock** buffer );

					void Invalidate( int address );
					void Clear();
					void Clear( bool realloc );
				};

			}
		}
	}
}
