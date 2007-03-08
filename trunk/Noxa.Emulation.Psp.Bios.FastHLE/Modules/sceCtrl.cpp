// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceCtrl.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Input;

void sceCtrl::Start()
{
	if( _threadRunning == true )
		this->Stop();

	_threadRunning = true;
	_thread = gcnew Thread( gcnew ThreadStart( this, &sceCtrl::InputThread ) );
	_thread->IsBackground = true;
	_thread->Name = "Kernel Input Thread";
	_thread->Priority = ThreadPriority::Normal;
	_thread->Start();
}

void sceCtrl::Stop()
{
	_threadRunning = false;
	if( _thread != nullptr )
		_thread->Join();
	_thread = nullptr;
}

void sceCtrl::Clear()
{
	if( _threadRunning == true )
		this->Stop();

	_sampleCycle = 0;
	_sampleMode = ControlSamplingMode::AnalogAndDigital;
	_buffer = gcnew CircularList<ControlSample^>( 100 );
	_dataPresent = gcnew AutoResetEvent( false );
}

void sceCtrl::InputThread()
{
	try
	{
		IInputDevice^ device = _kernel->_emu->Input;
		Debug::Assert( device != nullptr );
		if( device == nullptr )
		{
			_threadRunning = false;
			return;
		}

		while( _threadRunning == true )
		{
			ControlSample^ sample = gcnew ControlSample();
			sample->Timestamp = ( uint )( _kernel->RunTime * 1000 );

			device->Poll();
			sample->Buttons = device->Buttons;
			float max = ushort::MaxValue;
			if( sample->AnalogX == 0 )
				sample->AnalogX = ( int )( ( ( ( float )device->AnalogX / max ) + 0.5f ) * 256 );
			if( sample->AnalogY == 0 )
				sample->AnalogY = ( int )( ( ( -( float )device->AnalogY / max ) + 0.5f ) * 256 );

			Monitor::Enter( this );
			{
				_buffer->Add( sample );
				if( _buffer->Count == 1 )
					_dataPresent->Set();
			}
			Monitor::Exit( this );

			Thread::Sleep( InputPollInterval );
		}
	}
	catch( ThreadAbortException^ )
	{
	}
	catch( ThreadInterruptedException^ )
	{
	}
}

// int sceCtrlSetSamplingCycle(int cycle); (/ctrl/pspctrl.h:119)
int sceCtrl::sceCtrlSetSamplingCycle( int cycle )
{
	int old = _sampleCycle;
	_sampleCycle = cycle;
	Debug::WriteLine( String::Format( "sceCtrlSetSamplingCycle: set to {0} (was {1})", cycle, old ) );

	return old;
}

// int sceCtrlGetSamplingCycle(int *pcycle); (/ctrl/pspctrl.h:128)
int sceCtrl::sceCtrlGetSamplingCycle( IMemory^ memory, int pcycle )
{
	if( pcycle != 0x0 )
		memory->WriteWord( pcycle, 4, _sampleCycle );

	return 0;
}

// int sceCtrlSetSamplingMode(int mode); (/ctrl/pspctrl.h:137)
int sceCtrl::sceCtrlSetSamplingMode( int mode )
{
	int old = ( int )_sampleMode;
	_sampleMode = ( ControlSamplingMode )mode;

	return old;
}

// int sceCtrlGetSamplingMode(int *pmode); (/ctrl/pspctrl.h:146)
int sceCtrl::sceCtrlGetSamplingMode( IMemory^ memory, int pmode )
{
	if( pmode != 0x0 )
		memory->WriteWord( pmode, 4, ( int )_sampleMode );

	return 0;
}

// int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:148)
int sceCtrl::sceCtrlPeekBufferPositive( IMemory^ memory, int pad_data, int count )
{
	array<ControlSample^>^ samples;
	int toRead;
	Monitor::Enter( this );
	{
		toRead = MIN2( count, _buffer->Count );
		samples = gcnew array<ControlSample^>( toRead );
		for( int n = 0; n < toRead; n++ )
			samples[ n ] = _buffer->PeekAhead( n );
	}
	Monitor::Exit( this );

	if( pad_data != 0x0 )
	{
		int addr = pad_data;
		for( int n = 0; n < toRead; n++ )
		{
			ControlSample^ sample = samples[ n ];
			memory->WriteWord( addr + 0, 4, ( int )sample->Timestamp );
			memory->WriteWord( addr + 4, 4, ( int )sample->Buttons );
			memory->WriteWord( addr + 8, 1, ( byte )sample->AnalogX );
			memory->WriteWord( addr + 9, 1, ( byte )sample->AnalogY );
			// 6 bytes of junk
			addr += 16;
		}
	}

	return toRead;
}

