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
#include "R4000Hook.h"

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

extern uint _managedSyscallCount;
extern uint _nativeSyscallCount;
extern uint _cpuSyscallCount;
extern uint _unimplementedSyscallCount;

#ifdef SYSCALLSTATS
extern uint _syscallCounts[ 1024 ];
#endif

#define LOGSYSCALLS
#define NIRETURN		0

// If defined, even syscalls with DontTrace marked will be logged
//#define LOGALLSYSCALLS

#define g context->Generator

void __logSyscall( int syscallId, int address, bool implemented )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;

	BiosFunction^ function = cpu->_syscalls[ syscallId ];
	Debug::Assert( function != nullptr );
	if( function != nullptr )
	{
#ifndef LOGALLSYSCALLS
		if( function->DontTrace == true )
			return;
#endif
	
		R4000Ctx* ctx = ( R4000Ctx* )cpu->_ctx;
		String^ args;
		Debug::Assert( function->ParameterCount <= 12 );
		int paramCount = ( implemented == true ) ? function->ParameterCount : 8;
		switch( paramCount )
		{
		case 1:
			args = String::Format( "{0:X8}", ctx->Registers[ 4 ] );
			break;
		case 2:
			args = String::Format( "{0:X8}, {1:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ] );
			break;
		case 3:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ] );
			break;
		case 4:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ] );
			break;
		case 5:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ] );
			break;
		case 6:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ] );
			break;
		case 7:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ] );
			break;
		case 8:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}, {7:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ], ctx->Registers[ 11 ] );
			break;
		case 9:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}, {7:X8}, {8:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ], ctx->Registers[ 11 ], -1 );
			break;
		case 10:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}, {7:X8}, {8:X8}, {9:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ], ctx->Registers[ 11 ], -1, -1 );
			break;
		case 11:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}, {7:X8}, {8:X8}, {9:X8}, {10:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ], ctx->Registers[ 11 ], -1, -1, -1 );
			break;
		case 12:
			args = String::Format( "{0:X8}, {1:X8}, {2:X8}, {3:X8}, {4:X8}, {5:X8}, {6:X8}, {7:X8}, {8:X8}, {9:X8}, {10:X8}, {11:X8}", ctx->Registers[ 4 ], ctx->Registers[ 5 ], ctx->Registers[ 6 ], ctx->Registers[ 7 ], ctx->Registers[ 8 ], ctx->Registers[ 9 ], ctx->Registers[ 10 ], ctx->Registers[ 11 ], -1, -1, -1, -1 );
			break;
		default:
			args = "";
			break;
		}
		String^ log = String::Format( "{5}{0}::{1}({2}) from 0x{3:X8}{4}",
			( function->Module != nullptr ) ? function->Module->Name : "*unknown*",
			( function->Name != nullptr ) ? function->Name : String::Format( "{0:X8}", function->NID ),
			args, address - 4,
			function->IsImplemented ? "" : " (NI)",
			function->IsMissing ? "(NOT FOUND) " : "" );
		Log::WriteLine( Verbosity::Verbose, Feature::Syscall, log );

#if 0
		array<Frame^>^ frames = cpu->_hook->GetCallstack();
		Text::StringBuilder^ sb = gcnew Text::StringBuilder();
		for( int n = 0; n < frames->Length; n++ )
		{
			if( frames[ n ]->Type == FrameType::UserCode )
				sb->AppendFormat( "{0:X}, ", frames[ n ]->Address );
			else
				sb->AppendFormat( "{0}, ", frames[ n ]->Type );
		}
		if( sb->Length > 0 )
			Log::WriteLine( Verbosity::Verbose, Feature::Syscall, sb->ToString() );
#endif
	}
	else
	{
		String^ log = String::Format( "{0:X8}) from 0x{1:X8} (NOT FOUND)", syscallId, address - 4 );
		Log::WriteLine( Verbosity::Normal, Feature::Syscall, log );
		Debugger::Break();
	}
}

void __unimplementedSyscall( int syscallId, int address )
{
#ifdef LOGSYSCALLS
	__logSyscall( syscallId, address, false );
#endif
}

