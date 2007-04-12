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
				
				ref class R4000Capabilities : ICpuCapabilities
				{
				public:
					property bool AvcSupported
					{
						virtual bool get()
						{
							return false;
						}
					}

					property bool DebuggingSupported
					{
						virtual bool get()
						{
							return true;
						}
					}

					property Cpu::CpuStatisticsCapabilities SupportedStatistics
					{
						virtual Cpu::CpuStatisticsCapabilities get()
						{
//#ifdef STATISTICS
//							return Cpu::CpuStatisticsCapabilities::InstructionsPerSecond;
//#else
							return Cpu::CpuStatisticsCapabilities::None;
//#endif
						}
					}

				};

			}
		}
	}
}
