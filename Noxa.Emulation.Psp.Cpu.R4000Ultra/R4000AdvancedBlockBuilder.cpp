// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000BlockBuilder.h"
#include "R4000AdvancedBlockBuilder.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Generator.h"

#ifdef GENECHOFILE
#ifdef VERBOSEANNOTATE
#define EMITDEBUG
#endif
#endif

// If defined, a whole bunch of info will be printed out
//#define VERBOSEBUILD

// Maximum number of instructions per block - things are NOT handled
// properly when the code hits this limit, so it should be high!
#define MAXCODELENGTH 10000

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

R4000AdvancedBlockBuilder::R4000AdvancedBlockBuilder( R4000Cpu^ cpu, R4000Core^ core )
	: R4000BlockBuilder( cpu, core )
{
}

R4000AdvancedBlockBuilder::~R4000AdvancedBlockBuilder()
{
}

int R4000AdvancedBlockBuilder::InternalBuild( int startAddress, CodeBlock^ block )
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
	char nullDelayLabel[ 30 ];

	for( int pass = 0; pass <= 1; pass++ )
	{
		if( pass == 1 )
			GeneratePreamble();

		_ctx->InDelay = false;
		_ctx->JumpTarget = NULL;
		bool breakOut = false;
		bool checkNullDelay = false;
		int address = startAddress;
		for( int n = 0; n < MAXCODELENGTH; n++ )
		{
			GenerationResult result = GenerationResult::Invalid;

			Debug::Assert( n < MAXCODELENGTH - 1 );

			bool inDelay = _ctx->InDelay;
			uint code = ( uint )_memory->ReadWord( address );

			//this->EmitDebug( address, code );

#if _DEBUG
			// Debug breakpoint on instruction
			if( pass == 0 )
			{
				//if( address == 0x0890012C )
				//	Debugger::Break();
			}
			if( pass == 1 )
			{
				//if( address == 0x08900390 )
				//	g->int3();
			}
#endif

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				g->mov( EAX, MNULLDELAY( CTXP( _ctx->CtxPointer ) ) );
				g->cmp( EAX, 1 );
				sprintf_s( nullDelayLabel, 30, "l%Xnd", address );
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
					_gen->label( lm->Label );
				}
			}

#ifdef STATISTICS
			if( pass == 1 )
			{
				// Instruction counter increment - note that it has to be here cause
				// of null delay and the label marker above
				g->inc( MINSTRCOUNT( CTXP( _ctx->CtxPointer ) ) );
			}
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
								Debug::WriteLine( String::Format( "Cpu: attempted COP{0} function {1:X8}", cop, cofun ) );
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
				Debug::WriteLine( String::Format( "InternalBuild(0x{0:X8}): failed to generate code for [0x{1:X8}] {2:X8}", startAddress, address, code ) );
				Debugger::Break();
				break;
			}

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				sprintf_s( nullDelayLabel, 30, "l%Xnds", address );
				g->jmp( nullDelayLabel );

				sprintf_s( nullDelayLabel, 30, "l%Xnd", address );
				g->label( nullDelayLabel );

				g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );

				sprintf_s( nullDelayLabel, 30, "l%Xnds", address );
				g->label( nullDelayLabel );

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
			case GenerationResult::Branch:
			case GenerationResult::Jump:
			case GenerationResult::BranchAndNullifyDelay:
				_ctx->UpdatePC = true;
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
						GenerateTail( true, _ctx->JumpTarget );
					else
						GenerateTail( false, 0 );

					_ctx->JumpTarget = NULL;
					_ctx->InDelay = false;
					jumpDelay = false;
				}
				else if( inDelay == true )
				{
					LabelMarker^ lm = _ctx->BranchTarget;
					Debug::Assert( lm != nullptr );

					if( _ctx->IsBranchLocal( lm->Address ) == true )
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

						char noBranch[20];
						sprintf( noBranch, "l%Xnobr", address - 4 );

						g->cmp( MPCVALID( CTXP( _ctx->CtxPointer ) ), 1 );
						//g->mov( MPCVALID(), 0 ); - keep valid because PC is set
						g->jne( noBranch );
						g->mov( MPC( CTXP( _ctx->CtxPointer ) ), lm->Address );

						// Generate tail to bounce to target block (jump block/etc)
						GenerateTail( true, lm->Address );

						g->label( noBranch );
					}

					_ctx->InDelay = false;
					_ctx->BranchTarget = nullptr;
				}
			}

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
				if( _ctx->LastBranchTarget <= address )
				{
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
						_ctx->InDelay = true;
					}
				}
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
			GenerateTail( false, 0 );
	}

	block->EndsOnSyscall = ( lastResult == GenerationResult::Syscall );

#ifdef GENDEBUG
	Debug::WriteLine( String::Format( "!- Finished block at 0x{0:X8} ({1} instructions)", startAddress, count ) );
#endif

	return count;
}

void R4000AdvancedBlockBuilder::GeneratePreamble()
{
	R4000Generator *g = _gen;

	g->mov( MPC( CTXP( _ctx->CtxPointer ) ), _ctx->StartAddress );
	g->mov( MPCVALID( CTXP( _ctx->CtxPointer ) ), 0 );
	g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );
}

void __updateCorePC( int newPc )
{
	//R4000Cpu::GlobalCpu->_core0->PC = newPc;
	//( ( R4000Ctx* )R4000Cpu::GlobalCpu->_ctx )->PC = newPc;
}

void R4000AdvancedBlockBuilder::GenerateTail( bool tailJump, int targetAddress )
{
	R4000Generator *g = _gen;

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
		g->push( MPC( CTXP( _ctx->CtxPointer ) ) );
		g->call( (int)__updateCorePC );
		g->add( g->esp, 4 );
	}

	if( tailJump == true )
	{
		// Bounce out
		CodeBlock^ block = _ctx->_builder->_codeCache->Find( targetAddress );
		if( block == nullptr )
		{
#ifdef VERBOSEBUILD
			Debug::WriteLine( String::Format( "Target block 0x{0:X8} not found, emitting jumpblock", targetAddress ) );
#endif

			// Not found - we write the missing block jump code
			this->EmitJumpBlock( targetAddress );

#ifdef STATISTICS
			R4000Cpu::GlobalCpu->_stats->JumpBlockThunkCount++;
#endif
		}
		else
		{
#ifdef VERBOSEBUILD
			Debug::WriteLine( String::Format( "Target block 0x{0:X8} found, emitting direct jump", targetAddress ) );
#endif

			// Can do a direct jump to the translated block
			g->mov( EAX, ( int )block->Pointer );
			g->jmp( EAX );

#ifdef STATISTICS
			R4000Cpu::GlobalCpu->_stats->JumpBlockInlineCount++;
#endif
		}
	}
	else
	{
		// Return (to bounce)
		g->mov( EAX, 0 );
		g->ret();

#ifdef STATISTICS
		R4000Cpu::GlobalCpu->_stats->CodeBlockRetCount++;
#endif
	}
}