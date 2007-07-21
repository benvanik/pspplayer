// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#pragma unmanaged
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#pragma managed
#include "DebugOptions.h"
#include "R4000Controller.h"
#include "R4000Cpu.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Hook.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Debugging;

extern R4000Ctx* _cpuCtx;

#define DEBUG_HANDLE_CONTINUE	0
#define DEBUG_HANDLE_BREAK		1

#define DEBUG_RESUME_CONTINUE	0	// resume normal
#define DEBUG_RESUME_STEP		1	// param = see below
#define DEBUG_RESUME_SET_NEXT	2	// param = new address

#define DEBUG_STEP_INTO			0
#define DEBUG_STEP_OVER			1
#define DEBUG_STEP_OUT			2

HANDLE	_debugHandle;
int		_debugResumeMode;
uint	_debugResumeParam;

uint FindStepAddress( R4000Controller^ controller, uint address, uint stepType );
byte* FindInstructionStart( CodeBlock* block, uint address, int* size );
void SetBreakpoint( Breakpoint^ breakpoint );
void UnsetBreakpoint( Breakpoint^ breakpoint );

R4000Controller::R4000Controller( R4000Cpu^ cpu )
{
	this->Cpu = cpu;

	_debugHandle = CreateEvent( NULL, FALSE, FALSE, NULL );
	_debugResumeMode = DEBUG_RESUME_CONTINUE;
	_debugResumeParam = 0;

	array<Instruction^>^ _analysisInstructions = gcnew array<Instruction^>{
		gcnew Instruction( "beq",		0x10000000, 0xFC000000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "beql",		0x50000000, 0xFC000000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bgez",		0x04010000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bgezal",	0x04110000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::JumpAndLink ),
		gcnew Instruction( "bgezl",		0x04030000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bgtz",		0x1C000000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bgtzl",		0x5C000000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "blez",		0x18000000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "blezl",		0x58000000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bltz",		0x04000000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bltzl",		0x04020000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bltzal",	0x04100000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::JumpAndLink ),
		gcnew Instruction( "bltzall",	0x04120000, 0xFC1F0000,	AddressBits::Bits16,		InstructionType::JumpAndLink ),
		gcnew Instruction( "bne",		0x14000000, 0xFC000000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bnel",		0x54000000, 0xFC000000,	AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "j",			0x08000000, 0xFC000000,	AddressBits::Bits26,		InstructionType::Jump ),
		gcnew Instruction( "jr",		0x00000008, 0xFC1FFFFF,	AddressBits::Register,		InstructionType::Jump ),
		gcnew Instruction( "jalr",		0x00000009, 0xFC1F07FF,	AddressBits::Register,		InstructionType::JumpAndLink ),
		gcnew Instruction( "jal",		0x0C000000, 0xFC000000, AddressBits::Bits26,		InstructionType::JumpAndLink ),
		gcnew Instruction( "bc1f",		0x45000000, 0xFFFF0000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bc1fl",		0x45020000, 0xFFFF0000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bc1t",		0x45010000, 0xFFFF0000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bc1tl",		0x45030000, 0xFFFF0000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bvf",		0x49000000, 0xFFE30000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bvfl",		0x49020000, 0xFFE30000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bvt",		0x49010000, 0xFFE30000, AddressBits::Bits16,		InstructionType::Branch ),
		gcnew Instruction( "bvtl",		0x49030000, 0xFFE30000, AddressBits::Bits16,		InstructionType::Branch ),
	};
}

R4000Controller::~R4000Controller()
{
	CloseHandle( _debugHandle );
	_debugHandle = NULL;
}

void R4000Controller::Run()
{
	PulseEvent( _debugHandle );
}

void R4000Controller::RunUntil( uint address )
{
}

void R4000Controller::Break()
{
}

void R4000Controller::SetNext( uint address )
{
	_debugResumeMode = DEBUG_RESUME_SET_NEXT;
	_debugResumeParam = address;
	PulseEvent( _debugHandle );
}

void R4000Controller::Step()
{
	_debugResumeMode = DEBUG_RESUME_STEP;
	_debugResumeParam = DEBUG_STEP_INTO;
	PulseEvent( _debugHandle );
}

void R4000Controller::StepOver()
{
	_debugResumeMode = DEBUG_RESUME_STEP;
	_debugResumeParam = DEBUG_STEP_OVER;
	PulseEvent( _debugHandle );
}

void R4000Controller::StepOut()
{
	_debugResumeMode = DEBUG_RESUME_STEP;
	_debugResumeParam = DEBUG_STEP_OUT;
	PulseEvent( _debugHandle );
}

R4000Controller::InstructionType R4000Controller::AnalyzeJump( uint address, uint code, uint* ptarget )
{
	for( int n = 0; n < _analysisInstructions->Length; n++ )
	{
		Instruction^ instr = _analysisInstructions[ n ];
		if( ( code & instr->Mask ) == instr->Opcode )
		{
			int offset;
			uint target = 0;
			switch( instr->Bits )
			{
			case R4000Controller::AddressBits::Bits16:
				offset = ( short )( code & 0xFFFF );
				target = ( uint )( address + 4 + offset * 4 );
				break;
			case R4000Controller::AddressBits::Bits26:
				target = ( code & 0x03FFFFFF ) << 2;
				target += address & 0xF0000000;
				break;
			default:
				target = UInt32::MaxValue;
				break;
			}
			*ptarget = target;
			return instr->Type;
		}
	}
	return R4000Controller::InstructionType::Normal;
}

void HandleTracepoint( Breakpoint^ breakpoint )
{
	String^ name;
	if( breakpoint->Name != nullptr )
		name = String::Format( "{0} (0x{1:X8})", breakpoint->Name, breakpoint->Address );
	else
		name = String::Format( "0x{0:X8}", breakpoint->Address );
	Log::WriteLine( Verbosity::Verbose, Feature::Debug, "tracepoint {0} reached, count: {1}", 
		name,
		breakpoint->HitCount );
}

int __debugHandlerM( int breakpointId )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;

	Breakpoint^ breakpoint;
	if( cpu->_hook->Breakpoints->TryGetValue( breakpointId, breakpoint ) == false )
	{
		// Not found? Inconceivable!
		Log::WriteLine( Verbosity::Critical, Feature::Debug, "breakpoint {0} not found on __debugHandler - wtf?", breakpointId );
		return DEBUG_HANDLE_CONTINUE;
	}

	int cacheVersion = cpu->_codeCache->Version;

	breakpoint->HitCount++;

	// Make sure the PC state is valid
	_cpuCtx->PC = ( int )breakpoint->Address;

	switch( breakpoint->Mode )
	{
	default:
	case BreakpointMode::Silent:
		// Ignore
		break;
	case BreakpointMode::Trace:
		HandleTracepoint( breakpoint );
		break;
	case BreakpointMode::Break:
		{
			// General process:
			// - replace debug thunk jump with original
			// - notify debugger
			// - wait on debug resume handle
			// - handle re-add/etc

			// Log & ensure attached
			if( breakpoint->Type != BreakpointType::Stepping )
				Log::WriteLine( Verbosity::Critical, Feature::Debug, "Breakpoint hit: {0}", breakpoint->ToString() );
			else
			{
				// If we are stepping then we damn well better be attached
				Debug::Assert( Diag::IsAttached == true );
			}
			Diag::EnsureAttached( String::Format( "Breakpoint hit: {0}", breakpoint->ToString() ) );

			switch( breakpoint->Type )
			{
			case BreakpointType::Stepping:
				UnsetBreakpoint( breakpoint );
				// See if we need to handle/reset an old breakpoint
				int oldBreakpointId;
				if( cpu->_hook->BreakpointLookup->TryGetValue( breakpoint->Address, oldBreakpointId ) == true )
				{
					Breakpoint^ oldBreakpoint = cpu->_hook->Breakpoints[ oldBreakpointId ];
					oldBreakpoint->HitCount++;
					if( oldBreakpoint->Mode == BreakpointMode::Trace )
						HandleTracepoint( oldBreakpoint );
					SetBreakpoint( oldBreakpoint );
				}
				Diag::Instance->Client->Handler->OnStepComplete( breakpoint->Address );
				cpu->_hook->Breakpoints->Remove( breakpoint->ID );
				cpu->_hook->SteppingBreakpoints->Remove( breakpoint );
				break;
			default:
				Diag::Instance->Client->Handler->OnBreakpointHit( breakpoint->ID );
				break;
			}

			_debugResumeMode = DEBUG_RESUME_CONTINUE;
			WaitForSingleObject( _debugHandle, INFINITE );

			Diag::Instance->Client->Handler->OnContinue();

			switch( _debugResumeMode )
			{
			default:
			case DEBUG_RESUME_CONTINUE:
				// Nothing to do
				break;
			case DEBUG_RESUME_STEP:
				{
					uint findStepAddress = FindStepAddress( cpu->_controller, breakpoint->Address, _debugResumeParam );
					Breakpoint^ newBreakpoint = gcnew Breakpoint( Diag::Instance->Client->AllocateID(), BreakpointType::Stepping, findStepAddress );
					cpu->_hook->Breakpoints->Add( newBreakpoint->ID, newBreakpoint );
					cpu->_hook->SteppingBreakpoints->Add( newBreakpoint );
					SetBreakpoint( newBreakpoint );
				}
				break;
			case DEBUG_RESUME_SET_NEXT:
				break;
			}
		}
		break;
	}

	if( cpu->_codeCache->Version != cacheVersion )
	{
		// Code cache was cleared while we were broken!
		Log::WriteLine( Verbosity::Critical, Feature::Debug, "code cache cleared while broken - not supported!" );
		Debugger::Break();
	}

	return DEBUG_HANDLE_CONTINUE;
}

#pragma unmanaged
extern "C" void * _AddressOfReturnAddress ( void );
extern "C" void * _ReturnAddress ( void );
#pragma intrinsic ( _AddressOfReturnAddress )
#pragma intrinsic ( _ReturnAddress )

// Call like:
//    DEBUGTHUNKSIZE - ensure this matches the size of the block below!
//    12 bytes total
// 68 NN NN NN NN	PUSH [breakpointId]
// B8 NN NN NN NN	MOV EAX, __debugThunk
// FF D0			CALL EAX
// -- NO CLEANUP
#pragma warning( disable: 4731 )
void __debugThunk( int breakpointId )
{
	void* returnAddress = _ReturnAddress();

	if( __debugHandlerM( breakpointId ) == DEBUG_HANDLE_CONTINUE )
	{
		// Exit and return to the app to continue running the current block
		__asm
		{
			mov eax, returnAddress

			mov esp, ebp
			pop ebp
			
			add esp, 8 // 4 for ra, 4 for param

			jmp eax
		}
	}
	else
	{
		// Break out of the code - assume we are dead or something
		__asm
		{
			mov esp, ebp
			pop ebp
			
			add esp, 8 // 4 for ra, 4 for param

			xor eax, eax
			ret
		}
	}
}
#pragma warning( pop )
#pragma managed

uint FindStepAddress( R4000Controller^ controller, uint address, uint stepType )
{
	switch( stepType )
	{
	case DEBUG_STEP_INTO:
		if( _cpuCtx->InDelay == 1 )
		{
			// The PC that the CPU thinks is next
			return _cpuCtx->NextPC;
		}
		else
		{
			// Normal step
			return address + 4;
		}
		break;
	case DEBUG_STEP_OVER:
		if( _cpuCtx->InDelay == 1 )
		{
			// We need to analyze - we only support step over on jal/jalr's
			uint target;
			uint code = *( ( uint* )controller->Cpu->Memory->MemorySystem->Translate( address - 4 ) );
			R4000Controller::InstructionType type = controller->AnalyzeJump( address - 4, code, &target );
			if( type == R4000Controller::InstructionType::JumpAndLink )
			{
				// Use address + 4, which is where control will return to
				return address + 4;
			}
			else
			{
				// Normal step into
				return _cpuCtx->NextPC;
			}
		}
		else
		{
			// Normal step
			return address + 4;
		}
		break;
	case DEBUG_STEP_OUT:
		// Break on $ra
		return _cpuCtx->Registers[ 31 ];
	}

	return 0;
}

byte* FindInstructionStart( CodeBlock* block, uint address, int* size )
{
	int ioffset = ( int )address - block->Address;
	Debug::Assert( ioffset >= 0 );
	Debug::Assert( ioffset < block->Size );
	ioffset >>= 2;

	int sum = block->PreambleSize;
	for( int n = 0; n < ioffset; n++ )
		sum += block->InstructionSizes[ n ];
	// sum now holds the offset, in bytes, of the start of the instruction

	*size = block->InstructionSizes[ ioffset ];

	return ( ( byte* )block->Pointer + sum );
}

void SetBreakpoint( Breakpoint^ breakpoint )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;

	switch( breakpoint->Type )
	{
	case BreakpointType::Stepping:
	case BreakpointType::CodeExecute:
	case BreakpointType::BiosFunction:
		{
			CodeBlock* blocks[ 10 ];
			int blockCount = cpu->_codeCache->Search( ( int )breakpoint->Address, ( CodeBlock** )blocks );
			for( int n = 0; n < blockCount; n++ )
			{
				CodeBlock* block = blocks[ n ];
				int size;
				byte* start = FindInstructionStart( block, breakpoint->Address, &size );

				// Write break jump bytes (see __debugThunk for more info)
				assert( start[ 0 ] == 0x90 );
				start[ 0 ] = 0x68;
				*( ( int* )( &start[ 1 ] ) ) = breakpoint->ID;
				start[ 5 ] = 0xB8;
				*( ( int* )( &start[ 6 ] ) ) = ( int )&__debugThunk;
				start[ 10 ] = 0xFF;
				start[ 11 ] = 0xD0;
			}
		}
		break;
	case BreakpointType::MemoryAccess:
		break;
	}
}

void UnsetBreakpoint( Breakpoint^ breakpoint )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;

	switch( breakpoint->Type )
	{
	case BreakpointType::Stepping:
	case BreakpointType::CodeExecute:
	case BreakpointType::BiosFunction:
		{
			CodeBlock* blocks[ 10 ];
			int blockCount = cpu->_codeCache->Search( ( int )breakpoint->Address, ( CodeBlock** )blocks );
			for( int n = 0; n < blockCount; n++ )
			{
				CodeBlock* block = blocks[ n ];
				int size;
				byte* start = FindInstructionStart( block, breakpoint->Address, &size );

				memset( start, 0x90, 12 );
			}
		}
		break;
	case BreakpointType::MemoryAccess:
		break;
	}
}
