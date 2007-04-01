// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#pragma unmanaged
#include "MemoryPool.h"
#pragma managed
#include "R4000Cpu.h"
#include "R4000VideoInterface.h"
#include "VideoApi.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Video::Native;

#ifdef _DEBUG
#define BREAK __break()
#define BREAKIF( cond ) if( cond ) __break();
#else
#define BREAK
#define BREAKIF( cond )
#endif

#define DEFAULTPACKETCAPACITY 50000

VdlRef* _outstandingLists;
VdlRef* _outstandingListsTail;

MemorySystem* _memory;
VideoApi* _videoApi;
MemoryPool* _memoryPool;

void ClearOutstandingLists();

R4000VideoInterface::R4000VideoInterface( R4000Cpu^ cpu )
{
	_cpu = cpu;

	_outstandingLists = NULL;
	_outstandingListsTail = NULL;

	_pool = new MemoryPool();
	_memoryPool = _pool;
}

R4000VideoInterface::~R4000VideoInterface()
{
	ClearOutstandingLists();

	_memoryPool = NULL;
	SAFEDELETE( _pool );
}

void R4000VideoInterface::Prepare()
{
	_memory = _cpu->_memory->SystemInstance;
	_videoApi = ( VideoApi* )_cpu->Emulator->Video->NativeInterface.ToPointer();
	if( _videoApi != NULL )
		_videoApi->Setup( _cpu->_memory->SystemInstance, _pool );
}

void __break()
{
	Debugger::Break();
}

#pragma unmanaged

void AddOutstandingList( VideoDisplayList* list )
{
	VdlRef* ref = ( VdlRef* )malloc( sizeof( VdlRef ) );
	ref->List = list;
	ref->Next = NULL;

	if( _outstandingLists == NULL )
		_outstandingLists = ref;
	if( _outstandingListsTail != NULL )
		_outstandingListsTail->Next = ref;
	_outstandingListsTail = ref;
}

VideoDisplayList* FindOutstandingList( int listId )
{
	VdlRef* ref = _outstandingLists;

	// Check tail first
	if( _outstandingListsTail != NULL )
	{
		if( _outstandingListsTail->List->ID == listId )
			return _outstandingListsTail->List;
	}

	while( ref != NULL )
	{
		if( ref->List->ID == listId )
			return ref->List;
		ref = ref->Next;
	}

	return NULL;
}

void RemoveOutstandingList( int listId )
{
	VdlRef* ref = _outstandingLists;
	VdlRef* prev = NULL;

	while( ref != NULL )
	{
		if( ref->List->ID == listId )
		{
			if( prev != NULL )
				prev->Next = ref->Next;
			if( ref == _outstandingLists )
				_outstandingLists = ref->Next;
			if( ref == _outstandingListsTail )
				_outstandingListsTail = prev;
			SAFEFREE( ref );
			return;
		}
		prev = ref;
		ref = ref->Next;
	}
}

void RemoveOutstandingList( VideoDisplayList* list )
{
	VdlRef* ref = _outstandingLists;
	VdlRef* prev = NULL;

	while( ref != NULL )
	{
		if( ref->List == list )
		{
			if( prev != NULL )
				prev->Next = ref->Next;
			if( ref == _outstandingLists )
				_outstandingLists = ref->Next;
			if( ref == _outstandingListsTail )
				_outstandingListsTail = prev;
			SAFEFREE( ref );
			return;
		}
		prev = ref;
		ref = ref->Next;
	}
}

void ClearOutstandingLists()
{
	VdlRef* ref = _outstandingLists;
	while( ref != NULL )
	{
		VdlRef* next = ref->Next;
		SAFEFREE( ref );
		ref = next;
	}
	_outstandingLists = NULL;
	_outstandingListsTail = NULL;
}

