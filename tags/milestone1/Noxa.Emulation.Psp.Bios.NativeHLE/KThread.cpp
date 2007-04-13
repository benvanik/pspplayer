// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <malloc.h>
#include "Kernel.h"
#include "KThread.h"
#include "KEvent.h"
#include "KCallback.h"
#include "KPartition.h"
#include "CpuApi.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu::Native;

KThread::KThread( Bios::Kernel* kernel, KPartition* partition, char* name, uint entryAddress, int priority, KThreadAttributes attributes, uint stackSize )
{
	Kernel = kernel;

	Name = _strdup( name );
	EntryAddress = entryAddress;
	InitialPriority = priority;
	Priority = priority;
	Attributes = attributes;
	ReturnValue = int::MinValue;

	GlobalPointer = 0;
	ExitCode = 0;
	InterruptPreemptionCount = 0;
	ReleaseCount = 0;
	ThreadPreemptionCount = 0;
	WakeupCount = 0;
	RunClocks = 0;

	WaitingOn = 0;
	WaitAddress = 0;
	WaitArgument = 0;
	WaitEvent = NULL;
	WaitEventMode = 0;
	WaitThread = NULL;
	WaitTimeout = 0;
	WaitTimestamp = 0;

	SpecialHandlers = new Vector<KSpecialHandler*>( 10 );
	ActiveSpecialHandler = -1;

	State = KThreadStopped;

	Context = ( KThreadContext* )calloc( 1, sizeof( KThreadContext ) );

	ExitWaiters = new LL<KThread*>();

	assert( partition != NULL );

	Partition = partition;

	StackBlock = partition->Allocate( KAllocHigh, 0, stackSize );
	assert( StackBlock != NULL );

	TLSBlock = partition->Allocate( KAllocHigh, 0, 0x4000 ); // 16k enough?
	assert( TLSBlock != NULL );
}

KThread::~KThread()
{
	if( State != KThreadDead )
		this->Exit( 0 );

	SAFEDELETE( SpecialHandlers );

	SAFEDELETE( ExitWaiters );

	if( Context != NULL )
		Kernel->Cpu->ReleaseContextStorage( Context->ContextID );
	SAFEFREE( Context );
	SAFEFREE( Name );
}

void KThread::Start( uint argumentsLength, uint argumentsPointer )
{
	int registers[ 32 ];
	memset( registers, 0, sizeof( int ) * 32 );
	registers[ 4 ] = argumentsLength;
	registers[ 5 ] = argumentsPointer;
	registers[ 6 ] = NULL;
	registers[ 26 ] = TLSBlock->Address;		// TLS
	registers[ 28 ] = GlobalPointer;			// gp - set by cpu?
	registers[ 29 ] = StackBlock->Address;

	Context->ContextID = Kernel->Cpu->AllocateContextStorage( EntryAddress, registers );

	Kernel->Threads->Add( this );
	this->AddToSchedule();
}

void KThread::Exit( int code )
{
	State = KThreadDead;
	ExitCode = code;

	StackBlock->Partition->Free( StackBlock );
	StackBlock = NULL;

	TLSBlock->Partition->Free( TLSBlock );
	TLSBlock = NULL;

	while( ExitWaiters->GetCount() > 0 )
	{
		KThread* thread = ExitWaiters->Dequeue();
		thread->Wake();
	}

	// Remove from schedulable list and master thread list
	Kernel->Threads->Remove( this );
	this->RemoveFromSchedule();
}

void KThread::ChangePriority( int newPriority )
{
	Priority = newPriority;

	this->RemoveFromSchedule();
	this->AddToSchedule();
}

void KThread::Wake()
{
	State = KThreadReady;

	WakeupCount++;

	this->AddToSchedule();
}

void KThread::ReleaseWait()
{
	State = KThreadReady;

	ReleaseCount++;

	this->AddToSchedule();
}

void KThread::Suspend()
{
	Suspended = true;

	this->RemoveFromSchedule();
}

void KThread::Resume()
{
	Suspended = false;

	this->AddToSchedule();
}

void KThread::Sleep( bool canHandleCallbacks )
{
	State = KThreadWaiting;

	CanHandleCallbacks = canHandleCallbacks;

	WaitingOn = KThreadWaitSleep;

	this->RemoveFromSchedule();
}

