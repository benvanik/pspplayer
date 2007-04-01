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

void sceGe_user::Start()
{
	_callbacks = gcnew List<GeCallbackHandle^>();
}

void sceGe_user::Stop()
{
	_callbacks = nullptr;
}

void sceGe_user::Clear()
{
	_callbacks = nullptr;
}

// int sceGeSetCallback(PspGeCallbackData *cb); (/ge/pspge.h:193)
int sceGe_user::sceGeSetCallback( IMemory^ memory, int pcb )
{
	GeCallbackHandle^ cb = gcnew GeCallbackHandle( _kernel->AllocateID() );
	cb->SignalFunction = memory->ReadWord( pcb );
	cb->SignalArgument = memory->ReadWord( pcb + 4 );
	cb->FinishFunction = memory->ReadWord( pcb + 8 );
	cb->FinishArgument = memory->ReadWord( pcb + 16 );
	_kernel->AddHandle( cb );

	_callbacks->Add( cb );

	Debug::WriteLine( "sceGeSetCallback: callback set - not good!" );
	
	return cb->ID;
}

// int sceGeUnsetCallback(int cbid); (/ge/pspge.h:201)
int sceGe_user::sceGeUnsetCallback( int cbid )
{
	GeCallbackHandle^ cb = ( GeCallbackHandle^ )_kernel->FindHandle( cbid );
	if( cb == nullptr )
		return -1;

	_kernel->RemoveHandle( cb );

	_callbacks->Remove( cb );

	return 0;
}
