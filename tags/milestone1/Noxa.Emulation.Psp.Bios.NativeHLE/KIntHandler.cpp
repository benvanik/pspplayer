// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KIntHandler.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KIntHandler::KIntHandler( Bios::Kernel* kernel, int interruptNumber, int slot, uint address, uint argument )
{
	Kernel = kernel;
	InterruptNumber = interruptNumber;
	Slot = slot;
	Address = address;
	Argument = argument;
}

KIntHandler::~KIntHandler()
{
	if( _isInstalled == true )
		this->SetEnabled( false );
}

void KIntHandler::SetEnabled( bool enabled )
{
	if( _isInstalled == enabled )
		return;

	assert( Kernel->Cpu != NULL );
	if( _isInstalled == false )
	{
		Kernel->Cpu->RegisterInterruptHandler( InterruptNumber, Slot, Address, Argument );
		_isInstalled = true;
	}
	else
	{
		Kernel->Cpu->UnregisterInterruptHandler( InterruptNumber, Slot );
		_isInstalled = false;
	}
}