void DelayCallback( Kernel* kernel, void* arg )
{
	KThread* thread = ( KThread* )arg;

	thread->State = KThreadReady;
	thread->ReturnValue = 0;

	thread->AddToSchedule();
	
	// We cannot schedule here - in a weird thread
	kernel->Cpu->BreakExecute( 1 );
}

void KThread::Delay( uint waitTimeUs, bool canHandleCallbacks )
{
	State = KThreadWaiting;

	assert( waitTimeUs > 0 );

	CanHandleCallbacks = canHandleCallbacks;

	WaitingOn = KThreadWaitDelay;
	WaitTimestamp = Kernel->GetTick();
	WaitTimeout = waitTimeUs * 10;	// us -> ticks

	// Install timer
	Kernel->AddTimer( DelayCallback, this, waitTimeUs / 1000, 0 );

	this->RemoveFromSchedule();
}

void JoinCallback( Kernel* kernel, void* arg )
{
	KThread* thread = ( KThread* )arg;

	thread->State = KThreadReady;
	thread->ReturnValue = -1;

	thread->AddToSchedule();
	
	// We cannot schedule here - in a weird thread
	kernel->Cpu->BreakExecute( 1 );
}

void KThread::Join( KThread* targetThread, uint timeoutUs, bool canHandleCallbacks )
{
	State = KThreadWaiting;

	CanHandleCallbacks = canHandleCallbacks;

	WaitingOn = KThreadWaitJoin;
	if( timeoutUs > 0 )
	{
		WaitTimestamp = Kernel->GetTick();
		WaitTimeout = timeoutUs * 10;	// us -> ticks

		// Install timer
		Kernel->AddTimer( JoinCallback, this, timeoutUs / 1000, 0 );
	}
	else
		WaitTimeout = 0;
	WaitThread = targetThread;

	targetThread->ExitWaiters->Enqueue( this );

	this->RemoveFromSchedule();
}

void WaitCallback( Kernel* kernel, void* arg )
{
	KThread* thread = ( KThread* )arg;

	thread->State = KThreadReady;
	thread->ReturnValue = -1;
	
	thread->AddToSchedule();

	// We cannot schedule here - in a weird thread
	kernel->Cpu->BreakExecute( 1 );
}

void KThread::Wait( KEvent* ev, uint waitEventMode, uint userValue, uint outAddress, uint timeoutUs, bool canHandleCallbacks )
{
	State = KThreadWaiting;

	CanHandleCallbacks = canHandleCallbacks;

	ev->WaitingThreads->Enqueue( this );

	WaitingOn = KThreadWaitEvent;
	if( timeoutUs > 0 )
	{
		WaitTimestamp = Kernel->GetTick();
		WaitTimeout = timeoutUs * 10;	// us -> ticks

		// Install timer
		Kernel->AddTimer( WaitCallback, this, timeoutUs / 1000, 0 );
	}
	else
		WaitTimeout = 0;
	WaitEvent = ev;
	WaitEventMode = waitEventMode;
	WaitArgument = userValue;
	WaitAddress = outAddress;

	this->RemoveFromSchedule();
}

void KThread::AddToSchedule()
{
	assert( Kernel->SchedulableThreads->Find( this ) == NULL );

	// Find the right place by walking the thread list and inserting before a thread of higher priority
	LLEntry<KThread*>* e = Kernel->SchedulableThreads->GetHead();
	if( e == NULL )
	{
		// Special case
		Kernel->SchedulableThreads->Enqueue( this );
	}
	else
	{
		do
		{
			if( e->Value->Priority >= this->Priority )
			{
				// Insert here (this will be before any threads with the same priority)
				Kernel->SchedulableThreads->InsertBefore( this, e );
				break;
			}

			if( e->Next == NULL )
			{
				// Append
				Kernel->SchedulableThreads->Enqueue( this );
				break;
			}

			e = e->Next;
		} while( e != NULL );
	}
}

void KThread::RemoveFromSchedule()
{
	LLEntry<KThread*>* entry = Kernel->SchedulableThreads->Find( this );
	if( entry != NULL )
		Kernel->SchedulableThreads->Remove( entry );
}

int KThread::AddSpecialHandler( SpecialHandlerFn function, int arg )
{
	KSpecialHandler* handler = new KSpecialHandler();
	handler->Function = function;
	handler->Argument = arg;
	return SpecialHandlers->Add( handler );
}

void KThread::SetSpecialHandler( int specialId )
{
	ActiveSpecialHandler = specialId;
}
