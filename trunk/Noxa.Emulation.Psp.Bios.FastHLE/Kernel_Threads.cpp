// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "KernelStatistics.h"
#include "KernelThread.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

void Kernel::CreateThread( KernelThread^ thread )
{
	Debug::Assert( thread != nullptr );

	// Must be user for us, I think :)
	//Debug::Assert( ( thread->Attributes & KernelThreadAttributes::User ) == KernelThreadAttributes::User );

	Threads->Add( thread->ID, thread );
}

void Kernel::DeleteThread( KernelThread^ thread )
{
	Debug::Assert( thread != nullptr );

	if( Threads->ContainsKey( thread->ID ) == true )
		Threads->Remove( thread->ID );

	if( _activeThread == thread )
	{
		Debug::WriteLine( "Kernel::DeleteThread: called on the active thread!" );
		_activeThread = nullptr;

		//this->ContextSwitch();
	}
}

__inline KernelThread^ Kernel::FindThread( int id )
{
	KernelThread^ ret;
	if( Threads->TryGetValue( id, ret ) == true )
		return ret;
	else
		return nullptr;
}

void Kernel::WaitThreadOnEvent( KernelThread^ thread, KernelEvent^ ev, KernelThreadWaitTypes waitType, int bitMask, int outAddress )
{
	Debug::Assert( thread != nullptr );
	Debug::Assert( ev != nullptr );

	thread->Wait( ev, waitType, bitMask, outAddress );
	thread->CanHandleCallbacks = true;
	_threadsWaitingOnEvents->Add( thread );

	ev->WaitingThreads++;

	this->ContextSwitch();
}

void Kernel::SignalEvent( KernelEvent^ ev )
{
	bool needsSwitch = false;
	for( int n = 0; n < _threadsWaitingOnEvents->Count; n++ )
	{
		KernelThread^ thread = _threadsWaitingOnEvents[ n ];
		if( thread->WaitEvent == ev )
		{
			// Check for a match somehow
			bool matches = ev->Matches( thread->WaitID, thread->WaitType );
			if( matches == true )
			{
				thread->State = KernelThreadState::Ready;

				// Need to obey output param
				if( thread->OutAddress != 0x0 )
					_cpu->Memory->WriteWord( thread->OutAddress, 4, ev->BitMask );

				if( ( thread->WaitType & KernelThreadWaitTypes::ClearAll ) == KernelThreadWaitTypes::ClearAll )
					ev->BitMask = 0;
				else if( ( thread->WaitType & KernelThreadWaitTypes::ClearPattern ) == KernelThreadWaitTypes::ClearPattern )
					ev->BitMask = ev->BitMask & ~thread->WaitID;
				
				ev->WaitingThreads--;

				needsSwitch = true;

				// If we can't have multiple threads waiting, just return now
				if( ( ev->Attributes & KernelEventAttributes::WaitMultiple ) != KernelEventAttributes::WaitMultiple )
				{
					Debug::Assert( ev->WaitingThreads == 0 );
					break;
				}
			}
		}
	}
	if( needsSwitch == true )
		this->ContextSwitch();
}

void Kernel::SpawnDelayedThreadTimer( int64 targetTick )
{
	Debug::Assert( _delayedThreadTimer->Enabled == false );
	TimeSpan duration = DateTime( targetTick ) - DateTime::Now;
	_delayedThreadTimer->Interval = duration.TotalMilliseconds;
	_delayedThreadTimer->Start();
}

void Kernel::DelayedThreadTimerElapsed( Object^ sender, Timers::ElapsedEventArgs^ e )
{
	if( _delayedThreads->Count == 0 )
		return;

	Monitor::Enter( _delayedThreads );
	if( _delayedThreads->Count == 0 )
	{
		Monitor::Exit( _delayedThreads );
		return;
	}

	// We assume that if we have been called, the thread at the head is dead (hah)
	_delayedThreads[ 0 ]->State = KernelThreadState::Ready;
	_delayedThreads->RemoveAt( 0 );

	// If there are more, reset us - we will awaken any other expired waits here too
	int64 tick = DateTime::Now.Ticks;
	while( _delayedThreads->Count > 0 )
	{
		int64 nextTick = _delayedThreads[ 0 ]->WaitTimeout + _delayedThreads[ 0 ]->WaitTimeout;
		if( nextTick < tick )
		{
			_delayedThreads[ 0 ]->State = KernelThreadState::Ready;
			_delayedThreads->RemoveAt( 0 );
		}
		else
		{
			// Schedule us for later
			TimeSpan duration = DateTime( nextTick ) - DateTime( tick );
			_delayedThreadTimer->Interval = duration.TotalMilliseconds;
			_delayedThreadTimer->Start();
			break;
		}
	}

	Monitor::Exit( _delayedThreads );
}

