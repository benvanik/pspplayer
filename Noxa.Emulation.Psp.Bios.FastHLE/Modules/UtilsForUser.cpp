// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "UtilsForUser.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

int sceKernelUtilsMt19937InitN( byte* memory, int pctx, uint seed );
uint sceKernelUtilsMt19937UIntN( byte* memory, int pctx );

void* UtilsForUser::QueryNativePointer( uint nid )
{
	switch( nid )
	{
		// UtilsForUser_SHA1.cpp

		// UtilsForUser_MD5.cpp

		// UtilsForUser_MT19937.cpp
	case 0xE860E75E:	// sceKernelUtilsMt19937Init
		return &sceKernelUtilsMt19937InitN;
	case 0x06FB8A63:	// sceKernelUtilsMt19937UInt
		return &sceKernelUtilsMt19937UIntN;

	default:
		return 0x0;
	}
}
