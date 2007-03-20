// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "Tracer.h"
#include "R4000BlockBuilder.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000GenContext.h"
#include "R4000Cache.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

extern uint _jumpBlockThunkHits;

void __fixupBlockJump( void* sourceAddress, int newTarget );
void __missingBlockThunk( void* targetAddress, void* stackPointer );

R4000BlockBuilder::R4000BlockBuilder( R4000Cpu^ cpu, R4000Core^ core )
{
	_cpu = cpu;
	_core = core;
	_memory = ( R4000Memory^ )_cpu->Memory;
	_codeCache = _cpu->CodeCache;

	_gen = _cpu->_context->Generator;
#ifndef GENECHOFILE
	//R4000Generator::disableListing();
#endif

#ifdef GENECHOFILE
	//_gen->setEchoFile( GENECHOFILE );
#endif

	_ctx = _cpu->_context;

	// LOL confusing
	_ctx->CtxPointer = ( void* )_cpu->_ctx;
}

R4000BlockBuilder::~R4000BlockBuilder()
{
	SAFEDELETE( _gen );
}

#ifdef _DEBUG

static bool debugToggle = false;
void __runtimeDebugPrint( int address, int code )
{
	// Controller
	//if( address == 0x089062B4 ) // after video mode change
	//	debugToggle = true;
	//if( address == 0x08900694 ) // after screen clear
	//	debugToggle = true;

	if( debugToggle == true )
		Debug::WriteLine( String::Format( "[0x{0:X8}]: {1:X8}", address, code ) );
	//Debug::WriteLine( String::Format( "reg 31: {0:X8}", ( ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx )->Registers[ 31 ] ) );
}

void __runtimeDebugPrintForce( int address, int code )
{
	Debug::WriteLine( String::Format( "[0x{0:X8}]: {1:X8}", address, code ) );
}

void __runtimeRegsPrint()
{
	R4000Ctx* ctx = ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx;
	StringBuilder^ sb = gcnew StringBuilder();
	for( int n = 0; n < 32; n++ )
		sb->AppendFormat( "{0}={1:X8} ", n, ctx->Registers[ n ] );
	Debug::WriteLine( sb->ToString() );
}

#endif

#ifdef TRACE
void __traceRegs()
{
	R4000Ctx* ctx = ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx;
	StringBuilder^ sb = gcnew StringBuilder();
	for( int n = 0; n < 32; n++ )
		sb->AppendFormat( "{0}={1:X8} ", n, ctx->Registers[ n ] );
	sb->AppendLine();
	String^ str = sb->ToString();
	const char* str2 = ( char* )( void* )Marshal::StringToHGlobalAnsi( str );
	Tracer::WriteLine( str2 );
	Marshal::FreeHGlobal( ( IntPtr )( void* )str2 );
}

void __traceFpuRegs()
{
	R4000Ctx* ctx = ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx;
	StringBuilder^ sb = gcnew StringBuilder();
	for( int n = 0; n < 32; n++ )
		sb->AppendFormat( "{0}={1} ", n, ctx->Cp1Registers[ n * 4 ] );
	sb->AppendLine();
	String^ str = sb->ToString();
	const char* str2 = ( char* )( void* )Marshal::StringToHGlobalAnsi( str );
	Tracer::WriteLine( str2 );
	Marshal::FreeHGlobal( ( IntPtr )( void* )str2 );
}

#pragma unmanaged
static bool traceToggle = false;
void __traceLine( int address, int code )
{
#ifdef TRACEAFTER
	if( address == TRACEAFTER )
		traceToggle = true;
	else if( traceToggle == false )
		return;
#endif
	char buffer[ 50 ];
	sprintf_s( buffer, 50, "[0x%08X]: %08X\r\n", address, code );
	Tracer::WriteLine( buffer );
#ifdef TRACEREGISTERS
	__traceRegs();
#endif
#ifdef TRACEFPUREGS
	__traceFpuRegs();
#endif
}

void __flushTrace()
{
	Tracer::Flush();
}

#pragma managed
#endif /* TRACE */

void R4000BlockBuilder::EmitTrace( int address, int code )
{
#ifdef TRACE
	_gen->push( ( uint )code );
	_gen->push( ( uint )address );
	_gen->call( ( int )__traceLine );
	_gen->add( _gen->esp, 8 );
#endif
}

void R4000BlockBuilder::EmitDebug( int address, int code, char* codeString )
{
#ifdef GENECHOFILE
	_gen->annotate( "[%#08X]: %08X\t\t%s", address, code, codeString );
#endif

#ifdef RUNTIMEDEBUG
	_gen->push( ( uint )code );
	_gen->push( ( uint )address );

	_gen->call( ( int )__runtimeDebugPrint );

	_gen->add( _gen->esp, 8 );
#endif

#ifdef RUNTIMEREGS
	_gen->call( ( int )__runtimeRegsPrint );
#endif
}

