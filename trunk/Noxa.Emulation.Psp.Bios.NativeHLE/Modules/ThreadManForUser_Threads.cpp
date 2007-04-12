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

// SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKThreadEventHandler handler, void *common); (/user/pspthreadman.h:1729)
int ThreadManForUser::sceKernelRegisterThreadEventHandler( IMemory^ memory, int name, int threadID, int mask, int handler, int common ){ return NISTUBRETURN; }

// int sceKernelReleaseThreadEventHandler(SceUID uid); (/user/pspthreadman.h:1738)
int ThreadManForUser::sceKernelReleaseThreadEventHandler( int uid ){ return NISTUBRETURN; }

// int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKThreadEventHandlerInfo *info); (/user/pspthreadman.h:1748)
int ThreadManForUser::sceKernelReferThreadEventHandlerStatus( IMemory^ memory, int uid, int info ){ return NISTUBRETURN; }

// SceUID sceKernelCreateThread(const char *name, SceKThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKThreadOptParam *option); (/user/pspthreadman.h:169)
int ThreadManForUser::sceKernelCreateThread( IMemory^ memory, int name, int entry, int initPriority, int stackSize, int attr, int option )
{
	char buffer[ 64 ];
	int nameLength = KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )64 );

	KThread* thread = new KThread( _kernel, _kernel->Partitions[ 2 ], buffer, entry, initPriority, ( KThreadAttributes )attr, stackSize );

	_kernel->Handles->Add( thread );

	// Option unused?
	assert( option == NULL );

	Debug::WriteLine( String::Format( "sceKernelCreateThread: creating thread {0:X8} {1}", thread->UID, gcnew String( thread->Name ) ) );
	
	return thread->UID;
}

// int sceKernelDeleteThread(SceUID thid); (/user/pspthreadman.h:179)
int ThreadManForUser::sceKernelDeleteThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	assert( _kernel->ActiveThread != thread );

	Debug::WriteLine( String::Format( "sceKernelDeleteThread: deleting thread {0:X8} {1}", thread->UID, gcnew String( thread->Name ) ) );

	_kernel->Handles->Remove( thread );

	SAFEDELETE( thread );

	return 0;
}

// int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp); (/user/pspthreadman.h:188)
int ThreadManForUser::sceKernelStartThread( int thid, int arglen, int argp )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	Debug::WriteLine( String::Format( "sceKernelStartThread: starting thread {0:X8} {1}", thread->UID, gcnew String( thread->Name ) ) );

	// TODO: Switch partition based on kernel/user status?
	thread->Start( arglen, argp );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelExitThread(int status); (/user/pspthreadman.h:195)
int ThreadManForUser::sceKernelExitThread( int status )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Exit( status );

	if( _kernel->ActiveThread == thread )
		_kernel->Schedule();

	return 0;
}

// int sceKernelExitDeleteThread(int status); (/user/pspthreadman.h:202)
int ThreadManForUser::sceKernelExitDeleteThread( int status )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Exit( status );

	if( _kernel->ActiveThread == thread )
		_kernel->Schedule();

	_kernel->Handles->Remove( thread );

	SAFEDELETE( thread );

	return 0;
}

// int sceKernelTerminateThread(SceUID thid); (/user/pspthreadman.h:211)
int ThreadManForUser::sceKernelTerminateThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->Exit( 0 );

	if( _kernel->ActiveThread == thread )
		_kernel->Schedule();

	return 0;
}

// int sceKernelTerminateDeleteThread(SceUID thid); (/user/pspthreadman.h:220)
int ThreadManForUser::sceKernelTerminateDeleteThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->Exit( 0 );

	if( _kernel->ActiveThread == thread )
		_kernel->Schedule();

	_kernel->Handles->Remove( thread );

	SAFEDELETE( thread );

	return 0;
}

// int sceKernelSuspendDispatchThread(); (/user/pspthreadman.h:227)
int ThreadManForUser::sceKernelSuspendDispatchThread(){ return NISTUBRETURN; }

// int sceKernelResumeDispatchThread(int state); (/user/pspthreadman.h:237)
int ThreadManForUser::sceKernelResumeDispatchThread( int state ){ return NISTUBRETURN; }

// int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr); (/user/pspthreadman.h:364)
int ThreadManForUser::sceKernelChangeCurrentThreadAttr( int unknown, int attr )
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	thread->Attributes = ( KThreadAttributes )attr;

	return 0;
}

// int sceKernelChangeThreadPriority(SceUID thid, int priority); (/user/pspthreadman.h:381)
int ThreadManForUser::sceKernelChangeThreadPriority( int thid, int priority )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	if( thread->Priority != priority )
		thread->ChangePriority( priority );

	return 0;
}

