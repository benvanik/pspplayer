// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KHandle.h"
#include "HandleTable.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

#define HANDLECAPACITY 1024 * 5

HandleTable::HandleTable( Kernel* kernel )
{
	assert( kernel != NULL );
	_kernel = kernel;

	_hashTable = HTCreateDefault( HANDLECAPACITY );
	assert( _hashTable != NULL );
}

HandleTable::~HandleTable()
{
	HTDelete( _hashTable );
	_hashTable = NULL;
}

KHandle* HandleTable::Add( KHandle* handle )
{
	assert( handle != NULL );
	if( handle->UID <= 0 )
		handle->UID = _kernel->AllocateUID();
	HTAdd( _hashTable, ( PTR )handle->UID, handle );
	return handle;
}

void HandleTable::Remove( int uid )
{
	HTRemove( _hashTable, ( PTR )uid );
}

void HandleTable::Remove( KHandle* handle )
{
	assert( handle != NULL );
	if( handle == NULL )
		return;
	this->Remove( handle->UID );
}

KHandle* HandleTable::Lookup( int uid )
{
	return ( KHandle* )HTFind( _hashTable, ( PTR )uid );
}
