// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000BlockBuilder.h"
#include "R4000AdvancedBlockBuilder.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

R4000AdvancedBlockBuilder::R4000AdvancedBlockBuilder( R4000Cpu^ cpu, R4000Core^ core )
	: R4000BlockBuilder( cpu, core )
{
}

R4000AdvancedBlockBuilder::~R4000AdvancedBlockBuilder()
{
}