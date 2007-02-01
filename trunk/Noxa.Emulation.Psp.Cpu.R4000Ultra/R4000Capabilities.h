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
					property Cpu::Endianess Endianess
					{
						virtual Cpu::Endianess get()
						{
							return Cpu::Endianess::LittleEndian;
						}
					}

					property bool VectorFpuSupported
					{
						virtual bool get()
						{
							return false;
						}
					}

					property bool DmaSupported
					{
						virtual bool get()
						{
							return false;
						}
					}

					property bool AvcSupported
					{
						virtual bool get()
						{
							return false;
						}
					}

					property bool InternalMemorySupported
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
							return Cpu::CpuStatisticsCapabilities::None;
						}
					}

					property bool DebuggingSupported
					{
						virtual bool get()
						{
							return false;
						}
					}
				};

			}
		}
	}
}
