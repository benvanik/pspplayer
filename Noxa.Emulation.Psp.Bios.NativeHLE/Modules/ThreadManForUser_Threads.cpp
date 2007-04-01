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

// SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKernelThreadEventHandler handler, void *common); (/user/pspthreadman.h:1729)
int ThreadManForUser::sceKernelRegisterThreadEventHandler( IMemory^ memory, int name, int threadID, int mask, int handler, int common ){ return NISTUBRETURN; }

// int sceKernelReleaseThreadEventHandler(SceUID uid); (/user/pspthreadman.h:1738)
int ThreadManForUser::sceKernelReleaseThreadEventHandler( int uid ){ return NISTUBRETURN; }

// int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKernelThreadEventHandlerInfo *info); (/user/pspthreadman.h:1748)
int ThreadManForUser::sceKernelReferThreadEventHandlerStatus( IMemory^ memory, int uid, int info ){ return NISTUBRETURN; }

// SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option); (/user/pspthreadman.h:169)
int ThreadManForUser::sceKernelCreateThread( IMemory^ memory, int name, int entry, int initPriority, int stackSize, int attr, int option )
{
	String^ nameString = KernelHelpers::ReadString( memory, name );
	int id = _kernel->AllocateID();
	KernelThread^ thread = gcnew KernelThread( id, nameString, entry, initPriority, stackSize, attr );

	_kernel->AddHandle( thread );
	_kernel->CreateThread( thread );

	// Option unused?
	
	return id;
}

// int sceKernelDeleteThread(SceUID thid); (/user/pspthreadman.h:179)
int ThreadManForUser::sceKernelDeleteThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	Debug::Assert( _kernel->ActiveThread != thread );

	_kernel->DeleteThread( thread );
	_kernel->RemoveHandle( thread );

	return 0;
}

// int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp); (/user/pspthreadman.h:188)
int ThreadManForUser::sceKernelStartThread( int thid, int arglen, int argp )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	// TODO: Switch partition based on kernel/user status?
	thread->Start( _kernel->Partitions[ 1 ], arglen, argp );

	_kernel->ContextSwitch();

	return 0;
}

// void _sceKernelExitThread(); (/user/pspthreadman.h:1679)
void ThreadManForUser::_sceKernelExitThread(){}

// int sceKernelExitThread(int status); (/user/pspthreadman.h:195)
int ThreadManForUser::sceKernelExitThread( int status )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->Exit( status );

	if( _kernel->ActiveThread == thread )
		_kernel->ContextSwitch();

	return 0;
}

// int sceKernelExitDeleteThread(int status); (/user/pspthreadman.h:202)
int ThreadManForUser::sceKernelExitDeleteThread( int status )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->Exit( status );

	if( _kernel->ActiveThread == thread )
		_kernel->ContextSwitch();

	_kernel->DeleteThread( thread );
	_kernel->RemoveHandle( thread );

	return 0;
}

// int sceKernelTerminateThread(SceUID thid); (/user/pspthreadman.h:211)
int ThreadManForUser::sceKernelTerminateThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->Exit( 0 );

	if( _kernel->ActiveThread == thread )
		_kernel->ContextSwitch();

	return 0;
}

// int sceKernelTerminateDeleteThread(SceUID thid); (/user/pspthreadman.h:220)
int ThreadManForUser::sceKernelTerminateDeleteThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->Exit( 0 );

	if( _kernel->ActiveThread == thread )
		_kernel->ContextSwitch();

	_kernel->DeleteThread( thread );
	_kernel->RemoveHandle( thread );

	return 0;
}

// int sceKernelSuspendDispatchThread(); (/user/pspthreadman.h:227)
int ThreadManForUser::sceKernelSuspendDispatchThread(){ return NISTUBRETURN; }

// int sceKernelResumeDispatchThread(int state); (/user/pspthreadman.h:237)
int ThreadManForUser::sceKernelResumeDispatchThread( int state ){ return NISTUBRETURN; }

// int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr); (/user/pspthreadman.h:364)
int ThreadManForUser::sceKernelChangeCurrentThreadAttr( int unknown, int attr )
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	thread->Attributes = ( KernelThreadAttributes )attr;

	return 0;
}

// int sceKernelChangeThreadPriority(SceUID thid, int priority); (/user/pspthreadman.h:381)
int ThreadManForUser::sceKernelChangeThreadPriority( int thid, int priority )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->Priority = priority;

	return 0;
}

// int sceKernelRotateThreadReadyQueue(int priority); (/user/pspthreadman.h:390)
int ThreadManForUser::sceKernelRotateThreadReadyQueue( int priority ){ return NISTUBRETURN; }

