// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "ThreadManForUser.h"
#include "Kernel.h"
#include "KernelPool.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// - VPL ------------------------------------------------------------------------------------------------

// SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt); (/user/pspthreadman.h:1256)
int ThreadManForUser::sceKernelCreateVpl( IMemory^ memory, int name, int part, int attr, int size, int opt )
{ return NISTUBRETURN; }

// int sceKernelDeleteVpl(SceUID uid); (/user/pspthreadman.h:1265)
int ThreadManForUser::sceKernelDeleteVpl( int uid )
{ return NISTUBRETURN; }

// int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1277)
int ThreadManForUser::sceKernelAllocateVpl( IMemory^ memory, int uid, int size, int data, int timeout )
{ return NISTUBRETURN; }

// int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1289)
int ThreadManForUser::sceKernelAllocateVplCB( IMemory^ memory, int uid, int size, int data, int timeout )
{ return NISTUBRETURN; }

// int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data); (/user/pspthreadman.h:1300)
int ThreadManForUser::sceKernelTryAllocateVpl( IMemory^ memory, int uid, int size, int data )
{ return NISTUBRETURN; }

// int sceKernelFreeVpl(SceUID uid, void *data); (/user/pspthreadman.h:1310)
int ThreadManForUser::sceKernelFreeVpl( IMemory^ memory, int uid, int data )
{ return NISTUBRETURN; }

// int sceKernelCancelVpl(SceUID uid, int *pnum); (/user/pspthreadman.h:1320)
int ThreadManForUser::sceKernelCancelVpl( IMemory^ memory, int uid, int pnum )
{ return NISTUBRETURN; }

// int sceKernelReferVplStatus(SceUID uid, SceKernelVplInfo *info); (/user/pspthreadman.h:1340)
int ThreadManForUser::sceKernelReferVplStatus( IMemory^ memory, int uid, int info )
{ return NISTUBRETURN; }

// - FPL ------------------------------------------------------------------------------------------------

// int sceKernelCreateFpl(const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt); (/user/pspthreadman.h:1360)
int ThreadManForUser::sceKernelCreateFpl( IMemory^ memory, int name, int part, int attr, int size, int blocks, int opt )
{
	KernelPartition^ partition = _kernel->Partitions[ part ];
	Debug::Assert( partition != nullptr );
	if( partition == nullptr )
		return -1;

	String^ nameString = KernelHelpers::ReadString( memory, name );
	KernelPool^ pool = gcnew KernelPool( _kernel, _kernel->AllocateID(), nameString, partition, attr, size, blocks );
	_kernel->AddHandle( pool );

	return pool->ID;
}

// int sceKernelDeleteFpl(SceUID uid); (/user/pspthreadman.h:1369)
int ThreadManForUser::sceKernelDeleteFpl( int uid )
{
	KernelPool^ pool = ( KernelPool^ )_kernel->FindHandle( uid );
	Debug::Assert( pool != nullptr );
	if( pool == nullptr )
		return -1;

	_kernel->RemoveHandle( pool );

	pool->Cleanup();

	return 0;
}

// int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1380)
int ThreadManForUser::sceKernelAllocateFpl( IMemory^ memory, int uid, int data, int timeout )
{
	KernelPool^ pool = ( KernelPool^ )_kernel->FindHandle( uid );
	Debug::Assert( pool != nullptr );
	if( pool == nullptr )
		return -1;

	// TODO: put thread to sleep for timeout - wake early if block freed
	KernelMemoryBlock^ block = pool->Allocate();
	Debug::Assert( block != nullptr );
	if( block == nullptr )
		return -1;
	
	memory->WriteWord( data, 4, block->Address );

	return 0;
}

// int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1391)
int ThreadManForUser::sceKernelAllocateFplCB( IMemory^ memory, int uid, int data, int timeout )
{
	KernelPool^ pool = ( KernelPool^ )_kernel->FindHandle( uid );
	Debug::Assert( pool != nullptr );
	if( pool == nullptr )
		return -1;

	// TODO: put thread to sleep and wait on a free
	KernelMemoryBlock^ block = pool->Allocate();
	Debug::Assert( block != nullptr );
	if( block == nullptr )
		return -1;
	
	memory->WriteWord( data, 4, block->Address );

	return 0;
}

// int sceKernelTryAllocateFpl(SceUID uid, void **data); (/user/pspthreadman.h:1401)
int ThreadManForUser::sceKernelTryAllocateFpl( IMemory^ memory, int uid, int data )
{
	KernelPool^ pool = ( KernelPool^ )_kernel->FindHandle( uid );
	Debug::Assert( pool != nullptr );
	if( pool == nullptr )
		return -1;

	KernelMemoryBlock^ block = pool->Allocate();
	if( block == nullptr )
		return -1;
	
	memory->WriteWord( data, 4, block->Address );

	return 0;
}

// int sceKernelFreeFpl(SceUID uid, void *data); (/user/pspthreadman.h:1411)
int ThreadManForUser::sceKernelFreeFpl( IMemory^ memory, int uid, int data )
{
	KernelPool^ pool = ( KernelPool^ )_kernel->FindHandle( uid );
	Debug::Assert( pool != nullptr );
	if( pool == nullptr )
		return -1;

	pool->Free( data );

	return 0;
}

// int sceKernelCancelFpl(SceUID uid, int *pnum); (/user/pspthreadman.h:1421)
int ThreadManForUser::sceKernelCancelFpl( IMemory^ memory, int uid, int pnum )
{
	return NISTUBRETURN;
}

// int sceKernelReferFplStatus(SceUID uid, SceKernelFplInfo *info); (/user/pspthreadman.h:1442)
int ThreadManForUser::sceKernelReferFplStatus( IMemory^ memory, int uid, int info )
{
	return NISTUBRETURN;
}
