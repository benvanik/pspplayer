// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Options.h"

// Emit trace file
//#define TRACE
#define TRACEFILE		"Trace-Ultra.txt"
//#define TRACEAFTER		0x08911B98
#define TRACESYMBOLS			// Trace function names on calls - really slow, I think!
#define TRACEREGISTERS			// Trace register values on each instruction - ULTRA SLOW
//#define TRACEFPUREGS			// Trace the FPU registers (cop1)



// -- macro hacks --
#ifndef TRACE
#undef TRACESYMBOLS
#undef TRACEREGISTERS
#undef TRACEFPUREGS
#endif
