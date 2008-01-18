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

NativeMemorySystem*	_memory;
VideoApi*			_videoApi;
MemoryPool*			_memoryPool;

R4000VideoInterface::R4000VideoInterface( R4000Cpu^ cpu )
{
	_cpu = cpu;

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
	_memory = _cpu->_memory->NativeSystem;
	if( _cpu->Emulator->Video == nullptr )
		_videoApi = NULL;
	else
	{
		_videoApi = ( VideoApi* )_cpu->Emulator->Video->NativeInterface.ToPointer();
		if( _videoApi != NULL )
			_videoApi->Setup( _memory, _pool );
	}
}

void __break()
{
	Debugger::Break();
}

#pragma unmanaged

// sceDisplayForUser -----------------------------------

int sceDisplayGetVcount()
{
	return _videoApi->GetVcount();
}

int sceDisplaySetFrameBuf( int address, int bufferWidth, int pixelFormat, int syncMode )
{
	_videoApi->SwitchFrameBuffer( address, bufferWidth, pixelFormat, syncMode );
	return 0;
}

int sceDisplayWaitVblank()
{
	_videoApi->WaitForVsync();
	return _videoApi->GetFrameBuffer();
}

int sceDisplayWaitVblankStart()
{
	_videoApi->WaitForVsync();
	return _videoApi->GetFrameBuffer();
}

// sceGeUser -------------------------------------------

int sceGeListEnQueue( uint list, uint stall, int cbid, uint arg, int head )
{
	// Fix for cached addresses/etc
	list &= 0x3FFFFFFF;
	stall &= 0x3FFFFFFF;

	void* stallAddress = ( void* )-1;
	if( stall != 0 )
		stallAddress = _memory->Translate( stall );
	return _videoApi->EnqueueList( _memory->Translate( list ), stallAddress, cbid, ( head == 1 ) ? true : false );
}

int sceGeListDeQueue( int qid )
{
	_videoApi->CancelList( qid );
	return 0;
}

int sceGeListUpdateStallAddr( int qid, uint stall )
{
	// Fix for cached addresses/etc
	stall &= 0x3FFFFFFF;

	void* stallAddress = ( void* )-1;
	if( stall != 0 )
		stallAddress = _memory->Translate( stall );
	_videoApi->UpdateList( qid, stallAddress );
	return 0;
}

int sceGeListSync( int qid, int syncType )
{
	return _videoApi->SyncList( qid, syncType );
}

int sceGeDrawSync( int syncType )
{
	return _videoApi->Sync( syncType );
}

#pragma managed
