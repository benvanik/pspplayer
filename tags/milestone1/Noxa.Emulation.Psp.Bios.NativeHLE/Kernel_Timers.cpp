// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Kernel.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

void Kernel::SetupTimerQueue()
{
	_hTimerQueue = CreateTimerQueue();
	assert( _hTimerQueue != NULL );
}

void Kernel::DestroyTimerQueue()
{
	DeleteTimerQueue( _hTimerQueue );
	_hTimerQueue = NULL;
}

#pragma unmanaged
VOID CALLBACK TimerRoutine( PVOID lpParam, BOOLEAN TimerOrWaitFired )
{
	KernelTimer* t = ( KernelTimer* )lpParam;
	t->Function( t->Kernel, t->Args );

	if( t->Repeat == false )
		t->Kernel->CancelTimer( t );
}
#pragma managed

KernelTimer* Kernel::AddTimer( TimerFunction function, void* args, int dueTimeMs, int intervalMs )
{
	KernelTimer* t = ( KernelTimer* )malloc( sizeof( KernelTimer ) );
	t->Kernel = this;
	t->Function = function;
	t->Args = args;
	t->Repeat = ( intervalMs > 0 );

	HANDLE handle;
	BOOL created = CreateTimerQueueTimer( &handle, _hTimerQueue,
		TimerRoutine, t,
		dueTimeMs, intervalMs,
		WT_EXECUTEINTIMERTHREAD );
	assert( created == TRUE );

	t->Handle = handle;
	
	return t;
}

void Kernel::CancelTimer( KernelTimer* timer )
{
	assert( timer != NULL );

	DeleteTimerQueueTimer( _hTimerQueue, timer->Handle, NULL );

	SAFEFREE( timer );
}
