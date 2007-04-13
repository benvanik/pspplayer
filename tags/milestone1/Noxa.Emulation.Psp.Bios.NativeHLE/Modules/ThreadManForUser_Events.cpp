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
#include "KEvent.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt); (/user/pspthreadman.h:645)
int ThreadManForUser::sceKernelCreateEventFlag( IMemory^ memory, int name, int attr, int bits, int opt )
{
	byte buffer[ 32 ];
	KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )32 );

	KEvent* ev = new KEvent( ( char* )buffer, attr, bits );

	// options unused

	_kernel->Handles->Add( ev );

	return ev->UID;
}

// int sceKernelDeleteEventFlag(int evid); (/user/pspthreadman.h:709)
int ThreadManForUser::sceKernelDeleteEventFlag( int evid )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	// What if someone is waiting on this?
	assert( ev->WaitingThreads->GetCount() == 0 );

	_kernel->Handles->Remove( ev );

	SAFEDELETE( ev );

	return 0;
}

// int sceKernelSetEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:655)
int ThreadManForUser::sceKernelSetEventFlag( int evid, int bits )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	ev->Value = bits;
	if( ev->Signal( _kernel ) == true )
	{
		// Our event woke something!
		_kernel->Schedule();
	}

	return 0;
}

// int sceKernelClearEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:665)
int ThreadManForUser::sceKernelClearEventFlag( int evid, int bits )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	ev->Value = ev->Value & ~bits;

	return 0;
}

// int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:688)
int ThreadManForUser::sceKernelWaitEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	// TODO: event timeouts
	assert( timeout == 0 );

	// We may already be set!
	bool matches = ev->Matches( bits, wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
		{
			int* poutBits = ( int* )MSI( memory )->Translate( outBits );
			*poutBits = ev->Value;
		}

		if( ( wait & KThreadWaitClearAll ) != 0 )
			ev->Value = 0;
		else if( ( wait & KThreadWaitClearPattern ) != 0 )
			ev->Value = ev->Value & ~bits;

		return 0;
	}
	else
	{
		KThread* thread = _kernel->ActiveThread;
		assert( thread != NULL );
		thread->Wait( ev, wait, bits, outBits, timeout, false );
	}

	return 0;
}

// int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:700)
int ThreadManForUser::sceKernelWaitEventFlagCB( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	// TODO: event timeouts
	assert( timeout == 0 );

	// We may already be set!
	bool matches = ev->Matches( bits, wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
		{
			int* poutBits = ( int* )MSI( memory )->Translate( outBits );
			*poutBits = ev->Value;
		}

		if( ( wait & KThreadWaitClearAll ) != 0 )
			ev->Value = 0;
		else if( ( wait & KThreadWaitClearPattern ) != 0 )
			ev->Value = ev->Value & ~bits;

		return 0;
	}
	else
	{
		KThread* thread = _kernel->ActiveThread;
		assert( thread != NULL );
		thread->Wait( ev, wait, bits, outBits, timeout, true );
	}

	return 0;
}

// int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits); (/user/pspthreadman.h:676)
int ThreadManForUser::sceKernelPollEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits )
{
	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	bool matches = ev->Matches( bits, wait );
	if( matches == true )
	{
		if( outBits != 0x0 )
		{
			int* poutBits = ( int* )MSI( memory )->Translate( outBits );
			*poutBits = ev->Value;
		}

		// Don't clear? just Polling
		if( ( wait & KThreadWaitClearAll ) != 0 )
			ev->Value = 0;
		else if( ( wait & KThreadWaitClearPattern ) != 0 )
			ev->Value = ev->Value & ~bits;
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

	KEvent* ev = ( KEvent* )_kernel->Handles->Lookup( evid );
	if( ev == NULL )
		return -1;

	byte* p = ( byte* )MSI( memory )->Translate( status );

	int size = *( ( int* )p );
	if( size == 52 )
	{
		KernelHelpers::WriteString( MSI( memory ), status + 4, ev->Name );
		*( ( int* )( p + 36 ) ) = ( int )ev->Attributes;
		*( ( int* )( p + 40 ) ) = ev->InitialValue;
		*( ( int* )( p + 44 ) ) = ev->Value;
		*( ( int* )( p + 48 ) ) = ev->WaitingThreads->GetCount();
	}
	else
	{
		Debug::WriteLine( String::Format( "sceKernelReferEventFlagStatus: app passed in SceKernelEventFlagInfo of size {0}; expected size 52.", size ) );
		return -1;
	}

	return 0;
}
