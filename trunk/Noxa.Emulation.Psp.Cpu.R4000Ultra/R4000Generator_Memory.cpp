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

#include "Loader.hpp"
#include "CodeGenerator.hpp"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

#define g context->Generator

void EmitAddressTranslation( R4000Generator *gen )
{
	gen->and( gen->eax, 0x3FFFFFFF );
}

int __readMemoryThunk( int targetAddress )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
	Debug::Assert( cpu != nullptr );
	if( cpu != nullptr )
		return cpu->Memory->ReadWord( targetAddress );
	else
		return 0;
}

void __writeMemoryThunk( int targetAddress, int width, int value )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
	Debug::Assert( cpu != nullptr );
	if( cpu != nullptr )
		cpu->Memory->WriteWord( targetAddress, width, value );
	return;
}

void EmitDirectMemoryRead( R4000GenContext^ context, int address )
{
	char label1[20];
	sprintf( label1, "l%Xs1", address );
	char label2[20];
	sprintf( label2, "l%Xs2", address );

	// if < 0x0800000 && > 0x09FFFFFF, skip and do a read from method
	g->cmp( EAX, 0x08000000 );
	g->jb( label1 );
	g->cmp( EAX, 0x09FFFFFF );
	g->ja( label1 );

	// else, do a direct read
	g->sub( EAX, 0x08000000 ); // get to offset in main memory
	g->mov( EAX, g->dword_ptr[ EAX + (int)context->Memory->MainMemory ] );
	g->jmp( label2 );

	// case to handle read call
	g->label( label1 );

	g->mov( EBX, (int)&__readMemoryThunk );
	g->call( EBX );

	// done
	g->label( label2 );
}

void EmitDirectMemoryWrite( R4000GenContext^ context, int address, int width )
{
	char label1[20];
	sprintf( label1, "l%Xs1", address );
	char label2[20];
	sprintf( label2, "l%Xs2", address );

	// if < 0x0800000 && > 0x09FFFFFF, skip and do a write from method
	g->cmp( EAX, 0x08000000 );
	g->jb( label1 );
	g->cmp( EAX, 0x09FFFFFF );
	g->ja( label1 );

	// else, do a direct read
	g->sub( EAX, 0x08000000 ); // get to offset in main memory
	switch( width )
	{
	case 1:
		g->mov( g->byte_ptr[ EAX + (int)context->Memory->MainMemory ], BL );
		break;
	case 2:
		g->mov( g->word_ptr[ EAX + (int)context->Memory->MainMemory ], BX );
		break;
	case 4:
		g->mov( g->dword_ptr[ EAX + (int)context->Memory->MainMemory ], EBX );
		break;
	}
	g->jmp( label2 );

	// case to handle read call
	g->label( label1 );

	switch( width )
	{
	case 1:
		g->movzx( EBX, BL );
		break;
	case 2:
		g->movzx( EBX, BX );
		break;
	}
	g->push( EBX );
	g->push( (uint)width );
	g->push( EAX );
	g->mov( EBX, (int)&__writeMemoryThunk );
	g->call( EBX );
	g->add( ESP, 12 );

	// done
	g->label( label2 );
}

GenerationResult LB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Byte mask & sign extend
		g->movsx( EAX, AL );

		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LH( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Short mask & sign extend
		g->movsx( EAX, AX );

		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LWL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult LW( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LBU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Byte mask
		g->movzx( EAX, AL );

		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LHU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Short mask
		g->movzx( EAX, AX );

		g->mov( MREG( rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LWR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult SB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( rt ) );

		EmitDirectMemoryWrite( context, address, 1 );
	}
	return GenerationResult::Success;
}

GenerationResult SH( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( rt ) );

		EmitDirectMemoryWrite( context, address, 2 );
	}
	return GenerationResult::Success;
}

GenerationResult SWL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult SW( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( rt ) );

		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}

GenerationResult SWR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult CACHE( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	// Not implemented on purpose
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult LL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult SC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult LWCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	byte cop = ( byte )( opcode & 0x3 );
	if( ( cop == 0 ) || ( cop == 2 ) )
		return GenerationResult::Invalid;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		switch( cop )
		{
		case 0:
			//g->mov( MCP0REG( rt ), EAX );
			break;
		case 1:
			g->mov( MCP1REG( rt ), EAX );
			break;
		case 2:
			//g->mov( MCP2REG( rt ), EAX );
			break;
		}
	}
	return GenerationResult::Success;
}

GenerationResult SWCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	byte cop = ( byte )( opcode & 0x3 );
	if( ( cop == 0 ) || ( cop == 2 ) )
		return GenerationResult::Invalid;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MREG( rs ) );
		g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		
		switch( cop )
		{
		case 0:
			break;
		case 1:
			g->mov( EBX, MCP1REG( rt ) );
			break;
		case 2:
			break;
		}

		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}
