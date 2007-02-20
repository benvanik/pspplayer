// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"

#include "VideoApi.h"

#include "R4000VideoInterface.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Video::Native;

R4000VideoInterface::R4000VideoInterface()
{
}

R4000VideoInterface::~R4000VideoInterface()
{
}

void __inline GetVideoPacket( int code, int baseAddress, VideoPacket* packet )
{
	packet->Command = ( byte )( code >> 24 );
	packet->Argument = code & 0x00FFFFFF;

	switch( packet->Command )
	{
	case VADDR:
	case IADDR:
	case OFFSETADDR:
	case ORIGINADDR:
		packet->Argument = packet->Argument | baseAddress;
		break;
	}
}