// int sceKernelReleaseWaitThread(SceUID thid); (/user/pspthreadman.h:399)
int ThreadManForUser::sceKernelReleaseWaitThread( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	thread->State = KernelThreadState::Ready;
	thread->WaitID = 0;
	thread->ReleaseCount++;

	return 0;
}

// int sceKernelGetThreadId(); (/user/pspthreadman.h:406)
int ThreadManForUser::sceKernelGetThreadId()
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	return thread->ID;
}

// int sceKernelGetThreadCurrentPriority(); (/user/pspthreadman.h:413)
int ThreadManForUser::sceKernelGetThreadCurrentPriority()
{
	KernelThread^ thread = _kernel->ActiveThread;
	if( thread == nullptr )
		return -1;

	return thread->Priority;
}

// int sceKernelGetThreadExitStatus(SceUID thid); (/user/pspthreadman.h:422)
int ThreadManForUser::sceKernelGetThreadExitStatus( int thid )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	return thread->ExitCode;
}

// int sceKernelCheckThreadStack(); (/user/pspthreadman.h:429)
int ThreadManForUser::sceKernelCheckThreadStack(){ return NISTUBRETURN; }

// int sceKernelGetThreadStackFreeSize(SceUID thid); (/user/pspthreadman.h:439)
int ThreadManForUser::sceKernelGetThreadStackFreeSize( int thid ){ return NISTUBRETURN; }

// int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info); (/user/pspthreadman.h:458)
int ThreadManForUser::sceKernelReferThreadStatus( IMemory^ memory, int thid, int info )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	//typedef struct SceKernelThreadInfo {
	//    SceSize     size;
	//    char    	name[32];
	//    SceUInt     attr;
	//    int     	status;
	//    SceKernelThreadEntry    entry;
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
	//} SceKernelThreadInfo;

	SysClock runClocks;
	runClocks.QuadPart = thread->RunClocks;

	// Ensure 104 bytes
	if( memory->ReadWord( info ) != 104 )
	{
		Debug::WriteLine( String::Format( "ThreadManForUser::sceKernelReferThreadStatus: app passed struct with size {0}, expected 104",
			memory->ReadWord( info ) ) );
		return -1;
	}
	KernelHelpers::WriteString( memory, info + 4, thread->Name );
	memory->WriteWord( info + 36, 4, ( int )thread->Attributes );
	memory->WriteWord( info + 40, 4, ( int )thread->State );
	memory->WriteWord( info + 44, 4, thread->EntryAddress );
	memory->WriteWord( info + 48, 4, ( int )thread->StackBlock->Address );
	memory->WriteWord( info + 52, 4, ( int )thread->StackSize );
	memory->WriteWord( info + 56, 4, 0 ); // TODO: get thread gp pointer
	memory->WriteWord( info + 60, 4, thread->InitialPriority );
	memory->WriteWord( info + 64, 4, thread->Priority );
	memory->WriteWord( info + 68, 4, ( int )thread->WaitClass );
	memory->WriteWord( info + 72, 4, thread->WaitID );
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

// int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status); (/user/pspthreadman.h:468)
int ThreadManForUser::sceKernelReferThreadRunStatus( IMemory^ memory, int thid, int status )
{
	KernelThread^ thread = _kernel->FindThread( thid );
	if( thread == nullptr )
		return -1;

	//typedef struct SceKernelThreadRunStatus {
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
	//} SceKernelThreadRunStatus;

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
	memory->WriteWord( status + 12, 4, ( int )thread->WaitClass );
	memory->WriteWord( status + 16, 4, thread->WaitID );
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
	idleClocks.QuadPart = _kernel->IdleClocks;

	// Ensure 28 bytes
	if( memory->ReadWord( status ) != 28 )
	{
		Debug::WriteLine( String::Format( "ThreadManForUser: sceKernelReferSystemStatus app passed struct with size {0}, expected 28",
			memory->ReadWord( status ) ) );
		return -1;
	}
	memory->WriteWord( status +  4, 4, _kernel->Status );
	memory->WriteWord( status +  8, 4, ( int )idleClocks.LowPart );
	memory->WriteWord( status + 12, 4, ( int )idleClocks.HighPart );
	memory->WriteWord( status + 16, 4, ( int )_kernel->Statistics->LeaveIdleCount );
	memory->WriteWord( status + 20, 4, ( int )_kernel->Statistics->ThreadSwitchCount );
	memory->WriteWord( status + 24, 4, ( int )_kernel->Statistics->VfpuSwitchCount );

	// int
	return 0;
}

// int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount); (/user/pspthreadman.h:1075)
int ThreadManForUser::sceKernelGetThreadmanIdList( IMemory^ memory, int type, int readbuf, int readbufsize, int idcount ){ return NISTUBRETURN; }

// enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid); (/user/pspthreadman.h:1688)
int ThreadManForUser::sceKernelGetThreadmanIdType( int uid ){ return NISTUBRETURN; }
