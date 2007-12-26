// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#pragma unmanaged
#include "MemoryPool.h"
#pragma managed
#include "OglDriver.h"
#include "VideoApi.h"
#include "DisplayList.h"
#include "VideoCommands.h"
#include <string>

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

#ifdef _DEBUG
#define BREAK __break()
#else
#define BREAK
#endif

int64 _freq;

CRITICAL_SECTION _cs;
#define LOCK EnterCriticalSection( &_cs )
#define UNLOCK LeaveCriticalSection( &_cs )

int _workWaiting;
HANDLE _hWorkWaitingEvent;
HANDLE _hSyncEvent;
HANDLE _hListSyncEvent;

MemoryPool* _pool;
NativeMemorySystem* _memory;

int _fbAddress;

#define CALLBACKHANDLERCOUNT 5
CallbackHandlers _callbacks[ CALLBACKHANDLERCOUNT ];
int _lastCallbackId;

#define LISTCOUNT 10
DisplayList _lists[ LISTCOUNT ];
int _currentListId;
int _lastListId;
int _listCount;

bool _vsyncWaiting;
int64 _lastVsync;

// Statistics
extern uint64 _vcount;

void __break()
{
	Debugger::Break();
}

#pragma unmanaged

DisplayList* GetNextDisplayList()
{
	LOCK;

	DisplayList* list = &_lists[ _currentListId ];
	if( ( list->ID < 0 ) ||
		( list->Queued == true ) )
	{
		UNLOCK;
		return NULL;
	}

	list->Queued = true;

	_workWaiting--;
	_currentListId = ( _currentListId + 1 ) % LISTCOUNT;

	UNLOCK;

	return list;
}

void niSetup( NativeMemorySystem* memory, MemoryPool* pool )
{
	_memory = memory;
	_pool = pool;

	_workWaiting = 0;

	memset( _lists, 0, sizeof( DisplayList ) * LISTCOUNT );
	for( int n = 0; n < LISTCOUNT; n++ )
	{
		_lists[ n ].ID = -1;
		_lists[ n ].Done = true;
	}
	_lastListId = 0;
	_listCount = 0;
	_currentListId = 1;

	_lastCallbackId = 0;
	memset( _callbacks, 0, sizeof( CallbackHandlers ) * CALLBACKHANDLERCOUNT );
}

void niCleanup()
{
	LOCK;

	_workWaiting = 0;

	memset( _lists, 0, sizeof( DisplayList ) * LISTCOUNT );
	_lastListId = 0;
	_listCount = 0;
	_currentListId = 1;

	_lastCallbackId = 0;
	memset( _callbacks, 0, sizeof( CallbackHandlers ) * CALLBACKHANDLERCOUNT );

	_pool = NULL;
	_memory = NULL;

	UNLOCK;
}

uint _fakeVcount = 0;

uint niGetVcount()
{
	//return _vcount;
	return _fakeVcount++;
}

void niSwitchFrameBuffer( int address, int bufferWidth, int pixelFormat, int syncMode )
{
	// TODO: switch frame buffer
	_fbAddress = address;

	_vsyncWaiting = true;
}

int niGetFrameBuffer()
{
	return _fbAddress;
}

int niAddCallbackHandlers( CallbackHandlers* handlers )
{
	assert( _lastCallbackId + 1 < CALLBACKHANDLERCOUNT );
	if( _lastCallbackId + 1 >= CALLBACKHANDLERCOUNT )
		return -1;
	int cbid = _lastCallbackId + 1;
	memcpy( &_callbacks[ cbid ], handlers, sizeof( CallbackHandlers ) );
	_lastCallbackId++;
	return cbid;
}

void niRemoveCallbackHandlers( int cbid )
{
	// Not implemented
}

void niSaveContext( void* buffer )
{
	// Not implemented
}

void niRestoreContext( void* buffer )
{
	// Not implemented
}

int niEnqueueList( void* startAddress, void* stallAddress, int callbackId, bool immediate )
{
	//assert( _listCount + 1 < LISTCOUNT );

	// Safety - we can't go ahead of the current list!
	int waitTime = 0;
	while( _workWaiting >= ( LISTCOUNT - 1 ) )
	{
		_vsyncWaiting = true;
		waitTime++;
		if( waitTime > 100 ) // waits 1 second
			return 0;
		WaitForSingleObject( _hListSyncEvent, 10 );
	}

	LOCK;

	int listId = ( _lastListId + 1 ) % LISTCOUNT;

	DisplayList* list = &_lists[ listId ];
	list->ID = listId;
	_lastListId = listId;

	list->Packets = ( VideoPacket* )startAddress;
	list->StartAddress = startAddress;
	list->StallAddress = stallAddress;
	list->Base = 0x08000000; // safe to assume?
	//list->ReturnAddress = 0;
	list->StackIndex = 0;

	list->CallbackID = callbackId;
	list->Argument = 0; // ?

	list->Queued = false;
	list->Done = false;
	list->Stalled = false;
	list->Drawn = false;
	list->Cancelled = false;

	/*
	We'd do something different if we supported putting things at the head
	if( immediate == true ) {}
	else {}
	*/

	UNLOCK;

	// Signal graphics processor to start
	_workWaiting++;
	PulseEvent( _hWorkWaitingEvent );

	// HACK: wait until stalled or done
	while(
		( list->Done == false ) &&
		( list->Stalled == false ) )
	{
		WaitForSingleObject( _hListSyncEvent, 1 );
	}

	assert( listId < LISTCOUNT );
	return listId;
}

