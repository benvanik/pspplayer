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

uint _instructionsExecuted;
uint _codeBlocksExecuted;
uint _jumpBlockThunkHits;
uint _jumpBlockInlineHits;
uint _jumpBlockInlineMisses;

uint _nativeSyscallCount;

void R4000Statistics::GatherStats()
{
	InstructionsExecuted = _instructionsExecuted;
	CodeBlocksExecuted = _codeBlocksExecuted;
	JumpBlockThunkHits = _jumpBlockThunkHits;
	JumpBlockInlineHits = _jumpBlockInlineHits;
	JumpBlockInlineMisses = _jumpBlockInlineMisses;

	NativeSyscallCount = _nativeSyscallCount;

	_instructionsExecuted = 0;
	_codeBlocksExecuted = 0;
	_jumpBlockThunkHits = 0;
	_jumpBlockInlineHits = 0;
	_jumpBlockInlineMisses = 0;

	_nativeSyscallCount = 0;
}
