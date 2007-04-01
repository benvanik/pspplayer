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

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// ThreadManForUser_Time.cpp
extern int sceKernelGetSystemTimeLowN();

void* ThreadManForUser::QueryNativePointer( uint nid )
{
	switch( nid )
	{
	case 0x369ED59D:		// sceKernelGetSystemTimeLow
		return &sceKernelGetSystemTimeLowN;
	}
	return 0;
}
