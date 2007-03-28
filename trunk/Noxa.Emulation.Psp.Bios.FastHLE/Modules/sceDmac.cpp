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

int sceDmacMemcpyN( byte* memory, int dest, int source, int size );

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
int sceDmacMemcpyN( byte* memory, int dest, int source, int size )
{
	// Only support main memory copies - anything else can DIAF
	assert( ( dest >= MainMemoryBase ) && ( dest < MainMemoryBound ) );
	assert( ( source >= MainMemoryBase ) && ( source < MainMemoryBound ) );

	memcpy(
		( memory + ( dest - MainMemoryBase ) ),
		( memory + ( source - MainMemoryBase ) ),
		size );

	return dest;
}
#pragma managed

int sceDmac::sceDmacMemcpy( IMemory^ memory, int dest, int source, int size )
{
	byte* ptr = ( byte* )memory->MainMemoryPointer;

	memcpy(
		( ptr + ( dest - MainMemoryBase ) ),
		( ptr + ( source - MainMemoryBase ) ),
		size );

	return dest;
}
