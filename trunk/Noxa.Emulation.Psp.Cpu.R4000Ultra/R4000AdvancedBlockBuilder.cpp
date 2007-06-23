// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "DebugOptions.h"
#include "TraceOptions.h"
#include "R4000BlockBuilder.h"
#include "R4000AdvancedBlockBuilder.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Generator.h"
#include "R4000Hook.h"
#include "R4000Vfpu.h"
#include "Tracer.h"

using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

// #ifdef GENECHOFILE || VERBOSEANNOTATE
#ifdef GENECHOFILE
#define EMITDEBUG
#endif
#ifdef VERBOSEANNOTATE
#ifndef EMITDEBUG
#define EMITDEBUG
#endif
#endif

// If defined, a whole bunch of info will be printed out
//#define VERBOSEBUILD

// Maximum number of instructions per block - things are NOT handled
// properly when the code hits this limit, so it should be high!
#define MAXCODELENGTH 200

// Debugging addresses
//#define BREAKADDRESS1		0x08909ECC
//#define BREAKADDRESS2		0x08900160
//#define GENBREAKADDRESS		0x08891034

extern uint _instructionsExecuted;
extern uint _codeBlocksExecuted;
extern uint _jumpBlockInlineHits;
extern uint _jumpBlockInlineMisses;

extern uint _jumpBlockInlineCount;
extern uint _jumpBlockThunkCount;
extern uint _jumpBlockLookupCount;
extern uint _codeBlockRetCount;

void __flushTrace();

#ifdef DEBUGGING
// In R4000Controller
void SetBreakpoint( Breakpoint^ breakpoint );
#endif

#define ENDADDRESS ( startAddress + ( maxCodeLength << 2 ) )

R4000AdvancedBlockBuilder::R4000AdvancedBlockBuilder( R4000Cpu^ cpu, R4000Core^ core )
	: R4000BlockBuilder( cpu, core )
{
}

R4000AdvancedBlockBuilder::~R4000AdvancedBlockBuilder()
{
}

