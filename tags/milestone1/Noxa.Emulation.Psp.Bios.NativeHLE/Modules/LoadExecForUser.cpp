// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "LoadExecForUser.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KThread.h"
#include "KCallback.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// void sceKernelExitGame(); (/user/psploadexec.h:57)
void LoadExecForUser::sceKernelExitGame()
{
	this->sceKernelExitGameWithStatus( 0 );
}

// manual add
void LoadExecForUser::sceKernelExitGameWithStatus( int status )
{
	Debug::WriteLine( String::Format( "sceKernelExitGameWithStatus: status = {0}", status ) );
	_kernel->StopGame( status );
}

// int sceKernelRegisterExitCallback(int cbid); (/user/psploadexec.h:49)
int LoadExecForUser::sceKernelRegisterExitCallback( int cbid )
{
	KCallback* cb = ( KCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	LL<KCallback*>* list = _kernel->Callbacks->GetValue( _kernel->CallbackTypes.Exit );
	list->Enqueue( cb );

	return 0;
}
