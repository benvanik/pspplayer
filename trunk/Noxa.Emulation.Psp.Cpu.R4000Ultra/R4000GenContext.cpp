// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000GenContext.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

void R4000GenContext::Reset( int startAddress )
{
	StartAddress = startAddress;
	EndAddress = startAddress;

	UpdatePC = false;
	UseSyscalls = false;

	BranchLabels->Clear();
	LastBranchTarget = 0;
	InDelay = false;
	BranchTarget = nullptr;
	JumpTarget = 0;
	JumpRegister = 0;
}

void R4000GenContext::DefineBranchTarget( int address )
{
	if( BranchLabels->ContainsKey( address ) == false )
	{
		//Debug::WriteLine( String::Format( "Defining branch target {0:X8}", address ) );
		LabelMarker^ lm = gcnew LabelMarker( address );
		lm->Label = Generator->DefineLabel();
		BranchLabels->Add( address, lm );
	}
	if( LastBranchTarget < address )
		LastBranchTarget = address;
}
