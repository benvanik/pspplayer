// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "Tracer.h"
#pragma unmanaged
#include "LL.h"
#pragma managed

#include "R4000Ctx.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

typedef struct InterruptHandler_t
{
	int		SubNumber;
	int		CallbackAddress;
	int		CallbackArgs;
} InterruptHandler;

LL<InterruptHandler*>	_interrupts[ 67 ];
bool					_inIntHandler;			// true when in a handler
int						_pendingIntNo;			// we need a list of these (sorted by priority), but for now 
												// we assume there cannot be more than one ^_^

#pragma unmanaged

#pragma managed

uint R4000Cpu::InterruptsMask::get()
{
}

void R4000Cpu::InterruptsMask::set( uint value )
{
}

void R4000Cpu::RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument )
{
}

void R4000Cpu::UnregisterInterruptHandler( int interruptNumber, int slot )
{
}

void R4000Cpu::SetPendingInterrupt( int interruptNumber )
{
}
