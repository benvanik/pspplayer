// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Debugging::Statistics;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Statistics : public CounterSource
				{
				public:
					R4000Statistics();

					// We have this master #if here not so much cause it matters, but to ensure that
					// I'm not recording stats when I shouldn't be
#ifdef STATISTICS

					Counter^	InstructionsExecuted;
					Counter^	ExecutionLoops;							// # of times the execution loop has run
					Counter^	CodeBlocksExecuted;
					Counter^	CodeBlocksGenerated;

					Counter^	CodeCacheHits;
					Counter^	CodeCacheMisses;
					Counter^	CodeCacheBlockCount;					// # of blocks in the cache

					Counter^	CodeBlockLength;						// # of instructions per code block

					Counter^	JumpBlockInlineCount;					// # of inline jumps
					Counter^	JumpBlockThunkCount;					// # of jump block thunks
					Counter^	JumpBlockLookupCount;					// # of jump lookup bounces
					Counter^	CodeBlockRetCount;						// # of rets

					Counter^	JumpBlockThunkCalls;					// # of times thunks were called
					Counter^	JumpBlockThunkBuilds;					// # of times thunks built new blocks
					Counter^	JumpBlockThunkHits;						// # of times thunks had code cache hits and just did fixups
					Counter^	JumpBlockInlineHits;					// # of times inline block was able to find target
					Counter^	JumpBlockInlineMisses;					// # of times inline block had to return from bounce

					Counter^	ManagedMemoryReadCount;					// # of times R4000Memory was used to read instead of the inline code
					Counter^	ManagedMemoryWriteCount;				// # of times R4000Memory was used to write instead of the inline code

					Counter^	ManagedSyscallCount;					// # of syscalls that go to the BIOS (managed)
					Counter^	NativeSyscallCount;						// # of syscalls that use overriden stubs (native)
					Counter^	CpuSyscallCount;						// # of syscalls to CPU-implemented routines
					Counter^	UnimplementedSyscallCount;				// # of syscalls attempted against unimplemented calls

					Counter^	CodeSizeRatio;							// Ratio of MIPS code size to x86 code size

					Counter^	GenerationTime;							// Time to generate blocks, in seconds

#endif
					
					virtual void Sample() override;
				};

			}
		}
	}
}
