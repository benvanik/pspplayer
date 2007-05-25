// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Options.h"

// Must be defined to enable special debugging code (breakpoints/etc)
#define DEBUGGING

// When defined, BreakpointType::MemoryAccess will be supported
#define DEBUGMEMORY


// The size of the debug thunk - MUST BE ACCURATE
#define DEBUGTHUNKSIZE	12

// -- macro hacks --
#ifndef DEBUGGING
#undef DEBUGMEMORY
#endif