int R4000AdvancedBlockBuilder::InternalBuild( int startAddress, CodeBlock* block )
{
	int count = 0;
	int endAddress = startAddress;
	_ctx->Reset( startAddress );

#ifdef GENDEBUG
	Debug::WriteLine( String::Format( "!- Generating block at 0x{0:X8}:", startAddress ) );
#endif

	R4000Generator *g = _gen;

	bool jumpDelay = false;
	GenerationResult lastResult = GenerationResult::Success;

#ifdef EMITDEBUG
	char codeString[ 150 ];
#endif

	byte* mainMemory = _memory->MainMemory;

#ifdef DEBUGGING
	List<int>^ breakpoints = gcnew List<int>();
#endif

	int maxCodeLength = MAXCODELENGTH;
	for( int pass = 0; pass <= 1; pass++ )
	{
		if( pass == 1 )
		{
			GeneratePreamble();

#ifdef DEBUGGING
			// We know the length, so allocate the size buffer
			block->InstructionSizes = ( ushort* )malloc( sizeof( ushort ) * count );
#endif
		}

		// Regarding maxCodeLength, we need to be able to pad out block lengths by 1 in
		// the case of hitting the max while in a delay slot. Nasty, but it should work

		Label* nullDelayLabel = NULL;
		_ctx->InDelay = false;
		_ctx->JumpTarget = NULL;
		_ctx->JumpRegister = 0;
		bool breakOut = false;
		bool maxLengthHit = false;
		bool checkNullDelay = false;
		int address = startAddress;
		
#ifdef DEBUGGING
		// Used for sizing - we get the length now because the preamble has stuff
		int lastOffset = 0;
		if( pass == 1 )
		{
			lastOffset = _gen->GetLength();
			block->PreambleSize = lastOffset;
		}
#endif

		for( int n = 0; n < maxCodeLength; n++ )
		{
			GenerationResult result = GenerationResult::Invalid;

			//Debug::Assert( n < maxCodeLength - 1 );
			maxLengthHit = ( n == maxCodeLength - 1 );

			bool inDelay = _ctx->InDelay;
			uint code = *( ( uint* )( mainMemory + ( address - MainMemoryBase ) ) );

#if _DEBUG
			// Debug breakpoint on instruction
#ifdef GENBREAKADDRESS
			if( address == GENBREAKADDRESS ) Debugger::Break();
#endif
#ifdef BREAKADDRESS1
			if( pass == 1 ){ if( address == BREAKADDRESS1 ){
#ifdef TRACE
				g->call( ( uint )&__flushTrace );
#endif
				g->int3();
			} }
#endif
#ifdef BREAKADDRESS2
			if( pass == 1 ){ if( address == BREAKADDRESS2 ){
#ifdef TRACE
				g->call( ( uint )&__flushTrace );
#endif
				g->int3();
			} }
#endif
#endif

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				nullDelayLabel = g->DefineLabel();
				g->mov( EAX, MNULLDELAY( CTXP( _ctx->CtxPointer ) ) );
				g->cmp( EAX, 1 );
				g->je( nullDelayLabel );
			}

			if( pass == 0 )
			{
				// Mark labels
				LabelMarker^ lm;
				if( _ctx->BranchLabels->TryGetValue( address, lm ) == true )
				{
#ifdef VERBOSEBUILD
					Debug::WriteLine( String::Format( "Marking label for branch target {0:X8}", address ) );
#endif
					lm->Found = true;
				}
			}
			else if( pass == 1 )
			{
				// Shouldn't be here if in a delay slot
				LabelMarker^ lm;
				if( _ctx->BranchLabels->TryGetValue( address, lm ) == true )
				{
#ifdef VERBOSEBUILD
					Debug::WriteLine( String::Format( "Marking label for branch target {0:X8}", address ) );
#endif
					_gen->MarkLabel( lm->Label );
				}
			}

#ifdef DEBUGGING
			if( pass == 1 )
			{
				// We only need accurate PC tracking if we are debugging
				// This may be a bit overzealous, but it's easier than finding all the instances
				// that need an accurate PC and doing it then
				//g->mov( MPC( ( byte* )_ctx->CtxPointer ), address );
			}
#endif

#ifdef DEBUGGING
			if( pass == 1 )
			{
				// Add debug thunk space - 12 bytes
				Debug::Assert( DEBUGTHUNKSIZE == 12 );
				g->dd( ( uint )0x90909090 );
				g->dd( ( uint )0x90909090 );
				g->dd( ( uint )0x90909090 );

				// See if this instruction has a breakpoint defined
				int breakpointId;
				if( _cpu->_hook->BreakpointLookup->TryGetValue( address, breakpointId ) == true )
					breakpoints->Add( breakpointId );
			}
#endif

#ifdef STATISTICS
			if( pass == 1 )
			{
				// Instruction counter increment - note that it has to be here cause
				// of null delay and the label marker above
				g->add( g->dword_ptr[ &_instructionsExecuted ], 1 );
			}
#endif
#ifdef TRACE
			if( pass == 1 )
				this->EmitTrace( address, code );
#endif

			if( code != 0 )
			{
				uint opcode = ( code >> 26 ) & 0x3F;
				bool isCop = ( opcode == 0x10 ) || ( opcode == 0x11 ) || ( opcode == 0x12 );

				if( isCop == false )
				{
					switch( opcode )
					{
					case 0: // R type
						{
							byte function = ( byte )( code & 0x3F );
							byte rs = ( byte )( ( code >> 21 ) & 0x1F );
							byte rt = ( byte )( ( code >> 16 ) & 0x1F );
							byte rd = ( byte )( ( code >> 11 ) & 0x1F );
							byte shamt = ( byte )( ( code >> 6 ) & 0x1F );

							GenerateInstructionR instr = R4000Generator::TableR[ function ];
#ifdef EMITDEBUG
							if( pass == 1 )
							{
								const char* name = R4000Generator::TableR_n[ function ];
								sprintf_s( codeString, 150, "%s rs:%d rt:%d rd:%d shamt:%d", name, rs, rt, rd, shamt );
								this->EmitDebug( address, code, codeString );
							}
#endif
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, rs, rt, rd, shamt, function );
						}
						break;
					case 1: // J type
						{
							uint imm = code & 0x03FFFFFF;
							uint rt = ( code >> 16 ) & 0x1F;

							GenerateInstructionJ instr = R4000Generator::TableJ[ rt ];
#ifdef EMITDEBUG
							if( pass == 1 )
							{
								const char* name = R4000Generator::TableJ_n[ rt ];
								sprintf_s( codeString, 150, "%s imm:%d (%#08X)", name, imm, imm );
								this->EmitDebug( address, code, codeString );
							}
#endif
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, imm );
						}
						break;
					case 0x1C: // Allegrex special type (follows R convention)
						{
							byte function = ( byte )( code & 0x3F );
							byte rs = ( byte )( ( code >> 21 ) & 0x1F );
							byte rt = ( byte )( ( code >> 16 ) & 0x1F );
							byte rd = ( byte )( ( code >> 11 ) & 0x1F );

							GenerateInstructionR instr = R4000Generator::TableAllegrex[ function ];
#ifdef EMITDEBUG
							if( pass == 1 )
							{
								const char* name = R4000Generator::TableAllegrex_n[ function ];
								sprintf_s( codeString, 150, "%s rs:%d rt:%d rd:%d", name, rs, rt, rd );
								this->EmitDebug( address, code, codeString );
							}
#endif
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, rs, rt, rd, 0, function );
						}
						break;
					case 0x1F: // SPECIAL3 type
						{
							byte rt = ( byte )( ( code >> 16 ) & 0x1F );
							byte rd = ( byte )( ( code >> 11 ) & 0x1F );
							byte function = ( byte )( ( code >> 6 ) & 0x1F );
							uint bshfl = code & 0x3F;

							// Annoying....
							GenerateInstructionSpecial3 instr;
							if( ( bshfl == 0x0 ) ||
								( bshfl == 0x4 ) )
								instr = R4000Generator::TableSpecial3[ bshfl ];
							else
								instr = R4000Generator::TableSpecial3[ function ];
#ifdef EMITDEBUG
							if( pass == 1 )
							{
								const char* name;
								if( ( bshfl == 0x0 ) ||
									( bshfl == 0x4 ) )
									name = R4000Generator::TableSpecial3_n[ bshfl ];
								else
									name = R4000Generator::TableSpecial3_n[ function ];
								sprintf_s( codeString, 150, "%s rt:%d rd:%d func:%d bshfl:%d", name, rt, rd, function, bshfl );
								this->EmitDebug( address, code, codeString );
							}
#endif
							result = instr( _ctx, pass, address + 4, code, rt, rd, function, ( ushort )bshfl );
						}
						break;
					default: // Common I type
						{
							byte rs = ( byte )( ( code >> 21 ) & 0x1F );
							byte rt = ( byte )( ( code >> 16 ) & 0x1F );
							ushort imm = ( ushort )( code & 0xFFFF );

							GenerateInstructionI instr = R4000Generator::TableI[ opcode ];
#ifdef EMITDEBUG
							if( pass == 1 )
							{
								const char* name = R4000Generator::TableI_n[ opcode ];
								sprintf_s( codeString, 150, "%s rs:%d rt:%d imm:%d (%#08X)", name, rs, rt, SE( imm ), SE( imm ) );
								this->EmitDebug( address, code, codeString );
							}
#endif
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, rs, rt, imm );
						}
						break;
					}
				}
				else
				{
					// COP instructions

					uint copop = code >> 28;
					byte cop = ( byte )( ( code >> 26 ) & 0x3 ); // cop0, cop1, or cop2
					byte rs = ( byte )( ( code >> 21 ) & 0x1F );
					byte rt = ( byte )( ( code >> 16 ) & 0x1F );
					byte rd = ( byte )( ( code >> 11 ) & 0x1F );
					ushort imm = ( ushort )( code & 0xFFFF );

					// rs = bc sub-opcode
					// rt = branch condition

					switch( copop )
					{
					case 0x4:
						if( ( ( code >> 25 ) & 0x1 ) == 1 )	// COPz
						{
							uint cofun = code & 0x1FFFFFF;

							if( cop == 1 )
							{
								byte fmt = ( byte )( ( cofun >> 21 ) & 0x1F );
								// 0 = S = single binary fp
								// 1 = D = double binary fp
								// 4 = W = single 32 binary fixed point
								// 5 = L = longword 64 binary fixed point
								Debug::Assert( ( fmt != 1 ) && ( fmt != 5 ) );
								byte ft = rt; // source 2
								byte fs = rd; // source 1
								byte fd = ( byte )( ( code >> 6 ) & 0x1F ); // dest
								byte func = ( byte )( code & 0x3F );

								GenerateInstructionFpu instr = R4000Generator::TableFpu[ func ];
#ifdef EMITDEBUG
								if( pass == 1 )
								{
									const char* name = R4000Generator::TableFpu_n[ func ];
									sprintf_s( codeString, 150, "%s fmt:%d fs:%d ft:%d fd:%d ", name, fmt, fs, ft, fd );
									this->EmitDebug( address, code, codeString );
								}
#endif
								result = instr( _ctx, pass, address + 4, code, fmt, fs, ft, fd, func );
							}
							else
							{
								Log::WriteLine( Verbosity::Critical, Feature::Cpu, "InternalBuild(0x{2:X8}): attempted COP{0} function {1:X8}", cop, cofun, address );
								Debug::WriteLine( String::Format( "InternalBuild(0x{2:X8}): attempted COP{0} function {1:X8}", cop, cofun, address ) );
								result =  GenerationResult::Invalid;
							}
						}
						else
						{
							if( rs == 0x08 )
							{
								GenerateInstructionI instr = R4000Generator::TableCopB[ rt ];
#ifdef EMITDEBUG
								if( pass == 1 )
								{
									const char* name = R4000Generator::TableCopB_n[ rt ];
									sprintf_s( codeString, 150, "%s cop:%d imm:%d ", name, cop, SE( imm ) );
									this->EmitDebug( address, code, codeString );
								}
#endif
								result = instr( _ctx, pass, address + 4, code, cop, rs, rt, imm );
							}
							else
							{
								GenerateInstructionI instr = R4000Generator::TableCopA[ rs ];
#ifdef EMITDEBUG
								if( pass == 1 )
								{
									const char* name = R4000Generator::TableCopA_n[ rs ];
									sprintf_s( codeString, 150, "%s cop:%d rd:%d rt:%d imm:%d ", name, cop, rd, rt, SE( imm ) );
									this->EmitDebug( address, code, codeString );
								}
#endif
								result = instr( _ctx, pass, address + 4, code, cop, rd, rt, imm );
							}
						}
						break;
					}
				}
			}
			else
			{
				// NOP
				result = GenerationResult::Success;
#ifdef EMITDEBUG
				if( pass == 1 )
				{
					sprintf_s( codeString, 150, "NOP" );
					this->EmitDebug( address, code, codeString );
				}
#endif
			}

			if( result == GenerationResult::Invalid )
			{
				// Check against VFPU
				result = TryEmitVfpu( _ctx, pass, address + 4, code );
				
				// If still an error after that (means no VFPU impl found), error out
				if( result == GenerationResult::Invalid )
				{
					String^ errorMessage = String::Format( "InternalBuild(0x{0:X8}): failed to generate code for [0x{1:X8}] {2:X8}", startAddress, address, code );
					Log::WriteLine( Verbosity::Critical, Feature::Cpu, errorMessage );
					CpuError^ error = gcnew CpuError( CpuErrorCode::Generation, errorMessage, address );
					Diag::ThrowError( error );

					//Debug::WriteLine( errorMessage );
					//Debugger::Break();

					break;
				}
			}

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				Label* nullDelaySkipLabel = g->DefineLabel();

				g->jmp( nullDelaySkipLabel );

				g->MarkLabel( nullDelayLabel );
				g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );

				g->MarkLabel( nullDelaySkipLabel );

				checkNullDelay = false;
			}

			if( pass == 0 )
			{
				count++;
				endAddress = address;
				_ctx->EndAddress = endAddress;
			}

			address += 4;

			switch( result )
			{
			case GenerationResult::Jump:
			case GenerationResult::JumpNoBreakout:
			case GenerationResult::Branch:
			case GenerationResult::BranchAndNullifyDelay:
				_ctx->UpdatePC = true;
				if( pass == 0 )
				{
					if( maxLengthHit == true )
					{
						// Handling for max block size padding
						// By upping this, we will loop once more before ending!
#ifdef VERBOSEBUILD
						Debug::WriteLine( String::Format( "Max code length {0} hit at 0x{2:X8}, but block would end on br/j, so extending to {1}",
							maxCodeLength, maxCodeLength + 1, address - 4 ) );
#endif
						maxCodeLength++;
						maxLengthHit = false;
					}
				}
				break;
			case GenerationResult::Syscall:
				_ctx->UseSyscalls = true;
				break;
			}

			if( pass == 1 )
			{
				// Have to use local inDelay because the last instruction could have been the one to set it
				// Could also be in a jump delay, which only happens on non-breakout jumps
				if( jumpDelay == true )
				{
#ifdef VERBOSEBUILD
					Debug::WriteLine( String::Format( "Marking jump delay tail - target: 0x{0:X8}", _ctx->JumpTarget ) );
#endif
					// We may be doing a JR and not have a known target at gen time
					if( _ctx->JumpTarget != NULL )
						GenerateTail( address - 4, true, _ctx->JumpTarget );
					else if( _ctx->JumpRegister != 0 )
					{
						//Debug::WriteLine( "Would be doing a tail gen w/o jump thunk, but doing new jump instead!" );
						g->mov( EAX, MREG( CTXP( _ctx->CtxPointer ), _ctx->JumpRegister ) );
						GenerateTail( address - 4, true, -1 );
					}
					else
					{
						// Cannot do jump because we don't know and can't know where we are going ---- why?
						//Debug::Assert( false );
#ifdef GENDEBUG
						Debug::WriteLine( String::Format( "InternalBuild(0x{0:X8}): Full tail (no jumptarget/jumpregister)", address - 4 ) );
#endif
						GenerateTail( address - 4, false, 0 );
					}

					_ctx->JumpTarget = NULL;
					_ctx->JumpRegister = 0;
					_ctx->InDelay = false;
					jumpDelay = false;
				}
				else if( inDelay == true )
				{
					LabelMarker^ lm = _ctx->BranchTarget;
					Debug::Assert( lm != nullptr );

					if( ( _ctx->IsBranchLocal( lm->Address ) == true ) &&
						( lm->Address <= ENDADDRESS ) )
					{
						g->cmp( MPCVALID( CTXP( _ctx->CtxPointer ) ), 1 );
						g->mov( MPCVALID( CTXP( _ctx->CtxPointer ) ), 0 );
						g->je( lm->Label );
					}
					else
					{
#ifdef VERBOSEBUILD
						Debug::WriteLine( String::Format( "Aborting block early because branch target {0:X8} not found", lm->Address ) );
#endif
						Debug::Assert( lm->Address != 0 );

						Label* noBranch = g->DefineLabel();

						g->cmp( MPCVALID( CTXP( _ctx->CtxPointer ) ), 1 );
						//g->mov( MPCVALID(), 0 ); - keep valid because PC is set
						g->jne( noBranch );
						g->mov( MPC( CTXP( _ctx->CtxPointer ) ), lm->Address );

						// Generate tail to bounce to target block (jump block/etc)
						GenerateTail( address - 4, true, lm->Address );

						g->MarkLabel( noBranch );
					}

					_ctx->InDelay = false;
					_ctx->BranchTarget = nullptr;
				}
			}

