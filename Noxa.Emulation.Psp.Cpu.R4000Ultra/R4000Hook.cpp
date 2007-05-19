// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Hook.h"
#include "R4000Cpu.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Debugging;

R4000Hook::R4000Hook( R4000Cpu^ cpu )
{
	this->Cpu = cpu;
}

// -- CPU --

CoreState^ R4000Hook::GetCoreState( int core )
{
	Debug::Assert( core == 0 );

	if( core == 0 )
	{
		R4000Core^ core = this->Cpu->_core0;

		CoreState^ state = gcnew CoreState();

		state->ProgramCounter = *core->PC;
		state->GeneralRegisters = core->GeneralRegisters;
		state->Hi = *core->HI;
		state->Lo = *core->LO;
		state->LL = ( *core->LL == 1 ) ? true : false;

		state->Cp0ControlRegisters = core->Cp0->Control;
		state->Cp0Registers = core->Cp0->Registers;
		state->Cp0ConditionBit = core->Cp0->ConditionBit;

		state->FpuControlRegister = core->Cp1->Control;
		state->FpuRegisters = core->Cp1->Registers;

		// VFPU

		return state;
	}
	else
	{
		return nullptr;
	}
}

void R4000Hook::SetCoreState( int core, CoreState^ state )
{
}

array<Frame^>^ R4000Hook::GetCallstack()
{
	return nullptr;
}

void R4000Hook::AddCodeBreakpoint( int id, uint address )
{
}

void R4000Hook::RemoveCodeBreakpoint( int id )
{
}

void R4000Hook::SetCodeBreakpointState( int id, bool enabled )
{
}

// -- Memory --

void R4000Hook::AddMemoryBreakpoint( int id, uint address, MemoryAccessType accessType )
{
}

void R4000Hook::RemoveMemoryBreakpoint( int id )
{
}

void R4000Hook::SetMemoryBreakpointState( int id, bool enabled )
{
}

array<byte>^ R4000Hook::GetMemory( uint startAddress, int length )
{
	return nullptr;
}

void R4000Hook::SetMemory( uint startAddress, array<byte>^ buffer, int offset, int length )
{
}

array<uint>^ R4000Hook::SearchMemory( uint64 value, int width )
{
	return nullptr;
}

uint R4000Hook::Checksum( uint startAddress, int length )
{
	return 0;
}
