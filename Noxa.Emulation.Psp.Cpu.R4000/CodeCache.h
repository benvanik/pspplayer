#pragma once

using namespace System;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {
				
				ref class CodeCache
				{
				protected:
					R4000Cpu^			_cpu;

					Dictionary<unsigned int, CodeSegment^>^	_cache;

				public:

					void Execute( unsigned int address )
					{
					}

				};

				// This may be better off being unmanaged
				ref class CodeSegment
				{
				public:
					unsigned int Address;
					unsigned int CodeLength;
				}
			}
		}
	}
}
