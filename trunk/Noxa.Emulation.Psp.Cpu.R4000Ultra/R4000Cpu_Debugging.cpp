// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "Tracer.h"

#include "R4000AdvancedBlockBuilder.h"
#include "R4000Generator.h"
#include "R4000Ctx.h"
#include "R4000BiosStubs.h"
#include "R4000VideoInterface.h"

using namespace System::Diagnostics;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::DebugModel;

void R4000Cpu::EnableDebugging( IDebugger^ debugger )
{
	_debugger = debugger;
	if( _debugger != nullptr )
		_debugger->CpuHook = this;
}

CoreState^ R4000Cpu::GetCoreState( int core )
{
	Debug::Assert( core == 0 );

	if( core == 0 )
	{
		CoreState^ state = gcnew CoreState();

		state->ProgramCounter = *_core0->PC;
		state->GeneralRegisters = _core0->GeneralRegisters;
		state->Hi = *_core0->HI;
		state->Lo = *_core0->LO;
		state->LL = ( *_core0->LL == 1 ) ? true : false;

		state->Cp0ControlRegisters = _core0->Cp0->Control;
		state->Cp0Registers = _core0->Cp0->Registers;
		state->Cp0ConditionBit = _core0->Cp0->ConditionBit;

		state->FpuControlRegister = _core0->Cp1->Control;
		state->FpuRegisters = _core0->Cp1->Registers;

		return state;
	}
	else
	{
		return nullptr;
	}
}

array<Frame^>^ R4000Cpu::GetCallstack()
{
	return nullptr;
}
