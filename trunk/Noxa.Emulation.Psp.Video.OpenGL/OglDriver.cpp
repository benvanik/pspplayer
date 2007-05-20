// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

#include "OglDriver.h"
#include "VideoApi.h"
#include <string>

using namespace System::Diagnostics;
using namespace System::Drawing;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

// Number of vertical traces
uint64 _vcount;
bool Noxa::Emulation::Psp::Video::_speedLocked;
bool Noxa::Emulation::Psp::Video::_screenshotPending;

OglDriver::OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalDriver = this;

	_emu = emulator;
	_params = parameters;
	_props = gcnew DisplayProperties();
	_currentProps = _props;
	_caps = gcnew OglCapabilities();
	_stats = gcnew OglStatistics();
	Diag::Instance->Counters->RegisterSource( _stats );

	_nativeInterface = ( VideoApi* )malloc( sizeof( VideoApi ) );
	memset( _nativeInterface, 0, sizeof( VideoApi ) );
	this->SetupNativeInterface();

	_vcount = 0;
	_speedLocked = true;

	_screenWidth = 480;
	_screenHeight = 272;

	_screenshotEvent = gcnew AutoResetEvent( false );
}

OglDriver::~OglDriver()
{
	this->DestroyNativeInterface();
	SAFEFREE( _nativeInterface );
}

uint64 OglDriver::Vcount::get()
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

Bitmap^ OglDriver::CaptureScreen()
{
	_screenshotPending = true;
	if( _screenshotEvent->WaitOne( 5000, true ) == false )
		return nullptr;
	else
		return _screenshot;
}

void OglDriver::Cleanup()
{
	_stats->DumpCommandCounts();

	this->StopThread();

	// Cleanup everything else here

	_threadSync = nullptr;
}
