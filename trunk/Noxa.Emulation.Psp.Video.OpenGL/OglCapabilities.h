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
			namespace Video {
				
				ref class OglCapabilities : public IVideoCapabilities
				{
				public:
					property VideoStatisticsCapabilities SupportedStatistics
					{
						virtual VideoStatisticsCapabilities get()
						{
#ifdef STATISTICS
							return VideoStatisticsCapabilities::FramesPerSecond;
#else
							return VideoStatisticsCapabilities::None;
#endif
						}
					}
				};

			}
		}
	}
}
