// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "ModuleMgrForUser.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// int sceKernelSelfStopUnloadModule(int unknown, SceSize argsize, void *argp); (/user/pspmodulemgr.h:152)
int ModuleMgrForUser::sceKernelSelfStopUnloadModule( int status, int argsize, int argp )
{
	return this->sceKernelStopUnloadSelfModule( argsize, argp, status, 0 );
}

// int sceKernelStopUnloadSelfModule(SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:164)
int ModuleMgrForUser::sceKernelStopUnloadSelfModule( int argsize, int argp, int status, int option )
{
	_kernel->ExitGame();

	return 0;
}
