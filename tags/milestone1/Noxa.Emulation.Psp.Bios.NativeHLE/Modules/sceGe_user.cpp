// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceGe_user.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Video;

sceGe_user::sceGe_user( IntPtr kernel ) : Module( kernel )
{
}

sceGe_user::~sceGe_user()
{
}

void sceGe_user::Start()
{
	_callbacks = new LL<GeCallback*>();
}

void sceGe_user::Stop()
{
	SAFEDELETE( _callbacks );
}

void sceGe_user::Clear()
{
	SAFEDELETE( _callbacks );
}

// int sceGeSetCallback(PspGeCallbackData *cb); (/ge/pspge.h:193)
int sceGe_user::sceGeSetCallback( IMemory^ memory, int pcb )
{
	GeCallback* cb = new GeCallback();
	cb->SignalFunction = memory->ReadWord( pcb );
	cb->SignalArgument = memory->ReadWord( pcb + 4 );
	cb->FinishFunction = memory->ReadWord( pcb + 8 );
	cb->FinishArgument = memory->ReadWord( pcb + 16 );
	KHandle* handle = _kernel->Handles->Add( cb );

	_callbacks->Enqueue( cb );

	Debug::WriteLine( "sceGeSetCallback: callback set - not good!" );
	
	return cb->UID;
}

// int sceGeUnsetCallback(int cbid); (/ge/pspge.h:201)
int sceGe_user::sceGeUnsetCallback( int cbid )
{
	GeCallback* cb = ( GeCallback* )_kernel->Handles->Lookup( cbid );
	if( cb == NULL )
		return -1;

	_kernel->Handles->Remove( cb );
	_callbacks->Remove( cb );

	return 0;
}
