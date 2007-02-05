// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

R4000Cpu::R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalCpu = this;

	_emu = emulator;
	_params = parameters;
	_caps = gcnew R4000Capabilities();
	_clock = gcnew R4000Clock();
	_memory = gcnew R4000Memory();
	_core0 = gcnew R4000Core( this );

#ifdef _DEBUG
	_timer = gcnew PerformanceTimer();
	_timeSinceLastIpsPrint = 0.0;
#endif

	_lastSyscall = -1;
	_syscalls = gcnew array<BiosFunction^>( 1024 );
}

int R4000Cpu::RegisterSyscall( unsigned int nid )
{
	BiosFunction^ function = _emu->Bios->FindFunction( nid );
	if( function == nullptr )
		return -1;

	int sid = ++_lastSyscall;
	_syscalls[ sid ] = function;

	return sid;
}

void R4000Cpu::Cleanup()
{
	_memory->Clear();
}

int R4000Cpu::ExecuteBlock()
{
#ifdef _DEBUG
	double blockStart = _timer->Elapsed;
#endif

	int instructionsExecuted = 0;

#ifdef _DEBUG
	double blockTime = _timer->Elapsed - blockStart;
	if( blockTime <= 0.0 )
		blockTime = 0.000001;
	
	_timeSinceLastIpsPrint += blockTime;
	if( _timeSinceLastIpsPrint > 1.0 )
	{
		double ips = ( ( double )instructionsExecuted / blockTime );
		Debug::WriteLine( String::Format( "IPS: {0}", ( long )ips ) );
		_timeSinceLastIpsPrint = 0.0;
	}
#endif

	return instructionsExecuted;
}