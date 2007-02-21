// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglStatistics.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

uint _processedFrames;
uint _skippedFrames;
uint _displayListsProcessed;

void OglStatistics::GatherStats()
{
	ProcessedFrames = _processedFrames;
	SkippedFrames = _skippedFrames;
	DisplayListsProcessed = _displayListsProcessed;

	_processedFrames = 0;
	_skippedFrames = 0;
	_displayListsProcessed = 0;
}
