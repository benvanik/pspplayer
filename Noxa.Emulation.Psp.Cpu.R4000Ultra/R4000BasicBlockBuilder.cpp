// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000BlockBuilder.h"
#include "R4000BasicBlockBuilder.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

R4000BasicBlockBuilder::R4000BasicBlockBuilder( R4000Cpu^ cpu, R4000Core^ core )
	: R4000BlockBuilder( cpu, core )
{
}

R4000BasicBlockBuilder::~R4000BasicBlockBuilder()
{
}

#define MAXCODELENGTH 100

int R4000BasicBlockBuilder::InternalBuild( int startAddress, CodeBlock* block )
{
	//int count = 0;
	//int endAddress = startAddress;
	//_ctx->Reset( startAddress );

	//Debug::WriteLine( String::Format( "!- Generating block at 0x{0:X8}:", startAddress ) );

	//R4000Generator *g = _gen;

	//bool jumpDelay = false;
	//bool breakOut = false;
	//bool checkNullDelay = false;
	//int address = startAddress;
	//GenerationResult lastResult = GenerationResult::Success;
	//for( int n = 0; n < MAXCODELENGTH; n++ )
	//{
	//	GenerationResult result = GenerationResult::Invalid;

	//	count++;
	//	endAddress = address;

	//	if( checkNullDelay == true )
	//	{
	//		g->mov( EAX, MNULLDELAY( CTXP( _ctx->CtxPointer ) ) );
	//		g->cmp( EAX, 1 );
	//		g->je( "nullDelaySkip" );
	//	}

	//	bool inDelay = _ctx->InDelay;
	//	uint code = ( uint )_memory->ReadWord( address );

	//	if( code != 0 )
	//	{
	//		uint opcode = ( code >> 26 ) & 0x3F;
	//		bool isCop = ( opcode == 0x10 ) || ( opcode == 0x11 ) || ( opcode == 0x12 );

	//		if( isCop == false )
	//		{
	//			switch( opcode )
	//			{
	//			case 0: // R type
	//				{
	//					byte function = ( byte )( code & 0x3F );
	//					byte rs = ( byte )( ( code >> 21 ) & 0x1F );
	//					byte rt = ( byte )( ( code >> 16 ) & 0x1F );
	//					byte rd = ( byte )( ( code >> 11 ) & 0x1F );
	//					byte shamt = ( byte )( ( code >> 6 ) & 0x1F );

	//					GenerateInstructionR instr = R4000Generator::TableR[ function ];
	//					//if( pass == 1 )
	//					//	EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2} shamt:{3}", rs, rt, rd, shamt ) );
	//					result = instr( _ctx, 1, address + 4, code, ( byte )opcode, rs, rt, rd, shamt, function );
	//				}
	//				break;
	//			case 1: // J type
	//				{
	//					uint imm = code & 0x03FFFFFF;
	//					uint rt = ( code >> 16 ) & 0x1F;

	//					GenerateInstructionJ instr = R4000Generator::TableJ[ rt ];
	//					//if( pass == 1 )
	//						//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "imm:{0}", imm ) );
	//					result = instr( _ctx, 1, address + 4, code, ( byte )opcode, imm );
	//				}
	//				break;
	//			case 0x1C: // Allegrex special type (follows R convention)
	//				{
	//					byte function = ( byte )( code & 0x3F );
	//					byte rs = ( byte )( ( code >> 21 ) & 0x1F );
	//					byte rt = ( byte )( ( code >> 16 ) & 0x1F );
	//					byte rd = ( byte )( ( code >> 11 ) & 0x1F );

	//					GenerateInstructionR instr = R4000Generator::TableAllegrex[ function ];
	//					//if( pass == 1 )
	//						//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2}", rs, rt, rd ) );
	//					result = instr( _ctx, 1, address + 4, code, ( byte )opcode, rs, rt, rd, 0, function );
	//				}
	//				break;
	//			case 0x1F: // SPECIAL3 type
	//				{
	//					byte rt = ( byte )( ( code >> 16 ) & 0x1F );
	//					byte rd = ( byte )( ( code >> 11 ) & 0x1F );
	//					byte function = ( byte )( ( code >> 6 ) & 0x1F );
	//					uint bshfl = code & 0x3F;

	//					// Annoying....
	//					GenerateInstructionSpecial3 instr;
	//					if( ( bshfl == 0x0 ) ||
	//						( bshfl == 0x4 ) )
	//						instr = R4000Generator::TableSpecial3[ bshfl ];
	//					else
	//						instr = R4000Generator::TableSpecial3[ function ];
	//					//if( pass == 1 )
	//						//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "func:{0}", function ) );
	//					result = instr( _ctx, 1, address + 4, code, rt, rd, function, ( ushort )bshfl );
	//				}
	//				break;
	//			default: // Common I type
	//				{
	//					byte rs = ( byte )( ( code >> 21 ) & 0x1F );
	//					byte rt = ( byte )( ( code >> 16 ) & 0x1F );
	//					ushort imm = ( ushort )( code & 0xFFFF );

	//					GenerateInstructionI instr = R4000Generator::TableI[ opcode ];
	//					//if( pass == 1 )
	//						//EmitDebugInfo( _ctx, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} imm:{2}", rs, rt, ( int )( short )imm ) );
	//					result = instr( _ctx, 1, address + 4, code, ( byte )opcode, rs, rt, imm );
	//				}
	//				break;
	//			}
	//		}
	//		else
	//		{
	//			//result = CoreInstructions.Cop.HandleInstruction( _ctx, 1, address + 4, code );
	//		}
	//	}
	//	else
	//	{
	//		// NOP
	//	}

	//	if( result == GenerationResult::Invalid )
	//	{
	//		Debug::WriteLine( String::Format( "InternalBuild(0x{0:X8}): failed to generate code for [0x{1:X8}] {2:X8}", startAddress, address, code ) );
	//		Debugger::Break();
	//		break;
	//	}

	//	if( checkNullDelay == true )
	//	{
	//		g->jmp( "noNullDelay" );
	//		g->label( "nullDelaySkip" );
	//		g->mov( MNULLDELAY( CTXP( _ctx->CtxPointer ) ), 0 );
	//		g->label( "noNullDelay" );
	//	}

	//	address += 4;

	//	// Have to use local inDelay because the last instruction could have been the one to set it
	//	// Could also be in a jump delay, which only happens on non-breakout jumps
	//	if( jumpDelay == true )
	//	{
	//		// GENERATE TAIL TO MARK DELAY TAIL

	//		_ctx->InDelay = false;
	//		jumpDelay = false;
	//	}
	//	else if( inDelay == true )
	//	{
	//		// EXIT EARLY TO BRANCH TARGET

	//		_ctx->InDelay = false;
	//		_ctx->BranchTarget = nullptr;
	//	}

	//	// Syscalls always exit
	//	if( result == GenerationResult::Syscall )
	//	{
	//		lastResult = result;
	//		break;
	//	}

	//	// For delay slots
	//	if( breakOut == true )
	//		break;

	//	switch( result )
	//	{
	//	case GenerationResult::Branch:
	//		_ctx->InDelay = true;
	//		lastResult = result;
	//		break;
	//	case GenerationResult::Jump:
	//		// This is tricky - if lastTargetPc > currentPc, don't break out
	//		if( _ctx->LastBranchTarget <= address )
	//		{
	//			breakOut = true;
	//			lastResult = result;
	//		}
	//		else
	//		{
	//			jumpDelay = true;
	//			_ctx->InDelay = true;
	//		}
	//		break;
	//	case GenerationResult::BranchAndNullifyDelay:
	//		_ctx->InDelay = true;
	//		checkNullDelay = true;
	//		lastResult = result;
	//		break;
	//	}
	//}

	//return count;
	return 0;
}