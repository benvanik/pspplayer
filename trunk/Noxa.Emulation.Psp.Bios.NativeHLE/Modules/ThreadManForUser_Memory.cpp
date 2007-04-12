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
#include "KPool.h"
#include "KPartition.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// - VPL ------------------------------------------------------------------------------------------------

// SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt); (/user/pspthreadman.h:1256)
int ThreadManForUser::sceKernelCreateVpl( IMemory^ memory, int name, int part, int attr, int size, int opt )
{
	KPartition* partition = _kernel->Partitions[ part ];
	assert( partition != NULL );
	if( partition == NULL )
		return -1;

	char buffer[ 32 ];
	KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )32 );
	KPool* pool = new KVariablePool( partition, buffer, attr, size );
	
	_kernel->Handles->Add( pool );

	return pool->UID;
}

// int sceKernelDeleteVpl(SceUID uid); (/user/pspthreadman.h:1265)
int ThreadManForUser::sceKernelDeleteVpl( int uid )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	_kernel->Handles->Remove( pool->UID );

	SAFEDELETE( pool );

	return 0;
}

// int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1277)
int ThreadManForUser::sceKernelAllocateVpl( IMemory^ memory, int uid, int size, int data, int timeout )
{ return NISTUBRETURN; }

// int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1289)
int ThreadManForUser::sceKernelAllocateVplCB( IMemory^ memory, int uid, int size, int data, int timeout )
{ return NISTUBRETURN; }

// int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data); (/user/pspthreadman.h:1300)
int ThreadManForUser::sceKernelTryAllocateVpl( IMemory^ memory, int uid, int size, int data )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	KMemoryBlock* block = pool->Allocate();
	if( block == NULL )
		return -1;
	
	uint* pdata = ( uint* )MSI( memory )->Translate( data );
	*pdata = block->Address;

	return 0;
}

// int sceKernelFreeVpl(SceUID uid, void *data); (/user/pspthreadman.h:1310)
int ThreadManForUser::sceKernelFreeVpl( IMemory^ memory, int uid, int data )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	pool->Free( data );

	return 0;
}

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
	KPartition* partition = _kernel->Partitions[ part ];
	assert( partition != NULL );
	if( partition == NULL )
		return -1;

	char buffer[ 32 ];
	KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )32 );
	KPool* pool = new KFixedPool( partition, buffer, attr, size, blocks );
	
	_kernel->Handles->Add( pool );

	return pool->UID;
}

// int sceKernelDeleteFpl(SceUID uid); (/user/pspthreadman.h:1369)
int ThreadManForUser::sceKernelDeleteFpl( int uid )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	_kernel->Handles->Remove( pool->UID );

	SAFEDELETE( pool );

	return 0;
}

// int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1380)
int ThreadManForUser::sceKernelAllocateFpl( IMemory^ memory, int uid, int data, int timeout )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	KMemoryBlock* block = pool->Allocate();
	while( block == NULL )
	{
		/*uint timeoutUs = 0;
		if( timeout != 0 )
		{
			uint* ptimeout = ( uint* )MSI( memory )->Translate( timeout );
			timeoutUs = *ptimeout;
		}
		KThread* thread = _kernel->ActiveThread;
		thread->WaitPool( this, timeoutUs );
		if( _kernel->Schedule() == true )
		{
		}*/
		assert( false );
		return -1;
	}
	
	uint* pdata = ( uint* )MSI( memory )->Translate( data );
	*pdata = block->Address;

	return 0;
}

// int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1391)
int ThreadManForUser::sceKernelAllocateFplCB( IMemory^ memory, int uid, int data, int timeout )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	// TODO: put thread to sleep and wait on a free
	KMemoryBlock* block = pool->Allocate();
	assert( block != NULL );
	if( block == NULL )
		return -1;
	
	uint* pdata = ( uint* )MSI( memory )->Translate( data );
	*pdata = block->Address;

	return 0;
}

// int sceKernelTryAllocateFpl(SceUID uid, void **data); (/user/pspthreadman.h:1401)
int ThreadManForUser::sceKernelTryAllocateFpl( IMemory^ memory, int uid, int data )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
		return -1;

	KMemoryBlock* block = pool->Allocate();
	if( block == NULL )
		return -1;
	
	uint* pdata = ( uint* )MSI( memory )->Translate( data );
	*pdata = block->Address;

	return 0;
}

// int sceKernelFreeFpl(SceUID uid, void *data); (/user/pspthreadman.h:1411)
int ThreadManForUser::sceKernelFreeFpl( IMemory^ memory, int uid, int data )
{
	KPool* pool = ( KPool* )_kernel->Handles->Lookup( uid );
	assert( pool != NULL );
	if( pool == NULL )
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
