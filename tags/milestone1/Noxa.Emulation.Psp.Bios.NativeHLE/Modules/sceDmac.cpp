// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "sceDmac.h"
#include "Kernel.h"
#include <string.h>
#include <assert.h>

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

int sceDmacMemcpyN( MemorySystem* memory, int dest, int source, int size );

void* sceDmac::QueryNativePointer( uint nid )
{
	switch( nid )
	{
	case 0x617F3FE6:
		return &sceDmacMemcpyN;
	};

	return 0;
}

#pragma unmanaged
int sceDmacMemcpyN( MemorySystem* memory, int dest, int source, int size )
{
	byte* pdest = memory->Translate( dest );
	byte* psrc = memory->Translate( source );

	memcpy( pdest, psrc, size );

	return dest;
}
#pragma managed

int sceDmac::sceDmacMemcpy( IMemory^ memory, int dest, int source, int size )
{
	byte* pdest = MSI( memory )->Translate( dest );
	byte* psrc = MSI( memory )->Translate( source );

	memcpy( pdest, psrc, size );

	return dest;
}
