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

					int InstructionsExecuted;
					int ExecutionLoops;							// # of times the execution loop has run
					int CodeBlocksExecuted;
					int CodeBlocksGenerated;

					int CodeCacheHits;
					int CodeCacheMisses;

					int AverageCodeBlockLength;
					int LargestCodeBlockLength;

					int JumpBlockInlineCount;					// # of inline jumps
					int JumpBlockThunkCount;					// # of jump block thunks
					int JumpBlockLookupCount;					// # of jump lookup bounces
					int CodeBlockRetCount;						// # of rets

					int JumpBlockThunkCalls;					// # of times thunks were called
					int JumpBlockThunkBuilds;					// # of times thunks built new blocks
					int JumpBlockThunkHits;						// # of times thunks had code cache hits and just did fixups
					int JumpBlockInlineHits;					// # of times inline block was able to find target
					int JumpBlockInlineMisses;					// # of times inline block had to return from bounce

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
