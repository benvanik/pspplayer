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

					uint InstructionsExecuted;
					uint ExecutionLoops;						// # of times the execution loop has run
					uint CodeBlocksExecuted;
					uint CodeBlocksGenerated;

					uint CodeCacheHits;
					uint CodeCacheMisses;

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

					uint BiosSyscallCount;						// # of syscalls that go to the BIOS (managed)
					uint NativeSyscallCount;					// # of syscalls that use overriden stubs (native)

					double AverageCodeSizeRatio;				// Ratio of MIPS code size to x86 code size

					double AverageGenerationTime;
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