void niUpdateList( int listId, void* stallAddress )
{
	// If we get -1, chances are they want the last list ID
	if( listId == -1 )
	{
		OutputDebugString( L"niUpdateList: called with listId -1, using last list - this may be a bug" );
		listId = _lastListId;
	}

	assert( ( listId >= 0 ) && ( listId < LISTCOUNT ) );

	LOCK;

	DisplayList* list = &_lists[ listId ];
	assert( list->ID >= 0 );

	// HACK: wait until stalled or done
	if( list->StallAddress == false )
	{
		UNLOCK;
		while( list->Stalled == false )
			WaitForSingleObject( _hListSyncEvent, 1 );
		LOCK;
	}

	list->StallAddress = stallAddress;

	list->Stalled = false;
	//list->Drawn = false;

	UNLOCK;

	// Signal graphics processor to start
	PulseEvent( _hWorkWaitingEvent );

	// HACK: wait until stalled or done
	/*while(
		( list->Done == false ) &&
		( list->Stalled == false ) )
	{
		WaitForSingleObject( _hListSyncEvent, 1 );
	}*/
}

void niCancelList( int listId )
{
	assert( ( listId >= 0 ) && ( listId < LISTCOUNT ) );
	DisplayList* list = &_lists[ listId ];
	assert( list->ID >= 0 );

	// Not implemented
	//list->Cancelled = true;
}

void niSyncList( int listId, VideoSyncType syncType )
{
	assert( ( listId >= 0 ) && ( listId < LISTCOUNT ) );
	DisplayList* list = &_lists[ listId ];
	assert( list->ID >= 0 );

	//return;

	switch( syncType )
	{
	case SyncListDone:
		while( list->Done == false )
			WaitForSingleObject( _hListSyncEvent, 10 );
		break;
	case SyncListQueued:
		// ?
		break;
	case SyncListDrawn:
		//while( list->Drawn == false )
		//	WaitForSingleObject( _hListSyncEvent, 10 );
		break;
	case SyncListStalled:
		while( list->Stalled == false )
			WaitForSingleObject( _hListSyncEvent, 10 );
		break;
	case SyncListCancelled:
		//while( list->Cancelled == false )
		//	WaitForSingleObject( _hListSyncEvent, 10 );
		break;
	}
}

void UnstallAll()
{
	for( int n = 0; n < LISTCOUNT; n++ )
		_lists[ n ].Stalled = false;
}

void niSync( VideoSyncType syncType )
{
	//UnstallAll();

	switch( syncType )
	{
	case SyncListDone:
	case SyncListQueued:
	case SyncListDrawn:
	case SyncListStalled:
	case SyncListCancelled:
		break;
	}

	_vsyncWaiting = true;
}

void niWaitForVsync()
{
	//UnstallAll();

	int64 tick;
	QueryPerformanceCounter( ( LARGE_INTEGER* )&tick );
	int64 duration = ( tick - _lastVsync );
	_lastVsync = tick;
	
	// ticks are 100ns, we need ms
	duration /= ( _freq / 1000 );

	if( _speedLocked == true )
	{
		// If we are under 16ms, wait until 16ms
		// Sleep is pretty expensive, so if we are <= 1, don't do it - we can take the inaccuracy
		//if( ( ( duration > 1 ) &&
		if( ( duration > 0 ) && ( duration < 16 ) )
			Sleep( 16 - ( int )duration );
	}

	_vsyncWaiting = true;
}

#pragma managed

void OglDriver::SetupNativeInterface()
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;
	memset( ni, 0, sizeof( VideoApi ) );

	ni->Setup = &niSetup;
	ni->Cleanup = &niCleanup;
	ni->GetVcount = &niGetVcount;
	ni->SwitchFrameBuffer = &niSwitchFrameBuffer;
	ni->GetFrameBuffer = &niGetFrameBuffer;

	ni->AddCallbackHandlers = &niAddCallbackHandlers;
	ni->RemoveCallbackHandlers = &niRemoveCallbackHandlers;
	ni->SaveContext = &niSaveContext;
	ni->RestoreContext = &niRestoreContext;
	ni->EnqueueList = &niEnqueueList;
	ni->UpdateList = &niUpdateList;
	ni->CancelList = &niCancelList;
	ni->SyncList = &niSyncList;
	ni->Sync = &niSync;
	ni->WaitForVsync = &niWaitForVsync;

	InitializeCriticalSection( &_cs );

	_hWorkWaitingEvent = CreateEvent( NULL, false, false, NULL );
	_hSyncEvent = CreateEvent( NULL, false, false, NULL );
	_hListSyncEvent = CreateEvent( NULL, false, false, NULL );

	_lastVsync = 0;

	QueryPerformanceFrequency( ( LARGE_INTEGER* )&_freq );
}

void OglDriver::DestroyNativeInterface()
{
	memset( _nativeInterface, 0, sizeof( VideoApi ) );

	CloseHandle( _hWorkWaitingEvent );
	_hWorkWaitingEvent = NULL;
	CloseHandle( _hSyncEvent );
	_hSyncEvent = NULL;
	CloseHandle( _hWorkWaitingEvent );
	_hListSyncEvent = NULL;

	DeleteCriticalSection( &_cs );
}