void __syscallBounce( int syscallId, int address )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;

#ifdef LOGSYSCALLS
	__logSyscall( syscallId, address, true );
#endif

	BiosShim^ shim = cpu->_syscallShims[ syscallId ];
	Debug::Assert( shim != nullptr );
	if( shim == nullptr )
		return;

	shim( cpu );
}

void EmitNativeCall( R4000GenContext^ context, BiosFunction^ function, int sid )
{
	void* ptr = R4000Cpu::GlobalCpu->_syscallShimsN[ sid ].ToPointer();
	g->call( ( uint )ptr );
}

GenerationResult SYSCALL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte _function )
{
	int syscall = ( int )( ( code >> 6 ) & 0xFFFFF );

#ifdef SYSCALLSTATS
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
#endif

	BiosFunction^ function = R4000Cpu::GlobalCpu->_syscalls[ syscall ];
	bool willCall;
	bool canEmit;
	bool hasReturn;
	bool wideReturn;
	if( ( function != nullptr ) && ( function->IsMissing == false ) )
	{
		willCall = function->IsImplemented;

		// Take its word on its statelessness
		//context->LastSyscallStateless = function->IsStateless;
		context->LastSyscallStateless = true;

		hasReturn = ( function->MethodInfo->ReturnType != void::typeid );
		wideReturn = ( function->MethodInfo->ReturnType == Int64::typeid );

		if( function->IsImplemented == true )
		{
			// If it is implemented we can only emit if it is also stateless
			canEmit = function->IsStateless;
		}
		else
		{
			// If not implemented we can always emit
			canEmit = true;

			// Sanity check - we can't have unimplemented methods that have native pointers!
			Debug::Assert( function->NativeMethod == IntPtr::Zero );

			if( pass == 0 )
			{
				Log::WriteLine( Verbosity::Normal, Feature::Cpu, "R4000Generator: NID 0x{0:X8} {1} is not implemented",
					function->NID, function->Name );
			}
		}
	}
	else
	{
		willCall = false;

		// Assume stateless and we can emit
		canEmit = true;
		context->LastSyscallStateless = true;

		hasReturn = true;
		wideReturn = false;

		if( pass == 0 )
		{
			if( function != nullptr )
				Log::WriteLine( Verbosity::Normal, Feature::Cpu, "R4000Generator: unregistered syscall attempt (at 0x{0:X8}) to NID 0x{1:X8}", address, function->NID );
			else
				Log::WriteLine( Verbosity::Normal, Feature::Cpu, "R4000Generator: unregistered syscall attempt (at 0x{0:X8})", address );
		}
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

		// Override if we can - we do this regardless of whether or not the BIOS implements it
#ifdef OVERRIDESYSCALLS
		if( canEmit == true )
		{
			// Note we may not emit here!
			bool emitted = R4000Cpu::GlobalCpu->_biosStubs->EmitCall( context, g, address, function->NID );
			if( emitted == true  )
			{
				function->HasCpuImplementation = true;

				// If we emitted then we must be stateless
				context->LastSyscallStateless = true;

//#ifdef GENDEBUG
				Log::WriteLine( Verbosity::Verbose, Feature::Cpu, "Overrode {0} with native method", function->Name );
//#endif

				// Everything handled for us by our overrides
				if( hasReturn == true )
				{
					g->mov( MREG( CTX, 2 ), EAX );
					if( wideReturn == true )
						g->mov( MREG( CTX, 3 ), EBX );
				}

#ifdef STATISTICS
				g->inc( g->dword_ptr[ &_cpuSyscallCount ] );
#endif
#ifdef SYSCALLSTATS
				g->inc( g->dword_ptr[ &_syscallCounts[ syscall ] ] );
#endif
				return GenerationResult::Syscall;
			}
		}
#endif
		
		if( willCall == true )
		{
			// Couldn't do a native stub, so try to emit a native call

			if( function->NativeMethod != IntPtr::Zero )
			{
				EmitNativeCall( context, function, syscall );

#ifdef STATISTICS
				g->inc( g->dword_ptr[ &_nativeSyscallCount ] );
#endif
			}
			else
			{
				// We push $ra as the address cause it is where the stub will go back to
				g->push( MREG( CTX, 31 ) );
				g->push( ( uint )syscall );
				g->call( ( uint )&__syscallBounce );
				g->add( ESP, 8 );

#ifdef STATISTICS
				g->inc( g->dword_ptr[ &_managedSyscallCount ] );
#endif
			}
		}
		else
		{
			// No native stub AND not implemented - uh oh!

			g->push( MREG( CTX, 31 ) );
			g->push( ( uint )syscall );
			g->call( ( uint )&__unimplementedSyscall );
			g->add( ESP, 8 );

			if( function != nullptr )
			{
				if( hasReturn == true )
				{
					g->mov( MREG( CTX, 2 ), ( int )NIRETURN );
					if( wideReturn == true )
						g->mov( MREG( CTX, 3 ), ( int )NIRETURN );
				}
			}
			else
			{
				// We don't even have a freaking function - just put -1 in $v0 and call it a day
				g->mov( MREG( CTX, 2 ), ( int )NIRETURN );
			}

#ifdef STATISTICS
			g->inc( g->dword_ptr[ &_unimplementedSyscallCount ] );
#endif
		}

#ifdef SYSCALLSTATS
		g->inc( g->dword_ptr[ &_syscallCounts[ syscall ] ] );
#endif

		// Emit stop flag check - if the flag is set we return
		Label* skipStopLabel = g->DefineLabel();
		g->mov( EAX, MSTOPFLAG( CTX ) );
		g->test( EAX, EAX );
		g->je( skipStopLabel );
		//g->int3();
		g->mov( EAX, 1 );
		g->ret();
		g->MarkLabel( skipStopLabel );
	}

	return GenerationResult::Syscall;
}

