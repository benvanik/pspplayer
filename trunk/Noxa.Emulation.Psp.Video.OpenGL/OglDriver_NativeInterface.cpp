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
#include <string>

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

// This interface works by adding lists to a linked list of waiting lists.
// When Sync is called, the pending lists are set to _pendingBatch and the
// _lists/etc are reset to wait for more lists. When the video worker calls
// GetNextListBatch() it will grab _pendingBatch if possible. If there is
// already a batch in _pendingBatch when Sync is called, the existing
// _pendingBatch is cleared - effectively implementing frame skipping (for
// the case when the video worker cannot grab batches fast enough).
// If frameskipping is not desired, if Sync is called and _pendingBatch has
// data it should wait until GetNextListBatch() is called.

// After this many lists have been allocated, break - it means we probably have an issue
#define LISTCOUNTALERTTHRESHOLD 100

#ifdef _DEBUG
#define BREAK __break()
#else
#define BREAK
#endif

CRITICAL_SECTION _cs;
HANDLE _hSyncEvent;
#define LOCK EnterCriticalSection( &_cs )
#define UNLOCK LeaveCriticalSection( &_cs )

VdlRef* _lists;
VdlRef* _listsTail;
int _nextListId;
int _listCount;

VdlRef* _pendingBatch;
int _pendingCount;

MemoryPool* _pool;

// Statistics
extern uint _processedFrames;
extern uint _skippedFrames;
extern uint _vcount;

void __break()
{
	Debugger::Break();
}

VdlRef* GetNextListBatch();
void MigrateBatch( bool waitIfPending );
void FreeList( VideoDisplayList* list );

#pragma unmanaged

VdlRef* GetNextListBatch()
{
	LOCK;

	MigrateBatch( false );

	VdlRef* batch = _pendingBatch;
	_pendingBatch = NULL;
	_pendingCount = 0;

#ifdef STATISTICS
	if( batch != NULL )
		_processedFrames++;
#endif

	UNLOCK;

	PulseEvent( _hSyncEvent );

	return batch;
}

void FreeList( VideoDisplayList* list )
{
	// Should we be locked here?

	_pool->Release( list->Packets );
#ifdef _DEBUG
	memset( list, 0, sizeof( VideoDisplayList ) );
#endif
	SAFEFREE( list );
}

// Caller needs to lock!
void MigrateBatch( bool waitIfPending )
{
	VdlRef* batch = NULL;
	VdlRef* batchTail = NULL;
	int fetched = 0;

	VdlRef* ref;

	// Remove old batch if needed or wait on it
	if( _pendingBatch != NULL )
	{
		if( waitIfPending == true )
			WaitForSingleObject( _hSyncEvent, INFINITE );
		else
		{
			// Clear old list
			ref = _pendingBatch;
			while( ref != NULL )
			{
				VdlRef* next = ref->Next;

				FreeList( ref->List );
				SAFEFREE( ref );

				ref = next;
			}
			_pendingBatch = NULL;

#ifdef STATISTICS
			_skippedFrames++;
#endif
		}
	}

	_pendingCount = 0;

	ref = _lists;
	VdlRef* prev = NULL;
	while( ref != NULL )
	{
		if( ref->List->Ready == true )
		{
			VdlRef* next = ref->Next;
			if( prev != NULL )
				prev->Next = next;
			ref->Next = NULL;

			if( batch == NULL )
				batch = ref;
			if( batchTail != NULL )
				batchTail->Next = ref;
			batchTail = ref;

			if( ref == _lists )
				_lists = next;
			if( ref == _listsTail )
			{
				if( next == NULL )
					_listsTail = prev;
				else
					_listsTail = next;
			}

			fetched++;

			// prev stays what it is as it is the previous still in the list
			ref = next;
		}
		else
		{
			prev = ref;
			ref = ref->Next;
		}
	}

	_listCount -= fetched;
	_pendingCount = fetched;

	_pendingBatch = batch;
}

void niSetup( MemoryPool* pool )
{
	_pool = pool;
	_lists = NULL;
	_listsTail = NULL;
	_nextListId = 1;
	_listCount = 0;

	_pendingBatch = NULL;
	_pendingCount = 0;
}

