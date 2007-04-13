// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Kernel.h"
#include "KernelHelpers.h"
#include "HandleTable.h"

// The default return value for stubbed (unimplemented) methods
#define NISTUBRETURN -1

#define MSI( imemory ) ( ( MemorySystem* )imemory->MemorySystemInstance )
