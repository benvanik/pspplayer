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
					int CodeBlocksExecuted;
					int CodeBlocksGenerated;

					int CodeCacheHits;
					int CodeCacheMisses;

					int AverageCodeBlockLength;
					int LargestCodeBlockLength;

					int JumpBlockInlineCount;					// # of code blocks ended with an inline jump
					int JumpBlockThunkCount;					// # of code blocks ended with a jump block thunk
					int CodeBlockRetCount;						// # of code blocks returned with ret

					int JumpBlockThunkCalls;					// # of times thunks were called
					int JumpBlockThunkBuilds;					// # of times thunks built new blocks
					int JumpBlockThunkHits;						// # of times thunks had code cache hits and just did fixups

					double AverageCodeSizeRatio;				// Ratio of MIPS code size to x86 code size

					double AverageGenerationTime;
					double IPS;
					double RunTime;

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
