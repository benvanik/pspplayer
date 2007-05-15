// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"
#include "VideoApi.h"
#include <string>

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

// Number of vertical traces
uint _vcount;
bool Noxa::Emulation::Psp::Video::_speedLocked;

OglDriver::OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalDriver = this;

	_emu = emulator;
	_params = parameters;
	_props = gcnew DisplayProperties();
	_currentProps = _props;
	_caps = gcnew OglCapabilities();
	_stats = gcnew OglStatistics();

	_nativeInterface = ( VideoApi* )malloc( sizeof( VideoApi ) );
	memset( _nativeInterface, 0, sizeof( VideoApi ) );
	this->SetupNativeInterface();

	_vcount = 0;
	_speedLocked = true;
}

OglDriver::~OglDriver()
{
	this->DestroyNativeInterface();
	SAFEFREE( _nativeInterface );
}

uint OglDriver::Vcount::get()
{
	return _vcount;
}

void OglDriver::Suspend()
{
}

bool OglDriver::Resume()
{
	if( _thread == nullptr )
		this->StartThread();

	if( _props->HasChanged == false )
		return true;

	Log::WriteLine( Verbosity::Normal, Feature::Video, "video mode change" );

	_currentProps = ( DisplayProperties^ )_props->Clone();
	_props->HasChanged = false;
	_currentProps->HasChanged = false;

	return true;
}

void OglDriver::Cleanup()
{
	this->StopThread();

	// Cleanup everything else here

	_threadSync = nullptr;
}

void OglDriver::PrintStatistics()
{
#ifdef STATISTICS
		_stats->GatherStats();
		//_stats->AverageCodeBlockLength /= _stats->CodeBlocksGenerated;
		//_stats->AverageGenerationTime /= _stats->CodeBlocksGenerated;
		//_stats->RunTime = _timer->Elapsed - _stats->RunTime;
		//_stats->IPS = _stats->InstructionsExecuted / _stats->RunTime;
		TimeSpan elapsed = DateTime::Now - _startTime;
		_stats->FPS = _stats->ProcessedFrames / ( float )elapsed.TotalSeconds;
		_stats->AttemptedFPS = ( _stats->ProcessedFrames + _stats->SkippedFrames ) / ( float )elapsed.TotalSeconds;

		Log::WriteLine( Verbosity::Normal, Feature::Statistics, "OpenGL Video Driver Statistics: -----------------------------" );
		array<FieldInfo^>^ fields = ( OglStatistics::typeid )->GetFields();
		for( int n = 0; n < fields->Length; n++ )
		{
			Object^ value = fields[ n ]->GetValue( _stats );
			Log::WriteLine( Verbosity::Normal, Feature::Statistics, "{0}: {1}", fields[ n ]->Name, value );
		}

		Log::WriteLine( Verbosity::Verbose, Feature::Statistics, "Video Command Usage Count: ----------------------------------" );
		for( int n = 0; n < _stats->CommandCounts->Length; n++ )
		{
			if( _stats->CommandCounts[ n ] <= 1 )
				continue;
			Log::WriteLine( Verbosity::Verbose, Feature::Statistics, "{0:X2}: {1}\t{2}", n, _stats->CommandCounts[ n ], ( VideoCommand )n );
		}
#endif
}