#ifdef DEBUGGING
			if( pass == 1 )
			{
				// Sizing information
				int newOffset = _gen->GetLength();
				int lastSize = ( newOffset - lastOffset );
				Debug::Assert( lastSize < ushort::MaxValue );
				block->InstructionSizes[ n ] = lastSize;
				lastOffset = newOffset;
			}
#endif

			// Syscalls always exit
			if( result == GenerationResult::Syscall )
			{
				lastResult = result;
				break;
			}

			// For delay slots
			if( breakOut == true )
				break;

			switch( result )
			{
			case GenerationResult::Branch:
				_ctx->InDelay = true;
				lastResult = result;
				break;
			case GenerationResult::Jump:
				// This is tricky - if lastTargetPc > currentPc, don't break out
				if( ( _ctx->LastBranchTarget != 0x0 ) &&
					( _ctx->LastBranchTarget <= address ) )
				{
					// WARNING: THIS IS PROBABLY BROKEN!
					// Make sure that the delay handling is proper!

					// This is the last jump in the block - need to exit
#ifdef VERBOSEBUILD
					if( pass == 1 )
						Debug::WriteLine( String::Format( "Jump breakout at {0:X8}", address ) );
#endif
					// Should this be in pass==1 block?
					breakOut = true;
					lastResult = result;
				}
				else
				{
					// Remaining branches and such, so we can exit
					if( pass == 1 )
					{
#ifdef VERBOSEBUILD
						Debug::WriteLine( String::Format( "Ignoring jump breakout at {0:X8} because last target is {1:X8}", address, _ctx->LastBranchTarget ) );
#endif
						jumpDelay = true;
					}
					_ctx->InDelay = true;
				}
				break;
			case GenerationResult::JumpNoBreakout:
#ifdef VERBOSEBUILD
				Debug::WriteLine( String::Format( "JumpNoBreakout at {0:X8} - continuing", address ) );
#endif
				_ctx->InDelay = true;
				//lastResult = result;
				if( pass == 1 )
					jumpDelay = true;
				break;
			case GenerationResult::BranchAndNullifyDelay:
				_ctx->InDelay = true;
				if( pass == 1 )
					checkNullDelay = true;
				lastResult = result;
				break;
			}
		}

		if( pass == 1 )
		{
			if( ( lastResult == GenerationResult::Syscall ) &&
				( _ctx->LastSyscallStateless == false ) )
			{
				// We always return to the execution loop on syscalls
				GenerateTail( address - 4, false, 0 );
			}
			else
			{
				if( _ctx->JumpTarget != 0 )
					GenerateTail( address - 4, true, _ctx->JumpTarget );
				else if( _ctx->JumpRegister != 0 )
				{
					g->mov( EAX, MREG( CTXP( _ctx->CtxPointer ), _ctx->JumpRegister ) );
					GenerateTail( address - 4, true, -1 );
				}
				else
				{
					if( maxLengthHit == true )
					{
#ifdef VERBOSEBUILD
						Debug::WriteLine( String::Format( "Breaking out early due to max code length limit (ending on 0x{0:X8}, continuing to 0x{1:X8}", address - 4, address ) );
#endif
						// Special case of when we hit the upper bound of the max block size
						// We just generatetail to the next instruction
						_ctx->UpdatePC = false;
						GenerateTail( address - 4, true, address );
					}
					else
						GenerateTail( address - 4, false, 0 );
				}
			}
		}
	}

	block->EndsOnSyscall = ( lastResult == GenerationResult::Syscall );

	// Must be set for breakpoint searching
	block->Size = _gen->GetLength();
	block->InstructionCount = count;

	FunctionPointer ptr = _gen->GenerateCode();
	_cpu->_codeCache->UpdatePointer( block, ptr );

