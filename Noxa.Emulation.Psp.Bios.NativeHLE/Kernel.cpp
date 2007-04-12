// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Kernel.h"
#include "NativeBios.h"
#include "HandleTable.h"
#include "KPartition.h"
#include "KDevice.h"
#include "KFile.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

Kernel*		_currentKernel;

Kernel::Kernel( NativeBios^ bios )
{
	Debug::Assert( bios != nullptr );
	this->Bios = bios;
	this->Emu = bios->_emulator;

	this->MemoryPool = new Psp::MemoryPool();

	this->Modules = new Vector<KModule*>( 128 );
	this->Threads = new Vector<KThread*>( 128 );
	this->SchedulableThreads = new LL<KThread*>();

	this->Partitions = new KPartition*[ PARTITIONCOUNT ];
	this->Devices = new KDevice*[ DEVICECOUNT ];
	this->Interrupts = new KIntHandler**[ INTERRUPTCOUNT ];
	this->Callbacks = new Vector<LL<KCallback*>*>( 16 );

	this->Handles = new HandleTable( this );

	ActiveThread = NULL;
	_oldThread = NULL;

	StdErr = NULL;
	StdIn = NULL;
	StdOut = NULL;

	MainModule = NULL;
	MainThread = NULL;

	_currentKernel = this;
	
	_lastUid = 100;

	_hTimerQueue = NULL;
	Cpu = NULL;
	Memory = NULL;
}

Kernel::~Kernel()
{
	// TODO: proper tear-down - right now we leak like hell

	_currentKernel = NULL;

	SAFEDELETE( this->MemoryPool );

	// clear each vector elements first!
	SAFEDELETE( this->Modules );
	SAFEDELETE( this->Threads );

	SAFEDELETEA( this->Partitions );
	SAFEDELETEA( this->Devices );
	SAFEDELETEA( this->Interrupts );
	SAFEDELETE( this->SchedulableThreads );
	SAFEDELETE( this->Callbacks );
	
	SAFEDELETE( this->Handles );

	MainModule = NULL;

	MainThread = NULL;
	ActiveThread = NULL;

	StdIn = NULL;
	StdOut = NULL;
	StdErr = NULL;
}

void Kernel::StartGame()
{
	NativeBios^ bios = Bios;
	Debug::Assert( bios->Game != nullptr );
	if( bios->Game == nullptr )
		return;

	IEmulationInstance^ emu = Emu;
	CpuCore = emu->Cpu->Cores[ 0 ];
	Cpu = ( ( CpuApi* )emu->Cpu->NativeInterface.ToPointer() );
	Memory = ( ( MemorySystem* )emu->Cpu->Memory->MemorySystemInstance );

	Partitions[ 0 ] = new KPartition( this, 0x00000000, 0x00000000 ); // dummy
	Partitions[ 1 ] = new KPartition( this, 0x08000000, 0x00300000 ); // kernel 1 (0x8...)
	Partitions[ 2 ] = new KPartition( this, 0x08000000, 0x01800000 ); // user
	Partitions[ 3 ] = new KPartition( this, 0x08000000, 0x00300000 ); // kernel 1
	Partitions[ 4 ] = new KPartition( this, 0x08300000, 0x00100000 ); // kernel 2 (0x8...)
	Partitions[ 5 ] = new KPartition( this, 0x08400000, 0x00400000 ); // kernel 3 (0x8...)
	Partitions[ 6 ] = new KPartition( this, 0x08800000, 0x01800000 ); // user

	Devices[ 0 ] = new KDevice( "ms0", false, emu->MemoryStick, gcnew array<String^>{ "fatms0", "ms", "fatms" } );
	Devices[ 1 ] = new KDevice( "umd0", true, emu->Umd, gcnew array<String^>{ "disc0", "umd", "isofs", "isofs0" } );

	// For each interrupt handler, add 16 slots
	for( int n = 0; n < INTERRUPTCOUNT; n++ )
		Interrupts[ n ] = new KIntHandler*[ INTSLOTCOUNT ];

	QueryPerformanceFrequency( ( LARGE_INTEGER* )&TickFrequenecy );
	QueryPerformanceCounter( ( LARGE_INTEGER* )&StartTick );

	Callbacks->Clear();
	CallbackTypes.Exit = Callbacks->Add( new LL<KCallback*>() );
	CallbackTypes.Umd = Callbacks->Add( new LL<KCallback*>() );

	// ?? startmodules
	
	this->CreateStdio();

	this->SetupTimerQueue();
}

