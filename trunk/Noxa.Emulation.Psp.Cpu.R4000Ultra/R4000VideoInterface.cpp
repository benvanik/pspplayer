// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000VideoInterface.h"
#include "VideoApi.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Video::Native;

#define DEFAULTPACKETCAPACITY 200000

#define OUTSTANDINGLISTCOUNT 50
VideoDisplayList* _outstandingLists[ OUTSTANDINGLISTCOUNT ];
int _maxOutstandingList;

void* _memoryAddress;
VideoApi* _videoApi;
MemoryPool* _memoryPool;

R4000VideoInterface::R4000VideoInterface( R4000Cpu^ cpu )
{
	_cpu = cpu;

	memset( _outstandingLists, 0, OUTSTANDINGLISTCOUNT * sizeof( VideoDisplayList* ) );
	_maxOutstandingList = -1;

	_pool = new MemoryPool();
	_memoryPool = _pool;
}

R4000VideoInterface::~R4000VideoInterface()
{
	_memoryPool = NULL;
	SAFEDELETE( _pool );
}

void R4000VideoInterface::Prepare()
{
	_memoryAddress = _cpu->_memory->MainMemory;
	_videoApi = ( VideoApi* )_cpu->Emulator->Video->NativeInterface.ToPointer();

	_videoApi->Setup( _pool );
}

void __break()
{
#ifdef _DEBUG
	Debugger::Break();
#endif
}

#pragma unmanaged

void AddOutstandingList( VideoDisplayList* list )
{
	for( int n = 0; n < OUTSTANDINGLISTCOUNT; n++ )
	{
		if( _outstandingLists[ n ] == NULL )
		{
			if( n > _maxOutstandingList )
				_maxOutstandingList = n;
			_outstandingLists[ n ] = list;
			return;
		}
	}

	// If here, we ran out of room
	__break();
}

VideoDisplayList* FindOutstandingList( int listId )
{
	for( int n = 0; n < OUTSTANDINGLISTCOUNT; n++ )
	{
		VideoDisplayList* list = _outstandingLists[ n ];
		if( list == NULL )
			continue;
		if( n > _maxOutstandingList )
			break;

		if( list->ID == listId )
			return list;
	}

	return NULL;
}

void RemoveOutstandingList( VideoDisplayList* list )
{
	int lastMax = 0;
	for( int n = 0; n < OUTSTANDINGLISTCOUNT; n++ )
	{
		if( _outstandingLists[ n ] == list )
		{
			_outstandingLists[ n ] = NULL;
			if( _maxOutstandingList == n )
				_maxOutstandingList = lastMax;
			return;
		}
		else if( _outstandingLists[ n ] != NULL )
			lastMax = n;
	}

	// Not found
	__break();
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

bool ReadPackets( void* memoryAddress, uint pointer, uint stallAddress, VideoDisplayList* list )
{
	if( stallAddress == 0 )
		stallAddress = 0xFFFFFFFF;

	// Fix for cached addresses/etc
	pointer &= 0x3FFFFFFF;
	stallAddress &= 0x3FFFFFFF;

	int packetCount = list->PacketCount;

	int* memptr = ( int* )memoryAddress;
	while( pointer < stallAddress )
	{
		if( packetCount >= list->PacketCapacity )
		{
			// Ran out of space! No clue what to do! Ack!
			// Could we use MemoryPool to realloc and such?
			__break();
		}

		int code = *( ( int* )( ( byte* )memptr + ( pointer - 0x08000000 ) ) );
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
			__break();
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

bool ReadMorePackets( void* memoryAddress, VideoDisplayList* vdl, uint newStallAddress )
{
	int oldStallAddress = vdl->StallAddress;
	vdl->StallAddress = newStallAddress;
	bool done = ReadPackets( memoryAddress, oldStallAddress, vdl->StallAddress, vdl );
	vdl->Ready = done;
	if( done == true )
		RemoveOutstandingList( vdl );
	return done;
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
#ifdef _DEBUG
	memset( vdl->Packets, 0, DEFAULTPACKETCAPACITY * sizeof( VideoPacket ) );
#endif

	if( stall == NULL )
	{
		vdl->Ready = true;

		// Read all now
		bool done = ReadPackets( _memoryAddress, list, 0, vdl );
		if( done == false )
			__break();
	}
	else
	{
		vdl->Ready = false;
		vdl->StallAddress = stall;

		// Rest will follow
		bool done = ReadPackets( _memoryAddress, list, stall, vdl );
		if( done == true )
			__break();

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
	// TODO: get api from someplace
	VideoApi* ni = NULL;
	ni->DequeueList( qid );

	return 0;
}

int sceGeListUpdateStallAddr( int qid, uint stall )
{
	VideoApi* ni = _videoApi;

	VideoDisplayList* vdl = ni->FindList( qid );
	if( vdl == NULL )
	{
		__break();
		return -1;
	}

	if( vdl->Ready == false )
	{
		bool done = ReadMorePackets( _memoryAddress, vdl, stall );
		if( done == true )
			ni->SyncList( vdl->ID );
	}

	return 0;
}

int sceGeListSync( int qid, int syncType )
{
	VideoApi* ni = _videoApi;

	VideoDisplayList* vdl = ni->FindList( qid );
	if( vdl == NULL )
	{
		// Could have been processed already
		return 0;
	}

	if( vdl->Ready == false )
	{
		bool done = ReadMorePackets( _memoryAddress, vdl, 0 );
		if( done == false )
			__break();

		ni->SyncList( vdl->ID );
	}

	return 0;
}

int sceGeDrawSync( int syncType )
{
	// Can only handle syncType == 0
	if( syncType != 0 )
		__break();

	// This a full sync - we need to finish all lists
	for( int n = 0; n < _maxOutstandingList; n++ )
	{
		VideoDisplayList* vdl = _outstandingLists[ n ];
		if( vdl == NULL )
			continue;

		if( vdl->Ready == false )
		{
			bool done = ReadMorePackets( _memoryAddress, vdl, 0 );
			if( done == false )
				__break();
		}
	}

	VideoApi* ni = _videoApi;
	ni->Sync();

	return 0;
}

#pragma managed