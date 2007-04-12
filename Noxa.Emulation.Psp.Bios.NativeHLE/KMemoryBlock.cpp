// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KMemoryBlock.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KMemoryBlock::KMemoryBlock( KPartition* partition, uint address, uint size )
{
	Partition = partition;
	Address = address;
	Size = size;
	UpperBound = address + size;
	IsFree = true;
	Name = NULL;
}

KMemoryBlock::~KMemoryBlock()
{
	SAFEFREE( Name );
}
