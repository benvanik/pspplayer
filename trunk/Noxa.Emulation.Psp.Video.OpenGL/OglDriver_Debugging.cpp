// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"
#include "OglHook.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

bool Noxa::Emulation::Psp::Video::_debugEnabled = false;

bool OglDriver::SupportsDebugging::get()
{
#ifdef _DEBUG
	return true;
#else
	return false;
#endif
}

bool OglDriver::DebuggingEnabled::get()
{
	return ( _hook != nullptr );
}

void OglDriver::EnableDebugging()
{
	_hook = gcnew OglHook( this );
	_debugEnabled = true;
}
