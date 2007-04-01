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

// SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt); (/user/pspthreadman.h:645)
int ThreadManForUser::sceKernelCreateEventFlag( IMemory^ memory, int name, int attr, int bits, int opt )
{
	KernelEvent^ ev = gcnew KernelEvent( _kernel->AllocateID() );
	ev->Name = KernelHelpers::ReadString( memory, name );
	ev->Attributes = ( KernelEventAttributes )attr;
	ev->InitialBitMask = bits;
	ev->BitMask = bits;

	// options unused

	_kernel->AddHandle( ev );

	return ev->ID;
}

// int sceKernelDeleteEventFlag(int evid); (/user/pspthreadman.h:709)
int ThreadManForUser::sceKernelDeleteEventFlag( int evid )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	// What if someone is waiting on this?
	Debug::Assert( ev->WaitingThreads == 0 );

	_kernel->RemoveHandle( ev );

	return 0;
}

// int sceKernelSetEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:655)
int ThreadManForUser::sceKernelSetEventFlag( int evid, int bits )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	ev->BitMask = bits;
	_kernel->SignalEvent( ev );

	return 0;
}

// int sceKernelClearEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:665)
int ThreadManForUser::sceKernelClearEventFlag( int evid, int bits )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	ev->BitMask = ev->BitMask & ~bits;

	return 0;
}

// int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:688)
int ThreadManForUser::sceKernelWaitEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	// Timeout not handled
	Debug::Assert( timeout == 0 );

	// We may already be set!
	bool matches = ev->Matches( bits, ( KernelThreadWaitTypes )wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
			memory->WriteWord( outBits, 4, ev->BitMask );

		if( ( wait & ( int )KernelThreadWaitTypes::ClearAll ) != 0 )
			ev->BitMask = 0;
		else if( ( wait & ( int )KernelThreadWaitTypes::ClearPattern ) != 0 )
			ev->BitMask = ev->BitMask & ~bits;

		return 0;
	}
	else
	{
		KernelThread^ thread = _kernel->ActiveThread;
		_kernel->WaitThreadOnEvent( thread, ev, ( KernelThreadWaitTypes )wait, bits, outBits );
	}

	return 0;
}

// int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:700)
int ThreadManForUser::sceKernelWaitEventFlagCB( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	// Timeout not handled
	Debug::Assert( timeout == 0 );

	// We may already be set!
	bool matches = ev->Matches( bits, ( KernelThreadWaitTypes )wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
			memory->WriteWord( outBits, 4, ev->BitMask );

		if( ( wait & ( int )KernelThreadWaitTypes::ClearAll ) != 0 )
			ev->BitMask = 0;
		else if( ( wait & ( int )KernelThreadWaitTypes::ClearPattern ) != 0 )
			ev->BitMask = ev->BitMask & ~bits;

		return 0;
	}
	else
	{
		KernelThread^ thread = _kernel->ActiveThread;
		thread->CanHandleCallbacks = true;
		_kernel->WaitThreadOnEvent( thread, ev, ( KernelThreadWaitTypes )wait, bits, outBits );
	}

	return 0;
}

// int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits); (/user/pspthreadman.h:676)
int ThreadManForUser::sceKernelPollEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits )
{
	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	bool matches = ev->Matches( bits, ( KernelThreadWaitTypes )wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
			memory->WriteWord( outBits, 4, ev->BitMask );

		if( ( wait & ( int )KernelThreadWaitTypes::ClearAll ) != 0 )
			ev->BitMask = 0;
		else if( ( wait & ( int )KernelThreadWaitTypes::ClearPattern ) != 0 )
			ev->BitMask = ev->BitMask & ~bits;
	}

	// What is the proper return here?
	return 0;
}

// int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status); (/user/pspthreadman.h:719)
int ThreadManForUser::sceKernelReferEventFlagStatus( IMemory^ memory, int evid, int status )
{
	// SceSize  size 
	// char		name [32] 
	// SceUInt  attr 
	// SceUInt  initPattern 
	// SceUInt  currentPattern 
	// int		numWaitThreads 

	KernelEvent^ ev = ( KernelEvent^ )_kernel->FindHandle( evid );
	if( ev == nullptr )
		return -1;

	if( memory->ReadWord( status ) == 52 )
	{
		KernelHelpers::WriteString( memory, status + 4, ev->Name );
		memory->WriteWord( status + 36, 4, ( int )ev->Attributes );
		memory->WriteWord( status + 40, 4, ev->InitialBitMask );
		memory->WriteWord( status + 44, 4, ev->BitMask );
		memory->WriteWord( status + 48, 4, ev->WaitingThreads );
	}
	else
	{
		Debug::WriteLine( String::Format( "sceKernelReferEventFlagStatus: app passed in SceKernelEventFlagInfo of size {0}; expected size 52.", memory->ReadWord( status ) ) );
		return -1;
	}

	return 0;
}
