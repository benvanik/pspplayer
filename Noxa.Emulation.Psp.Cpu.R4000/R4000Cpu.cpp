#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "Memory.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

// TODO: Make this an option
#define DefaultBlockCount	50000

R4000Cpu::R4000Cpu( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	_emu = emulator;
	_params = parameters;
	_caps = gcnew R4000Capabilities();
	_clock = gcnew R4000Clock();
	
	_lastSyscall = -1;
	_syscalls = gcnew array<BiosFunction^>( 1024 );

#ifdef _DEBUG
	_timer = gcnew PerformanceTimer();
	_timeSinceLastIpsPrint = 0.0;
#endif

	_memory = gcnew Cpu::Memory();

	// Order matters as the lookup is linear and stupid... should be changed somehow
	_memory->DefineSegment( MemoryType::PhysicalMemory, "Main Memory", 0x08000000, 0x01FFFFFF );
	_memory->DefineSegment( MemoryType::PhysicalMemory, "Hardware Vectors", 0x1FC00000, 0x000FFFFF );
	_memory->DefineSegment( MemoryType::PhysicalMemory, "Scratchpad", 0x00010000, 0x00003FFF );
	_memory->DefineSegment( MemoryType::PhysicalMemory, "Frame Buffer", 0x04000000, 0x001FFFFF );
	_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 1", 0x1C000000, 0x03BFFFFF );
	_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 2", 0x1FD00000, 0x002FFFFF );
	// TODO: Since this is fixed, maybe hardcode it for speed?

	_cores = gcnew array<R4000Core^>( 2 );
	_cores[ 0 ] = gcnew R4000Core( this, "Allegrex", 0, CoreAttributes::Default | CoreAttributes::HasCp2 );
	_cores[ 1 ] = gcnew R4000Core( this, "Media Engine", 1, CoreAttributes::Default );
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

int R4000Cpu::ExecuteBlock()
{
#ifdef _DEBUG
	double blockStart = _timer->Elapsed;
#endif

	int count = 0;
	for( int n = 0; n < DefaultBlockCount; n++ )
	{
		R4000Core^ core = _cores[ 0 ];

		int address = core->Pc;
		address = core->TranslateAddress( address );
		bool delayNop = core->DelayNop;

		//_debug = true;
		if( address == 0x8900350 )
		{
			_debug = true;
		}
		//if( address == 0x08901594 )
		//	Debugger::Break();
		//if( address == 0x089044a4 )
		//	Debugger::Break();

		/*if( _debug == true )
		{
			String^ s = String::Format( "0={0:X8} 1={1:X8} 2={2:X8} 3={3:X8} 4={4:X8} 5={5:X8} 6={6:X8} 7={7:X8} 8={8:X8} 9={9:X8} 10={10:X8} 11={11:X8} 12={12:X8} 13={13:X8} 14={14:X8} 15={15:X8} 16={16:X8} 17={17:X8} 18={18:X8} 19={19:X8} 20={20:X8} 21={21:X8} 22={22:X8} 23={23:X8} 24={24:X8} 25={25:X8} 26={26:X8} 27={27:X8} 28={28:X8} 29={29:X8} 30={30:X8} 31={31:X8}",
				core->Registers[ 0 ], core->Registers[ 1 ], core->Registers[ 2 ], core->Registers[ 3 ], core->Registers[ 4 ], core->Registers[ 5 ], core->Registers[ 6 ], core->Registers[ 7 ], core->Registers[ 8 ], core->Registers[ 9 ],
				core->Registers[ 10 ], core->Registers[ 11 ], core->Registers[ 12 ], core->Registers[ 13 ], core->Registers[ 14 ], core->Registers[ 15 ], core->Registers[ 16 ], core->Registers[ 17 ], core->Registers[ 18 ], core->Registers[ 19 ],
				core->Registers[ 20 ], core->Registers[ 21 ], core->Registers[ 22 ], core->Registers[ 23 ], core->Registers[ 24 ], core->Registers[ 25 ], core->Registers[ 26 ], core->Registers[ 27 ], core->Registers[ 28 ], core->Registers[ 29 ],
				core->Registers[ 30 ], core->Registers[ 31 ] );
			Debug::WriteLine( s );
		}*/

		// Fetch
		int instruction;
		//if( core->InDelaySlot == true )
			instruction = _memory->ReadWord( address );
		//else
		//	instruction = _memory->ReadWord( address );

		if( core->DelayNop == true )
		{
			instruction = 0;
			core->DelayNop = false;
		}
		
		// Execute
		count++;

#if _DEBUG
		if( _debug == true )
			Debug::WriteLine( String::Format( "0x{0:X8}: {1:X8}", address, instruction ) );
#endif

		if( core->Process( instruction ) == false )
			break;
	}

#ifdef _DEBUG

	double blockTime = _timer->Elapsed - blockStart;
	if( blockTime <= 0.0 )
		blockTime = 0.000001;
	
	_timeSinceLastIpsPrint += blockTime;
	if( _timeSinceLastIpsPrint > 1.0 )
	{
		double ips = ( ( double )count / blockTime );
		Debug::WriteLine( String::Format( "IPS: {0}", ( long )ips ) );
		_timeSinceLastIpsPrint = 0.0;
	}

#endif

	return count;
}