// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
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

				ref class R4000Hook : public ICpuHook
				{
				public:
					R4000Cpu^			Cpu;

				public:
					R4000Hook( R4000Cpu^ cpu );

					// -- CPU --

					virtual CoreState^ GetCoreState( int core );
					virtual void SetCoreState( int core, CoreState^ state );

					virtual array<Frame^>^ GetCallstack();

					virtual void AddCodeBreakpoint( int id, uint address );
					virtual void RemoveCodeBreakpoint( int id );
					virtual void SetCodeBreakpointState( int id, bool enabled );

					// -- Memory --

					virtual void AddMemoryBreakpoint( int id, uint address, MemoryAccessType accessType );
					virtual void RemoveMemoryBreakpoint( int id );
					virtual void SetMemoryBreakpointState( int id, bool enabled );

					virtual array<byte>^ GetMemory( uint startAddress, int length );
					virtual void SetMemory( uint startAddress, array<byte>^ buffer, int offset, int length );

					virtual array<uint>^ SearchMemory( uint64 value, int width );
					virtual uint Checksum( uint startAddress, int length );
				};

			}
		}
	}
}
