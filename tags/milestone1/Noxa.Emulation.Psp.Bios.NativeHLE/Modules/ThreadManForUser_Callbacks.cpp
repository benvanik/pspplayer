// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "ThreadManForUser.h"
#include "KernelHelpers.h"
#include "KPartition.h"
#include "KThread.h"
#include "KCallback.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg); (/user/pspthreadman.h:985)
int ThreadManForUser::sceKernelCreateCallback( IMemory^ memory, int name, int func, int arg )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	char buffer[ 64 ];
	KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )64 );

	KCallback* cb = new KCallback( buffer, thread, func, arg );
	
	_kernel->Handles->Add( cb );

	return cb->UID;
}

// int sceKernelDeleteCallback(SceUID cb); (/user/pspthreadman.h:1005)
int ThreadManForUser::sceKernelDeleteCallback( int cbid )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	_kernel->Handles->Remove( cb );

	return 0;
}

// int sceKernelNotifyCallback(SceUID cb, int arg2); (/user/pspthreadman.h:1015)
int ThreadManForUser::sceKernelNotifyCallback( int cbid, int arg2 )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	cb->NotifyCount++;
	cb->NotifyArguments = arg2;

	// Need the CPU API for this!
	assert( _kernel->Cpu != NULL );
	_kernel->Cpu->MarshalCallback( cb->Thread->Context->ContextID, cb->Address, cb->NotifyArguments, NULL );

	// Return is set by marshaller
	return 0;
}

// int sceKernelCancelCallback(SceUID cb); (/user/pspthreadman.h:1024)
int ThreadManForUser::sceKernelCancelCallback( int cbid )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	// Don't do anything, I think
	
	return 0;
}

// int sceKernelGetCallbackCount(SceUID cb); (/user/pspthreadman.h:1033)
int ThreadManForUser::sceKernelGetCallbackCount( int cbid )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	return cb->NotifyCount;
}

// int sceKernelCheckCallback(); (/user/pspthreadman.h:1040)
int ThreadManForUser::sceKernelCheckCallback()
{
	//return NISTUBRETURN;
	return 0;
}

// int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status); (/user/pspthreadman.h:996)
int ThreadManForUser::sceKernelReferCallbackStatus( IMemory^ memory, int cbid, int status )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	if( memory->ReadWord( status ) == 56 )
	{
		KernelHelpers::WriteString( MSI( memory ), ( const int )( status + 4 ), cb->Name );
		memory->WriteWord( status + 36, 4, cb->Thread->UID );
		memory->WriteWord( status + 40, 4, cb->Address );
		memory->WriteWord( status + 44, 4, cb->CommonAddress );
		memory->WriteWord( status + 48, 4, cb->NotifyCount );
		memory->WriteWord( status + 52, 4, cb->NotifyArguments );

		return 0;
	}
	else
	{
		Debug::WriteLine( String::Format( "sceKernelReferCallbackStatus: expected SceKernelCallbackInfo of size 56, not {0}", memory->ReadWord( status ) ) );
		return -1;
	}
}
