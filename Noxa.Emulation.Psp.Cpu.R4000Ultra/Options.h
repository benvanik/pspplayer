// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

// When enabled, code dealing with certain critical areas that may later
// be accessed from multiple threads will be gaurded with locks
//#define MULTITHREADED

// Runtime statistic generation - will slow things down - this is needed for IPS
// information and such
#define STATISTICS

// ---------------------- Debug options -------------------------------------
#ifdef _DEBUG

// When enabled and _DEBUG is defined, an assembly listing for each block generated will be
// written to the defined path. It is overwritten during each block gen, so make sure to
// set a breakpoint at the end!
//#define GENECHOFILE "C:\\Dev\\Noxa.Emulation\\trunk\\debug\\gen.txt"

// When defined, the echo file will be cleared after each generated block
#define CLEARECHOFILE

// When defined with GENECHOFILE, really verbose messages will be added to the file
//#define VERBOSEANNOTATE

// When defined, generation info will be emitted
//#define GENDEBUG

// When defined with _DEBUG, each instruction executed will print itself to debug out
//#define RUNTIMEDEBUG
//#define RUNTIMEREGS

// Gather and print out statistics about syscalls
#define SYSCALLSTATS

#endif