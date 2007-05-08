// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::DebugData;
using namespace Noxa::Emulation::Psp::Debugging::DebugModel;
using namespace Noxa::Emulation::Psp::Games;
using namespace Noxa::Emulation::Psp::Utilities;

#include "CpuApi.h"
#include "R4000Cache.h"
#include "R4000Capabilities.h"
#include "R4000Core.h"
#include "R4000GenContext.h"
#include "R4000Memory.h"
#include "R4000Statistics.h"

using namespace Noxa::Emulation::Psp::Cpu::Native;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class UltraCpu;
				ref class R4000BiosStubs;
				ref class R4000VideoInterface;
				ref class R4000BlockBuilder;

				delegate void BiosShim( R4000Cpu^ cpu );

				ref class R4000Cpu : ICpu, ICpuHook
				{
				public:
					static R4000Cpu^			GlobalCpu;

				protected:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					R4000Capabilities^			_caps;

					bool						_hasExecuted;

				internal:
					R4000Statistics^			_stats;
#ifdef STATISTICS
					PerformanceTimer^			_timer;
					double						_timeSinceLastIpsPrint;
#endif
					TimerQueue^					_timerQueue;

				internal:
					IDebugger^					_debugger;

					R4000Memory^				_memory;
					R4000Core^					_core0;

					R4000Cache*					_codeCache;
					R4000GenContext^			_context;
					R4000BlockBuilder^			_builder;
					R4000BiosStubs^				_biosStubs;
					R4000VideoInterface^		_videoInterface;

					void*						_ctx;
					void*						_bounce;

					CpuApi*						_nativeInterface;
					List<IntPtr>^				_threadContexts;

					bool						_broken;
					CpuResumeCallback^			_resumeCallback;
					Object^						_resumeState;

				internal:
					int							_lastSyscall;
					array<BiosFunction^>^		_syscalls;
					array<BiosShim^>^			_syscallShims;
					array<IntPtr>^				_syscallShimsN;
#ifdef SYSCALLSTATS
					array<int>^					_syscallCounts;
#endif
					array<IModule^>^			_moduleInstances;
					Dictionary<uint, uint>^		_userExports;		// NID -> address

					FieldInfo^					_privateMemoryFieldInfo;
					FieldInfo^					_privateModuleInstancesFieldInfo;

					IProgramDebugData^			_symbols;

				public:

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

					property Type^ Factory
					{
						virtual Type^ get()
						{
							return UltraCpu::typeid;
						}
					}

					property ICpuCapabilities^ Capabilities
					{
						virtual ICpuCapabilities^ get()
						{
							return _caps;
						}
					}

					property ICpuStatistics^ Statistics
					{
						virtual ICpuStatistics^ get()
						{
							return _stats;
						}
					}

					property array<ICpuCore^>^ Cores
					{
						virtual array<ICpuCore^>^ get()
						{
							array<ICpuCore^>^ ret = gcnew array<ICpuCore^>( 1 );
							ret[ 0 ] = _core0;
							return ret;
						}
					}

					property ICpuCore^ default[ int ]
					{
						virtual ICpuCore^ get( int core )
						{
							switch( core )
							{
							case 0:
								return ( ICpuCore^ )_core0;
							default:
								return nullptr;
							}
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

					property IntPtr NativeInterface
					{
						virtual IntPtr get()
						{
							return IntPtr( _nativeInterface );
						}
					}

					property bool DebuggingEnabled
					{
						virtual bool get()
						{
							return ( _debugger != nullptr );
						}
					}
					
					property IDebugger^ Debugger
					{
						virtual IDebugger^ get()
						{
							return _debugger;
						}
					}

					property ICpuHook^ DebugHook
					{
						virtual ICpuHook^ get()
						{
							return this;
						}
					}

					property R4000Cache* CodeCache
					{
						virtual R4000Cache* get()
						{
							return _codeCache;
						}
					}

					R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~R4000Cpu();

					virtual void EnableDebugging( IDebugger^ debugger );

					virtual void Cleanup();

					// -- Syscalls / Exports
					virtual uint RegisterSyscall( uint nid );
					virtual void RegisterUserExports( BiosModule^ module );
					virtual uint LookupUserExport( uint nid );

					// -- Interrupts
					property uint InterruptsMask
					{
						virtual uint get();
						virtual void set( uint value );
					}
					virtual void RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument );
					virtual void UnregisterInterruptHandler( int interruptNumber, int slot );
					virtual void SetPendingInterrupt( int interruptNumber );

					// -- Threading
					virtual int AllocateContextStorage( uint pc, array<uint>^ registers );
					virtual void ReleaseContextStorage( int tcsId );
					virtual void SetContextSafetyCallback( int tcsId, ContextSafetyDelegate^ callback, int state );
					virtual uint GetContextRegister( int tcsId, int reg );
					virtual void SetContextRegister( int tcsId, int reg, uint value );
					virtual void SwitchContext( int newTcsId );
					virtual void MarshalCall( int tcsId, uint address, array<uint>^ arguments, MarshalCompleteDelegate^ resultCallback, int state );

					virtual void SetupGame( GameInformation^ game, Stream^ bootStream );
					
					virtual void Execute(
						[System::Runtime::InteropServices::Out] bool% breakFlag,
						[System::Runtime::InteropServices::Out] uint% instructionsExecuted );
					virtual void BreakExecution();

					// -- Execution Control
					virtual void Resume();
					void ResumeInternal( bool timedOut );
					void BreakTimeout( Timer^ timer );
					virtual void BreakAndWait();
					virtual void BreakAndWait( int timeoutMs );
					virtual void BreakAndWait( int timeoutMs, CpuResumeCallback^ callback, Object^ state );

					virtual void Stop();

					virtual void PrintStatistics();

				protected:
					int LookupOrAddModule( IModule^ module );
					BiosShim^ EmitShim( BiosFunction^ function, MemorySystem^ memory, void* registers );
					void* EmitShimN( BiosFunction^ function, NativeMemorySystem* memory, void* registers );

					void SetupNativeInterface();
					void DestroyNativeInterface();

					void SetupThreading();
					void DestroyThreading();

				public:
					virtual CoreState^ GetCoreState( int core );
					virtual array<Frame^>^ GetCallstack();
				};

			}
		}
	}
}
