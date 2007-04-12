// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "ThreadManForUser.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KPartition.h"
#include "KThread.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// int sceKernelSleepThread(); (/user/pspthreadman.h:244)
int ThreadManForUser::sceKernelSleepThread()
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Sleep( false );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelSleepThreadCB(); (/user/pspthreadman.h:255)
int ThreadManForUser::sceKernelSleepThreadCB()
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Sleep( true );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelWakeupThread(SceUID thid); (/user/pspthreadman.h:264)
int ThreadManForUser::sceKernelWakeupThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->Wake();
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelCancelWakeupThread(SceUID thid); (/user/pspthreadman.h:273)
int ThreadManForUser::sceKernelCancelWakeupThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	// cancel wakeup not supported - perhaps we shouldn't cs in sceKernelWakeupThread?
	assert( false );

	if( thread->State == KThreadReady )
	{
		thread->Sleep( thread->CanHandleCallbacks );

		return 0;
	}
	else
		return -1;
}

// int sceKernelSuspendThread(SceUID thid); (/user/pspthreadman.h:282)
int ThreadManForUser::sceKernelSuspendThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->Suspend();
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelResumeThread(SceUID thid); (/user/pspthreadman.h:291)
int ThreadManForUser::sceKernelResumeThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->Resume();
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:301)
int ThreadManForUser::sceKernelWaitThreadEnd( IMemory^ memory, int thid, int timeout )
{
	KThread* thread = ( KThread* )_kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	KThread* targetThread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( targetThread == NULL )
		return -1;

	// If already stopped, return
	if( ( targetThread->State == KThreadDead ) ||
		( targetThread->State == KThreadStopped ) )
		return 0;

	uint timeoutUs;
	if( timeout != 0 )
	{
		uint* ptimeout = ( uint* )MSI( memory )->Translate( timeout );
		timeoutUs = *ptimeout;
	}

	thread->Join( targetThread, timeoutUs, false );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:311)
int ThreadManForUser::sceKernelWaitThreadEndCB( IMemory^ memory, int thid, int timeout )
{
	KThread* thread = ( KThread* )_kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	KThread* targetThread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( targetThread == NULL )
		return -1;

	// If already stopped, return
	if( ( targetThread->State == KThreadDead ) ||
		( targetThread->State == KThreadStopped ) )
		return 0;

	uint timeoutUs;
	if( timeout != 0 )
	{
		uint* ptimeout = ( uint* )MSI( memory )->Translate( timeout );
		timeoutUs = *ptimeout;
	}

	thread->Join( targetThread, timeoutUs, true );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelDelayThread(SceUInt delay); (/user/pspthreadman.h:323)
int ThreadManForUser::sceKernelDelayThread( int delay )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Delay( delay, false );
	if( _kernel->Schedule() == true )
	{
	}
	
	return 0;
}

// int sceKernelDelayThreadCB(SceUInt delay); (/user/pspthreadman.h:335)
int ThreadManForUser::sceKernelDelayThreadCB( int delay )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Delay( delay, true );
	if( _kernel->Schedule() == true )
	{
	}
	
	return 0;
}

// int sceKernelDelaySysClockThread(SceKernelSysClock *delay); (/user/pspthreadman.h:344)
int ThreadManForUser::sceKernelDelaySysClockThread( IMemory^ memory, int delay )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;
	
	return NISTUBRETURN;
}

// int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay); (/user/pspthreadman.h:354)
int ThreadManForUser::sceKernelDelaySysClockThreadCB( IMemory^ memory, int delay )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;
	
	return NISTUBRETURN;
}
