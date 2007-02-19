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

int _codeBlocksExecuted;
int _jumpBlockThunkHits;
int _jumpBlockInlineHits;
int _jumpBlockInlineMisses;

void R4000Statistics::GatherStats()
{
	CodeBlocksExecuted = _codeBlocksExecuted;
	JumpBlockThunkHits = _jumpBlockThunkHits;
	JumpBlockInlineHits = _jumpBlockInlineHits;
	JumpBlockInlineMisses = _jumpBlockInlineMisses;

	_codeBlocksExecuted = 0;
	_jumpBlockThunkHits = 0;
	_jumpBlockInlineHits = 0;
	_jumpBlockInlineMisses = 0;
}
