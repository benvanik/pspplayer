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

#define MAXCODELENGTH 100

int R4000AdvancedBlockBuilder::InternalBuild( int startAddress )
{
	int count = 0;
	int endAddress = startAddress;
	_ctx->Reset( startAddress );

	Debug::WriteLine( String::Format( "!- Generating block at 0x{0:X8}:", startAddress ) );

	R4000Generator *g = _gen;

	bool jumpDelay = false;
	GenerationResult lastResult = GenerationResult::Success;

	for( int pass = 0; pass <= 1; pass++ )
	{
		if( pass == 1 )
			GeneratePreamble();

		_ctx->InDelay = false;
		bool breakOut = false;
		bool checkNullDelay = false;
		int address = startAddress;
		for( int n = 0; n < MAXCODELENGTH; n++ )
		{
			GenerationResult result = GenerationResult::Invalid;

			bool inDelay = _ctx->InDelay;
			uint code = ( uint )_memory->ReadWord( address );

			if( pass == 1 )
			{
				g->annotate( "[%#08X]: %08X", address, code );
			}

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				g->mov( EAX, MNULLDELAY( CTXP( _ctx->CtxPointer ) ) );
				g->cmp( EAX, 1 );
				g->je( "nullDelaySkip" );
			}

			if( pass == 0 )
			{
				// Mark labels
				LabelMarker^ lm;
				if( _ctx->BranchLabels->TryGetValue( address, lm ) == true )
				{
					Debug::WriteLine( String::Format( "Marking label for branch target {0:X8}", address ) );
					lm->Found = true;
				}
			}
			else if( pass == 1 )
			{
				// Shouldn't be here if in a delay slot
				LabelMarker^ lm;
				if( _ctx->BranchLabels->TryGetValue( address, lm ) == true )
				{
					Debug::WriteLine( String::Format( "Marking label for branch target {0:X8}", address ) );
					_gen->label( lm->Label );
				}
			}

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
							//if( pass == 1 )
							//	EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2} shamt:{3}", rs, rt, rd, shamt ) );
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, rs, rt, rd, shamt, function );
						}
						break;
					case 1: // J type
						{
							uint imm = code & 0x03FFFFFF;
							uint rt = ( code >> 16 ) & 0x1F;

							GenerateInstructionJ instr = R4000Generator::TableJ[ rt ];
							//if( pass == 1 )
								//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "imm:{0}", imm ) );
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
							//if( pass == 1 )
								//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2}", rs, rt, rd ) );
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
							//if( pass == 1 )
								//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "func:{0}", function ) );
							result = instr( _ctx, pass, address + 4, code, rt, rd, function, ( ushort )bshfl );
						}
						break;
					default: // Common I type
						{
							byte rs = ( byte )( ( code >> 21 ) & 0x1F );
							byte rt = ( byte )( ( code >> 16 ) & 0x1F );
							ushort imm = ( ushort )( code & 0xFFFF );

							GenerateInstructionI instr = R4000Generator::TableI[ opcode ];
							//if( pass == 1 )
								//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} imm:{2}", rs, rt, ( int )( short )imm ) );
							result = instr( _ctx, pass, address + 4, code, ( byte )opcode, rs, rt, imm );
						}
						break;
					}
				}
				else
				{
					//result = CoreInstructions.Cop.HandleInstruction( _ctx, 1, address + 4, code );
				}
			}
			else
			{
				// NOP
				result = GenerationResult::Success;
			}

			if( result == GenerationResult::Invalid )
			{
				Debug::WriteLine( String::Format( "InternalBuild(0x{0:X8}): failed to generate code for [0x{1:X8}] {2:X8}", startAddress, address, code ) );
				Debugger::Break();
				break;
			}

			if( ( pass == 1 ) && ( checkNullDelay == true ) )
			{
				g->jmp( "noNullDelay" );
				g->label( "nullDelaySkip" );
				g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );
				g->label( "noNullDelay" );
			}

			if( pass == 0 )
			{
				count++;
				endAddress = address;
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
					Debug::WriteLine( "Marking jump delay tail" );
					GenerateTail( false, 0 );

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
						Debug::WriteLine( String::Format( "Aborting block early because branch target {0:X8} not found", lm->Address ) );
						Debug::Assert( lm->Address != 0 );

						char noBranch[20];
						sprintf( noBranch, "l%Xnobr", address );

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
					Debug::WriteLine( String::Format( "Jump breakout at {0:X8}", address ) );
					breakOut = true;
					lastResult = result;
				}
				else
				{
					Debug::WriteLine( String::Format( "Ignoring jump breakout at {0:X8} because last target is {1:X8}", address, _ctx->LastBranchTarget ) );
					if( pass == 1 )
					{
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
	R4000Cpu::GlobalCpu->_core0->PC = newPc;
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
			Debug::WriteLine( String::Format( "Target block 0x{0:X8} not found, emitting jumpblock", targetAddress ) );

			// Not found - we write the missing block jump code
			this->EmitJumpBlock( targetAddress );
		}
		else
		{
			Debug::WriteLine( String::Format( "Target block 0x{0:X8} found, emitting direct jump", targetAddress ) );

			// Can do a direct jump to the translated block
			g->mov( EAX, ( int )block->Pointer );
			g->jmp( EAX );
		}
	}
	else
	{
		// Return (to bounce)
		g->mov( EAX, 0 );
		g->ret();
	}
}