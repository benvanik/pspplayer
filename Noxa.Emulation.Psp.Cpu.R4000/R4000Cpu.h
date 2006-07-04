#pragma once

#include "R4000Capabilities.h"
#include "R4000Clock.h"
#include "Memory.h"

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Core;
				
				ref class R4000Cpu : ICpu
				{
				protected:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					R4000Capabilities^			_caps;
					R4000Clock^					_clock;
					Memory^						_memory;
					array<R4000Core^>^			_cores;

#ifdef _DEBUG
					PerformanceTimer^			_timer;
					double						_timeSinceLastIpsPrint;

					bool						_debug;
#endif

				internal:
					int							_lastSyscall;
					array<BiosFunction^>^		_syscalls;

				public:
					R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters );

					property ComponentParameters^ Parameters
					{
						virtual ComponentParameters^ get()
						{
							return _params;
						}
					}

					property IEmulationInstance^ Emulator
					{
						virtual IEmulationInstance^ get()
						{
							return _emu;
						}
					}

					property ICpuCapabilities^ Capabilities
					{
						virtual ICpuCapabilities^ get()
						{
							return _caps;
						}
					}

					property IClock^ Clock
					{
						virtual IClock^ get()
						{
							return _clock;
						}
					}

					property array<ICpuCore^>^ Cores
					{
						virtual array<ICpuCore^>^ get()
						{
							return ( array<ICpuCore^>^ )_cores;
						}
					}

					property ICpuCore^ default[ int ]
					{
						virtual ICpuCore^ get( int core )
						{
							return ( ICpuCore^ )_cores[ core ];
						}
					}

					property IDmaController^ Dma
					{
						virtual IDmaController^ get()
						{
							return nullptr;
						}
					}

					property IAvcDecoder^ Avc
					{
						virtual IAvcDecoder^ get()
						{
							return nullptr;
						}
					}

					property IMemory^ Memory
					{
						virtual IMemory^ get()
						{
							return _memory;
						}
					}

					virtual void Cleanup()
					{
					}

					virtual int RegisterSyscall( unsigned int nid );

					virtual int ExecuteBlock();

					virtual void PrintStatistics()
					{
					}
				};

			}
		}
	}
}
