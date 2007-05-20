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

uint64 _processedFrames;
uint64 _skippedFrames;
uint64 _displayListsProcessed;
uint64 _abortedLists;

uint _commandCounts[ 256 ];

OglStatistics::OglStatistics() : CounterSource( "Video Driver" )
{
	this->Frames = gcnew Counter( "Frames", "The number of frames rendered." );
	this->SkippedFrames = gcnew Counter( "Skipped Frames", "The number of frames skipped due to frame skipping." );
	this->DisplayLists = gcnew Counter( "Display Lists", "The number of display lists processed." );
	this->AbortedDisplayLists = gcnew Counter( "Aborted Lists", "The number of display lists aborted by the game." );

	this->RegisterCounter( this->Frames );
	this->RegisterCounter( this->SkippedFrames );
	this->RegisterCounter( this->DisplayLists );
	this->RegisterCounter( this->AbortedDisplayLists );
}

void OglStatistics::Sample()
{
	this->Frames->Update( ( double )_processedFrames );
	this->SkippedFrames->Update( ( double )_skippedFrames );
	this->DisplayLists->Update( ( double )_displayListsProcessed );
	this->AbortedDisplayLists->Update( ( double )_abortedLists );
}

void OglStatistics::DumpCommandCounts()
{
#ifdef STATISTICS
		Log::WriteLine( Verbosity::Verbose, Feature::Statistics, "Video Command Usage Count: ----------------------------------" );
		for( int n = 0; n < 255; n++ )
		{
			if( _commandCounts[ n ] <= 1 )
				continue;
			Log::WriteLine( Verbosity::Verbose, Feature::Statistics, "{0:X2}: {1}\t{2}", n, _commandCounts[ n ], ( VideoCommand )n );
		}
#endif
}