void FreeLL( VdlRef* head )
{
	VdlRef* ref = head;
	while( ref != NULL )
	{
		VdlRef* next = ref->Next;

		FreeList( ref->List );
		SAFEFREE( ref );

		ref = next;
	}
}

void niCleanup()
{
	LOCK;

	FreeLL( _pendingBatch );
	_pendingBatch = NULL;

	_listsTail = NULL;
	FreeLL( _lists );
	_lists = NULL;

	_nextListId = 1;
	_listCount = 0;
	_pendingCount = 0;

	_pool = NULL;

	UNLOCK;
}

uint niGetVcount()
{
	return _vcount;
}

void niSwitchFrameBuffer( int address, int bufferWidth, int pixelFormat, int syncMode )
{
	// TODO: switch frame buffer
}

VideoDisplayList* niFindList( int listId )
{
	LOCK;

	VdlRef* ref = _lists;
	if( ref == NULL )
	{
		UNLOCK;
		return NULL;
	}

	// Check tail first - we often get called when updating the last list
	if( _listsTail != NULL )
	{
		if( _listsTail->List->ID == listId )
		{
			VideoDisplayList* vdl = _listsTail->List;
			UNLOCK;
			return vdl;
		}
	}

	while( ref != NULL )
	{
		if( ref->List->ID == listId )
		{
			VideoDisplayList* vdl = ref->List;
			UNLOCK;
			return vdl;
		}
		ref = ref->Next;
	}

	UNLOCK;
	return NULL;
}

int niEnqueueList( VideoDisplayList* list, bool immediate )
{
	int listId = _nextListId++;
	list->ID = listId;

	VdlRef* ref = ( VdlRef* )malloc( sizeof( VdlRef ) );
	ref->List = list;

	LOCK;

	if( immediate == true )
	{
		// Insert at head
		ref->Next = _lists;
		_lists = ref;
		if( _listsTail == NULL )
			_listsTail = ref;
	}
	else
	{
		// Append
		ref->Next = NULL;
		if( _listsTail != NULL )
			_listsTail->Next = ref;
		_listsTail = ref;
		if( _lists == NULL )
			_lists = ref;
	}

	_listCount++;

	UNLOCK;

#ifdef _DEBUG
	if( _listCount > LISTCOUNTALERTTHRESHOLD )
		BREAK;
#endif

	return listId;
}

void niDequeueList( int listId )
{
	LOCK;

	VdlRef* ref = _lists;
	if( ref == NULL )
	{
		UNLOCK;
		return;
	}

	VdlRef* prev = NULL;
	while( ref != NULL )
	{
		if( ref->List->ID == listId )
		{
			VdlRef* next = ref->Next;
			if( prev != NULL )
				prev->Next = next;

			if( ref == _lists )
				_lists = next;
			if( ref == _listsTail )
				_listsTail = prev;

			FreeList( ref->List );
			SAFEFREE( ref );

			_listCount--;

			UNLOCK;
			return;
		}
		prev = ref;
		ref = ref->Next;
	}

	UNLOCK;
}

void niSyncList( int listId )
{
}

void niSync()
{
	LOCK;

#ifdef FRAMESKIPPING
	MigrateBatch( false );
#else
	MigrateBatch( true );
#endif

	UNLOCK;
}

void niWaitForVsync()
{
}

#pragma managed

void OglDriver::SetupNativeInterface()
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;

	ni->Setup = &niSetup;
	ni->Cleanup = &niCleanup;
	ni->GetVcount = &niGetVcount;
	ni->SwitchFrameBuffer = &niSwitchFrameBuffer;
	ni->FindList = &niFindList;
	ni->EnqueueList = &niEnqueueList;
	ni->DequeueList = &niDequeueList;
	ni->SyncList = &niSyncList;
	ni->Sync = &niSync;
	ni->WaitForVsync = &niWaitForVsync;

	InitializeCriticalSection( &_cs );

	_hSyncEvent = CreateEvent( NULL, false, false, NULL );
}

void OglDriver::DestroyNativeInterface()
{
	CloseHandle( _hSyncEvent );
	_hSyncEvent = NULL;

	DeleteCriticalSection( &_cs );
}
