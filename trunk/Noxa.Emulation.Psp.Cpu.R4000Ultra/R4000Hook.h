// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::DebugModel;
using namespace Noxa::Emulation::Psp::Debugging::Hooks;
using namespace Noxa::Emulation::Psp::Debugging::Protocol;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;

				ref class R4000Hook : public ICpuHook, MarshalByRefObject
				{
				public:
					R4000Cpu^			Cpu;

					List<Breakpoint^>^	Breakpoints;

				public:
					R4000Hook( R4000Cpu^ cpu );

					// -- Breakpoints --

					virtual void AddBreakpoint( Breakpoint^ breakpoint );
					virtual Breakpoint^ FindBreakpoint( int id );
					virtual bool UpdateBreakpoint( Breakpoint^ newBreakpoint );
					virtual void RemoveBreakpoint( int id );

					// -- CPU --

					virtual CoreState^ GetCoreState( int core );
					virtual void SetCoreState( int core, CoreState^ state );
					virtual CoreState^ GetThreadCoreState( int core, int internalThreadId );
					
					generic<typename T>
					virtual T GetRegister( RegisterSet set, int ordinal );
					generic<typename T>
					virtual void SetRegister( RegisterSet set, int ordinal, T value );

					virtual array<Frame^>^ GetCallstack();

					// -- Memory --

					virtual array<byte>^ GetMemory( uint startAddress, int length );
					virtual void SetMemory( uint startAddress, array<byte>^ buffer, int offset, int length );
					virtual IntPtr GetMemoryPointer( uint address );

					virtual array<uint>^ SearchMemory( uint64 value, int width );
					virtual uint Checksum( uint startAddress, int length );
					virtual array<uint>^ GetMethodBody( Method^ method );

				public:
					List<Breakpoint^>^		SteppingBreakpoints;
					void RefreshBreakpointTable();
				};

			}
		}
	}
}
