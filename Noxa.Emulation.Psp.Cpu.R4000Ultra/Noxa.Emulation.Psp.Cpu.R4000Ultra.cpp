// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "stdafx.h"

#include "Noxa.Emulation.Psp.Cpu.R4000Ultra.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#pragma warning(disable:4793)
bool CheckSSE()
{
	bool hasSSE = false;
	__asm
	{
		// EDX = feature bits
		mov eax, 1
		cpuid

		// bit 25 = SSE, bit 26 = SSE2 = 0x600000
		// we require SSE2 cause I'm awesome = 0x4000000
		test edx, 0x4000000

		jz noSSE

		mov [hasSSE], 1

		noSSE:
	}
	
	return hasSSE;
}
#pragma warning(default:4793)

IList<ComponentIssue^>^ UltraCpu::Test( ComponentParameters^ parameters )
{
	List<ComponentIssue^>^ issues = gcnew List<ComponentIssue^>();

	bool is64 = ( sizeof( void* ) > 4 );
	if( is64 == true )
		issues->Add( gcnew ComponentIssue( this, IssueLevel::Error, "The CPU is compiled for 64-bit - this is NOT supported! Recompile!" ) );

	bool hasSSE = CheckSSE();
	if( hasSSE == false )
		issues->Add( gcnew ComponentIssue( this, IssueLevel::Error, "SSE support is required. Please get a CPU made within the last half-decade." ) );

	return issues;
}