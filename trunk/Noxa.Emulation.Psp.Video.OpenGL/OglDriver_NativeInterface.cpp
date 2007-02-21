// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"
#include "VideoApi.h"
#include "MemoryPool.h"
#include <string>

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

typedef struct VdlRef_t
{
	VideoDisplayList*	List;
	VdlRef_t*			Next;
} VdlRef;

VdlRef* _lists;
VdlRef* _listsTail;
int _nextListId;

MemoryPool* _pool;

void __break()
{
#ifdef _DEBUG
	Debugger::Break();
#endif
}

#pragma unmanaged

void niSetup( MemoryPool* pool )
{
	_pool = pool;
	_lists = NULL;
	_listsTail = NULL;
	_nextListId = 1;
}

VideoDisplayList* niFindList( int listId )
{
	VdlRef* ref = _lists;
	if( ref == NULL )
		return NULL;

	// Check tail first - we often get called when updating the last list
	if( _listsTail != NULL )
	{
		if( _listsTail->List->ID == listId )
			return _listsTail->List;
	}

	while( ref != NULL )
	{
		if( ref->List->ID == listId )
			return ref->List;
		ref = ref->Next;
	}

	return NULL;
}

int niEnqueueList( VideoDisplayList* list, bool immediate )
{
	int listId = _nextListId++;
	list->ID = listId;

	VdlRef* ref = ( VdlRef* )malloc( sizeof( VdlRef ) );
	ref->List = list;

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

	return listId;
}

void niDequeueList( int listId )
{
	VdlRef* ref = _lists;
	if( ref == NULL )
		return;

	VdlRef* last = NULL;
	while( ref != NULL )
	{
		if( ref->List->ID == listId )
		{
			if( last != NULL )
				last->Next = ref->Next;
			_pool->Release( ref->List->Packets );
#ifdef _DEBUG
			memset( ref->List, 0, sizeof( VideoDisplayList ) );
#endif
			SAFEFREE( ref->List );
			SAFEFREE( ref );

			return;
		}
		last = ref;
		ref = ref->Next;
	}
}

void niSyncList( int listId )
{
}

void niSync()
{
}

#pragma managed

void OglDriver::FillNativeInterface()
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;

	ni->Setup = &niSetup;
	ni->FindList = &niFindList;
	ni->EnqueueList = &niEnqueueList;
	ni->DequeueList = &niDequeueList;
	ni->SyncList = &niSyncList;
	ni->Sync = &niSync;
}