void Kernel::StopGame( int exitCode )
{
	this->DestroyTimerQueue();

	// Free each 16 interrupt handlers
	for( int n = 0; n < 67; n++ )
		SAFEDELETEA( Interrupts[ n ] );

	NativeBios^ bios = Bios;
	bios->StopModules();
	bios->Game = nullptr;
	bios->BootStream = nullptr;
}

void Kernel::CreateStdio()
{
	StdIn = new KStdFile( HSTDIN );
	StdOut = new KStdFile( HSTDOUT );
	StdErr = new KStdFile( HSTDERR );

	this->Handles->Add( StdIn );
	this->Handles->Add( StdOut );
	this->Handles->Add( StdErr );
}

KDevice* Kernel::FindDevice( const char* path )
{
	const char* colon = strchr( path, ':' );
	if( colon == NULL )
	{
		IMediaFolder^ currentPath = CurrentPath;
		IMediaDevice^ device = currentPath->Device;
		for( int n = 0; n < 2; n++ )
		{
			KDevice* fileDevice = Devices[ n ];
			IMediaDevice^ fd = fileDevice->Device;
			if( fd == device )
				return fileDevice;
		}
		return NULL;
	}
	else
	{
		int colonPos = colon - path;
		char dev[ 10 ];
		strncpy_s( dev, 10, path, colonPos );
		for( int n = 0; n < 2; n++ )
		{
			KDevice* device = this->Devices[ n ];
			if( _strcmpi( dev, device->Name ) == 0 )
				return device;
			for( int m = 0; m < device->AliasCount; m++ )
			{
				if( _strcmpi( dev, device->Aliases[ m ] ) == 0 )
					return device;
			}
		}
		assert( false );
		return NULL;
	}
}

IMediaDevice^ Kernel::FindMediaDevice( const char* path )
{
	const char* colon = strchr( path, ':' );
	assert( colon != NULL );
	if( colon != NULL )
	{
		int colonPos = colon - path;
		char dev[ 10 ];
		strncpy_s( dev, 10, path, colonPos );
		for( int n = 0; n < 2; n++ )
		{
			KDevice* device = this->Devices[ n ];
			if( _strcmpi( dev, device->Name ) == 0 )
				return device->Device;
			for( int m = 0; m < device->AliasCount; m++ )
			{
				if( _strcmpi( dev, device->Aliases[ m ] ) == 0 )
					return device->Device;
			}
		}
		assert( false );
		return nullptr;
	}
	return nullptr;
}

// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
#pragma unmanaged
int64 Kernel::GetClockTime()
{
	// Convert Windows time to Unix time
	::FILETIME ft;
	GetSystemTimeAsFileTime( ( ::FILETIME* )&ft );
	return 11644473600 + *( ( int64* )&ft );
}
#pragma managed

// Time in microseconds since the game started.
#pragma unmanaged
int64 Kernel::GetRunTime()
{
	::FILETIME ft;
	GetSystemTimeAsFileTime( ( ::FILETIME* )&ft );
	int64 current = *( ( int64* )&ft );
	return current - StartTick;
}
#pragma managed

// Time, in TickFrequency, since whenever.
#pragma unmanaged
int64 Kernel::GetTick()
{
	int64 tick;
	QueryPerformanceCounter( ( LARGE_INTEGER* )&tick );
	return tick;
}
#pragma managed

int Kernel::RegisterCallbackType()
{
	return Callbacks->Add( new LL<KCallback*>() );
}