/* Execution:
   Blocks require that esp + 4 always point to the R4000Ctx structure. This structure contains
   everything needed for execution.
*/

CodeBlock^ R4000BlockBuilder::Build( int address )
{
#ifdef STATISTICS
	double blockStart = _cpu->_timer->Elapsed;
#endif

	address &= 0x3FFFFFFF;

	CodeBlock^ block = gcnew CodeBlock();
	block->Address = address;

	// Don't try to re-add blocks
	Debug::Assert( _codeCache->Find( address ) == nullptr );

#ifdef CLEARECHOFILE
	_gen->clearEchoFile();
#endif

#ifdef GENECHOFILE
	_gen->annotate( "Block @ [%#08X]: ----------------------------------------------------------", address );
#endif

	block->InstructionCount = InternalBuild( address, block );

	FunctionPointer ptr = _gen->GenerateCode();
	block->Pointer = ptr;
	block->Size = _gen->GetLength();

#ifdef _DEBUG
	// Listing
	//const char *listing = _gen->getListing();
	//Debug::WriteLine( String::Format( "{0:X8}:\n", address ) + gcnew String( listing ) );
#endif

	// Reset so the generator is usable next build
	_gen->Reset();

	_codeCache->Add( block );

#ifdef STATISTICS
	R4000Statistics^ stats = _cpu->_stats;
	stats->CodeBlocksGenerated++;

	double ratio = block->Size / ( block->InstructionCount * 4 );
	stats->AverageCodeSizeRatio = ( stats->AverageCodeSizeRatio * .5 ) + ( ratio * .5 );

	double genTime = _cpu->_timer->Elapsed - blockStart;
	if( genTime <= 0.0 )
		genTime = 0.000001;
	stats->AverageGenerationTime += genTime;

	//Debug::WriteLine( String::Format( "gen block at 0x{0:X8} took {1}s ({2} instructions)", address, genTime, block->InstructionCount ) );

	stats->AverageCodeBlockLength += block->InstructionCount;
	if( block->InstructionCount > stats->LargestCodeBlockLength )
		stats->LargestCodeBlockLength = block->InstructionCount;
#endif

	return block;
}

// The bounce function takes an integer address, sets up the stack, and jumps there.
// It then cleans things up when done.
void* R4000BlockBuilder::BuildBounce()
{
	//int bouncefn( int targetAddress );

	_gen->push( _gen->ebp );
	_gen->mov( _gen->ebp, _gen->esp );

	_gen->push( _gen->eax );

	_gen->mov( _gen->eax, _gen->dword_ptr[ _gen->esp + 12 ] ); // target address

	// Nasty, but oh well - note we do this after the above command so we can get those values first
	_gen->pushad();
	
	//_gen->int3();
	_gen->call( _gen->eax );

	_gen->popad();

	_gen->pop( _gen->eax );

	_gen->mov( _gen->esp, _gen->ebp );
	_gen->pop( _gen->ebp );

	// This assumes caller address on top of the stack, which it should be
	_gen->ret();

	FunctionPointer ptr = _gen->GenerateCode();
	_gen->Reset();

	return ptr;
}

/* There are two ways I can think of that would be good for jumping to new code that DOESN'T require
   a dispatch loop. Both methods will insert a JMP to the target address if it is cached, otherwise
   they will insert a call to a thunk. The behavior of the thunk is what differs:
   1) The thunk method generates the code then goes back and fixes up the calling address to JMP to
      the method directly instead of calling the thunk. Finally, the thunk will JMP to the newly
	  generated method. This thunk is CALL'ed.
	  Adv:
	   - Fastest method, I think - in the steady state there would be no re-entrance back in to
	     application (managed) code
      Dis:
       - After replacement there will be a lot of NOPs
	   - Can't use SoftWire to generate the jump blocks, as code length/etc needs to be determined
	     at generation time
	   - Need a way to find the location(s) in the method that make the call
   2) The thunk method will look up the target method to execute and execute it. This thunk is
      not CALL'ed, but JMP'ed.
      Adv:
	   - Easier than 1)
	     - Can use SoftWire to generate jump blocks
		 - No messing with machine code
	  Dis:
	   - Slower as a call and native-to-managed switch (and switchback) is required for each jump

    For the momement I have chosen to implement 1). If I find that there are an excessive number
	of NOPs I will implement 2) and profile both. I think in the long run 1) will be better.
*/

