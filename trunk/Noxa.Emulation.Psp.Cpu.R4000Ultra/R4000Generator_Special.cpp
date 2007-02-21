// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Generator.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000GenContext.h"
#include "R4000BiosStubs.h"

#include "Loader.hpp"
#include "CodeGenerator.hpp"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

extern int _nativeSyscallCount;

#define g context->Generator

int __syscallBounce( int syscallId, int a0, int a1, int a2, int a3, int sp )
{
	BiosFunction^ function = R4000Cpu::GlobalCpu->_syscalls[ syscallId ];
	Debug::Assert( function != nullptr );

#ifdef SYSCALLSTATS
	R4000Cpu::GlobalCpu->_stats->BiosSyscallCount++;

	int currentStat = R4000Cpu::GlobalCpu->_syscallCounts[ syscallId ];
	R4000Cpu::GlobalCpu->_syscallCounts[ syscallId ] = currentStat + 1;
#endif

	return function->Target( R4000Cpu::GlobalCpu->_memory, a0, a1, a2, a3, sp );
}

GenerationResult SYSCALL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	int syscall = ( int )( ( code >> 6 ) & 0xFFFFF );

	BiosFunction^ biosFunction = R4000Cpu::GlobalCpu->_syscalls[ syscall ];
	bool willCall;
	bool hasReturn;
	int paramCount;
	if( biosFunction != nullptr )
	{
		willCall = biosFunction->IsImplemented;
		hasReturn = biosFunction->HasReturn;
		paramCount = biosFunction->ParameterCount;

		if( biosFunction->IsImplemented == false )
		{
			if( pass == 0 )
			{
				Debug::WriteLine( String::Format( "R4000Generator: NID 0x{0:X8} {1} is not implemented",
					biosFunction->NID, biosFunction->Name ) );
			}
		}
	}
	else
	{
		willCall = false;
		hasReturn = false;
		paramCount = 0;

		if( pass == 0 )
			Debug::WriteLine( "R4000Generator: unregistered syscall attempt" );
	}

	context->UseSyscalls = true;

	if( pass == 1 )
	{
		// It's important that we save what we think is the current PC
		// If we had an UpdatePc, it means a branch has updated it before us
		// and we need to save it - otherwise, save the PC following us
		if( context->UpdatePC == true )
		{
			// Already changed
		}
		else
		{
			g->mov( MPC( CTX ), address + 4 );
			//g->mov( MPCVALID( CTX ), 1 );
		}

		if( willCall == true )
		{
			// Override if we can
			bool emitted = false;
			context->LastSyscallAtomic = biosFunction->IsAtomic;
#ifdef OVERRIDESYSCALLS
			if( biosFunction->IsAtomic == true )
			{
				// Note we may not emit here!
				emitted = R4000Cpu::GlobalCpu->_biosStubs->EmitCall( context, g, address, biosFunction->NID );
			}
#endif

			if( emitted == true  )
			{
#ifdef GENDEBUG
				Debug::WriteLine( String::Format( "Overrode {0} with native method", biosFunction->Name ) );
#endif

				// Everything handled for us by our overrides
				if( hasReturn == true )
					g->mov( MREG( CTX, 2 ), EAX );

#ifdef STATISTICS
				g->inc( g->dword_ptr[ &_nativeSyscallCount ] );
#endif
			}
			else
			{
				// Otherwise we use the thunk

				// up to 5 registers (4-7 + 29)
				if( paramCount > 0 )
				{
					if( paramCount > 4 )
						g->push( MREG( CTX, 29 ) );
					else
						g->push( ( uint )0 );

					if( paramCount > 3 )
						g->push( MREG( CTX, 7 ) );
					else
						g->push( ( uint )0 );

					if( paramCount > 2 )
						g->push( MREG( CTX, 6 ) );
					else
						g->push( ( uint )0 );

					if( paramCount > 1 )
						g->push( MREG( CTX, 5 ) );
					else
						g->push( ( uint )0 );

					g->push( MREG( CTX, 4 ) );
				}
				else
				{
					g->push( ( uint )0 );
					g->push( ( uint )0 );
					g->push( ( uint )0 );
					g->push( ( uint )0 );
					g->push( ( uint )0 );
				}

				g->push( ( uint )syscall );

				// 6 ints on stack

				g->call( ( int )__syscallBounce );

				g->add( ESP, 6 * 4 );
				
				if( hasReturn == true )
					g->mov( MREG( CTX, 2 ), EAX );
			}
		}
		else
		{
			if( hasReturn == true )
				g->mov( MREG( CTX, 2 ), ( int )-1 );
		}
	}

	return GenerationResult::Syscall;
}

GenerationResult BREAK( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: BREAK not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult SYNC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	// pg 629 - not needed?
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: SYNC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult COP1( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult COP2( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult HALT( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: HALT not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult MFIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: MFIC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult MTIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: MTIC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}
