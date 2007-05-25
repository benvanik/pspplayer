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
#define DEBUG_RESUME_STEP		1	// param = next address to stop at
#define DEBUG_RESUME_SET_NEXT	2	// param = new address

HANDLE	_debugHandle;
int		_debugResumeMode;
uint	_debugResumeParam;

byte* FindInstructionStart( CodeBlock* block, uint address, int* size );
void SetBreakpoint( Breakpoint^ breakpoint );
void UnsetBreakpoint( Breakpoint^ breakpoint );
//void RestoreOriginal( Breakpoint^ breakpoint );
//void RestoreRecovery( Breakpoint^ breakpoint );

R4000Controller::R4000Controller( R4000Cpu^ cpu )
{
	this->Cpu = cpu;

	_debugHandle = CreateEvent( NULL, FALSE, FALSE, NULL );
	_debugResumeMode = DEBUG_RESUME_CONTINUE;
	_debugResumeParam = 0;
}

R4000Controller::~R4000Controller()
{
	CloseHandle( _debugHandle );
	_debugHandle = NULL;
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
		break;
	case BreakpointMode::Break:
		{
			// General process:
			// - replace debug thunk jump with original
			// - notify debugger
			// - wait on debug resume handle
			// - handle re-add/etc

			//RestoreOriginal( breakpoint );

			switch( breakpoint->Type )
			{
			case BreakpointType::Stepping:
				Diag::Instance->Client->Handler->OnStepComplete( breakpoint->Address );
				break;
			default:
				//Diag::Instance->Client->Handler->OnBreakpointHit( breakpoint->ID );
				break;
			}

			//WaitForSingleObject( _debugHandle, INFINITE );
			_debugResumeMode = DEBUG_RESUME_CONTINUE;

			switch( _debugResumeMode )
			{
			default:
			case DEBUG_RESUME_CONTINUE:
				// Nothing to do
				break;
			case DEBUG_RESUME_STEP:
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

				// Get original bytes
				/*if( breakpoint->Internal == nullptr )
					breakpoint->Internal = gcnew array<byte>( DEBUGTHUNKSIZE );
				pin_ptr<byte> pob = &( ( array<byte>^ )breakpoint->Internal )[ 0 ];
				memcpy( pob, start, DEBUGTHUNKSIZE );*/

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

//void RestoreOriginal( Breakpoint^ breakpoint )
//{
//	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
//
//	// Restoring a breakpoint that was never set?
//	Debug::Assert( breakpoint->Internal != nullptr );
//
//	switch( breakpoint->Type )
//	{
//	case BreakpointType::CodeExecute:
//	case BreakpointType::BiosFunction:
//		{
//			CodeBlock* blocks[ 10 ];
//			int blockCount = cpu->_codeCache->Search( ( int )breakpoint->Address, ( CodeBlock** )blocks );
//			for( int n = 0; n < blockCount; n++ )
//			{
//				CodeBlock* block = blocks[ n ];
//				int size;
//				byte* start = FindInstructionStart( block, breakpoint->Address, &size );
//
//				// Restore original
//				pin_ptr<byte> pob = &( ( array<byte>^ )breakpoint->Internal )[ 0 ];
//				memcpy( start, pob, DEBUGTHUNKSIZE );
//			}
//		}
//		break;
//	case BreakpointType::MemoryAccess:
//		break;
//	}
//}

//void RestoreRecovery( Breakpoint^ breakpoint )
//{
//	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
//
//	// Restoring a breakpoint that was never set?
//	Debug::Assert( breakpoint->Internal != nullptr );
//
//	switch( breakpoint->Type )
//	{
//	case BreakpointType::CodeExecute:
//	case BreakpointType::BiosFunction:
//		{
//			CodeBlock* blocks[ 10 ];
//			int blockCount = cpu->_codeCache->Search( ( int )breakpoint->Address, ( CodeBlock** )blocks );
//			for( int n = 0; n < blockCount; n++ )
//			{
//				CodeBlock* block = blocks[ n ];
//				int size;
//				byte* start = FindInstructionStart( block, breakpoint->Address, &size );
//
//				// Skip to the next instruction
//				start += size;
//
//				// Restore original
//				pin_ptr<byte> pob = &( ( array<byte>^ )breakpoint->Internal )[ 0 ];
//				memcpy( start, pob, DEBUGTHUNKSIZE );
//			}
//		}
//		break;
//	case BreakpointType::MemoryAccess:
//		break;
//	}
//}