/* Format for a jump that has not been cached (as per method 1 above):
   PUSH ESP				54					push stack pointer
   PUSH NNNNNNNN		68 NN NN NN NN		push target address
   MOV EAX, NNNNNNNN	B8 NN NN NN NN		eax = __missingBlockThunk address
   CALL					FF D0				call eax
       13 bytes [THUNKJUMPSIZE]

   This is replaced with the following code:
   MOV EAX, NNNNNNNN	B8 NN NN NN NN		eax = address of target method
   JMP EAX				FF E0				jump to target
   [NOP padding until same as above]
       7 bytes [THUNKJUMPNEWSIZE]
	     + 6 byte pad (0x90 = NOP)
*/
#define THUNKJUMPSIZE		13
#define THUNKJUMPNEWSIZE	7

void R4000BlockBuilder::EmitJumpBlock( int targetAddress )
{
	_gen->db( 0x54 );
	_gen->db( 0x68 ); _gen->dd( targetAddress );				// PUSH targetAddress
	_gen->db( 0xB8 ); _gen->dd( ( int )__missingBlockThunk );	// MOV EAX, &__missingBlockThunk
	_gen->db( 0xFF ); _gen->db( 0xD0 );							// CALL EAX
}

// This method is really tricky. When we emit a method and a jump target is
// not cached we add the thunk call (see below), but after something has
// been generated we don't want to be calling the thunk every time. This
// method will go and replace the callers thunk call with a direct jump
// to the generated block.
#pragma unmanaged
void __fixupBlockJump( void* sourceAddress, int newTarget )
{
	// Starting point is 15 bytes back (before all instructions)
	byte* startPtr = ( byte* )sourceAddress - THUNKJUMPSIZE;

	// NOP out stuff we wont write over
	memset( (void*)( startPtr + THUNKJUMPNEWSIZE ), 0x90, THUNKJUMPSIZE - THUNKJUMPNEWSIZE );
	
	// Store target
	*startPtr = 0xB8 + 0; // MOV+rd (+rd for EAX = 0)
	startPtr++;
	int* istartPtr = ( int* )startPtr;
	*istartPtr = newTarget;
	startPtr += 4;

	// Add jump to real target
	*startPtr = 0xFF; // JMP opcode
	startPtr++;
	*startPtr = 0xE0; // ModRM = source EAX
	//startPtr++;
}
#pragma managed

// This method is called by generated code when the target block is not found.
// Here we try to look it up and see if we can find the block, and if not we
// generate it. Once we do that, we go back and fix up the caller.
int __missingBlockThunkM( void* sourceAddress, void* targetAddress, void* stackPointer )
{
	R4000BlockBuilder^ builder = R4000Cpu::GlobalCpu->_builder;
	Debug::Assert( builder != nullptr );

#ifdef STATISTICS
	R4000Cpu::GlobalCpu->_stats->JumpBlockThunkCalls++;
#endif

	CodeBlock^ targetBlock = builder->_codeCache->Find( ( int )targetAddress );
	if( targetBlock == nullptr )
	{
		// Not found, must build
		targetBlock = builder->Build( ( int )targetAddress );
		Debug::Assert( targetBlock != nullptr );

#ifdef STATISTICS
		R4000Cpu::GlobalCpu->_stats->JumpBlockThunkBuilds++;
#endif
	}
	else
	{
		// Found, just need to do the patchup below

#ifdef STATISTICS
		R4000Cpu::GlobalCpu->_stats->JumpBlockThunkHits++;
#endif
	}

	return ( int )targetBlock->Pointer;
}

#ifdef __cplusplus
#define EXTERNC extern "C"
#else
#define EXTERNC 
#endif

EXTERNC void * _AddressOfReturnAddress ( void );
EXTERNC void * _ReturnAddress ( void );

#pragma intrinsic ( _AddressOfReturnAddress )
#pragma intrinsic ( _ReturnAddress )

// Unmanaged portion of the thunk
#pragma unmanaged
void __missingBlockThunk( void* targetAddress, void* stackPointer )
{
	void* sourceAddress = _ReturnAddress();

	// We try to use the unmanaged fast QPL in the code cache first
	int jumpTarget = QuickPointerLookup( ( int )targetAddress );
	if( jumpTarget == NULL )
	{
		// If we failed to get the pointer, it probably doesn't exist
		jumpTarget = __missingBlockThunkM( sourceAddress, targetAddress, stackPointer );
	}
	else
	{
#ifdef STATISTICS
		_jumpBlockThunkHits++;
#endif
	}

	// Fixup to target
	__fixupBlockJump( sourceAddress, jumpTarget );

	// We cannot do RET, so need to do jump, but must fix up stack pointer first
	__asm
	{
		mov esp, stackPointer
		mov eax, jumpTarget
		jmp eax
	}
}
#pragma managed
