// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

OglDriver::OglDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalDriver = this;

	_emu = emulator;
	_params = parameters;
}

OglDriver::~OglDriver()
{
}

void OglDriver::Suspend()
{
}

bool OglDriver::Resume()
{
	return false;
}

void OglDriver::Cleanup()
{
}
