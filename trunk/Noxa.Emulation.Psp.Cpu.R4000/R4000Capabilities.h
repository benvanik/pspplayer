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
				};

			}
		}
	}
}
