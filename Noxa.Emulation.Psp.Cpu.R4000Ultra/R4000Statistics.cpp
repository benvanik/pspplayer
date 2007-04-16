// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Statistics.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#ifdef STATISTICS
uint _instructionsExecuted;
uint _codeBlocksExecuted;
uint _jumpBlockThunkHits;
uint _jumpBlockInlineHits;
uint _jumpBlockInlineMisses;

uint _executionLoops;
uint _codeCacheHits;
uint _codeCacheMisses;

uint _managedMemoryReadCount;
uint _managedMemoryWriteCount;

uint _nativeSyscallCount;
#endif

void R4000Statistics::GatherStats()
{
#ifdef STATISTICS
	InstructionsExecuted = _instructionsExecuted;
	CodeBlocksExecuted = _codeBlocksExecuted;
	JumpBlockThunkHits = _jumpBlockThunkHits;
	JumpBlockInlineHits = _jumpBlockInlineHits;
	JumpBlockInlineMisses = _jumpBlockInlineMisses;

	ExecutionLoops = _executionLoops;
	CodeCacheHits = _codeCacheHits;
	CodeCacheMisses = _codeCacheMisses;

	ManagedMemoryReadCount = _managedMemoryReadCount;
	ManagedMemoryWriteCount = _managedMemoryWriteCount;

	NativeSyscallCount = _nativeSyscallCount;

	_instructionsExecuted = 0;
	_codeBlocksExecuted = 0;
	_jumpBlockThunkHits = 0;
	_jumpBlockInlineHits = 0;
	_jumpBlockInlineMisses = 0;

	_executionLoops = 0;
	_codeCacheHits = 0;
	_codeCacheMisses = 0;
	
	_managedMemoryReadCount = 0;
	_managedMemoryWriteCount = 0;

	_nativeSyscallCount = 0;
#endif
}
