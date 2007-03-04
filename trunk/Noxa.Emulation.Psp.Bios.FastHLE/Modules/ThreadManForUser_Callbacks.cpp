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

// int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg); (/user/pspthreadman.h:985)
int ThreadManForUser::sceKernelCreateCallback( IMemory^ memory, int name, int func, int arg )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	KernelCallback^ callback = gcnew KernelCallback( _kernel->AllocateID() );
	callback->Name = KernelHelpers::ReadString( memory, name );
	callback->FunctionAddress = func;
	callback->CommonAddress = arg;
	callback->Thread = thread;

	thread->Callbacks->Add( callback );
	_kernel->AddHandle( callback );

	return callback->ID;
}

// int sceKernelDeleteCallback(SceUID cb); (/user/pspthreadman.h:1005)
int ThreadManForUser::sceKernelDeleteCallback( int cb )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cb );
	if( callback == nullptr )
		return -1;

	callback->Thread->Callbacks->Remove( callback );
	_kernel->RemoveHandle( callback );

	return 0;
}

// int sceKernelNotifyCallback(SceUID cb, int arg2); (/user/pspthreadman.h:1015)
int ThreadManForUser::sceKernelNotifyCallback( int cb, int arg2 )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cb );
	if( callback == nullptr )
		return -1;

	callback->NotifyCount++;
	callback->NotifyArguments = arg2;

	return NISTUBRETURN;
}

// int sceKernelCancelCallback(SceUID cb); (/user/pspthreadman.h:1024)
int ThreadManForUser::sceKernelCancelCallback( int cb )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cb );
	if( callback == nullptr )
		return -1;

	// Don't do anything, I think
	
	return 0;
}

// int sceKernelGetCallbackCount(SceUID cb); (/user/pspthreadman.h:1033)
int ThreadManForUser::sceKernelGetCallbackCount( int cb )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cb );
	if( callback == nullptr )
		return -1;

	return callback->NotifyCount;
}

// int sceKernelCheckCallback(); (/user/pspthreadman.h:1040)
int ThreadManForUser::sceKernelCheckCallback()
{
	return NISTUBRETURN;
}

// int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status); (/user/pspthreadman.h:996)
int ThreadManForUser::sceKernelReferCallbackStatus( IMemory^ memory, int cb, int status )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cb );
	if( callback == nullptr )
		return -1;

	if( memory->ReadWord( status ) == 56 )
	{
		KernelHelpers::WriteString( memory, status + 4, callback->Name );
		memory->WriteWord( status + 36, 4, callback->Thread->ID );
		memory->WriteWord( status + 40, 4, callback->FunctionAddress );
		memory->WriteWord( status + 44, 4, callback->CommonAddress );
		memory->WriteWord( status + 48, 4, callback->NotifyCount );
		memory->WriteWord( status + 52, 4, callback->NotifyArguments );

		return 0;
	}
	else
	{
		Debug::WriteLine( String::Format( "sceKernelReferCallbackStatus: expected SceKernelCallbackInfo of size 56, not {0}", memory->ReadWord( status ) ) );
		return -1;
	}
}
