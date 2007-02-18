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

				//delegate int DynamicCodeDelegate( Core core0, Memory memory, int[] generalRegisters, BiosFunction[] syscallList, int branchTarget );

#pragma unmanaged
				int QuickPointerLookup( int address );
#pragma managed

				ref class CodeBlock
				{
				public:
					int			Address;
					int			Size;
					int			InstructionCount;
					void*		Pointer;
					bool		EndsOnSyscall;
					
					int			ExecutionCount;
				};

				ref class R4000Cache
				{
				protected:
					array<array<array<CodeBlock^>^>^>^	_lookup;
					int***			_ptrLookup;
					Object^			_syncRoot;

				public:
					R4000Cache();
					~R4000Cache();

				public:
					void Add( CodeBlock^ block );
					CodeBlock^ Find( int address );
					void Invalidate( int address );
					void Clear();
					void Clear( bool realloc );
				};

			}
		}
	}
}
