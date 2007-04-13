// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceDisplay.h"
#include "Kernel.h"
#include "KThread.h"
#include "VideoApi.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

void sceDisplay::Start()
{
	IEmulationInstance^ emu = _kernel->Emu;
	_videoApi = emu->Video->NativeInterface.ToPointer();
}

// int sceDisplaySetMode(int mode, int width, int height); (/display/pspdisplay.h:53)
int sceDisplay::sceDisplaySetMode( int mode, int width, int height )
{
	IEmulationInstance^ emu = _kernel->Emu;
	emu->Video->Suspend();

	DisplayProperties^ props =emu->Video->Properties;
	props->Mode = mode;
	props->Width = width;
	props->Height = height;

	if( emu->Video->Resume() != true )
		return -1;

	return 0;
}

// int sceDisplayGetMode(int *pmode, int *pwidth, int *pheight); (/display/pspdisplay.h:64)
int sceDisplay::sceDisplayGetMode( IMemory^ memory, int pmode, int pwidth, int pheight )
{
	IEmulationInstance^ emu = _kernel->Emu;
	DisplayProperties^ props = emu->Video->Properties;

	if( pmode != 0x0 )
		memory->WriteWord( pmode, 4, props->Mode );
	if( pwidth != 0x0 )
		memory->WriteWord( pwidth, 4, props->Width );
	if( pheight != 0x0 )
		memory->WriteWord( pheight, 4, props->Height );

	return 0;
}

// void sceDisplaySetFrameBuf(void *topaddr, int bufferwidth, int pixelformat, int sync); (/display/pspdisplay.h:74)
int sceDisplay::sceDisplaySetFrameBuf( IMemory^ memory, int topaddr, int bufferwidth, int pixelformat, int sync )
{
	if( _videoApi == NULL )
	{
		IEmulationInstance^ emu = _kernel->Emu;
		emu->Video->Suspend();

		DisplayProperties^ props = emu->Video->Properties;
		props->BufferAddress = ( uint )( topaddr & 0x0FFFFFFF );
		props->BufferSize = ( uint )bufferwidth;
		props->PixelFormat = ( PixelFormat )pixelformat;
		props->SyncMode = ( BufferSyncMode )sync;

		if( props->HasChanged == true )
			emu->Video->Resume();
	}
	else
	{
		VideoApi* vapi = ( VideoApi* )_videoApi;
		vapi->SwitchFrameBuffer( topaddr & 0x0FFFFFFFF, bufferwidth, pixelformat, sync );
	}

	return 0;
}

// int sceDisplayGetFrameBuf(void **topaddr, int *bufferwidth, int *pixelformat, int *unk1); (/display/pspdisplay.h:84)
int sceDisplay::sceDisplayGetFrameBuf( IMemory^ memory, int topaddr, int bufferwidth, int pixelformat, int sync )
{
	IEmulationInstance^ emu = _kernel->Emu;
	DisplayProperties^ props = emu->Video->Properties;

	if( topaddr != 0x0 )
		memory->WriteWord( topaddr, 4, ( int )props->BufferAddress );
	if( bufferwidth != 0x0 )
		memory->WriteWord( bufferwidth, 4, ( int )props->BufferSize );
	if( pixelformat != 0x0 )
		memory->WriteWord( pixelformat, 4, ( int )props->PixelFormat );
	if( sync != 0x0 )
		memory->WriteWord( sync, 4, ( int )props->SyncMode );

	return 0;
}

// unsigned int sceDisplayGetVcount(); (/display/pspdisplay.h:89)
int sceDisplay::sceDisplayGetVcount()
{
	if( _videoApi == NULL )
	{
		IEmulationInstance^ emu = _kernel->Emu;
		return emu->Video->Vcount;
	}
	else
	{
		VideoApi* vapi = ( VideoApi* )_videoApi;
		return vapi->GetVcount();
	}
}

// int sceDisplayWaitVblank(); (/display/pspdisplay.h:104)
int sceDisplay::sceDisplayWaitVblank()
{
	if( _videoApi == NULL )
	{
	//IVideoDriver^ video = _kernel->_emu->Video;
	//if( video->Vblank != nullptr )
	//	video->Vblank->WaitOne( 1000, false );
	}
	else
	{
		VideoApi* vapi = ( VideoApi* )_videoApi;
		vapi->WaitForVsync();
	}
	
	return 0;
}

// int sceDisplayWaitVblankCB(); (/display/pspdisplay.h:109)
int sceDisplay::sceDisplayWaitVblankCB()
{
	KThread* thread = _kernel->ActiveThread;
	thread->Delay( 1000, true );

	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceDisplayWaitVblankStart(); (/display/pspdisplay.h:94)
int sceDisplay::sceDisplayWaitVblankStart()
{
	if( _videoApi == NULL )
	{
	//IVideoDriver^ video = _kernel->_emu->Video;
	//if( video->Vblank != nullptr )
	//	video->Vblank->WaitOne( 1000, false );
	}
	else
	{
		VideoApi* vapi = ( VideoApi* )_videoApi;
		vapi->WaitForVsync();
	}

	return 0;
}

// int sceDisplayWaitVblankStartCB(); (/display/pspdisplay.h:99)
int sceDisplay::sceDisplayWaitVblankStartCB()
{
	KThread* thread = _kernel->ActiveThread;
	thread->Delay( 1000, true );

	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}
