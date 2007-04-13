// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KModule.h"
#include <malloc.h>
#include <string.h>
#include "SDK/prxtypes.h"

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KModule::KModule( Bios::Kernel* kernel, BiosModule^ module )
{
	Kernel = kernel;

	Module = module;

	ModuleInfo = 0;
	ModuleBootStart = 0;
	ModuleRebootBefore = 0;
	ModuleStart = 0;
	ModuleStartThreadParam = 0;
	ModuleStop = 0;
	ModuleStopThreadParam = 0;

	if( module != nullptr )
	{
		Name = KernelHelpers::ToNativeString( module->Name );

		for each( StubExport^ ex in module->Exports )
		{
			switch( ex->NID )
			{
			case MODULE_INFO:
				ModuleInfo = ex->Address;
				break;
			case MODULE_BOOTSTART:
				ModuleBootStart = ex->Address;
				break;
			case MODULE_REBOOT_BEFORE:
				ModuleRebootBefore = ex->Address;
				break;
			case MODULE_START:
				ModuleStart = ex->Address;
				break;
			case MODULE_START_THREAD_PARAMETER:
				ModuleStartThreadParam = ex->Address;
				break;
			case MODULE_STOP:
				ModuleStop = ex->Address;
				break;
			case MODULE_STOP_THREAD_PARAMETER:
				ModuleStopThreadParam = ex->Address;
				break;
			}
		}
	}
	else
		Name = NULL;
}

KModule::~KModule()
{
	SAFEFREE( Name );
}
