// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

// When enabled, code dealing with certain critical areas that may later
// be accessed from multiple threads will be gaurded with locks
//#define MULTITHREADED

// When defined, memory will first be reserved and only committed when it is
// needed. This may slow down generation slightly, but has the advantage of
// wasting much less memory. Since I care about speed and memory is cheap,
// this will probably remain off.
//#define RESERVEANDCOMMITMEMORY

#define STATISTICS

#define TRACE

// ---------------------- Debug options -------------------------------------
#ifdef _DEBUG

#endif

// -- macro hacks --
