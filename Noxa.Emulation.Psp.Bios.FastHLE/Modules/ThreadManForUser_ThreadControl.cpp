// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "ThreadManForUser.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KernelPartition.h"
#include "KernelStatistics.h"
#include "KernelThread.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// int sceKernelSleepThread(); (/user/pspthreadman.h:244)
int ThreadManForUser::sceKernelSleepThread()
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitID = 0;
	thread->WaitClass = KernelThreadWait::Sleep;
	thread->CanHandleCallbacks = false;
	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelSleepThreadCB(); (/user/pspthreadman.h:255)
int ThreadManForUser::sceKernelSleepThreadCB()
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitID = 0;
	thread->WaitClass = KernelThreadWait::Sleep;
	thread->CanHandleCallbacks = true;
	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelWakeupThread(SceUID thid); (/user/pspthreadman.h:264)
int ThreadManForUser::sceKernelWakeupThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Ready;
	thread->WakeupCount++;

	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelCancelWakeupThread(SceUID thid); (/user/pspthreadman.h:273)
int ThreadManForUser::sceKernelCancelWakeupThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	// cancel wakeup not supported - perhaps we shouldn't cs in sceKernelWakeupThread?
	Debug::Assert( false );

	if( thread->State == KernelThreadState::Ready )
	{
		thread->State = KernelThreadState::Waiting;
		thread->WaitID = 0;
		thread->WaitClass = KernelThreadWait::Sleep;

		return 0;
	}
	else
		return -1;
}

// int sceKernelSuspendThread(SceUID thid); (/user/pspthreadman.h:282)
int ThreadManForUser::sceKernelSuspendThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Suspended;

	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelResumeThread(SceUID thid); (/user/pspthreadman.h:291)
int ThreadManForUser::sceKernelResumeThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Ready;

	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:301)
int ThreadManForUser::sceKernelWaitThreadEnd( IMemory^ memory, int thid, int timeout )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitID = thid;
	thread->WaitClass = KernelThreadWait::ThreadEnd;
	if( timeout != 0x0 )
		thread->WaitTimeout = memory->ReadWord( timeout );
	else
		thread->WaitTimeout = 0;
	thread->CanHandleCallbacks = false;

	_kernel->ContextSwitch();

	return 0;
}

// int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:311)
int ThreadManForUser::sceKernelWaitThreadEndCB( IMemory^ memory, int thid, int timeout )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitID = thid;
	thread->WaitClass = KernelThreadWait::ThreadEnd;
	if( timeout != 0x0 )
		thread->WaitTimeout = memory->ReadWord( timeout );
	else
		thread->WaitTimeout = 0;
	thread->CanHandleCallbacks = true;

	_kernel->ContextSwitch();

	return 0;
}

int ThreadManForUser::ThreadDelayComparer( KernelThread^ a, KernelThread^ b )
{
	int64 aend = a->WaitTimestamp + a->WaitTimeout;
	int64 bend = b->WaitTimestamp + a->WaitTimeout;
	return aend.CompareTo( bend );
}

// int sceKernelDelayThread(SceUInt delay); (/user/pspthreadman.h:323)
int ThreadManForUser::sceKernelDelayThread( int delay )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitClass = KernelThreadWait::Delay;
	thread->WaitID = 0;
	thread->WaitTimeout = delay * 10; // in us - convert to ticks
	thread->WaitTimestamp = DateTime::Now.Ticks;
	thread->CanHandleCallbacks = false;

	_kernel->_delayedThreads->Add( thread );
	_kernel->_delayedThreads->Sort( gcnew Comparison<KernelThread^>( this, &ThreadManForUser::ThreadDelayComparer ) );

	if( _kernel->_delayedThreadTimer->Enabled == false )
		_kernel->SpawnDelayedThreadTimer( thread->WaitTimeout + thread->WaitTimestamp );

	_kernel->ContextSwitch();
	
	return 0;
}

// int sceKernelDelayThreadCB(SceUInt delay); (/user/pspthreadman.h:335)
int ThreadManForUser::sceKernelDelayThreadCB( int delay )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Waiting;
	thread->WaitClass = KernelThreadWait::Delay;
	thread->WaitID = 0;
	thread->WaitTimeout = delay * 10; // us->ticks
	thread->WaitTimestamp = DateTime::Now.Ticks;
	thread->CanHandleCallbacks = true;

	_kernel->_delayedThreads->Add( thread );
	_kernel->_delayedThreads->Sort( gcnew Comparison<KernelThread^>( this, &ThreadManForUser::ThreadDelayComparer ) );

	if( _kernel->_delayedThreadTimer->Enabled == false )
		_kernel->SpawnDelayedThreadTimer( thread->WaitTimeout + thread->WaitTimestamp );

	_kernel->ContextSwitch();
	
	return 0;
}

// int sceKernelDelaySysClockThread(SceKernelSysClock *delay); (/user/pspthreadman.h:344)
int ThreadManForUser::sceKernelDelaySysClockThread( IMemory^ memory, int delay )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;
	
	return NISTUBRETURN;
}

// int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay); (/user/pspthreadman.h:354)
int ThreadManForUser::sceKernelDelaySysClockThreadCB( IMemory^ memory, int delay )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;
	
	return NISTUBRETURN;
}