extern void __runtimeRegsPrint();
extern void __runtimeDebugPrintForce( int address, int code );
void __dumpMemory()
{
	R4000Memory^ memory = R4000Cpu::GlobalCpu->_memory;
	memory->DumpMainMemory( "dump.bin" );
}

GenerationResult BREAK( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		//Debug::WriteLine( String::Format( "R4000Generator: BREAK not implemented (at 0x{0:X8})", address ) );
	}
	else if( pass == 1 )
	{
#ifdef _DEBUG
		g->push( ( uint )code );
		g->push( ( uint )( address - 4 ) );
		g->call( ( uint )&__runtimeDebugPrintForce );
		g->add( ESP, 8 );
		g->call( ( uint )&__runtimeRegsPrint );
		g->call( ( uint )&__dumpMemory );
		g->int3();
#endif
	}
	return GenerationResult::Success;
}

GenerationResult SYNC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	// pg 629 - not needed?
	if( pass == 0 )
	{
		Log::WriteLine( Verbosity::Verbose, Feature::Cpu, "R4000Generator: SYNC not implemented (at 0x{0:X8})", address );
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
		Log::WriteLine( Verbosity::Verbose, Feature::Cpu, "R4000Generator: HALT not implemented (at 0x{0:X8})", address );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

//int __mfic()
//{
//	return R4000Cpu::GlobalCpu->_core0->InterruptState;
//}

GenerationResult MFIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		//g->call( ( uint )&__mfic );
		//g->mov( MREG( CTX, rt ), EAX );
		g->mov( EAX, MINTMASK( CTX ) );
		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

//void __mtic( int value )
//{
//	R4000Cpu::GlobalCpu->_core0->InterruptState = value;
//}

GenerationResult MTIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		/*if( rt == 0 )
			g->push( ( uint )0 );
		else
			g->push( MREG( CTX, rt ) );
		g->call( ( uint )&__mtic );
		g->add( ESP, 4 );*/
		g->mov( EAX, MREG( CTX, rt ) );
		g->mov( MINTMASK( CTX ), EAX );
		// Fire pending interrupts?
	}
	return GenerationResult::Success;
}
