// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Utilities;

#include "R4000Cache.h"
#include "R4000Capabilities.h"
#include "R4000Clock.h"
#include "R4000Core.h"
#include "R4000GenContext.h"
#include "R4000Memory.h"
#include "R4000Statistics.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class UltraCpu;
				ref class R4000BiosStubs;

				ref class R4000Cpu : ICpu
				{
				public:
					static R4000Cpu^			GlobalCpu;

				protected:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					R4000Capabilities^			_caps;
					R4000Clock^					_clock;
					
					EventHandler<BreakpointEventArgs^>^ _breakpointTriggeredHandler;

				internal:
					R4000Statistics^			_stats;
#ifdef STATISTICS
					PerformanceTimer^			_timer;
					double						_timeSinceLastIpsPrint;
#endif

				internal:
					R4000Memory^				_memory;
					R4000Core^					_core0;

					R4000Cache^					_codeCache;
					R4000GenContext^			_context;
					R4000BiosStubs^				_biosStubs;

					void*						_ctx;
					void*						_bounce;

				internal:
					int							_lastSyscall;
					array<BiosFunction^>^		_syscalls;
#ifdef SYSCALLSTATS
					array<int>^					_syscallCounts;
#endif

				public:

					event EventHandler<BreakpointEventArgs^>^ BreakpointTriggered
					{
						virtual void add( EventHandler<BreakpointEventArgs^>^ value )
						{
							_breakpointTriggeredHandler += value;
						}
						virtual void remove( EventHandler<BreakpointEventArgs^>^ value )
						{
							_breakpointTriggeredHandler -= value;
						}
					}

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

					property array<byte>^ InternalMemory
					{
						virtual array<byte>^ get()
						{
							return nullptr;
						}
					}

					property IntPtr InternalMemoryPointer
					{
						virtual IntPtr get()
						{
							return IntPtr( ( void* )_memory->MainMemory );
						}
					}

					property int InternalMemoryBaseAddress
					{
						virtual int get()
						{
							return 0x08000000;
						}
					}

					property ExecutionMode ExecutionMode
					{
						virtual Cpu::ExecutionMode get()
						{
							return Cpu::ExecutionMode::Run;
						}
						virtual void set( Cpu::ExecutionMode value )
						{
							throw gcnew NotImplementedException();
						}
					}

					property int ExecutionParameter
					{
						virtual int get()
						{
							return 0;
						}
						virtual void set( int value )
						{
							throw gcnew NotImplementedException();
						}
					}

					property bool DebuggingEnabled
					{
						virtual bool get()
						{
							return false;
						}
					}

					property R4000Cache^ CodeCache
					{
						virtual R4000Cache^ get()
						{
							return _codeCache;
						}
					}

					R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters );
					~R4000Cpu();

					virtual void EnableDebugging()
					{
						throw gcnew NotImplementedException();
					}

					virtual void Cleanup();

					virtual int RegisterSyscall( unsigned int nid );

					virtual void Resume()
					{
						throw gcnew NotImplementedException();
					}

					virtual void Break()
					{
						throw gcnew NotImplementedException();
					}

					virtual int ExecuteBlock();

					virtual void PrintStatistics();
				};

			}
		}
	}
}
