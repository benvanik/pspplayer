// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include "KCallback.h"
#include "KThread.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KCallback::KCallback( char* name, KThread* thread, uint address, uint commonAddress )
{
	Name = _strdup( name );
	Thread = thread;
	Address = address;
	CommonAddress = commonAddress;
}

KCallback::~KCallback()
{
	SAFEFREE( Name );
}
