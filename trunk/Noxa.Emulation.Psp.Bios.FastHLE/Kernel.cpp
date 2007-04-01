// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "Kernel.h"
#include "FastBios.h"
#include "KernelDevice.h"
#include "KernelStatistics.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Media;

int64 _startTick;

Kernel::Kernel( FastBios^ bios )
{
	Debug::Assert( bios != nullptr );
	_bios = bios;

	_gameEvent = gcnew AutoResetEvent( false );
	Timer = gcnew PerformanceTimer();
	UnixBaseTime = DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind::Utc );

	_lastId = 0;
	_handles = gcnew Dictionary<int, KernelHandle^>();
	Threads = gcnew Dictionary<int, KernelThread^>();
	_threadsWaitingOnEvents = gcnew List<KernelThread^>();
	_delayedThreads = gcnew List<KernelThread^>();
	_delayedThreadTimer = gcnew Timers::Timer();
	_delayedThreadTimer->AutoReset = false;
	_delayedThreadTimer->Elapsed += gcnew Timers::ElapsedEventHandler( this, &Kernel::DelayedThreadTimerElapsed );

	_devices = gcnew List<KernelDevice^>();
	_deviceMap = gcnew Dictionary<String^, KernelDevice^>();

	Callbacks = gcnew Dictionary<KernelCallbackType, KernelCallback^>();
	InterruptHandlers = gcnew array<array<KernelInterruptHandler^>^>( 80 );

	_emu = _bios->Emulator;

	// Took out MSB because addresses come translated
	Partitions = gcnew array<KernelPartition^>{
		gcnew KernelPartition( this, 0, 0x0, 0x0 ), /* dummy */
		gcnew KernelPartition( this, 1, 0x08000000, 0x00300000 ), /* Kernel 1 0x8*/
		gcnew KernelPartition( this, 2, 0x08000000, 0x01800000 ), /* User */
		gcnew KernelPartition( this, 3, 0x08000000, 0x00300000 ), /* Kernel 1 */
		gcnew KernelPartition( this, 4, 0x08300000, 0x00100000 ), /* Kernel 2 0x8 */
		gcnew KernelPartition( this, 5, 0x08400000, 0x00400000 ), /* Kernel 3 0x8 */
		gcnew KernelPartition( this, 6, 0x08800000, 0x01800000 ), /* User */
	};

	Statistics = gcnew KernelStatistics();
}

GameInformation^ Kernel::Game::get()
{
	return _game;
}

void Kernel::Game::set( GameInformation^ value )
{
	Debug::Assert( _game == nullptr );
	if( _game != nullptr )
	{
		// Shouldn't happen?
	}
	_game = value;
	this->StartGame();
}

void Kernel::StartGame()
{
	Timer->Reset();
	StartTime = 0.0;
	StartTick = DateTime::Now.Ticks;
	StartDateTime = DateTime::Now;
	ULARGE_INTEGER time;
	GetSystemTimeAsFileTime( ( FILETIME* )&time );
	_startTick = time.QuadPart;

	_cpu = _emu->Cpu;
	_core0 = _cpu->Cores[ 0 ];

	_deviceMap->Clear();
	_devices->Clear();

	// We do this here because we want to make sure the file systems are setup
	IMediaDevice^ msDevice = _emu->MemoryStick;
	IMediaDevice^ umdDevice = _emu->Umd;
	_devices->Add( gcnew KernelFileDevice( "MemoryStick",
		gcnew array<String^>{ "fatms0", "ms", "ms0", "fatms" },
		true, ( msDevice != nullptr ) ? msDevice->IsReadOnly : true, msDevice,
		( msDevice != nullptr ) ? msDevice->Root : nullptr ) );
	_devices->Add( gcnew KernelFileDevice( "UMD",
		gcnew array<String^>{ "umd", "umd0", "isofs", "isofs0", "disc0" },
		true, true, umdDevice,
		( umdDevice != nullptr ) ? umdDevice->Root : nullptr ) );
	//_devices.Add( new KernelFileDevice( "flash0", new string[] { "flash0", "flashfat", "flashfat0" }, true, false, null, null ) );
	//_devices.Add( new KernelFileDevice( "flash1", new string[] { "flash1", "flashfat1" }, true, false, null, null ) );

	for each( KernelDevice^ device in _devices )
	{
		for each( String^ path in device->Paths )
			_deviceMap->Add( path, device );
	}

	CurrentPath = _game->Folder;

	_bios->ClearModules();
	_bios->StartModules();

	_gameEvent->Set();
}

void Kernel::ExitGame( int status )
{
	_bios->StopModules();

	_deviceMap->Clear();
	_devices->Clear();

	_game = nullptr;
	_gameEvent->Set();
}

void Kernel::Execute()
{
	if( _activeThread != nullptr )
	{
		// Execute active thread
		int instructionCount = _cpu->ExecuteBlock();

		//Debug::WriteLine( String::Format( "Kernel: execute ended after {0} instructions", instructionCount ) );
	}
	else
	{
		// Load game and run until it creates its first thread

		// Waiting for the game...
		if( _game == nullptr )
			_gameEvent->WaitOne();

		// Clear everything
		_emu->LightReset();

		GameLoader^ g = gcnew GameLoader();
		uint lowerBounds;
		uint upperBounds;
		uint entryAddress;
		if( g->LoadBoot( _game, _emu, lowerBounds, upperBounds, entryAddress ) == false )
		{
			Debug::WriteLine( "Kernel: game load failed, aborting" );
			_game = nullptr;
			return;
		}

		this->CreateStdio();

		// Have to allocate the stuff taken by the elf
		Partitions[ 1 ]->Allocate( KernelAllocationType::SpecificAddress, lowerBounds, upperBounds - lowerBounds );
		_elfUpperBounds = upperBounds;

		int preThreadCount = 0;
		while( _activeThread == nullptr )
		{
			preThreadCount += _cpu->ExecuteBlock();
		}

		Debug::WriteLine( String::Format( "Kernel: first thread created, executed {0} instructions", preThreadCount ) );
	}
}
