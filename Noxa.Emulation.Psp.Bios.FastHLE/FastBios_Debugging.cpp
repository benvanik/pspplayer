// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "FastBios.h"
#include "Kernel.h"
#include "Module.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Debugging;
using namespace Noxa::Emulation::Psp::Debugging::DebugModel;

void FastBios::EnableDebugging( IDebugger^ debugger )
{
	_debugger = debugger;
	if( _debugger != nullptr )
		_debugger->BiosHook = this;
}
