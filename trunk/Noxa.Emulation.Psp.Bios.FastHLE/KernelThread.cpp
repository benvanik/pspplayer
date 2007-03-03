// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelThread.h"
#include "KernelEvent.h"
#include "KernelPartition.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;

KernelThread::KernelThread( int id, String^ name, int entryAddress, int priority, uint stackSize, int attributes )
	: KernelHandle( KernelHandleType::Thread, id )
{
	Name = name;
	EntryAddress = entryAddress;
	Priority = priority;
	InitialPriority = priority;
	StackSize = stackSize;
	Attributes = ( KernelThreadAttributes )attributes;
	
	RunClocks = 0;

	Callbacks = gcnew List<KernelCallback^>();

	State = KernelThreadState::Stopped;
}

void KernelThread::Start( KernelPartition^ partition, int argumentsLength, int argumentsPointer )
{
	StackBlock = partition->Allocate( KernelAllocationType::High, 0, StackSize );

	State = KernelThreadState::Running;
	ArgumentsLength = argumentsLength;
	ArgumentsPointer = argumentsPointer;
}

void KernelThread::Exit( int code )
{
	State = KernelThreadState::Killed;
	ExitCode = code;
	StackBlock->Partition->Free( StackBlock );
	StackBlock = nullptr;
}

void KernelThread::Wait( KernelEvent^ ev, int bitMask, int outAddress )
{
	State = KernelThreadState::Waiting;
	WaitType = KernelThreadWait::Event;
	WaitEvent = ev;
	WaitID = bitMask;
	OutAddress = outAddress;
}
