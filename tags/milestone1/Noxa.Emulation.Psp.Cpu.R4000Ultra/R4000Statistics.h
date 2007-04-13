// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Statistics : ICpuStatistics
				{
				public:
					R4000Statistics(){}

					// We have this master #if here not so much cause it matters, but to ensure that
					// I'm not recording stats when I shouldn't be
#ifdef STATISTICS

					uint InstructionsExecuted;
					uint ExecutionLoops;						// # of times the execution loop has run
					uint CodeBlocksExecuted;
					uint CodeBlocksGenerated;

					uint CodeCacheHits;
					uint CodeCacheMisses;
					uint CodeCacheBlockCount;					// # of blocks in the cache
					uint CodeCacheLevel2Count;					// # of 2nd level lists
					uint CodeCacheLevel3Count;					// # of 3rd level lists
					uint CodeCacheTableSize;					// Size, in bytes, of the table

					int AverageCodeBlockLength;
					int LargestCodeBlockLength;

					uint JumpBlockInlineCount;					// # of inline jumps
					uint JumpBlockThunkCount;					// # of jump block thunks
					uint JumpBlockLookupCount;					// # of jump lookup bounces
					uint CodeBlockRetCount;						// # of rets

					uint JumpBlockThunkCalls;					// # of times thunks were called
					uint JumpBlockThunkBuilds;					// # of times thunks built new blocks
					uint JumpBlockThunkHits;					// # of times thunks had code cache hits and just did fixups
					uint JumpBlockInlineHits;					// # of times inline block was able to find target
					uint JumpBlockInlineMisses;					// # of times inline block had to return from bounce

					uint ManagedMemoryReadCount;				// # of times R4000Memory was used to read instead of the inline code
					uint ManagedMemoryWriteCount;				// # of times R4000Memory was used to write instead of the inline code

					uint ManagedSyscallCount;					// # of syscalls that go to the BIOS (managed)
					uint NativeSyscallCount;					// # of syscalls that use overriden stubs (native)
					uint UnimplementedSyscallCount;				// # of syscalls attempted against unimplemented calls

					double AverageCodeSizeRatio;				// Ratio of MIPS code size to x86 code size

					double AverageGenerationTime;

#endif

					double IPS;
					double RunTime;

					void GatherStats();

					property int InstructionsPerSecond
					{
						virtual int get()
						{
							return ( int )IPS;
						}
					}
				};

			}
		}
	}
}