void __inline GetVideoPacket( int code, int baseAddress, VideoPacket* packet )
{
	//packet->Command = ( byte )( code >> 24 );
	//packet->Argument = code & 0x00FFFFFF;
	*( ( int* )packet ) = code;

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

bool ReadPackets( uint pointer, uint stallAddress, VideoDisplayList* list )
{
	if( stallAddress == 0 )
		stallAddress = 0xFFFFFFFF;

	// Fix for cached addresses/etc
	pointer &= 0x3FFFFFFF;
	stallAddress &= 0x3FFFFFFF;

	int packetCount = list->PacketCount;
	while( pointer < stallAddress )
	{
		if( packetCount >= list->PacketCapacity )
		{
			// Ran out of space! No clue what to do! Ack!
			// Could we use MemoryPool to realloc and such?
			BREAK;
		}

		int code = *( ( int* )( _memory->Translate( pointer ) ) );
		VideoPacket* packet = list->Packets + packetCount;
		GetVideoPacket( code, list->Base, packet );
		packetCount++;

		pointer += 4;

		switch( packet->Command )
		{
		case BASE:
			list->Base = packet->Argument << 8;
			break;
		case JUMP:
			pointer = packet->Argument | list->Base;
			break;

			// Not implemented
		case CALL:
		case RET:
		case BJUMP:
			BREAK;
			break;

			// Stop conditions?
		case FINISH:
		case END:
		case Unknown0x11:
			list->PacketCount = packetCount;
			return true;
		}
	}

	list->PacketCount = packetCount;
	return false;
}

bool ReadMorePackets( VideoDisplayList* vdl, uint newStallAddress )
{
	int oldStallAddress = vdl->StallAddress;
	vdl->StallAddress = newStallAddress;
	bool done = ReadPackets( oldStallAddress, vdl->StallAddress, vdl );
	vdl->Ready = done;
	return done;
}

// sceDisplayForUser -----------------------------------

int sceDisplayGetVcount()
{
	VideoApi* ni = _videoApi;
	return ni->GetVcount();
}

int sceDisplaySetFrameBuf( int address, int bufferWidth, int pixelFormat, int syncMode )
{
	VideoApi* ni = _videoApi;
	ni->SwitchFrameBuffer( address, bufferWidth, pixelFormat, syncMode );
	return 0;
}

void sceDisplayWaitVblank()
{
	VideoApi* ni = _videoApi;
	ni->WaitForVsync();
}

void sceDisplayWaitVblankStart()
{
	VideoApi* ni = _videoApi;
	ni->WaitForVsync();
}

// sceGeUser -------------------------------------------

int sceGeListEnQueue( uint list, uint stall, int cbid, uint arg, int head )
{
	VideoDisplayList* vdl = ( VideoDisplayList* )malloc( sizeof( VideoDisplayList ) );
	memset( vdl, 0, sizeof( VideoDisplayList ) );
	vdl->CallbackID = cbid;
	vdl->Argument = arg;

	vdl->PacketCapacity = DEFAULTPACKETCAPACITY;
	vdl->Packets = ( VideoPacket* )_memoryPool->Request( DEFAULTPACKETCAPACITY * sizeof( VideoPacket ) );

	// BUG: seems to be a big issue here sometimes...
//#ifdef _DEBUG
//	memset( vdl->Packets, 0, DEFAULTPACKETCAPACITY * sizeof( VideoPacket ) );
//#endif

	if( stall == NULL )
	{
		vdl->Ready = true;

		// Read all now
		bool done = ReadPackets( list, 0, vdl );
		BREAKIF( done == false );
	}
	else
	{
		vdl->Ready = false;
		vdl->StallAddress = stall;

		// Rest will follow
		bool done = ReadPackets( list, stall, vdl );
		BREAKIF( done == true );

		AddOutstandingList( vdl );
	}

	VideoApi* ni = _videoApi;
	int listId = ni->EnqueueList( vdl, ( head == 1 ) ? true : false );
	if( listId >= 0 )
		return listId;
	else
		return -1;
}

int sceGeListDeQueue( int qid )
{
	VideoApi* ni = _videoApi;

	RemoveOutstandingList( qid );
	ni->DequeueList( qid );

	return 0;
}

int sceGeListUpdateStallAddr( int qid, uint stall )
{
	VideoApi* ni = _videoApi;

	//VideoDisplayList* vdl = ni->FindList( qid );
	VideoDisplayList* vdl = FindOutstandingList( qid );
	if( vdl == NULL )
	{
		BREAK;
		return -1;
	}

	if( vdl->Ready == false )
	{
		bool done = ReadMorePackets( vdl, stall );
		if( done == true )
		{
			RemoveOutstandingList( vdl );
			ni->SyncList( vdl->ID );
		}
	}

	return 0;
}

int sceGeListSync( int qid, int syncType )
{
	VideoApi* ni = _videoApi;

	//VideoDisplayList* vdl = ni->FindList( qid );
	VideoDisplayList* vdl = FindOutstandingList( qid );
	if( vdl == NULL )
	{
		// Could have been processed already
		return 0;
	}

	if( vdl->Ready == false )
	{
		bool done = ReadMorePackets( vdl, 0 );
		if( done == false )
			BREAK;
		else
		{
			RemoveOutstandingList( vdl );
			ni->SyncList( vdl->ID );
		}
	}

	return 0;
}

int sceGeDrawSync( int syncType )
{
	// Can only handle syncType == 0
	BREAKIF( syncType != 0 );

	// This a full sync - we need to finish all lists
	VdlRef* ref = _outstandingLists;
	while( ref != NULL )
	{
		VideoDisplayList* vdl = ref->List;
		if( vdl->Ready == false )
		{
			bool done = ReadMorePackets( vdl, 0 );
			BREAKIF( done == false );
		}
		ref = ref->Next;
	}

	ClearOutstandingLists();

	VideoApi* ni = _videoApi;
	ni->Sync();

	return 0;
}

#pragma managed