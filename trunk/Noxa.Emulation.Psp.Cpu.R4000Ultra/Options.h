// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

// When enabled, code dealing with certain critical areas that may later
// be accessed from multiple threads will be gaurded with locks.
//#define MULTITHREADED

// When enabled and _DEBUG is defined, an assembly listing for each block generated will be
// written to the defined path. It is overwritten during each block gen, so make sure to
// set a breakpoint at the end!
#define GENECHOFILE "C:\\Dev\\Noxa.Emulation\\trunk\\debug\\gen.txt"