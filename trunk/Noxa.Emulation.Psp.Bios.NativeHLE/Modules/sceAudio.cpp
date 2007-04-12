// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceAudio.h"
#include "Kernel.h"
#include "KThread.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Audio;

void sceAudio::Start()
{
	IEmulationInstance^ emu = _kernel->Emu;
	_driver = emu->Audio;
}

void sceAudio::Stop()
{
	if( _driver != nullptr )
		_driver->ReleaseAllChannels();
}

void sceAudio::Clear()
{
	this->Stop();
	_driver = nullptr;
}

// int sceAudioOutput(int channel, int vol, void *buf); (/audio/pspaudio.h:73)
int sceAudio::sceAudioOutput( IMemory^ memory, int channel, int vol, int buf )
{
	if( _driver == nullptr )
		return 0;

	IntPtr buffer = IntPtr( ( void* )MSI( memory )->Translate( buf ) );
	_driver->Output( channel, buffer, false, vol );

	return 0;
}

// int sceAudioOutputBlocking(int channel, int vol, void *buf); (/audio/pspaudio.h:79)
int sceAudio::sceAudioOutputBlocking( IMemory^ memory, int channel, int vol, int buf )
{
	// TODO: proper blocking
	KThread* thread = _kernel->ActiveThread;
	thread->Suspend();
	if( _kernel->Schedule() == true )
	{
	}

	if( _driver == nullptr )
		return 0;

	IntPtr buffer = IntPtr( ( void* )MSI( memory )->Translate( buf ) );
	_driver->Output( channel, buffer, true, vol );

	return 0;
}

// int sceAudioOutputPanned(int channel, int leftvol, int rightvol, void *buffer); (/audio/pspaudio.h:85)
int sceAudio::sceAudioOutputPanned( IMemory^ memory, int channel, int leftvol, int rightvol, int buf )
{
	if( _driver == nullptr )
		return 0;

	IntPtr buffer = IntPtr( ( void* )MSI( memory )->Translate( buf ) );
	_driver->Output( channel, buffer, false, leftvol, rightvol );

	return 0;
}

// int sceAudioOutputPannedBlocking(int channel, int leftvol, int rightvol, void *buffer); (/audio/pspaudio.h:91)
int sceAudio::sceAudioOutputPannedBlocking( IMemory^ memory, int channel, int leftvol, int rightvol, int buf )
{
	// TODO: proper blocking
	KThread* thread = _kernel->ActiveThread;
	thread->Suspend();
	if( _kernel->Schedule() == true )
	{
	}

	if( _driver == nullptr )
		return 0;

	IntPtr buffer = IntPtr( ( void* )MSI( memory )->Translate( buf ) );
	_driver->Output( channel, buffer, true, leftvol, rightvol );

	return 0;
}

// int sceAudioChReserve(int channel, int samplecount, int format); (/audio/pspaudio.h:61)
int sceAudio::sceAudioChReserve( int channel, int samplecount, int format )
{
	if( _driver == nullptr )
		return 0;

	return _driver->ReserveChannel( channel, samplecount, ( AudioFormat )format );
}

// int sceAudioChRelease(int channel); (/audio/pspaudio.h:70)
int sceAudio::sceAudioChRelease( int channel )
{
	if( _driver == nullptr )
		return 0;

	_driver->ReleaseChannel( channel );

	return 0;
}