int Kernel::ThreadPriorityComparer( KernelThread^ a, KernelThread^ b )
{
	if( a->Priority < b->Priority )
		return -1;
	else if( a->Priority > b->Priority )
		return 1;
	else
		return 0;
}

void Kernel::ContextSwitch()
{
	if( Threads->Count == 0 )
	{
		_activeThread = nullptr;
		return;
	}

	List<KernelThread^>^ threads = gcnew List<KernelThread^>( Threads->Count );
	threads->AddRange( Threads->Values );
	threads->Sort( gcnew Comparison<KernelThread^>( this, &Kernel::ThreadPriorityComparer ) );

	// Find the first thread (lowest priority) that is runnable
	// While doing this, also check to see if we can wake threads up
	KernelThread^ newThread = nullptr;
	for( int n = 0; n < threads->Count; n++ )
	{
		KernelThread^ thread = threads[ n ];
		if( ( thread->State == KernelThreadState::Waiting ) ||
			( thread->State == KernelThreadState::Suspended ) ||
			( thread->State == KernelThreadState::Stopped ) ||
			( thread->State == KernelThreadState::Killed ) )
		{
			if( thread->State == KernelThreadState::Waiting )
			{
				switch( thread->WaitClass )
				{
					case KernelThreadWait::Sleep:
						// Cannot un-wait automatically
						break;
					case KernelThreadWait::Delay:
						// Time - check to see if it has expired - if so, wake it up and use it
						{
							int64 remaining = ( DateTime::Now.Ticks - ( thread->WaitTimestamp + thread->WaitTimeout ) );
							if( remaining <= 0 )
							{
								// Wake up!
								thread->State = KernelThreadState::Ready;
								Monitor::Enter( _delayedThreads );
								if( _delayedThreads->Contains( thread ) == true )
									_delayedThreads->Remove( thread );
								Monitor::Exit( _delayedThreads );
							}
						}
						break;
					case KernelThreadWait::ThreadEnd:
						// Check thread
						KernelThread^ waitThread;
						if( Threads->TryGetValue( thread->WaitID, waitThread ) == true )
						{
							if( ( waitThread->State == KernelThreadState::Killed ) ||
								( waitThread->State == KernelThreadState::Stopped ) )
							{
								// Thread ended, wake
								thread->State = KernelThreadState::Ready;
							}
						}
						else
						{
							// Thread died, wake
							thread->State = KernelThreadState::Ready;
						}
						break;
					case KernelThreadWait::Event:
						// Waiting on WaitEvent for bitmask in WaitId
						break;
				}
				// If we didn't wake, skip
				if( thread->State == KernelThreadState::Waiting )
					continue;
			}
			else
				continue;
		}

		// So that we only get the first (lowest pri) one
		if( newThread == nullptr )
			newThread = thread;
	}

	if( newThread == nullptr )
	{
		// No threads to run
		_activeThread = nullptr;

		// Check to see if we have any delayed threads
		if( _delayedThreads->Count > 0 )
		{
			// Wait until one of those is awake
			int64 end = _delayedThreads[ 0 ]->WaitTimestamp + _delayedThreads[ 0 ]->WaitTimeout;
			int64 now = DateTime::Now.Ticks;
			if( end - now > 0 )
				Thread::Sleep( ( int )( ( ( end - now ) / TimeSpan::TicksPerSecond ) * 1000 ) );

			// Re-context switch to select it again
			this->ContextSwitch();
		}

		Debug::WriteLine( "Kernel: ran out of threads to run, ending sim" );
		_emu->Stop();
		return;
	}

	// Switch
	if( _activeThread != newThread )
	{
		if( _activeThread != nullptr )
		{
			// Save context
			if( _activeThread->Context == nullptr )
				_activeThread->Context = gcnew KernelThreadContext();
			_activeThread->Context->CoreState = _core0->Context;

			// Thread may have stopped/gone to sleep
			if( _activeThread->State == KernelThreadState::Running )
				_activeThread->State = KernelThreadState::Ready;
		}

		_activeThread = newThread;

		Debug::WriteLine( String::Format( "Kernel: context switch, new pc={0:X8}", _activeThread->EntryAddress ) );

		if( _activeThread->Context != nullptr )
		{
			// Restore context
			_core0->Context = _activeThread->Context->CoreState;
		}
		else
		{
			// Start of thread
			// Set entry address and argc / argv
			_core0->ProgramCounter = _activeThread->EntryAddress; // PC is weird - interpreted needs -4!
			_core0->SetGeneralRegister( 29, ( int )_activeThread->StackBlock->Address );
			_core0->SetGeneralRegister( 4, _activeThread->ArgumentsLength );
			_core0->SetGeneralRegister( 5, _activeThread->ArgumentsPointer );
		}

		_activeThread->State = KernelThreadState::Running;

		Statistics->ThreadSwitchCount++;
	}
}
