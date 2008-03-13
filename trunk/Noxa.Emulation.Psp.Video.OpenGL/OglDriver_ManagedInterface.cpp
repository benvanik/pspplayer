// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"
#include "VideoApi.h"
#include <string>

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

bool OglDriver::SetMode( int mode, int width, int height )
{
	return true;
}

bool OglDriver::SetFrameBuffer( uint bufferAddres, uint bufferSize, PixelFormat pixelFormat, BufferSyncMode bufferSyncMode )
{
	return true;
}
