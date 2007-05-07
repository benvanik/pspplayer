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
uint _abortedLists;

uint _commandCounts[ 256 ];

void OglStatistics::GatherStats()
{
	ProcessedFrames = _processedFrames;
	SkippedFrames = _skippedFrames;
	DisplayListsProcessed = _displayListsProcessed;
	AbortedDisplayLists = _abortedLists;
	CommandCounts = gcnew array<uint>( 256 );
	for( int n = 0; n < 256; n++ )
	{
		CommandCounts[ n ] = _commandCounts[ n ];
		_commandCounts[ n ] = 0;
	}

	_processedFrames = 0;
	_skippedFrames = 0;
	_displayListsProcessed = 0;
	_abortedLists = 0;
}