// int sceKernelRotateThreadReadyQueue(int priority); (/user/pspthreadman.h:390)
int ThreadManForUser::sceKernelRotateThreadReadyQueue( int priority ){ return NISTUBRETURN; }

// int sceKernelReleaseWaitThread(SceUID thid); (/user/pspthreadman.h:399)
int ThreadManForUser::sceKernelReleaseWaitThread( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	thread->ReleaseWait();

	// Is tihs right?
	assert( false );
	if( _kernel->Schedule() == true )
	{
	}

	return 0;
}

// int sceKernelGetThreadId(); (/user/pspthreadman.h:406)
int ThreadManForUser::sceKernelGetThreadId()
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	return thread->UID;
}

// int sceKernelGetThreadCurrentPriority(); (/user/pspthreadman.h:413)
int ThreadManForUser::sceKernelGetThreadCurrentPriority()
{
	KThread* thread = _kernel->ActiveThread;
	if( thread == NULL )
		return -1;

	return thread->Priority;
}

// int sceKernelGetThreadExitStatus(SceUID thid); (/user/pspthreadman.h:422)
int ThreadManForUser::sceKernelGetThreadExitStatus( int thid )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	return thread->ExitCode;
}

// int sceKernelCheckThreadStack(); (/user/pspthreadman.h:429)
int ThreadManForUser::sceKernelCheckThreadStack(){ return NISTUBRETURN; }

// int sceKernelGetThreadStackFreeSize(SceUID thid); (/user/pspthreadman.h:439)
int ThreadManForUser::sceKernelGetThreadStackFreeSize( int thid ){ return NISTUBRETURN; }

// int sceKernelReferThreadStatus(SceUID thid, SceKThreadInfo *info); (/user/pspthreadman.h:458)
int ThreadManForUser::sceKernelReferThreadStatus( IMemory^ memory, int thid, int info )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	//typedef struct SceKThreadInfo {
	//    SceSize     size;
	//    char    	name[32];
	//    SceUInt     attr;
	//    int     	status;
	//    SceKThreadEntry    entry;
	//    void *  	stack;
	//    int     	stackSize;
	//    void *  	gpReg;
	//    int     	initPriority;
	//    int     	currentPriority;
	//    int     	waitType;
	//    SceUID  	waitId;
	//    int     	wakeupCount;
	//    int     	exitStatus;
	//    SceKernelSysClock   runClocks;
	//    SceUInt     intrPreemptCount;
	//    SceUInt     threadPreemptCount;
	//    SceUInt     releaseCount;
	//} SceKThreadInfo;

	SysClock runClocks;
	runClocks.QuadPart = thread->RunClocks;

	// Ensure 104 bytes
	if( memory->ReadWord( info ) != 104 )
	{
		Debug::WriteLine( String::Format( "ThreadManForUser::sceKernelReferThreadStatus: app passed struct with size {0}, expected 104",
			memory->ReadWord( info ) ) );
		return -1;
	}
	KernelHelpers::WriteString( MSI( memory ), ( const int )( info + 4 ), ( const char* )thread->Name );
	memory->WriteWord( info + 36, 4, ( int )thread->Attributes );
	memory->WriteWord( info + 40, 4, ( int )thread->State );
	memory->WriteWord( info + 44, 4, thread->EntryAddress );
	memory->WriteWord( info + 48, 4, ( int )thread->StackBlock->Address );
	memory->WriteWord( info + 52, 4, ( int )thread->StackBlock->Size );
	memory->WriteWord( info + 56, 4, thread->GlobalPointer );
	memory->WriteWord( info + 60, 4, thread->InitialPriority );
	memory->WriteWord( info + 64, 4, thread->Priority );
	memory->WriteWord( info + 68, 4, ( int )thread->WaitingOn );
	if( thread->WaitingOn == KThreadWaitEvent )
		memory->WriteWord( info + 72, 4, thread->WaitEvent->UID );
	else if( thread->WaitingOn == KThreadWaitJoin )
		memory->WriteWord( info + 72, 4, thread->WaitThread->UID );
	else
		memory->WriteWord( info + 72, 4, 0 );
	memory->WriteWord( info + 76, 4, ( int )thread->WakeupCount );
	memory->WriteWord( info + 80, 4, thread->ExitCode );
	memory->WriteWord( info + 84, 4, ( int )runClocks.LowPart );
	memory->WriteWord( info + 88, 4, ( int )runClocks.HighPart );
	memory->WriteWord( info + 92, 4, ( int )thread->InterruptPreemptionCount );
	memory->WriteWord( info + 96, 4, ( int )thread->ThreadPreemptionCount );
	memory->WriteWord( info + 100, 4, ( int )thread->ReleaseCount );

	// int
	return 0;
}

