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
			namespace Video {

				ref class OglStatistics : public CounterSource
				{
				public:
					OglStatistics();

					Counter^	Frames;
					Counter^	SkippedFrames;
					Counter^	DisplayLists;
					Counter^	AbortedDisplayLists;

					virtual void Sample() override;

					void DumpCommandCounts();
				};

			}
		}
	}
}
