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

OglDriver::OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalDriver = this;

	_emu = emulator;
	_params = parameters;
	_props = gcnew DisplayProperties();
	_currentProps = _props;
	_caps = gcnew OglCapabilities();
	_stats = gcnew OglStatistics();

	_nativeInterface = malloc( sizeof( VideoApi ) );
	memset( _nativeInterface, 0, sizeof( VideoApi ) );
	this->FillNativeInterface();

	_vcount = 0;
}

OglDriver::~OglDriver()
{
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
	if( _props->HasChanged == false )
		return true;

	Debug::WriteLine( "OglDriver: video mode change" );

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