// int sceKernelReferThreadRunStatus(SceUID thid, SceKThreadRunStatus *status); (/user/pspthreadman.h:468)
int ThreadManForUser::sceKernelReferThreadRunStatus( IMemory^ memory, int thid, int status )
{
	KThread* thread = ( KThread* )_kernel->Handles->Lookup( thid );
	if( thread == NULL )
		return -1;

	//typedef struct SceKThreadRunStatus {
	//    SceSize 	size;
	//    int 		status;
	//    int 		currentPriority;
	//    int 		waitType;
	//    int 		waitId;
	//    int 		wakeupCount;
	//    SceKernelSysClock runClocks;
	//    SceUInt 	intrPreemptCount;
	//    SceUInt 	threadPreemptCount;
	//    SceUInt 	releaseCount;
	//} SceKThreadRunStatus;

	SysClock runClocks;
	runClocks.QuadPart = thread->RunClocks;

	// Ensure 44 bytes
	if( memory->ReadWord( status ) != 44 )
	{
		Debug::WriteLine( String::Format( "ThreadManForUser: sceKernelReferThreadRunStatus app passed struct with size {0}, expected 44",
			memory->ReadWord( status ) ) );
		return -1;
	}
	memory->WriteWord( status +  4, 4, ( int )thread->State );
	memory->WriteWord( status +  8, 4, thread->Priority );
	memory->WriteWord( status + 12, 4, ( int )thread->WaitingOn );
	if( thread->WaitingOn == KThreadWaitEvent )
		memory->WriteWord( status + 16, 4, thread->WaitEvent->UID );
	else if( thread->WaitingOn == KThreadWaitJoin )
		memory->WriteWord( status + 16, 4, thread->WaitThread->UID );
	else
		memory->WriteWord( status + 16, 4, 0 );
	memory->WriteWord( status + 20, 4, ( int )thread->WakeupCount );
	memory->WriteWord( status + 24, 4, ( int )runClocks.LowPart );
	memory->WriteWord( status + 28, 4, ( int )runClocks.HighPart );
	memory->WriteWord( status + 32, 4, ( int )thread->InterruptPreemptionCount );
	memory->WriteWord( status + 36, 4, ( int )thread->ThreadPreemptionCount );
	memory->WriteWord( status + 40, 4, ( int )thread->ReleaseCount );

	// int
	return 0;
}

// int sceKernelReferSystemStatus(SceKernelSystemStatus *status); (/user/pspthreadman.h:1100)
int ThreadManForUser::sceKernelReferSystemStatus( IMemory^ memory, int status )
{
	//typedef struct SceKernelSystemStatus {
	//    SceSize 	size;
	//    SceUInt 	status;
	//    SceKernelSysClock 	idleClocks;
	//    SceUInt 	comesOutOfIdleCount;
	//    SceUInt 	threadSwitchCount;
	//    SceUInt 	vfpuSwitchCount;
	//} SceKernelSystemStatus;

	SysClock idleClocks;
	idleClocks.QuadPart = ( int64 )0;// _kernel->IdleClocks;

	// Ensure 28 bytes
	if( memory->ReadWord( status ) != 28 )
	{
		Debug::WriteLine( String::Format( "ThreadManForUser: sceKernelReferSystemStatus app passed struct with size {0}, expected 28",
			memory->ReadWord( status ) ) );
		return -1;
	}
	memory->WriteWord( status +  4, 4, 1 ); // ?????
	memory->WriteWord( status +  8, 4, ( int )idleClocks.LowPart );
	memory->WriteWord( status + 12, 4, ( int )idleClocks.HighPart );
	//memory->WriteWord( status + 16, 4, ( int )_kernel->Statistics->LeaveIdleCount );
	//memory->WriteWord( status + 20, 4, ( int )_kernel->Statistics->ThreadSwitchCount );
	//memory->WriteWord( status + 24, 4, ( int )_kernel->Statistics->VfpuSwitchCount );
	memory->WriteWord( status + 16, 4, 0 );
	memory->WriteWord( status + 20, 4, 0 );
	memory->WriteWord( status + 24, 4, 0 );

	// int
	return 0;
}

// int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount); (/user/pspthreadman.h:1075)
int ThreadManForUser::sceKernelGetThreadmanIdList( IMemory^ memory, int type, int readbuf, int readbufsize, int idcount ){ return NISTUBRETURN; }

// enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid); (/user/pspthreadman.h:1688)
int ThreadManForUser::sceKernelGetThreadmanIdType( int uid ){ return NISTUBRETURN; }