#ifdef DEBUGGING
	// Add breakpoints that are defined in this method
	for( int n = 0; n < breakpoints->Count; n++ )
	{
		Breakpoint^ bp = _cpu->_hook->Breakpoints[ breakpoints[ n ] ];
		SetBreakpoint( bp );
	}
#endif

#ifdef GENDEBUG
	Debug::WriteLine( String::Format( "!- Finished block at 0x{0:X8} ({1} instructions)", startAddress, count ) );
#endif

	return count;
}

#ifdef TRACESYMBOLS
void __traceMethod( int methodAddress, int currentAddress )
{
	String^ methodName;
	if( methodAddress == 0x0 )
	{
		methodName = "[UnknownMethod]";
	}
	else
	{
		Method^ method = R4000Cpu::GlobalCpu->_symbols->FindMethod( methodAddress );
		methodName = method->Name;
	}
	String^ str = String::Format( "\r\n{0} ({1:X8}): {2}\r\n", methodName, methodAddress,
		( methodAddress != currentAddress ) ? String::Format( "entered at {0:X8}", currentAddress ) : "" );
	const char* str2 = ( char* )( void* )Marshal::StringToHGlobalAnsi( str );
	Tracer::WriteLine( str2 );
	Marshal::FreeHGlobal( ( IntPtr )( void* )str2 );
}
#endif