// int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:150)
int sceCtrl::sceCtrlPeekBufferNegative( IMemory^ memory, int pad_data, int count )
{
	// Same as above, but complement button mask

	array<ControlSample^>^ samples;
	int toRead;
	Monitor::Enter( this );
	{
		toRead = MIN2( count, _buffer->Count );
		samples = gcnew array<ControlSample^>( toRead );
		for( int n = 0; n < toRead; n++ )
			samples[ n ] = _buffer->PeekAhead( n );
	}
	Monitor::Exit( this );

	if( pad_data != 0x0 )
	{
		int addr = pad_data;
		for( int n = 0; n < toRead; n++ )
		{
			ControlSample^ sample = samples[ n ];
			memory->WriteWord( addr + 0, 4, ( int )sample->Timestamp );
			memory->WriteWord( addr + 4, 4, ~( int )sample->Buttons );
			memory->WriteWord( addr + 8, 1, ( byte )sample->AnalogX );
			memory->WriteWord( addr + 9, 1, ( byte )sample->AnalogY );
			// 6 bytes of junk
			addr += 16;
		}
	}

	return toRead;
}

// int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:168)
int sceCtrl::sceCtrlReadBufferPositive( IMemory^ memory, int pad_data, int count )
{
	array<ControlSample^>^ samples = gcnew array<ControlSample^>( count );
	int read = 0;
	while( read < count )
	{
		Monitor::Enter( this );
		if( _buffer->Count == 0 )
			_dataPresent->WaitOne( InputPollInterval * 2, true );
		while( ( _buffer->Count > 0 ) && ( read < count ) )
			samples[ read++ ] = _buffer->Dequeue();
		Monitor::Exit( this );
	}

	if( pad_data != 0x0 )
	{
		int addr = pad_data;
		for( int n = 0; n < read; n++ )
		{
			ControlSample^ sample = samples[ n ];
			memory->WriteWord( addr + 0, 4, ( int )sample->Timestamp );
			memory->WriteWord( addr + 4, 4, ( int )sample->Buttons );
			memory->WriteWord( addr + 8, 1, ( byte )sample->AnalogX );
			memory->WriteWord( addr + 9, 1, ( byte )sample->AnalogY );
			// 6 bytes of junk
			addr += 16;
		}
	}

	return read;
}

// int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:170)
int sceCtrl::sceCtrlReadBufferNegative( IMemory^ memory, int pad_data, int count )
{
	array<ControlSample^>^ samples = gcnew array<ControlSample^>( count );
	int read = 0;
	while( read < count )
	{
		Monitor::Enter( this );
		if( _buffer->Count == 0 )
			_dataPresent->WaitOne( InputPollInterval * 2, true );
		while( _buffer->Count > 0 )
			samples[ read++ ] = _buffer->Dequeue();
		Monitor::Exit( this );
	}

	if( pad_data != 0x0 )
	{
		int addr = pad_data;
		for( int n = 0; n < read; n++ )
		{
			ControlSample^ sample = samples[ n ];
			memory->WriteWord( addr + 0, 4, ( int )sample->Timestamp );
			memory->WriteWord( addr + 4, 4, ~( int )sample->Buttons );
			memory->WriteWord( addr + 8, 1, ( byte )sample->AnalogX );
			memory->WriteWord( addr + 9, 1, ( byte )sample->AnalogY );
			// 6 bytes of junk
			addr += 16;
		}
	}

	return read;
}

// int sceCtrlPeekLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:172)
int sceCtrl::sceCtrlPeekLatch( IMemory^ memory, int latch_data ){ return NISTUBRETURN; }

// int sceCtrlReadLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:174)
int sceCtrl::sceCtrlReadLatch( IMemory^ memory, int latch_data ){ return NISTUBRETURN; }
