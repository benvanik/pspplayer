// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"
#include "KernelCallback.h"
#include "KernelEvent.h"
#include "KernelPartition.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Games;
using namespace Noxa::Emulation::Psp::Media;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class FastBios;
				ref class KernelDevice;
				ref class KernelFileHandle;
				ref class KernelPartition;
				ref class KernelStatistics;
				ref class KernelThread;

				ref class Kernel : public IKernel
				{
				internal:
					FastBios^							_bios;
					IEmulationInstance^					_emu;
					ICpu^								_cpu;
					ICpuCore^							_core0;
					GameInformation^					_game;
					AutoResetEvent^						_gameEvent;

					int									_lastId;
					Dictionary<int, KernelHandle^>^		_handles;

					KernelThread^						_activeThread;
					List<KernelThread^>^				_threadsWaitingOnEvents;

					List<KernelDevice^>^				_devices;
					Dictionary<String^, KernelDevice^>^	_deviceMap;

				public:
					PerformanceTimer^					Timer;
					double								StartTime;
					uint								StartTick;
					DateTime							UnixBaseTime;

					KernelStatistics^					Statistics;

					Dictionary<int, KernelThread^>^		Threads;
					array<KernelPartition^>^			Partitions;

					KernelFileHandle^					StdIn;
					KernelFileHandle^					StdOut;
					KernelFileHandle^					StdErr;

					IMediaFolder^						CurrentPath;

					Dictionary<KernelCallbackType, KernelCallback^>^	Callbacks;

				public:
					Kernel( FastBios^ bios );

					property GameInformation^ Game
					{
						virtual GameInformation^ get();
						virtual void set( GameInformation^ value );
					}

					property KernelThread^ ActiveThread
					{
						KernelThread^ get()
						{
							return _activeThread;
						}
					}

					void StartGame();
					void ExitGame();

					virtual void Execute();

					void AddHandle( KernelHandle^ handle );
					void RemoveHandle( KernelHandle^ handle );

					__inline KernelHandle^ FindHandle( int id );
					__inline KernelDevice^ FindDevice( String^ path );

				public:
					void CreateThread( KernelThread^ thread );
					void DeleteThread( KernelThread^ thread );
					__inline KernelThread^ FindThread( int id );
					void WaitThreadOnEvent( KernelThread^ thread, KernelEvent^ ev, int bitMask, int outAddress );
					void SignalEvent( KernelEvent^ ev );
					void ContextSwitch();

				public:
					/// <summary>
					/// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
					/// </summary>
					property uint ClockTime
					{
						uint get()
						{
							// 1000000 us per second
							// 10000000 ticks per second
							TimeSpan elapsed = DateTime::UtcNow - UnixBaseTime;
							return ( uint )( elapsed.Ticks / 10 );
						}
					}

					property double RunTime
					{
						double get()
						{
							return Timer->Elapsed - StartTime;
						}
					}

				private:
					void CreateStdio();
					int ThreadPriorityComparer( KernelThread^ a, KernelThread^ b );

				internal:
					int AllocateID()
					{
						return ++_lastId;
					}
				};

			}
		}
	}
}
