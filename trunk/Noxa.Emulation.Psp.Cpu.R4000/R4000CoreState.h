#pragma once

#include "R4000Cp0State.h"
#include "R4000Cp1State.h"
#include "R4000Cp2State.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000CoreState : ICpuCoreState
				{
				protected:

					ref class R4000CoreContext
					{
					public:
						
					};

				public:

					R4000CoreState( int cpuId )
					{
						
					}

					
				};

			}
		}
	}
}
