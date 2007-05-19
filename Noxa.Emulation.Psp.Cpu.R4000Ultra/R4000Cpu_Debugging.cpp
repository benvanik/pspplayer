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
#include "R4000Hook.h"
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

bool Noxa::Emulation::Psp::Cpu::_debugEnabled = false;

bool R4000Cpu::SupportsDebugging::get()
{
#ifdef _DEBUG
	return true;
#else
	return false;
#endif
}

bool R4000Cpu::DebuggingEnabled::get()
{
	return ( _hook != nullptr );
}

void R4000Cpu::EnableDebugging()
{
	_hook = gcnew R4000Hook( this );
	_debugEnabled = true;
}