void R4000AdvancedBlockBuilder::GeneratePreamble()
{
	R4000Generator *g = _gen;

	g->mov( MPC( CTXP( _ctx->CtxPointer ) ), _ctx->StartAddress );
	g->mov( MPCVALID( CTXP( _ctx->CtxPointer ) ), 0 );
	g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );

#ifdef STATISTICS
	// Block count
	g->add( g->dword_ptr[ &_codeBlocksExecuted ], 1 );
#endif

//#if 0
#ifdef TRACESYMBOLS
	// Trace entry to method
	// Since we can't store an object reference, we store the real start address
	// of the method. This makes the lookup in FindMethod simpler than if we gave
	// it random addresses inside of methods.
	Method^ method = _cpu->_symbols->FindMethod( _ctx->StartAddress );
	g->push( ( uint )_ctx->StartAddress );
	if( method != nullptr )
		g->push( ( uint )method->EntryAddress );
	else
		g->push( ( uint )0 );
	g->call( ( uint )&__traceMethod );
	g->add( ESP, 8 );
#endif
}

void __updateCorePC( int newPc )
{
	//R4000Cpu::GlobalCpu->_core0->PC = newPc;
	//( ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx )->PC = newPc;
}

// If tailJump == true, targetAddress must either be a valid address or -1 - -1 implies that EAX has the address to jump to
void R4000AdvancedBlockBuilder::GenerateTail( int address, bool tailJump, int targetAddress )
{
	R4000Generator *g = _gen;

	// NOTE: EAX has jump target address if tailJump==true && targetAddress==-1 - DO NOT OVERWRITE
	// This is for the jr case

	// 1 = pc updated, 0 = pc update needed
	if( _ctx->UpdatePC == true )
	{
		// PC should be good
	}
	else
	{
		// PC was never touched (wow!) - need to update now ourselves
		int instructionLength = 4 * ( _ctx->EndAddress - _ctx->StartAddress ) ;
		g->mov( MPC( CTXP( _ctx->CtxPointer ) ), _ctx->StartAddress + instructionLength );
		g->mov( MPCVALID( CTXP( _ctx->CtxPointer ) ), 1 );
	}

	if( ( _ctx->UseSyscalls == false ) &&
		( tailJump == false ) )
	{
		// Store ctx PC back in to real ctx
		// This is only needed when we aren't tail jumping
		//g->push( MPC( CTXP( _ctx->CtxPointer ) ) );
		//g->call( ( uint )&__updateCorePC );
		//g->add( g->esp, 4 );
	}

	if( tailJump == true )
	{
		if( targetAddress == -1 )
		{
			Label* nullPtrLabel = g->DefineLabel();

			//g->int3();

			g->push( EAX );
			g->call( ( uint )&QuickPointerLookup );
			g->pop( EBX ); // used below, maybe
			//g->add( ESP, 4 );

			// EAX = NULL or address to jump to
			g->test( EAX, 0xFFFFFFFF );
			g->jz( nullPtrLabel );

#ifdef STATISTICS
			g->add( g->dword_ptr[ &_jumpBlockInlineHits ], 1 );
#endif

			// Jump!
			g->jmp( EAX );

			// NULL, so just return
			g->MarkLabel( nullPtrLabel );

#ifdef STATISTICS
			g->add( g->dword_ptr[ &_jumpBlockInlineMisses ], 1 );
#endif

			// We can just return and let the main loop take care of it
			//g->xor( EAX, EAX );
			//g->ret();
#ifdef VERBOSEBUILD
			Debug::WriteLine( "No target block, emitting tail jump block to register" );
#endif
			// EBX contains the user code address
			this->EmitJumpBlockEbx();

#ifdef STATISTICS
			_jumpBlockLookupCount++;
#endif
		}
		else
		{
			// Bounce out
			CodeBlock* block = _codeCache->Find( targetAddress );

			// Note we have to check the case of us bouncing to ourselves - this is bad
			// as at this point we don't have a pointer to ourselves, so we need to just write 0!
			if( ( block == NULL ) ||
				( block->Pointer == 0 ) )
			{
#ifdef VERBOSEBUILD
				Debug::WriteLine( String::Format( "Target block 0x{0:X8} not found, emitting jumpblock", targetAddress ) );
#endif

				// Not found - we write the missing block jump code
				this->EmitJumpBlock( targetAddress );

#ifdef STATISTICS
				_jumpBlockThunkCount++;
#endif
			}
			else
			{
#ifdef VERBOSEBUILD
				Debug::WriteLine( String::Format( "Target block 0x{0:X8} found, emitting direct jump", targetAddress ) );
#endif

				// Can do a direct jump to the translated block
				Debug::Assert( block->Pointer != NULL );
				g->jmp( ( int )block->Pointer );

#ifdef STATISTICS
				_jumpBlockInlineCount++;
#endif
			}
		}
	}
	else
	{
		// Return (to bounce)
		g->xor( EAX, EAX );
		g->ret();

#ifdef STATISTICS
		_codeBlockRetCount++;
#endif
	}
}