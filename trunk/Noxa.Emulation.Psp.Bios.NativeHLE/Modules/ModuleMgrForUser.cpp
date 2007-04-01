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
	_kernel->ExitGame( status );

	return 0;
}

// SceUID sceKernelLoadModuleByID(SceUID fid, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:91)
int ModuleMgrForUser::sceKernelLoadModuleByID( IMemory^ memory, int fid, int flags, int option )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fid );
	if( handle == nullptr )
		return -1;

	return 0;
}

// SceUID sceKernelLoadModule(const char *path, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:68)
int ModuleMgrForUser::sceKernelLoadModule( IMemory^ memory, int path, int flags, int option )
{
	String^ modulePath = KernelHelpers::ReadString( memory, path );
	IMediaFile^ file = ( IMediaFile^ )KernelHelpers::FindPath( _kernel, modulePath );
	if( file == nullptr )
	{
		Debug::WriteLine( String::Format( "sceKernelLoadModule: module not found: {0}", modulePath ) );
		return -1;
	}

	Debug::WriteLine( String::Format( "sceKernelLoadModule: loading module {0}", modulePath ) );
	
	return 0;
}

// int sceKernelStartModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:119)
int ModuleMgrForUser::sceKernelStartModule( IMemory^ memory, int modid, int argsize, int argp, int status, int option )
{
	return 0;
}
