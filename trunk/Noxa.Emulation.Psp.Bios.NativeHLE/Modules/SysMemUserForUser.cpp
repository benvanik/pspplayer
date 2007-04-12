// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "SysMemUserForUser.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KPartition.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// SceSize sceKernelMaxFreeMemSize(); (/user/pspsysmem.h:88)
int SysMemUserForUser::sceKernelMaxFreeMemSize()
{
	uint size = 0;
	for( int n = 0; n < PARTITIONCOUNT; n++ )
		size += _kernel->Partitions[ n ]->Size;
	return ( int )size;
}

// SceSize sceKernelTotalFreeMemSize(); (/user/pspsysmem.h:81)
int SysMemUserForUser::sceKernelTotalFreeMemSize()
{
	uint size = 0;
	for( int n = 0; n < PARTITIONCOUNT; n++ )
		size += _kernel->Partitions[ n ]->FreeSize;
	return ( int )size;
}

// manual add
int SysMemUserForUser::sceKPartitionMaxFreeMemSize( int partitionid )
{
	KPartition* partition = _kernel->Partitions[ partitionid ];
	return ( int )partition->Size;
}

// manual add
int SysMemUserForUser::sceKPartitionTotalFreeMemSize( int partitionid )
{
	KPartition* partition = _kernel->Partitions[ partitionid ];
	return ( int )partition->FreeSize;
}

// SceUID sceKernelAllocPartitionMemory(SceUID partitionid, const char *name, int type, SceSize size, void *addr); (/user/pspsysmem.h:56)
int SysMemUserForUser::sceKernelAllocPartitionMemory( IMemory^ memory, int partitionid, int name, int type, int size, int addr )
{
	KPartition* partition = _kernel->Partitions[ partitionid ];
	KMemoryBlock* block = partition->Allocate( ( KAllocType )type, addr, size );
	assert( block != NULL );
	
	char buffer[ 32 ];
	if( KernelHelpers::ReadString( MSI( memory ), ( const int )name, ( byte* )buffer, ( const int )32 ) > 0 )
		block->Name = _strdup( buffer );

	_kernel->Handles->Add( block );
	
	return block->UID;
}

// int sceKernelFreePartitionMemory(SceUID blockid); (/user/pspsysmem.h:65)
int SysMemUserForUser::sceKernelFreePartitionMemory( int blockid )
{
	KMemoryBlock* block = ( KMemoryBlock* )_kernel->Handles->Lookup( blockid );
	if( block == NULL )
		return -1;

	block->Partition->Free( block );
	_kernel->Handles->Remove( block );
	
	return 0;
}

// void * sceKernelGetBlockHeadAddr(SceUID blockid); (/user/pspsysmem.h:74)
int SysMemUserForUser::sceKernelGetBlockHeadAddr( int blockid )
{
	KMemoryBlock* block = ( KMemoryBlock* )_kernel->Handles->Lookup( blockid );
	if( block == NULL )
		return -1;
	
	return ( int )block->Address;
}

// int sceKernelDevkitVersion(); (/user/pspsysmem.h:104)
int SysMemUserForUser::sceKernelDevkitVersion()
{
	// 0x01000300 on v1.00 unit
	// 0x01050001 on v1.50 unit

	Debug::WriteLine( "SysMemUserForUser::sceKernelDevkitVersion: game requested devkit version - make sure it isn't doing something tricky!" );

	// int
	return 0x01050001;
}
