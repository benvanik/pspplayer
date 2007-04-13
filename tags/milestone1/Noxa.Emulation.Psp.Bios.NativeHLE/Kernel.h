// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "MemoryPool.h"
#include "Vector.h"
#include "LL.h"
#include "CpuApi.h"
#include "gcref.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Cpu::Native;
using namespace Noxa::Emulation::Psp::Media;

#define PARTITIONCOUNT	7
#define INTERRUPTCOUNT	67
#define INTSLOTCOUNT	16
#define DEVICECOUNT		2

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class NativeBios;

				class HandleTable;

				class KThread;
				class KPartition;
				class KIntHandler;
				class KHandle;
				class KFile;
				class KDevice;
				class KModule;
				class KCallback;

				class Kernel;

				typedef void (*TimerFunction)( Kernel* kernel, void* args );

				typedef struct KernelTimer_t
				{
					Kernel*				Kernel;
					HANDLE				Handle;
					bool				Repeat;
					TimerFunction		Function;
					void*				Args;
				} KernelTimer;

				class Kernel
				{
				public:
					gcref<NativeBios^>			Bios;
					gcref<IEmulationInstance^>	Emu;
					gcref<ICpuCore^>			CpuCore;
					
					gcref<IMediaFolder^>		CurrentPath;

				public:
					CpuApi*				Cpu;
					MemorySystem*		Memory;
					MemoryPool*			MemoryPool;

					Vector<KModule*>*	Modules;
					KModule*			MainModule;

					HandleTable*		Handles;
					Vector<KThread*>*	Threads;
					LL<KThread*>*		SchedulableThreads;
					KPartition**		Partitions;

					KThread*			MainThread;
					KThread*			ActiveThread;

					KDevice**			Devices;
					KIntHandler***		Interrupts;		// List of 67 interrupts, each with 16 slots
					
					// Slotted callback lists - each slot is reserved by a module for a given callback type
					// and new callbacks are added to the lists
					Vector<LL<KCallback*>*>*	Callbacks;
					struct
					{
						int		Exit;			// Issued on 'home' press
						int		Umd;			// Issued on UMD state change
					}					CallbackTypes;

					KFile*				StdIn;
					KFile*				StdOut;
					KFile*				StdErr;

					int64				StartTick;
					int64				TickFrequenecy;

				public:
					Kernel( NativeBios^ bios );
					~Kernel();

					void StartGame();
					void StopGame( int exitCode );

					KernelTimer* AddTimer( TimerFunction function, void* args, int dueTimeMs, int intervalMs );
					void CancelTimer( KernelTimer* timer );

					bool Schedule();
					void Execute();

					int IssueCallback( int callbackId, int argument );
					void CallbackComplete();

					KDevice* FindDevice( const char* path );
					IMediaDevice^ FindMediaDevice( const char* path );
					
					// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
					int64 GetClockTime();
					// Time in microseconds since the game started.
					int64 GetRunTime();
					// Ticks, in TickFrequenecy, since whenever.
					int64 GetTick();
					
					__inline int AllocateUID(){ return _lastUid++; }

				public:
					int RegisterCallbackType();

				private:
					int					_lastUid;
					HANDLE				_hTimerQueue;

					KThread*			_oldThread;

				private:
					void CreateStdio();
					void SetupTimerQueue();
					void DestroyTimerQueue();
				};

			}
		}
	}
}
