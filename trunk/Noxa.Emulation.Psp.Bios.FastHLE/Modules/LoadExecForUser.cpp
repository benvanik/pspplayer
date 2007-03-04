// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "LoadExecForUser.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KernelStatistics.h"
#include "KernelThread.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// void sceKernelExitGame(); (/user/psploadexec.h:57)
void LoadExecForUser::sceKernelExitGame()
{
	Debug::WriteLine( "sceKernelExitGame: called" );
	_kernel->ExitGame( 0 );
}

// manual add
void LoadExecForUser::sceKernelExitGameWithStatus( int status )
{
	Debug::WriteLine( String::Format( "sceKernelExitGameWithStatus: status = {0}", status ) );
	_kernel->ExitGame( status );
}

// int sceKernelRegisterExitCallback(int cbid); (/user/psploadexec.h:49)
int LoadExecForUser::sceKernelRegisterExitCallback( int cbid )
{
	KernelCallback^ callback = ( KernelCallback^ )_kernel->FindHandle( cbid );
	if( callback == nullptr )
		return -1;

	_kernel->Callbacks->Add( KernelCallbackType::Exit, callback );

	return 0;
}
