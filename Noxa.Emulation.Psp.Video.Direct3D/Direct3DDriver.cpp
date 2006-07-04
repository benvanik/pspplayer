#include "StdAfx.h"
#include "Direct3DDriver.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

Direct3DDriver::Direct3DDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	_emu = emulator;
	_params = parameters;
	
	_timer = gcnew PerformanceTimer();

	_props = gcnew DisplayProperties();
}

void Direct3DDriver::Cleanup()
{
}

void Direct3DDriver::Suspend()
{
}

bool Direct3DDriver::Resume()
{
	return false;
}